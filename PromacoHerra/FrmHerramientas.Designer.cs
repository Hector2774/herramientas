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
        private PromacoHerra.Controls.MaterialTextBox txtCodigo;
        private System.Windows.Forms.Label lblNombre;
        private PromacoHerra.Controls.MaterialTextBox txtNombre;
        private System.Windows.Forms.Label lblCategoria;
        private PromacoHerra.Controls.MaterialComboBox cboCategoria;
        private System.Windows.Forms.Label lblMarca;
        private PromacoHerra.Controls.MaterialComboBox cboMarca;
        private System.Windows.Forms.Label lblUbicacion;
        private PromacoHerra.Controls.MaterialComboBox cboUbicacion;
        private System.Windows.Forms.Label lblStockTotal;
        private System.Windows.Forms.NumericUpDown nudStockTotal;
        private System.Windows.Forms.Label lblCaracteristicas;
        private PromacoHerra.Controls.MaterialTextBox txtCaracteristicas;
        private PromacoHerra.Controls.MaterialButton btnNuevo;
        private PromacoHerra.Controls.MaterialButton btnGuardar;
        private PromacoHerra.Controls.MaterialButton btnEditar;
        private PromacoHerra.Controls.MaterialButton btnCancelar;
        private PromacoHerra.Controls.MaterialButton btnEliminar;
        private System.Windows.Forms.Label lblBuscar;
        private PromacoHerra.Controls.MaterialTextBox txtBuscar;

        // ── TAB CATEGORÍAS ────────────────────────────────────────
        private System.Windows.Forms.GroupBox grpDatosCategoria;
        private System.Windows.Forms.Label lblNombreCategoria;
        private PromacoHerra.Controls.MaterialTextBox txtNombreCategoria;
        private System.Windows.Forms.Label lblDescCategoria;
        private PromacoHerra.Controls.MaterialTextBox txtDescCategoria;
        private PromacoHerra.Controls.MaterialButton btnNuevoCategoria;
        private PromacoHerra.Controls.MaterialButton btnGuardarCategoria;
        private PromacoHerra.Controls.MaterialButton btnEditarCategoria;
        private PromacoHerra.Controls.MaterialButton btnCancelarCategoria;
        private PromacoHerra.Controls.MaterialButton btnEliminarCategoria;
        private System.Windows.Forms.DataGridView dgvCategorias;

        // ── TAB MARCAS ────────────────────────────────────────────
        private System.Windows.Forms.GroupBox grpDatosMarca;
        private System.Windows.Forms.Label lblNombreMarca;
        private PromacoHerra.Controls.MaterialTextBox txtNombreMarca;
        private System.Windows.Forms.Label lblDescMarca;
        private PromacoHerra.Controls.MaterialTextBox txtDescMarca;
        private PromacoHerra.Controls.MaterialButton btnNuevoMarca;
        private PromacoHerra.Controls.MaterialButton btnGuardarMarca;
        private PromacoHerra.Controls.MaterialButton btnEditarMarca;
        private PromacoHerra.Controls.MaterialButton btnCancelarMarca;
        private PromacoHerra.Controls.MaterialButton btnEliminarMarca;
        private System.Windows.Forms.DataGridView dgvMarcas;

        // ── TAB MANTENIMIENTO ─────────────────────────────────────
        // Grid superior — mantenimientos activos
        private System.Windows.Forms.GroupBox grpMantenimientosActivos;
        private System.Windows.Forms.DataGridView dgvMantenimientos;
        // Panel inferior — acciones
        private System.Windows.Forms.GroupBox grpAccionMantenimiento;
        // Abrir mantenimiento
        private System.Windows.Forms.Label lblHerramientaMant;
        private PromacoHerra.Controls.MaterialComboBox cboHerramientaMant;
        private System.Windows.Forms.Label lblTipoMant;
        private PromacoHerra.Controls.MaterialComboBox cboTipoMant;
        private System.Windows.Forms.Label lblRealizadoPor;
        private PromacoHerra.Controls.MaterialTextBox txtRealizadoPor;
        private System.Windows.Forms.Label lblDescMant;
        private PromacoHerra.Controls.MaterialTextBox txtDescMant;
        private PromacoHerra.Controls.MaterialButton btnAbrirMantenimiento;
        // Cerrar mantenimiento
        private System.Windows.Forms.Label lblCosto;
        private PromacoHerra.Controls.MaterialTextBox txtCosto;
        private System.Windows.Forms.Label lblDescCierre;
        private PromacoHerra.Controls.MaterialTextBox txtDescCierre;
        private PromacoHerra.Controls.MaterialButton btnCerrarMantenimiento;
        // Historial
        private PromacoHerra.Controls.MaterialButton btnVerHistorial;

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
            dgvHerramientas = new DataGridView();
            grpDatos = new GroupBox();
            lblCodigo = new Label();
            txtCodigo = new PromacoHerra.Controls.MaterialTextBox();
            lblNombre = new Label();
            txtNombre = new PromacoHerra.Controls.MaterialTextBox();
            lblCategoria = new Label();
            cboCategoria = new PromacoHerra.Controls.MaterialComboBox();
            lblMarca = new Label();
            cboMarca = new PromacoHerra.Controls.MaterialComboBox();
            lblUbicacion = new Label();
            cboUbicacion = new PromacoHerra.Controls.MaterialComboBox();
            lblStockTotal = new Label();
            nudStockTotal = new NumericUpDown();
            lblCaracteristicas = new Label();
            txtCaracteristicas = new PromacoHerra.Controls.MaterialTextBox();
            btnNuevo = new PromacoHerra.Controls.MaterialButton();
            btnGuardar = new PromacoHerra.Controls.MaterialButton();
            btnEditar = new PromacoHerra.Controls.MaterialButton();
            btnCancelar = new PromacoHerra.Controls.MaterialButton();
            btnEliminar = new PromacoHerra.Controls.MaterialButton();
            lblBuscar = new Label();
            txtBuscar = new PromacoHerra.Controls.MaterialTextBox();
            tabCategorias = new TabPage();
            grpDatosCategoria = new GroupBox();
            lblNombreCategoria = new Label();
            txtNombreCategoria = new PromacoHerra.Controls.MaterialTextBox();
            lblDescCategoria = new Label();
            txtDescCategoria = new PromacoHerra.Controls.MaterialTextBox();
            btnNuevoCategoria = new PromacoHerra.Controls.MaterialButton();
            btnGuardarCategoria = new PromacoHerra.Controls.MaterialButton();
            btnEditarCategoria = new PromacoHerra.Controls.MaterialButton();
            btnCancelarCategoria = new PromacoHerra.Controls.MaterialButton();
            btnEliminarCategoria = new PromacoHerra.Controls.MaterialButton();
            dgvCategorias = new DataGridView();
            tabMarcas = new TabPage();
            grpDatosMarca = new GroupBox();
            lblNombreMarca = new Label();
            txtNombreMarca = new PromacoHerra.Controls.MaterialTextBox();
            lblDescMarca = new Label();
            txtDescMarca = new PromacoHerra.Controls.MaterialTextBox();
            btnNuevoMarca = new PromacoHerra.Controls.MaterialButton();
            btnGuardarMarca = new PromacoHerra.Controls.MaterialButton();
            btnEditarMarca = new PromacoHerra.Controls.MaterialButton();
            btnCancelarMarca = new PromacoHerra.Controls.MaterialButton();
            btnEliminarMarca = new PromacoHerra.Controls.MaterialButton();
            dgvMarcas = new DataGridView();
            tabMantenimiento = new TabPage();
            grpMantenimientosActivos = new GroupBox();
            dgvMantenimientos = new DataGridView();
            grpAccionMantenimiento = new GroupBox();
            lblHerramientaMant = new Label();
            cboHerramientaMant = new PromacoHerra.Controls.MaterialComboBox();
            lblTipoMant = new Label();
            cboTipoMant = new PromacoHerra.Controls.MaterialComboBox();
            lblRealizadoPor = new Label();
            txtRealizadoPor = new PromacoHerra.Controls.MaterialTextBox();
            lblDescMant = new Label();
            txtDescMant = new PromacoHerra.Controls.MaterialTextBox();
            btnAbrirMantenimiento = new PromacoHerra.Controls.MaterialButton();
            lblCosto = new Label();
            txtCosto = new PromacoHerra.Controls.MaterialTextBox();
            lblDescCierre = new Label();
            txtDescCierre = new PromacoHerra.Controls.MaterialTextBox();
            btnCerrarMantenimiento = new PromacoHerra.Controls.MaterialButton();
            btnVerHistorial = new PromacoHerra.Controls.MaterialButton();
            lblSecAbrir = new Label();
            divisor = new Label();
            lblSecCerrar = new Label();
            tabControl.SuspendLayout();
            tabHerramientas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvHerramientas).BeginInit();
            grpDatos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudStockTotal).BeginInit();
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
            lblTitulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(1269, 55);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Gestión de Herramientas";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tabControl
            // 
            tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl.Controls.Add(tabHerramientas);
            tabControl.Controls.Add(tabCategorias);
            tabControl.Controls.Add(tabMarcas);
            tabControl.Controls.Add(tabMantenimiento);
            tabControl.Font = new Font("Segoe UI", 11.5F);
            tabControl.Location = new Point(12, 58);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(1227, 916);
            tabControl.TabIndex = 1;
            // 
            // tabHerramientas
            // 
            tabHerramientas.Controls.Add(dgvHerramientas);
            tabHerramientas.Controls.Add(grpDatos);
            tabHerramientas.Controls.Add(lblBuscar);
            tabHerramientas.Controls.Add(txtBuscar);
            tabHerramientas.Location = new Point(4, 29);
            tabHerramientas.Name = "tabHerramientas";
            tabHerramientas.Padding = new Padding(8);
            tabHerramientas.Size = new Size(1219, 883);
            tabHerramientas.TabIndex = 0;
            tabHerramientas.Text = "  Herramientas  ";
            // 
            // dgvHerramientas
            // 
            dgvHerramientas.AllowUserToAddRows = false;
            dgvHerramientas.AllowUserToDeleteRows = false;
            dgvHerramientas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvHerramientas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHerramientas.Location = new Point(8, 388);
            dgvHerramientas.MultiSelect = false;
            dgvHerramientas.Name = "dgvHerramientas";
            dgvHerramientas.ReadOnly = true;
            dgvHerramientas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHerramientas.Size = new Size(1169, 484);
            dgvHerramientas.TabIndex = 4;
            // 
            // grpDatos
            // 
            grpDatos.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
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
            grpDatos.Size = new Size(1197, 324);
            grpDatos.TabIndex = 0;
            grpDatos.TabStop = false;
            grpDatos.Text = "Datos de la Herramienta";
            // 
            // lblCodigo
            // 
            lblCodigo.Location = new Point(17, 56);
            lblCodigo.Name = "lblCodigo";
            lblCodigo.Size = new Size(78, 23);
            lblCodigo.TabIndex = 0;
            lblCodigo.Text = "Código:";
            // 
            // txtCodigo
            // 
            txtCodigo.BackColor = Color.White;
            txtCodigo.FloatingLabelText = "";
            txtCodigo.Location = new Point(145, 56);
            txtCodigo.MaxLength = 32767;
            txtCodigo.MinimumSize = new Size(0, 38);
            txtCodigo.Multiline = false;
            txtCodigo.Name = "txtCodigo";
            txtCodigo.Padding = new Padding(0, 0, 0, 4);
            txtCodigo.PasswordChar = '\0';
            txtCodigo.PlaceholderText = "";
            txtCodigo.ReadOnly = false;
            txtCodigo.SelectionStart = 0;
            txtCodigo.Size = new Size(130, 38);
            txtCodigo.TabIndex = 1;
            txtCodigo.UseFloatingLabel = false;
            // 
            // lblNombre
            // 
            lblNombre.Location = new Point(407, 56);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(76, 23);
            lblNombre.TabIndex = 2;
            lblNombre.Text = "Nombre:";
            // 
            // txtNombre
            // 
            txtNombre.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtNombre.BackColor = Color.White;
            txtNombre.FloatingLabelText = "";
            txtNombre.Location = new Point(489, 41);
            txtNombre.MaxLength = 32767;
            txtNombre.MinimumSize = new Size(0, 38);
            txtNombre.Multiline = false;
            txtNombre.Name = "txtNombre";
            txtNombre.Padding = new Padding(0, 0, 0, 4);
            txtNombre.PasswordChar = '\0';
            txtNombre.PlaceholderText = "";
            txtNombre.ReadOnly = false;
            txtNombre.SelectionStart = 0;
            txtNombre.Size = new Size(607, 38);
            txtNombre.TabIndex = 3;
            txtNombre.UseFloatingLabel = false;
            // 
            // lblCategoria
            // 
            lblCategoria.Location = new Point(17, 121);
            lblCategoria.Name = "lblCategoria";
            lblCategoria.Size = new Size(90, 23);
            lblCategoria.TabIndex = 4;
            lblCategoria.Text = "Categoría:";
            // 
            // cboCategoria
            // 
            cboCategoria.BackColor = Color.White;
            cboCategoria.DataSource = null;
            cboCategoria.DisplayMember = "";
            cboCategoria.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCategoria.Location = new Point(145, 106);
            cboCategoria.MinimumSize = new Size(0, 38);
            cboCategoria.Name = "cboCategoria";
            cboCategoria.Padding = new Padding(0, 0, 0, 4);
            cboCategoria.SelectedIndex = -1;
            cboCategoria.SelectedItem = null;
            cboCategoria.SelectedValue = null;
            cboCategoria.Size = new Size(170, 38);
            cboCategoria.TabIndex = 5;
            cboCategoria.ValueMember = "";
            // 
            // lblMarca
            // 
            lblMarca.Location = new Point(407, 121);
            lblMarca.Name = "lblMarca";
            lblMarca.Size = new Size(57, 23);
            lblMarca.TabIndex = 6;
            lblMarca.Text = "Marca:";
            // 
            // cboMarca
            // 
            cboMarca.BackColor = Color.White;
            cboMarca.DataSource = null;
            cboMarca.DisplayMember = "";
            cboMarca.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMarca.Location = new Point(489, 106);
            cboMarca.MinimumSize = new Size(0, 38);
            cboMarca.Name = "cboMarca";
            cboMarca.Padding = new Padding(0, 0, 0, 4);
            cboMarca.SelectedIndex = -1;
            cboMarca.SelectedItem = null;
            cboMarca.SelectedValue = null;
            cboMarca.Size = new Size(170, 38);
            cboMarca.TabIndex = 7;
            cboMarca.ValueMember = "";
            // 
            // lblUbicacion
            // 
            lblUbicacion.Location = new Point(17, 175);
            lblUbicacion.Name = "lblUbicacion";
            lblUbicacion.Size = new Size(90, 23);
            lblUbicacion.TabIndex = 8;
            lblUbicacion.Text = "Ubicación:";
            // 
            // cboUbicacion
            // 
            cboUbicacion.BackColor = Color.White;
            cboUbicacion.DataSource = null;
            cboUbicacion.DisplayMember = "";
            cboUbicacion.DropDownStyle = ComboBoxStyle.DropDownList;
            cboUbicacion.Location = new Point(145, 160);
            cboUbicacion.MinimumSize = new Size(0, 38);
            cboUbicacion.Name = "cboUbicacion";
            cboUbicacion.Padding = new Padding(0, 0, 0, 4);
            cboUbicacion.SelectedIndex = -1;
            cboUbicacion.SelectedItem = null;
            cboUbicacion.SelectedValue = null;
            cboUbicacion.Size = new Size(170, 38);
            cboUbicacion.TabIndex = 9;
            cboUbicacion.ValueMember = "";
            // 
            // lblStockTotal
            // 
            lblStockTotal.Location = new Point(407, 160);
            lblStockTotal.Name = "lblStockTotal";
            lblStockTotal.Size = new Size(48, 23);
            lblStockTotal.TabIndex = 10;
            lblStockTotal.Text = "Stock:";
            // 
            // nudStockTotal
            // 
            nudStockTotal.Location = new Point(489, 155);
            nudStockTotal.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            nudStockTotal.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudStockTotal.Name = "nudStockTotal";
            nudStockTotal.Size = new Size(80, 28);
            nudStockTotal.TabIndex = 11;
            nudStockTotal.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblCaracteristicas
            // 
            lblCaracteristicas.Location = new Point(17, 228);
            lblCaracteristicas.Name = "lblCaracteristicas";
            lblCaracteristicas.Size = new Size(122, 23);
            lblCaracteristicas.TabIndex = 12;
            lblCaracteristicas.Text = "Características:";
            // 
            // txtCaracteristicas
            // 
            txtCaracteristicas.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtCaracteristicas.BackColor = Color.White;
            txtCaracteristicas.FloatingLabelText = "";
            txtCaracteristicas.Location = new Point(145, 213);
            txtCaracteristicas.MaxLength = 32767;
            txtCaracteristicas.MinimumSize = new Size(0, 38);
            txtCaracteristicas.Multiline = false;
            txtCaracteristicas.Name = "txtCaracteristicas";
            txtCaracteristicas.Padding = new Padding(0, 0, 0, 4);
            txtCaracteristicas.PasswordChar = '\0';
            txtCaracteristicas.PlaceholderText = "";
            txtCaracteristicas.ReadOnly = false;
            txtCaracteristicas.SelectionStart = 0;
            txtCaracteristicas.Size = new Size(782, 38);
            txtCaracteristicas.TabIndex = 13;
            txtCaracteristicas.UseFloatingLabel = false;
            // 
            // btnNuevo
            // 
            btnNuevo.CornerRadius = 8;
            btnNuevo.FlatStyle = FlatStyle.Flat;
            btnNuevo.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnNuevo.Icon = FontAwesome.Sharp.IconChar.Add;
            btnNuevo.IconColor = null;
            btnNuevo.IconSize = 20;
            btnNuevo.Location = new Point(7, 277);
            btnNuevo.Name = "btnNuevo";
            btnNuevo.Size = new Size(111, 34);
            btnNuevo.TabIndex = 14;
            btnNuevo.Text = "Nuevo";
            btnNuevo.Variant = PromacoHerra.Controls.MaterialButtonVariant.Default;
            // 
            // btnGuardar
            // 
            btnGuardar.CornerRadius = 8;
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnGuardar.Icon = FontAwesome.Sharp.IconChar.Save;
            btnGuardar.IconColor = null;
            btnGuardar.IconSize = 20;
            btnGuardar.Location = new Point(124, 277);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(126, 34);
            btnGuardar.TabIndex = 15;
            btnGuardar.Text = "Guardar";
            btnGuardar.Variant = PromacoHerra.Controls.MaterialButtonVariant.Default;
            // 
            // btnEditar
            // 
            btnEditar.CornerRadius = 8;
            btnEditar.FlatStyle = FlatStyle.Flat;
            btnEditar.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnEditar.Icon = FontAwesome.Sharp.IconChar.Edit;
            btnEditar.IconColor = null;
            btnEditar.IconSize = 20;
            btnEditar.Location = new Point(256, 277);
            btnEditar.Name = "btnEditar";
            btnEditar.Size = new Size(106, 34);
            btnEditar.TabIndex = 16;
            btnEditar.Text = "Editar";
            btnEditar.Variant = PromacoHerra.Controls.MaterialButtonVariant.Default;
            // 
            // btnCancelar
            // 
            btnCancelar.CornerRadius = 8;
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnCancelar.Icon = FontAwesome.Sharp.IconChar.Close;
            btnCancelar.IconColor = null;
            btnCancelar.IconSize = 20;
            btnCancelar.Location = new Point(369, 277);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(114, 34);
            btnCancelar.TabIndex = 17;
            btnCancelar.Text = "Cancelar";
            btnCancelar.Variant = PromacoHerra.Controls.MaterialButtonVariant.Default;
            // 
            // btnEliminar
            // 
            btnEliminar.CornerRadius = 8;
            btnEliminar.FlatStyle = FlatStyle.Flat;
            btnEliminar.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnEliminar.Icon = FontAwesome.Sharp.IconChar.Trash;
            btnEliminar.IconColor = null;
            btnEliminar.IconSize = 20;
            btnEliminar.Location = new Point(489, 277);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(150, 34);
            btnEliminar.TabIndex = 18;
            btnEliminar.Text = "Dar de Baja";
            btnEliminar.Variant = PromacoHerra.Controls.MaterialButtonVariant.Default;
            // 
            // lblBuscar
            // 
            lblBuscar.Location = new Point(5, 339);
            lblBuscar.Name = "lblBuscar";
            lblBuscar.Size = new Size(66, 23);
            lblBuscar.TabIndex = 1;
            lblBuscar.Text = "Buscar:";
            // 
            // txtBuscar
            // 
            txtBuscar.BackColor = Color.White;
            txtBuscar.FloatingLabelText = "";
            txtBuscar.Location = new Point(77, 336);
            txtBuscar.MaxLength = 32767;
            txtBuscar.MinimumSize = new Size(0, 38);
            txtBuscar.Multiline = false;
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Padding = new Padding(0, 0, 0, 4);
            txtBuscar.PasswordChar = '\0';
            txtBuscar.PlaceholderText = "Nombre o código...";
            txtBuscar.ReadOnly = false;
            txtBuscar.SelectionStart = 0;
            txtBuscar.Size = new Size(280, 38);
            txtBuscar.TabIndex = 2;
            txtBuscar.UseFloatingLabel = false;
            // 
            // tabCategorias
            // 
            tabCategorias.Controls.Add(grpDatosCategoria);
            tabCategorias.Controls.Add(dgvCategorias);
            tabCategorias.Location = new Point(4, 29);
            tabCategorias.Name = "tabCategorias";
            tabCategorias.Padding = new Padding(8);
            tabCategorias.Size = new Size(1219, 883);
            tabCategorias.TabIndex = 1;
            tabCategorias.Text = "  Categorías  ";
            // 
            // grpDatosCategoria
            // 
            grpDatosCategoria.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
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
            grpDatosCategoria.Size = new Size(1197, 155);
            grpDatosCategoria.TabIndex = 0;
            grpDatosCategoria.TabStop = false;
            grpDatosCategoria.Text = "Datos de la Categoría";
            // 
            // lblNombreCategoria
            // 
            lblNombreCategoria.Location = new Point(12, 30);
            lblNombreCategoria.Name = "lblNombreCategoria";
            lblNombreCategoria.Size = new Size(77, 23);
            lblNombreCategoria.TabIndex = 0;
            lblNombreCategoria.Text = "Nombre:";
            // 
            // txtNombreCategoria
            // 
            txtNombreCategoria.BackColor = Color.White;
            txtNombreCategoria.FloatingLabelText = "";
            txtNombreCategoria.Location = new Point(95, 27);
            txtNombreCategoria.MaxLength = 32767;
            txtNombreCategoria.MinimumSize = new Size(0, 38);
            txtNombreCategoria.Multiline = false;
            txtNombreCategoria.Name = "txtNombreCategoria";
            txtNombreCategoria.Padding = new Padding(0, 0, 0, 4);
            txtNombreCategoria.PasswordChar = '\0';
            txtNombreCategoria.PlaceholderText = "";
            txtNombreCategoria.ReadOnly = false;
            txtNombreCategoria.SelectionStart = 0;
            txtNombreCategoria.Size = new Size(200, 38);
            txtNombreCategoria.TabIndex = 1;
            txtNombreCategoria.UseFloatingLabel = false;
            // 
            // lblDescCategoria
            // 
            lblDescCategoria.Location = new Point(365, 30);
            lblDescCategoria.Name = "lblDescCategoria";
            lblDescCategoria.Size = new Size(100, 23);
            lblDescCategoria.TabIndex = 2;
            lblDescCategoria.Text = "Descripción:";
            // 
            // txtDescCategoria
            // 
            txtDescCategoria.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtDescCategoria.BackColor = Color.White;
            txtDescCategoria.FloatingLabelText = "";
            txtDescCategoria.Location = new Point(471, 27);
            txtDescCategoria.MaxLength = 32767;
            txtDescCategoria.MinimumSize = new Size(0, 38);
            txtDescCategoria.Multiline = false;
            txtDescCategoria.Name = "txtDescCategoria";
            txtDescCategoria.Padding = new Padding(0, 0, 0, 4);
            txtDescCategoria.PasswordChar = '\0';
            txtDescCategoria.PlaceholderText = "";
            txtDescCategoria.ReadOnly = false;
            txtDescCategoria.SelectionStart = 0;
            txtDescCategoria.Size = new Size(687, 69);
            txtDescCategoria.TabIndex = 3;
            txtDescCategoria.UseFloatingLabel = false;
            // 
            // btnNuevoCategoria
            // 
            btnNuevoCategoria.CornerRadius = 8;
            btnNuevoCategoria.FlatStyle = FlatStyle.Flat;
            btnNuevoCategoria.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnNuevoCategoria.Icon = FontAwesome.Sharp.IconChar.Add;
            btnNuevoCategoria.IconColor = null;
            btnNuevoCategoria.IconSize = 20;
            btnNuevoCategoria.Location = new Point(15, 102);
            btnNuevoCategoria.Name = "btnNuevoCategoria";
            btnNuevoCategoria.Size = new Size(115, 34);
            btnNuevoCategoria.TabIndex = 4;
            btnNuevoCategoria.Text = "Nuevo";
            btnNuevoCategoria.Variant = PromacoHerra.Controls.MaterialButtonVariant.Default;
            // 
            // btnGuardarCategoria
            // 
            btnGuardarCategoria.CornerRadius = 8;
            btnGuardarCategoria.FlatStyle = FlatStyle.Flat;
            btnGuardarCategoria.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnGuardarCategoria.Icon = FontAwesome.Sharp.IconChar.Save;
            btnGuardarCategoria.IconColor = null;
            btnGuardarCategoria.IconSize = 20;
            btnGuardarCategoria.Location = new Point(136, 102);
            btnGuardarCategoria.Name = "btnGuardarCategoria";
            btnGuardarCategoria.Size = new Size(126, 34);
            btnGuardarCategoria.TabIndex = 5;
            btnGuardarCategoria.Text = "Guardar";
            btnGuardarCategoria.Variant = PromacoHerra.Controls.MaterialButtonVariant.Default;
            // 
            // btnEditarCategoria
            // 
            btnEditarCategoria.CornerRadius = 8;
            btnEditarCategoria.FlatStyle = FlatStyle.Flat;
            btnEditarCategoria.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnEditarCategoria.Icon = FontAwesome.Sharp.IconChar.Edit;
            btnEditarCategoria.IconColor = null;
            btnEditarCategoria.IconSize = 20;
            btnEditarCategoria.Location = new Point(268, 102);
            btnEditarCategoria.Name = "btnEditarCategoria";
            btnEditarCategoria.Size = new Size(118, 34);
            btnEditarCategoria.TabIndex = 6;
            btnEditarCategoria.Text = "Editar";
            btnEditarCategoria.Variant = PromacoHerra.Controls.MaterialButtonVariant.Default;
            // 
            // btnCancelarCategoria
            // 
            btnCancelarCategoria.CornerRadius = 8;
            btnCancelarCategoria.FlatStyle = FlatStyle.Flat;
            btnCancelarCategoria.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnCancelarCategoria.Icon = FontAwesome.Sharp.IconChar.Close;
            btnCancelarCategoria.IconColor = null;
            btnCancelarCategoria.IconSize = 20;
            btnCancelarCategoria.Location = new Point(392, 102);
            btnCancelarCategoria.Name = "btnCancelarCategoria";
            btnCancelarCategoria.Size = new Size(119, 34);
            btnCancelarCategoria.TabIndex = 7;
            btnCancelarCategoria.Text = "Cancelar";
            btnCancelarCategoria.Variant = PromacoHerra.Controls.MaterialButtonVariant.Default;
            // 
            // btnEliminarCategoria
            // 
            btnEliminarCategoria.CornerRadius = 8;
            btnEliminarCategoria.FlatStyle = FlatStyle.Flat;
            btnEliminarCategoria.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnEliminarCategoria.Icon = FontAwesome.Sharp.IconChar.Trash;
            btnEliminarCategoria.IconColor = null;
            btnEliminarCategoria.IconSize = 20;
            btnEliminarCategoria.Location = new Point(517, 102);
            btnEliminarCategoria.Name = "btnEliminarCategoria";
            btnEliminarCategoria.Size = new Size(121, 34);
            btnEliminarCategoria.TabIndex = 8;
            btnEliminarCategoria.Text = "Eliminar";
            btnEliminarCategoria.Variant = PromacoHerra.Controls.MaterialButtonVariant.Default;
            // 
            // dgvCategorias
            // 
            dgvCategorias.AllowUserToAddRows = false;
            dgvCategorias.AllowUserToDeleteRows = false;
            dgvCategorias.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvCategorias.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCategorias.Location = new Point(11, 169);
            dgvCategorias.MultiSelect = false;
            dgvCategorias.Name = "dgvCategorias";
            dgvCategorias.ReadOnly = true;
            dgvCategorias.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCategorias.Size = new Size(1199, 669);
            dgvCategorias.TabIndex = 1;
            // 
            // tabMarcas
            // 
            tabMarcas.Controls.Add(grpDatosMarca);
            tabMarcas.Controls.Add(dgvMarcas);
            tabMarcas.Location = new Point(4, 29);
            tabMarcas.Name = "tabMarcas";
            tabMarcas.Padding = new Padding(8);
            tabMarcas.Size = new Size(1219, 883);
            tabMarcas.TabIndex = 2;
            tabMarcas.Text = "  Marcas  ";
            // 
            // grpDatosMarca
            // 
            grpDatosMarca.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
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
            grpDatosMarca.Size = new Size(1197, 152);
            grpDatosMarca.TabIndex = 0;
            grpDatosMarca.TabStop = false;
            grpDatosMarca.Text = "Datos de la Marca";
            // 
            // lblNombreMarca
            // 
            lblNombreMarca.Location = new Point(12, 30);
            lblNombreMarca.Name = "lblNombreMarca";
            lblNombreMarca.Size = new Size(71, 23);
            lblNombreMarca.TabIndex = 0;
            lblNombreMarca.Text = "Nombre:";
            // 
            // txtNombreMarca
            // 
            txtNombreMarca.BackColor = Color.White;
            txtNombreMarca.FloatingLabelText = "";
            txtNombreMarca.Location = new Point(89, 30);
            txtNombreMarca.MaxLength = 32767;
            txtNombreMarca.MinimumSize = new Size(0, 38);
            txtNombreMarca.Multiline = false;
            txtNombreMarca.Name = "txtNombreMarca";
            txtNombreMarca.Padding = new Padding(0, 0, 0, 4);
            txtNombreMarca.PasswordChar = '\0';
            txtNombreMarca.PlaceholderText = "";
            txtNombreMarca.ReadOnly = false;
            txtNombreMarca.SelectionStart = 0;
            txtNombreMarca.Size = new Size(200, 38);
            txtNombreMarca.TabIndex = 1;
            txtNombreMarca.UseFloatingLabel = false;
            // 
            // lblDescMarca
            // 
            lblDescMarca.Location = new Point(349, 30);
            lblDescMarca.Name = "lblDescMarca";
            lblDescMarca.Size = new Size(114, 23);
            lblDescMarca.TabIndex = 2;
            lblDescMarca.Text = "Descripción:";
            // 
            // txtDescMarca
            // 
            txtDescMarca.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtDescMarca.BackColor = Color.White;
            txtDescMarca.FloatingLabelText = "";
            txtDescMarca.Location = new Point(469, 27);
            txtDescMarca.MaxLength = 32767;
            txtDescMarca.MinimumSize = new Size(0, 38);
            txtDescMarca.Multiline = false;
            txtDescMarca.Name = "txtDescMarca";
            txtDescMarca.Padding = new Padding(0, 0, 0, 4);
            txtDescMarca.PasswordChar = '\0';
            txtDescMarca.PlaceholderText = "";
            txtDescMarca.ReadOnly = false;
            txtDescMarca.SelectionStart = 0;
            txtDescMarca.Size = new Size(687, 79);
            txtDescMarca.TabIndex = 3;
            txtDescMarca.UseFloatingLabel = false;
            // 
            // btnNuevoMarca
            // 
            btnNuevoMarca.CornerRadius = 8;
            btnNuevoMarca.FlatStyle = FlatStyle.Flat;
            btnNuevoMarca.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnNuevoMarca.Icon = FontAwesome.Sharp.IconChar.Add;
            btnNuevoMarca.IconColor = null;
            btnNuevoMarca.IconSize = 20;
            btnNuevoMarca.Location = new Point(6, 112);
            btnNuevoMarca.Name = "btnNuevoMarca";
            btnNuevoMarca.Size = new Size(106, 34);
            btnNuevoMarca.TabIndex = 4;
            btnNuevoMarca.Text = "Nuevo";
            btnNuevoMarca.Variant = PromacoHerra.Controls.MaterialButtonVariant.Default;
            // 
            // btnGuardarMarca
            // 
            btnGuardarMarca.CornerRadius = 8;
            btnGuardarMarca.FlatStyle = FlatStyle.Flat;
            btnGuardarMarca.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnGuardarMarca.Icon = FontAwesome.Sharp.IconChar.Save;
            btnGuardarMarca.IconColor = null;
            btnGuardarMarca.IconSize = 20;
            btnGuardarMarca.Location = new Point(118, 112);
            btnGuardarMarca.Name = "btnGuardarMarca";
            btnGuardarMarca.Size = new Size(118, 34);
            btnGuardarMarca.TabIndex = 5;
            btnGuardarMarca.Text = "Guardar";
            btnGuardarMarca.Variant = PromacoHerra.Controls.MaterialButtonVariant.Default;
            // 
            // btnEditarMarca
            // 
            btnEditarMarca.CornerRadius = 8;
            btnEditarMarca.FlatStyle = FlatStyle.Flat;
            btnEditarMarca.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnEditarMarca.Icon = FontAwesome.Sharp.IconChar.Edit;
            btnEditarMarca.IconColor = null;
            btnEditarMarca.IconSize = 20;
            btnEditarMarca.Location = new Point(242, 112);
            btnEditarMarca.Name = "btnEditarMarca";
            btnEditarMarca.Size = new Size(127, 34);
            btnEditarMarca.TabIndex = 6;
            btnEditarMarca.Text = "Editar";
            btnEditarMarca.Variant = PromacoHerra.Controls.MaterialButtonVariant.Default;
            // 
            // btnCancelarMarca
            // 
            btnCancelarMarca.CornerRadius = 8;
            btnCancelarMarca.FlatStyle = FlatStyle.Flat;
            btnCancelarMarca.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnCancelarMarca.Icon = FontAwesome.Sharp.IconChar.Close;
            btnCancelarMarca.IconColor = null;
            btnCancelarMarca.IconSize = 20;
            btnCancelarMarca.Location = new Point(375, 112);
            btnCancelarMarca.Name = "btnCancelarMarca";
            btnCancelarMarca.Size = new Size(120, 34);
            btnCancelarMarca.TabIndex = 7;
            btnCancelarMarca.Text = "Cancelar";
            btnCancelarMarca.Variant = PromacoHerra.Controls.MaterialButtonVariant.Default;
            // 
            // btnEliminarMarca
            // 
            btnEliminarMarca.CornerRadius = 8;
            btnEliminarMarca.FlatStyle = FlatStyle.Flat;
            btnEliminarMarca.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnEliminarMarca.Icon = FontAwesome.Sharp.IconChar.Trash;
            btnEliminarMarca.IconColor = null;
            btnEliminarMarca.IconSize = 20;
            btnEliminarMarca.Location = new Point(501, 112);
            btnEliminarMarca.Name = "btnEliminarMarca";
            btnEliminarMarca.Size = new Size(129, 34);
            btnEliminarMarca.TabIndex = 8;
            btnEliminarMarca.Text = "Eliminar";
            btnEliminarMarca.Variant = PromacoHerra.Controls.MaterialButtonVariant.Default;
            // 
            // dgvMarcas
            // 
            dgvMarcas.AllowUserToAddRows = false;
            dgvMarcas.AllowUserToDeleteRows = false;
            dgvMarcas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvMarcas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMarcas.Location = new Point(8, 166);
            dgvMarcas.MultiSelect = false;
            dgvMarcas.Name = "dgvMarcas";
            dgvMarcas.ReadOnly = true;
            dgvMarcas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMarcas.Size = new Size(1199, 672);
            dgvMarcas.TabIndex = 1;
            // 
            // tabMantenimiento
            // 
            tabMantenimiento.Controls.Add(grpMantenimientosActivos);
            tabMantenimiento.Controls.Add(grpAccionMantenimiento);
            tabMantenimiento.Location = new Point(4, 29);
            tabMantenimiento.Name = "tabMantenimiento";
            tabMantenimiento.Padding = new Padding(8);
            tabMantenimiento.Size = new Size(1219, 883);
            tabMantenimiento.TabIndex = 3;
            tabMantenimiento.Text = "  Mantenimiento  ";
            // 
            // grpMantenimientosActivos
            // 
            grpMantenimientosActivos.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpMantenimientosActivos.Controls.Add(dgvMantenimientos);
            grpMantenimientosActivos.Location = new Point(8, 8);
            grpMantenimientosActivos.Name = "grpMantenimientosActivos";
            grpMantenimientosActivos.Size = new Size(1199, 270);
            grpMantenimientosActivos.TabIndex = 0;
            grpMantenimientosActivos.TabStop = false;
            grpMantenimientosActivos.Text = "Mantenimientos en curso — selecciona uno para cerrarlo";
            // 
            // dgvMantenimientos
            // 
            dgvMantenimientos.AllowUserToAddRows = false;
            dgvMantenimientos.AllowUserToDeleteRows = false;
            dgvMantenimientos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvMantenimientos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMantenimientos.Location = new Point(10, 22);
            dgvMantenimientos.MultiSelect = false;
            dgvMantenimientos.Name = "dgvMantenimientos";
            dgvMantenimientos.ReadOnly = true;
            dgvMantenimientos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMantenimientos.Size = new Size(1179, 238);
            dgvMantenimientos.TabIndex = 0;
            // 
            // grpAccionMantenimiento
            // 
            grpAccionMantenimiento.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
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
            grpAccionMantenimiento.Size = new Size(1199, 334);
            grpAccionMantenimiento.TabIndex = 1;
            grpAccionMantenimiento.TabStop = false;
            grpAccionMantenimiento.Text = "Acciones";
            // 
            // lblHerramientaMant
            // 
            lblHerramientaMant.Location = new Point(10, 65);
            lblHerramientaMant.Name = "lblHerramientaMant";
            lblHerramientaMant.Size = new Size(106, 23);
            lblHerramientaMant.TabIndex = 0;
            lblHerramientaMant.Text = "Herramienta:";
            // 
            // cboHerramientaMant
            // 
            cboHerramientaMant.BackColor = Color.White;
            cboHerramientaMant.DataSource = null;
            cboHerramientaMant.DisplayMember = "";
            cboHerramientaMant.DropDownStyle = ComboBoxStyle.DropDownList;
            cboHerramientaMant.Location = new Point(131, 65);
            cboHerramientaMant.MinimumSize = new Size(0, 38);
            cboHerramientaMant.Name = "cboHerramientaMant";
            cboHerramientaMant.Padding = new Padding(0, 0, 0, 4);
            cboHerramientaMant.SelectedIndex = -1;
            cboHerramientaMant.SelectedItem = null;
            cboHerramientaMant.SelectedValue = null;
            cboHerramientaMant.Size = new Size(230, 38);
            cboHerramientaMant.TabIndex = 1;
            cboHerramientaMant.ValueMember = "";
            cboHerramientaMant.Load += cboHerramientaMant_Load;
            // 
            // lblTipoMant
            // 
            lblTipoMant.Location = new Point(10, 120);
            lblTipoMant.Name = "lblTipoMant";
            lblTipoMant.Size = new Size(80, 23);
            lblTipoMant.TabIndex = 2;
            lblTipoMant.Text = "Tipo:";
            // 
            // cboTipoMant
            // 
            cboTipoMant.BackColor = Color.White;
            cboTipoMant.DataSource = null;
            cboTipoMant.DisplayMember = "";
            cboTipoMant.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTipoMant.Location = new Point(131, 120);
            cboTipoMant.MinimumSize = new Size(0, 38);
            cboTipoMant.Name = "cboTipoMant";
            cboTipoMant.Padding = new Padding(0, 0, 0, 4);
            cboTipoMant.SelectedIndex = -1;
            cboTipoMant.SelectedItem = null;
            cboTipoMant.SelectedValue = null;
            cboTipoMant.Size = new Size(150, 38);
            cboTipoMant.TabIndex = 3;
            cboTipoMant.ValueMember = "";
            // 
            // lblRealizadoPor
            // 
            lblRealizadoPor.Location = new Point(10, 164);
            lblRealizadoPor.Name = "lblRealizadoPor";
            lblRealizadoPor.Size = new Size(115, 23);
            lblRealizadoPor.TabIndex = 4;
            lblRealizadoPor.Text = "Realizado por:";
            // 
            // txtRealizadoPor
            // 
            txtRealizadoPor.BackColor = Color.White;
            txtRealizadoPor.FloatingLabelText = "";
            txtRealizadoPor.Location = new Point(131, 164);
            txtRealizadoPor.MaxLength = 32767;
            txtRealizadoPor.MinimumSize = new Size(0, 38);
            txtRealizadoPor.Multiline = false;
            txtRealizadoPor.Name = "txtRealizadoPor";
            txtRealizadoPor.Padding = new Padding(0, 0, 0, 4);
            txtRealizadoPor.PasswordChar = '\0';
            txtRealizadoPor.PlaceholderText = "";
            txtRealizadoPor.ReadOnly = false;
            txtRealizadoPor.SelectionStart = 0;
            txtRealizadoPor.Size = new Size(230, 38);
            txtRealizadoPor.TabIndex = 5;
            txtRealizadoPor.UseFloatingLabel = false;
            // 
            // lblDescMant
            // 
            lblDescMant.Location = new Point(10, 220);
            lblDescMant.Name = "lblDescMant";
            lblDescMant.Size = new Size(98, 23);
            lblDescMant.TabIndex = 6;
            lblDescMant.Text = "Descripción:";
            // 
            // txtDescMant
            // 
            txtDescMant.BackColor = Color.White;
            txtDescMant.FloatingLabelText = "";
            txtDescMant.Location = new Point(131, 220);
            txtDescMant.MaxLength = 32767;
            txtDescMant.MinimumSize = new Size(0, 38);
            txtDescMant.Multiline = false;
            txtDescMant.Name = "txtDescMant";
            txtDescMant.Padding = new Padding(0, 0, 0, 4);
            txtDescMant.PasswordChar = '\0';
            txtDescMant.PlaceholderText = "";
            txtDescMant.ReadOnly = false;
            txtDescMant.SelectionStart = 0;
            txtDescMant.Size = new Size(230, 38);
            txtDescMant.TabIndex = 7;
            txtDescMant.UseFloatingLabel = false;
            // 
            // btnAbrirMantenimiento
            // 
            btnAbrirMantenimiento.CornerRadius = 8;
            btnAbrirMantenimiento.FlatStyle = FlatStyle.Flat;
            btnAbrirMantenimiento.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnAbrirMantenimiento.Icon = FontAwesome.Sharp.IconChar.Wrench;
            btnAbrirMantenimiento.IconColor = null;
            btnAbrirMantenimiento.IconSize = 20;
            btnAbrirMantenimiento.Location = new Point(10, 267);
            btnAbrirMantenimiento.Name = "btnAbrirMantenimiento";
            btnAbrirMantenimiento.Size = new Size(200, 38);
            btnAbrirMantenimiento.TabIndex = 8;
            btnAbrirMantenimiento.Text = "Abrir mantenimiento";
            btnAbrirMantenimiento.Variant = PromacoHerra.Controls.MaterialButtonVariant.Default;
            // 
            // lblCosto
            // 
            lblCosto.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblCosto.Location = new Point(767, 65);
            lblCosto.Name = "lblCosto";
            lblCosto.Size = new Size(80, 23);
            lblCosto.TabIndex = 9;
            lblCosto.Text = "Costo (L):";
            // 
            // txtCosto
            // 
            txtCosto.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtCosto.BackColor = Color.White;
            txtCosto.FloatingLabelText = "";
            txtCosto.Location = new Point(884, 65);
            txtCosto.MaxLength = 32767;
            txtCosto.MinimumSize = new Size(0, 38);
            txtCosto.Multiline = false;
            txtCosto.Name = "txtCosto";
            txtCosto.Padding = new Padding(0, 0, 0, 4);
            txtCosto.PasswordChar = '\0';
            txtCosto.PlaceholderText = "";
            txtCosto.ReadOnly = false;
            txtCosto.SelectionStart = 0;
            txtCosto.Size = new Size(130, 38);
            txtCosto.TabIndex = 10;
            txtCosto.UseFloatingLabel = false;
            // 
            // lblDescCierre
            // 
            lblDescCierre.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblDescCierre.Location = new Point(762, 125);
            lblDescCierre.Name = "lblDescCierre";
            lblDescCierre.Size = new Size(116, 23);
            lblDescCierre.TabIndex = 11;
            lblDescCierre.Text = "Notas cierre:";
            // 
            // txtDescCierre
            // 
            txtDescCierre.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtDescCierre.BackColor = Color.White;
            txtDescCierre.FloatingLabelText = "";
            txtDescCierre.Location = new Point(884, 125);
            txtDescCierre.MaxLength = 32767;
            txtDescCierre.MinimumSize = new Size(0, 38);
            txtDescCierre.Multiline = false;
            txtDescCierre.Name = "txtDescCierre";
            txtDescCierre.Padding = new Padding(0, 0, 0, 4);
            txtDescCierre.PasswordChar = '\0';
            txtDescCierre.PlaceholderText = "";
            txtDescCierre.ReadOnly = false;
            txtDescCierre.SelectionStart = 0;
            txtDescCierre.Size = new Size(301, 38);
            txtDescCierre.TabIndex = 12;
            txtDescCierre.UseFloatingLabel = false;
            // 
            // btnCerrarMantenimiento
            // 
            btnCerrarMantenimiento.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCerrarMantenimiento.CornerRadius = 8;
            btnCerrarMantenimiento.FlatStyle = FlatStyle.Flat;
            btnCerrarMantenimiento.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnCerrarMantenimiento.Icon = FontAwesome.Sharp.IconChar.CheckCircle;
            btnCerrarMantenimiento.IconColor = null;
            btnCerrarMantenimiento.IconSize = 20;
            btnCerrarMantenimiento.Location = new Point(755, 190);
            btnCerrarMantenimiento.Name = "btnCerrarMantenimiento";
            btnCerrarMantenimiento.Size = new Size(254, 38);
            btnCerrarMantenimiento.TabIndex = 13;
            btnCerrarMantenimiento.Text = "Cerrar mantenimiento";
            btnCerrarMantenimiento.Variant = PromacoHerra.Controls.MaterialButtonVariant.Default;
            // 
            // btnVerHistorial
            // 
            btnVerHistorial.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnVerHistorial.CornerRadius = 8;
            btnVerHistorial.FlatStyle = FlatStyle.Flat;
            btnVerHistorial.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnVerHistorial.Icon = FontAwesome.Sharp.IconChar.ClockRotateLeft;
            btnVerHistorial.IconColor = null;
            btnVerHistorial.IconSize = 20;
            btnVerHistorial.Location = new Point(755, 245);
            btnVerHistorial.Name = "btnVerHistorial";
            btnVerHistorial.Size = new Size(254, 38);
            btnVerHistorial.TabIndex = 14;
            btnVerHistorial.Text = "Ver historial de herramienta";
            btnVerHistorial.Variant = PromacoHerra.Controls.MaterialButtonVariant.Default;
            // 
            // lblSecAbrir
            // 
            lblSecAbrir.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            lblSecAbrir.Location = new Point(10, 22);
            lblSecAbrir.Name = "lblSecAbrir";
            lblSecAbrir.Size = new Size(380, 20);
            lblSecAbrir.TabIndex = 15;
            lblSecAbrir.Text = "▶  Abrir nuevo mantenimiento";
            // 
            // divisor
            // 
            divisor.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            divisor.BorderStyle = BorderStyle.Fixed3D;
            divisor.Location = new Point(747, 22);
            divisor.Name = "divisor";
            divisor.Size = new Size(2, 299);
            divisor.TabIndex = 16;
            // 
            // lblSecCerrar
            // 
            lblSecCerrar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblSecCerrar.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            lblSecCerrar.Location = new Point(762, 22);
            lblSecCerrar.Name = "lblSecCerrar";
            lblSecCerrar.Size = new Size(380, 20);
            lblSecCerrar.TabIndex = 17;
            lblSecCerrar.Text = "▶  Cerrar mantenimiento seleccionado";
            // 
            // FrmHerramientas
            // 
            ClientSize = new Size(1269, 978);
            Controls.Add(lblTitulo);
            Controls.Add(tabControl);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FrmHerramientas";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Herramientas";
            tabControl.ResumeLayout(false);
            tabHerramientas.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvHerramientas).EndInit();
            grpDatos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)nudStockTotal).EndInit();
            tabCategorias.ResumeLayout(false);
            grpDatosCategoria.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvCategorias).EndInit();
            tabMarcas.ResumeLayout(false);
            grpDatosMarca.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvMarcas).EndInit();
            tabMantenimiento.ResumeLayout(false);
            grpMantenimientosActivos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvMantenimientos).EndInit();
            grpAccionMantenimiento.ResumeLayout(false);
            ResumeLayout(false);
        }

        private Label lblSecAbrir;
        private Label divisor;
        private Label lblSecCerrar;
        private DataGridView dgvHerramientas;
    }
}