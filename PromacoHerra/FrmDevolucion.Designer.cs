namespace PromacoHerra
{
  
        partial class FrmDevolucion
        {
            private System.ComponentModel.IContainer components = null;

            private System.Windows.Forms.Label lblTitulo;

            private System.Windows.Forms.GroupBox grpPrestamos;
            private System.Windows.Forms.Label lblBuscar;
            private System.Windows.Forms.TextBox txtBuscar;
            private System.Windows.Forms.DataGridView dgvPrestamos;

            private System.Windows.Forms.GroupBox grpDetalle;
            private System.Windows.Forms.DataGridView dgvDetalle;

            private System.Windows.Forms.Button btnDevolver;
            private System.Windows.Forms.Button btnDevolverTodo;
            private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.Label lblObservacion;
        private System.Windows.Forms.TextBox txtObservacion;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.ComboBox cboEstado;


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
            grpPrestamos = new GroupBox();
            lblBuscar = new Label();
            txtBuscar = new TextBox();
            dgvPrestamos = new DataGridView();
            grpDetalle = new GroupBox();
            dgvDetalle = new DataGridView();
            btnDevolver = new Button();
            btnDevolverTodo = new Button();
            btnCerrar = new Button();
            lblObservacion = new Label();
            txtObservacion = new TextBox();
            lblEstado = new Label();
            cboEstado = new ComboBox();
            grpPrestamos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPrestamos).BeginInit();
            grpDetalle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDetalle).BeginInit();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(829, 60);
            lblTitulo.TabIndex = 5;
            lblTitulo.Text = "Devolución de Herramientas";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // grpPrestamos
            // 
            grpPrestamos.Controls.Add(lblBuscar);
            grpPrestamos.Controls.Add(txtBuscar);
            grpPrestamos.Controls.Add(dgvPrestamos);
            grpPrestamos.Location = new Point(20, 70);
            grpPrestamos.Name = "grpPrestamos";
            grpPrestamos.Size = new Size(760, 200);
            grpPrestamos.TabIndex = 4;
            grpPrestamos.TabStop = false;
            grpPrestamos.Text = "Préstamos Activos";
            // 
            // lblBuscar
            // 
            lblBuscar.Location = new Point(20, 30);
            lblBuscar.Name = "lblBuscar";
            lblBuscar.Size = new Size(54, 23);
            lblBuscar.TabIndex = 0;
            lblBuscar.Text = "Buscar:";
            // 
            // txtBuscar
            // 
            txtBuscar.Location = new Point(80, 25);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Size = new Size(250, 23);
            txtBuscar.TabIndex = 1;
            // 
            // dgvPrestamos
            // 
            dgvPrestamos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPrestamos.Location = new Point(20, 60);
            dgvPrestamos.MultiSelect = false;
            dgvPrestamos.Name = "dgvPrestamos";
            dgvPrestamos.ReadOnly = true;
            dgvPrestamos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPrestamos.Size = new Size(720, 120);
            dgvPrestamos.TabIndex = 2;
            
            // 
            // grpDetalle
            // 
            grpDetalle.Controls.Add(dgvDetalle);
            grpDetalle.Location = new Point(20, 280);
            grpDetalle.Name = "grpDetalle";
            grpDetalle.Size = new Size(760, 200);
            grpDetalle.TabIndex = 3;
            grpDetalle.TabStop = false;
            grpDetalle.Text = "Detalle del Préstamo";
            // 
            // dgvDetalle
            // 
            dgvDetalle.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDetalle.Location = new Point(20, 30);
            dgvDetalle.MultiSelect = false;
            dgvDetalle.Name = "dgvDetalle";
            dgvDetalle.ReadOnly = true;
            dgvDetalle.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDetalle.Size = new Size(720, 150);
            dgvDetalle.TabIndex = 0;
            // 
            // btnDevolver
            // 
            btnDevolver.Location = new Point(24, 514);
            btnDevolver.Name = "btnDevolver";
            btnDevolver.Size = new Size(200, 40);
            btnDevolver.TabIndex = 2;
            btnDevolver.Text = "Devolver Seleccionado";
            // 
            // btnDevolverTodo
            // 
            btnDevolverTodo.Location = new Point(244, 514);
            btnDevolverTodo.Name = "btnDevolverTodo";
            btnDevolverTodo.Size = new Size(200, 40);
            btnDevolverTodo.TabIndex = 1;
            btnDevolverTodo.Text = "Devolver Todo";
            // 
            // btnCerrar
            // 
            btnCerrar.Location = new Point(644, 514);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(140, 40);
            btnCerrar.TabIndex = 0;
            btnCerrar.Text = "Cerrar";
            // 
            // lblObservacion
            // 
            lblObservacion.Location = new Point(20, 490);
            lblObservacion.Name = "lblObservacion";
            lblObservacion.Size = new Size(94, 23);
            lblObservacion.TabIndex = 0;
            lblObservacion.Text = "Observación:";
            // 
            // txtObservacion
            // 
            txtObservacion.Location = new Point(120, 485);
            txtObservacion.Name = "txtObservacion";
            txtObservacion.Size = new Size(300, 23);
            txtObservacion.TabIndex = 1;
            // 
            // lblEstado
            // 
            lblEstado.Location = new Point(450, 490);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(64, 23);
            lblEstado.TabIndex = 2;
            lblEstado.Text = "Estado:";
            // 
            // cboEstado
            // 
            cboEstado.DropDownStyle = ComboBoxStyle.DropDownList;
            cboEstado.Location = new Point(520, 485);
            cboEstado.Name = "cboEstado";
            cboEstado.Size = new Size(150, 23);
            cboEstado.TabIndex = 3;
            // 
            // FrmDevolucion
            // 
            ClientSize = new Size(829, 577);
            Controls.Add(lblObservacion);
            Controls.Add(txtObservacion);
            Controls.Add(lblEstado);
            Controls.Add(cboEstado);
            Controls.Add(btnCerrar);
            Controls.Add(btnDevolverTodo);
            Controls.Add(btnDevolver);
            Controls.Add(grpDetalle);
            Controls.Add(grpPrestamos);
            Controls.Add(lblTitulo);
            Name = "FrmDevolucion";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Devoluciones";
            grpPrestamos.ResumeLayout(false);
            grpPrestamos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPrestamos).EndInit();
            grpDetalle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvDetalle).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
    }
