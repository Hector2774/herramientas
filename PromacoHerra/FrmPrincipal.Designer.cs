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
            panel1 = new Panel();
            lblTitulo = new Label();
            panelMenu = new Panel();
            btnInicio = new PromacoHerra.Controls.MaterialButton();
            btnHerramientas = new PromacoHerra.Controls.MaterialButton();
            btnEmpleados = new PromacoHerra.Controls.MaterialButton();
            btnPrestamos = new PromacoHerra.Controls.MaterialButton();
            btnDevoluciones = new PromacoHerra.Controls.MaterialButton();
            btnReportes = new PromacoHerra.Controls.MaterialButton();
            btnSalir = new PromacoHerra.Controls.MaterialButton();
            panelContenedor.SuspendLayout();
            panel1.SuspendLayout();
            panelMenu.SuspendLayout();
            SuspendLayout();
            // 
            // panelContenedor
            // 
            panelContenedor.Controls.Add(panel1);
            panelContenedor.Dock = DockStyle.Fill;
            panelContenedor.Location = new Point(209, 0);
            panelContenedor.Name = "panelContenedor";
            panelContenedor.Size = new Size(1406, 933);
            panelContenedor.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(lblTitulo);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1406, 66);
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
            // panelMenu
            // 
            panelMenu.BackColor = Color.White;
            panelMenu.Controls.Add(btnInicio);
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
            panelMenu.Size = new Size(209, 933);
            panelMenu.TabIndex = 4;
            // 
            // btnInicio
            // 
            btnInicio.Icon = FontAwesome.Sharp.IconChar.Home;
            btnInicio.IconSize = 38;
            btnInicio.Location = new Point(12, 12);
            btnInicio.Name = "btnInicio";
            btnInicio.Size = new Size(182, 60);
            btnInicio.TabIndex = 6;
            btnInicio.Text = "Inicio";
            //
            // btnHerramientas
            //
            btnHerramientas.Icon = FontAwesome.Sharp.IconChar.Toolbox;
            btnHerramientas.IconSize = 38;
            btnHerramientas.Location = new Point(12, 99);
            btnHerramientas.Name = "btnHerramientas";
            btnHerramientas.Size = new Size(182, 60);
            btnHerramientas.TabIndex = 0;
            btnHerramientas.Text = "Herramientas";
            //
            // btnEmpleados
            //
            btnEmpleados.Icon = FontAwesome.Sharp.IconChar.Users;
            btnEmpleados.IconSize = 38;
            btnEmpleados.Location = new Point(12, 178);
            btnEmpleados.Name = "btnEmpleados";
            btnEmpleados.Size = new Size(182, 60);
            btnEmpleados.TabIndex = 1;
            btnEmpleados.Text = "Empleados";
            //
            // btnPrestamos
            //
            btnPrestamos.Icon = FontAwesome.Sharp.IconChar.HandHolding;
            btnPrestamos.IconSize = 38;
            btnPrestamos.Location = new Point(12, 260);
            btnPrestamos.Name = "btnPrestamos";
            btnPrestamos.Size = new Size(182, 60);
            btnPrestamos.TabIndex = 2;
            btnPrestamos.Text = "Préstamos";
            //
            // btnDevoluciones
            //
            btnDevoluciones.Icon = FontAwesome.Sharp.IconChar.Undo;
            btnDevoluciones.IconSize = 38;
            btnDevoluciones.Location = new Point(12, 351);
            btnDevoluciones.Name = "btnDevoluciones";
            btnDevoluciones.Size = new Size(182, 60);
            btnDevoluciones.TabIndex = 3;
            btnDevoluciones.Text = "Devoluciones";
            //
            // btnReportes
            //
            btnReportes.Icon = FontAwesome.Sharp.IconChar.ChartBar;
            btnReportes.IconSize = 38;
            btnReportes.Location = new Point(12, 436);
            btnReportes.Name = "btnReportes";
            btnReportes.Size = new Size(182, 60);
            btnReportes.TabIndex = 4;
            btnReportes.Text = "Reportes";
            //
            // btnSalir
            //
            btnSalir.Icon = FontAwesome.Sharp.IconChar.SignOutAlt;
            btnSalir.IconSize = 38;
            btnSalir.Location = new Point(12, 526);
            btnSalir.Name = "btnSalir";
            btnSalir.Size = new Size(182, 60);
            btnSalir.TabIndex = 5;
            btnSalir.Text = "Salir";
            // 
            // FrmPrincipal
            // 
            ClientSize = new Size(1615, 933);
            Controls.Add(panelContenedor);
            Controls.Add(panelMenu);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FrmPrincipal";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Menú Principal";
            panelContenedor.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panelMenu.ResumeLayout(false);
            ResumeLayout(false);


        }

        private Panel panelMenu;
        private PromacoHerra.Controls.MaterialButton btnHerramientas;
        private PromacoHerra.Controls.MaterialButton btnEmpleados;
        private PromacoHerra.Controls.MaterialButton btnPrestamos;
        private PromacoHerra.Controls.MaterialButton btnDevoluciones;
        private PromacoHerra.Controls.MaterialButton btnReportes;
        private PromacoHerra.Controls.MaterialButton btnSalir;
        private Panel panel1;
        private Label lblTitulo;
        private PromacoHerra.Controls.MaterialButton btnInicio;
    }
    }
