namespace PromacoHerra
{
    partial class FrmPrestamo
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.GroupBox grpPrestamo;
        private System.Windows.Forms.Label lblEmpleado;
        private PromacoHerra.Controls.EmpleadoPickerControl pickerEmpleado;
        private System.Windows.Forms.Label lblAprobadoPor;
        private PromacoHerra.Controls.EmpleadoPickerControl pickerAprobador;
        private System.Windows.Forms.Label lblFechaPrestamo;
        private System.Windows.Forms.DateTimePicker dtpFecha;
        private System.Windows.Forms.Label lblFechaDevolucion;
        private System.Windows.Forms.DateTimePicker dtpFechaDevolucion;
        private System.Windows.Forms.Label lblObservaciones;
        private PromacoHerra.Controls.MaterialTextBox txtObservaciones;
        private System.Windows.Forms.GroupBox grpHerramientas;
        private System.Windows.Forms.TableLayoutPanel tlpHerramientas;
        private System.Windows.Forms.DataGridView dgvDisponibles;
        private System.Windows.Forms.Panel pnlBotonesTransferencia;
        private System.Windows.Forms.DataGridView dgvSeleccionadas;
        private PromacoHerra.Controls.MaterialButton btnAgregar;
        private PromacoHerra.Controls.MaterialButton btnQuitar;
        private PromacoHerra.Controls.MaterialButton btnGuardar;
        private PromacoHerra.Controls.MaterialButton btnCancelar;

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
            pickerEmpleado = new PromacoHerra.Controls.EmpleadoPickerControl();
            lblAprobadoPor = new Label();
            pickerAprobador = new PromacoHerra.Controls.EmpleadoPickerControl();
            lblFechaPrestamo = new Label();
            dtpFecha = new DateTimePicker();
            lblFechaDevolucion = new Label();
            dtpFechaDevolucion = new DateTimePicker();
            lblObservaciones = new Label();
            txtObservaciones = new PromacoHerra.Controls.MaterialTextBox();
            grpHerramientas = new GroupBox();
            tlpHerramientas = new TableLayoutPanel();
            dgvDisponibles = new DataGridView();
            pnlBotonesTransferencia = new Panel();
            btnAgregar = new PromacoHerra.Controls.MaterialButton();
            btnQuitar = new PromacoHerra.Controls.MaterialButton();
            dgvSeleccionadas = new DataGridView();
            btnGuardar = new PromacoHerra.Controls.MaterialButton();
            btnCancelar = new PromacoHerra.Controls.MaterialButton();
            grpPrestamo.SuspendLayout();
            grpHerramientas.SuspendLayout();
            tlpHerramientas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDisponibles).BeginInit();
            pnlBotonesTransferencia.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSeleccionadas).BeginInit();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(925, 60);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Registro de Préstamos";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // grpPrestamo
            // 
            grpPrestamo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpPrestamo.Controls.Add(lblEmpleado);
            grpPrestamo.Controls.Add(pickerEmpleado);
            grpPrestamo.Controls.Add(lblAprobadoPor);
            grpPrestamo.Controls.Add(pickerAprobador);
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
            grpPrestamo.Enter += grpPrestamo_Enter;
            // 
            // lblEmpleado
            // 
            lblEmpleado.Location = new Point(15, 30);
            lblEmpleado.Name = "lblEmpleado";
            lblEmpleado.Size = new Size(110, 23);
            lblEmpleado.TabIndex = 0;
            lblEmpleado.Text = "Empleado:";
            // 
            // pickerEmpleado
            // 
            pickerEmpleado.Location = new Point(157, 30);
            pickerEmpleado.MinimumSize = new Size(200, 28);
            pickerEmpleado.Name = "pickerEmpleado";
            pickerEmpleado.Size = new Size(200, 28);
            pickerEmpleado.TabIndex = 1;
            // 
            // lblAprobadoPor
            // 
            lblAprobadoPor.Location = new Point(386, 30);
            lblAprobadoPor.Name = "lblAprobadoPor";
            lblAprobadoPor.Size = new Size(158, 23);
            lblAprobadoPor.TabIndex = 2;
            lblAprobadoPor.Text = "Aprobado por:";
            // 
            // pickerAprobador
            // 
            pickerAprobador.Location = new Point(592, 30);
            pickerAprobador.MinimumSize = new Size(200, 28);
            pickerAprobador.Name = "pickerAprobador";
            pickerAprobador.Size = new Size(200, 28);
            pickerAprobador.TabIndex = 3;
            // 
            // lblFechaPrestamo
            // 
            lblFechaPrestamo.Location = new Point(15, 70);
            lblFechaPrestamo.Name = "lblFechaPrestamo";
            lblFechaPrestamo.Size = new Size(136, 23);
            lblFechaPrestamo.TabIndex = 4;
            lblFechaPrestamo.Text = "Fecha préstamo:";
            // 
            // dtpFecha
            // 
            dtpFecha.CustomFormat = "dd/MM/yyyy";
            dtpFecha.Enabled = false;
            dtpFecha.Format = DateTimePickerFormat.Custom;
            dtpFecha.Location = new Point(157, 70);
            dtpFecha.Name = "dtpFecha";
            dtpFecha.Size = new Size(200, 23);
            dtpFecha.TabIndex = 5;
            // 
            // lblFechaDevolucion
            // 
            lblFechaDevolucion.Location = new Point(386, 70);
            lblFechaDevolucion.Name = "lblFechaDevolucion";
            lblFechaDevolucion.Size = new Size(200, 23);
            lblFechaDevolucion.TabIndex = 6;
            lblFechaDevolucion.Text = "Fecha devolución esperada:";
            // 
            // dtpFechaDevolucion
            // 
            dtpFechaDevolucion.CustomFormat = "dd/MM/yyyy";
            dtpFechaDevolucion.Format = DateTimePickerFormat.Custom;
            dtpFechaDevolucion.Location = new Point(592, 67);
            dtpFechaDevolucion.Name = "dtpFechaDevolucion";
            dtpFechaDevolucion.Size = new Size(200, 23);
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
            txtObservaciones.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtObservaciones.BackColor = Color.White;
            txtObservaciones.FloatingLabelText = "";
            txtObservaciones.Location = new Point(157, 110);
            txtObservaciones.MaxLength = 32767;
            txtObservaciones.MinimumSize = new Size(0, 32);
            txtObservaciones.Multiline = false;
            txtObservaciones.Name = "txtObservaciones";
            txtObservaciones.Padding = new Padding(0, 0, 0, 4);
            txtObservaciones.PasswordChar = '\0';
            txtObservaciones.PlaceholderText = "";
            txtObservaciones.ReadOnly = false;
            txtObservaciones.SelectionStart = 0;
            txtObservaciones.Size = new Size(540, 32);
            txtObservaciones.TabIndex = 9;
            txtObservaciones.UseFloatingLabel = false;
            // 
            // grpHerramientas
            // 
            grpHerramientas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            grpHerramientas.Controls.Add(tlpHerramientas);
            grpHerramientas.Location = new Point(20, 238);
            grpHerramientas.Name = "grpHerramientas";
            grpHerramientas.Size = new Size(880, 340);
            grpHerramientas.TabIndex = 2;
            grpHerramientas.TabStop = false;
            grpHerramientas.Text = "Selección de Herramientas";
            // 
            // tlpHerramientas
            // 
            tlpHerramientas.ColumnCount = 3;
            tlpHerramientas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
            tlpHerramientas.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));
            tlpHerramientas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
            tlpHerramientas.Controls.Add(dgvDisponibles, 0, 0);
            tlpHerramientas.Controls.Add(pnlBotonesTransferencia, 1, 0);
            tlpHerramientas.Controls.Add(dgvSeleccionadas, 2, 0);
            tlpHerramientas.Dock = DockStyle.Fill;
            tlpHerramientas.Location = new Point(3, 19);
            tlpHerramientas.Name = "tlpHerramientas";
            tlpHerramientas.Padding = new Padding(7, 6, 7, 6);
            tlpHerramientas.RowCount = 1;
            tlpHerramientas.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpHerramientas.Size = new Size(874, 318);
            tlpHerramientas.TabIndex = 0;
            // 
            // dgvDisponibles
            // 
            dgvDisponibles.AllowUserToAddRows = false;
            dgvDisponibles.AllowUserToDeleteRows = false;
            dgvDisponibles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDisponibles.Dock = DockStyle.Fill;
            dgvDisponibles.Location = new Point(10, 9);
            dgvDisponibles.Margin = new Padding(3, 3, 6, 3);
            dgvDisponibles.MultiSelect = false;
            dgvDisponibles.Name = "dgvDisponibles";
            dgvDisponibles.ReadOnly = true;
            dgvDisponibles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDisponibles.Size = new Size(376, 300);
            dgvDisponibles.TabIndex = 0;
            // 
            // pnlBotonesTransferencia
            // 
            pnlBotonesTransferencia.Anchor = AnchorStyles.None;
            pnlBotonesTransferencia.Controls.Add(btnAgregar);
            pnlBotonesTransferencia.Controls.Add(btnQuitar);
            pnlBotonesTransferencia.Location = new Point(399, 113);
            pnlBotonesTransferencia.Margin = new Padding(0);
            pnlBotonesTransferencia.Name = "pnlBotonesTransferencia";
            pnlBotonesTransferencia.Size = new Size(75, 91);
            pnlBotonesTransferencia.TabIndex = 1;
            // 
            // btnAgregar
            // 
            btnAgregar.CornerRadius = 8;
            btnAgregar.FlatStyle = FlatStyle.Flat;
            btnAgregar.Font = new Font("Segoe UI", 11.5F, FontStyle.Bold);
            btnAgregar.Icon = FontAwesome.Sharp.IconChar.ArrowRight;
            btnAgregar.IconColor = null;
            btnAgregar.IconSize = 22;
            btnAgregar.Location = new Point(0, 0);
            btnAgregar.Name = "btnAgregar";
            btnAgregar.Size = new Size(75, 38);
            btnAgregar.TabIndex = 0;
            btnAgregar.Variant = PromacoHerra.Controls.MaterialButtonVariant.Default;
            // 
            // btnQuitar
            // 
            btnQuitar.CornerRadius = 8;
            btnQuitar.FlatStyle = FlatStyle.Flat;
            btnQuitar.Font = new Font("Segoe UI", 11.5F, FontStyle.Bold);
            btnQuitar.Icon = FontAwesome.Sharp.IconChar.ArrowLeft;
            btnQuitar.IconColor = null;
            btnQuitar.IconSize = 22;
            btnQuitar.Location = new Point(0, 53);
            btnQuitar.Name = "btnQuitar";
            btnQuitar.Size = new Size(75, 38);
            btnQuitar.TabIndex = 1;
            btnQuitar.Variant = PromacoHerra.Controls.MaterialButtonVariant.Default;
            // 
            // dgvSeleccionadas
            // 
            dgvSeleccionadas.AllowUserToAddRows = false;
            dgvSeleccionadas.AllowUserToDeleteRows = false;
            dgvSeleccionadas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSeleccionadas.Dock = DockStyle.Fill;
            dgvSeleccionadas.Location = new Point(488, 9);
            dgvSeleccionadas.Margin = new Padding(6, 3, 3, 3);
            dgvSeleccionadas.MultiSelect = false;
            dgvSeleccionadas.Name = "dgvSeleccionadas";
            dgvSeleccionadas.ReadOnly = true;
            dgvSeleccionadas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSeleccionadas.Size = new Size(376, 300);
            dgvSeleccionadas.TabIndex = 1;
            dgvSeleccionadas.CellContentClick += dgvSeleccionadas_CellContentClick;
            // 
            // btnGuardar
            // 
            btnGuardar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnGuardar.CornerRadius = 8;
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnGuardar.Icon = FontAwesome.Sharp.IconChar.Save;
            btnGuardar.IconColor = null;
            btnGuardar.IconSize = 22;
            btnGuardar.Location = new Point(640, 590);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(120, 40);
            btnGuardar.TabIndex = 3;
            btnGuardar.Text = "Guardar";
            btnGuardar.Variant = PromacoHerra.Controls.MaterialButtonVariant.Default;
            // 
            // btnCancelar
            // 
            btnCancelar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancelar.CornerRadius = 8;
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnCancelar.Icon = FontAwesome.Sharp.IconChar.Close;
            btnCancelar.IconColor = null;
            btnCancelar.IconSize = 22;
            btnCancelar.Location = new Point(775, 590);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(120, 40);
            btnCancelar.TabIndex = 4;
            btnCancelar.Text = "Cancelar";
            btnCancelar.Variant = PromacoHerra.Controls.MaterialButtonVariant.Default;
            // 
            // FrmPrestamo
            // 
            ClientSize = new Size(925, 650);
            Controls.Add(lblTitulo);
            Controls.Add(grpPrestamo);
            Controls.Add(grpHerramientas);
            Controls.Add(btnGuardar);
            Controls.Add(btnCancelar);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FrmPrestamo";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Préstamos";
            grpPrestamo.ResumeLayout(false);
            grpHerramientas.ResumeLayout(false);
            tlpHerramientas.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvDisponibles).EndInit();
            pnlBotonesTransferencia.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvSeleccionadas).EndInit();
            ResumeLayout(false);
        }
    }
}