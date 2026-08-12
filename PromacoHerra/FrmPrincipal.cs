using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

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
            this.btnInicio.Click += new System.EventHandler(this.btnInicio_Click);
            this.btnHerramientas.Click += new System.EventHandler(this.btnHerramientas_Click);
            this.btnEmpleados.Click += new System.EventHandler(this.btnEmpleados_Click);
            this.btnPrestamos.Click += new System.EventHandler(this.btnPrestamos_Click);
            this.btnDevoluciones.Click += new System.EventHandler(this.btnDevoluciones_Click);
            this.btnReportes.Click += new System.EventHandler(this.btnReportes_Click);
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
        }



        private void AbrirFormulario(Form frm)
        {
            panelContenedor.Controls.Clear();

            frm.TopLevel = false;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;

            panelContenedor.Controls.Add(frm);
            frm.Show();
        }

        private void AbrirEnPanel(Form frm)
        {
            panelContenedor.Controls.Clear();

            frm.TopLevel = false;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;

            panelContenedor.Controls.Add(frm);
            panelContenedor.Tag = frm;

            frm.Show();
        }
        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            AbrirEnPanel(new FrmDashboard());
        }
        private void btnInicio_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmDashboard());
        }
        private void btnHerramientas_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmHerramientas());
        }

        private void btnEmpleados_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmEmpleados());
        }

        private void btnPrestamos_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmPrestamo());
        }

        private void btnDevoluciones_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmDevolucion());
        }

        private void btnReportes_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmReportes());
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void FrmPrincipal_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        }
    }



