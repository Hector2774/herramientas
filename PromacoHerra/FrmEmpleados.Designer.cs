namespace PromacoHerra
{
    partial class FrmEmpleados
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblSubtitulo;
        private PromacoHerra.Controls.MaterialButton btnSincronizar;
        private System.Windows.Forms.ProgressBar prgSincronizacion;
        private System.Windows.Forms.Panel pnlFiltros;
        private FontAwesome.Sharp.IconPictureBox icoBuscar;
        private PromacoHerra.Controls.MaterialTextBox txtBuscar;
        private PromacoHerra.Controls.MaterialComboBox cboDepartamento;
        private System.Windows.Forms.Label lblMostrando;
        private System.Windows.Forms.Panel pnlContenido;
        private System.Windows.Forms.Panel pnlVacio;
        private FontAwesome.Sharp.IconPictureBox icoVacio;
        private System.Windows.Forms.Label lblVacioTitulo;
        private System.Windows.Forms.Label lblVacioSubtitulo;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pnlHeader = new Panel();
            lblTitulo = new Label();
            lblSubtitulo = new Label();
            btnSincronizar = new PromacoHerra.Controls.MaterialButton();
            prgSincronizacion = new ProgressBar();
            pnlFiltros = new Panel();
            icoBuscar = new FontAwesome.Sharp.IconPictureBox();
            txtBuscar = new PromacoHerra.Controls.MaterialTextBox();
            cboDepartamento = new PromacoHerra.Controls.MaterialComboBox();
            lblMostrando = new Label();
            pnlContenido = new Panel();
            pnlVacio = new Panel();
            icoVacio = new FontAwesome.Sharp.IconPictureBox();
            lblVacioTitulo = new Label();
            lblVacioSubtitulo = new Label();
            pnlHeader.SuspendLayout();
            pnlFiltros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)icoBuscar).BeginInit();
            pnlContenido.SuspendLayout();
            pnlVacio.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)icoVacio).BeginInit();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.Controls.Add(lblTitulo);
            pnlHeader.Controls.Add(lblSubtitulo);
            pnlHeader.Controls.Add(btnSincronizar);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1000, 86);
            pnlHeader.TabIndex = 0;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblTitulo.Location = new Point(22, 14);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(114, 28);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Empleados";
            // 
            // lblSubtitulo
            // 
            lblSubtitulo.AutoEllipsis = true;
            lblSubtitulo.Font = new Font("Segoe UI", 9.5F);
            lblSubtitulo.Location = new Point(24, 50);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(700, 22);
            lblSubtitulo.TabIndex = 1;
            lblSubtitulo.Text = "0 empleados";
            // 
            // btnSincronizar
            // 
            btnSincronizar.CornerRadius = 8;
            btnSincronizar.FlatStyle = FlatStyle.Flat;
            btnSincronizar.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnSincronizar.Icon = FontAwesome.Sharp.IconChar.Refresh;
            btnSincronizar.IconColor = null;
            btnSincronizar.IconSize = 16;
            btnSincronizar.Location = new Point(746, 22);
            btnSincronizar.Name = "btnSincronizar";
            btnSincronizar.Size = new Size(230, 40);
            btnSincronizar.TabIndex = 2;
            btnSincronizar.Text = "Sincronizar Empleados";
            btnSincronizar.Variant = PromacoHerra.Controls.MaterialButtonVariant.Primary;
            // 
            // prgSincronizacion
            // 
            prgSincronizacion.Dock = DockStyle.Top;
            prgSincronizacion.Location = new Point(0, 86);
            prgSincronizacion.MarqueeAnimationSpeed = 25;
            prgSincronizacion.Name = "prgSincronizacion";
            prgSincronizacion.Size = new Size(1000, 4);
            prgSincronizacion.Style = ProgressBarStyle.Marquee;
            prgSincronizacion.TabIndex = 1;
            prgSincronizacion.Visible = false;
            // 
            // pnlFiltros
            // 
            pnlFiltros.Controls.Add(icoBuscar);
            pnlFiltros.Controls.Add(txtBuscar);
            pnlFiltros.Controls.Add(cboDepartamento);
            pnlFiltros.Controls.Add(lblMostrando);
            pnlFiltros.Dock = DockStyle.Top;
            pnlFiltros.Location = new Point(0, 90);
            pnlFiltros.Name = "pnlFiltros";
            pnlFiltros.Size = new Size(1000, 60);
            pnlFiltros.TabIndex = 2;
            // 
            // icoBuscar
            // 
            icoBuscar.BackColor = SystemColors.Control;
            icoBuscar.ForeColor = SystemColors.ControlText;
            icoBuscar.IconChar = FontAwesome.Sharp.IconChar.Search;
            icoBuscar.IconColor = SystemColors.ControlText;
            icoBuscar.IconFont = FontAwesome.Sharp.IconFont.Auto;
            icoBuscar.IconSize = 22;
            icoBuscar.Location = new Point(24, 19);
            icoBuscar.Name = "icoBuscar";
            icoBuscar.Size = new Size(22, 22);
            icoBuscar.TabIndex = 0;
            icoBuscar.TabStop = false;
            // 
            // txtBuscar
            // 
            txtBuscar.BackColor = Color.White;
            txtBuscar.FloatingLabelText = "";
            txtBuscar.Location = new Point(52, 10);
            txtBuscar.MaxLength = 32767;
            txtBuscar.MinimumSize = new Size(0, 38);
            txtBuscar.Multiline = false;
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Padding = new Padding(0, 0, 0, 4);
            txtBuscar.PasswordChar = '\0';
            txtBuscar.PlaceholderText = "Buscar por nombre o código…";
            txtBuscar.ReadOnly = false;
            txtBuscar.SelectionStart = 0;
            txtBuscar.Size = new Size(280, 38);
            txtBuscar.TabIndex = 1;
            txtBuscar.UseFloatingLabel = false;
            // 
            // cboDepartamento
            // 
            cboDepartamento.BackColor = Color.White;
            cboDepartamento.DataSource = null;
            cboDepartamento.DisplayMember = "";
            cboDepartamento.DropDownStyle = ComboBoxStyle.DropDownList;
            cboDepartamento.Location = new Point(348, 10);
            cboDepartamento.MinimumSize = new Size(0, 38);
            cboDepartamento.Name = "cboDepartamento";
            cboDepartamento.Padding = new Padding(0, 0, 0, 4);
            cboDepartamento.SelectedIndex = -1;
            cboDepartamento.SelectedItem = null;
            cboDepartamento.SelectedValue = null;
            cboDepartamento.Size = new Size(200, 38);
            cboDepartamento.TabIndex = 2;
            cboDepartamento.ValueMember = "";
            // 
            // lblMostrando
            // 
            lblMostrando.Font = new Font("Segoe UI", 9F);
            lblMostrando.Location = new Point(676, 20);
            lblMostrando.Name = "lblMostrando";
            lblMostrando.Size = new Size(300, 20);
            lblMostrando.TabIndex = 3;
            lblMostrando.Text = "Mostrando 0 de 0 empleados";
            lblMostrando.TextAlign = ContentAlignment.MiddleRight;
            // 
            // pnlContenido
            // 
            pnlContenido.Controls.Add(pnlVacio);
            pnlContenido.Dock = DockStyle.Fill;
            pnlContenido.Location = new Point(0, 150);
            pnlContenido.Name = "pnlContenido";
            pnlContenido.Padding = new Padding(24, 0, 24, 20);
            pnlContenido.Size = new Size(1000, 490);
            pnlContenido.TabIndex = 3;
            // 
            // pnlVacio
            // 
            pnlVacio.BackColor = Color.White;
            pnlVacio.Controls.Add(icoVacio);
            pnlVacio.Controls.Add(lblVacioTitulo);
            pnlVacio.Controls.Add(lblVacioSubtitulo);
            pnlVacio.Dock = DockStyle.Fill;
            pnlVacio.Location = new Point(24, 0);
            pnlVacio.Name = "pnlVacio";
            pnlVacio.Size = new Size(952, 470);
            pnlVacio.TabIndex = 0;
            pnlVacio.Visible = false;
            // 
            // icoVacio
            // 
            icoVacio.BackColor = Color.White;
            icoVacio.ForeColor = SystemColors.ControlText;
            icoVacio.IconChar = FontAwesome.Sharp.IconChar.Users;
            icoVacio.IconColor = SystemColors.ControlText;
            icoVacio.IconFont = FontAwesome.Sharp.IconFont.Auto;
            icoVacio.IconSize = 64;
            icoVacio.Location = new Point(440, 120);
            icoVacio.Name = "icoVacio";
            icoVacio.Size = new Size(72, 64);
            icoVacio.TabIndex = 0;
            icoVacio.TabStop = false;
            // 
            // lblVacioTitulo
            // 
            lblVacioTitulo.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblVacioTitulo.Location = new Point(200, 196);
            lblVacioTitulo.Name = "lblVacioTitulo";
            lblVacioTitulo.Size = new Size(552, 30);
            lblVacioTitulo.TabIndex = 1;
            lblVacioTitulo.Text = "No hay empleados cargados";
            lblVacioTitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblVacioSubtitulo
            // 
            lblVacioSubtitulo.Font = new Font("Segoe UI", 10F);
            lblVacioSubtitulo.Location = new Point(200, 228);
            lblVacioSubtitulo.Name = "lblVacioSubtitulo";
            lblVacioSubtitulo.Size = new Size(552, 24);
            lblVacioSubtitulo.TabIndex = 2;
            lblVacioSubtitulo.Text = "Presiona 'Sincronizar Empleados' para cargar la lista de empleados";
            lblVacioSubtitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // FrmEmpleados
            // 
            ClientSize = new Size(1000, 640);
            Controls.Add(pnlContenido);
            Controls.Add(pnlFiltros);
            Controls.Add(prgSincronizacion);
            Controls.Add(pnlHeader);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FrmEmpleados";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Empleados";
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlFiltros.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)icoBuscar).EndInit();
            pnlContenido.ResumeLayout(false);
            pnlVacio.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)icoVacio).EndInit();
            ResumeLayout(false);
        }
    }
}
