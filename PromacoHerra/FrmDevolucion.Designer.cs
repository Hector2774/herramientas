namespace PromacoHerra
{
    partial class FrmDevolucion
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            panel1 = new System.Windows.Forms.Panel();
            panel5 = new System.Windows.Forms.Panel();
            txtObservacion = new PromacoHerra.Controls.MaterialTextBox();
            lblObservacion = new System.Windows.Forms.Label();
            btnDevolver = new PromacoHerra.Controls.MaterialButton();
            btnDevolverTodo = new PromacoHerra.Controls.MaterialButton();
            lblEstado = new System.Windows.Forms.Label();
            cboEstado = new PromacoHerra.Controls.MaterialComboBox();
            grpDetalle = new System.Windows.Forms.GroupBox();
            panel2 = new System.Windows.Forms.Panel();
            dgvDetalle = new System.Windows.Forms.DataGridView();
            grpPrestamos = new System.Windows.Forms.GroupBox();
            panel4 = new System.Windows.Forms.Panel();
            dgvPrestamos = new System.Windows.Forms.DataGridView();
            panel3 = new System.Windows.Forms.Panel();
            txtBuscar = new PromacoHerra.Controls.MaterialTextBox();
            lblBuscar = new System.Windows.Forms.Label();
            lblTitulo = new System.Windows.Forms.Label();

            panel1.SuspendLayout();
            panel5.SuspendLayout();
            grpDetalle.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDetalle).BeginInit();
            grpPrestamos.SuspendLayout();
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPrestamos).BeginInit();
            panel3.SuspendLayout();
            SuspendLayout();

            // panel1 — contenedor raíz
            panel1.Controls.Add(panel5);
            panel1.Controls.Add(grpDetalle);
            panel1.Controls.Add(grpPrestamos);
            panel1.Controls.Add(lblTitulo);
            panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(1055, 818);

            // lblTitulo
            lblTitulo.Dock = System.Windows.Forms.DockStyle.Top;
            lblTitulo.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new System.Drawing.Size(1055, 60);
            lblTitulo.Text = "Devolución de Herramientas";
            lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // grpPrestamos
            grpPrestamos.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            grpPrestamos.Controls.Add(panel3);
            grpPrestamos.Controls.Add(panel4);
            grpPrestamos.Location = new System.Drawing.Point(20, 81);
            grpPrestamos.Name = "grpPrestamos";
            grpPrestamos.Size = new System.Drawing.Size(1010, 263);
            grpPrestamos.TabStop = false;
            grpPrestamos.Text = "Préstamos Activos";

            // panel3 — buscador
            panel3.Controls.Add(lblBuscar);
            panel3.Controls.Add(txtBuscar);
            panel3.Location = new System.Drawing.Point(20, 19);
            panel3.Name = "panel3";
            panel3.Size = new System.Drawing.Size(344, 35);

            lblBuscar.Location = new System.Drawing.Point(8, 8);
            lblBuscar.Size = new System.Drawing.Size(54, 23);
            lblBuscar.Text = "Buscar:";

            txtBuscar.Location = new System.Drawing.Point(68, 3);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Size = new System.Drawing.Size(250, 23);
            txtBuscar.PlaceholderText = "Nombre o código empleado...";

            // panel4 — grid préstamos
            panel4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            panel4.Controls.Add(dgvPrestamos);
            panel4.Location = new System.Drawing.Point(20, 61);
            panel4.Name = "panel4";
            panel4.Size = new System.Drawing.Size(970, 196);

            dgvPrestamos.AllowUserToAddRows = false;
            dgvPrestamos.AllowUserToDeleteRows = false;
            dgvPrestamos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvPrestamos.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvPrestamos.MultiSelect = false;
            dgvPrestamos.Name = "dgvPrestamos";
            dgvPrestamos.ReadOnly = true;
            dgvPrestamos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            // grpDetalle
            grpDetalle.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            grpDetalle.Controls.Add(panel2);
            grpDetalle.Location = new System.Drawing.Point(20, 360);
            grpDetalle.Name = "grpDetalle";
            grpDetalle.Size = new System.Drawing.Size(1010, 205);
            grpDetalle.TabStop = false;
            grpDetalle.Text = "Detalle del Préstamo";

            // panel2 — grid detalle
            panel2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            panel2.Controls.Add(dgvDetalle);
            panel2.Location = new System.Drawing.Point(10, 22);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(990, 172);

            dgvDetalle.AllowUserToAddRows = false;
            dgvDetalle.AllowUserToDeleteRows = false;
            dgvDetalle.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvDetalle.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvDetalle.MultiSelect = false;
            dgvDetalle.Name = "dgvDetalle";
            dgvDetalle.ReadOnly = true;
            dgvDetalle.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            // panel5 — controles de devolución
            panel5.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            panel5.Controls.Add(lblObservacion);
            panel5.Controls.Add(txtObservacion);
            panel5.Controls.Add(lblEstado);
            panel5.Controls.Add(cboEstado);
            panel5.Controls.Add(btnDevolver);
            panel5.Controls.Add(btnDevolverTodo);
            panel5.Location = new System.Drawing.Point(20, 578);
            panel5.Name = "panel5";
            panel5.Size = new System.Drawing.Size(1010, 90);

            lblObservacion.Location = new System.Drawing.Point(3, 21);
            lblObservacion.Size = new System.Drawing.Size(94, 23);
            lblObservacion.Text = "Observación:";

            txtObservacion.Location = new System.Drawing.Point(103, 18);
            txtObservacion.Name = "txtObservacion";
            txtObservacion.Size = new System.Drawing.Size(300, 23);

            lblEstado.Location = new System.Drawing.Point(420, 21);
            lblEstado.Size = new System.Drawing.Size(90, 23);
            lblEstado.Text = "Estado devolución:";

            // cboEstado — valores correctos: Bueno / Dañado / Perdido
            cboEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboEstado.Location = new System.Drawing.Point(515, 18);
            cboEstado.Name = "cboEstado";
            cboEstado.Size = new System.Drawing.Size(130, 23);

            btnDevolver.Icon = FontAwesome.Sharp.IconChar.Undo;
            btnDevolver.IconSize = 22;
            btnDevolver.Location = new System.Drawing.Point(5, 52);
            btnDevolver.Name = "btnDevolver";
            btnDevolver.Size = new System.Drawing.Size(200, 36);
            btnDevolver.Text = "Devolver seleccionado";
            btnDevolver.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);

            btnDevolverTodo.Icon = FontAwesome.Sharp.IconChar.CheckDouble;
            btnDevolverTodo.IconSize = 22;
            btnDevolverTodo.Location = new System.Drawing.Point(215, 52);
            btnDevolverTodo.Name = "btnDevolverTodo";
            btnDevolverTodo.Size = new System.Drawing.Size(200, 36);
            btnDevolverTodo.Text = "Devolver todo";
            btnDevolverTodo.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);

            // Form
            ClientSize = new System.Drawing.Size(1055, 818);
            Controls.Add(panel1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FrmDevolucion";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Devoluciones";

            panel1.ResumeLayout(false);
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            grpDetalle.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvDetalle).EndInit();
            grpPrestamos.ResumeLayout(false);
            panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvPrestamos).EndInit();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ResumeLayout(false);
        }

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblObservacion;
        private PromacoHerra.Controls.MaterialTextBox txtObservacion;
        private System.Windows.Forms.Label lblEstado;
        private PromacoHerra.Controls.MaterialComboBox cboEstado;
        private PromacoHerra.Controls.MaterialButton btnDevolver;
        private PromacoHerra.Controls.MaterialButton btnDevolverTodo;
        private System.Windows.Forms.GroupBox grpDetalle;
        private System.Windows.Forms.DataGridView dgvDetalle;
        private System.Windows.Forms.GroupBox grpPrestamos;
        private System.Windows.Forms.Label lblBuscar;
        private PromacoHerra.Controls.MaterialTextBox txtBuscar;
        private System.Windows.Forms.DataGridView dgvPrestamos;
    }
}