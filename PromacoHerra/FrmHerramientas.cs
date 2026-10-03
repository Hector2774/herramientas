using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using PromacoHerra.Controls;
using PromacoHerra.Models;
using PromacoHerra.Services;

namespace PromacoHerra
{
    // Administración de herramientas en 3 pestañas:
    //   Herramientas   → lista (búsqueda + chips de categoría) | detalle editable con stock y últimas unidades
    //   Catálogos      → categorías, marcas, ubicaciones y proveedores en la misma grilla (control segmentado)
    //   Mantenimiento  → mantenimientos activos | pendientes de reparación (unidades dañadas)
    //                    + barra de acción para la fila seleccionada (cerrar / enviar a mantenimiento)
    public partial class FrmHerramientas : Form
    {
        private const string SinCategoria = "Sin categoría";
        private static readonly Font FuenteCodigo = new("Consolas", 9F);

        // Herramientas
        private List<HerramientaResumen> _herramientas = new();
        private HerramientaResumen? _actual;          // herramienta mostrada en el detalle
        private bool _modoNuevo;
        private string? _categoriaFiltro;             // null = Todas

        // Catálogos
        private enum TipoCatalogo { Categorias, Marcas, Ubicaciones, Proveedores }   // mismo orden que segCatalogo.Opciones
        private TipoCatalogo CatalogoActual => (TipoCatalogo)segCatalogo.Seleccionado;
        private int? _catalogoId;

        // Mantenimiento
        private DataView? _mantView;
        private DataRow? _mantSeleccionado;          // mantenimiento activo elegido en la grilla
        private DataRow? _pendienteSeleccionado;     // unidad dañada elegida en la bandeja

        // Columnas de las grillas. Se crean en código y no en el Designer: el diseñador de
        // Visual Studio las eliminaba al volver a guardar el formulario.
        private readonly DataGridViewTextBoxColumn colUCodigo = Columna("Codigo", "Código unidad", 25);
        private readonly DataGridViewTextBoxColumn colUEstado = Columna("Estado", "Estado", 25);
        private readonly DataGridViewTextBoxColumn colUPrestadaA = Columna("PrestadaA", "Prestada a", 30);
        private readonly DataGridViewTextBoxColumn colUDevolucion = Columna("Devolucion", "Devolución", 20);
        private readonly DataGridViewTextBoxColumn colCatNombre = Columna("Nombre", "Nombre", 30);
        private readonly DataGridViewTextBoxColumn colCatDescripcion = Columna("Descripcion", "Descripción", 55);
        private readonly DataGridViewTextBoxColumn colCatTelefono = Columna("Telefono", "Teléfono", 18);
        private readonly DataGridViewTextBoxColumn colCatHerramientas = Columna("Herramientas", "Herramientas", 15);
        private readonly DataGridViewTextBoxColumn colMHerramienta = Columna("Herramienta", "Herramienta", 28);
        private readonly DataGridViewTextBoxColumn colMTipo = Columna("TipoMantenimiento", "Tipo", 15);
        private readonly DataGridViewTextBoxColumn colMRealizadoPor = Columna("RealizadoPor", "Responsable", 22);
        private readonly DataGridViewTextBoxColumn colMDias = Columna("DiasEnMantenimiento", "Días activo", 13);
        private readonly DataGridViewTextBoxColumn colMDescripcion = Columna("Descripcion", "Descripción", 22);
        private readonly DataGridViewTextBoxColumn colPUnidad = Columna("Herramienta", "Unidad", 58);
        private readonly DataGridViewTextBoxColumn colPReporte = Columna("ReportadoPor", "Reportada", 42);

        // Foto del detalle: columna derecha de tlpForm (también se crea en código)
        private const int LadoFoto = 200;
        private readonly PictureBox picFoto = new()
        {
            Name = "picFoto",
            Size = new Size(LadoFoto, LadoFoto),
            SizeMode = PictureBoxSizeMode.Zoom,
            BackColor = Color.FromArgb(240, 242, 246)
        };
        private readonly MaterialButton btnCambiarFoto = new()
        {
            Name = "btnCambiarFoto",
            Text = "Cambiar foto",
            Icon = FontAwesome.Sharp.IconChar.Camera,
            IconSize = 14,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            CornerRadius = 8,
            Size = new Size(LadoFoto, 34)
        };

