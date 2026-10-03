// Services/UsuarioService.cs
// Cubre: Login, la sesión del usuario (ligado a un Empleado) y la administración de usuarios

using System.Data;
using PromacoHerra.Data;

namespace PromacoHerra.Services
{
    // ══════════════════════════════════════════════════════════════
    // SESIÓN ACTUAL
    // ══════════════════════════════════════════════════════════════
    public static class Sesion
    {
        public static int UsuarioId { get; private set; }
        public static string Username { get; private set; } = "";
        public static int EmpleadoId { get; private set; }
        public static string NombreEmpleado { get; private set; } = "";
        public static string Rol { get; private set; } = "";

        // Permiso del rol (Rol.AdministraUsuarios), no su nombre
        public static bool AdministraUsuarios { get; private set; }

        // Contraseña temporal (usuario nuevo o restablecido): hay que cambiarla antes de entrar
        public static bool DebeCambiarPassword { get; internal set; }

        public static bool Activa => UsuarioId > 0;

        internal static void Iniciar(DataRow r)
        {
            UsuarioId = Convert.ToInt32(r["UsuarioId"]);
            DebeCambiarPassword = Convert.ToBoolean(r["DebeCambiarPassword"]);
            Refrescar(r);
        }

        // Datos que un administrador puede editar (también los suyos propios)
        internal static void Refrescar(DataRow r)
        {
            Username = r["Username"].ToString() ?? "";
            EmpleadoId = Convert.ToInt32(r["EmpleadoId"]);
            NombreEmpleado = r["NombreEmpleado"].ToString() ?? "";
            Rol = r["Rol"].ToString() ?? "";
            AdministraUsuarios = Convert.ToBoolean(r["AdministraUsuarios"]);
        }

        public static void Cerrar()
        {
            UsuarioId = EmpleadoId = 0;
            Username = NombreEmpleado = Rol = "";
            AdministraUsuarios = DebeCambiarPassword = false;
        }
    }

    // ══════════════════════════════════════════════════════════════
    // USUARIOS
    // ══════════════════════════════════════════════════════════════
    public static class UsuarioService
    {
        // Si el usuario no existe se verifica contra este hash igualmente, para que la respuesta
        // tarde lo mismo y no delate qué nombres de usuario existen.
        private static readonly Lazy<string> HashFicticio = new(() => PasswordHasher.Hash(Guid.NewGuid().ToString()));

        /// <summary>
        /// Valida las credenciales y, si son correctas, inicia la Sesion.
        /// Si el usuario aún tenía la contraseña en texto plano, la migra a hash.
        /// Revisar Sesion.DebeCambiarPassword después de un login correcto.
        /// </summary>
        public static bool Login(string username, string password)
        {
            var r = Autenticar(username, password);
            if (r == null) return false;

            Sesion.Iniciar(r);
            return true;
        }

        // Fila del usuario si las credenciales son correctas; null si no
        private static DataRow? Autenticar(string username, string password)
        {
            var dt = Db.QuerySP("sp_Usuario_ObtenerParaLogin",
                Db.Param("@Username", username));

            if (dt.Rows.Count == 0)
            {
                PasswordHasher.Verificar(password, HashFicticio.Value);
                return null;
            }

            var r = dt.Rows[0];
            int usuarioId = Convert.ToInt32(r["UsuarioId"]);

            if (r["PasswordHash"] is string hash)
            {
                if (!PasswordHasher.Verificar(password, hash)) return null;

                if (PasswordHasher.NecesitaRehash(hash))
                    GuardarPassword(usuarioId, password, Convert.ToBoolean(r["DebeCambiarPassword"]));
            }
            else
            {
                // Contraseña previa a la migración 015: se compara y se reemplaza por su hash.
                // Si no cumple las reglas actuales (p. ej. es muy corta) se pide cambiarla ya.
                if (r["PasswordLegacy"] is not string legacy || !PasswordHasher.IgualesTextoPlano(password, legacy))
                    return null;

                bool debil = PasswordHasher.ValidarNueva(password, username) != null;
                GuardarPassword(usuarioId, password, debil);
                r["DebeCambiarPassword"] = debil;
            }

            return r;
        }

