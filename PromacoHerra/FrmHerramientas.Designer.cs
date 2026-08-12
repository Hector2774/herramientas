namespace PromacoHerra
{
    partial class FrmHerramientas
    {
        private System.ComponentModel.IContainer components = null;

        // Título
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabHerramientas;
        private System.Windows.Forms.TabPage tabCategorias;
        private System.Windows.Forms.TabPage tabMarcas;
        private System.Windows.Forms.TabPage tabMantenimiento;

        // ── TAB HERRAMIENTAS ──────────────────────────────────────
        private System.Windows.Forms.GroupBox grpDatos;
        private System.Windows.Forms.Label lblCodigo;
        private System.Windows.Forms.TextBox txtCodigo;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label lblCategoria;
        private System.Windows.Forms.ComboBox cboCategoria;
        private System.Windows.Forms.Label lblMarca;
        private System.Windows.Forms.ComboBox cboMarca;
        private System.Windows.Forms.Label lblUbicacion;
        private System.Windows.Forms.ComboBox cboUbicacion;
        private System.Windows.Forms.Label lblStockTotal;
        private System.Windows.Forms.NumericUpDown nudStockTotal;
        private System.Windows.Forms.Label lblCaracteristicas;
        private System.Windows.Forms.TextBox txtCaracteristicas;
        private System.Windows.Forms.Button btnNuevo;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Label lblBuscar;
        private System.Windows.Forms.TextBox txtBuscar;
        private System.Windows.Forms.DataGridView dgvHerramientas;

        // ── TAB CATEGORÍAS ────────────────────────────────────────
        private System.Windows.Forms.GroupBox grpDatosCategoria;
        private System.Windows.Forms.Label lblNombreCategoria;
        private System.Windows.Forms.TextBox txtNombreCategoria;
        private System.Windows.Forms.Label lblDescCategoria;
        private System.Windows.Forms.TextBox txtDescCategoria;
        private System.Windows.Forms.Button btnNuevoCategoria;
        private System.Windows.Forms.Button btnGuardarCategoria;
        private System.Windows.Forms.Button btnEditarCategoria;
        private System.Windows.Forms.Button btnCancelarCategoria;
        private System.Windows.Forms.Button btnEliminarCategoria;
        private System.Windows.Forms.DataGridView dgvCategorias;

        // ── TAB MARCAS ────────────────────────────────────────────
        private System.Windows.Forms.GroupBox grpDatosMarca;
        private System.Windows.Forms.Label lblNombreMarca;
        private System.Windows.Forms.TextBox txtNombreMarca;
        private System.Windows.Forms.Label lblDescMarca;
        private System.Windows.Forms.TextBox txtDescMarca;
        private System.Windows.Forms.Button btnNuevoMarca;
        private System.Windows.Forms.Button btnGuardarMarca;
        private System.Windows.Forms.Button btnEditarMarca;
        private System.Windows.Forms.Button btnCancelarMarca;
        private System.Windows.Forms.Button btnEliminarMarca;
        private System.Windows.Forms.DataGridView dgvMarcas;

        // ── TAB MANTENIMIENTO ─────────────────────────────────────
        // Grid superior — mantenimientos activos
        private System.Windows.Forms.GroupBox grpMantenimientosActivos;
        private System.Windows.Forms.DataGridView dgvMantenimientos;
        // Panel inferior — acciones
        private System.Windows.Forms.GroupBox grpAccionMantenimiento;
        // Abrir mantenimiento
        private System.Windows.Forms.Label lblHerramientaMant;
        private System.Windows.Forms.ComboBox cboHerramientaMant;
        private System.Windows.Forms.Label lblTipoMant;
        private System.Windows.Forms.ComboBox cboTipoMant;
        private System.Windows.Forms.Label lblRealizadoPor;
        private System.Windows.Forms.TextBox txtRealizadoPor;
        private System.Windows.Forms.Label lblDescMant;
        private System.Windows.Forms.TextBox txtDescMant;
        private System.Windows.Forms.Button btnAbrirMantenimiento;
        // Cerrar mantenimiento
        private System.Windows.Forms.Label lblCosto;
        private System.Windows.Forms.TextBox txtCosto;
        private System.Windows.Forms.Label lblDescCierre;
        private System.Windows.Forms.TextBox txtDescCierre;
        private System.Windows.Forms.Button btnCerrarMantenimiento;
        // Historial
        private System.Windows.Forms.Button btnVerHistorial;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitulo = new Label();
            tabControl = new TabControl();
            tabHerramientas = new TabPage();
            grpDatos = new GroupBox();
            lblCodigo = new Label();
            txtCodigo = new TextBox();
            lblNombre = new Label();
            txtNombre = new TextBox();
            lblCategoria = new Label();
            cboCategoria = new ComboBox();
            lblMarca = new Label();
            cboMarca = new ComboBox();
            lblUbicacion = new Label();
            cboUbicacion = new ComboBox();
            lblStockTotal = new Label();
            nudStockTotal = new NumericUpDown();
            lblCaracteristicas = new Label();
            txtCaracteristicas = new TextBox();
            btnNuevo = new Button();
            btnGuardar = new Button();
            btnEditar = new Button();
            btnCancelar = new Button();
            btnEliminar = new Button();
            lblBuscar = new Label();
            txtBuscar = new TextBox();
            dgvHerramientas = new DataGridView();
            tabCategorias = new TabPage();
            grpDatosCategoria = new GroupBox();
            lblNombreCategoria = new Label();
            txtNombreCategoria = new TextBox();
            lblDescCategoria = new Label();
            txtDescCategoria = new TextBox();
            btnNuevoCategoria = new Button();
            btnGuardarCategoria = new Button();
            btnEditarCategoria = new Button();
            btnCancelarCategoria = new Button();
            btnEliminarCategoria = new Button();
            dgvCategorias = new DataGridView();
            tabMarcas = new TabPage();
            grpDatosMarca = new GroupBox();
            lblNombreMarca = new Label();
            txtNombreMarca = new TextBox();
            lblDescMarca = new Label();
            txtDescMarca = new TextBox();
            btnNuevoMarca = new Button();
            btnGuardarMarca = new Button();
            btnEditarMarca = new Button();
            btnCancelarMarca = new Button();
            btnEliminarMarca = new Button();
            dgvMarcas = new DataGridView();
            tabMantenimiento = new TabPage();
            grpMantenimientosActivos = new GroupBox();
            dgvMantenimientos = new DataGridView();
            grpAccionMantenimiento = new GroupBox();
            lblHerramientaMant = new Label();
            cboHerramientaMant = new ComboBox();
            lblTipoMant = new Label();
            cboTipoMant = new ComboBox();
            lblRealizadoPor = new Label();
            txtRealizadoPor = new TextBox();
            lblDescMant = new Label();
            txtDescMant = new TextBox();
            btnAbrirMantenimiento = new Button();
            lblCosto = new Label();
            txtCosto = new TextBox();
            lblDescCierre = new Label();
            txtDescCierre = new TextBox();
            btnCerrarMantenimiento = new Button();
            btnVerHistorial = new Button();
            lblSecAbrir = new Label();
            divisor = new Label();
            lblSecCerrar = new Label();
            tabControl.SuspendLayout();
            tabHerramientas.SuspendLayout();
            grpDatos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudStockTotal).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvHerramientas).BeginInit();
            tabCategorias.SuspendLayout();
            grpDatosCategoria.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCategorias).BeginInit();
            tabMarcas.SuspendLayout();
            grpDatosMarca.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMarcas).BeginInit();
            tabMantenimiento.SuspendLayout();
            grpMantenimientosActivos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMantenimientos).BeginInit();
            grpAccionMantenimiento.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(1028, 55);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Gestión de Herramientas";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabHerramientas);
            tabControl.Controls.Add(tabCategorias);
            tabControl.Controls.Add(tabMarcas);
            tabControl.Controls.Add(tabMantenimiento);
            tabControl.Font = new Font("Segoe UI", 10F);
            tabControl.Location = new Point(21, 93);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(840, 628);
            tabControl.TabIndex = 1;
            // 
            // tabHerramientas
            // 
            tabHerramientas.Controls.Add(grpDatos);
            tabHerramientas.Controls.Add(lblBuscar);
            tabHerramientas.Controls.Add(txtBuscar);
            tabHerramientas.Controls.Add(dgvHerramientas);
            tabHerramientas.Location = new Point(4, 26);
            tabHerramientas.Name = "tabHerramientas";
            tabHerramientas.Padding = new Padding(8);
            tabHerramientas.Size = new Size(832, 598);
            tabHerramientas.TabIndex = 0;
            tabHerramientas.Text = "  Herramientas  ";
            // 
            // grpDatos
            // 
            grpDatos.Controls.Add(lblCodigo);
            grpDatos.Controls.Add(txtCodigo);
            grpDatos.Controls.Add(lblNombre);
            grpDatos.Controls.Add(txtNombre);
            grpDatos.Controls.Add(lblCategoria);
            grpDatos.Controls.Add(cboCategoria);
            grpDatos.Controls.Add(lblMarca);
            grpDatos.Controls.Add(cboMarca);
            grpDatos.Controls.Add(lblUbicacion);
            grpDatos.Controls.Add(cboUbicacion);
            grpDatos.Controls.Add(lblStockTotal);
            grpDatos.Controls.Add(nudStockTotal);
            grpDatos.Controls.Add(lblCaracteristicas);
            grpDatos.Controls.Add(txtCaracteristicas);
            grpDatos.Controls.Add(btnNuevo);
            grpDatos.Controls.Add(btnGuardar);
            grpDatos.Controls.Add(btnEditar);
            grpDatos.Controls.Add(btnCancelar);
            grpDatos.Controls.Add(btnEliminar);
            grpDatos.Location = new Point(8, 8);
            grpDatos.Name = "grpDatos";
            grpDatos.Size = new Size(810, 235);
            grpDatos.TabIndex = 0;
            grpDatos.TabStop = false;
            grpDatos.Text = "Datos de la Herramienta";
            // 
            // lblCodigo
            // 
            lblCodigo.Location = new Point(12, 30);
            lblCodigo.Name = "lblCodigo";
            lblCodigo.Size = new Size(55, 23);
            lblCodigo.TabIndex = 0;
            lblCodigo.Text = "Código:";
            // 
            // txtCodigo
            // 
            txtCodigo.Location = new Point(72, 27);
            txtCodigo.Name = "txtCodigo";
            txtCodigo.Size = new Size(130, 25);
            txtCodigo.TabIndex = 1;
            // 
            // lblNombre
            // 
            lblNombre.Location = new Point(220, 30);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(64, 23);
            lblNombre.TabIndex = 2;
            lblNombre.Text = "Nombre:";
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(290, 24);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(220, 25);
            txtNombre.TabIndex = 3;
            // 
            // lblCategoria
            // 
            lblCategoria.Location = new Point(12, 68);
            lblCategoria.Name = "lblCategoria";
            lblCategoria.Size = new Size(68, 23);
            lblCategoria.TabIndex = 4;
            lblCategoria.Text = "Categoría:";
            // 
            // cboCategoria
            // 
            cboCategoria.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCategoria.Location = new Point(82, 65);
            cboCategoria.Name = "cboCategoria";
            cboCategoria.Size = new Size(170, 25);
            cboCategoria.TabIndex = 5;
            // 
            // lblMarca
            // 
            lblMarca.Location = new Point(270, 68);
            lblMarca.Name = "lblMarca";
            lblMarca.Size = new Size(48, 23);
            lblMarca.TabIndex = 6;
            lblMarca.Text = "Marca:";
            // 
            // cboMarca
            // 
            cboMarca.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMarca.Location = new Point(322, 65);
            cboMarca.Name = "cboMarca";
            cboMarca.Size = new Size(170, 25);
            cboMarca.TabIndex = 7;
            // 
            // lblUbicacion
            // 
            lblUbicacion.Location = new Point(12, 106);
            lblUbicacion.Name = "lblUbicacion";
            lblUbicacion.Size = new Size(68, 23);
            lblUbicacion.TabIndex = 8;
            lblUbicacion.Text = "Ubicación:";
            // 
            // cboUbicacion
            // 
            cboUbicacion.DropDownStyle = ComboBoxStyle.DropDownList;
            cboUbicacion.Location = new Point(82, 103);
            cboUbicacion.Name = "cboUbicacion";
            cboUbicacion.Size = new Size(170, 25);
            cboUbicacion.TabIndex = 9;
            // 
            // lblStockTotal
            // 
            lblStockTotal.Location = new Point(270, 106);
            lblStockTotal.Name = "lblStockTotal";
            lblStockTotal.Size = new Size(48, 23);
            lblStockTotal.TabIndex = 10;
            lblStockTotal.Text = "Stock:";
            // 
            // nudStockTotal
            // 
            nudStockTotal.Location = new Point(322, 103);
            nudStockTotal.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            nudStockTotal.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudStockTotal.Name = "nudStockTotal";
            nudStockTotal.Size = new Size(80, 25);
            nudStockTotal.TabIndex = 11;
            nudStockTotal.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblCaracteristicas
            // 
            lblCaracteristicas.Location = new Point(12, 144);
            lblCaracteristicas.Name = "lblCaracteristicas";
            lblCaracteristicas.Size = new Size(90, 23);
            lblCaracteristicas.TabIndex = 12;
            lblCaracteristicas.Text = "Características:";
            // 
            // txtCaracteristicas
            // 
            txtCaracteristicas.Location = new Point(106, 141);
            txtCaracteristicas.Name = "txtCaracteristicas";
            txtCaracteristicas.Size = new Size(395, 25);
            txtCaracteristicas.TabIndex = 13;
            // 
            // btnNuevo
            // 
            btnNuevo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnNuevo.Location = new Point(12, 185);
            btnNuevo.Name = "btnNuevo";
            btnNuevo.Size = new Size(95, 34);
            btnNuevo.TabIndex = 14;
            btnNuevo.Text = "Nuevo";
            // 
            // btnGuardar
            // 
            btnGuardar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnGuardar.Location = new Point(112, 185);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(95, 34);
            btnGuardar.TabIndex = 15;
            btnGuardar.Text = "Guardar";
            // 
            // btnEditar
            // 
            btnEditar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnEditar.Location = new Point(212, 185);
            btnEditar.Name = "btnEditar";
            btnEditar.Size = new Size(95, 34);
            btnEditar.TabIndex = 16;
            btnEditar.Text = "Editar";
            // 
            // btnCancelar
            // 
            btnCancelar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnCancelar.Location = new Point(312, 185);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(95, 34);
            btnCancelar.TabIndex = 17;
            btnCancelar.Text = "Cancelar";
            // 
            // btnEliminar
            // 
            btnEliminar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnEliminar.Location = new Point(412, 185);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(100, 34);
            btnEliminar.TabIndex = 18;
            btnEliminar.Text = "Dar de Baja";
            // 
            // lblBuscar
            // 
            lblBuscar.Location = new Point(8, 252);
            lblBuscar.Name = "lblBuscar";
            lblBuscar.Size = new Size(55, 23);
            lblBuscar.TabIndex = 1;
            lblBuscar.Text = "Buscar:";
            // 
            // txtBuscar
            // 
            txtBuscar.Location = new Point(68, 249);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.PlaceholderText = "Nombre o código...";
            txtBuscar.Size = new Size(280, 25);
            txtBuscar.TabIndex = 2;
            // 
            // dgvHerramientas
            // 
            dgvHerramientas.AllowUserToAddRows = false;
            dgvHerramientas.AllowUserToDeleteRows = false;
            dgvHerramientas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHerramientas.Location = new Point(8, 280);
            dgvHerramientas.MultiSelect = false;
            dgvHerramientas.Name = "dgvHerramientas";
            dgvHerramientas.ReadOnly = true;
            dgvHerramientas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHerramientas.Size = new Size(812, 275);
            dgvHerramientas.TabIndex = 3;
            // 
            // tabCategorias
            // 
            tabCategorias.Controls.Add(grpDatosCategoria);
            tabCategorias.Controls.Add(dgvCategorias);
            tabCategorias.Location = new Point(4, 26);
            tabCategorias.Name = "tabCategorias";
            tabCategorias.Padding = new Padding(8);
            tabCategorias.Size = new Size(832, 598);
            tabCategorias.TabIndex = 1;
            tabCategorias.Text = "  Categorías  ";
            // 
            // grpDatosCategoria
            // 
            grpDatosCategoria.Controls.Add(lblNombreCategoria);
            grpDatosCategoria.Controls.Add(txtNombreCategoria);
            grpDatosCategoria.Controls.Add(lblDescCategoria);
            grpDatosCategoria.Controls.Add(txtDescCategoria);
            grpDatosCategoria.Controls.Add(btnNuevoCategoria);
            grpDatosCategoria.Controls.Add(btnGuardarCategoria);
            grpDatosCategoria.Controls.Add(btnEditarCategoria);
            grpDatosCategoria.Controls.Add(btnCancelarCategoria);
            grpDatosCategoria.Controls.Add(btnEliminarCategoria);
            grpDatosCategoria.Location = new Point(8, 8);
            grpDatosCategoria.Name = "grpDatosCategoria";
            grpDatosCategoria.Size = new Size(810, 130);
            grpDatosCategoria.TabIndex = 0;
            grpDatosCategoria.TabStop = false;
            grpDatosCategoria.Text = "Datos de la Categoría";
            // 
            // lblNombreCategoria
            // 
            lblNombreCategoria.Location = new Point(12, 30);
            lblNombreCategoria.Name = "lblNombreCategoria";
            lblNombreCategoria.Size = new Size(55, 23);
            lblNombreCategoria.TabIndex = 0;
            lblNombreCategoria.Text = "Nombre:";
            // 
            // txtNombreCategoria
            // 
            txtNombreCategoria.Location = new Point(72, 27);
            txtNombreCategoria.Name = "txtNombreCategoria";
            txtNombreCategoria.Size = new Size(200, 25);
            txtNombreCategoria.TabIndex = 1;
            // 
            // lblDescCategoria
            // 
            lblDescCategoria.Location = new Point(290, 30);
            lblDescCategoria.Name = "lblDescCategoria";
            lblDescCategoria.Size = new Size(80, 23);
            lblDescCategoria.TabIndex = 2;
            lblDescCategoria.Text = "Descripción:";
            // 
            // txtDescCategoria
            // 
            txtDescCategoria.Location = new Point(375, 27);
            txtDescCategoria.Name = "txtDescCategoria";
            txtDescCategoria.Size = new Size(300, 25);
            txtDescCategoria.TabIndex = 3;
            // 
            // btnNuevoCategoria
            // 
            btnNuevoCategoria.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnNuevoCategoria.Location = new Point(12, 75);
            btnNuevoCategoria.Name = "btnNuevoCategoria";
            btnNuevoCategoria.Size = new Size(90, 34);
            btnNuevoCategoria.TabIndex = 4;
            btnNuevoCategoria.Text = "Nuevo";
            // 
            // btnGuardarCategoria
            // 
            btnGuardarCategoria.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnGuardarCategoria.Location = new Point(107, 75);
            btnGuardarCategoria.Name = "btnGuardarCategoria";
            btnGuardarCategoria.Size = new Size(90, 34);
            btnGuardarCategoria.TabIndex = 5;
            btnGuardarCategoria.Text = "Guardar";
            // 
            // btnEditarCategoria
            // 
            btnEditarCategoria.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnEditarCategoria.Location = new Point(202, 75);
            btnEditarCategoria.Name = "btnEditarCategoria";
            btnEditarCategoria.Size = new Size(90, 34);
            btnEditarCategoria.TabIndex = 6;
            btnEditarCategoria.Text = "Editar";
            // 
            // btnCancelarCategoria
            // 
            btnCancelarCategoria.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnCancelarCategoria.Location = new Point(297, 75);
            btnCancelarCategoria.Name = "btnCancelarCategoria";
            btnCancelarCategoria.Size = new Size(90, 34);
            btnCancelarCategoria.TabIndex = 7;
            btnCancelarCategoria.Text = "Cancelar";
            // 
            // btnEliminarCategoria
            // 
            btnEliminarCategoria.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnEliminarCategoria.Location = new Point(392, 75);
            btnEliminarCategoria.Name = "btnEliminarCategoria";
            btnEliminarCategoria.Size = new Size(90, 34);
            btnEliminarCategoria.TabIndex = 8;
            btnEliminarCategoria.Text = "Eliminar";
            // 
            // dgvCategorias
            // 
            dgvCategorias.AllowUserToAddRows = false;
            dgvCategorias.AllowUserToDeleteRows = false;
            dgvCategorias.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCategorias.Location = new Point(8, 148);
            dgvCategorias.MultiSelect = false;
            dgvCategorias.Name = "dgvCategorias";
            dgvCategorias.ReadOnly = true;
            dgvCategorias.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCategorias.Size = new Size(812, 405);
            dgvCategorias.TabIndex = 1;
            // 
            // tabMarcas
            // 
            tabMarcas.Controls.Add(grpDatosMarca);
            tabMarcas.Controls.Add(dgvMarcas);
            tabMarcas.Location = new Point(4, 26);
            tabMarcas.Name = "tabMarcas";
            tabMarcas.Padding = new Padding(8);
            tabMarcas.Size = new Size(832, 598);
            tabMarcas.TabIndex = 2;
            tabMarcas.Text = "  Marcas  ";
            // 
            // grpDatosMarca
            // 
            grpDatosMarca.Controls.Add(lblNombreMarca);
            grpDatosMarca.Controls.Add(txtNombreMarca);
            grpDatosMarca.Controls.Add(lblDescMarca);
            grpDatosMarca.Controls.Add(txtDescMarca);
            grpDatosMarca.Controls.Add(btnNuevoMarca);
            grpDatosMarca.Controls.Add(btnGuardarMarca);
            grpDatosMarca.Controls.Add(btnEditarMarca);
            grpDatosMarca.Controls.Add(btnCancelarMarca);
            grpDatosMarca.Controls.Add(btnEliminarMarca);
            grpDatosMarca.Location = new Point(8, 8);
            grpDatosMarca.Name = "grpDatosMarca";
            grpDatosMarca.Size = new Size(810, 130);
            grpDatosMarca.TabIndex = 0;
            grpDatosMarca.TabStop = false;
            grpDatosMarca.Text = "Datos de la Marca";
            // 
            // lblNombreMarca
            // 
            lblNombreMarca.Location = new Point(12, 30);
            lblNombreMarca.Name = "lblNombreMarca";
            lblNombreMarca.Size = new Size(55, 23);
            lblNombreMarca.TabIndex = 0;
            lblNombreMarca.Text = "Nombre:";
            // 
            // txtNombreMarca
            // 
            txtNombreMarca.Location = new Point(72, 27);
            txtNombreMarca.Name = "txtNombreMarca";
            txtNombreMarca.Size = new Size(200, 25);
            txtNombreMarca.TabIndex = 1;
            // 
            // lblDescMarca
            // 
            lblDescMarca.Location = new Point(290, 30);
            lblDescMarca.Name = "lblDescMarca";
            lblDescMarca.Size = new Size(80, 23);
            lblDescMarca.TabIndex = 2;
            lblDescMarca.Text = "Descripción:";
            // 
            // txtDescMarca
            // 
            txtDescMarca.Location = new Point(375, 27);
            txtDescMarca.Name = "txtDescMarca";
            txtDescMarca.Size = new Size(300, 25);
            txtDescMarca.TabIndex = 3;
            // 
            // btnNuevoMarca
            // 
            btnNuevoMarca.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnNuevoMarca.Location = new Point(12, 75);
            btnNuevoMarca.Name = "btnNuevoMarca";
            btnNuevoMarca.Size = new Size(90, 34);
            btnNuevoMarca.TabIndex = 4;
            btnNuevoMarca.Text = "Nuevo";
            // 
            // btnGuardarMarca
            // 
            btnGuardarMarca.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnGuardarMarca.Location = new Point(107, 75);
            btnGuardarMarca.Name = "btnGuardarMarca";
            btnGuardarMarca.Size = new Size(90, 34);
            btnGuardarMarca.TabIndex = 5;
            btnGuardarMarca.Text = "Guardar";
            // 
            // btnEditarMarca
            // 
            btnEditarMarca.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnEditarMarca.Location = new Point(202, 75);
            btnEditarMarca.Name = "btnEditarMarca";
            btnEditarMarca.Size = new Size(90, 34);
            btnEditarMarca.TabIndex = 6;
            btnEditarMarca.Text = "Editar";
            // 
            // btnCancelarMarca
            // 
            btnCancelarMarca.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnCancelarMarca.Location = new Point(297, 75);
            btnCancelarMarca.Name = "btnCancelarMarca";
            btnCancelarMarca.Size = new Size(90, 34);
            btnCancelarMarca.TabIndex = 7;
            btnCancelarMarca.Text = "Cancelar";
            // 
            // btnEliminarMarca
            // 
            btnEliminarMarca.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnEliminarMarca.Location = new Point(392, 75);
            btnEliminarMarca.Name = "btnEliminarMarca";
            btnEliminarMarca.Size = new Size(90, 34);
            btnEliminarMarca.TabIndex = 8;
            btnEliminarMarca.Text = "Eliminar";
            // 
            // dgvMarcas
            // 
            dgvMarcas.AllowUserToAddRows = false;
            dgvMarcas.AllowUserToDeleteRows = false;
            dgvMarcas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMarcas.Location = new Point(8, 148);
            dgvMarcas.MultiSelect = false;
            dgvMarcas.Name = "dgvMarcas";
            dgvMarcas.ReadOnly = true;
            dgvMarcas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMarcas.Size = new Size(812, 405);
            dgvMarcas.TabIndex = 1;
            // 
            // tabMantenimiento
            // 
            tabMantenimiento.Controls.Add(grpMantenimientosActivos);
            tabMantenimiento.Controls.Add(grpAccionMantenimiento);
            tabMantenimiento.Location = new Point(4, 26);
            tabMantenimiento.Name = "tabMantenimiento";
            tabMantenimiento.Padding = new Padding(8);
            tabMantenimiento.Size = new Size(832, 598);
            tabMantenimiento.TabIndex = 3;
            tabMantenimiento.Text = "  Mantenimiento  ";
            // 
            // grpMantenimientosActivos
            // 
            grpMantenimientosActivos.Controls.Add(dgvMantenimientos);
            grpMantenimientosActivos.Location = new Point(8, 8);
            grpMantenimientosActivos.Name = "grpMantenimientosActivos";
            grpMantenimientosActivos.Size = new Size(812, 270);
            grpMantenimientosActivos.TabIndex = 0;
            grpMantenimientosActivos.TabStop = false;
            grpMantenimientosActivos.Text = "Mantenimientos en curso — selecciona uno para cerrarlo";
            // 
            // dgvMantenimientos
            // 
            dgvMantenimientos.AllowUserToAddRows = false;
            dgvMantenimientos.AllowUserToDeleteRows = false;
            dgvMantenimientos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMantenimientos.Location = new Point(10, 22);
            dgvMantenimientos.MultiSelect = false;
            dgvMantenimientos.Name = "dgvMantenimientos";
            dgvMantenimientos.ReadOnly = true;
            dgvMantenimientos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMantenimientos.Size = new Size(792, 238);
            dgvMantenimientos.TabIndex = 0;
            // 
            // grpAccionMantenimiento
            // 
            grpAccionMantenimiento.Controls.Add(lblHerramientaMant);
            grpAccionMantenimiento.Controls.Add(cboHerramientaMant);
            grpAccionMantenimiento.Controls.Add(lblTipoMant);
            grpAccionMantenimiento.Controls.Add(cboTipoMant);
            grpAccionMantenimiento.Controls.Add(lblRealizadoPor);
            grpAccionMantenimiento.Controls.Add(txtRealizadoPor);
            grpAccionMantenimiento.Controls.Add(lblDescMant);
            grpAccionMantenimiento.Controls.Add(txtDescMant);
            grpAccionMantenimiento.Controls.Add(btnAbrirMantenimiento);
            grpAccionMantenimiento.Controls.Add(lblCosto);
            grpAccionMantenimiento.Controls.Add(txtCosto);
            grpAccionMantenimiento.Controls.Add(lblDescCierre);
            grpAccionMantenimiento.Controls.Add(txtDescCierre);
            grpAccionMantenimiento.Controls.Add(btnCerrarMantenimiento);
            grpAccionMantenimiento.Controls.Add(btnVerHistorial);
            grpAccionMantenimiento.Controls.Add(lblSecAbrir);
            grpAccionMantenimiento.Controls.Add(divisor);
            grpAccionMantenimiento.Controls.Add(lblSecCerrar);
            grpAccionMantenimiento.Location = new Point(8, 290);
            grpAccionMantenimiento.Name = "grpAccionMantenimiento";
            grpAccionMantenimiento.Size = new Size(812, 270);
            grpAccionMantenimiento.TabIndex = 1;
            grpAccionMantenimiento.TabStop = false;
            grpAccionMantenimiento.Text = "Acciones";
            // 
            // lblHerramientaMant
            // 
            lblHerramientaMant.Location = new Point(10, 50);
            lblHerramientaMant.Name = "lblHerramientaMant";
            lblHerramientaMant.Size = new Size(80, 23);
            lblHerramientaMant.TabIndex = 0;
            lblHerramientaMant.Text = "Herramienta:";
            // 
            // cboHerramientaMant
            // 
            cboHerramientaMant.DropDownStyle = ComboBoxStyle.DropDownList;
            cboHerramientaMant.Location = new Point(95, 47);
            cboHerramientaMant.Name = "cboHerramientaMant";
            cboHerramientaMant.Size = new Size(230, 25);
            cboHerramientaMant.TabIndex = 1;
            // 
            // lblTipoMant
            // 
            lblTipoMant.Location = new Point(10, 85);
            lblTipoMant.Name = "lblTipoMant";
            lblTipoMant.Size = new Size(80, 23);
            lblTipoMant.TabIndex = 2;
            lblTipoMant.Text = "Tipo:";
            // 
            // cboTipoMant
            // 
            cboTipoMant.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTipoMant.Items.AddRange(new object[] { "Preventivo", "Correctivo" });
            cboTipoMant.Location = new Point(95, 82);
            cboTipoMant.Name = "cboTipoMant";
            cboTipoMant.Size = new Size(150, 25);
            cboTipoMant.TabIndex = 3;
            // 
            // lblRealizadoPor
            // 
            lblRealizadoPor.Location = new Point(10, 120);
            lblRealizadoPor.Name = "lblRealizadoPor";
            lblRealizadoPor.Size = new Size(80, 23);
            lblRealizadoPor.TabIndex = 4;
            lblRealizadoPor.Text = "Realizado por:";
            // 
            // txtRealizadoPor
            // 
            txtRealizadoPor.Location = new Point(95, 117);
            txtRealizadoPor.Name = "txtRealizadoPor";
            txtRealizadoPor.Size = new Size(230, 25);
            txtRealizadoPor.TabIndex = 5;
            // 
            // lblDescMant
            // 
            lblDescMant.Location = new Point(10, 155);
            lblDescMant.Name = "lblDescMant";
            lblDescMant.Size = new Size(80, 23);
            lblDescMant.TabIndex = 6;
            lblDescMant.Text = "Descripción:";
            // 
            // txtDescMant
            // 
            txtDescMant.Location = new Point(95, 152);
            txtDescMant.Name = "txtDescMant";
            txtDescMant.Size = new Size(230, 25);
            txtDescMant.TabIndex = 7;
            // 
            // btnAbrirMantenimiento
            // 
            btnAbrirMantenimiento.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnAbrirMantenimiento.Location = new Point(10, 195);
            btnAbrirMantenimiento.Name = "btnAbrirMantenimiento";
            btnAbrirMantenimiento.Size = new Size(200, 38);
            btnAbrirMantenimiento.TabIndex = 8;
            btnAbrirMantenimiento.Text = "Abrir mantenimiento";
            // 
            // lblCosto
            // 
            lblCosto.Location = new Point(375, 50);
            lblCosto.Name = "lblCosto";
            lblCosto.Size = new Size(80, 23);
            lblCosto.TabIndex = 9;
            lblCosto.Text = "Costo (L):";
            // 
            // txtCosto
            // 
            txtCosto.Location = new Point(460, 47);
            txtCosto.Name = "txtCosto";
            txtCosto.Size = new Size(130, 25);
            txtCosto.TabIndex = 10;
            // 
            // lblDescCierre
            // 
            lblDescCierre.Location = new Point(375, 85);
            lblDescCierre.Name = "lblDescCierre";
            lblDescCierre.Size = new Size(80, 23);
            lblDescCierre.TabIndex = 11;
            lblDescCierre.Text = "Notas cierre:";
            // 
            // txtDescCierre
            // 
            txtDescCierre.Location = new Point(460, 82);
            txtDescCierre.Name = "txtDescCierre";
            txtDescCierre.Size = new Size(320, 25);
            txtDescCierre.TabIndex = 12;
            // 
            // btnCerrarMantenimiento
            // 
            btnCerrarMantenimiento.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnCerrarMantenimiento.Location = new Point(375, 125);
            btnCerrarMantenimiento.Name = "btnCerrarMantenimiento";
            btnCerrarMantenimiento.Size = new Size(200, 38);
            btnCerrarMantenimiento.TabIndex = 13;
            btnCerrarMantenimiento.Text = "Cerrar mantenimiento";
            // 
            // btnVerHistorial
            // 
            btnVerHistorial.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnVerHistorial.Location = new Point(375, 180);
            btnVerHistorial.Name = "btnVerHistorial";
            btnVerHistorial.Size = new Size(200, 38);
            btnVerHistorial.TabIndex = 14;
            btnVerHistorial.Text = "Ver historial de herramienta";
            // 
            // lblSecAbrir
            // 
            lblSecAbrir.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSecAbrir.Location = new Point(10, 22);
            lblSecAbrir.Name = "lblSecAbrir";
            lblSecAbrir.Size = new Size(380, 20);
            lblSecAbrir.TabIndex = 15;
            lblSecAbrir.Text = "▶  Abrir nuevo mantenimiento";
            // 
            // divisor
            // 
            divisor.BorderStyle = BorderStyle.Fixed3D;
            divisor.Location = new Point(360, 22);
            divisor.Name = "divisor";
            divisor.Size = new Size(2, 235);
            divisor.TabIndex = 16;
            // 
            // lblSecCerrar
            // 
            lblSecCerrar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSecCerrar.Location = new Point(375, 22);
            lblSecCerrar.Name = "lblSecCerrar";
            lblSecCerrar.Size = new Size(380, 20);
            lblSecCerrar.TabIndex = 17;
            lblSecCerrar.Text = "▶  Cerrar mantenimiento seleccionado";
            // 
            // FrmHerramientas
            // 
            ClientSize = new Size(1028, 742);
            Controls.Add(lblTitulo);
            Controls.Add(tabControl);
            Name = "FrmHerramientas";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Herramientas";
            tabControl.ResumeLayout(false);
            tabHerramientas.ResumeLayout(false);
            tabHerramientas.PerformLayout();
            grpDatos.ResumeLayout(false);
            grpDatos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudStockTotal).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvHerramientas).EndInit();
            tabCategorias.ResumeLayout(false);
            grpDatosCategoria.ResumeLayout(false);
            grpDatosCategoria.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCategorias).EndInit();
            tabMarcas.ResumeLayout(false);
            grpDatosMarca.ResumeLayout(false);
            grpDatosMarca.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMarcas).EndInit();
            tabMantenimiento.ResumeLayout(false);
            grpMantenimientosActivos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvMantenimientos).EndInit();
            grpAccionMantenimiento.ResumeLayout(false);
            grpAccionMantenimiento.PerformLayout();
            ResumeLayout(false);
        }

        private Label lblSecAbrir;
        private Label divisor;
        private Label lblSecCerrar;
    }
}