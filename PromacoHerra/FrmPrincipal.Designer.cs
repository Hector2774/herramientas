namespace PromacoHerra
{
          partial class FrmPrincipal
        {
            private System.ComponentModel.IContainer components = null;

            private System.Windows.Forms.Label lblTitulo;
            private System.Windows.Forms.Panel panelMenu;
            private System.Windows.Forms.Button btnHerramientas;
            private System.Windows.Forms.Button btnEmpleados;
            private System.Windows.Forms.Button btnPrestamos;
            private System.Windows.Forms.Button btnDevoluciones;
            private System.Windows.Forms.Button btnReportes;
            private System.Windows.Forms.Button btnSalir;

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
            lblTitulo = new Label();
            panelMenu = new Panel();
            btnHerramientas = new Button();
            btnEmpleados = new Button();
            btnPrestamos = new Button();
            btnDevoluciones = new Button();
            btnReportes = new Button();
            btnSalir = new Button();
            panelMenu.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(1091, 70);
            lblTitulo.TabIndex = 1;
            lblTitulo.Text = "Sistema de Control de Herramientas - PROMACO";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panelMenu
            // 
            panelMenu.Controls.Add(btnHerramientas);
            panelMenu.Controls.Add(btnEmpleados);
            panelMenu.Controls.Add(btnPrestamos);
            panelMenu.Controls.Add(btnDevoluciones);
            panelMenu.Controls.Add(btnReportes);
            panelMenu.Controls.Add(btnSalir);
            panelMenu.Dock = DockStyle.Fill;
            panelMenu.Location = new Point(0, 70);
            panelMenu.Name = "panelMenu";
            panelMenu.Padding = new Padding(50);
            panelMenu.Size = new Size(1091, 553);
            panelMenu.TabIndex = 0;
            // 
            // btnHerramientas
            // 
            btnHerramientas.Location = new Point(50, 20);
            btnHerramientas.Name = "btnHerramientas";
            btnHerramientas.Size = new Size(200, 60);
            btnHerramientas.TabIndex = 0;
            btnHerramientas.Text = "Herramientas";
            btnHerramientas.Click += btnHerramientas_Click;
            // 
            // btnEmpleados
            // 
            btnEmpleados.Location = new Point(50, 100);
            btnEmpleados.Name = "btnEmpleados";
            btnEmpleados.Size = new Size(200, 60);
            btnEmpleados.TabIndex = 1;
            btnEmpleados.Text = "Empleados";
            btnEmpleados.Click += btnEmpleados_Click;
            // 
            // btnPrestamos
            // 
            btnPrestamos.Location = new Point(50, 180);
            btnPrestamos.Name = "btnPrestamos";
            btnPrestamos.Size = new Size(200, 60);
            btnPrestamos.TabIndex = 2;
            btnPrestamos.Text = "Préstamos";
            btnPrestamos.Click += btnPrestamos_Click;
            // 
            // btnDevoluciones
            // 
            btnDevoluciones.Location = new Point(50, 260);
            btnDevoluciones.Name = "btnDevoluciones";
            btnDevoluciones.Size = new Size(200, 60);
            btnDevoluciones.TabIndex = 3;
            btnDevoluciones.Text = "Devoluciones";
            btnDevoluciones.Click += btnDevoluciones_Click;
            // 
            // btnReportes
            // 
            btnReportes.Location = new Point(50, 340);
            btnReportes.Name = "btnReportes";
            btnReportes.Size = new Size(200, 60);
            btnReportes.TabIndex = 4;
            btnReportes.Text = "Reportes";
            btnReportes.Click += btnReportes_Click;
            // 
            // btnSalir
            // 
            btnSalir.Location = new Point(50, 420);
            btnSalir.Name = "btnSalir";
            btnSalir.Size = new Size(200, 60);
            btnSalir.TabIndex = 5;
            btnSalir.Text = "Salir";
            btnSalir.Click += btnSalir_Click;
            // 
            // FrmPrincipal
            // 
            ClientSize = new Size(1091, 623);
            Controls.Add(panelMenu);
            Controls.Add(lblTitulo);
            Name = "FrmPrincipal";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Menú Principal";
            Load += FrmPrincipal_Load;
            panelMenu.ResumeLayout(false);
            ResumeLayout(false);


        }
    }
    }
