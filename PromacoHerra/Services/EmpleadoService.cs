// Services/EmpleadoService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using PromacoHerra.Data;
using PromacoHerra.Models;

namespace PromacoHerra.Services
{
    public class EmpleadoService
    {
        // ── Cambia esta IP por la de tu laptop Fedora ──────────────
        private const string API_URL = "http://192.168.0.15:8000/api/empleados";

        private static readonly HttpClient _http = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(10)
        };

        // ── Trae la lista desde la API ─────────────────────────────
        public async Task<List<EmpleadoDTO>> ObtenerDesdeApiAsync()
        {
            try
            {
                // Se decodifica como UTF-8 explícito, sin depender del charset del Content-Type
                var bytes = await _http.GetByteArrayAsync(API_URL);
                var json = Encoding.UTF8.GetString(bytes);

                var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var lista = JsonSerializer.Deserialize<List<EmpleadoDTO>>(json, opts) ?? new List<EmpleadoDTO>();

                // La API devuelve los textos ya dañados ("ORDOÃ‘EZ"): se reparan aquí
                foreach (var e in lista)
                {
                    e.Codigo = (e.Codigo ?? "").Trim();
                    e.Nombre = TextoReparado.Reparar(e.Nombre ?? "").Trim();
                    e.Departamento = TextoReparado.Reparar(e.Departamento ?? "").Trim();
                }
                return lista;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception(
                    "No se pudo conectar a la API de empleados.\n" +
                    "Verifica que la laptop con Fedora esté encendida y en la misma red.\n\n" +
                    $"Detalle: {ex.Message}");
            }
            catch (TaskCanceledException)
            {
                throw new Exception(
                    "La API tardó demasiado en responder.\n" +
                    "Verifica que el servicio esté corriendo en Fedora.");
            }
        }

        // ── Sincroniza API → base local ────────────────────────────
        // 0. Repara textos con mal encoding que ya estaban guardados
        // 1. Inserta departamentos nuevos en tabla Departamento
        // 2. Marca como Activo=0 (salvo los ligados a un Usuario); el MERGE reactiva los que siguen en la API
        // 3. Hace MERGE de empleados vinculándolos a su DepartamentoId
        // Todo en una transacción: si algo falla no quedan empleados desactivados a medias.
        public async Task<ResultadoSincronizacion> SincronizarAsync()
        {
            var empleados = await ObtenerDesdeApiAsync();

            // Una respuesta vacía desactivaría a todos los empleados
            if (empleados.Count == 0)
                throw new Exception("La API de RRHH no devolvió empleados. No se modificó la base local.");

            // La parte de BD corre fuera del hilo de la UI para que el formulario no se congele
            return await Task.Run(() => GuardarEnBd(empleados));
        }

