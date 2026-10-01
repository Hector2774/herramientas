namespace PromacoHerra
{
    partial class FrmPrincipal
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelContenedor;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblTitulo;
        private PromacoHerra.Controls.SidebarControl sidebar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            panelContenedor = new Panel();
            panel1 = new Panel();
            lblTitulo = new Label();
            sidebar = new PromacoHerra.Controls.SidebarControl();
            panelContenedor.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            //
            // panelContenedor
            //
            panelContenedor.Controls.Add(panel1);
            panelContenedor.Dock = DockStyle.Fill;
            panelContenedor.Location = new Point(220, 0);
            panelContenedor.Name = "panelContenedor";
            panelContenedor.Size = new Size(1395, 995);
            panelContenedor.TabIndex = 0;
            //
            // panel1
            //
            panel1.Controls.Add(lblTitulo);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1395, 66);
            panel1.TabIndex = 7;
            //
            // lblTitulo
            //
            lblTitulo.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitulo.Location = new Point(184, 9);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(688, 45);
            lblTitulo.TabIndex = 6;
            lblTitulo.Text = "Sistema de Control de Herramientas - PROMACO";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            //
            // sidebar
            //
            sidebar.Dock = DockStyle.Left;
            sidebar.Location = new Point(0, 0);
            sidebar.Name = "sidebar";
            sidebar.Size = new Size(220, 995);
            sidebar.TabIndex = 4;
            //
            // FrmPrincipal
            //
            ClientSize = new Size(1615, 995);
            Controls.Add(panelContenedor);
            Controls.Add(sidebar);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FrmPrincipal";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Menú Principal";
            panelContenedor.ResumeLayout(false);
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}
