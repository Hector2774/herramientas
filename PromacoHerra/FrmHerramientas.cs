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
    //   Catálogos      → categorías y marcas en la misma grilla (control segmentado)
    //   Mantenimiento  → mantenimientos activos + panel para abrir uno nuevo o cerrar el seleccionado
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
        private bool EsMarcas => segCatalogo.Seleccionado == 1;
        private int? _catalogoId;

        // Mantenimiento
        private DataView? _mantView;
        private int? _mantenimientoId;
        private DataTable _unidadesMant = new();

        // Columnas de las grillas. Se crean en código y no en el Designer: el diseñador de
        // Visual Studio las eliminaba al volver a guardar el formulario.
        private readonly DataGridViewTextBoxColumn colUCodigo = Columna("Codigo", "Código unidad", 25);
        private readonly DataGridViewTextBoxColumn colUEstado = Columna("Estado", "Estado", 25);
        private readonly DataGridViewTextBoxColumn colUPrestadaA = Columna("PrestadaA", "Prestada a", 30);
        private readonly DataGridViewTextBoxColumn colUDevolucion = Columna("Devolucion", "Devolución", 20);
        private readonly DataGridViewTextBoxColumn colCatNombre = Columna("Nombre", "Nombre", 30);
        private readonly DataGridViewTextBoxColumn colCatDescripcion = Columna("Descripcion", "Descripción", 55);
        private readonly DataGridViewTextBoxColumn colCatHerramientas = Columna("Herramientas", "Herramientas", 15);
        private readonly DataGridViewTextBoxColumn colMHerramienta = Columna("Herramienta", "Herramienta", 28);
        private readonly DataGridViewTextBoxColumn colMTipo = Columna("TipoMantenimiento", "Tipo", 15);
        private readonly DataGridViewTextBoxColumn colMRealizadoPor = Columna("RealizadoPor", "Realizado por", 17);
        private readonly DataGridViewTextBoxColumn colMDias = Columna("DiasEnMantenimiento", "Días activo", 12);
        private readonly DataGridViewTextBoxColumn colMDescripcion = Columna("Descripcion", "Descripción", 28);

        public FrmHerramientas()
        {
            InitializeComponent();
            ConfigurarColumnas();
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
            btnNuevoMant.Click += (s, e) => ModoAbrirMantenimiento();
            // "Nuevo" siempre en la esquina superior derecha del panel de acciones
            pnlAccionesMant.Resize += (s, e) =>
                btnNuevoMant.Left = pnlAccionesMant.ClientSize.Width - pnlAccionesMant.Padding.Right - btnNuevoMant.Width;
            btnAbrirMant.Click += btnAbrirMant_Click;
            btnCerrarMant.Click += btnCerrarMant_Click;
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
                (dgvCatalogo, new[] { colCatNombre, colCatDescripcion, colCatHerramientas }),
                (dgvMant, new[] { colMHerramienta, colMTipo, colMRealizadoPor, colMDias, colMDescripcion })
            })
            {
                dgv.AutoGenerateColumns = false;
                dgv.Columns.Clear();
                dgv.Columns.AddRange(columnas);
            }
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

            foreach (var lbl in new[] { lblListaVacia, lblSeleccionaHerramienta, lblCodigoDet, lblCerrarInfo })
                lbl.ForeColor = ThemeManager.TextSecondary;
            foreach (var lbl in new[] { lblCodigoF, lblUbicacionF, lblNombreF, lblCategoriaF, lblMarcaF, lblStockInicialF,
                                        lblCaracteristicasF, lblUnidadMant, lblTipoMant, lblRealizadoPor, lblDescMant,
                                        lblCosto, lblNotasCierre })
                lbl.ForeColor = ThemeManager.TextSecondary;
            foreach (var lnk in new[] { lnkVerTodas, lnkCatCancelar, lnkHistorial })
                lnk.LinkColor = lnk.ActiveLinkColor = ThemeManager.AccentBlue;

            txtCaracteristicasF.BorderStyle = BorderStyle.FixedSingle;

            tileTotal.ColorValor = ThemeManager.TextPrimary;
            tileDisponibles.ColorValor = Paleta.Verde;
            tilePrestadas.ColorValor = Paleta.Amarillo;
            tileMantenimiento.ColorValor = Paleta.Naranja;
            tileDañadas.ColorValor = Paleta.Rojo;

            segCatalogo.Opciones = new[] { "Categorías", "Marcas" };
            chipMantActivos.Fondo = Paleta.NaranjaSuave;
            chipMantActivos.Frente = Paleta.Naranja;

            // Variantes que la heurística de ThemeManager (por texto) no adivina
            btnNuevaHerramienta.Variant = MaterialButtonVariant.Primary;
            btnDarDeBaja.Variant = MaterialButtonVariant.Default;
            btnCatAgregar.Variant = MaterialButtonVariant.Primary;
            btnCerrarMant.Variant = MaterialButtonVariant.Primary;
            btnNuevoMant.Variant = MaterialButtonVariant.Default;
            pnlAccionesMant.CornerRadius = 12;
            btnNuevoMant.BringToFront();

            EstilizarGrid(dgvUnidadesMini, 38);
            EstilizarGrid(dgvCatalogo, 42);
            EstilizarGrid(dgvMant, 52);
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

            if (cboTipoMant.Items.Count == 0)
                cboTipoMant.Items.AddRange(new object[] { "Preventivo", "Correctivo", "Calibración" });
            cboTipoMant.SelectedIndex = 0;

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
                CargarUnidadesMant();
            }
            catch (Exception ex) { Error(ex.Message); }
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
            catch (Exception ex) { Error(ex.Message); _herramientas = new(); }

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
            catch (Exception ex) { Error(ex.Message); }

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
                CargarCatalogo();          // cambia el conteo de herramientas por categoría/marca
                CargarUnidadesMant();      // una herramienta nueva trae unidades nuevas
            }
            catch (Exception ex) { Error(ex.Message); }
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
                CargarUnidadesMant();
            }
            catch (Exception ex) { Error(ex.Message); }
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
            CargarUnidadesMant();
        }

        // ══════════════════════════════════════════════════════════
        // CATÁLOGOS (categorías / marcas)
        // ══════════════════════════════════════════════════════════
        private string IdColumna => EsMarcas ? "MarcaId" : "CategoriaId";

        private void CargarCatalogo()
        {
            try
            {
                dgvCatalogo.DataSource = EsMarcas ? MarcaService.ObtenerTodas() : CategoriaService.ObtenerTodas();
            }
            catch (Exception ex) { Error(ex.Message); }
            dgvCatalogo.ClearSelection();
            LimpiarFormCatalogo();
        }

        private void LimpiarFormCatalogo()
        {
            _catalogoId = null;
            txtCatNombre.Clear();
            txtCatDescripcion.Clear();
            txtCatNombre.PlaceholderText = EsMarcas ? "Nombre de la marca" : "Nombre de la categoría";
            MostrarBotonesEdicion(false);
            dgvCatalogo.ClearSelection();
        }

        private void dgvCatalogo_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvCatalogo.Rows[e.RowIndex].DataBoundItem is not DataRowView r) return;
            _catalogoId = Convert.ToInt32(r[IdColumna]);
            txtCatNombre.Text = r["Nombre"]?.ToString() ?? "";
            txtCatDescripcion.Text = r["Descripcion"]?.ToString() ?? "";
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
                if (EsMarcas) MarcaService.Insertar(txtCatNombre.Text.Trim(), txtCatDescripcion.Text.Trim());
                else CategoriaService.Insertar(txtCatNombre.Text.Trim(), txtCatDescripcion.Text.Trim());
                DespuesDeCambiarCatalogo(EsMarcas ? "Marca agregada." : "Categoría agregada.");
            }
            catch (Exception ex) { Error(ex.Message); }
        }

        private void btnCatGuardar_Click(object? sender, EventArgs e)
        {
            if (_catalogoId == null) return;
            if (string.IsNullOrWhiteSpace(txtCatNombre.Text)) { Aviso("El nombre es obligatorio."); return; }
            try
            {
                if (EsMarcas) MarcaService.Actualizar(_catalogoId.Value, txtCatNombre.Text.Trim(), txtCatDescripcion.Text.Trim());
                else CategoriaService.Actualizar(_catalogoId.Value, txtCatNombre.Text.Trim(), txtCatDescripcion.Text.Trim());
                DespuesDeCambiarCatalogo(EsMarcas ? "Marca guardada." : "Categoría guardada.");
            }
            catch (Exception ex) { Error(ex.Message); }
        }

        private void btnCatEliminar_Click(object? sender, EventArgs e)
        {
            if (_catalogoId == null || dgvCatalogo.CurrentRow?.DataBoundItem is not DataRowView r) return;
            int usadas = Convert.ToInt32(r["Herramientas"]);
            string que = EsMarcas ? "marca" : "categoría";
            string aviso = usadas > 0 ? $"\n\n{usadas} herramienta(s) quedarán sin {que}." : "";
            if (Confirmar($"¿Eliminar la {que} \"{r["Nombre"]}\"?{aviso}") != DialogResult.Yes) return;
            try
            {
                if (EsMarcas) MarcaService.Eliminar(_catalogoId.Value);
                else CategoriaService.Eliminar(_catalogoId.Value);
                DespuesDeCambiarCatalogo(EsMarcas ? "Marca eliminada." : "Categoría eliminada.");
            }
            catch (Exception ex) { Error(ex.Message); }
        }

        // Categorías y marcas alimentan combos, chips y la lista de herramientas
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
        // Unidades que pueden entrar a mantenimiento: disponibles o dañadas de herramientas activas
        private void CargarUnidadesMant()
        {
            _unidadesMant = PromacoHerra.Data.Db.Query(@"
                SELECT u.UnidadId,
                       u.HerramientaId,
                       u.CodigoUnidad + ' — ' + h.Nombre + ' (' + u.Estado + ')' AS Display
                FROM   vw_HerramientaUnidad u
                INNER  JOIN Herramienta h ON h.HerramientaId = u.HerramientaId
                WHERE  h.Activa = 1
                  AND  u.Estado IN ('Disponible', 'Dañada')
                ORDER  BY h.Nombre, u.Numero");
            CargarCombo(cboUnidadMant, _unidadesMant, "Display", "UnidadId");
        }

        private void CargarMantenimientos()
        {
            try
            {
                _mantView = MantenimientoService.ObtenerActivos().DefaultView;
                dgvMant.DataSource = _mantView;
            }
            catch (Exception ex) { Error(ex.Message); }

            FiltrarMantenimientos();
            ModoAbrirMantenimiento();
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
                e.PaintBackground(e.CellBounds, sel);
                var b = e.CellBounds;
                TextRenderer.DrawText(e.Graphics, r["Herramienta"]?.ToString(), new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    new Rectangle(b.X + 10, b.Y + 7, b.Width - 14, 20), ThemeManager.TextPrimary,
                    TextFormatFlags.Left | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);
                TextRenderer.DrawText(e.Graphics, r["CodigoHerramienta"]?.ToString(), FuenteCodigo,
                    new Rectangle(b.X + 10, b.Y + 28, b.Width - 14, 18), ThemeManager.TextSecondary,
                    TextFormatFlags.Left | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);
                e.Handled = true;
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
                // Verde < 7 días, amarillo 7-15, rojo > 15
                e.PaintBackground(e.CellBounds, sel);
                int dias = e.Value is int d ? d : 0;
                var (fondo, frente) = dias > 15 ? (Paleta.RojoSuave, Paleta.Rojo)
                                    : dias >= 7 ? (Paleta.AmarilloSuave, Paleta.Amarillo)
                                    : (Paleta.VerdeSuave, Paleta.Verde);
                StatusChip.DibujarEnCelda(e.Graphics, e.CellBounds, dias == 1 ? "1 día" : $"{dias} días", fondo, frente);
                e.Handled = true;
            }
        }

        private void dgvMant_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvMant.Rows[e.RowIndex].DataBoundItem is not DataRowView r) return;
            _mantenimientoId = Convert.ToInt32(r["MantenimientoId"]);

            int dias = Convert.ToInt32(r["DiasEnMantenimiento"]);
            lblCerrarInfo.Text = $"{r["Herramienta"]}  ·  {r["CodigoHerramienta"]}  ·  {r["TipoMantenimiento"]}  ·  " +
                                 $"{(dias == 1 ? "1 día" : $"{dias} días")} en mantenimiento" +
                                 (r["RealizadoPor"] is string tec && tec != "" ? $"  ·  {tec}" : "");
            nudCosto.Value = 0;
            txtNotasCierre.Clear();

            pnlAbrirMant.Visible = false;
            pnlCerrarMant.Visible = true;
        }

        private void ModoAbrirMantenimiento()
        {
            _mantenimientoId = null;
            dgvMant.ClearSelection();
            pnlCerrarMant.Visible = false;
            pnlAbrirMant.Visible = true;
        }

        private void btnAbrirMant_Click(object? sender, EventArgs e)
        {
            if (cboUnidadMant.SelectedValue == null) { Aviso("Seleccione una unidad."); return; }
            if (cboTipoMant.SelectedIndex < 0) { Aviso("Seleccione el tipo de mantenimiento."); return; }

            try
            {
                int unidadId = Convert.ToInt32(cboUnidadMant.SelectedValue);
                int herramientaId = _unidadesMant.AsEnumerable()
                    .First(r => r.Field<int>("UnidadId") == unidadId)
                    .Field<int>("HerramientaId");

                MantenimientoService.RegistrarEntrada(
                    herramientaId: herramientaId,
                    unidadIds: new[] { unidadId },
                    tipoMantenimiento: cboTipoMant.SelectedItem!.ToString()!,
                    descripcion: txtDescMant.Text.Trim(),
                    realizadoPor: txtRealizadoPor.Text.Trim());

                OK("Mantenimiento abierto. La unidad quedó fuera del stock disponible.\n" +
                   "Para enviar todo el grupo, use \"Unidades / Stock\" en la pestaña Herramientas.");
                cboTipoMant.SelectedIndex = 0;
                txtRealizadoPor.Clear();
                txtDescMant.Clear();
                CargarMantenimientos();
                CargarUnidadesMant();
                CargarHerramientas(_actual?.HerramientaId);
            }
            catch (Exception ex) { Error(ex.Message); }
        }

        private void btnCerrarMant_Click(object? sender, EventArgs e)
        {
            if (_mantenimientoId == null) { Aviso("Seleccione un mantenimiento de la lista."); return; }

            decimal? costo = nudCosto.Value > 0 ? nudCosto.Value : null;

            var resultado = MessageBox.Show(
                "¿La unidad quedó reparada?\n\n" +
                "Sí: vuelve al stock disponible.\n" +
                "No: es irreparable y se da de baja (sale del stock).",
                "Cerrar mantenimiento", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (resultado == DialogResult.Cancel) return;
            bool reparada = resultado == DialogResult.Yes;

            try
            {
                MantenimientoService.RegistrarSalida(
                    mantenimientoId: _mantenimientoId.Value,
                    descripcion: txtNotasCierre.Text.Trim(),
                    costo: costo,
                    reparada: reparada);

                OK(reparada
                    ? "Mantenimiento cerrado. La unidad volvió a estado Disponible."
                    : "Mantenimiento cerrado. La unidad se dio de baja y salió del stock.");
                CargarMantenimientos();
                CargarUnidadesMant();
                CargarHerramientas(_actual?.HerramientaId);
            }
            catch (Exception ex) { Error(ex.Message); }
        }

        // Historial de mantenimientos de la herramienta del mantenimiento seleccionado
        private void VerHistorial()
        {
            if (dgvMant.CurrentRow?.DataBoundItem is not DataRowView r || _mantenimientoId == null)
            { Aviso("Seleccione un mantenimiento de la lista para ver el historial de esa herramienta."); return; }

            int herramientaId = Convert.ToInt32(r["HerramientaId"]);
            var dt = MantenimientoService.ObtenerPorHerramienta(herramientaId);

            using var frmHistorial = new Form
            {
                Text = $"Historial de mantenimiento — {r["Herramienta"]}",
                Size = new Size(900, 480),
                StartPosition = FormStartPosition.CenterParent
            };
            var dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                DataSource = dt,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            frmHistorial.Controls.Add(dgv);
            ThemeManager.ApplyTheme(frmHistorial);
            frmHistorial.ShowDialog(this);
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
