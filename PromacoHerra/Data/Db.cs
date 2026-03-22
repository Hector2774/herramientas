using System.Data;
using Microsoft.Data.SqlClient;

namespace PromacoHerra.Data
{
    public static class Db
    {
        private static readonly string _cs =
            "Server=localhost;Database=PROMACO_Herramientas;Trusted_Connection=True;TrustServerCertificate=True;";

        public static DataTable Query(string sql)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = new SqlCommand(sql, cn);
            using var da = new SqlDataAdapter(cmd);

            var dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public static int Execute(string sql, params Microsoft.Data.SqlClient.SqlParameter[] ps)
        {
            using var cn = GetConnection();
            cn.Open();

            using var cmd = new Microsoft.Data.SqlClient.SqlCommand(sql, cn);

            if (ps != null)
                cmd.Parameters.AddRange(ps);

            return cmd.ExecuteNonQuery();
        }

        public static Microsoft.Data.SqlClient.SqlConnection GetConnection()
        {
            return new Microsoft.Data.SqlClient.SqlConnection(_cs);
        }
    }
}