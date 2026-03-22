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
        }
        private void btnHerramientas_Click(object sender, EventArgs e)
        {
            FrmHerramientas frm = new FrmHerramientas();
            frm.ShowDialog();
        }

        private void btnEmpleados_Click(object sender, EventArgs e)
        {
            FrmEmpleados frm = new FrmEmpleados();
            frm.ShowDialog();
        }

        private void btnPrestamos_Click(object sender, EventArgs e)
        {
            FrmPrestamo frm = new FrmPrestamo();
            frm.ShowDialog();
        }

        private void btnDevoluciones_Click(object sender, EventArgs e)
        {
            FrmDevolucion frm = new FrmDevolucion();
            frm.ShowDialog();
        }

        private void btnReportes_Click(object sender, EventArgs e)
        {
            FrmReportes frm = new FrmReportes();
            frm.ShowDialog();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {

        }
    }
}
