namespace PromacoHerra
{
    partial class FrmHerramientas
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblCodigo;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.Label lblCaracteristicas;
        private System.Windows.Forms.Label lblEstado;

        private System.Windows.Forms.TextBox txtCodigo;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.TextBox txtCaracteristicas;
        private System.Windows.Forms.ComboBox cboEstado;

        private System.Windows.Forms.Button btnNuevo;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnEliminar;

        private System.Windows.Forms.DataGridView dgvHerramientas;
        private System.Windows.Forms.GroupBox grpDatos;

        private System.Windows.Forms.Label lblBuscar;
        private System.Windows.Forms.TextBox txtBuscar;

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
            grpDatos = new GroupBox();
            lblCodigo = new Label();
            txtCodigo = new TextBox();
            lblNombre = new Label();
            txtNombre = new TextBox();
            lblEstado = new Label();
            cboEstado = new ComboBox();
            lblCaracteristicas = new Label();
            txtCaracteristicas = new TextBox();
            btnNuevo = new Button();
            btnGuardar = new Button();
            btnEditar = new Button();
            btnCancelar = new Button();
            btnEliminar = new Button();
            dgvHerramientas = new DataGridView();
            lblBuscar = new Label();
            txtBuscar = new TextBox();
            grpDatos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvHerramientas).BeginInit();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(800, 60);
            lblTitulo.TabIndex = 2;
            lblTitulo.Text = "Gestión de Herramientas";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // grpDatos
            // 
            grpDatos.Controls.Add(lblCodigo);
            grpDatos.Controls.Add(txtCodigo);
            grpDatos.Controls.Add(lblNombre);
            grpDatos.Controls.Add(txtNombre);
            grpDatos.Controls.Add(lblEstado);
            grpDatos.Controls.Add(cboEstado);
            grpDatos.Controls.Add(lblCaracteristicas);
            grpDatos.Controls.Add(txtCaracteristicas);
            grpDatos.Controls.Add(btnNuevo);
            grpDatos.Controls.Add(btnGuardar);
            grpDatos.Controls.Add(btnEditar);
            grpDatos.Controls.Add(btnCancelar);
            grpDatos.Controls.Add(btnEliminar);
            grpDatos.Location = new Point(20, 70);
            grpDatos.Name = "grpDatos";
            grpDatos.Size = new Size(740, 150);
            grpDatos.TabIndex = 1;
            grpDatos.TabStop = false;
            grpDatos.Text = "Datos de la Herramienta";
            // 
            // lblCodigo
            // 
            lblCodigo.Location = new Point(20, 30);
            lblCodigo.Name = "lblCodigo";
            lblCodigo.Size = new Size(75, 23);
            lblCodigo.TabIndex = 0;
            lblCodigo.Text = "Código:";
            // 
            // txtCodigo
            // 
            txtCodigo.Location = new Point(100, 25);
            txtCodigo.Name = "txtCodigo";
            txtCodigo.Size = new Size(150, 23);
            txtCodigo.TabIndex = 1;
            // 
            // lblNombre
            // 
            lblNombre.Location = new Point(265, 30);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(89, 23);
            lblNombre.TabIndex = 2;
            lblNombre.Text = "Nombre:";
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(360, 25);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(200, 23);
            txtNombre.TabIndex = 3;
            // 
            // lblEstado
            // 
            lblEstado.Location = new Point(20, 70);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(75, 23);
            lblEstado.TabIndex = 4;
            lblEstado.Text = "Estado:";
            // 
            // cboEstado
            // 
            cboEstado.DropDownStyle = ComboBoxStyle.DropDownList;
            cboEstado.Location = new Point(100, 65);
            cboEstado.Name = "cboEstado";
            cboEstado.Size = new Size(150, 23);
            cboEstado.TabIndex = 5;
            // 
            // lblCaracteristicas
            // 
            lblCaracteristicas.Location = new Point(265, 70);
            lblCaracteristicas.Name = "lblCaracteristicas";
            lblCaracteristicas.Size = new Size(94, 23);
            lblCaracteristicas.TabIndex = 6;
            lblCaracteristicas.Text = "Características:";
            // 
            // txtCaracteristicas
            // 
            txtCaracteristicas.Location = new Point(360, 67);
            txtCaracteristicas.Name = "txtCaracteristicas";
            txtCaracteristicas.Size = new Size(300, 23);
            txtCaracteristicas.TabIndex = 7;
            // 
            // btnNuevo
            // 
            btnNuevo.Location = new Point(20, 110);
            btnNuevo.Name = "btnNuevo";
            btnNuevo.Size = new Size(75, 23);
            btnNuevo.TabIndex = 8;
            btnNuevo.Text = "Nuevo";
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(110, 110);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(75, 23);
            btnGuardar.TabIndex = 10;
            btnGuardar.Text = "Guardar";
            // 
            // btnEditar
            // 
            btnEditar.Location = new Point(210, 110);
            btnEditar.Name = "btnEditar";
            btnEditar.Size = new Size(75, 23);
            btnEditar.TabIndex = 9;
            btnEditar.Text = "Editar";
            // 
            // btnCancelar
            // 
            btnCancelar.Location = new Point(310, 110);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(75, 23);
            btnCancelar.TabIndex = 11;
            btnCancelar.Text = "Cancelar";
            // 
            // btnEliminar
            // 
            btnEliminar.Location = new Point(410, 110);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(75, 23);
            btnEliminar.TabIndex = 12;
            btnEliminar.Text = "Eliminar";
            // 
            // dgvHerramientas
            // 
            dgvHerramientas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHerramientas.Location = new Point(20, 240);
            dgvHerramientas.MultiSelect = false;
            dgvHerramientas.Name = "dgvHerramientas";
            dgvHerramientas.ReadOnly = true;
            dgvHerramientas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHerramientas.Size = new Size(740, 250);
            dgvHerramientas.TabIndex = 0;
            // 
            // lblBuscar
            // 
            lblBuscar.Location = new Point(20, 210);
            lblBuscar.Name = "lblBuscar";
            lblBuscar.Size = new Size(54, 23);
            lblBuscar.TabIndex = 0;
            lblBuscar.Text = "Buscar:";
            // 
            // txtBuscar
            // 
            txtBuscar.Location = new Point(80, 205);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Size = new Size(250, 23);
            txtBuscar.TabIndex = 1;
            // 
            // FrmHerramientas
            // 
            ClientSize = new Size(800, 520);
            Controls.Add(lblBuscar);
            Controls.Add(txtBuscar);
            Controls.Add(dgvHerramientas);
            Controls.Add(grpDatos);
            Controls.Add(lblTitulo);
            Name = "FrmHerramientas";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Herramientas";
            Load += FrmHerramientas_Load;
            grpDatos.ResumeLayout(false);
            grpDatos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvHerramientas).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}