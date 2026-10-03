using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using FontAwesome.Sharp;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using PromacoHerra.Controls;
using PromacoHerra.Data;
using PromacoHerra.Services;
using Color = System.Drawing.Color;

namespace PromacoHerra
{
    // Reportes: KPIs arriba, tarjetas para elegir el reporte, filtros según el reporte,
    // y una grilla cuyas celdas se dibujan según el tipo de dato (chips, avatar, barras, ranking).
    // Cada reporte define sus columnas (Col) y eso mismo se usa para dibujar y para exportar.
    public partial class FrmReportes : Form
    {
        private const string Moneda = "Q.";

        private enum Reporte { Historial, Vencidos, PorEmpleado, Ranking, Mantenimiento, Danadas, Inventario }

        // Cómo se dibuja (y exporta) cada columna
        private enum Render
        {
            Texto, Secundario, Prestamo, Empleado, Fecha, FechaHora, EstadoPrestamo, DiasVencido, Notificar,
            ActivosEmpleado, Numero, NumeroGrande, Rank, HerramientaRanking, Categoria, Barra, Decimal1,
            HerramientaUnidad, TipoMantenimiento, Duracion, Costo, Condicion, Disponibles
        }

        private sealed record Col(string Campo, string Titulo, Render Render, int Ancho = 0, float Peso = 100);

        private sealed record Opcion(object? Valor, string Texto)
        {
            public override string ToString() => Texto;
        }

        private sealed record InfoReporte(string Titulo, string Descripcion, IconChar Icono, Control[] Filtros, bool UsaFechas);

        private static readonly Font FuenteNegrita = new("Segoe UI", 9.5F, FontStyle.Bold);
        private static readonly Font FuenteChica = new("Segoe UI", 8F);
        private static readonly Font FuenteMono = new("Consolas", 9F);
        private static readonly Font FuenteGrande = new("Segoe UI", 11F, FontStyle.Bold);
        private static readonly Font FuenteBoton = new("Segoe UI", 8.5F, FontStyle.Bold);

        // KPIs
        private readonly KpiTile kpiVencidos = new("Préstamos vencidos", IconChar.TriangleExclamation, Paleta.Rojo, Paleta.RojoSuave) { Clickeable = true };
        private readonly KpiTile kpiActivos = new("Préstamos activos", IconChar.Handshake, ThemeManager.AccentBlue, ThemeManager.AccentBlueSoft);
        private readonly KpiTile kpiDisponibles = new("Herramientas disponibles", IconChar.CircleCheck, Paleta.Verde, Paleta.VerdeSuave);
        private readonly KpiTile kpiMantenimiento = new("En mantenimiento", IconChar.ScrewdriverWrench, Paleta.Naranja, Paleta.NaranjaSuave);
        private readonly KpiTile kpiDanadas = new("Herramientas dañadas", IconChar.HeartCrack, Paleta.Violeta, Paleta.VioletaSuave);

        // Controles de filtro (se reubican en el panel según el reporte activo)
        // Empleados y herramientas son demasiados para un combo: se eligen con el buscador.
        // Incluyen inactivos / dados de baja porque pueden aparecer en el historial.
        private readonly EmpleadoPickerControl pickEmpleado = new() { IncluirInactivos = true, Height = 38, MinimumSize = new Size(200, 38) };
        private readonly HerramientaPickerControl pickHerramienta = new() { IncluirInactivas = true, Height = 38, MinimumSize = new Size(200, 38) };
        private readonly MaterialComboBox cboEstadoPrestamo = Combo();
        private readonly MaterialComboBox cboCategoria = Combo();
        private readonly MaterialComboBox cboDepartamento = Combo();
        private readonly MaterialComboBox cboTipoMant = Combo();
        private readonly MaterialComboBox cboEstadoMant = Combo();
        private readonly MaterialComboBox cboCondicion = Combo();
        private readonly MaterialComboBox cboMostrar = Combo();
        private readonly DateTimePicker dtpDesde = Fecha();
        private readonly DateTimePicker dtpHasta = Fecha();
        private readonly Dictionary<Control, Panel> _campos = new();

        private readonly Dictionary<Reporte, InfoReporte> _info = new();
        private readonly Dictionary<Reporte, ReporteCard> _tarjetas = new();
        private Reporte _reporte = Reporte.Historial;

        // Resultado actual (lo usan la grilla y la exportación)
        private readonly DataGridView dgvReporte = new();
        private DataTable? _datos;
        private List<Col> _columnas = new();
        private string _periodo = "";

        public FrmReportes()
        {
            InitializeComponent();
            ThemeManager.ApplyTheme(this);
            ConstruirKpis();
            ConstruirFiltros();
            ConstruirTarjetas();
            ConstruirGrid();
            AplicarEstilos();

            this.Load += FrmReportes_Load;
            btnGenerar.Click += (s, e) => Generar();
            btnExcel.Click += (s, e) => ExportarExcel();
            btnPDF.Click += (s, e) => ExportarPDF();
            kpiVencidos.Click += (s, e) => { SeleccionarReporte(Reporte.Vencidos); Generar(); };

            // Lo alineado a la derecha se ubica al cambiar de tamaño (sin Anchor)
            pnlFiltros.Resize += (s, e) => AlinearDerecha();
            pnlResultadosHeader.Resize += (s, e) => AlinearDerecha();
            pnlSinIncidentes.Resize += (s, e) => CentrarSinIncidentes();
            flpTarjetas.Resize += (s, e) => AjustarTarjetas();
        }

        private static MaterialComboBox Combo() => new() { DropDownStyle = ComboBoxStyle.DropDownList };

        private static DateTimePicker Fecha() => new()
        {
            Format = DateTimePickerFormat.Custom,
            CustomFormat = "dd/MM/yyyy",
            Width = 130
        };

