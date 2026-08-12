namespace PromacoHerra
{
    partial class FrmPrestamo
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.GroupBox grpPrestamo;
        private System.Windows.Forms.Label lblEmpleado;
        private System.Windows.Forms.ComboBox cboEmpleado;
        private System.Windows.Forms.Label lblAprobadoPor;
        private System.Windows.Forms.ComboBox cboAprobadoPor;
        private System.Windows.Forms.Label lblFechaPrestamo;
        private System.Windows.Forms.DateTimePicker dtpFecha;
        private System.Windows.Forms.Label lblFechaDevolucion;
        private System.Windows.Forms.DateTimePicker dtpFechaDevolucion;
        private System.Windows.Forms.Label lblObservaciones;
        private System.Windows.Forms.TextBox txtObservaciones;
        private System.Windows.Forms.GroupBox grpHerramientas;
        private System.Windows.Forms.DataGridView dgvDisponibles;
        private System.Windows.Forms.DataGridView dgvSeleccionadas;
        private System.Windows.Forms.Button btnAgregar;
        private System.Windows.Forms.Button btnQuitar;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnCancelar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitulo = new Label();
            grpPrestamo = new GroupBox();
            lblEmpleado = new Label();
            cboEmpleado = new ComboBox();
            lblAprobadoPor = new Label();
            cboAprobadoPor = new ComboBox();
            lblFechaPrestamo = new Label();
            dtpFecha = new DateTimePicker();
            lblFechaDevolucion = new Label();
            dtpFechaDevolucion = new DateTimePicker();
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
            lblTitulo.Size = new Size(925, 60);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Registro de Préstamos";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // grpPrestamo
            // 
            grpPrestamo.Controls.Add(lblEmpleado);
            grpPrestamo.Controls.Add(cboEmpleado);
            grpPrestamo.Controls.Add(lblAprobadoPor);
            grpPrestamo.Controls.Add(cboAprobadoPor);
            grpPrestamo.Controls.Add(lblFechaPrestamo);
            grpPrestamo.Controls.Add(dtpFecha);
            grpPrestamo.Controls.Add(lblFechaDevolucion);
            grpPrestamo.Controls.Add(dtpFechaDevolucion);
            grpPrestamo.Controls.Add(lblObservaciones);
            grpPrestamo.Controls.Add(txtObservaciones);
            grpPrestamo.Location = new Point(20, 70);
            grpPrestamo.Name = "grpPrestamo";
            grpPrestamo.Size = new Size(880, 155);
            grpPrestamo.TabIndex = 1;
            grpPrestamo.TabStop = false;
            grpPrestamo.Text = "Datos del Préstamo";
            // 
            // lblEmpleado
            // 
            lblEmpleado.Location = new Point(15, 30);
            lblEmpleado.Name = "lblEmpleado";
            lblEmpleado.Size = new Size(75, 23);
            lblEmpleado.TabIndex = 0;
            lblEmpleado.Text = "Empleado:";
            // 
            // cboEmpleado
            // 
            cboEmpleado.AutoCompleteMode = AutoCompleteMode.Append;
            cboEmpleado.AutoCompleteSource = AutoCompleteSource.ListItems;
            cboEmpleado.DropDownStyle = ComboBoxStyle.DropDownList;
            cboEmpleado.Location = new Point(95, 27);
            cboEmpleado.Name = "cboEmpleado";
            cboEmpleado.Size = new Size(230, 23);
            cboEmpleado.TabIndex = 1;
            // 
            // lblAprobadoPor
            // 
            lblAprobadoPor.Location = new Point(345, 30);
            lblAprobadoPor.Name = "lblAprobadoPor";
            lblAprobadoPor.Size = new Size(85, 23);
            lblAprobadoPor.TabIndex = 2;
            lblAprobadoPor.Text = "Aprobado por:";
            // 
            // cboAprobadoPor
            // 
            cboAprobadoPor.DropDownStyle = ComboBoxStyle.DropDownList;
            cboAprobadoPor.Location = new Point(435, 27);
            cboAprobadoPor.Name = "cboAprobadoPor";
            cboAprobadoPor.Size = new Size(230, 23);
            cboAprobadoPor.TabIndex = 3;
            // 
            // lblFechaPrestamo
            // 
            lblFechaPrestamo.Location = new Point(15, 70);
            lblFechaPrestamo.Name = "lblFechaPrestamo";
            lblFechaPrestamo.Size = new Size(105, 23);
            lblFechaPrestamo.TabIndex = 4;
            lblFechaPrestamo.Text = "Fecha préstamo:";
            // 
            // dtpFecha
            // 
            dtpFecha.Enabled = false;
            dtpFecha.Format = DateTimePickerFormat.Short;
            dtpFecha.Location = new Point(125, 67);
            dtpFecha.Name = "dtpFecha";
            dtpFecha.Size = new Size(180, 23);
            dtpFecha.TabIndex = 5;
            // 
            // lblFechaDevolucion
            // 
            lblFechaDevolucion.Location = new Point(325, 70);
            lblFechaDevolucion.Name = "lblFechaDevolucion";
            lblFechaDevolucion.Size = new Size(145, 23);
            lblFechaDevolucion.TabIndex = 6;
            lblFechaDevolucion.Text = "Fecha devolución esperada:";
            // 
            // dtpFechaDevolucion
            // 
            dtpFechaDevolucion.Format = DateTimePickerFormat.Short;
            dtpFechaDevolucion.Location = new Point(475, 67);
            dtpFechaDevolucion.Name = "dtpFechaDevolucion";
            dtpFechaDevolucion.Size = new Size(180, 23);
            dtpFechaDevolucion.TabIndex = 7;
            // 
            // lblObservaciones
            // 
            lblObservaciones.Location = new Point(15, 110);
            lblObservaciones.Name = "lblObservaciones";
            lblObservaciones.Size = new Size(95, 23);
            lblObservaciones.TabIndex = 8;
            lblObservaciones.Text = "Observaciones:";
            // 
            // txtObservaciones
            // 
            txtObservaciones.Location = new Point(115, 107);
            txtObservaciones.Name = "txtObservaciones";
            txtObservaciones.Size = new Size(540, 23);
            txtObservaciones.TabIndex = 9;
            // 
            // grpHerramientas
            // 
            grpHerramientas.Controls.Add(dgvDisponibles);
            grpHerramientas.Controls.Add(dgvSeleccionadas);
            grpHerramientas.Controls.Add(btnAgregar);
            grpHerramientas.Controls.Add(btnQuitar);
            grpHerramientas.Location = new Point(20, 238);
            grpHerramientas.Name = "grpHerramientas";
            grpHerramientas.Size = new Size(880, 340);
            grpHerramientas.TabIndex = 2;
            grpHerramientas.TabStop = false;
            grpHerramientas.Text = "Selección de Herramientas";
            // 
            // dgvDisponibles
            // 
            dgvDisponibles.AllowUserToAddRows = false;
            dgvDisponibles.AllowUserToDeleteRows = false;
            dgvDisponibles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDisponibles.Location = new Point(10, 25);
            dgvDisponibles.MultiSelect = false;
            dgvDisponibles.Name = "dgvDisponibles";
            dgvDisponibles.ReadOnly = true;
            dgvDisponibles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDisponibles.Size = new Size(380, 300);
            dgvDisponibles.TabIndex = 0;
            // 
            // dgvSeleccionadas
            // 
            dgvSeleccionadas.AllowUserToAddRows = false;
            dgvSeleccionadas.AllowUserToDeleteRows = false;
            dgvSeleccionadas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSeleccionadas.Location = new Point(480, 25);
            dgvSeleccionadas.MultiSelect = false;
            dgvSeleccionadas.Name = "dgvSeleccionadas";
            dgvSeleccionadas.ReadOnly = true;
            dgvSeleccionadas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSeleccionadas.Size = new Size(390, 300);
            dgvSeleccionadas.TabIndex = 1;
            // 
            // btnAgregar
            // 
            btnAgregar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnAgregar.Location = new Point(398, 120);
            btnAgregar.Name = "btnAgregar";
            btnAgregar.Size = new Size(75, 38);
            btnAgregar.TabIndex = 2;
            btnAgregar.Text = ">>";
            // 
            // btnQuitar
            // 
            btnQuitar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnQuitar.Location = new Point(398, 175);
            btnQuitar.Name = "btnQuitar";
            btnQuitar.Size = new Size(75, 38);
            btnQuitar.TabIndex = 3;
            btnQuitar.Text = "<<";
            // 
            // btnGuardar
            // 
            btnGuardar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnGuardar.Location = new Point(640, 590);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(120, 40);
            btnGuardar.TabIndex = 3;
            btnGuardar.Text = "Guardar";
            // 
            // btnCancelar
            // 
            btnCancelar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnCancelar.Location = new Point(775, 590);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(120, 40);
            btnCancelar.TabIndex = 4;
            btnCancelar.Text = "Cancelar";
            // 
            // FrmPrestamo
            // 
            ClientSize = new Size(925, 650);
            Controls.Add(lblTitulo);
            Controls.Add(grpPrestamo);
            Controls.Add(grpHerramientas);
            Controls.Add(btnGuardar);
            Controls.Add(btnCancelar);
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