        private static ResultadoSincronizacion GuardarEnBd(List<EmpleadoDTO> empleados)
        {
            int insertados = 0;
            int actualizados = 0;

            using var cn = Db.GetConnection();
            cn.Open();
            using var tx = cn.BeginTransaction();

            // -- Paso 0: reparar textos dañados ya guardados --
            RepararTextosGuardados(cn, tx);

            // -- Paso 1: sincronizar departamentos --
            foreach (var depto in empleados.Select(e => e.Departamento).Where(d => d != "").Distinct())
            {
                using var cmdDepto = new SqlCommand(@"
                    IF NOT EXISTS (SELECT 1 FROM Departamento WHERE Nombre = @Nombre)
                        INSERT INTO Departamento (Nombre) VALUES (@Nombre)", cn, tx);
                cmdDepto.Parameters.AddWithValue("@Nombre", depto);
                cmdDepto.ExecuteNonQuery();
            }

            // -- Paso 2: marcar todos inactivos; el MERGE reactiva los que siguen --
            // Excepto los empleados ligados a un usuario del sistema (p. ej. ADMIN): no vienen de
            // RRHH y, si se desactivan, ese usuario ya no puede iniciar sesión ni aprobar préstamos.
            using (var cmdInac = new SqlCommand(
                "UPDATE Empleado SET Activo = 0 WHERE EmpleadoId NOT IN (SELECT EmpleadoId FROM Usuario)", cn, tx))
                cmdInac.ExecuteNonQuery();

            // -- Paso 3: MERGE de empleados --
            foreach (var emp in empleados)
            {
                var sqlMerge = @"
                    DECLARE @DeptoId INT;
                    SELECT @DeptoId = DepartamentoId
                    FROM   Departamento
                    WHERE  Nombre = @Departamento;

                    MERGE Empleado AS target
                    USING (SELECT @Codigo AS Codigo) AS source
                       ON target.Codigo = source.Codigo

                    WHEN MATCHED THEN
                        UPDATE SET
                            Nombre         = @Nombre,
                            DepartamentoId = @DeptoId,
                            Activo         = 1

                    WHEN NOT MATCHED THEN
                        INSERT (Codigo, Nombre, DepartamentoId, Activo)
                        VALUES (@Codigo, @Nombre, @DeptoId, 1)

                    OUTPUT $action;";

                using var cmdMerge = new SqlCommand(sqlMerge, cn, tx);
                cmdMerge.Parameters.AddWithValue("@Codigo", emp.Codigo);
                cmdMerge.Parameters.AddWithValue("@Nombre", emp.Nombre);
                cmdMerge.Parameters.AddWithValue("@Departamento", emp.Departamento);

                var accion = cmdMerge.ExecuteScalar()?.ToString();
                if (accion == "INSERT") insertados++;
                else actualizados++;
            }

            // -- Paso 4: registrar la sincronización --
            int total;
            using (var cmdTotal = new SqlCommand("SELECT COUNT(*) FROM Empleado WHERE Activo = 1", cn, tx))
                total = Convert.ToInt32(cmdTotal.ExecuteScalar());

            DateTime fecha;
            using (var cmdLog = new SqlCommand(@"
                INSERT INTO SincronizacionEmpleados (Nuevos, Actualizados, TotalActivos)
                OUTPUT inserted.Fecha
                VALUES (@Nuevos, @Actualizados, @Total)", cn, tx))
            {
                cmdLog.Parameters.AddWithValue("@Nuevos", insertados);
                cmdLog.Parameters.AddWithValue("@Actualizados", actualizados);
                cmdLog.Parameters.AddWithValue("@Total", total);
                fecha = Convert.ToDateTime(cmdLog.ExecuteScalar());
            }

            tx.Commit();
            return new ResultadoSincronizacion(insertados, actualizados, total, fecha);
        }

        // ── Repara nombres de empleados y departamentos guardados con mal encoding ──
        // Un departamento reparado puede coincidir con otro ya correcto: en ese caso se
        // pasan sus empleados al correcto y se elimina el dañado (evita duplicados).
        public static void RepararTextosGuardados(SqlConnection cn, SqlTransaction tx)
        {
            var deptos = new List<(int Id, string Nombre)>();
            using (var cmd = new SqlCommand(
                "SELECT DepartamentoId, Nombre FROM Departamento WHERE Nombre LIKE '%Ã%' OR Nombre LIKE '%Â%'", cn, tx))
            using (var rd = cmd.ExecuteReader())
                while (rd.Read()) deptos.Add((rd.GetInt32(0), rd.GetString(1)));

            foreach (var (id, nombre) in deptos)
            {
                var reparado = TextoReparado.Reparar(nombre);
                if (reparado == nombre) continue;

                using var cmd = new SqlCommand(@"
                    DECLARE @Existente INT = (SELECT TOP 1 DepartamentoId FROM Departamento
                                              WHERE Nombre = @Reparado AND DepartamentoId <> @Id);
                    IF @Existente IS NULL
                        UPDATE Departamento SET Nombre = @Reparado WHERE DepartamentoId = @Id;
                    ELSE
                    BEGIN
                        UPDATE Empleado SET DepartamentoId = @Existente WHERE DepartamentoId = @Id;
                        DELETE FROM Departamento WHERE DepartamentoId = @Id;
                    END", cn, tx);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Reparado", reparado);
                cmd.ExecuteNonQuery();
            }

            var nombres = new List<(int Id, string Nombre)>();
            using (var cmd = new SqlCommand(
                "SELECT EmpleadoId, Nombre FROM Empleado WHERE Nombre LIKE N'%Ã%' OR Nombre LIKE N'%Â%'", cn, tx))
            using (var rd = cmd.ExecuteReader())
                while (rd.Read()) nombres.Add((rd.GetInt32(0), rd.GetString(1)));

            foreach (var (id, nombre) in nombres)
            {
                var reparado = TextoReparado.Reparar(nombre);
                if (reparado == nombre) continue;
                using var cmd = new SqlCommand("UPDATE Empleado SET Nombre = @Nombre WHERE EmpleadoId = @Id", cn, tx);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Nombre", reparado);
                cmd.ExecuteNonQuery();
            }
        }

        // ── Lee empleados activos desde la base local ──────────────
        // Lo usa el formulario para mostrar la tabla.
        public List<EmpleadoDTO> ObtenerLocales()
        {
            var lista = new List<EmpleadoDTO>();
            var sql = @"
                SELECT e.Codigo,
                       e.Nombre,
                       ISNULL(d.Nombre, '—') AS Departamento
                FROM   Empleado e
                LEFT   JOIN Departamento d ON e.DepartamentoId = d.DepartamentoId
                WHERE  e.Activo = 1
                ORDER  BY e.Nombre ASC";

            var dt = Db.Query(sql);
            foreach (System.Data.DataRow row in dt.Rows)
            {
                lista.Add(new EmpleadoDTO
                {
                    Codigo = row["Codigo"].ToString(),
                    Nombre = row["Nombre"].ToString(),
                    Departamento = row["Departamento"].ToString()
                });
            }
            return lista;
        }

        // ── Fecha de la última sincronización (null = nunca) ────────
        public DateTime? ObtenerUltimaSincronizacion()
        {
            var dt = Db.Query("SELECT MAX(Fecha) AS Fecha FROM SincronizacionEmpleados");
            return dt.Rows.Count > 0 && dt.Rows[0]["Fecha"] is DateTime f ? f : null;
        }
    }

    public record ResultadoSincronizacion(int Nuevos, int Actualizados, int TotalActivos, DateTime Fecha);

    // Repara textos con "mojibake": UTF-8 que alguien leyó como Windows-1252/Latin-1.
    //   "ORDOÃ‘EZ" → bytes C3 91 → UTF-8 → "ORDOÑEZ"
    // Si el texto no tiene ese patrón o no es UTF-8 válido, se devuelve tal cual.
    public static class TextoReparado
    {
        private static readonly Dictionary<char, byte> Cp1252;

        static TextoReparado()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var cp1252 = Encoding.GetEncoding(1252);
            Cp1252 = new Dictionary<char, byte>();
            for (int b = 0x80; b <= 0x9F; b++)
            {
                var c = cp1252.GetString(new[] { (byte)b })[0];
                if (c != '?' && c > 0xFF) Cp1252[c] = (byte)b;   // ‘ ’ “ ” – — € …
            }
        }

        public static string Reparar(string texto)
        {
            if (string.IsNullOrEmpty(texto) || (texto.IndexOf('Ã') < 0 && texto.IndexOf('Â') < 0))
                return texto;

            var bytes = new List<byte>(texto.Length);
            foreach (var c in texto)
            {
                if (c <= 0xFF) bytes.Add((byte)c);                    // Latin-1 (incluye U+0080–U+009F)
                else if (Cp1252.TryGetValue(c, out var b)) bytes.Add(b);
                else return texto;                                    // no es mojibake
            }

            try { return new UTF8Encoding(false, throwOnInvalidBytes: true).GetString(bytes.ToArray()); }
            catch (DecoderFallbackException) { return texto; }
        }
    }
}