        private void AplicarEstilos()
        {
            lblSubtitulo.ForeColor = ThemeManager.TextSecondary;
            lblConteo.ForeColor = ThemeManager.TextSecondary;
            lblResumen.ForeColor = ThemeManager.TextPrimary;
            lblSinResultados.ForeColor = ThemeManager.TextSecondary;
            lblSinIncidentes.ForeColor = Paleta.Verde;
            icoSinIncidentes.IconColor = Paleta.Verde;
            icoSinIncidentes.BackColor = Color.White;
            pnlSinIncidentes.BackColor = Color.White;
            pnlFiltros.ShowShadow = false;
            pnlFiltros.CornerRadius = 10;
            btnGenerar.Variant = MaterialButtonVariant.Primary;
            btnExcel.Variant = MaterialButtonVariant.Default;
            btnPDF.Variant = MaterialButtonVariant.Default;
        }

        private void AlinearDerecha()
        {
            btnGenerar.Location = new Point(pnlFiltros.Width - 20 - btnGenerar.Width, 30);
            flpFiltros.SetBounds(14, 10, Math.Max(200, btnGenerar.Left - 24), 66);
            btnPDF.Location = new Point(pnlResultadosHeader.Width - btnPDF.Width, 0);
            btnExcel.Location = new Point(btnPDF.Left - 8 - btnExcel.Width, 0);
            lblConteo.Left = lblReporte.Right + 10;
        }

