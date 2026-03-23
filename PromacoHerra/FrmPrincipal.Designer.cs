namespace PromacoHerra
{
          partial class FrmPrincipal
        {
            private System.ComponentModel.IContainer components = null;
            private System.Windows.Forms.Panel panelContenedor;
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
            panelMenu = new Panel();
            btnHerramientas = new Button();
            btnEmpleados = new Button();
            btnPrestamos = new Button();
            btnDevoluciones = new Button();
            btnReportes = new Button();
            btnSalir = new Button();
            lblTitulo = new Label();
            panel1 = new Panel();
            panelContenedor.SuspendLayout();
            panelMenu.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panelContenedor
            // 
            panelContenedor.Controls.Add(panel1);
            panelContenedor.Dock = DockStyle.Fill;
            panelContenedor.Location = new Point(200, 0);
            panelContenedor.Name = "panelContenedor";
            panelContenedor.Size = new Size(1093, 873);
            panelContenedor.TabIndex = 0;
            // 
            // panelMenu
            // 
            panelMenu.BackColor = Color.White;
            panelMenu.Controls.Add(btnHerramientas);
            panelMenu.Controls.Add(btnEmpleados);
            panelMenu.Controls.Add(btnPrestamos);
            panelMenu.Controls.Add(btnDevoluciones);
            panelMenu.Controls.Add(btnReportes);
            panelMenu.Controls.Add(btnSalir);
            panelMenu.Dock = DockStyle.Left;
            panelMenu.Location = new Point(0, 0);
            panelMenu.Name = "panelMenu";
            panelMenu.Padding = new Padding(50);
            panelMenu.Size = new Size(200, 873);
            panelMenu.TabIndex = 4;
            // 
            // btnHerramientas
            // 
            btnHerramientas.Location = new Point(12, 20);
            btnHerramientas.Name = "btnHerramientas";
            btnHerramientas.Size = new Size(182, 60);
            btnHerramientas.TabIndex = 0;
            btnHerramientas.Text = "Herramientas";
            // 
            // btnEmpleados
            // 
            btnEmpleados.Location = new Point(12, 99);
            btnEmpleados.Name = "btnEmpleados";
            btnEmpleados.Size = new Size(182, 60);
            btnEmpleados.TabIndex = 1;
            btnEmpleados.Text = "Empleados";
            // 
            // btnPrestamos
            // 
            btnPrestamos.Location = new Point(12, 181);
            btnPrestamos.Name = "btnPrestamos";
            btnPrestamos.Size = new Size(182, 60);
            btnPrestamos.TabIndex = 2;
            btnPrestamos.Text = "Préstamos";
            // 
            // btnDevoluciones
            // 
            btnDevoluciones.Location = new Point(12, 272);
            btnDevoluciones.Name = "btnDevoluciones";
            btnDevoluciones.Size = new Size(182, 60);
            btnDevoluciones.TabIndex = 3;
            btnDevoluciones.Text = "Devoluciones";
            // 
            // btnReportes
            // 
            btnReportes.Location = new Point(12, 357);
            btnReportes.Name = "btnReportes";
            btnReportes.Size = new Size(182, 60);
            btnReportes.TabIndex = 4;
            btnReportes.Text = "Reportes";
            // 
            // btnSalir
            // 
            btnSalir.Location = new Point(12, 447);
            btnSalir.Name = "btnSalir";
            btnSalir.Size = new Size(182, 60);
            btnSalir.TabIndex = 5;
            btnSalir.Text = "Salir";
            // 
            // lblTitulo
            // 
            lblTitulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitulo.Location = new Point(184, 9);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(688, 45);
            lblTitulo.TabIndex = 6;
            lblTitulo.Text = "Sistema de Control de Herramientas - PROMACO";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            panel1.Controls.Add(lblTitulo);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1093, 66);
            panel1.TabIndex = 7;
            // 
            // FrmPrincipal
            // 
            ClientSize = new Size(1293, 873);
            Controls.Add(panelContenedor);
            Controls.Add(panelMenu);
            Name = "FrmPrincipal";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Menú Principal";
            panelContenedor.ResumeLayout(false);
            panelMenu.ResumeLayout(false);
            panel1.ResumeLayout(false);
            ResumeLayout(false);


        }

        private Panel panelMenu;
        private Button btnHerramientas;
        private Button btnEmpleados;
        private Button btnPrestamos;
        private Button btnDevoluciones;
        private Button btnReportes;
        private Button btnSalir;
        private Panel panel1;
        private Label lblTitulo;
    }
    }