        public FrmHerramientas()
        {
            InitializeComponent();
            ConfigurarColumnas();
            ConstruirPanelFoto();
            ThemeManager.ApplyTheme(this);
            AplicarEstilos();

            this.Load += FrmHerramientas_Load;
            tabMain.SelectedIndexChanged += (s, e) => flpHeaderAcciones.Visible = tabMain.SelectedTab == tabHerramientas;

            // Herramientas
            btnNuevaHerramienta.Click += (s, e) => NuevaHerramienta();
            btnDarDeBaja.Click += btnDarDeBaja_Click;
            txtBuscarH.TextChanged += (s, e) => AplicarFiltroLista(_actual?.HerramientaId);
            lstHerramientas.DrawItem += lstHerramientas_DrawItem;
            lstHerramientas.SelectedIndexChanged += lstHerramientas_SelectedIndexChanged;
            btnGuardarH.Click += btnGuardarH_Click;
            btnCancelarH.Click += btnCancelarH_Click;
            btnUnidades.Click += (s, e) => AbrirUnidades();
            btnCambiarFoto.Click += btnCambiarFoto_Click;
            lnkVerTodas.LinkClicked += (s, e) => AbrirUnidades();
            dgvUnidadesMini.CellPainting += dgvUnidadesMini_CellPainting;
            pnlDetalleFooter.Paint += (s, e) => LineaSuperior(e, pnlDetalleFooter);

            // Catálogos
            segCatalogo.SeleccionCambiada += (s, e) => CargarCatalogo();
            dgvCatalogo.CellClick += dgvCatalogo_CellClick;
            dgvCatalogo.CellPainting += dgvCatalogo_CellPainting;
            btnCatAgregar.Click += btnCatAgregar_Click;
            btnCatGuardar.Click += btnCatGuardar_Click;
            btnCatEliminar.Click += btnCatEliminar_Click;
            lnkCatCancelar.LinkClicked += (s, e) => LimpiarFormCatalogo();

            // Mantenimiento
            txtBuscarMant.TextChanged += (s, e) => FiltrarMantenimientos();
            dgvMant.CellClick += dgvMant_CellClick;
            dgvMant.CellPainting += dgvMant_CellPainting;
            dgvMant.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) EjecutarAccionMantenimiento(); };
            dgvPendientes.CellClick += dgvPendientes_CellClick;
            dgvPendientes.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) EjecutarAccionMantenimiento(); };
            dgvPendientes.CellPainting += dgvPendientes_CellPainting;
            btnNuevoMant.Click += (s, e) => AbrirMantenimiento(null);
            btnAccionMant.Click += (s, e) => EjecutarAccionMantenimiento();
            lnkHistorial.LinkClicked += (s, e) => VerHistorial();

            // Lo alineado a la derecha se ubica al cambiar de tamaño y no con Anchor: el detalle
            // arranca oculto y, al mostrarse, los controles anclados quedaban fuera de la vista.
            pnlDetHeader.Resize += (s, e) => AlinearDetalle();
            pnlUnidadesHeader.Resize += (s, e) => AlinearDetalle();
            pnlDetalleFooter.Resize += (s, e) => AlinearDetalle();
        }

        private static DataGridViewTextBoxColumn Columna(string propiedad, string encabezado, float peso) => new()
        {
            DataPropertyName = propiedad,
            HeaderText = encabezado,
            Name = "col" + propiedad,
            FillWeight = peso,
            ReadOnly = true
        };

        private void ConfigurarColumnas()
        {
            foreach (var (dgv, columnas) in new[]
            {
                (dgvUnidadesMini, new[] { colUCodigo, colUEstado, colUPrestadaA, colUDevolucion }),
                (dgvCatalogo, new[] { colCatNombre, colCatDescripcion, colCatTelefono, colCatHerramientas }),
                (dgvMant, new[] { colMHerramienta, colMTipo, colMRealizadoPor, colMDias, colMDescripcion }),
                (dgvPendientes, new[] { colPUnidad, colPReporte })
            })
            {
                dgv.AutoGenerateColumns = false;
                dgv.Columns.Clear();
                dgv.Columns.AddRange(columnas);
            }
        }

        // Foto + "Cambiar foto" a la derecha de los campos, ocupando todas las filas del formulario
        private void ConstruirPanelFoto()
        {
            using (var path = RoundedGeometry.RoundedRect(new Rectangle(0, 0, LadoFoto, LadoFoto), 10))
                picFoto.Region = new Region(path);
            btnCambiarFoto.Location = new Point(0, LadoFoto + 8);

            var pnlFoto = new Panel
            {
                Name = "pnlFoto",
                Size = new Size(LadoFoto, btnCambiarFoto.Bottom),
                Margin = new Padding(20, 0, 0, 12),
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            pnlFoto.Controls.Add(picFoto);
            pnlFoto.Controls.Add(btnCambiarFoto);

            tlpForm.SuspendLayout();
            tlpForm.ColumnCount = 3;
            tlpForm.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, LadoFoto + pnlFoto.Margin.Horizontal));
            tlpForm.Controls.Add(pnlFoto, 2, 0);
            tlpForm.SetRowSpan(pnlFoto, tlpForm.RowCount);
            tlpForm.ResumeLayout();
        }

        private void AlinearDetalle()
        {
            foreach (var c in new Control[] { btnUnidades, lnkVerTodas, btnGuardarH, btnCancelarH, lblNombreDet, lblCodigoDet })
                c.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            btnUnidades.Location = new Point(pnlDetHeader.Width - btnUnidades.Width, 4);
            int anchoTitulo = Math.Max(100, (btnUnidades.Visible ? btnUnidades.Left - 12 : pnlDetHeader.Width));
            lblNombreDet.SetBounds(0, 0, anchoTitulo, 32);
            lblCodigoDet.SetBounds(0, 34, anchoTitulo, 22);

            lnkVerTodas.Location = new Point(pnlUnidadesHeader.Width - lnkVerTodas.Width, 4);

            btnGuardarH.Location = new Point(pnlDetalleFooter.Width - 24 - btnGuardarH.Width, 12);
            btnCancelarH.Location = new Point(btnGuardarH.Left - 10 - btnCancelarH.Width, 12);
        }

        // ThemeManager fija colores genéricos; aquí se ajusta lo propio de este diseño
        private void AplicarEstilos()
        {
            splitHerramientas.Panel1.BackColor = ThemeManager.CardBackground;
            splitHerramientas.Panel2.BackColor = ThemeManager.AppBackground;
            splitHerramientas.BackColor = ThemeManager.BorderColor;   // color de la línea divisoria
            tlpLista.BackColor = ThemeManager.CardBackground;
            lstHerramientas.BackColor = ThemeManager.CardBackground;
            pnlDetalleFooter.BackColor = ThemeManager.CardBackground;
            icoBuscarH.BackColor = ThemeManager.CardBackground;
            icoBuscarH.IconColor = ThemeManager.TextSecondary;

            foreach (var lbl in new[] { lblListaVacia, lblSeleccionaHerramienta, lblCodigoDet, lblAccionInfo, lblPendientesVacio })
                lbl.ForeColor = ThemeManager.TextSecondary;
            foreach (var lbl in new[] { lblCodigoF, lblUbicacionF, lblNombreF, lblCategoriaF, lblMarcaF, lblStockInicialF,
                                        lblCaracteristicasF })
                lbl.ForeColor = ThemeManager.TextSecondary;
            foreach (var lnk in new[] { lnkVerTodas, lnkCatCancelar, lnkHistorial })
                lnk.LinkColor = lnk.ActiveLinkColor = ThemeManager.AccentBlue;

            txtCaracteristicasF.BorderStyle = BorderStyle.FixedSingle;

            tileTotal.ColorValor = ThemeManager.TextPrimary;
            tileDisponibles.ColorValor = Paleta.Verde;
            tilePrestadas.ColorValor = Paleta.Amarillo;
            tileMantenimiento.ColorValor = Paleta.Naranja;
            tileDañadas.ColorValor = Paleta.Rojo;

            segCatalogo.Opciones = new[] { "Categorías", "Marcas", "Ubicaciones", "Proveedores" };
            chipMantActivos.Fondo = Paleta.NaranjaSuave;
            chipMantActivos.Frente = Paleta.Naranja;
            chipPendientes.Fondo = Paleta.RojoSuave;
            chipPendientes.Frente = Paleta.Rojo;

            // Variantes que la heurística de ThemeManager (por texto) no adivina
            btnNuevaHerramienta.Variant = MaterialButtonVariant.Primary;
            btnDarDeBaja.Variant = MaterialButtonVariant.Default;
            btnCambiarFoto.Variant = MaterialButtonVariant.Default;
            btnCatAgregar.Variant = MaterialButtonVariant.Primary;
            btnAccionMant.Variant = MaterialButtonVariant.Primary;
            btnNuevoMant.Variant = MaterialButtonVariant.Primary;
            pnlAccionesMant.CornerRadius = 12;
            pnlPendientes.CornerRadius = 12;
            pnlPendientesHeader.BackColor = ThemeManager.CardBackground;

            EstilizarGrid(dgvUnidadesMini, 38);
            EstilizarGrid(dgvCatalogo, 42);
            EstilizarGrid(dgvMant, 52);
            EstilizarGrid(dgvPendientes, 52);
            colUCodigo.DefaultCellStyle.Font = FuenteCodigo;
        }

        // Grilla clara: encabezado blanco con texto gris, filas de alto fijo, sin filas alternas
        private static void EstilizarGrid(DataGridView dgv, int altoFila)
        {
            dgv.BackgroundColor = ThemeManager.CardBackground;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = ThemeManager.CardBackground;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = ThemeManager.TextSecondary;
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = ThemeManager.CardBackground;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.ColumnHeadersHeight = 38;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = ThemeManager.CardBackground;
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgv.RowTemplate.Height = altoFila;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.MultiSelect = false;
        }

        private static void LineaSuperior(PaintEventArgs e, Control c)
        {
            using var pen = new Pen(ThemeManager.BorderColor);
            e.Graphics.DrawLine(pen, 0, 0, c.Width, 0);
        }

        // ══════════════════════════════════════════════════════════
        // CARGA INICIAL
        // ══════════════════════════════════════════════════════════
        private void FrmHerramientas_Load(object? sender, EventArgs e)
        {
            splitHerramientas.SplitterDistance = 340;

            CargarCombos();
            CargarHerramientas(null);
            CargarCatalogo();
            CargarMantenimientos();

            BeginInvoke(new Action(() => ActiveControl = null));   // que se vean los placeholders
        }

        private void CargarCombos()
        {
            try
            {
                var categorias = CategoriaService.ObtenerTodas();
                CargarCombo(cboCategoriaF, categorias, "Nombre", "CategoriaId");
                CargarCombo(cboMarcaF, MarcaService.ObtenerTodas(), "Nombre", "MarcaId");
                CargarCombo(cboUbicacionF, UbicacionService.ObtenerTodas(), "Nombre", "UbicacionId");
                ConstruirChips(categorias);
            }
            catch (Exception ex) { Error(Errores.Mensaje(ex)); }
        }

        private static void CargarCombo(MaterialComboBox cbo, DataTable dt, string display, string value)
        {
            cbo.DataSource = dt;
            cbo.DisplayMember = display;
            cbo.ValueMember = value;
            cbo.SelectedIndex = -1;
        }

        private static void SeleccionarEnCombo(MaterialComboBox cbo, int? id)
        {
            if (id == null) { cbo.SelectedIndex = -1; return; }
            cbo.SelectedValue = id.Value;
            if (!Equals(cbo.SelectedValue, id.Value)) cbo.SelectedIndex = -1;
        }

        // ══════════════════════════════════════════════════════════
        // HERRAMIENTAS — lista
        // ══════════════════════════════════════════════════════════
        private void CargarHerramientas(int? seleccionarId)
        {
            try { _herramientas = HerramientaService.Listar(); }
            catch (Exception ex) { Error(Errores.Mensaje(ex)); _herramientas = new(); }

            if (seleccionarId == null) MostrarVacio();
            AplicarFiltroLista(seleccionarId, refrescarDetalle: true);
        }

        private void ConstruirChips(DataTable categorias)
        {
            var nombres = categorias.AsEnumerable().Select(r => r.Field<string>("Nombre") ?? "").ToList();
            if (_categoriaFiltro != null && _categoriaFiltro != SinCategoria && !nombres.Contains(_categoriaFiltro))
                _categoriaFiltro = null;

            flpChips.SuspendLayout();
            foreach (Control c in flpChips.Controls.Cast<Control>().ToList()) c.Dispose();
            flpChips.Controls.Clear();

            foreach (var (texto, cat) in new[] { ("Todas", (string?)null) }.Concat(nombres.Select(n => (n, (string?)n))))
            {
                var chip = new ChipToggle(texto, cat) { Seleccionado = cat == _categoriaFiltro };
                chip.Click += (s, e) =>
                {
                    _categoriaFiltro = ((ChipToggle)s!).Categoria;
                    foreach (ChipToggle c in flpChips.Controls) c.Seleccionado = c.Categoria == _categoriaFiltro;
                    AplicarFiltroLista(_actual?.HerramientaId);
                };
                flpChips.Controls.Add(chip);
            }
            flpChips.ResumeLayout();
        }

        // refrescarDetalle: solo cuando los datos vienen recién leídos de la BD. Al buscar o filtrar
        // no se toca el detalle, para no perder cambios sin guardar.
        private void AplicarFiltroLista(int? seleccionarId, bool refrescarDetalle = false)
        {
            string termino = txtBuscarH.Text.Trim();
            var visibles = _herramientas.Where(h =>
                (_categoriaFiltro == null || string.Equals(h.Categoria, _categoriaFiltro, StringComparison.OrdinalIgnoreCase)) &&
                (termino == "" || Contiene(h.Nombre, termino) || Contiene(h.Codigo, termino) ||
                 Contiene(h.Categoria, termino) || Contiene(h.Marca, termino) || Contiene(h.Ubicacion, termino)))
                .ToList();

            lstHerramientas.BeginUpdate();
            lstHerramientas.SelectedIndexChanged -= lstHerramientas_SelectedIndexChanged;
            lstHerramientas.Items.Clear();
            lstHerramientas.Items.AddRange(visibles.Cast<object>().ToArray());
            int idx = visibles.FindIndex(h => h.HerramientaId == seleccionarId);
            lstHerramientas.SelectedIndex = idx;
            lstHerramientas.SelectedIndexChanged += lstHerramientas_SelectedIndexChanged;
            lstHerramientas.EndUpdate();

            lblListaVacia.Visible = visibles.Count == 0;

            // Tras recargar, mostrar la herramienta pedida con sus datos nuevos
            if (refrescarDetalle && seleccionarId != null && !_modoNuevo)
            {
                var h = _herramientas.FirstOrDefault(x => x.HerramientaId == seleccionarId);
                if (h != null) MostrarHerramienta(h);
                else MostrarVacio();
            }
        }

        private static bool Contiene(string texto, string termino) =>
            !string.IsNullOrEmpty(texto) &&
            CultureInfo.InvariantCulture.CompareInfo.IndexOf(texto, termino,
                CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0;

        private void lstHerramientas_DrawItem(object? sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || lstHerramientas.Items[e.Index] is not HerramientaResumen h) return;
            HerramientaListItem.Dibujar(e.Graphics, e.Bounds, h, (e.State & DrawItemState.Selected) != 0);
        }

        private void lstHerramientas_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (lstHerramientas.SelectedItem is HerramientaResumen h) MostrarHerramienta(h);
        }

        // ══════════════════════════════════════════════════════════
        // HERRAMIENTAS — detalle
        // ══════════════════════════════════════════════════════════
        private void MostrarVacio()
        {
            _actual = null;
            _modoNuevo = false;
            pnlDetalle.Visible = false;
            lblSeleccionaHerramienta.Visible = true;
            btnDarDeBaja.Enabled = false;
        }

        private void MostrarHerramienta(HerramientaResumen h)
        {
            _actual = h;
            _modoNuevo = false;

            lblNombreDet.Text = h.Nombre;
            lblCodigoDet.Text = string.Join("  ·  ", new[] { h.Codigo, h.Categoria, h.Marca,
                h.PrestamoHabilitado ? "" : "Préstamo suspendido" }.Where(s => !string.IsNullOrEmpty(s)));

            tileTotal.Valor = h.StockTotal;
            tileDisponibles.Valor = h.StockDisponible;
            tilePrestadas.Valor = h.StockPrestado;
            tileMantenimiento.Valor = h.StockMantenimiento;
            tileDañadas.Valor = h.StockDañado;

            txtCodigoF.Text = h.Codigo;
            txtNombreF.Text = h.Nombre;
            txtCaracteristicasF.Text = h.Caracteristicas;
            SeleccionarEnCombo(cboCategoriaF, h.CategoriaId);
            SeleccionarEnCombo(cboMarcaF, h.MarcaId);
            SeleccionarEnCombo(cboUbicacionF, h.UbicacionId);
            ImagenHelper.MostrarEn(picFoto, h.FotoNombre);

            ModoFormulario(nuevo: false);
            CargarUnidadesMini(h.HerramientaId);
        }

        private void NuevaHerramienta()
        {
            lstHerramientas.ClearSelected();
            _actual = null;
            _modoNuevo = true;

            lblNombreDet.Text = "Nueva herramienta";
            lblCodigoDet.Text = "El código se genera automáticamente al guardar";
            txtCodigoF.Text = "(automático)";
            txtNombreF.Clear();
            txtCaracteristicasF.Clear();
            cboCategoriaF.SelectedIndex = -1;
            cboMarcaF.SelectedIndex = -1;
            cboUbicacionF.SelectedIndex = -1;
            nudStockInicial.Value = 1;
            ImagenHelper.MostrarEn(picFoto, null);

            ModoFormulario(nuevo: true);
            txtNombreF.Focus();
        }

        // Nueva: sin stock, unidades ni "Unidades / Stock"; con "Unidades iniciales"
        private void ModoFormulario(bool nuevo)
        {
            tlpDetalle.SuspendLayout();
            tlpTiles.Visible = !nuevo;
            btnUnidades.Visible = !nuevo;
            pnlUnidadesHeader.Visible = !nuevo;
            dgvUnidadesMini.Visible = !nuevo;
            lblStockInicialF.Visible = nuevo;
            nudStockInicial.Visible = nuevo;
            btnGuardarH.Text = nuevo ? "Crear herramienta" : "Guardar cambios";
            btnCambiarFoto.Enabled = !nuevo;   // el archivo se nombra con el código, que aún no existe
            tlpDetalle.ResumeLayout();

            btnDarDeBaja.Enabled = !nuevo;
            lblSeleccionaHerramienta.Visible = false;
            pnlDetalle.Visible = true;
            pnlDetalleScroll.AutoScrollPosition = Point.Empty;
            AlinearDetalle();
        }

        // Últimas 5 unidades (las más recientes que siguen en inventario)
        private void CargarUnidadesMini(int herramientaId)
        {
            var dt = new DataTable();
            dt.Columns.Add("Codigo");
            dt.Columns.Add("Estado");
            dt.Columns.Add("PrestadaA");
            dt.Columns.Add("Devolucion");

            int total = 0;
            try
            {
                var unidades = HerramientaService.ObtenerUnidades(herramientaId).AsEnumerable()
                    .Where(r => r.Field<string>("Estado") != "Baja").ToList();
                total = unidades.Count;

                foreach (var r in unidades.OrderByDescending(r => r.Field<DateTime>("FechaAlta"))
                                          .ThenByDescending(r => r.Field<int>("UnidadId")).Take(5))
                {
                    dt.Rows.Add(
                        r["CodigoUnidad"],
                        r["Estado"],
                        r["PrestadaA"] is string emp && emp != "" ? emp : "—",
                        r["FechaDevolucionEsperada"] is DateTime f ? f.ToString("dd/MM/yyyy") : "—");
                }
            }
            catch (Exception ex) { Error(Errores.Mensaje(ex)); }

            dgvUnidadesMini.DataSource = dt;
            dgvUnidadesMini.Height = dgvUnidadesMini.ColumnHeadersHeight + Math.Max(1, dt.Rows.Count) * dgvUnidadesMini.RowTemplate.Height + 2;
            dgvUnidadesMini.ClearSelection();
            lnkVerTodas.Text = $"Ver todas ({total}) →";
        }

        private void dgvUnidadesMini_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != colUEstado.Index || e.Graphics == null) return;
            e.PaintBackground(e.CellBounds, false);
            string estado = e.Value?.ToString() ?? "";
            var (fondo, frente) = StatusChip.ColoresEstadoUnidad(estado);
            StatusChip.DibujarEnCelda(e.Graphics, e.CellBounds, estado, fondo, frente);
            e.Handled = true;
        }

        private void btnGuardarH_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombreF.Text)) { Aviso("El Nombre es obligatorio."); return; }

            int? catId = cboCategoriaF.SelectedValue as int?;
            int? marId = cboMarcaF.SelectedValue as int?;
            int? ubiId = cboUbicacionF.SelectedValue as int?;

            try
            {
                int id;
                if (_modoNuevo)
                {
                    id = HerramientaService.Insertar(txtNombreF.Text.Trim(), txtCaracteristicasF.Text.Trim(),
                                                     catId, marId, ubiId, (int)nudStockInicial.Value);
                    _modoNuevo = false;
                    OK("Herramienta creada.");
                }
                else if (_actual != null)
                {
                    id = _actual.HerramientaId;
                    HerramientaService.Actualizar(id, txtNombreF.Text.Trim(), txtCaracteristicasF.Text.Trim(),
                                                  catId, marId, ubiId);
                    OK("Cambios guardados.");
                }
                else return;

                CargarHerramientas(id);
                CargarCatalogo();          // cambia el conteo de herramientas por categoría/marca/ubicación
            }
            catch (Exception ex) { Error(Errores.Mensaje(ex)); }
        }

        private void btnCancelarH_Click(object? sender, EventArgs e)
        {
            if (_modoNuevo || _actual == null) { MostrarVacio(); return; }
            MostrarHerramienta(_actual);   // descarta cambios sin guardar
        }

        private void btnDarDeBaja_Click(object? sender, EventArgs e)
        {
            if (_actual == null) { Aviso("Seleccione una herramienta."); return; }
            if (Confirmar($"¿Dar de baja la herramienta COMPLETA \"{_actual.Nombre}\" (todas sus unidades)?\n\n" +
                          "Para dar de baja una sola unidad, o suspender el préstamo sin darla de baja, use \"Unidades / Stock\".") != DialogResult.Yes) return;
            try
            {
                HerramientaService.DarDeBaja(_actual.HerramientaId);
                OK("Herramienta dada de baja.");
                CargarHerramientas(null);
                CargarCatalogo();
                CargarMantenimientos();    // sus unidades dañadas salen de pendientes
            }
            catch (Exception ex) { Error(Errores.Mensaje(ex)); }
        }

        // Se guarda al momento (no espera a "Guardar cambios"): la foto queda en Fotos\{Código}.ext
        private void btnCambiarFoto_Click(object? sender, EventArgs e)
        {
            if (_actual == null) return;

            try
            {
                string? nombreArchivo = ImagenHelper.SeleccionarYCopiarFoto(_actual.Codigo, this);
                if (nombreArchivo == null) return;

                HerramientaService.ActualizarFoto(_actual.HerramientaId, nombreArchivo);
                _actual.FotoNombre = nombreArchivo;   // misma instancia que en _herramientas
                ImagenHelper.MostrarEn(picFoto, nombreArchivo);
            }
            catch (Exception ex) { Error(Errores.Mensaje(ex)); }
        }

        // Unidades / Stock: acciones sobre unidades concretas o sobre todo el grupo
        private void AbrirUnidades()
        {
            if (_actual == null) return;
            int id = _actual.HerramientaId;
            using (var frm = new FrmUnidades(id))
                frm.ShowDialog(this);

            CargarHerramientas(id);
            CargarMantenimientos();
        }

        // ══════════════════════════════════════════════════════════
        // CATÁLOGOS (categorías / marcas / ubicaciones / proveedores)
        // ══════════════════════════════════════════════════════════
        private bool EsProveedores => CatalogoActual == TipoCatalogo.Proveedores;

        private string IdColumna => CatalogoActual switch
        {
            TipoCatalogo.Marcas => "MarcaId",
            TipoCatalogo.Ubicaciones => "UbicacionId",
            TipoCatalogo.Proveedores => "ProveedorId",
            _ => "CategoriaId"
        };

        private string NombreCatalogo => CatalogoActual switch
        {
            TipoCatalogo.Marcas => "marca",
            TipoCatalogo.Ubicaciones => "ubicación",
            TipoCatalogo.Proveedores => "proveedor",
            _ => "categoría"
        };

        // "la marca" / "el proveedor"; "Marca agregada." / "Proveedor agregado."
        private string Articulo => EsProveedores ? "el" : "la";
        private string Mensaje(string accion) =>
            char.ToUpper(NombreCatalogo[0]) + NombreCatalogo[1..] + " " + accion + (EsProveedores ? "o." : "a.");

        private void CargarCatalogo()
        {
            try
            {
                dgvCatalogo.DataSource = CatalogoActual switch
                {
                    TipoCatalogo.Marcas => MarcaService.ObtenerTodas(),
                    TipoCatalogo.Ubicaciones => UbicacionService.ObtenerTodas(),
                    TipoCatalogo.Proveedores => ProveedorService.ObtenerTodos(),
                    _ => CategoriaService.ObtenerTodas()
                };
            }
            catch (Exception ex) { Error(Errores.Mensaje(ex)); }

            // Proveedores: teléfono y conteo de mantenimientos en lugar de herramientas
            colCatTelefono.Visible = txtCatTelefono.Visible = EsProveedores;
            colCatHerramientas.DataPropertyName = EsProveedores ? "Mantenimientos" : "Herramientas";
            colCatHerramientas.HeaderText = EsProveedores ? "Mantenimientos" : "Herramientas";
            dgvCatalogo.ClearSelection();
            LimpiarFormCatalogo();
        }

        private void LimpiarFormCatalogo()
        {
            _catalogoId = null;
            txtCatNombre.Clear();
            txtCatDescripcion.Clear();
            txtCatTelefono.Clear();
            txtCatNombre.PlaceholderText = $"Nombre {(EsProveedores ? "del" : "de la")} {NombreCatalogo}";
            txtCatDescripcion.PlaceholderText = EsProveedores ? "Especialidad, contacto…" : "Descripción";
            // Largo de las columnas Descripcion: Ubicacion/Proveedor VARCHAR(250), Categoria/Marca VARCHAR(400)
            txtCatNombre.MaxLength = 120;
            txtCatDescripcion.MaxLength = CatalogoActual is TipoCatalogo.Ubicaciones or TipoCatalogo.Proveedores ? 250 : 400;
            MostrarBotonesEdicion(false);
            dgvCatalogo.ClearSelection();
        }

        private void dgvCatalogo_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvCatalogo.Rows[e.RowIndex].DataBoundItem is not DataRowView r) return;
            _catalogoId = Convert.ToInt32(r[IdColumna]);
            txtCatNombre.Text = r["Nombre"]?.ToString() ?? "";
            txtCatDescripcion.Text = r["Descripcion"]?.ToString() ?? "";
            txtCatTelefono.Text = EsProveedores ? r["Telefono"]?.ToString() ?? "" : "";
            MostrarBotonesEdicion(true);
        }

        // Uno por uno y en orden visual: al hacerse visible, cada control crea su ventana y
        // WinForms reordena los hijos del FlowLayoutPanel según el orden de creación.
        // (a.Visible = b.Visible = true se evalúa de derecha a izquierda y los invertía)
        private void MostrarBotonesEdicion(bool editando)
        {
            btnCatAgregar.Visible = !editando;
            btnCatGuardar.Visible = editando;
            btnCatEliminar.Visible = editando;
            lnkCatCancelar.Visible = editando;
        }

        // Conteo de herramientas como badge
        private void dgvCatalogo_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != colCatHerramientas.Index || e.Graphics == null) return;
            e.PaintBackground(e.CellBounds, (e.State & DataGridViewElementStates.Selected) != 0);
            int n = e.Value is int v ? v : 0;
            var (fondo, frente) = n > 0 ? (ThemeManager.AccentBlueSoft, ThemeManager.AccentBlue) : (Paleta.GrisSuave, Paleta.Gris);
            StatusChip.DibujarEnCelda(e.Graphics, e.CellBounds, n.ToString(), fondo, frente);
            e.Handled = true;
        }

        private void btnCatAgregar_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCatNombre.Text)) { Aviso("El nombre es obligatorio."); return; }
            try
            {
                string nombre = txtCatNombre.Text.Trim(), descripcion = txtCatDescripcion.Text.Trim();
                switch (CatalogoActual)
                {
                    case TipoCatalogo.Marcas: MarcaService.Insertar(nombre, descripcion); break;
                    case TipoCatalogo.Ubicaciones: UbicacionService.Insertar(nombre, descripcion); break;
                    case TipoCatalogo.Proveedores: ProveedorService.Insertar(nombre, txtCatTelefono.Text.Trim(), descripcion); break;
                    default: CategoriaService.Insertar(nombre, descripcion); break;
                }
                DespuesDeCambiarCatalogo(Mensaje("agregad"));
            }
            catch (Exception ex) { Error(Errores.Mensaje(ex)); }
        }

        private void btnCatGuardar_Click(object? sender, EventArgs e)
        {
            if (_catalogoId == null) return;
            if (string.IsNullOrWhiteSpace(txtCatNombre.Text)) { Aviso("El nombre es obligatorio."); return; }
            try
            {
                int id = _catalogoId.Value;
                string nombre = txtCatNombre.Text.Trim(), descripcion = txtCatDescripcion.Text.Trim();
                switch (CatalogoActual)
                {
                    case TipoCatalogo.Marcas: MarcaService.Actualizar(id, nombre, descripcion); break;
                    case TipoCatalogo.Ubicaciones: UbicacionService.Actualizar(id, nombre, descripcion); break;
                    case TipoCatalogo.Proveedores: ProveedorService.Actualizar(id, nombre, txtCatTelefono.Text.Trim(), descripcion); break;
                    default: CategoriaService.Actualizar(id, nombre, descripcion); break;
                }
                DespuesDeCambiarCatalogo(Mensaje("guardad"));
            }
            catch (Exception ex) { Error(Errores.Mensaje(ex)); }
        }

        private void btnCatEliminar_Click(object? sender, EventArgs e)
        {
            if (_catalogoId == null || dgvCatalogo.CurrentRow?.DataBoundItem is not DataRowView r) return;
            int usadas = Convert.ToInt32(r[colCatHerramientas.DataPropertyName]);
            string que = NombreCatalogo;
            string aviso = usadas == 0 ? ""
                : EsProveedores ? $"\n\nTiene {usadas} mantenimiento(s) registrados: se desactivará y su historial se conserva."
                : $"\n\n{usadas} herramienta(s) quedarán sin {que}.";
            if (Confirmar($"¿Eliminar {Articulo} {que} \"{r["Nombre"]}\"?{aviso}") != DialogResult.Yes) return;
            try
            {
                switch (CatalogoActual)
                {
                    case TipoCatalogo.Marcas: MarcaService.Eliminar(_catalogoId.Value); break;
                    case TipoCatalogo.Ubicaciones: UbicacionService.Eliminar(_catalogoId.Value); break;
                    case TipoCatalogo.Proveedores: ProveedorService.Eliminar(_catalogoId.Value); break;
                    default: CategoriaService.Eliminar(_catalogoId.Value); break;
                }
                DespuesDeCambiarCatalogo(Mensaje("eliminad"));
            }
            catch (Exception ex) { Error(Errores.Mensaje(ex)); }
        }

        // Categorías, marcas y ubicaciones alimentan combos, chips y la lista de herramientas.
        // Los proveedores se leen al abrir un mantenimiento.
        private void DespuesDeCambiarCatalogo(string mensaje)
        {
            OK(mensaje);
            CargarCatalogo();
            CargarCombos();
            CargarHerramientas(_modoNuevo ? null : _actual?.HerramientaId);
        }

        // ══════════════════════════════════════════════════════════
        // MANTENIMIENTO
        // ══════════════════════════════════════════════════════════
        // Recarga los activos y la bandeja de pendientes, y limpia la selección
        private void CargarMantenimientos()
        {
            try
            {
                _mantView = MantenimientoService.ObtenerActivos().DefaultView;
                dgvMant.DataSource = _mantView;

                // Pendientes: unidades dañadas que esperan reparación
                var pendientes = MantenimientoService.UnidadesElegibles().AsEnumerable()
                    .Where(r => r.Field<string>("Estado") == "Dañada");
                dgvPendientes.DataSource = pendientes.Any() ? pendientes.CopyToDataTable() : null;
                int n = dgvPendientes.Rows.Count;
                chipPendientes.Text = n.ToString();
                chipPendientes.Left = lblPendientesTitulo.Right + 8;
                lblPendientesVacio.Visible = n == 0;
                dgvPendientes.Visible = n > 0;
            }
            catch (Exception ex) { Error(Errores.Mensaje(ex)); }

            FiltrarMantenimientos();
            SinSeleccionMantenimiento();
        }

        private void FiltrarMantenimientos()
        {
            if (_mantView == null) return;
            string t = txtBuscarMant.Text.Trim();
            if (t == "") _mantView.RowFilter = "";
            else
            {
                // Escapar comillas y comodines para RowFilter
                string esc = t.Replace("'", "''").Replace("[", "[[]").Replace("*", "[*]").Replace("%", "[%]");
                _mantView.RowFilter = string.Join(" OR ",
                    new[] { "Herramienta", "CodigoHerramienta", "RealizadoPor", "Descripcion", "TipoMantenimiento" }
                    .Select(c => $"{c} LIKE '%{esc}%'"));
            }

            int total = _mantView.Table?.Rows.Count ?? 0;
            chipMantActivos.Text = total == 1 ? "1 activo" : $"{total} activos";
            dgvMant.ClearSelection();
        }

        private void dgvMant_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Graphics == null || dgvMant.Rows[e.RowIndex].DataBoundItem is not DataRowView r) return;
            bool sel = (e.State & DataGridViewElementStates.Selected) != 0;

            if (e.ColumnIndex == colMHerramienta.Index)
            {
                // Nombre (negrita) y código de unidad (monospace) en dos líneas
                DibujarDosLineas(e, sel, r["Herramienta"]?.ToString(), r["CodigoHerramienta"]?.ToString(), FuenteCodigo);
            }
            else if (e.ColumnIndex == colMRealizadoPor.Index)
            {
                // Responsable y, debajo, si el servicio es interno (empleado) o externo (proveedor)
                DibujarDosLineas(e, sel, r["RealizadoPor"]?.ToString(), r["TipoServicio"]?.ToString(), e.CellStyle!.Font!);
            }
            else if (e.ColumnIndex == colMTipo.Index)
            {
                e.PaintBackground(e.CellBounds, sel);
                string tipo = e.Value?.ToString() ?? "";
                var (fondo, frente) = StatusChip.ColoresTipoMantenimiento(tipo);
                StatusChip.DibujarEnCelda(e.Graphics, e.CellBounds, tipo, fondo, frente);
                e.Handled = true;
            }
            else if (e.ColumnIndex == colMDias.Index)
            {
                e.PaintBackground(e.CellBounds, sel);
                int dias = e.Value is int d ? d : 0;
                var (fondo, frente) = ColoresEspera(dias);
                StatusChip.DibujarEnCelda(e.Graphics, e.CellBounds, dias == 1 ? "1 día" : $"{dias} días", fondo, frente);
                e.Handled = true;
            }
        }

        private void dgvPendientes_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Graphics == null || dgvPendientes.Rows[e.RowIndex].DataBoundItem is not DataRowView r) return;
            bool sel = (e.State & DataGridViewElementStates.Selected) != 0;

            if (e.ColumnIndex == colPUnidad.Index)
                DibujarDosLineas(e, sel, r["Herramienta"]?.ToString(), r["CodigoUnidad"]?.ToString(), FuenteCodigo);
            else if (e.ColumnIndex == colPReporte.Index)
            {
                // Quién la devolvió dañada y hace cuánto (sin devolución: se marcó dañada en almacén)
                string quien = r["ReportadoPor"] is string q ? q : "En almacén";
                string cuando = r["DiasEsperando"] is int d ? (d == 0 ? "hoy" : d == 1 ? "hace 1 día" : $"hace {d} días") : "";
                DibujarDosLineas(e, sel, quien, cuando, e.CellStyle!.Font!);
            }
        }

        // Verde < 7 días, amarillo 7-15, rojo > 15
        private static (Color Fondo, Color Frente) ColoresEspera(int dias) =>
            dias > 15 ? (Paleta.RojoSuave, Paleta.Rojo)
            : dias >= 7 ? (Paleta.AmarilloSuave, Paleta.Amarillo)
            : (Paleta.VerdeSuave, Paleta.Verde);

        private static readonly Font FuenteNegrita = new("Segoe UI", 9.5F, FontStyle.Bold);

        private static void DibujarDosLineas(DataGridViewCellPaintingEventArgs e, bool sel, string? arriba, string? abajo, Font fuenteAbajo)
        {
            e.PaintBackground(e.CellBounds, sel);
            var b = e.CellBounds;
            TextRenderer.DrawText(e.Graphics!, arriba, FuenteNegrita,
                new Rectangle(b.X + 10, b.Y + 7, b.Width - 14, 20), ThemeManager.TextPrimary,
                TextFormatFlags.Left | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics!, abajo, fuenteAbajo,
                new Rectangle(b.X + 10, b.Y + 28, b.Width - 14, 18), ThemeManager.TextSecondary,
                TextFormatFlags.Left | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);
            e.Handled = true;
        }

        // ── Selección: la barra inferior actúa sobre la última fila elegida en cualquiera de las dos grillas ──
        private void dgvMant_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvMant.Rows[e.RowIndex].DataBoundItem is not DataRowView r) return;
            _mantSeleccionado = r.Row;
            _pendienteSeleccionado = null;
            dgvPendientes.ClearSelection();

            int dias = Convert.ToInt32(r["DiasEnMantenimiento"]);
            lblAccionInfo.Text = $"{r["Herramienta"]}  ·  {r["CodigoHerramienta"]}  ·  {r["TipoMantenimiento"]}  ·  " +
                                 $"{r["TipoServicio"]}: {r["RealizadoPor"]}  ·  " +
                                 (dias == 1 ? "1 día" : $"{dias} días") + " en mantenimiento";
            MostrarAccion("Cerrar mantenimiento", FontAwesome.Sharp.IconChar.CheckCircle);
        }

        private void dgvPendientes_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvPendientes.Rows[e.RowIndex].DataBoundItem is not DataRowView r) return;
            _pendienteSeleccionado = r.Row;
            _mantSeleccionado = null;
            dgvMant.ClearSelection();

            string reporte = r["ReportadoPor"] is string quien
                ? $"Devuelta dañada por {quien}" + (r["FechaReporte"] is DateTime f ? $" el {f:dd/MM/yyyy}" : "")
                : "Marcada como dañada en almacén";
            if (r["NotaReporte"] is string nota && nota != "") reporte += $": \"{nota}\"";
            lblAccionInfo.Text = $"{r["Herramienta"]}  ·  {r["CodigoUnidad"]}  ·  {reporte}";
            MostrarAccion("Enviar a mantenimiento", FontAwesome.Sharp.IconChar.Wrench);
        }

        private void MostrarAccion(string texto, FontAwesome.Sharp.IconChar icono)
        {
            btnAccionMant.Text = texto;
            btnAccionMant.Icon = icono;
            lblAccionInfo.ForeColor = ThemeManager.TextPrimary;
            btnAccionMant.Visible = true;
            lnkHistorial.Visible = true;
        }

        private void SinSeleccionMantenimiento()
        {
            _mantSeleccionado = null;
            _pendienteSeleccionado = null;
            dgvMant.ClearSelection();
            dgvPendientes.ClearSelection();
            lblAccionInfo.Text = "Seleccione un mantenimiento para cerrarlo, o una unidad pendiente para enviarla a reparación.";
            lblAccionInfo.ForeColor = ThemeManager.TextSecondary;
            btnAccionMant.Visible = false;
            lnkHistorial.Visible = false;
        }

        private void EjecutarAccionMantenimiento()
        {
            if (_mantSeleccionado != null) CerrarMantenimiento(_mantSeleccionado);
            else if (_pendienteSeleccionado != null) AbrirMantenimiento(_pendienteSeleccionado.Field<int>("UnidadId"));
        }

        private void AbrirMantenimiento(int? unidadId)
        {
            int registradas;
            using (var frm = new FrmAbrirMantenimiento(unidadId is int id ? new[] { id } : null))
            {
                if (frm.ShowDialog(this) != DialogResult.OK) return;
                registradas = frm.UnidadesRegistradas;
            }

            OK(registradas == 1
                ? "Mantenimiento abierto. La unidad quedó fuera del stock disponible."
                : $"Mantenimiento abierto para {registradas} unidades. Quedaron fuera del stock disponible.");
            DespuesDeCambiarMantenimiento();
        }

        private void CerrarMantenimiento(DataRow mantenimiento)
        {
            using (var frm = new FrmCerrarMantenimiento(mantenimiento))
                if (frm.ShowDialog(this) != DialogResult.OK) return;

            OK("Mantenimiento cerrado.");
            DespuesDeCambiarMantenimiento();
        }

        // El stock de las herramientas cambia al abrir o cerrar un mantenimiento
        private void DespuesDeCambiarMantenimiento()
        {
            CargarMantenimientos();
            CargarHerramientas(_modoNuevo ? null : _actual?.HerramientaId);
        }

        // Historial de mantenimientos de la herramienta de la fila seleccionada (activa o pendiente)
        private void VerHistorial()
        {
            var fila = _mantSeleccionado ?? _pendienteSeleccionado;
            if (fila == null) return;

            int herramientaId = fila.Field<int>("HerramientaId");
            DataTable dt;
            try { dt = MantenimientoService.ObtenerPorHerramienta(herramientaId); }
            catch (Exception ex) { Error(Errores.Mensaje(ex)); return; }
            if (dt.Rows.Count == 0) { Aviso($"\"{fila["Herramienta"]}\" no tiene mantenimientos registrados."); return; }

            // Encabezado con el nombre y código del grupo (la fila de la bandeja trae la unidad)
            string titulo = fila["Herramienta"]?.ToString() ?? "";
            try
            {
                var h = HerramientaService.ObtenerPorId(herramientaId);
                if (h.Rows.Count > 0) titulo = $"{h.Rows[0]["Nombre"]}  ·  {h.Rows[0]["Codigo"]}";
            }
            catch { /* el encabezado conserva el texto de la fila */ }

            using var frm = new FrmHistorialMantenimiento(titulo, dt);
            frm.ShowDialog(this);
        }

        // ══════════════════════════════════════════════════════════
        // HELPERS
        // ══════════════════════════════════════════════════════════
        private void Aviso(string m) => MessageBox.Show(m, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        private void OK(string m) => MessageBox.Show(m, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        private void Error(string m) => MessageBox.Show(m, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        private DialogResult Confirmar(string m) => MessageBox.Show(m, "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
    }
}