        // ══════════════════════════════════════════════════════════
        // CONSTRUCCIÓN
        // ══════════════════════════════════════════════════════════
        private void ConstruirKpis()
        {
            var tiles = new[] { kpiVencidos, kpiActivos, kpiDisponibles, kpiMantenimiento, kpiDanadas };
            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i].Dock = DockStyle.Fill;
                tiles[i].Margin = new Padding(0, 0, i < tiles.Length - 1 ? 10 : 0, 0);
                tlpKpis.Controls.Add(tiles[i], i, 0);
            }
        }

        // Cada control de filtro va dentro de un "campo": etiqueta pequeña en mayúsculas + control
        private void ConstruirFiltros()
        {
            Campo("Empleado", pickEmpleado, 230);
            Campo("Herramienta", pickHerramienta, 210);
            Campo("Estado", cboEstadoPrestamo, 130);
            Campo("Desde", dtpDesde, 130);
            Campo("Hasta", dtpHasta, 130);
            Campo("Categoría", cboCategoria, 190);
            Campo("Departamento", cboDepartamento, 230);
            Campo("Tipo", cboTipoMant, 150);
            Campo("Estado", cboEstadoMant, 130);
            Campo("Condición", cboCondicion, 130);
            Campo("Mostrar", cboMostrar, 220);

            Llenar(cboEstadoPrestamo, new Opcion(null, "Todos"), new Opcion("Activo", "Activo"),
                   new Opcion("Cerrado", "Cerrado"), new Opcion("Vencido", "Vencido"));
            Llenar(cboTipoMant, new Opcion(null, "Todos"), new Opcion("Preventivo", "Preventivo"),
                   new Opcion("Correctivo", "Correctivo"), new Opcion("Calibración", "Calibración"));
            Llenar(cboEstadoMant, new Opcion(null, "Todos"), new Opcion("Activo", "Activo"), new Opcion("Cerrado", "Cerrado"));
            Llenar(cboCondicion, new Opcion(null, "Todas"), new Opcion("Dañada", "Dañada"), new Opcion("Perdida", "Perdida"));
            Llenar(cboMostrar, new Opcion("Todas", "Todas"), new Opcion("StockBajo", "Solo stock bajo (< 30 %)"),
                   new Opcion("SinDisponibles", "Solo sin disponibles"));

            dtpDesde.Value = DateTime.Today.AddDays(-30);
            dtpHasta.Value = DateTime.Today;

            _info[Reporte.Historial] = new("Historial de préstamos", "Préstamos por fecha, empleado y estado", IconChar.ClockRotateLeft,
                new Control[] { pickEmpleado, pickHerramienta, cboEstadoPrestamo, dtpDesde, dtpHasta }, true);
            _info[Reporte.Vencidos] = new("Vencidos / Pendientes", "Préstamos fuera de plazo hoy", IconChar.CalendarXmark,
                new Control[] { pickEmpleado, cboCategoria }, false);
            _info[Reporte.PorEmpleado] = new("Préstamos por empleado", "Actividad y atrasos por persona", IconChar.Users,
                new Control[] { dtpDesde, dtpHasta, cboDepartamento }, true);
            _info[Reporte.Ranking] = new("Herramientas más usadas", "Ranking de demanda en el período", IconChar.Trophy,
                new Control[] { dtpDesde, dtpHasta, cboCategoria }, true);
            _info[Reporte.Mantenimiento] = new("Historial de mantenimiento", "Costos, duración y técnicos", IconChar.ScrewdriverWrench,
                new Control[] { dtpDesde, dtpHasta, cboTipoMant, cboEstadoMant }, true);
            _info[Reporte.Danadas] = new("Dañadas y perdidas", "Incidentes en devoluciones", IconChar.TriangleExclamation,
                new Control[] { dtpDesde, dtpHasta, cboCondicion, pickEmpleado }, true);
            _info[Reporte.Inventario] = new("Inventario por categoría", "Disponibilidad actual del stock", IconChar.BoxesStacked,
                new Control[] { cboCategoria, cboMostrar }, false);
        }

        private void Campo(string etiqueta, Control control, int ancho)
        {
            var panel = new Panel { Size = new Size(ancho, 64), Margin = new Padding(0, 0, 14, 0), BackColor = Color.White };
            var lbl = new Label
            {
                Text = etiqueta.ToUpperInvariant(),
                Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                ForeColor = ThemeManager.TextSecondary,
                AutoSize = true,
                Location = new Point(0, 0)
            };
            control.Width = ancho;
            control.Location = new Point(0, control is DateTimePicker ? 26 : 20);
            panel.Controls.Add(lbl);
            panel.Controls.Add(control);
            _campos[control] = panel;
        }

        private static void Llenar(MaterialComboBox cbo, params Opcion[] opciones)
        {
            cbo.Items.Clear();
            cbo.Items.AddRange(opciones);
            cbo.SelectedIndex = 0;
        }

        private static object? Valor(MaterialComboBox cbo) => (cbo.SelectedItem as Opcion)?.Valor;
        private static int? ValorInt(MaterialComboBox cbo) => Valor(cbo) as int?;
        private static string? ValorTexto(MaterialComboBox cbo) => Valor(cbo) as string;
        private int? EmpleadoFiltro => pickEmpleado.EmpleadoId > 0 ? pickEmpleado.EmpleadoId : null;
        private int? HerramientaFiltro => pickHerramienta.HerramientaId > 0 ? pickHerramienta.HerramientaId : null;

        private void ConstruirTarjetas()
        {
            foreach (var (tipo, info) in _info)
            {
                var card = new ReporteCard(info.Titulo, info.Descripcion, info.Icono);
                card.Click += (s, e) => { SeleccionarReporte(tipo); Generar(); };
                _tarjetas[tipo] = card;
                flpTarjetas.Controls.Add(card);
            }
        }

        // Las 7 tarjetas se reparten el ancho disponible; si no caben (mín. 150px), pasan a otra fila
        private void AjustarTarjetas()
        {
            int n = _tarjetas.Count;
            if (n == 0) return;
            int ancho = (flpTarjetas.ClientSize.Width - 8 * (n - 1)) / n;
            ancho = Math.Max(150, ancho - 1);
            foreach (var card in _tarjetas.Values)
            {
                card.Width = ancho;
                card.Margin = new Padding(0, 0, card == _tarjetas.Values.Last() ? 0 : 8, 8);
            }
        }

        private void ConstruirGrid()
        {
            dgvReporte.Dock = DockStyle.Fill;
            dgvReporte.AutoGenerateColumns = false;
            EmpleadoGrid.Configurar(dgvReporte);
            dgvReporte.ColumnHeadersHeight = 32;
            dgvReporte.ShowCellToolTips = true;
            dgvReporte.CellPainting += dgvReporte_CellPainting;
            dgvReporte.CellClick += dgvReporte_CellClick;
            dgvReporte.CellMouseMove += (s, e) =>
                dgvReporte.Cursor = e.RowIndex >= 0 && e.ColumnIndex >= 0 &&
                    (dgvReporte.Columns[e.ColumnIndex].Tag as Col)?.Render == Render.Notificar ? Cursors.Hand : Cursors.Default;
            pnlGridHost.Controls.Add(dgvReporte);
            dgvReporte.BringToFront();
        }

        // ══════════════════════════════════════════════════════════
        // CARGA INICIAL
        // ══════════════════════════════════════════════════════════
        private void FrmReportes_Load(object? sender, EventArgs e)
        {
            try
            {
                CargarListas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Errores.Mensaje(ex), "Error al cargar filtros", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            AlinearDerecha();
            SeleccionarReporte(Reporte.Historial);
            Generar();
        }

        private void CargarListas()
        {
            pickEmpleado.EstablecerTodos("Todos los empleados");
            pickHerramienta.EstablecerTodas("Todas las herramientas");

            var cat = CategoriaService.ObtenerTodas();
            Llenar(cboCategoria, new[] { new Opcion(null, "Todas las categorías") }
                .Concat(cat.AsEnumerable().Select(r => new Opcion(r.Field<int>("CategoriaId"), r.Field<string>("Nombre") ?? ""))).ToArray());

            var dep = Db.Query("SELECT DepartamentoId, Nombre FROM Departamento ORDER BY Nombre");
            Llenar(cboDepartamento, new[] { new Opcion(null, "Todos los departamentos") }
                .Concat(dep.AsEnumerable().Select(r => new Opcion(r.Field<int>("DepartamentoId"), r.Field<string>("Nombre") ?? ""))).ToArray());
        }

        private void CargarKpis()
        {
            try
            {
                var k = ReporteService.KPIs();
                int I(string c) => Convert.ToInt32(k[c]);
                string Cambio(int n, string texto, string sinCambio) => n > 0 ? $"↑ {n} {texto}" : sinCambio;

                kpiVencidos.Valor = I("Vencidos");
                kpiVencidos.Delta = Cambio(I("VencidosHoy"), I("VencidosHoy") == 1 ? "venció hoy" : "vencieron hoy", "= ninguno nuevo hoy");
                kpiActivos.Valor = I("Activos");
                kpiActivos.Delta = Cambio(I("ActivosHoy"), I("ActivosHoy") == 1 ? "nuevo hoy" : "nuevos hoy", "= sin préstamos nuevos hoy");
                kpiDisponibles.Valor = I("Disponibles");
                kpiDisponibles.Delta = Cambio(I("DevueltasHoy"), I("DevueltasHoy") == 1 ? "devuelta hoy" : "devueltas hoy", "= sin devoluciones hoy");
                kpiMantenimiento.Valor = I("EnMantenimiento");
                kpiMantenimiento.Delta = Cambio(I("MantenimientoHoy"), I("MantenimientoHoy") == 1 ? "ingresó hoy" : "ingresaron hoy", "= sin ingresos hoy");
                kpiDanadas.Valor = I("Danadas");
                kpiDanadas.Delta = Cambio(I("DanadasHoy"), I("DanadasHoy") == 1 ? "reportada hoy" : "reportadas hoy", "= sin reportes hoy");

                lblSubtitulo.Text = $"Indicadores al {DateTime.Now:dd/MM/yyyy HH:mm}";
            }
            catch (Exception ex)
            {
                lblSubtitulo.Text = "No se pudieron cargar los indicadores: " + Errores.Mensaje(ex);
            }
        }

        // ══════════════════════════════════════════════════════════
        // SELECCIÓN Y GENERACIÓN
        // ══════════════════════════════════════════════════════════
        private void SeleccionarReporte(Reporte tipo)
        {
            _reporte = tipo;
            foreach (var (t, card) in _tarjetas) card.Seleccionada = t == tipo;

            flpFiltros.SuspendLayout();
            flpFiltros.Controls.Clear();
            foreach (var c in _info[tipo].Filtros) flpFiltros.Controls.Add(_campos[c]);
            flpFiltros.ResumeLayout();
        }

        private void Generar()
        {
            if (_info[_reporte].UsaFechas && dtpDesde.Value.Date > dtpHasta.Value.Date)
            {
                MessageBox.Show("La fecha \"Desde\" no puede ser posterior a \"Hasta\".", "Fechas",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            CargarKpis();
            Cursor = Cursors.WaitCursor;
            try
            {
                switch (_reporte)
                {
                    case Reporte.Historial: CargarHistorial(); break;
                    case Reporte.Vencidos: CargarVencidos(); break;
                    case Reporte.PorEmpleado: CargarPorEmpleado(); break;
                    case Reporte.Ranking: CargarRanking(); break;
                    case Reporte.Mantenimiento: CargarMantenimiento(); break;
                    case Reporte.Danadas: CargarDanadas(); break;
                    case Reporte.Inventario: CargarInventario(); break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Errores.Mensaje(ex), "Error al generar reporte", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { Cursor = Cursors.Default; }
        }

        // Línea que acompaña al título en las exportaciones
        private string PeriodoFechas => $"Período: {dtpDesde.Value:dd/MM/yyyy} — {dtpHasta.Value:dd/MM/yyyy}";

        private void CargarHistorial()
        {
            var dt = ReporteService.HistorialPorPrestamo(EmpleadoFiltro, HerramientaFiltro,
                dtpDesde.Value, dtpHasta.Value, ValorTexto(cboEstadoPrestamo));
            Mostrar(dt, PeriodoFechas, new()
            {
                new("PrestamoId", "Préstamo", Render.Prestamo, 80),
                new("Empleado", "Empleado", Render.Empleado, 0, 30),
                new("Herramientas", "Herramientas", Render.Texto, 0, 34),
                new("FechaPrestamo", "Fecha préstamo", Render.FechaHora, 130),
                new("FechaDevolucionEsperada", "Devolución esperada", Render.Fecha, 130),
                new("FechaCierre", "Fecha cierre", Render.Fecha, 110),
                new("Estado", "Estado", Render.EstadoPrestamo, 110),
            });
        }

        private void CargarVencidos()
        {
            var dt = ReporteService.Vencidos(EmpleadoFiltro, ValorInt(cboCategoria));
            Mostrar(dt, $"Vencidos al {DateTime.Now:dd/MM/yyyy}", new()
            {
                new("PrestamoId", "Préstamo", Render.Prestamo, 80),
                new("Empleado", "Empleado", Render.Empleado, 0, 34),
                new("Herramientas", "Herramientas", Render.Texto, 0, 40),
                new("FechaDevolucionEsperada", "Debía devolver", Render.Fecha, 120),
                new("DiasVencido", "Días vencido", Render.DiasVencido, 110),
                new("PrestamoId", "", Render.Notificar, 110),
            });
        }

        private void CargarPorEmpleado()
        {
            var dt = ReporteService.PrestamosPorEmpleado(dtpDesde.Value, dtpHasta.Value, ValorInt(cboDepartamento));
            Mostrar(dt, PeriodoFechas, new()
            {
                new("Empleado", "Empleado", Render.Empleado, 0, 40),
                new("TotalPrestamos", "Total préstamos", Render.NumeroGrande, 120),
                new("Activos", "Activos ahora", Render.ActivosEmpleado, 120),
                new("Vencidos", "Vencidos", Render.Numero, 90),
                new("Devueltos", "Devueltos", Render.Numero, 90),
                new("HerramientasUnicas", "Herramientas únicas", Render.Numero, 140),
            });
        }

        private void CargarRanking()
        {
            var dt = ReporteService.RankingHerramientas(dtpDesde.Value, dtpHasta.Value, ValorInt(cboCategoria));

            // Ranking y demanda relativa al primero
            dt.Columns.Add("Rank", typeof(int));
            dt.Columns.Add("Pct", typeof(double));
            int max = dt.Rows.Count > 0 ? dt.AsEnumerable().Max(r => r.Field<int>("TotalPrestamos")) : 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["Rank"] = i + 1;
                dt.Rows[i]["Pct"] = max > 0 ? 100.0 * dt.Rows[i].Field<int>("TotalPrestamos") / max : 0;
            }

            Mostrar(dt, PeriodoFechas, new()
            {
                new("Rank", "#", Render.Rank, 56),
                new("Herramienta", "Herramienta", Render.HerramientaRanking, 0, 40),
                new("Categoria", "Categoría", Render.Categoria, 160),
                new("TotalPrestamos", "Préstamos", Render.NumeroGrande, 100),
                new("Pct", "Demanda", Render.Barra, 0, 28),
                new("DiasPromedio", "Días prom. por préstamo", Render.Decimal1, 170),
            });
        }

        private void CargarMantenimiento()
        {
            var dt = ReporteService.HistorialMantenimiento(dtpDesde.Value, dtpHasta.Value,
                ValorTexto(cboTipoMant), ValorTexto(cboEstadoMant));
            Mostrar(dt, PeriodoFechas, new()
            {
                new("Herramienta", "Herramienta", Render.HerramientaUnidad, 0, 30),
                new("Tipo", "Tipo", Render.TipoMantenimiento, 120),
                new("RealizadoPor", "Realizado por", Render.Texto, 0, 18),
                new("Servicio", "Servicio", Render.Texto, 90),
                new("FechaInicio", "Fecha inicio", Render.Fecha, 110),
                new("FechaFin", "Fecha cierre", Render.Fecha, 110),
                new("Duracion", "Duración", Render.Duracion, 100),
                new("Costo", "Costo", Render.Costo, 120),
            });

            // Resumen sobre la grilla
            var filas = dt.AsEnumerable().ToList();
            decimal costo = filas.Where(r => r["Costo"] != DBNull.Value).Sum(r => Convert.ToDecimal(r["Costo"]));
            double duracion = filas.Count > 0 ? filas.Average(r => Convert.ToInt32(r["Duracion"])) : 0;
            int herramientas = filas.Select(r => r.Field<int>("HerramientaId")).Distinct().Count();
            lblResumen.Text = $"Total: {filas.Count}     |     Costo total: {Moneda} {costo:N2}     |     " +
                              $"Duración promedio: {duracion:0.0} días     |     Herramientas distintas: {herramientas}";
            lblResumen.Visible = true;
        }

        private void CargarDanadas()
        {
            var dt = ReporteService.DanadasPerdidas(dtpDesde.Value, dtpHasta.Value,
                ValorTexto(cboCondicion), EmpleadoFiltro);
            Mostrar(dt, PeriodoFechas, new()
            {
                new("Herramienta", "Herramienta / Unidad", Render.HerramientaUnidad, 0, 28),
                new("Condicion", "Condición", Render.Condicion, 110),
                new("Empleado", "Responsable", Render.Empleado, 0, 30),
                new("PrestamoId", "Préstamo", Render.Prestamo, 90),
                new("FechaDevolucion", "Fecha devolución", Render.Fecha, 130),
                new("Nota", "Nota", Render.Secundario, 0, 24),
            });

            // Sin incidentes: mensaje positivo en lugar de "sin resultados"
            if (dt.Rows.Count == 0)
            {
                lblSinResultados.Visible = false;
                pnlSinIncidentes.Visible = true;
                CentrarSinIncidentes();
            }
        }

        private void CargarInventario()
        {
            var dt = ReporteService.InventarioCategoria(ValorInt(cboCategoria), ValorTexto(cboMostrar) ?? "Todas");
            Mostrar(dt, $"Inventario al {DateTime.Now:dd/MM/yyyy HH:mm}", new()
            {
                new("Herramienta", "Herramienta", Render.HerramientaRanking, 0, 34),
                new("Categoria", "Categoría", Render.Categoria, 150),
                new("Total", "Total", Render.Numero, 70),
                new("Disponibles", "Disponibles", Render.Disponibles, 100),
                new("Prestadas", "Prestadas", Render.Numero, 90),
                new("EnMantenimiento", "En mant.", Render.Numero, 80),
                new("Danadas", "Dañadas", Render.Numero, 80),
                new("PctDisponible", "Disponibilidad", Render.Barra, 0, 26),
            });
        }

        // Enlaza los datos y arma las columnas del reporte
        private void Mostrar(DataTable dt, string periodo, List<Col> columnas)
        {
            _datos = dt;
            _columnas = columnas;
            _periodo = periodo;

            dgvReporte.SuspendLayout();
            dgvReporte.DataSource = null;
            dgvReporte.Columns.Clear();
            foreach (var c in columnas)
            {
                var col = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = c.Campo,
                    HeaderText = c.Titulo.ToUpperInvariant(),
                    Tag = c,
                    ReadOnly = true,
                    SortMode = DataGridViewColumnSortMode.NotSortable,
                    AutoSizeMode = c.Ancho > 0 ? DataGridViewAutoSizeColumnMode.None : DataGridViewAutoSizeColumnMode.Fill
                };
                if (c.Ancho > 0) col.Width = c.Ancho; else col.FillWeight = c.Peso;
                dgvReporte.Columns.Add(col);
            }
            dgvReporte.DataSource = dt;
            dgvReporte.ResumeLayout();
            dgvReporte.ClearSelection();
            EmpleadoGrid.ReiniciarHover(dgvReporte);

            lblReporte.Text = _info[_reporte].Titulo;
            lblConteo.Text = dt.Rows.Count == 1 ? "1 registro encontrado" : $"{dt.Rows.Count} registros encontrados";
            lblResumen.Visible = false;
            pnlSinIncidentes.Visible = false;
            // Sin filas: se oculta la grilla y queda el aviso (no depende del orden de los controles)
            dgvReporte.Visible = dt.Rows.Count > 0;
            lblSinResultados.Visible = dt.Rows.Count == 0;
            AlinearDerecha();

            // Al enlazar, la grilla preselecciona la primera fila: se quita
            BeginInvoke(new Action(() => dgvReporte.ClearSelection()));
        }

        private void CentrarSinIncidentes()
        {
            int cx = pnlSinIncidentes.Width / 2, cy = pnlSinIncidentes.Height / 2;
            icoSinIncidentes.Location = new Point(cx - icoSinIncidentes.Width / 2, cy - 50);
            lblSinIncidentes.SetBounds(20, icoSinIncidentes.Bottom + 10, Math.Max(100, pnlSinIncidentes.Width - 40), 30);
        }

        // ══════════════════════════════════════════════════════════
        // DIBUJO DE CELDAS
        // ══════════════════════════════════════════════════════════
        private static string S(DataRowView r, string campo) => r.Row.Table.Columns.Contains(campo) ? r[campo]?.ToString() ?? "" : "";
        private static int I(DataRowView r, string campo) => r[campo] == DBNull.Value ? 0 : Convert.ToInt32(r[campo]);
        private static DateTime? F(DataRowView r, string campo) => r[campo] is DateTime d ? d : null;

        private void dgvReporte_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Graphics == null) return;
            if (dgvReporte.Columns[e.ColumnIndex].Tag is not Col c || dgvReporte.Rows[e.RowIndex].DataBoundItem is not DataRowView r) return;

            EmpleadoGrid.PintarFondo(dgvReporte, e);
            var g = e.Graphics;
            var b = e.CellBounds;

            switch (c.Render)
            {
                case Render.Texto:
                    EmpleadoGrid.DibujarTexto(g, b, S(r, c.Campo), dgvReporte.DefaultCellStyle.Font!, ThemeManager.TextPrimary);
                    break;
                case Render.Secundario:
                    EmpleadoGrid.DibujarTexto(g, b, S(r, c.Campo) is { Length: > 0 } t ? t : "—",
                        dgvReporte.DefaultCellStyle.Font!, ThemeManager.TextSecondary);
                    break;
                case Render.Prestamo:
                    EmpleadoGrid.DibujarTexto(g, b, $"#{S(r, c.Campo)}", FuenteMono, ThemeManager.TextSecondary);
                    break;
                case Render.Empleado:
                    DibujarEmpleado(g, b, S(r, "CodigoEmpleado"), S(r, "Empleado"), S(r, "Departamento"));
                    break;
                case Render.Fecha:
                    EmpleadoGrid.DibujarTexto(g, b, F(r, c.Campo)?.ToString("dd/MM/yyyy") ?? "—",
                        dgvReporte.DefaultCellStyle.Font!, F(r, c.Campo) == null ? ThemeManager.TextSecondary : ThemeManager.TextPrimary);
                    break;
                case Render.FechaHora:
                    EmpleadoGrid.DibujarTexto(g, b, F(r, c.Campo)?.ToString("dd/MM/yyyy HH:mm") ?? "—",
                        dgvReporte.DefaultCellStyle.Font!, ThemeManager.TextPrimary);
                    break;
                case Render.EstadoPrestamo:
                {
                    var (texto, fondo, frente) = S(r, c.Campo) switch
                    {
                        "Vencido" => ("● Vencido", Paleta.RojoSuave, Paleta.Rojo),
                        "Cerrado" => ("Cerrado", Paleta.GrisSuave, Paleta.Gris),
                        _ => ("Activo", ThemeManager.AccentBlueSoft, ThemeManager.AccentBlue)
                    };
                    StatusChip.DibujarEnCelda(g, b, texto, fondo, frente);
                    break;
                }
                case Render.DiasVencido:
                {
                    int d = I(r, c.Campo);
                    EmpleadoGrid.DibujarTexto(g, b, d == 1 ? "1 día" : $"{d} días", FuenteNegrita, Paleta.Rojo);
                    break;
                }
                case Render.Notificar:
                    DibujarBoton(g, b, "Notificar");
                    break;
                case Render.ActivosEmpleado:
                {
                    int activos = I(r, c.Campo);
                    bool conVencidos = I(r, "Vencidos") > 0;
                    if (activos == 0)
                        EmpleadoGrid.DibujarTexto(g, b, "0", dgvReporte.DefaultCellStyle.Font!, ThemeManager.TextSecondary);
                    else
                        StatusChip.DibujarEnCelda(g, b, activos.ToString(),
                            conVencidos ? Paleta.RojoSuave : ThemeManager.AccentBlueSoft,
                            conVencidos ? Paleta.Rojo : ThemeManager.AccentBlue);
                    break;
                }
                case Render.Numero:
                    EmpleadoGrid.DibujarTexto(g, b, I(r, c.Campo).ToString("N0"), dgvReporte.DefaultCellStyle.Font!, ThemeManager.TextPrimary);
                    break;
                case Render.NumeroGrande:
                    EmpleadoGrid.DibujarTexto(g, b, I(r, c.Campo).ToString("N0"), FuenteGrande, ThemeManager.TextPrimary);
                    break;
                case Render.Decimal1:
                    EmpleadoGrid.DibujarTexto(g, b, r[c.Campo] == DBNull.Value ? "—" : Convert.ToDouble(r[c.Campo]).ToString("0.0"),
                        dgvReporte.DefaultCellStyle.Font!, ThemeManager.TextPrimary);
                    break;
                case Render.Rank:
                    DibujarRank(g, b, I(r, c.Campo));
                    break;
                case Render.HerramientaRanking:
                {
                    string sub = r.Row.Table.Columns.Contains("TotalUnidades")
                        ? $"{S(r, "Codigo")}  ·  {I(r, "TotalUnidades")} unidades"
                        : S(r, "Codigo");
                    DibujarDosLineas(g, b, S(r, c.Campo), sub, FuenteMono);
                    break;
                }
                case Render.HerramientaUnidad:
                    DibujarDosLineas(g, b, S(r, c.Campo), S(r, "CodigoUnidad"), FuenteMono);
                    break;
                case Render.Categoria:
                    EmpleadoGrid.DibujarChipDepartamento(g, b, S(r, c.Campo) is { Length: > 0 } cat ? cat : "—");
                    break;
                case Render.Barra:
                {
                    double pct = r[c.Campo] == DBNull.Value ? 0 : Convert.ToDouble(r[c.Campo]);
                    // Inventario: verde > 60 %, amarillo 20-60 %, rojo < 20 %. Ranking: azul.
                    Color color = _reporte == Reporte.Inventario
                        ? (pct > 60 ? Paleta.Verde : pct >= 20 ? Paleta.Amarillo : Paleta.Rojo)
                        : ThemeManager.AccentBlue;
                    DibujarBarra(g, b, pct, color);
                    break;
                }
                case Render.TipoMantenimiento:
                {
                    string tipo = S(r, c.Campo);
                    var (fondo, frente) = StatusChip.ColoresTipoMantenimiento(tipo);
                    StatusChip.DibujarEnCelda(g, b, tipo, fondo, frente);
                    break;
                }
                case Render.Duracion:
                {
                    int d = I(r, c.Campo);
                    var color = d > 15 ? Paleta.Rojo : d >= 7 ? Paleta.Amarillo : ThemeManager.TextPrimary;
                    EmpleadoGrid.DibujarTexto(g, b, d == 1 ? "1 día" : $"{d} días", d >= 7 ? FuenteNegrita : dgvReporte.DefaultCellStyle.Font!, color);
                    break;
                }
                case Render.Costo:
                    if (r["FechaFin"] == DBNull.Value)
                        EmpleadoGrid.DibujarTexto(g, b, "Pendiente", dgvReporte.DefaultCellStyle.Font!, ThemeManager.TextSecondary);
                    else if (r["EnGarantia"] is true)
                        EmpleadoGrid.DibujarTexto(g, b, "Garantía", dgvReporte.DefaultCellStyle.Font!, ThemeManager.TextSecondary);
                    else
                        EmpleadoGrid.DibujarTexto(g, b, r[c.Campo] == DBNull.Value ? "—" : $"{Moneda} {Convert.ToDecimal(r[c.Campo]):N2}",
                            dgvReporte.DefaultCellStyle.Font!, ThemeManager.TextPrimary);
                    break;
                case Render.Condicion:
                {
                    string cond = S(r, c.Campo);
                    var (fondo, frente) = cond == "Perdida" ? (Paleta.RojoSuave, Paleta.Rojo) : (Paleta.AmarilloSuave, Paleta.Amarillo);
                    StatusChip.DibujarEnCelda(g, b, cond, fondo, frente);
                    break;
                }
                case Render.Disponibles:
                {
                    double pct = r["PctDisponible"] == DBNull.Value ? 0 : Convert.ToDouble(r["PctDisponible"]);
                    var color = pct > 60 ? Paleta.Verde : pct >= 20 ? Paleta.Amarillo : Paleta.Rojo;
                    EmpleadoGrid.DibujarTexto(g, b, I(r, c.Campo).ToString(), FuenteNegrita, color);
                    break;
                }
            }
            e.Handled = true;
        }

        // Avatar 32px + nombre en negrita + departamento pequeño
        private static void DibujarEmpleado(Graphics g, Rectangle b, string codigo, string nombre, string depto)
        {
            var celdaAvatar = new Rectangle(b.X + 4, b.Y, 40, b.Height);
            EmpleadoGrid.DibujarAvatar(g, celdaAvatar, codigo, nombre);
            var texto = new Rectangle(b.X + 50, b.Y, Math.Max(10, b.Width - 56), b.Height);
            DibujarDosLineas(g, new Rectangle(texto.X - 8, b.Y, texto.Width + 8, b.Height), nombre,
                string.IsNullOrEmpty(depto) ? "Sin departamento" : depto, FuenteChica);
        }

        private static void DibujarDosLineas(Graphics g, Rectangle b, string principal, string secundario, Font fuenteSecundaria)
        {
            var flags = TextFormatFlags.Left | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding;
            TextRenderer.DrawText(g, principal, FuenteNegrita, new Rectangle(b.X + 8, b.Y + 6, b.Width - 14, 18), ThemeManager.TextPrimary, flags);
            TextRenderer.DrawText(g, secundario, fuenteSecundaria, new Rectangle(b.X + 8, b.Y + 25, b.Width - 14, 16), ThemeManager.TextSecondary, flags);
        }

        // Cuadro con el puesto: oro / plata / bronce para el top 3
        private static void DibujarRank(Graphics g, Rectangle b, int rank)
        {
            var (fondo, frente) = rank switch
            {
                1 => (Color.FromArgb(234, 179, 8), Color.White),
                2 => (Color.FromArgb(148, 163, 184), Color.White),
                3 => (Color.FromArgb(180, 83, 9), Color.White),
                _ => (Paleta.Surface2, ThemeManager.TextSecondary)
            };
            var r = new Rectangle(b.X + (b.Width - 28) / 2, b.Y + (b.Height - 28) / 2, 28, 28);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (var path = RoundedGeometry.RoundedRect(r, 6))
            using (var brush = new SolidBrush(fondo))
                g.FillPath(brush, path);
            TextRenderer.DrawText(g, rank.ToString(), FuenteNegrita, r, frente,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
        }

        private static void DibujarBarra(Graphics g, Rectangle b, double pct, Color color)
        {
            pct = Math.Max(0, Math.Min(100, pct));
            var fondo = new Rectangle(b.X + 10, b.Y + b.Height / 2 - 3, Math.Max(20, b.Width - 64), 6);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (var path = RoundedGeometry.RoundedRect(fondo, 3))
            using (var brush = new SolidBrush(Paleta.Surface2))
                g.FillPath(brush, path);
            int ancho = (int)(fondo.Width * pct / 100);
            if (ancho > 0)
                using (var path = RoundedGeometry.RoundedRect(new Rectangle(fondo.X, fondo.Y, Math.Max(6, ancho), 6), 3))
                using (var brush = new SolidBrush(color))
                    g.FillPath(brush, path);
            TextRenderer.DrawText(g, $"{pct:0}%", FuenteChica, new Rectangle(fondo.Right + 6, b.Y, 46, b.Height), color,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
        }

        private static void DibujarBoton(Graphics g, Rectangle b, string texto)
        {
            var r = new Rectangle(b.X + 8, b.Y + (b.Height - 28) / 2, Math.Min(96, b.Width - 16), 28);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (var path = RoundedGeometry.RoundedRect(r, 6))
            {
                using (var brush = new SolidBrush(Color.White)) g.FillPath(brush, path);
                using (var pen = new Pen(ThemeManager.AccentBlue)) g.DrawPath(pen, path);
            }
            TextRenderer.DrawText(g, texto, FuenteBoton, r, ThemeManager.AccentBlue,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        // "Notificar" (por ahora solo registra el aviso)
        private void dgvReporte_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if ((dgvReporte.Columns[e.ColumnIndex].Tag as Col)?.Render != Render.Notificar) return;
            if (dgvReporte.Rows[e.RowIndex].DataBoundItem is not DataRowView r) return;

            MessageBox.Show($"Notificación registrada para {S(r, "Empleado")} (préstamo #{S(r, "PrestamoId")}).",
                "Notificar", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ══════════════════════════════════════════════════════════
        // EXPORTACIÓN
        // ══════════════════════════════════════════════════════════
        private List<Col> ColumnasExportables => _columnas.Where(c => c.Render != Render.Notificar).ToList();

        // Texto plano de una celda; en PDF los estados van entre paréntesis: "(Activo)"
        private string TextoExport(DataRow row, Col c, bool pdf)
        {
            object v = row[c.Campo];
            string Fecha(string formato) => v is DateTime d ? d.ToString(formato) : "—";
            string Estado(string s) => pdf && s != "" ? $"({s})" : s;

            return c.Render switch
            {
                Render.Prestamo => $"#{v}",
                Render.Empleado => $"{row["Empleado"]} ({(row["Departamento"] is string d && d != "" ? d : "Sin departamento")})",
                Render.Fecha => Fecha("dd/MM/yyyy"),
                Render.FechaHora => Fecha("dd/MM/yyyy HH:mm"),
                Render.EstadoPrestamo or Render.TipoMantenimiento or Render.Condicion => Estado(v?.ToString() ?? ""),
                Render.DiasVencido or Render.Duracion => $"{v} días",
                Render.Barra => v == DBNull.Value ? "0%" : $"{Convert.ToDouble(v):0}%",
                Render.Decimal1 => v == DBNull.Value ? "—" : Convert.ToDouble(v).ToString("0.0"),
                Render.HerramientaRanking => $"{v} ({row["Codigo"]})",
                Render.HerramientaUnidad => $"{v} ({row["CodigoUnidad"]})",
                Render.Costo => row["FechaFin"] == DBNull.Value ? "Pendiente"
                               : row["EnGarantia"] is true ? "Garantía"
                               : v == DBNull.Value ? "—" : $"{Moneda} {Convert.ToDecimal(v):N2}",
                _ => v == DBNull.Value ? "—" : v?.ToString() ?? ""
            };
        }

        private bool HayDatos()
        {
            if (_datos != null && _datos.Rows.Count > 0) return true;
            MessageBox.Show("Genera un reporte con resultados primero.", "Sin datos",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return false;
        }

        private string Titulo => _info[_reporte].Titulo;

        private void ExportarExcel()
        {
            if (!HayDatos()) return;

            using var sfd = new SaveFileDialog
            {
                Filter = "Excel (*.xlsx)|*.xlsx",
                FileName = $"{Titulo} {DateTime.Now:yyyy-MM-dd}"
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                GuardarExcel(sfd.FileName);
                MessageBox.Show("Exportado a Excel correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(Errores.Mensaje(ex), "Error al exportar a Excel", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GuardarExcel(string ruta)
        {
            var cols = ColumnasExportables;
            {
                using var wb = new ClosedXML.Excel.XLWorkbook();
                var ws = wb.Worksheets.Add("Reporte");

                // Encabezado: nombre, período y fecha de generación
                ws.Cell(1, 1).Value = Titulo.ToUpper();
                ws.Cell(1, 1).Style.Font.Bold = true;
                ws.Cell(1, 1).Style.Font.FontSize = 16;
                ws.Cell(1, 1).Style.Font.FontColor = ClosedXML.Excel.XLColor.FromHtml("#1a1d23");
                ws.Range(1, 1, 1, cols.Count).Merge();
                ws.Cell(2, 1).Value = _periodo;
                ws.Cell(3, 1).Value = "Generado el: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                ws.Cell(2, 1).Style.Font.FontColor = ClosedXML.Excel.XLColor.FromHtml("#5a6275");
                ws.Cell(3, 1).Style.Font.FontColor = ClosedXML.Excel.XLColor.FromHtml("#5a6275");

                const int filaInicio = 5;
                for (int i = 0; i < cols.Count; i++)
                {
                    var celda = ws.Cell(filaInicio, i + 1);
                    celda.Value = cols[i].Titulo;
                    celda.Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.FromHtml("#f0f2f6");
                    celda.Style.Font.FontColor = ClosedXML.Excel.XLColor.FromHtml("#5a6275");
                    celda.Style.Font.Bold = true;
                }

                for (int f = 0; f < _datos!.Rows.Count; f++)
                {
                    for (int i = 0; i < cols.Count; i++)
                    {
                        var celda = ws.Cell(filaInicio + 1 + f, i + 1);
                        celda.Value = TextoExport(_datos.Rows[f], cols[i], pdf: false);
                        celda.Style.Border.BottomBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                        celda.Style.Border.BottomBorderColor = ClosedXML.Excel.XLColor.FromHtml("#e2e6ed");
                        if (f % 2 == 1)
                            celda.Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.FromHtml("#f7f8fa");
                    }
                }

                ws.Columns().AdjustToContents();
                wb.SaveAs(ruta);
            }
        }

        private void ExportarPDF()
        {
            if (!HayDatos()) return;

            using var sfd = new SaveFileDialog
            {
                Filter = "PDF (*.pdf)|*.pdf",
                FileName = $"{Titulo} {DateTime.Now:yyyy-MM-dd}"
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                GuardarPDF(sfd.FileName);
                MessageBox.Show("Reporte PDF generado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(Errores.Mensaje(ex), "Error al exportar a PDF", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GuardarPDF(string ruta)
        {
            var cols = ColumnasExportables;
            {
                using var writer = new PdfWriter(ruta);
                using var pdf = new PdfDocument(writer);
                using var doc = new Document(pdf, iText.Kernel.Geom.PageSize.A4.Rotate());
                doc.SetMargins(30, 30, 30, 30);

                doc.Add(new Paragraph(Titulo.ToUpper())
                    .SetFontSize(16)
                    .SetFontColor(new DeviceRgb(26, 29, 35)));
                doc.Add(new Paragraph($"{_periodo}   |   Generado el: {DateTime.Now:dd/MM/yyyy HH:mm}")
                    .SetFontSize(9)
                    .SetFontColor(new DeviceRgb(90, 98, 117))
                    .SetMarginBottom(12));

                var table = new Table(UnitValue.CreatePercentArray(cols.Count)).UseAllAvailableWidth();
                foreach (var c in cols)
                {
                    table.AddHeaderCell(new Cell()
                        .Add(new Paragraph(c.Titulo.ToUpper()).SetFontSize(8))
                        .SetBackgroundColor(new DeviceRgb(240, 242, 246))
                        .SetFontColor(new DeviceRgb(90, 98, 117))
                        .SetPadding(5));
                }

                for (int f = 0; f < _datos!.Rows.Count; f++)
                {
                    foreach (var c in cols)
                    {
                        var celda = new Cell()
                            .Add(new Paragraph(TextoExport(_datos.Rows[f], c, pdf: true)))
                            .SetPadding(4)
                            .SetFontSize(8);
                        if (f % 2 == 1) celda.SetBackgroundColor(new DeviceRgb(247, 248, 250));
                        table.AddCell(celda);
                    }
                }

                doc.Add(table);
                doc.Close();
            }
        }
    }
}
