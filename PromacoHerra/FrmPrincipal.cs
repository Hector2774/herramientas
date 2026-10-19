using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FontAwesome.Sharp;
using PromacoHerra.Services;

namespace PromacoHerra
{
    // Ventana principal del sistema (se abre después del login).
    // A la izquierda está el menú lateral (sidebar) y a la derecha un panel donde se
    // "incrustan" los demás formularios (Inicio, Herramientas, Empleados, etc.), de modo
    // que todo el sistema funciona dentro de una sola ventana.
    public partial class FrmPrincipal : Form
    {
        // ══════════════════════════════════════════════════════════
        // INICIO — construcción del menú lateral
        // ══════════════════════════════════════════════════════════

        // Crea los ítems del menú. "Usuarios" solo aparece si el usuario en sesión tiene
        // permiso para administrar usuarios. Al hacer clic en un ítem se llama a AbrirSeccion.
        public FrmPrincipal()
        {
            InitializeComponent();
            this.Load += new System.EventHandler(this.FrmPrincipal_Load);
            this.FormClosed += FrmPrincipal_FormClosed;
            ThemeManager.ApplyTheme(this);

            // Menú lateral: mismo tamaño y estilo (solid) de ícono en todos los ítems
            sidebar.AgregarItem("inicio", "Inicio", IconChar.House);
            sidebar.AgregarItem("herramientas", "Herramientas", IconChar.Toolbox);
            sidebar.AgregarItem("empleados", "Empleados", IconChar.Users);
            sidebar.AgregarItem("prestamos", "Préstamos", IconChar.HandHolding);
            sidebar.AgregarItem("devoluciones", "Devoluciones", IconChar.ArrowRotateLeft);
            sidebar.AgregarItem("reportes", "Reportes", IconChar.ChartColumn);
            if (Sesion.AdministraUsuarios)
                sidebar.AgregarItem("usuarios", "Usuarios", IconChar.UserShield);
            sidebar.AgregarItem("password", "Mi contraseña", IconChar.Key, alFinal: true);
            sidebar.AgregarItem("salir", "Cerrar sesión", IconChar.RightFromBracket, alFinal: true);
            sidebar.ItemSeleccionado += (s, clave) => AbrirSeccion(clave);

            // Cierre de sesión por inactividad: se revisa cada 15 segundos
            Inactividad.Iniciar();
            _revisarInactividad.Tick += (s, e) => RevisarInactividad();
            _revisarInactividad.Start();
        }

        // ══════════════════════════════════════════════════════════
        // NAVEGACIÓN — mostrar formularios dentro del panel central
        // ══════════════════════════════════════════════════════════

        // Quita el formulario que estaba abierto y coloca el nuevo dentro del panel central,
        // sin bordes y ocupando todo el espacio (como si fuera una "página" del sistema).
        private void AbrirFormulario(Form frm)
        {
            panelContenedor.Controls.Clear();

            frm.TopLevel = false;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;

            panelContenedor.Controls.Add(frm);
            frm.Show();

            // El ítem activo del menú sigue al formulario abierto (también si lo abre otro formulario)
            sidebar.Activo = frm switch
            {
                FrmDashboard => "inicio",
                FrmHerramientas => "herramientas",
                FrmEmpleados => "empleados",
                FrmPrestamo => "prestamos",
                FrmDevolucion => "devoluciones",
                FrmReportes => "reportes",
                FrmUsuarios => "usuarios",
                _ => sidebar.Activo
            };
        }

        // Permite que un formulario incrustado (p. ej. el dashboard) abra otro en el panel
        public void Navegar(Form frm) => AbrirFormulario(frm);

        // Decide qué hacer según el ítem del menú que se pulsó: abrir una sección en el panel,
        // abrir el diálogo de cambio de contraseña o cerrar la sesión.
        private void AbrirSeccion(string clave)
        {
            switch (clave)
            {
                case "inicio": AbrirFormulario(new FrmDashboard()); break;
                case "herramientas": AbrirFormulario(new FrmHerramientas()); break;
                case "empleados": AbrirFormulario(new FrmEmpleados()); break;
                case "prestamos": AbrirFormulario(new FrmPrestamo()); break;
                case "devoluciones": AbrirFormulario(new FrmDevolucion()); break;
                case "reportes": AbrirFormulario(new FrmReportes()); break;
                case "usuarios" when Sesion.AdministraUsuarios: AbrirFormulario(new FrmUsuarios()); break;
                case "password":
                    using (var frm = new FrmCambiarPassword(obligatorio: false))
                        frm.ShowDialog(this);
                    break;
                case "salir": CerrarSesion(); break;
            }
        }

        // ══════════════════════════════════════════════════════════
        // SESIÓN Y CIERRE — cerrar sesión o salir del sistema
        // ══════════════════════════════════════════════════════════

        // Indica si la ventana se está cerrando por "Cerrar sesión" (volver al login)
        // o por la X (salir de la aplicación)
        private bool _cerrandoSesion;

        private readonly System.Windows.Forms.Timer _revisarInactividad = new() { Interval = 15_000 };
        private bool _porInactividad;

        // Si nadie tocó el teclado ni el mouse en 15 minutos, se cierra la sesión. Antes se cierran
        // los diálogos que estén abiertos (formularios y mensajes); la sesión se cierra en la
        // siguiente revisión, un segundo después, cuando ya no queda ninguno.
        private void RevisarInactividad()
        {
            if (Inactividad.TiempoInactivo < Inactividad.Limite) return;

            if (Inactividad.CerrarDialogos())
            {
                _revisarInactividad.Interval = 1_000;
                return;
            }

            _revisarInactividad.Stop();
            _porInactividad = true;
            CerrarSesion();
        }

        // Cierra la sesión del usuario actual y vuelve a la pantalla de login (sin cerrar la aplicación)
        private void CerrarSesion()
        {
            _cerrandoSesion = true;
            Sesion.Cerrar();
            Close();
        }

        // Al abrirse la ventana principal se muestra el dashboard (Inicio) por defecto
        private void FrmPrincipal_Load(object? sender, EventArgs e)
        {
            AbrirFormulario(new FrmDashboard());
        }

        // Al cerrarse la ventana: si fue por "Cerrar sesión" se vuelve a mostrar el login
        // (que estaba oculto); en cualquier otro caso se cierra toda la aplicación
        private void FrmPrincipal_FormClosed(object? sender, FormClosedEventArgs e)
        {
            _revisarInactividad.Dispose();
            var login = Application.OpenForms.OfType<FrmLogin>().FirstOrDefault();
            if (_cerrandoSesion && login != null)
            {
                login.MostrarDeNuevo();
                if (_porInactividad) login.AvisarCierrePorInactividad();
            }
            else
                Application.Exit();   // cerrar la ventana con la X sigue saliendo del sistema
        }
    }
}
