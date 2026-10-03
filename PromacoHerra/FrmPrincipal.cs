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
    public partial class FrmPrincipal : Form
    {
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
            sidebar.AgregarItem("salir", "Salir", IconChar.RightFromBracket, alFinal: true);
            sidebar.ItemSeleccionado += (s, clave) => AbrirSeccion(clave);
        }

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
                case "salir": Application.Exit(); break;
            }
        }

        private void FrmPrincipal_Load(object? sender, EventArgs e)
        {
            AbrirFormulario(new FrmDashboard());
        }

        private void FrmPrincipal_FormClosed(object? sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
