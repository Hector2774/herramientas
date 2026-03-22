namespace PromacoHerra
{
         partial class FrmPrestamo
        {
            private System.ComponentModel.IContainer components = null;

            private System.Windows.Forms.Label lblTitulo;

            private System.Windows.Forms.GroupBox grpPrestamo;
            private System.Windows.Forms.Label lblEmpleado;
            private System.Windows.Forms.Label lblFecha;
            private System.Windows.Forms.Label lblObservaciones;

            private System.Windows.Forms.ComboBox cboEmpleado;
            private System.Windows.Forms.DateTimePicker dtpFecha;
            private System.Windows.Forms.TextBox txtObservaciones;

            private System.Windows.Forms.GroupBox grpHerramientas;
            private System.Windows.Forms.DataGridView dgvDisponibles;
            private System.Windows.Forms.DataGridView dgvSeleccionadas;

            private System.Windows.Forms.Button btnAgregar;
            private System.Windows.Forms.Button btnQuitar;

            private System.Windows.Forms.Button btnGuardar;
            private System.Windows.Forms.Button btnCancelar;

        private System.Windows.Forms.Label lblFechaDev;
        private System.Windows.Forms.DateTimePicker dtpFechaDevolucion;

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
            grpPrestamo = new GroupBox();
            lblFechaDev = new Label();
            dtpFechaDevolucion = new DateTimePicker();
            lblEmpleado = new Label();
            cboEmpleado = new ComboBox();
            lblFecha = new Label();
            dtpFecha = new DateTimePicker();
            lblObservaciones = new Label();
            txtObservaciones = new TextBox();
            grpHerramientas = new GroupBox();
            dgvDisponibles = new DataGridView();
            dgvSeleccionadas = new DataGridView();
            btnAgregar = new Button();
            btnQuitar = new Button();
            btnGuardar = new Button();
            btnCancelar = new Button();
            grpPrestamo.SuspendLayout();
            grpHerramientas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDisponibles).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvSeleccionadas).BeginInit();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(800, 60);
            lblTitulo.TabIndex = 4;
            lblTitulo.Text = "Registro de Préstamos";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // grpPrestamo
            // 
            grpPrestamo.Controls.Add(lblFechaDev);
            grpPrestamo.Controls.Add(dtpFechaDevolucion);
            grpPrestamo.Controls.Add(lblEmpleado);
            grpPrestamo.Controls.Add(cboEmpleado);
            grpPrestamo.Controls.Add(lblFecha);
            grpPrestamo.Controls.Add(dtpFecha);
            grpPrestamo.Controls.Add(lblObservaciones);
            grpPrestamo.Controls.Add(txtObservaciones);
            grpPrestamo.Location = new Point(20, 70);
            grpPrestamo.Name = "grpPrestamo";
            grpPrestamo.Size = new Size(760, 130);
            grpPrestamo.TabIndex = 3;
            grpPrestamo.TabStop = false;
            grpPrestamo.Text = "Datos del Préstamo";
            // 
            // lblFechaDev
            // 
            lblFechaDev.Location = new Point(380, 70);
            lblFechaDev.Name = "lblFechaDev";
            lblFechaDev.Size = new Size(114, 23);
            lblFechaDev.TabIndex = 0;
            lblFechaDev.Text = "Fecha devolución:";
            // 
            // dtpFechaDevolucion
            // 
            dtpFechaDevolucion.Format = DateTimePickerFormat.Short;
            dtpFechaDevolucion.Location = new Point(500, 65);
            dtpFechaDevolucion.Name = "dtpFechaDevolucion";
            dtpFechaDevolucion.Size = new Size(200, 23);
            dtpFechaDevolucion.TabIndex = 1;
            // 
            // lblEmpleado
            // 
            lblEmpleado.Location = new Point(20, 30);
            lblEmpleado.Name = "lblEmpleado";
            lblEmpleado.Size = new Size(74, 23);
            lblEmpleado.TabIndex = 0;
            lblEmpleado.Text = "Empleado:";
            // 
            // cboEmpleado
            // 
            cboEmpleado.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboEmpleado.AutoCompleteSource = AutoCompleteSource.ListItems;
            cboEmpleado.Location = new Point(100, 25);
            cboEmpleado.Name = "cboEmpleado";
            cboEmpleado.Size = new Size(250, 23);
            cboEmpleado.TabIndex = 1;
            // 
            // lblFecha
            // 
            lblFecha.Location = new Point(380, 30);
            lblFecha.Name = "lblFecha";
            lblFecha.Size = new Size(54, 23);
            lblFecha.TabIndex = 2;
            lblFecha.Text = "Fecha:";
            // 
            // dtpFecha
            // 
            dtpFecha.Format = DateTimePickerFormat.Short;
            dtpFecha.Location = new Point(440, 25);
            dtpFecha.Name = "dtpFecha";
            dtpFecha.Size = new Size(200, 23);
            dtpFecha.TabIndex = 3;
            // 
            // lblObservaciones
            // 
            lblObservaciones.Location = new Point(20, 70);
            lblObservaciones.Name = "lblObservaciones";
            lblObservaciones.Size = new Size(88, 23);
            lblObservaciones.TabIndex = 4;
            lblObservaciones.Text = "Observaciones:";
            // 
            // txtObservaciones
            // 
            txtObservaciones.Location = new Point(114, 65);
            txtObservaciones.Name = "txtObservaciones";
            txtObservaciones.Size = new Size(236, 23);
            txtObservaciones.TabIndex = 5;
            // 
            // grpHerramientas
            // 
            grpHerramientas.Controls.Add(dgvDisponibles);
            grpHerramientas.Controls.Add(dgvSeleccionadas);
            grpHerramientas.Controls.Add(btnAgregar);
            grpHerramientas.Controls.Add(btnQuitar);
            grpHerramientas.Location = new Point(20, 220);
            grpHerramientas.Name = "grpHerramientas";
            grpHerramientas.Size = new Size(760, 260);
            grpHerramientas.TabIndex = 2;
            grpHerramientas.TabStop = false;
            grpHerramientas.Text = "Selección de Herramientas";
            // 
            // dgvDisponibles
            // 
            dgvDisponibles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDisponibles.Location = new Point(10, 25);
            dgvDisponibles.MultiSelect = false;
            dgvDisponibles.Name = "dgvDisponibles";
            dgvDisponibles.ReadOnly = true;
            dgvDisponibles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDisponibles.Size = new Size(300, 200);
            dgvDisponibles.TabIndex = 0;
            // 
            // dgvSeleccionadas
            // 
            dgvSeleccionadas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSeleccionadas.Location = new Point(440, 25);
            dgvSeleccionadas.MultiSelect = false;
            dgvSeleccionadas.Name = "dgvSeleccionadas";
            dgvSeleccionadas.ReadOnly = true;
            dgvSeleccionadas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSeleccionadas.Size = new Size(300, 200);
            dgvSeleccionadas.TabIndex = 1;
            // 
            // btnAgregar
            // 
            btnAgregar.Location = new Point(330, 80);
            btnAgregar.Name = "btnAgregar";
            btnAgregar.Size = new Size(80, 40);
            btnAgregar.TabIndex = 2;
            btnAgregar.Text = ">>";
            // 
            // btnQuitar
            // 
            btnQuitar.Location = new Point(330, 140);
            btnQuitar.Name = "btnQuitar";
            btnQuitar.Size = new Size(80, 40);
            btnQuitar.TabIndex = 3;
            btnQuitar.Text = "<<";
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(520, 500);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(120, 40);
            btnGuardar.TabIndex = 0;
            btnGuardar.Text = "Guardar";
            // 
            // btnCancelar
            // 
            btnCancelar.Location = new Point(660, 500);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(120, 40);
            btnCancelar.TabIndex = 1;
            btnCancelar.Text = "Cancelar";
            btnCancelar.Click += btnCancelar_Click;
            // 
            // FrmPrestamo
            // 
            ClientSize = new Size(800, 560);
            Controls.Add(btnGuardar);
            Controls.Add(btnCancelar);
            Controls.Add(grpHerramientas);
            Controls.Add(grpPrestamo);
            Controls.Add(lblTitulo);
            Name = "FrmPrestamo";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Préstamos";
            grpPrestamo.ResumeLayout(false);
            grpPrestamo.PerformLayout();
            grpHerramientas.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvDisponibles).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvSeleccionadas).EndInit();
            ResumeLayout(false);
        }
    }
    
}