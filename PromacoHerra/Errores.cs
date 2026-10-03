using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace PromacoHerra
{
    // Mensajes de error para el usuario y registro del detalle técnico.
    //   · Los errores de negocio de los procedimientos (THROW 50000) ya vienen en español: pasan tal cual.
    //   · Los fallos de conexión con SQL Server se traducen a un mensaje que dice qué revisar.
    //   · Lo que nadie atrapa (Program.cs) se muestra con un mensaje amable y se registra en errores.log.
    public static class Errores
    {
        public static readonly string ArchivoLog = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PROMACO", "errores.log");

        // Números de error de SqlClient que significan "no hay conexión con el servidor"
        private static readonly int[] SinConexion = { -2, -1, 2, 53, 121, 233, 258, 1225, 10053, 10054, 10060, 10061, 11001 };

        public static string Mensaje(Exception ex)
        {
            var sql = Buscar<SqlException>(ex);
            if (sql == null) return ex.Message;

            if (sql.Number == 50000) return sql.Message;             // regla de negocio de un procedimiento
            if (Array.IndexOf(SinConexion, sql.Number) >= 0)
                return "No se pudo conectar con la base de datos.\n\n" +
                       "Verifique que SQL Server esté en ejecución y que esta computadora tenga acceso al servidor.";
            if (sql.Number == 4060)
                return "No se encontró la base de datos PROMACO_Herramientas en el servidor.\n\n" +
                       "Revise la instalación (Database/instalacion) o la cadena de conexión en Data/Db.cs.";
            if (sql.Number == 18456)
                return "Su usuario de Windows no tiene permiso para entrar a la base de datos.\n\n" +
                       "Pida al administrador del servidor que le dé acceso.";

            return "Error de la base de datos: " + sql.Message;
        }

        // Para lo que ningún formulario atrapó: mensaje amable + detalle técnico en el log
        public static void MostrarNoControlado(Exception ex)
        {
            string? ruta = Registrar(ex);
            MessageBox.Show(
                Mensaje(ex) + (ruta != null ? $"\n\nEl detalle técnico se guardó en:\n{ruta}" : ""),
                "PROMACO · Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Devuelve la ruta del log, o null si no se pudo escribir (no debe causar otro error)
        public static string? Registrar(Exception ex)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(ArchivoLog)!);
                File.AppendAllText(ArchivoLog, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {ex}{Environment.NewLine}{Environment.NewLine}");
                return ArchivoLog;
            }
            catch { return null; }
        }

        private static T? Buscar<T>(Exception? ex) where T : Exception
        {
            for (; ex != null; ex = ex.InnerException)
                if (ex is T t) return t;
            return null;
        }
    }
}