        /// <summary>"Cambiar mi contraseña". Verifica la actual salvo que sea un cambio obligatorio.</summary>
        public static void CambiarMiPassword(string? actual, string nueva)
        {
            if (!Sesion.Activa) throw new InvalidOperationException("No hay una sesión activa.");

            if (!Sesion.DebeCambiarPassword)
            {
                if (actual == null || Autenticar(Sesion.Username, actual) == null)
                    throw new InvalidOperationException("La contraseña actual no es correcta.");
            }

            string? error = PasswordHasher.ValidarNueva(nueva, Sesion.Username);
            if (error != null) throw new InvalidOperationException(error);

            GuardarPassword(Sesion.UsuarioId, nueva, debeCambiar: false);
            Sesion.DebeCambiarPassword = false;
        }

        private static void GuardarPassword(int usuarioId, string password, bool debeCambiar) =>
            Db.ExecuteSP("sp_Usuario_GuardarPassword",
                Db.Param("@UsuarioId", usuarioId),
                Db.Param("@PasswordHash", PasswordHasher.Hash(password)),
                Db.Param("@DebeCambiarPassword", debeCambiar));

        // ── Administración (solo administradores; el SP también lo valida) ──

        public static DataTable ObtenerTodos() =>
            Db.QuerySP("sp_Usuario_ObtenerTodos",
                Db.Param("@AdminId", Sesion.UsuarioId));

        /// <summary>Crea el usuario con una contraseña temporal que deberá cambiar al entrar.</summary>
        public static int Crear(string username, int empleadoId, int rolId, string passwordTemporal)
        {
            var result = Db.ExecuteSPScalar("sp_Usuario_Insertar",
                Db.Param("@AdminId", Sesion.UsuarioId),
                Db.Param("@Username", username),
                Db.Param("@PasswordHash", PasswordHasher.Hash(passwordTemporal)),
                Db.Param("@EmpleadoId", empleadoId),
                Db.Param("@RolId", rolId));
            return Convert.ToInt32(result);
        }

        /// <summary>
        /// Edita usuario, empleado, rol y estado (la contraseña no se edita aquí).
        /// Si el administrador se editó a sí mismo, la sesión toma los datos nuevos.
        /// </summary>
        public static void Actualizar(int usuarioId, string username, int empleadoId, int rolId, bool activo)
        {
            var dt = Db.QuerySP("sp_Usuario_Actualizar",
                Db.Param("@AdminId", Sesion.UsuarioId),
                Db.Param("@UsuarioId", usuarioId),
                Db.Param("@Username", username),
                Db.Param("@EmpleadoId", empleadoId),
                Db.Param("@RolId", rolId),
                Db.Param("@Activo", activo));

            if (usuarioId == Sesion.UsuarioId && dt.Rows.Count > 0)
                Sesion.Refrescar(dt.Rows[0]);
        }

        public static void RestablecerPassword(int usuarioId, string passwordTemporal) =>
            Db.ExecuteSP("sp_Usuario_RestablecerPassword",
                Db.Param("@AdminId", Sesion.UsuarioId),
                Db.Param("@UsuarioId", usuarioId),
                Db.Param("@PasswordHash", PasswordHasher.Hash(passwordTemporal)));
    }

    // ══════════════════════════════════════════════════════════════
    // ROLES (catálogo: tabla Rol)
    // ══════════════════════════════════════════════════════════════
    public static class RolService
    {
        /// <summary>RolId, Nombre, Descripcion, AdministraUsuarios.</summary>
        public static DataTable ObtenerTodos() => Db.QuerySP("sp_Rol_ObtenerTodos");
    }
}
