namespace PromacoHerra
{
    partial class FrmHerramientas
    {
        private System.ComponentModel.IContainer components = null;

        // ── Encabezado ──
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.FlowLayoutPanel flpHeaderAcciones;
        private PromacoHerra.Controls.MaterialButton btnNuevaHerramienta;
        private PromacoHerra.Controls.MaterialButton btnDarDeBaja;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabHerramientas;
        private System.Windows.Forms.TabPage tabCatalogos;
        private System.Windows.Forms.TabPage tabMantenimiento;

        // ── Tab Herramientas: lista ──
        private System.Windows.Forms.SplitContainer splitHerramientas;
        private System.Windows.Forms.TableLayoutPanel tlpLista;
        private System.Windows.Forms.Panel pnlBuscarH;
        private FontAwesome.Sharp.IconPictureBox icoBuscarH;
        private PromacoHerra.Controls.MaterialTextBox txtBuscarH;
        private System.Windows.Forms.FlowLayoutPanel flpChips;
        private System.Windows.Forms.ListBox lstHerramientas;
        private System.Windows.Forms.Label lblListaVacia;

        // ── Tab Herramientas: detalle ──
        private System.Windows.Forms.Label lblSeleccionaHerramienta;
        private System.Windows.Forms.Panel pnlDetalle;
        private System.Windows.Forms.Panel pnlDetalleFooter;
        private PromacoHerra.Controls.MaterialButton btnGuardarH;
        private PromacoHerra.Controls.MaterialButton btnCancelarH;
        private System.Windows.Forms.Panel pnlDetalleScroll;
        private System.Windows.Forms.TableLayoutPanel tlpDetalle;
        private System.Windows.Forms.Panel pnlDetHeader;
        private System.Windows.Forms.Label lblNombreDet;
        private System.Windows.Forms.Label lblCodigoDet;
        private PromacoHerra.Controls.MaterialButton btnUnidades;
        private System.Windows.Forms.TableLayoutPanel tlpTiles;
        private PromacoHerra.Controls.StockTile tileTotal;
        private PromacoHerra.Controls.StockTile tileDisponibles;
        private PromacoHerra.Controls.StockTile tilePrestadas;
        private PromacoHerra.Controls.StockTile tileMantenimiento;
        private PromacoHerra.Controls.StockTile tileDañadas;
        private System.Windows.Forms.TableLayoutPanel tlpForm;
        private System.Windows.Forms.Label lblCodigoF;
        private PromacoHerra.Controls.MaterialTextBox txtCodigoF;
        private System.Windows.Forms.Label lblUbicacionF;
        private PromacoHerra.Controls.MaterialComboBox cboUbicacionF;
        private System.Windows.Forms.Label lblNombreF;
        private PromacoHerra.Controls.MaterialTextBox txtNombreF;
        private System.Windows.Forms.Label lblCategoriaF;
        private PromacoHerra.Controls.MaterialComboBox cboCategoriaF;
        private System.Windows.Forms.Label lblMarcaF;
        private PromacoHerra.Controls.MaterialComboBox cboMarcaF;
        private System.Windows.Forms.Label lblStockInicialF;
        private System.Windows.Forms.NumericUpDown nudStockInicial;
        private System.Windows.Forms.Label lblCaracteristicasF;
        private System.Windows.Forms.TextBox txtCaracteristicasF;
        private System.Windows.Forms.Panel pnlUnidadesHeader;
        private System.Windows.Forms.Label lblUnidadesTitulo;
        private System.Windows.Forms.LinkLabel lnkVerTodas;
        private System.Windows.Forms.DataGridView dgvUnidadesMini;

        // ── Tab Catálogos ──
        private System.Windows.Forms.TableLayoutPanel tlpCatalogos;
        private PromacoHerra.Controls.SegmentedControl segCatalogo;
        private System.Windows.Forms.FlowLayoutPanel flpCatForm;
        private PromacoHerra.Controls.MaterialTextBox txtCatNombre;
        private PromacoHerra.Controls.MaterialTextBox txtCatDescripcion;
        private PromacoHerra.Controls.MaterialTextBox txtCatTelefono;
        private PromacoHerra.Controls.MaterialButton btnCatAgregar;
        private PromacoHerra.Controls.MaterialButton btnCatGuardar;
        private PromacoHerra.Controls.MaterialButton btnCatEliminar;
        private System.Windows.Forms.LinkLabel lnkCatCancelar;
        private System.Windows.Forms.DataGridView dgvCatalogo;

