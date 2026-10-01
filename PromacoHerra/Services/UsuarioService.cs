// Services/UsuarioService.cs
// Cubre: Login y la sesión del usuario (ligado a un Empleado)

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

        public static bool Activa => UsuarioId > 0;

        internal static void Iniciar(DataRow r)
        {
            UsuarioId = Convert.ToInt32(r["UsuarioId"]);
            Username = r["Username"].ToString() ?? "";
            EmpleadoId = Convert.ToInt32(r["EmpleadoId"]);
            NombreEmpleado = r["NombreEmpleado"].ToString() ?? "";
        }
    }

    // ══════════════════════════════════════════════════════════════
    // USUARIOS
    // ══════════════════════════════════════════════════════════════
    public static class UsuarioService
    {
        /// <summary>
        /// Valida las credenciales y, si son correctas, inicia la Sesion.
        /// </summary>
        public static bool Login(string username, string password)
        {
            var dt = Db.QuerySP("sp_Usuario_Login",
                Db.Param("@Username", username),
                Db.Param("@Password", password));

            if (dt.Rows.Count == 0) return false;

            Sesion.Iniciar(dt.Rows[0]);
            return true;
        }
    }
}
