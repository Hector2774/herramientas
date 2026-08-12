// Services/EmpleadoService.cs
using System;
using System.Collections.Generic;
using System.Net.Http;
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
                var json = await _http.GetStringAsync(API_URL);
                var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<List<EmpleadoDTO>>(json, opts)
                       ?? new List<EmpleadoDTO>();
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
        // 1. Inserta departamentos nuevos en tabla Departamento
        // 2. Hace MERGE de empleados vinculándolos a su DepartamentoId
        // 3. Marca como Activo=0 los que ya no vienen en la API
        public async Task<(int insertados, int actualizados)> SincronizarAsync()
        {
            var empleados = await ObtenerDesdeApiAsync();

            int insertados = 0;
            int actualizados = 0;

            using var cn = Db.GetConnection();
            cn.Open();

            // -- Paso 1: sincronizar departamentos --
            foreach (var emp in empleados)
            {
                var sqlDepto = @"
                    IF NOT EXISTS (SELECT 1 FROM Departamento WHERE Nombre = @Nombre)
                        INSERT INTO Departamento (Nombre)
                        VALUES (@Nombre)";

                using var cmdDepto = new SqlCommand(sqlDepto, cn);
                cmdDepto.Parameters.AddWithValue("@Nombre", emp.Departamento);
                cmdDepto.ExecuteNonQuery();
            }

            // -- Paso 2: marcar todos inactivos; el MERGE reactiva los que siguen --
            using (var cmdInac = new SqlCommand("UPDATE Empleado SET Activo = 0", cn))
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

                using var cmdMerge = new SqlCommand(sqlMerge, cn);
                cmdMerge.Parameters.AddWithValue("@Codigo", emp.Codigo);
                cmdMerge.Parameters.AddWithValue("@Nombre", emp.Nombre);
                cmdMerge.Parameters.AddWithValue("@Departamento", emp.Departamento);

                var accion = cmdMerge.ExecuteScalar()?.ToString();
                if (accion == "INSERT") insertados++;
                else actualizados++;
            }

            return (insertados, actualizados);
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
    }
}