        // ── Tab Mantenimiento ──
        private System.Windows.Forms.TableLayoutPanel tlpMant;
        private System.Windows.Forms.FlowLayoutPanel flpMantTop;
        private PromacoHerra.Controls.MaterialTextBox txtBuscarMant;
        private PromacoHerra.Controls.ChipLabel chipMantActivos;
        private System.Windows.Forms.DataGridView dgvMant;
        private PromacoHerra.Controls.MaterialButton btnNuevoMant;
        private PromacoHerra.RoundedPanel pnlPendientes;
        private System.Windows.Forms.Panel pnlPendientesHeader;
        private System.Windows.Forms.Label lblPendientesTitulo;
        private PromacoHerra.Controls.ChipLabel chipPendientes;
        private System.Windows.Forms.DataGridView dgvPendientes;
        private System.Windows.Forms.Label lblPendientesVacio;
        private PromacoHerra.RoundedPanel pnlAccionesMant;
        private System.Windows.Forms.Label lblAccionInfo;
        private System.Windows.Forms.FlowLayoutPanel flpAccionesMant;
        private PromacoHerra.Controls.MaterialButton btnAccionMant;
        private System.Windows.Forms.LinkLabel lnkHistorial;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pnlHeader = new Panel();
            lblTitulo = new Label();
            flpHeaderAcciones = new FlowLayoutPanel();
            btnNuevaHerramienta = new PromacoHerra.Controls.MaterialButton();
            btnDarDeBaja = new PromacoHerra.Controls.MaterialButton();
            tabMain = new TabControl();
            tabHerramientas = new TabPage();
            splitHerramientas = new SplitContainer();
            tlpLista = new TableLayoutPanel();
            pnlBuscarH = new Panel();
            txtBuscarH = new PromacoHerra.Controls.MaterialTextBox();
            icoBuscarH = new FontAwesome.Sharp.IconPictureBox();
            flpChips = new FlowLayoutPanel();
            lstHerramientas = new ListBox();
            lblListaVacia = new Label();
            pnlDetalle = new Panel();
            pnlDetalleScroll = new Panel();
            tlpDetalle = new TableLayoutPanel();
            pnlDetHeader = new Panel();
            lblNombreDet = new Label();
            lblCodigoDet = new Label();
            btnUnidades = new PromacoHerra.Controls.MaterialButton();
            tlpTiles = new TableLayoutPanel();
            tileTotal = new PromacoHerra.Controls.StockTile();
            tileDisponibles = new PromacoHerra.Controls.StockTile();
            tilePrestadas = new PromacoHerra.Controls.StockTile();
            tileMantenimiento = new PromacoHerra.Controls.StockTile();
            tileDañadas = new PromacoHerra.Controls.StockTile();
            tlpForm = new TableLayoutPanel();
            lblCodigoF = new Label();
            lblUbicacionF = new Label();
            txtCodigoF = new PromacoHerra.Controls.MaterialTextBox();
            cboUbicacionF = new PromacoHerra.Controls.MaterialComboBox();
            lblNombreF = new Label();
            txtNombreF = new PromacoHerra.Controls.MaterialTextBox();
            lblCategoriaF = new Label();
            lblMarcaF = new Label();
            cboCategoriaF = new PromacoHerra.Controls.MaterialComboBox();
            cboMarcaF = new PromacoHerra.Controls.MaterialComboBox();
            lblStockInicialF = new Label();
            nudStockInicial = new NumericUpDown();
            lblCaracteristicasF = new Label();
            txtCaracteristicasF = new TextBox();
            pnlUnidadesHeader = new Panel();
            lblUnidadesTitulo = new Label();
            lnkVerTodas = new LinkLabel();
            dgvUnidadesMini = new DataGridView();
            pnlDetalleFooter = new Panel();
            btnCancelarH = new PromacoHerra.Controls.MaterialButton();
            btnGuardarH = new PromacoHerra.Controls.MaterialButton();
            lblSeleccionaHerramienta = new Label();
            tabCatalogos = new TabPage();
            tlpCatalogos = new TableLayoutPanel();
            segCatalogo = new PromacoHerra.Controls.SegmentedControl();
            flpCatForm = new FlowLayoutPanel();
            txtCatNombre = new PromacoHerra.Controls.MaterialTextBox();
            txtCatDescripcion = new PromacoHerra.Controls.MaterialTextBox();
            txtCatTelefono = new PromacoHerra.Controls.MaterialTextBox();
            btnCatAgregar = new PromacoHerra.Controls.MaterialButton();
            btnCatGuardar = new PromacoHerra.Controls.MaterialButton();
            btnCatEliminar = new PromacoHerra.Controls.MaterialButton();
            lnkCatCancelar = new LinkLabel();
            dgvCatalogo = new DataGridView();
            tabMantenimiento = new TabPage();
            tlpMant = new TableLayoutPanel();
            flpMantTop = new FlowLayoutPanel();
            txtBuscarMant = new PromacoHerra.Controls.MaterialTextBox();
            chipMantActivos = new PromacoHerra.Controls.ChipLabel();
            dgvMant = new DataGridView();
            btnNuevoMant = new PromacoHerra.Controls.MaterialButton();
            pnlPendientes = new RoundedPanel();
            pnlPendientesHeader = new Panel();
            lblPendientesTitulo = new Label();
            chipPendientes = new PromacoHerra.Controls.ChipLabel();
            dgvPendientes = new DataGridView();
            lblPendientesVacio = new Label();
            pnlAccionesMant = new RoundedPanel();
            lblAccionInfo = new Label();
            flpAccionesMant = new FlowLayoutPanel();
            btnAccionMant = new PromacoHerra.Controls.MaterialButton();
            lnkHistorial = new LinkLabel();
            pnlHeader.SuspendLayout();
            flpHeaderAcciones.SuspendLayout();
            tabMain.SuspendLayout();
            tabHerramientas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitHerramientas).BeginInit();
            splitHerramientas.Panel1.SuspendLayout();
            splitHerramientas.Panel2.SuspendLayout();
            splitHerramientas.SuspendLayout();
            tlpLista.SuspendLayout();
            pnlBuscarH.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)icoBuscarH).BeginInit();
            pnlDetalle.SuspendLayout();
            pnlDetalleScroll.SuspendLayout();
            tlpDetalle.SuspendLayout();
            pnlDetHeader.SuspendLayout();
            tlpTiles.SuspendLayout();
            tlpForm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudStockInicial).BeginInit();
            pnlUnidadesHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUnidadesMini).BeginInit();
            pnlDetalleFooter.SuspendLayout();
            tabCatalogos.SuspendLayout();
            tlpCatalogos.SuspendLayout();
            flpCatForm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCatalogo).BeginInit();
            tabMantenimiento.SuspendLayout();
            tlpMant.SuspendLayout();
            flpMantTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMant).BeginInit();
            pnlPendientes.SuspendLayout();
            pnlPendientesHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPendientes).BeginInit();
            pnlAccionesMant.SuspendLayout();
            flpAccionesMant.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.Controls.Add(lblTitulo);
            pnlHeader.Controls.Add(flpHeaderAcciones);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Padding = new Padding(20, 10, 20, 6);
            pnlHeader.Size = new Size(1200, 64);
            pnlHeader.TabIndex = 0;
            // 
            // lblTitulo
            // 
            lblTitulo.Dock = DockStyle.Fill;
            lblTitulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitulo.Location = new Point(20, 10);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(794, 48);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Herramientas";
            lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // flpHeaderAcciones
            // 
            flpHeaderAcciones.AutoSize = true;
            flpHeaderAcciones.Controls.Add(btnNuevaHerramienta);
            flpHeaderAcciones.Controls.Add(btnDarDeBaja);
            flpHeaderAcciones.Dock = DockStyle.Right;
            flpHeaderAcciones.FlowDirection = FlowDirection.RightToLeft;
            flpHeaderAcciones.Location = new Point(814, 10);
            flpHeaderAcciones.Name = "flpHeaderAcciones";
            flpHeaderAcciones.Size = new Size(366, 48);
            flpHeaderAcciones.TabIndex = 1;
            flpHeaderAcciones.WrapContents = false;
            // 
            // btnNuevaHerramienta
            // 
            btnNuevaHerramienta.CornerRadius = 8;
            btnNuevaHerramienta.FlatStyle = FlatStyle.Flat;
            btnNuevaHerramienta.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnNuevaHerramienta.Icon = FontAwesome.Sharp.IconChar.Add;
            btnNuevaHerramienta.IconColor = null;
            btnNuevaHerramienta.IconSize = 16;
            btnNuevaHerramienta.Location = new Point(166, 4);
            btnNuevaHerramienta.Margin = new Padding(8, 4, 0, 0);
            btnNuevaHerramienta.Name = "btnNuevaHerramienta";
            btnNuevaHerramienta.Size = new Size(200, 40);
            btnNuevaHerramienta.TabIndex = 0;
            btnNuevaHerramienta.Text = "Nueva herramienta";
            btnNuevaHerramienta.Variant = PromacoHerra.Controls.MaterialButtonVariant.Primary;
            // 
            // btnDarDeBaja
            // 
            btnDarDeBaja.CornerRadius = 8;
            btnDarDeBaja.Enabled = false;
            btnDarDeBaja.FlatStyle = FlatStyle.Flat;
            btnDarDeBaja.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnDarDeBaja.Icon = FontAwesome.Sharp.IconChar.TrashAlt;
            btnDarDeBaja.IconColor = null;
            btnDarDeBaja.IconSize = 16;
            btnDarDeBaja.Location = new Point(8, 4);
            btnDarDeBaja.Margin = new Padding(8, 4, 0, 0);
            btnDarDeBaja.Name = "btnDarDeBaja";
            btnDarDeBaja.Size = new Size(150, 40);
            btnDarDeBaja.TabIndex = 1;
            btnDarDeBaja.Text = "Dar de baja";
            btnDarDeBaja.Variant = PromacoHerra.Controls.MaterialButtonVariant.Default;
            // 
            // tabMain
            // 
            tabMain.Controls.Add(tabHerramientas);
            tabMain.Controls.Add(tabCatalogos);
            tabMain.Controls.Add(tabMantenimiento);
            tabMain.Dock = DockStyle.Fill;
            tabMain.Location = new Point(0, 64);
            tabMain.Name = "tabMain";
            tabMain.SelectedIndex = 0;
            tabMain.Size = new Size(1200, 656);
            tabMain.TabIndex = 1;
            // 
            // tabHerramientas
            // 
            tabHerramientas.Controls.Add(splitHerramientas);
            tabHerramientas.Location = new Point(4, 24);
            tabHerramientas.Name = "tabHerramientas";
            tabHerramientas.Size = new Size(1192, 628);
            tabHerramientas.TabIndex = 0;
            tabHerramientas.Text = "Herramientas";
            // 
            // splitHerramientas
            // 
            splitHerramientas.Dock = DockStyle.Fill;
            splitHerramientas.FixedPanel = FixedPanel.Panel1;
            splitHerramientas.IsSplitterFixed = true;
            splitHerramientas.Location = new Point(0, 0);
            splitHerramientas.Name = "splitHerramientas";
            // 
            // splitHerramientas.Panel1
            // 
            splitHerramientas.Panel1.Controls.Add(tlpLista);
            // 
            // splitHerramientas.Panel2
            // 
            splitHerramientas.Panel2.Controls.Add(pnlDetalle);
            splitHerramientas.Panel2.Controls.Add(lblSeleccionaHerramienta);
            splitHerramientas.Size = new Size(1192, 628);
            splitHerramientas.SplitterDistance = 340;
            splitHerramientas.SplitterWidth = 1;
            splitHerramientas.TabIndex = 0;
            // 
            // tlpLista
            // 
            tlpLista.ColumnCount = 1;
            tlpLista.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpLista.Controls.Add(pnlBuscarH, 0, 0);
            tlpLista.Controls.Add(flpChips, 0, 1);
            tlpLista.Controls.Add(lstHerramientas, 0, 2);
            tlpLista.Controls.Add(lblListaVacia, 0, 3);
            tlpLista.Dock = DockStyle.Fill;
            tlpLista.Location = new Point(0, 0);
            tlpLista.Name = "tlpLista";
            tlpLista.Padding = new Padding(0, 12, 0, 0);
            tlpLista.RowCount = 4;
            tlpLista.RowStyles.Add(new RowStyle());
            tlpLista.RowStyles.Add(new RowStyle());
            tlpLista.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpLista.RowStyles.Add(new RowStyle());
            tlpLista.Size = new Size(340, 628);
            tlpLista.TabIndex = 0;
            // 
            // pnlBuscarH
            // 
            pnlBuscarH.Controls.Add(txtBuscarH);
            pnlBuscarH.Controls.Add(icoBuscarH);
            pnlBuscarH.Dock = DockStyle.Fill;
            pnlBuscarH.Location = new Point(12, 12);
            pnlBuscarH.Margin = new Padding(12, 0, 12, 0);
            pnlBuscarH.Name = "pnlBuscarH";
            pnlBuscarH.Size = new Size(316, 40);
            pnlBuscarH.TabIndex = 0;
            // 
            // txtBuscarH
            // 
            txtBuscarH.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtBuscarH.BackColor = Color.White;
            txtBuscarH.FloatingLabelText = "";
            txtBuscarH.Location = new Point(30, 0);
            txtBuscarH.MaxLength = 32767;
            txtBuscarH.MinimumSize = new Size(0, 38);
            txtBuscarH.Multiline = false;
            txtBuscarH.Name = "txtBuscarH";
            txtBuscarH.Padding = new Padding(0, 0, 0, 4);
            txtBuscarH.PasswordChar = '\0';
            txtBuscarH.PlaceholderText = "Buscar herramienta…";
            txtBuscarH.ReadOnly = false;
            txtBuscarH.SelectionStart = 0;
            txtBuscarH.Size = new Size(286, 38);
            txtBuscarH.TabIndex = 1;
            txtBuscarH.UseFloatingLabel = false;
            // 
            // icoBuscarH
            // 
            icoBuscarH.BackColor = SystemColors.Control;
            icoBuscarH.ForeColor = SystemColors.ControlText;
            icoBuscarH.IconChar = FontAwesome.Sharp.IconChar.Search;
            icoBuscarH.IconColor = SystemColors.ControlText;
            icoBuscarH.IconFont = FontAwesome.Sharp.IconFont.Auto;
            icoBuscarH.IconSize = 22;
            icoBuscarH.Location = new Point(2, 9);
            icoBuscarH.Name = "icoBuscarH";
            icoBuscarH.Size = new Size(22, 22);
            icoBuscarH.TabIndex = 0;
            icoBuscarH.TabStop = false;
            // 
            // flpChips
            // 
            flpChips.AutoSize = true;
            flpChips.Dock = DockStyle.Fill;
            flpChips.Location = new Point(12, 62);
            flpChips.Margin = new Padding(12, 10, 12, 6);
            flpChips.Name = "flpChips";
            flpChips.Size = new Size(316, 1);
            flpChips.TabIndex = 1;
            // 
            // lstHerramientas
            // 
            lstHerramientas.BorderStyle = BorderStyle.None;
            lstHerramientas.Dock = DockStyle.Fill;
            lstHerramientas.DrawMode = DrawMode.OwnerDrawFixed;
            lstHerramientas.IntegralHeight = false;
            lstHerramientas.ItemHeight = 64;
            lstHerramientas.Location = new Point(0, 68);
            lstHerramientas.Margin = new Padding(0);
            lstHerramientas.Name = "lstHerramientas";
            lstHerramientas.Size = new Size(340, 500);
            lstHerramientas.TabIndex = 2;
            // 
            // lblListaVacia
            // 
            lblListaVacia.Dock = DockStyle.Fill;
            lblListaVacia.Font = new Font("Segoe UI", 9.5F);
            lblListaVacia.Location = new Point(3, 568);
            lblListaVacia.Name = "lblListaVacia";
            lblListaVacia.Size = new Size(334, 60);
            lblListaVacia.TabIndex = 3;
            lblListaVacia.Text = "No hay herramientas que coincidan.";
            lblListaVacia.TextAlign = ContentAlignment.TopCenter;
            lblListaVacia.Visible = false;
            // 
            // pnlDetalle
            // 
            pnlDetalle.Controls.Add(pnlDetalleScroll);
            pnlDetalle.Controls.Add(pnlDetalleFooter);
            pnlDetalle.Dock = DockStyle.Fill;
            pnlDetalle.Location = new Point(0, 0);
            pnlDetalle.Name = "pnlDetalle";
            pnlDetalle.Size = new Size(851, 628);
            pnlDetalle.TabIndex = 1;
            pnlDetalle.Visible = false;
            // 
            // pnlDetalleScroll
            // 
            pnlDetalleScroll.AutoScroll = true;
            pnlDetalleScroll.Controls.Add(tlpDetalle);
            pnlDetalleScroll.Dock = DockStyle.Fill;
            pnlDetalleScroll.Location = new Point(0, 0);
            pnlDetalleScroll.Name = "pnlDetalleScroll";
            pnlDetalleScroll.Padding = new Padding(24, 16, 24, 8);
            pnlDetalleScroll.Size = new Size(851, 564);
            pnlDetalleScroll.TabIndex = 0;
            // 
            // tlpDetalle
            // 
            tlpDetalle.AutoSize = true;
            tlpDetalle.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpDetalle.ColumnCount = 1;
            tlpDetalle.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpDetalle.Controls.Add(pnlDetHeader, 0, 0);
            tlpDetalle.Controls.Add(tlpTiles, 0, 1);
            tlpDetalle.Controls.Add(tlpForm, 0, 2);
            tlpDetalle.Controls.Add(pnlUnidadesHeader, 0, 3);
            tlpDetalle.Controls.Add(dgvUnidadesMini, 0, 4);
            tlpDetalle.Dock = DockStyle.Top;
            tlpDetalle.Location = new Point(24, 16);
            tlpDetalle.Name = "tlpDetalle";
            tlpDetalle.RowCount = 5;
            tlpDetalle.RowStyles.Add(new RowStyle());
            tlpDetalle.RowStyles.Add(new RowStyle());
            tlpDetalle.RowStyles.Add(new RowStyle());
            tlpDetalle.RowStyles.Add(new RowStyle());
            tlpDetalle.RowStyles.Add(new RowStyle());
            tlpDetalle.Size = new Size(786, 786);
            tlpDetalle.TabIndex = 0;
            // 
            // pnlDetHeader
            // 
            pnlDetHeader.Controls.Add(lblNombreDet);
            pnlDetHeader.Controls.Add(lblCodigoDet);
            pnlDetHeader.Controls.Add(btnUnidades);
            pnlDetHeader.Dock = DockStyle.Fill;
            pnlDetHeader.Location = new Point(0, 0);
            pnlDetHeader.Margin = new Padding(0, 0, 0, 12);
            pnlDetHeader.Name = "pnlDetHeader";
            pnlDetHeader.Size = new Size(786, 62);
            pnlDetHeader.TabIndex = 0;
            // 
            // lblNombreDet
            // 
            lblNombreDet.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblNombreDet.AutoEllipsis = true;
            lblNombreDet.Font = new Font("Segoe UI", 13.5F, FontStyle.Bold);
            lblNombreDet.Location = new Point(0, 0);
            lblNombreDet.Name = "lblNombreDet";
            lblNombreDet.Size = new Size(593, 32);
            lblNombreDet.TabIndex = 0;
            lblNombreDet.Text = "Herramienta";
            // 
            // lblCodigoDet
            // 
            lblCodigoDet.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblCodigoDet.AutoEllipsis = true;
            lblCodigoDet.Font = new Font("Segoe UI", 9.5F);
            lblCodigoDet.Location = new Point(0, 34);
            lblCodigoDet.Name = "lblCodigoDet";
            lblCodigoDet.Size = new Size(593, 22);
            lblCodigoDet.TabIndex = 1;
            lblCodigoDet.Text = "HER-0000";
            // 
            // btnUnidades
            // 
            btnUnidades.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnUnidades.CornerRadius = 8;
            btnUnidades.FlatStyle = FlatStyle.Flat;
            btnUnidades.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnUnidades.Icon = FontAwesome.Sharp.IconChar.BoxesStacked;
            btnUnidades.IconColor = null;
            btnUnidades.IconSize = 16;
            btnUnidades.Location = new Point(603, 7);
            btnUnidades.Name = "btnUnidades";
            btnUnidades.Size = new Size(180, 40);
            btnUnidades.TabIndex = 2;
            btnUnidades.Text = "Unidades / Stock";
            btnUnidades.Variant = PromacoHerra.Controls.MaterialButtonVariant.Default;
            // 
            // tlpTiles
            // 
            tlpTiles.ColumnCount = 5;
            tlpTiles.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpTiles.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpTiles.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpTiles.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpTiles.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpTiles.Controls.Add(tileTotal, 0, 0);
            tlpTiles.Controls.Add(tileDisponibles, 1, 0);
            tlpTiles.Controls.Add(tilePrestadas, 2, 0);
            tlpTiles.Controls.Add(tileMantenimiento, 3, 0);
            tlpTiles.Controls.Add(tileDañadas, 4, 0);
            tlpTiles.Dock = DockStyle.Fill;
            tlpTiles.Location = new Point(0, 74);
            tlpTiles.Margin = new Padding(0, 0, 0, 18);
            tlpTiles.Name = "tlpTiles";
            tlpTiles.RowCount = 1;
            tlpTiles.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpTiles.Size = new Size(786, 70);
            tlpTiles.TabIndex = 1;
            // 
            // tileTotal
            // 
            tileTotal.ColorValor = Color.FromArgb(25, 30, 50);
            tileTotal.Dock = DockStyle.Fill;
            tileTotal.Etiqueta = "Total";
            tileTotal.Location = new Point(0, 0);
            tileTotal.Margin = new Padding(0, 0, 10, 0);
            tileTotal.Name = "tileTotal";
            tileTotal.Size = new Size(147, 70);
            tileTotal.TabIndex = 0;
            tileTotal.Valor = 0;
            // 
            // tileDisponibles
            // 
            tileDisponibles.ColorValor = Color.FromArgb(25, 30, 50);
            tileDisponibles.Dock = DockStyle.Fill;
            tileDisponibles.Etiqueta = "Disponibles";
            tileDisponibles.Location = new Point(157, 0);
            tileDisponibles.Margin = new Padding(0, 0, 10, 0);
            tileDisponibles.Name = "tileDisponibles";
            tileDisponibles.Size = new Size(147, 70);
            tileDisponibles.TabIndex = 1;
            tileDisponibles.Valor = 0;
            // 
            // tilePrestadas
            // 
            tilePrestadas.ColorValor = Color.FromArgb(25, 30, 50);
            tilePrestadas.Dock = DockStyle.Fill;
            tilePrestadas.Etiqueta = "Prestadas";
            tilePrestadas.Location = new Point(314, 0);
            tilePrestadas.Margin = new Padding(0, 0, 10, 0);
            tilePrestadas.Name = "tilePrestadas";
            tilePrestadas.Size = new Size(147, 70);
            tilePrestadas.TabIndex = 2;
            tilePrestadas.Valor = 0;
            // 
            // tileMantenimiento
            // 
            tileMantenimiento.ColorValor = Color.FromArgb(25, 30, 50);
            tileMantenimiento.Dock = DockStyle.Fill;
            tileMantenimiento.Etiqueta = "En mant.";
            tileMantenimiento.Location = new Point(471, 0);
            tileMantenimiento.Margin = new Padding(0, 0, 10, 0);
            tileMantenimiento.Name = "tileMantenimiento";
            tileMantenimiento.Size = new Size(147, 70);
            tileMantenimiento.TabIndex = 3;
            tileMantenimiento.Valor = 0;
            // 
            // tileDañadas
            // 
            tileDañadas.ColorValor = Color.FromArgb(25, 30, 50);
            tileDañadas.Dock = DockStyle.Fill;
            tileDañadas.Etiqueta = "Dañadas";
            tileDañadas.Location = new Point(628, 0);
            tileDañadas.Margin = new Padding(0);
            tileDañadas.Name = "tileDañadas";
            tileDañadas.Size = new Size(158, 70);
            tileDañadas.TabIndex = 4;
            tileDañadas.Valor = 0;
            // 
            // tlpForm
            // 
            tlpForm.AutoSize = true;
            tlpForm.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpForm.ColumnCount = 2;
            tlpForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpForm.Controls.Add(lblCodigoF, 0, 0);
            tlpForm.Controls.Add(lblUbicacionF, 1, 0);
            tlpForm.Controls.Add(txtCodigoF, 0, 1);
            tlpForm.Controls.Add(cboUbicacionF, 1, 1);
            tlpForm.Controls.Add(lblNombreF, 0, 2);
            tlpForm.Controls.Add(txtNombreF, 0, 3);
            tlpForm.Controls.Add(lblCategoriaF, 0, 4);
            tlpForm.Controls.Add(lblMarcaF, 1, 4);
            tlpForm.Controls.Add(cboCategoriaF, 0, 5);
            tlpForm.Controls.Add(cboMarcaF, 1, 5);
            tlpForm.Controls.Add(lblStockInicialF, 0, 6);
            tlpForm.Controls.Add(nudStockInicial, 0, 7);
            tlpForm.Controls.Add(lblCaracteristicasF, 0, 8);
            tlpForm.Controls.Add(txtCaracteristicasF, 0, 9);
            tlpForm.Dock = DockStyle.Fill;
            tlpForm.Location = new Point(0, 162);
            tlpForm.Margin = new Padding(0, 0, 0, 16);
            tlpForm.Name = "tlpForm";
            tlpForm.RowCount = 10;
            tlpForm.RowStyles.Add(new RowStyle());
            tlpForm.RowStyles.Add(new RowStyle());
            tlpForm.RowStyles.Add(new RowStyle());
            tlpForm.RowStyles.Add(new RowStyle());
            tlpForm.RowStyles.Add(new RowStyle());
            tlpForm.RowStyles.Add(new RowStyle());
            tlpForm.RowStyles.Add(new RowStyle());
            tlpForm.RowStyles.Add(new RowStyle());
            tlpForm.RowStyles.Add(new RowStyle());
            tlpForm.RowStyles.Add(new RowStyle());
            tlpForm.Size = new Size(786, 346);
            tlpForm.TabIndex = 2;
            // 
            // lblCodigoF
            // 
            lblCodigoF.AutoSize = true;
            lblCodigoF.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblCodigoF.Location = new Point(0, 0);
            lblCodigoF.Margin = new Padding(0, 0, 12, 2);
            lblCodigoF.Name = "lblCodigoF";
            lblCodigoF.Size = new Size(45, 15);
            lblCodigoF.TabIndex = 0;
            lblCodigoF.Text = "Código";
            // 
            // lblUbicacionF
            // 
            lblUbicacionF.AutoSize = true;
            lblUbicacionF.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblUbicacionF.Location = new Point(393, 0);
            lblUbicacionF.Margin = new Padding(0, 0, 0, 2);
            lblUbicacionF.Name = "lblUbicacionF";
            lblUbicacionF.Size = new Size(61, 15);
            lblUbicacionF.TabIndex = 2;
            lblUbicacionF.Text = "Ubicación";
            // 
            // txtCodigoF
            // 
            txtCodigoF.BackColor = Color.White;
            txtCodigoF.Dock = DockStyle.Fill;
            txtCodigoF.FloatingLabelText = "";
            txtCodigoF.Location = new Point(0, 17);
            txtCodigoF.Margin = new Padding(0, 0, 12, 12);
            txtCodigoF.MaxLength = 32767;
            txtCodigoF.MinimumSize = new Size(0, 38);
            txtCodigoF.Multiline = false;
            txtCodigoF.Name = "txtCodigoF";
            txtCodigoF.Padding = new Padding(0, 0, 0, 4);
            txtCodigoF.PasswordChar = '\0';
            txtCodigoF.PlaceholderText = "";
            txtCodigoF.ReadOnly = true;
            txtCodigoF.SelectionStart = 0;
            txtCodigoF.Size = new Size(381, 38);
            txtCodigoF.TabIndex = 1;
            txtCodigoF.UseFloatingLabel = false;
            // 
            // cboUbicacionF
            // 
            cboUbicacionF.BackColor = Color.White;
            cboUbicacionF.DataSource = null;
            cboUbicacionF.DisplayMember = "";
            cboUbicacionF.Dock = DockStyle.Fill;
            cboUbicacionF.DropDownStyle = ComboBoxStyle.DropDownList;
            cboUbicacionF.Location = new Point(393, 17);
            cboUbicacionF.Margin = new Padding(0, 0, 0, 12);
            cboUbicacionF.MinimumSize = new Size(0, 38);
            cboUbicacionF.Name = "cboUbicacionF";
            cboUbicacionF.Padding = new Padding(0, 0, 0, 4);
            cboUbicacionF.SelectedIndex = -1;
            cboUbicacionF.SelectedItem = null;
            cboUbicacionF.SelectedValue = null;
            cboUbicacionF.Size = new Size(393, 38);
            cboUbicacionF.TabIndex = 3;
            cboUbicacionF.ValueMember = "";
            // 
            // lblNombreF
            // 
            lblNombreF.AutoSize = true;
            tlpForm.SetColumnSpan(lblNombreF, 2);
            lblNombreF.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblNombreF.Location = new Point(0, 67);
            lblNombreF.Margin = new Padding(0, 0, 0, 2);
            lblNombreF.Name = "lblNombreF";
            lblNombreF.Size = new Size(53, 15);
            lblNombreF.TabIndex = 4;
            lblNombreF.Text = "Nombre";
            // 
            // txtNombreF
            // 
            txtNombreF.BackColor = Color.White;
            tlpForm.SetColumnSpan(txtNombreF, 2);
            txtNombreF.Dock = DockStyle.Fill;
            txtNombreF.FloatingLabelText = "";
            txtNombreF.Location = new Point(0, 84);
            txtNombreF.Margin = new Padding(0, 0, 0, 12);
            txtNombreF.MaxLength = 32767;
            txtNombreF.MinimumSize = new Size(0, 38);
            txtNombreF.Multiline = false;
            txtNombreF.Name = "txtNombreF";
            txtNombreF.Padding = new Padding(0, 0, 0, 4);
            txtNombreF.PasswordChar = '\0';
            txtNombreF.PlaceholderText = "";
            txtNombreF.ReadOnly = false;
            txtNombreF.SelectionStart = 0;
            txtNombreF.Size = new Size(786, 38);
            txtNombreF.TabIndex = 5;
            txtNombreF.UseFloatingLabel = false;
            // 
            // lblCategoriaF
            // 
            lblCategoriaF.AutoSize = true;
            lblCategoriaF.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblCategoriaF.Location = new Point(0, 134);
            lblCategoriaF.Margin = new Padding(0, 0, 12, 2);
            lblCategoriaF.Name = "lblCategoriaF";
            lblCategoriaF.Size = new Size(60, 15);
            lblCategoriaF.TabIndex = 6;
            lblCategoriaF.Text = "Categoría";
            // 
            // lblMarcaF
            // 
            lblMarcaF.AutoSize = true;
            lblMarcaF.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblMarcaF.Location = new Point(393, 134);
            lblMarcaF.Margin = new Padding(0, 0, 0, 2);
            lblMarcaF.Name = "lblMarcaF";
            lblMarcaF.Size = new Size(41, 15);
            lblMarcaF.TabIndex = 8;
            lblMarcaF.Text = "Marca";
            // 
            // cboCategoriaF
            // 
            cboCategoriaF.BackColor = Color.White;
            cboCategoriaF.DataSource = null;
            cboCategoriaF.DisplayMember = "";
            cboCategoriaF.Dock = DockStyle.Fill;
            cboCategoriaF.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCategoriaF.Location = new Point(0, 151);
            cboCategoriaF.Margin = new Padding(0, 0, 12, 12);
            cboCategoriaF.MinimumSize = new Size(0, 38);
            cboCategoriaF.Name = "cboCategoriaF";
            cboCategoriaF.Padding = new Padding(0, 0, 0, 4);
            cboCategoriaF.SelectedIndex = -1;
            cboCategoriaF.SelectedItem = null;
            cboCategoriaF.SelectedValue = null;
            cboCategoriaF.Size = new Size(381, 38);
            cboCategoriaF.TabIndex = 7;
            cboCategoriaF.ValueMember = "";
            // 
            // cboMarcaF
            // 
            cboMarcaF.BackColor = Color.White;
            cboMarcaF.DataSource = null;
            cboMarcaF.DisplayMember = "";
            cboMarcaF.Dock = DockStyle.Fill;
            cboMarcaF.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMarcaF.Location = new Point(393, 151);
            cboMarcaF.Margin = new Padding(0, 0, 0, 12);
            cboMarcaF.MinimumSize = new Size(0, 38);
            cboMarcaF.Name = "cboMarcaF";
            cboMarcaF.Padding = new Padding(0, 0, 0, 4);
            cboMarcaF.SelectedIndex = -1;
            cboMarcaF.SelectedItem = null;
            cboMarcaF.SelectedValue = null;
            cboMarcaF.Size = new Size(393, 38);
            cboMarcaF.TabIndex = 9;
            cboMarcaF.ValueMember = "";
            // 
            // lblStockInicialF
            // 
            lblStockInicialF.AutoSize = true;
            lblStockInicialF.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblStockInicialF.Location = new Point(0, 201);
            lblStockInicialF.Margin = new Padding(0, 0, 12, 2);
            lblStockInicialF.Name = "lblStockInicialF";
            lblStockInicialF.Size = new Size(104, 15);
            lblStockInicialF.TabIndex = 10;
            lblStockInicialF.Text = "Unidades iniciales";
            // 
            // nudStockInicial
            // 
            nudStockInicial.Location = new Point(0, 218);
            nudStockInicial.Margin = new Padding(0, 0, 12, 12);
            nudStockInicial.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            nudStockInicial.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudStockInicial.Name = "nudStockInicial";
            nudStockInicial.Size = new Size(120, 23);
            nudStockInicial.TabIndex = 11;
            nudStockInicial.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblCaracteristicasF
            // 
            lblCaracteristicasF.AutoSize = true;
            tlpForm.SetColumnSpan(lblCaracteristicasF, 2);
            lblCaracteristicasF.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblCaracteristicasF.Location = new Point(0, 253);
            lblCaracteristicasF.Margin = new Padding(0, 0, 0, 2);
            lblCaracteristicasF.Name = "lblCaracteristicasF";
            lblCaracteristicasF.Size = new Size(87, 15);
            lblCaracteristicasF.TabIndex = 12;
            lblCaracteristicasF.Text = "Características";
            // 
            // txtCaracteristicasF
            // 
            tlpForm.SetColumnSpan(txtCaracteristicasF, 2);
            txtCaracteristicasF.Dock = DockStyle.Fill;
            txtCaracteristicasF.Location = new Point(0, 270);
            txtCaracteristicasF.Margin = new Padding(0);
            txtCaracteristicasF.Multiline = true;
            txtCaracteristicasF.Name = "txtCaracteristicasF";
            txtCaracteristicasF.ScrollBars = ScrollBars.Vertical;
            txtCaracteristicasF.Size = new Size(786, 76);
            txtCaracteristicasF.TabIndex = 13;
            // 
            // pnlUnidadesHeader
            // 
            pnlUnidadesHeader.Controls.Add(lblUnidadesTitulo);
            pnlUnidadesHeader.Controls.Add(lnkVerTodas);
            pnlUnidadesHeader.Dock = DockStyle.Fill;
            pnlUnidadesHeader.Location = new Point(0, 524);
            pnlUnidadesHeader.Margin = new Padding(0, 0, 0, 6);
            pnlUnidadesHeader.Name = "pnlUnidadesHeader";
            pnlUnidadesHeader.Size = new Size(786, 30);
            pnlUnidadesHeader.TabIndex = 3;
            // 
            // lblUnidadesTitulo
            // 
            lblUnidadesTitulo.AutoSize = true;
            lblUnidadesTitulo.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            lblUnidadesTitulo.Location = new Point(0, 4);
            lblUnidadesTitulo.Name = "lblUnidadesTitulo";
            lblUnidadesTitulo.Size = new Size(123, 19);
            lblUnidadesTitulo.TabIndex = 0;
            lblUnidadesTitulo.Text = "Últimas unidades";
            // 
            // lnkVerTodas
            // 
            lnkVerTodas.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lnkVerTodas.Font = new Font("Segoe UI", 9.5F);
            lnkVerTodas.LinkBehavior = LinkBehavior.HoverUnderline;
            lnkVerTodas.Location = new Point(586, 4);
            lnkVerTodas.Name = "lnkVerTodas";
            lnkVerTodas.Size = new Size(200, 22);
            lnkVerTodas.TabIndex = 1;
            lnkVerTodas.TabStop = true;
            lnkVerTodas.Text = "Ver todas →";
            lnkVerTodas.TextAlign = ContentAlignment.MiddleRight;
            // 
            // dgvUnidadesMini
            // 
            dgvUnidadesMini.AllowUserToAddRows = false;
            dgvUnidadesMini.AllowUserToDeleteRows = false;
            dgvUnidadesMini.Dock = DockStyle.Fill;
            dgvUnidadesMini.Location = new Point(0, 560);
            dgvUnidadesMini.Margin = new Padding(0);
            dgvUnidadesMini.Name = "dgvUnidadesMini";
            dgvUnidadesMini.ReadOnly = true;
            dgvUnidadesMini.ScrollBars = ScrollBars.None;
            dgvUnidadesMini.Size = new Size(786, 226);
            dgvUnidadesMini.TabIndex = 4;
            // 
            // pnlDetalleFooter
            // 
            pnlDetalleFooter.Controls.Add(btnCancelarH);
            pnlDetalleFooter.Controls.Add(btnGuardarH);
            pnlDetalleFooter.Dock = DockStyle.Bottom;
            pnlDetalleFooter.Location = new Point(0, 564);
            pnlDetalleFooter.Name = "pnlDetalleFooter";
            pnlDetalleFooter.Size = new Size(851, 64);
            pnlDetalleFooter.TabIndex = 1;
            // 
            // btnCancelarH
            // 
            btnCancelarH.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCancelarH.CornerRadius = 8;
            btnCancelarH.FlatStyle = FlatStyle.Flat;
            btnCancelarH.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnCancelarH.Icon = null;
            btnCancelarH.IconColor = null;
            btnCancelarH.IconSize = 16;
            btnCancelarH.Location = new Point(517, 12);
            btnCancelarH.Name = "btnCancelarH";
            btnCancelarH.Size = new Size(130, 40);
            btnCancelarH.TabIndex = 0;
            btnCancelarH.Text = "Cancelar";
            btnCancelarH.Variant = PromacoHerra.Controls.MaterialButtonVariant.Secondary;
            // 
            // btnGuardarH
            // 
            btnGuardarH.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnGuardarH.CornerRadius = 8;
            btnGuardarH.FlatStyle = FlatStyle.Flat;
            btnGuardarH.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnGuardarH.Icon = FontAwesome.Sharp.IconChar.Save;
            btnGuardarH.IconColor = null;
            btnGuardarH.IconSize = 16;
            btnGuardarH.Location = new Point(657, 12);
            btnGuardarH.Name = "btnGuardarH";
            btnGuardarH.Size = new Size(170, 40);
            btnGuardarH.TabIndex = 1;
            btnGuardarH.Text = "Guardar cambios";
            btnGuardarH.Variant = PromacoHerra.Controls.MaterialButtonVariant.Primary;
            // 
            // lblSeleccionaHerramienta
            // 
            lblSeleccionaHerramienta.Dock = DockStyle.Fill;
            lblSeleccionaHerramienta.Font = new Font("Segoe UI", 11F);
            lblSeleccionaHerramienta.Location = new Point(0, 0);
            lblSeleccionaHerramienta.Name = "lblSeleccionaHerramienta";
            lblSeleccionaHerramienta.Size = new Size(851, 628);
            lblSeleccionaHerramienta.TabIndex = 0;
            lblSeleccionaHerramienta.Text = "Selecciona una herramienta";
            lblSeleccionaHerramienta.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tabCatalogos
            // 
            tabCatalogos.Controls.Add(tlpCatalogos);
            tabCatalogos.Location = new Point(4, 24);
            tabCatalogos.Name = "tabCatalogos";
            tabCatalogos.Padding = new Padding(20, 16, 20, 16);
            tabCatalogos.Size = new Size(1192, 628);
            tabCatalogos.TabIndex = 1;
            tabCatalogos.Text = "Catálogos";
            // 
            // tlpCatalogos
            // 
            tlpCatalogos.ColumnCount = 1;
            tlpCatalogos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpCatalogos.Controls.Add(segCatalogo, 0, 0);
            tlpCatalogos.Controls.Add(flpCatForm, 0, 1);
            tlpCatalogos.Controls.Add(dgvCatalogo, 0, 2);
            tlpCatalogos.Dock = DockStyle.Fill;
            tlpCatalogos.Location = new Point(20, 16);
            tlpCatalogos.Name = "tlpCatalogos";
            tlpCatalogos.RowCount = 3;
            tlpCatalogos.RowStyles.Add(new RowStyle());
            tlpCatalogos.RowStyles.Add(new RowStyle());
            tlpCatalogos.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpCatalogos.Size = new Size(1152, 596);
            tlpCatalogos.TabIndex = 0;
            // 
            // segCatalogo
            // 
            segCatalogo.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            segCatalogo.Location = new Point(0, 0);
            segCatalogo.Margin = new Padding(0, 0, 0, 12);
            segCatalogo.Name = "segCatalogo";
            segCatalogo.Size = new Size(440, 38);
            segCatalogo.TabIndex = 0;
            // 
            // flpCatForm
            // 
            flpCatForm.AutoSize = true;
            flpCatForm.Controls.Add(txtCatNombre);
            flpCatForm.Controls.Add(txtCatDescripcion);
            flpCatForm.Controls.Add(txtCatTelefono);
            flpCatForm.Controls.Add(btnCatAgregar);
            flpCatForm.Controls.Add(btnCatGuardar);
            flpCatForm.Controls.Add(btnCatEliminar);
            flpCatForm.Controls.Add(lnkCatCancelar);
            flpCatForm.Dock = DockStyle.Fill;
            flpCatForm.Location = new Point(0, 50);
            flpCatForm.Margin = new Padding(0, 0, 0, 12);
            flpCatForm.Name = "flpCatForm";
            flpCatForm.Size = new Size(1152, 38);
            flpCatForm.TabIndex = 1;
            flpCatForm.WrapContents = false;
            // 
            // txtCatNombre
            // 
            txtCatNombre.BackColor = Color.White;
            txtCatNombre.FloatingLabelText = "";
            txtCatNombre.Location = new Point(0, 0);
            txtCatNombre.Margin = new Padding(0, 0, 10, 0);
            txtCatNombre.MaxLength = 32767;
            txtCatNombre.MinimumSize = new Size(0, 38);
            txtCatNombre.Multiline = false;
            txtCatNombre.Name = "txtCatNombre";
            txtCatNombre.Padding = new Padding(0, 0, 0, 4);
            txtCatNombre.PasswordChar = '\0';
            txtCatNombre.PlaceholderText = "Nombre";
            txtCatNombre.ReadOnly = false;
            txtCatNombre.SelectionStart = 0;
            txtCatNombre.Size = new Size(160, 38);
            txtCatNombre.TabIndex = 0;
            txtCatNombre.UseFloatingLabel = false;
            // 
            // txtCatDescripcion
            // 
            txtCatDescripcion.BackColor = Color.White;
            txtCatDescripcion.FloatingLabelText = "";
            txtCatDescripcion.Location = new Point(170, 0);
            txtCatDescripcion.Margin = new Padding(0, 0, 10, 0);
            txtCatDescripcion.MaxLength = 32767;
            txtCatDescripcion.MinimumSize = new Size(0, 38);
            txtCatDescripcion.Multiline = false;
            txtCatDescripcion.Name = "txtCatDescripcion";
            txtCatDescripcion.Padding = new Padding(0, 0, 0, 4);
            txtCatDescripcion.PasswordChar = '\0';
            txtCatDescripcion.PlaceholderText = "Descripción";
            txtCatDescripcion.ReadOnly = false;
            txtCatDescripcion.SelectionStart = 0;
            txtCatDescripcion.Size = new Size(220, 38);
            txtCatDescripcion.TabIndex = 1;
            txtCatDescripcion.UseFloatingLabel = false;
            // 
            // txtCatTelefono
            // 
            txtCatTelefono.BackColor = Color.White;
            txtCatTelefono.FloatingLabelText = "";
            txtCatTelefono.Location = new Point(400, 0);
            txtCatTelefono.Margin = new Padding(0, 0, 10, 0);
            txtCatTelefono.MaxLength = 30;
            txtCatTelefono.MinimumSize = new Size(0, 38);
            txtCatTelefono.Multiline = false;
            txtCatTelefono.Name = "txtCatTelefono";
            txtCatTelefono.Padding = new Padding(0, 0, 0, 4);
            txtCatTelefono.PasswordChar = '\0';
            txtCatTelefono.PlaceholderText = "Teléfono";
            txtCatTelefono.ReadOnly = false;
            txtCatTelefono.SelectionStart = 0;
            txtCatTelefono.Size = new Size(150, 38);
            txtCatTelefono.TabIndex = 2;
            txtCatTelefono.UseFloatingLabel = false;
            txtCatTelefono.Visible = false;
            // 
            // btnCatAgregar
            // 
            btnCatAgregar.CornerRadius = 8;
            btnCatAgregar.FlatStyle = FlatStyle.Flat;
            btnCatAgregar.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnCatAgregar.Icon = FontAwesome.Sharp.IconChar.Add;
            btnCatAgregar.IconColor = null;
            btnCatAgregar.IconSize = 14;
            btnCatAgregar.Location = new Point(400, 0);
            btnCatAgregar.Margin = new Padding(0, 0, 8, 0);
            btnCatAgregar.Name = "btnCatAgregar";
            btnCatAgregar.Size = new Size(120, 38);
            btnCatAgregar.TabIndex = 2;
            btnCatAgregar.Text = "Agregar";
            btnCatAgregar.Variant = PromacoHerra.Controls.MaterialButtonVariant.Primary;
            // 
            // btnCatGuardar
            // 
            btnCatGuardar.CornerRadius = 8;
            btnCatGuardar.FlatStyle = FlatStyle.Flat;
            btnCatGuardar.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnCatGuardar.Icon = null;
            btnCatGuardar.IconColor = null;
            btnCatGuardar.IconSize = 16;
            btnCatGuardar.Location = new Point(528, 0);
            btnCatGuardar.Margin = new Padding(0, 0, 8, 0);
            btnCatGuardar.Name = "btnCatGuardar";
            btnCatGuardar.Size = new Size(110, 38);
            btnCatGuardar.TabIndex = 3;
            btnCatGuardar.Text = "Guardar";
            btnCatGuardar.Variant = PromacoHerra.Controls.MaterialButtonVariant.Default;
            btnCatGuardar.Visible = false;
            // 
            // btnCatEliminar
            // 
            btnCatEliminar.CornerRadius = 8;
            btnCatEliminar.FlatStyle = FlatStyle.Flat;
            btnCatEliminar.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnCatEliminar.Icon = null;
            btnCatEliminar.IconColor = null;
            btnCatEliminar.IconSize = 16;
            btnCatEliminar.Location = new Point(646, 0);
            btnCatEliminar.Margin = new Padding(0, 0, 8, 0);
            btnCatEliminar.Name = "btnCatEliminar";
            btnCatEliminar.Size = new Size(110, 38);
            btnCatEliminar.TabIndex = 4;
            btnCatEliminar.Text = "Eliminar";
            btnCatEliminar.Variant = PromacoHerra.Controls.MaterialButtonVariant.Danger;
            btnCatEliminar.Visible = false;
            // 
            // lnkCatCancelar
            // 
            lnkCatCancelar.AutoSize = true;
            lnkCatCancelar.Font = new Font("Segoe UI", 9.5F);
            lnkCatCancelar.LinkBehavior = LinkBehavior.HoverUnderline;
            lnkCatCancelar.Location = new Point(768, 10);
            lnkCatCancelar.Margin = new Padding(4, 10, 0, 0);
            lnkCatCancelar.Name = "lnkCatCancelar";
            lnkCatCancelar.Size = new Size(104, 17);
            lnkCatCancelar.TabIndex = 5;
            lnkCatCancelar.TabStop = true;
            lnkCatCancelar.Text = "Cancelar edición";
            lnkCatCancelar.Visible = false;
            // 
            // dgvCatalogo
            // 
            dgvCatalogo.AllowUserToAddRows = false;
            dgvCatalogo.AllowUserToDeleteRows = false;
            dgvCatalogo.Dock = DockStyle.Fill;
            dgvCatalogo.Location = new Point(0, 100);
            dgvCatalogo.Margin = new Padding(0);
            dgvCatalogo.Name = "dgvCatalogo";
            dgvCatalogo.ReadOnly = true;
            dgvCatalogo.Size = new Size(1152, 496);
            dgvCatalogo.TabIndex = 2;
            // 
            // tabMantenimiento
            // 
            tabMantenimiento.Controls.Add(tlpMant);
            tabMantenimiento.Location = new Point(4, 24);
            tabMantenimiento.Name = "tabMantenimiento";
            tabMantenimiento.Padding = new Padding(20, 16, 20, 16);
            tabMantenimiento.Size = new Size(1192, 628);
            tabMantenimiento.TabIndex = 2;
            tabMantenimiento.Text = "Mantenimiento";
            // 
            // tlpMant
            // 
            tlpMant.ColumnCount = 2;
            tlpMant.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62F));
            tlpMant.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38F));
            tlpMant.Controls.Add(flpMantTop, 0, 0);
            tlpMant.Controls.Add(btnNuevoMant, 1, 0);
            tlpMant.Controls.Add(dgvMant, 0, 1);
            tlpMant.Controls.Add(pnlPendientes, 1, 1);
            tlpMant.Controls.Add(pnlAccionesMant, 0, 2);
            tlpMant.SetColumnSpan(pnlAccionesMant, 2);
            tlpMant.Dock = DockStyle.Fill;
            tlpMant.Location = new Point(20, 16);
            tlpMant.Name = "tlpMant";
            tlpMant.RowCount = 3;
            tlpMant.RowStyles.Add(new RowStyle());
            tlpMant.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpMant.RowStyles.Add(new RowStyle(SizeType.Absolute, 68F));
            tlpMant.Size = new Size(1152, 596);
            tlpMant.TabIndex = 0;
            // 
            // flpMantTop
            // 
            flpMantTop.AutoSize = true;
            flpMantTop.Controls.Add(txtBuscarMant);
            flpMantTop.Controls.Add(chipMantActivos);
            flpMantTop.Dock = DockStyle.Fill;
            flpMantTop.Location = new Point(0, 0);
            flpMantTop.Margin = new Padding(0, 0, 0, 12);
            flpMantTop.Name = "flpMantTop";
            flpMantTop.Size = new Size(714, 38);
            flpMantTop.TabIndex = 0;
            flpMantTop.WrapContents = false;
            // 
            // txtBuscarMant
            // 
            txtBuscarMant.BackColor = Color.White;
            txtBuscarMant.FloatingLabelText = "";
            txtBuscarMant.Location = new Point(0, 0);
            txtBuscarMant.Margin = new Padding(0, 0, 12, 0);
            txtBuscarMant.MaxLength = 32767;
            txtBuscarMant.MinimumSize = new Size(0, 38);
            txtBuscarMant.Multiline = false;
            txtBuscarMant.Name = "txtBuscarMant";
            txtBuscarMant.Padding = new Padding(0, 0, 0, 4);
            txtBuscarMant.PasswordChar = '\0';
            txtBuscarMant.PlaceholderText = "Buscar por herramienta, código, responsable…";
            txtBuscarMant.ReadOnly = false;
            txtBuscarMant.SelectionStart = 0;
            txtBuscarMant.Size = new Size(340, 38);
            txtBuscarMant.TabIndex = 0;
            txtBuscarMant.UseFloatingLabel = false;
            // 
            // chipMantActivos
            // 
            chipMantActivos.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            chipMantActivos.Location = new Point(352, 7);
            chipMantActivos.Margin = new Padding(0, 7, 0, 0);
            chipMantActivos.Name = "chipMantActivos";
            chipMantActivos.Size = new Size(69, 24);
            chipMantActivos.TabIndex = 1;
            chipMantActivos.Text = "0 activos";
            // 
            // btnNuevoMant
            // 
            btnNuevoMant.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNuevoMant.CornerRadius = 8;
            btnNuevoMant.FlatStyle = FlatStyle.Flat;
            btnNuevoMant.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnNuevoMant.Icon = FontAwesome.Sharp.IconChar.Add;
            btnNuevoMant.IconColor = null;
            btnNuevoMant.IconSize = 14;
            btnNuevoMant.Location = new Point(942, 0);
            btnNuevoMant.Margin = new Padding(0, 0, 0, 12);
            btnNuevoMant.Name = "btnNuevoMant";
            btnNuevoMant.Size = new Size(210, 38);
            btnNuevoMant.TabIndex = 1;
            btnNuevoMant.Text = "Nuevo mantenimiento";
            btnNuevoMant.Variant = PromacoHerra.Controls.MaterialButtonVariant.Primary;
            // 
            // dgvMant
            // 
            dgvMant.AllowUserToAddRows = false;
            dgvMant.AllowUserToDeleteRows = false;
            dgvMant.Dock = DockStyle.Fill;
            dgvMant.Location = new Point(0, 50);
            dgvMant.Margin = new Padding(0, 0, 12, 12);
            dgvMant.Name = "dgvMant";
            dgvMant.ReadOnly = true;
            dgvMant.Size = new Size(702, 466);
            dgvMant.TabIndex = 2;
            // 
            // pnlPendientes
            // 
            pnlPendientes.BackColor = Color.White;
            pnlPendientes.Controls.Add(dgvPendientes);
            pnlPendientes.Controls.Add(lblPendientesVacio);
            pnlPendientes.Controls.Add(pnlPendientesHeader);
            pnlPendientes.Dock = DockStyle.Fill;
            pnlPendientes.Location = new Point(714, 50);
            pnlPendientes.Margin = new Padding(0, 0, 0, 12);
            pnlPendientes.Name = "pnlPendientes";
            pnlPendientes.Padding = new Padding(14, 8, 14, 14);
            pnlPendientes.Size = new Size(438, 466);
            pnlPendientes.TabIndex = 3;
            // 
            // pnlPendientesHeader
            // 
            pnlPendientesHeader.Controls.Add(chipPendientes);
            pnlPendientesHeader.Controls.Add(lblPendientesTitulo);
            pnlPendientesHeader.Dock = DockStyle.Top;
            pnlPendientesHeader.Location = new Point(14, 8);
            pnlPendientesHeader.Name = "pnlPendientesHeader";
            pnlPendientesHeader.Size = new Size(410, 40);
            pnlPendientesHeader.TabIndex = 0;
            // 
            // lblPendientesTitulo
            // 
            lblPendientesTitulo.AutoSize = true;
            lblPendientesTitulo.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblPendientesTitulo.Location = new Point(0, 8);
            lblPendientesTitulo.Name = "lblPendientesTitulo";
            lblPendientesTitulo.Size = new Size(186, 20);
            lblPendientesTitulo.TabIndex = 0;
            lblPendientesTitulo.Text = "Pendientes de reparación";
            // 
            // chipPendientes
            // 
            chipPendientes.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            chipPendientes.Location = new Point(200, 8);
            chipPendientes.Name = "chipPendientes";
            chipPendientes.Size = new Size(30, 24);
            chipPendientes.TabIndex = 1;
            chipPendientes.Text = "0";
            // 
            // lblPendientesVacio
            // 
            lblPendientesVacio.Dock = DockStyle.Fill;
            lblPendientesVacio.Font = new Font("Segoe UI", 9.5F);
            lblPendientesVacio.Location = new Point(14, 48);
            lblPendientesVacio.Name = "lblPendientesVacio";
            lblPendientesVacio.Size = new Size(410, 404);
            lblPendientesVacio.TabIndex = 1;
            lblPendientesVacio.Text = "No hay unidades dañadas esperando reparación.";
            lblPendientesVacio.TextAlign = ContentAlignment.MiddleCenter;
            lblPendientesVacio.Visible = false;
            // 
            // dgvPendientes
            // 
            dgvPendientes.AllowUserToAddRows = false;
            dgvPendientes.AllowUserToDeleteRows = false;
            dgvPendientes.Dock = DockStyle.Fill;
            dgvPendientes.Location = new Point(14, 48);
            dgvPendientes.Name = "dgvPendientes";
            dgvPendientes.ReadOnly = true;
            dgvPendientes.Size = new Size(410, 404);
            dgvPendientes.TabIndex = 2;
            // 
            // pnlAccionesMant
            // 
            pnlAccionesMant.BackColor = Color.White;
            pnlAccionesMant.Controls.Add(lblAccionInfo);
            pnlAccionesMant.Controls.Add(flpAccionesMant);
            pnlAccionesMant.Dock = DockStyle.Fill;
            pnlAccionesMant.Location = new Point(0, 528);
            pnlAccionesMant.Margin = new Padding(0);
            pnlAccionesMant.Name = "pnlAccionesMant";
            pnlAccionesMant.Padding = new Padding(18, 14, 14, 14);
            pnlAccionesMant.Size = new Size(1152, 68);
            pnlAccionesMant.TabIndex = 4;
            // 
            // lblAccionInfo
            // 
            lblAccionInfo.AutoEllipsis = true;
            lblAccionInfo.Dock = DockStyle.Fill;
            lblAccionInfo.Font = new Font("Segoe UI", 9.5F);
            lblAccionInfo.Location = new Point(18, 14);
            lblAccionInfo.Name = "lblAccionInfo";
            lblAccionInfo.Size = new Size(780, 40);
            lblAccionInfo.TabIndex = 0;
            lblAccionInfo.Text = "Seleccione un mantenimiento para cerrarlo, o una unidad pendiente para enviarla a reparación.";
            lblAccionInfo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // flpAccionesMant
            // 
            flpAccionesMant.AutoSize = true;
            flpAccionesMant.Controls.Add(btnAccionMant);
            flpAccionesMant.Controls.Add(lnkHistorial);
            flpAccionesMant.Dock = DockStyle.Right;
            flpAccionesMant.FlowDirection = FlowDirection.RightToLeft;
            flpAccionesMant.Location = new Point(798, 14);
            flpAccionesMant.Name = "flpAccionesMant";
            flpAccionesMant.Size = new Size(340, 40);
            flpAccionesMant.TabIndex = 1;
            flpAccionesMant.WrapContents = false;
            // 
            // btnAccionMant
            // 
            btnAccionMant.CornerRadius = 8;
            btnAccionMant.FlatStyle = FlatStyle.Flat;
            btnAccionMant.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnAccionMant.Icon = FontAwesome.Sharp.IconChar.CheckCircle;
            btnAccionMant.IconColor = null;
            btnAccionMant.IconSize = 16;
            btnAccionMant.Location = new Point(130, 0);
            btnAccionMant.Margin = new Padding(0);
            btnAccionMant.Name = "btnAccionMant";
            btnAccionMant.Size = new Size(210, 40);
            btnAccionMant.TabIndex = 1;
            btnAccionMant.Text = "Cerrar mantenimiento";
            btnAccionMant.Variant = PromacoHerra.Controls.MaterialButtonVariant.Primary;
            btnAccionMant.Visible = false;
            // 
            // lnkHistorial
            // 
            lnkHistorial.AutoSize = true;
            lnkHistorial.Font = new Font("Segoe UI", 9.5F);
            lnkHistorial.LinkBehavior = LinkBehavior.HoverUnderline;
            lnkHistorial.Location = new Point(28, 11);
            lnkHistorial.Margin = new Padding(0, 11, 16, 0);
            lnkHistorial.Name = "lnkHistorial";
            lnkHistorial.Size = new Size(86, 17);
            lnkHistorial.TabIndex = 0;
            lnkHistorial.TabStop = true;
            lnkHistorial.Text = "Ver historial";
            lnkHistorial.Visible = false;
            // 
            // FrmHerramientas
            // 
            ClientSize = new Size(1200, 720);
            Controls.Add(tabMain);
            Controls.Add(pnlHeader);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FrmHerramientas";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Herramientas";
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            flpHeaderAcciones.ResumeLayout(false);
            tabMain.ResumeLayout(false);
            tabHerramientas.ResumeLayout(false);
            splitHerramientas.Panel1.ResumeLayout(false);
            splitHerramientas.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitHerramientas).EndInit();
            splitHerramientas.ResumeLayout(false);
            tlpLista.ResumeLayout(false);
            tlpLista.PerformLayout();
            pnlBuscarH.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)icoBuscarH).EndInit();
            pnlDetalle.ResumeLayout(false);
            pnlDetalleScroll.ResumeLayout(false);
            pnlDetalleScroll.PerformLayout();
            tlpDetalle.ResumeLayout(false);
            tlpDetalle.PerformLayout();
            pnlDetHeader.ResumeLayout(false);
            tlpTiles.ResumeLayout(false);
            tlpForm.ResumeLayout(false);
            tlpForm.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudStockInicial).EndInit();
            pnlUnidadesHeader.ResumeLayout(false);
            pnlUnidadesHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUnidadesMini).EndInit();
            pnlDetalleFooter.ResumeLayout(false);
            tabCatalogos.ResumeLayout(false);
            tlpCatalogos.ResumeLayout(false);
            tlpCatalogos.PerformLayout();
            flpCatForm.ResumeLayout(false);
            flpCatForm.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCatalogo).EndInit();
            tabMantenimiento.ResumeLayout(false);
            tlpMant.ResumeLayout(false);
            tlpMant.PerformLayout();
            flpMantTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvMant).EndInit();
            pnlPendientes.ResumeLayout(false);
            pnlPendientesHeader.ResumeLayout(false);
            pnlPendientesHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPendientes).EndInit();
            pnlAccionesMant.ResumeLayout(false);
            pnlAccionesMant.PerformLayout();
            flpAccionesMant.ResumeLayout(false);
            flpAccionesMant.PerformLayout();
            ResumeLayout(false);
        }
    }
}
