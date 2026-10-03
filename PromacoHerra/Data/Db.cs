using System.Data;
using Microsoft.Data.SqlClient;

namespace PromacoHerra.Data
{
    public static class Db
    {
        private static readonly string _cs =
            "Server=localhost;Database=PROMACO_Herramientas;Trusted_Connection=True;TrustServerCertificate=True;Connect Timeout=8;";

        // ── Métodos originales (sin cambios) ──────────────────────

        public static DataTable Query(string sql)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = new SqlCommand(sql, cn);
            using var da = new SqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public static int Execute(string sql, params SqlParameter[] ps)
        {
            using var cn = GetConnection();
            cn.Open();
            using var cmd = new SqlCommand(sql, cn);
            if (ps != null) cmd.Parameters.AddRange(ps);
            return cmd.ExecuteNonQuery();
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(_cs);
        }

        // ── Métodos nuevos para Stored Procedures ─────────────────

        /// <summary>
        /// Ejecuta un SP y devuelve un DataTable con los resultados.
        /// Úsalo cuando el SP hace SELECT.
        /// </summary>
        public static DataTable QuerySP(string sp, params SqlParameter[] ps)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = new SqlCommand(sp, cn)
            {
                CommandType = CommandType.StoredProcedure
            };
            if (ps != null) cmd.Parameters.AddRange(ps);
            using var da = new SqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        /// <summary>
        /// Ejecuta un SP que devuelve múltiples resultsets (ej: encabezado + detalle).
        /// Devuelve un DataSet con un DataTable por cada SELECT del SP.
        /// </summary>
        public static DataSet QuerySPMultiple(string sp, params SqlParameter[] ps)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = new SqlCommand(sp, cn)
            {
                CommandType = CommandType.StoredProcedure
            };
            if (ps != null) cmd.Parameters.AddRange(ps);
            using var da = new SqlDataAdapter(cmd);
            var ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        /// <summary>
        /// Ejecuta un SP que no devuelve filas (INSERT, UPDATE, DELETE).
        /// Devuelve las filas afectadas.
        /// </summary>
        public static int ExecuteSP(string sp, params SqlParameter[] ps)
        {
            using var cn = new SqlConnection(_cs);
            cn.Open();
            using var cmd = new SqlCommand(sp, cn)
            {
                CommandType = CommandType.StoredProcedure
            };
            if (ps != null) cmd.Parameters.AddRange(ps);
            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Ejecuta un SP y devuelve el primer valor del primer resultado.
        /// Úsalo cuando el SP devuelve un solo valor (ej: SCOPE_IDENTITY, COUNT).
        /// </summary>
        public static object ExecuteSPScalar(string sp, params SqlParameter[] ps)
        {
            using var cn = new SqlConnection(_cs);
            cn.Open();
            using var cmd = new SqlCommand(sp, cn)
            {
                CommandType = CommandType.StoredProcedure
            };
            if (ps != null) cmd.Parameters.AddRange(ps);
            return cmd.ExecuteScalar();
        }

        /// <summary>
        /// Crea un SqlParameter de forma rápida.
        /// Uso: Db.Param("@Nombre", "Martillo")
        /// </summary>
        public static SqlParameter Param(string name, object value)
        {
            return new SqlParameter(name, value ?? DBNull.Value);
        }

        /// <summary>
        /// Crea un SqlParameter con tipo explícito.
        /// Úsalo para XML, fechas, o cuando el tipo importa.
        /// Uso: Db.Param("@Fecha", SqlDbType.DateTime, DateTime.Now)
        /// </summary>
        public static SqlParameter Param(string name, SqlDbType type, object value)
        {
            return new SqlParameter(name, type) { Value = value ?? DBNull.Value };
        }
    }
}