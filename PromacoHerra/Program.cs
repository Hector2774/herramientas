namespace PromacoHerra
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            // Errores que ningún formulario atrapó: mensaje claro en lugar del cuadro técnico de .NET,
            // detalle en errores.log y la aplicación sigue abierta
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (_, e) => Errores.MostrarNoControlado(e.Exception);
            AppDomain.CurrentDomain.UnhandledException += (_, e) =>
            {
                if (e.ExceptionObject is Exception ex) Errores.MostrarNoControlado(ex);
            };

            ApplicationConfiguration.Initialize();
            Directory.CreateDirectory(ImagenHelper.FotosDir);   // fotos de herramientas, junto al .exe
            Application.Run(new FrmLogin());
        }
    }
}