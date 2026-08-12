// FrmEmpleados.Designer.cs
// Reemplaza completamente el Designer anterior.
// Se eliminan los controles del CRUD y se agregan:
//   - btnSincronizar
//   - lblSincronizacion
//   - txtBuscar
// Se mantiene dgvEmpleados y lblTitulo que ya existían.

namespace PromacoHerra
{
    partial class FrmEmpleados
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Button btnSincronizar;
        private System.Windows.Forms.Label lblSincronizacion;
        private System.Windows.Forms.Label lblBuscar;
        private System.Windows.Forms.TextBox txtBuscar;
        private System.Windows.Forms.DataGridView dgvEmpleados;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitulo = new System.Windows.Forms.Label();
            btnSincronizar = new System.Windows.Forms.Button();
            lblSincronizacion = new System.Windows.Forms.Label();
            lblBuscar = new System.Windows.Forms.Label();
            txtBuscar = new System.Windows.Forms.TextBox();
            dgvEmpleados = new System.Windows.Forms.DataGridView();

            ((System.ComponentModel.ISupportInitialize)dgvEmpleados).BeginInit();
            SuspendLayout();

            // lblTitulo
            lblTitulo.Dock = System.Windows.Forms.DockStyle.Top;
            lblTitulo.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            lblTitulo.Size = new System.Drawing.Size(800, 60);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Empleados";
            lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // btnSincronizar
            btnSincronizar.Location = new System.Drawing.Point(20, 75);
            btnSincronizar.Size = new System.Drawing.Size(180, 36);
            btnSincronizar.TabIndex = 1;
            btnSincronizar.Text = "⟳  Sincronizar empleados";

            // lblSincronizacion
            lblSincronizacion.Location = new System.Drawing.Point(215, 83);
            lblSincronizacion.Size = new System.Drawing.Size(560, 20);
            lblSincronizacion.TabIndex = 2;
            lblSincronizacion.Text = "Presiona 'Sincronizar' para obtener los empleados desde RRHH.";
            lblSincronizacion.ForeColor = System.Drawing.Color.Gray;

            // lblBuscar
            lblBuscar.Location = new System.Drawing.Point(20, 128);
            lblBuscar.Size = new System.Drawing.Size(55, 23);
            lblBuscar.TabIndex = 3;
            lblBuscar.Text = "Buscar:";

            // txtBuscar
            txtBuscar.Location = new System.Drawing.Point(80, 125);
            txtBuscar.Size = new System.Drawing.Size(300, 23);
            txtBuscar.TabIndex = 4;
            txtBuscar.PlaceholderText = "Nombre o código...";

            // dgvEmpleados
            dgvEmpleados.AllowUserToAddRows = false;
            dgvEmpleados.AllowUserToDeleteRows = false;
            dgvEmpleados.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvEmpleados.Location = new System.Drawing.Point(20, 162);
            dgvEmpleados.MultiSelect = false;
            dgvEmpleados.ReadOnly = true;
            dgvEmpleados.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvEmpleados.Size = new System.Drawing.Size(740, 300);
            dgvEmpleados.TabIndex = 5;

            // FrmEmpleados
            ClientSize = new System.Drawing.Size(800, 500);
            Controls.Add(lblTitulo);
            Controls.Add(btnSincronizar);
            Controls.Add(lblSincronizacion);
            Controls.Add(lblBuscar);
            Controls.Add(txtBuscar);
            Controls.Add(dgvEmpleados);
            Name = "FrmEmpleados";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Empleados";

            ((System.ComponentModel.ISupportInitialize)dgvEmpleados).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}