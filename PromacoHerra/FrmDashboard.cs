using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using FontAwesome.Sharp;
using PromacoHerra.Controls;
using PromacoHerra.Services;

namespace PromacoHerra
{
    // Tablero de inicio. Todo se dibuja con GDI+ (sin DataGridView):
    //   KPIs · dona de unidades por estado · líneas de actividad (30 días)
    //   barras por departamento · próximas devoluciones · sin stock + alertas · actividad de hoy
    // Se abre dentro de FrmPrincipal, que ya tiene el menú lateral de navegación.
    public partial class FrmDashboard : Form
    {
        // Colores: la paleta del proyecto (ThemeManager + Paleta)
        private static readonly Color BgPage = ThemeManager.AppBackground;
        private static readonly Color Surface = Color.White;
        private static readonly Color Surface2 = Paleta.Surface2;
        private static readonly Color Border = ThemeManager.BorderColor;
        private static readonly Color FgPrimary = ThemeManager.TextPrimary;
        private static readonly Color FgSecondary = ThemeManager.TextSecondary;
        private static readonly Color Accent = ThemeManager.AccentBlue;
        private static readonly Color AccentSoft = ThemeManager.AccentBlueSoft;

        private static readonly Font FuenteTitulo = new("Segoe UI Semibold", 10F);
        private static readonly Font FuenteSaludo = new("Segoe UI Semibold", 15F);
        private static readonly Font Fuente7 = new("Segoe UI", 7F);
        private static readonly Font Fuente8 = new("Segoe UI", 8F);
        private static readonly Font Fuente8B = new("Segoe UI", 8F, FontStyle.Bold);
        private static readonly Font Fuente8S = new("Segoe UI Semibold", 8.5F);
        private static readonly Font Fuente9 = new("Segoe UI", 9F);
        private static readonly Font Fuente9S = new("Segoe UI Semibold", 9F);
        private static readonly Font Fuente9B = new("Segoe UI", 9F, FontStyle.Bold);
        private static readonly Font FuenteTotal = new("Segoe UI", 18F, FontStyle.Bold);

        // KPIs
        private int _vencidos, _activos, _disponibles, _enMantenimiento, _daniadas;
        // Dona: unidades por estado
        private int _uDisponibles, _uPrestadas, _uVencidas, _uMantenimiento, _uDanadas;
        private List<(DateTime Fecha, int Prestamos, int Devoluciones)> _actividadMensual = new();
        private List<(string Depto, int Total)> _departamentos = new();
        private DataTable _proximasDevoluciones = new();
        private DataTable _sinStockList = new();
        private DataTable _actividadReciente = new();
        private string? _error;

        private readonly KpiTile kpiVencidos = new("Préstamos vencidos", IconChar.TriangleExclamation, Paleta.Rojo, Paleta.RojoSuave) { Clickeable = true };
        private readonly KpiTile kpiActivos = new("Préstamos activos", IconChar.Handshake, ThemeManager.AccentBlue, ThemeManager.AccentBlueSoft);
        private readonly KpiTile kpiDisponibles = new("Disponibles", IconChar.CircleCheck, Paleta.Verde, Paleta.VerdeSuave);
        private readonly KpiTile kpiMantenimiento = new("En mantenimiento", IconChar.ScrewdriverWrench, Paleta.Naranja, Paleta.NaranjaSuave);
        private readonly KpiTile kpiDanadas = new("Dañadas", IconChar.HeartCrack, Paleta.Violeta, Paleta.VioletaSuave);

        public FrmDashboard()
        {
            InitializeComponent();
            ThemeManager.ApplyTheme(this);

            BackColor = BgPage;
            pnlScroll.BackColor = BgPage;
            lblActividadTitulo.ForeColor = FgPrimary;
            foreach (var p in new Control[] { pnlDona, pnlLineas, pnlDepartamentos, pnlProximas, pnlStockAlertas })
                p.BackColor = BgPage;   // fuera de las esquinas redondeadas se ve el fondo de la página

            var tiles = new[] { kpiVencidos, kpiActivos, kpiDisponibles, kpiMantenimiento, kpiDanadas };
            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i].Dock = DockStyle.Fill;
                tiles[i].Margin = new Padding(0, 0, i < tiles.Length - 1 ? 12 : 0, 0);
                tlpKpis.Controls.Add(tiles[i], i, 0);
            }
            // Vencidos → pantalla de Devoluciones (ahí se resuelven)
            kpiVencidos.Click += (s, e) => { if (TopLevelControl is FrmPrincipal p) p.Navegar(new FrmDevolucion()); };

            pnlTopbar.Paint += PintarTopbar;
            pnlDona.Paint += PintarDona;
            pnlLineas.Paint += PintarLineas;
            pnlDepartamentos.Paint += PintarDepartamentos;
            pnlProximas.Paint += PintarProximas;
            pnlStockAlertas.Paint += PintarStockAlertas;

            this.Load += (s, e) => LoadDashboard();
        }

        // ══════════════════════════════════════════════════════════
        // DATOS
        // ══════════════════════════════════════════════════════════
        private void LoadDashboard()
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                var k = ReporteService.KPIs();
                _vencidos = Convert.ToInt32(k["Vencidos"]);
                _activos = Convert.ToInt32(k["Activos"]);
                _disponibles = Convert.ToInt32(k["Disponibles"]);
                _enMantenimiento = Convert.ToInt32(k["EnMantenimiento"]);
                _daniadas = Convert.ToInt32(k["Danadas"]);

                var u = DashboardService.DistribucionUnidades();
                int N(string c) => u[c] == DBNull.Value ? 0 : Convert.ToInt32(u[c]);
                _uDisponibles = N("Disponibles");
                _uPrestadas = N("Prestadas");
                _uVencidas = N("Vencidas");
                _uMantenimiento = N("EnMantenimiento");
                _uDanadas = N("Danadas");

                _actividadMensual = DashboardService.ActividadMensual().AsEnumerable()
                    .Select(r => (Convert.ToDateTime(r["Fecha"]), Convert.ToInt32(r["Prestamos"]), Convert.ToInt32(r["Devoluciones"])))
                    .ToList();
                _departamentos = DashboardService.PrestamosPorDepartamento().AsEnumerable()
                    .Select(r => (r["Departamento"].ToString() ?? "", Convert.ToInt32(r["Total"])))
                    .ToList();
                _proximasDevoluciones = DashboardService.ProximasDevoluciones();
                _sinStockList = DashboardService.HerramientasSinStock();
                _actividadReciente = DashboardService.ActividadReciente();
                _error = null;
            }
            catch (Exception ex)
            {
                _error = ex.Message;
            }
            finally
            {
                Cursor = Cursors.Default;
            }

            ActualizarKpis();
            ConstruirActividad();
            Invalidate(true);
        }

        private void ActualizarKpis()
        {
            kpiVencidos.Valor = _vencidos;
            kpiVencidos.Delta = _vencidos > 0 ? "requieren atención" : "todo al día";
            kpiVencidos.ColorDelta = _vencidos > 0 ? Paleta.Rojo : FgSecondary;
            kpiActivos.Valor = _activos;
            kpiActivos.Delta = "en préstamo ahora";
            kpiDisponibles.Valor = _disponibles;
            kpiDisponibles.Delta = "unidades listas";
            kpiMantenimiento.Valor = _enMantenimiento;
            kpiMantenimiento.Delta = "en reparación";
            kpiDanadas.Valor = _daniadas;
            kpiDanadas.Delta = "unidades registradas";
        }

        // ══════════════════════════════════════════════════════════
        // HELPERS DE DIBUJO
        // ══════════════════════════════════════════════════════════
        private static GraphicsPath RoundedRect(Rectangle r, int radius) => RoundedGeometry.RoundedRect(r, radius);

        // Tarjeta blanca redondeada con borde; devuelve el área interior
        private static Rectangle PintarTarjeta(Graphics g, Control c)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(BgPage);
            var r = new Rectangle(0, 0, c.Width - 1, c.Height - 1);
            using var path = RoundedRect(r, 10);
            using (var brush = new SolidBrush(Surface)) g.FillPath(brush, path);
            using (var pen = new Pen(Border)) g.DrawPath(pen, path);
            return r;
        }

        private static void Texto(Graphics g, string texto, Font fuente, Color color, Rectangle r,
            TextFormatFlags extra = TextFormatFlags.Left) =>
            TextRenderer.DrawText(g, texto, fuente, r, color,
                extra | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);

        private static Rectangle Chip(Graphics g, string texto, Color fondo, Color frente, int derecha, int centroY)
        {
            var tam = TextRenderer.MeasureText(g, texto, Fuente8B, Size.Empty, TextFormatFlags.NoPadding);
            var r = new Rectangle(derecha - tam.Width - 16, centroY - 11, tam.Width + 16, 22);
            using (var path = RoundedRect(r, 10))
            using (var brush = new SolidBrush(fondo))
                g.FillPath(brush, path);
            TextRenderer.DrawText(g, texto, Fuente8B, r, frente,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
            return r;
        }

        private static string Truncar(string? s, int max) =>
            string.IsNullOrEmpty(s) ? "" : s.Length <= max ? s : s[..(max - 1)].TrimEnd() + "…";

        private static string Plural(int n, string singular, string plural) => n == 1 ? $"1 {singular}" : $"{n} {plural}";

        private static void Vacio(Graphics g, Rectangle area, string texto, Color? color = null) =>
            TextRenderer.DrawText(g, texto, Fuente9, area, color ?? FgSecondary,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak);

        // ══════════════════════════════════════════════════════════
        // TOPBAR
        // ══════════════════════════════════════════════════════════
        private void PintarTopbar(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Surface);
            using (var pen = new Pen(Border))
                g.DrawLine(pen, 0, pnlTopbar.Height - 1, pnlTopbar.Width, pnlTopbar.Height - 1);

            int h = DateTime.Now.Hour;
            string saludo = h < 12 ? "Buenos días" : h < 19 ? "Buenas tardes" : "Buenas noches";
            string texto = $"{saludo}, {NombreUsuario()}";
            var tam = TextRenderer.MeasureText(g, texto, FuenteSaludo);
            TextRenderer.DrawText(g, texto, FuenteSaludo, new Point(22, (pnlTopbar.Height - tam.Height) / 2), FgPrimary);

            // Fecha a la derecha
            string fecha = DateTime.Now.ToString("dddd, d 'de' MMMM 'de' yyyy", new CultureInfo("es-HN"));
            fecha = char.ToUpper(fecha[0]) + fecha[1..];
            var tamFecha = TextRenderer.MeasureText(g, fecha, Fuente9);
            int xFecha = pnlTopbar.Width - 24 - tamFecha.Width;
            TextRenderer.DrawText(g, fecha, Fuente9, new Point(xFecha, (pnlTopbar.Height - tamFecha.Height) / 2), FgSecondary);

            // Resumen del día en una pastilla gris
            if (_error != null) return;
            int porVencer = _proximasDevoluciones.AsEnumerable().Count(r => Convert.ToInt32(r["DiasRestantes"]) >= 0);
            string resumen = $"Hoy: {Plural(_activos, "préstamo activo", "préstamos activos")}  ·  " +
                             $"{porVencer} por vencer en 7 días  ·  {Plural(_vencidos, "vencido", "vencidos")}";
            var tamRes = TextRenderer.MeasureText(g, resumen, Fuente9);
            int x = 22 + tam.Width + 14;
            var pill = new Rectangle(x, (pnlTopbar.Height - 30) / 2, tamRes.Width + 24, 30);
            if (pill.Right > xFecha - 12) return;   // no hay espacio: se omite antes que encimarse
            using (var path = RoundedRect(pill, 15))
            using (var brush = new SolidBrush(Surface2))
                g.FillPath(brush, path);
            TextRenderer.DrawText(g, resumen, Fuente9, pill, _vencidos > 0 ? Paleta.Rojo : FgSecondary,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        // Primer nombre del empleado en sesión ("ABNER ISAI ..." → "Abner")
        private static string NombreUsuario()
        {
            var nombre = Sesion.NombreEmpleado?.Trim();
            if (string.IsNullOrEmpty(nombre)) return string.IsNullOrEmpty(Sesion.Username) ? "bienvenido" : Sesion.Username;
            var primero = nombre.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0];
            return CultureInfo.GetCultureInfo("es-HN").TextInfo.ToTitleCase(primero.ToLowerInvariant());
        }

        // ══════════════════════════════════════════════════════════
        // DONA: unidades por estado
        // ══════════════════════════════════════════════════════════
        private void PintarDona(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            var card = PintarTarjeta(g, pnlDona);
            Texto(g, "Estado de las unidades", FuenteTitulo, FgPrimary, new Rectangle(14, 12, card.Width - 28, 20));

            var segmentos = new (string Nombre, int Valor, Color Color)[]
            {
                ("Disponibles", _uDisponibles, Paleta.Verde),
                ("Prestadas", _uPrestadas, Accent),
                ("Prestadas vencidas", _uVencidas, Paleta.Rojo),
                ("En mantenimiento", _uMantenimiento, Paleta.Naranja),
                ("Dañadas", _uDanadas, Paleta.Violeta),
            };
            int total = segmentos.Sum(s => s.Valor);

            int d = Math.Min(160, Math.Min(card.Width - 40, card.Height - 140));
            var circulo = new Rectangle((card.Width - d) / 2, 42, d, d);
            if (total == 0)
            {
                using var brush = new SolidBrush(Surface2);
                g.FillEllipse(brush, circulo);
            }
            else
            {
                float angulo = -90;
                foreach (var s in segmentos.Where(s => s.Valor > 0))
                {
                    float barrido = 360f * s.Valor / total;
                    using var brush = new SolidBrush(s.Color);
                    g.FillPie(brush, circulo, angulo, barrido);
                    angulo += barrido;
                }
            }

            // Hueco central con el total
            int hueco = d * 6 / 10;
            var centro = new Rectangle(circulo.X + (d - hueco) / 2, circulo.Y + (d - hueco) / 2, hueco, hueco);
            using (var brush = new SolidBrush(Surface)) g.FillEllipse(brush, centro);
            TextRenderer.DrawText(g, total.ToString("N0"), FuenteTotal,
                new Rectangle(centro.X, centro.Y + 4, centro.Width, centro.Height / 2), FgPrimary,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.Bottom);
            TextRenderer.DrawText(g, "unidades", Fuente8,
                new Rectangle(centro.X, centro.Y + centro.Height / 2 + 4, centro.Width, 16), FgSecondary,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.Top);

            // Leyenda en dos columnas
            int y0 = circulo.Bottom + 14;
            int colAncho = (card.Width - 28) / 2;
            for (int i = 0; i < segmentos.Length; i++)
            {
                var s = segmentos[i];
                int x = 14 + (i % 2) * colAncho;
                int y = y0 + (i / 2) * 20;
                using (var brush = new SolidBrush(s.Color))
                using (var path = RoundedRect(new Rectangle(x, y + 3, 10, 10), 2))
                    g.FillPath(brush, path);
                int pct = total > 0 ? (int)Math.Round(100.0 * s.Valor / total) : 0;
                Texto(g, $"{s.Nombre}  {s.Valor} ({pct}%)", Fuente8, FgSecondary, new Rectangle(x + 16, y, colAncho - 18, 16));
            }
        }

        // ══════════════════════════════════════════════════════════
        // LÍNEAS: actividad últimos 30 días
        // ══════════════════════════════════════════════════════════
        private void PintarLineas(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            var card = PintarTarjeta(g, pnlLineas);
            Texto(g, "Actividad — últimos 30 días", FuenteTitulo, FgPrimary, new Rectangle(14, 12, card.Width / 2, 20));

            // Leyenda arriba a la derecha
            int xl = card.Width - 16;
            foreach (var (nombre, color) in new[] { ("Devoluciones", Paleta.Verde), ("Préstamos", Accent) })
            {
                var tam = TextRenderer.MeasureText(g, nombre, Fuente8);
                xl -= tam.Width;
                TextRenderer.DrawText(g, nombre, Fuente8, new Point(xl, 15), FgSecondary);
                xl -= 16;
                using (var brush = new SolidBrush(color))
                using (var path = RoundedRect(new Rectangle(xl, 18, 10, 10), 2))
                    g.FillPath(brush, path);
                xl -= 18;
            }

            if (_actividadMensual.Count < 2) { Vacio(g, card, _error ?? "Sin datos"); return; }

            var area = new Rectangle(44, 46, card.Width - 44 - 24, card.Height - 46 - 34);
            int maximo = Math.Max(1, _actividadMensual.Max(a => Math.Max(a.Prestamos, a.Devoluciones)));
            int tope = Math.Max(4, (int)Math.Ceiling(maximo / 4.0) * 4);   // múltiplo de 4 → 5 líneas enteras

            // Guías horizontales y eje Y
            using (var pen = new Pen(Border))
            {
                for (int i = 0; i <= 4; i++)
                {
                    int y = area.Bottom - area.Height * i / 4;
                    g.DrawLine(pen, area.Left, y, area.Right, y);
                    TextRenderer.DrawText(g, (tope * i / 4).ToString(), Fuente7, new Rectangle(0, y - 8, area.Left - 8, 16),
                        FgSecondary, TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
                }
            }

            int n = _actividadMensual.Count;
            PointF Punto(int i, int valor) => new(
                area.Left + area.Width * i / (float)(n - 1),
                area.Bottom - area.Height * valor / (float)tope);

            void Serie(Func<(DateTime Fecha, int Prestamos, int Devoluciones), int> valor, Color color)
            {
                var puntos = Enumerable.Range(0, n).Select(i => Punto(i, valor(_actividadMensual[i]))).ToArray();
                var poligono = puntos.Concat(new[] { new PointF(puntos[^1].X, area.Bottom), new PointF(puntos[0].X, area.Bottom) }).ToArray();
                using (var brush = new SolidBrush(Color.FromArgb(30, color))) g.FillPolygon(brush, poligono);
                using (var pen = new Pen(color, 2f) { LineJoin = LineJoin.Round }) g.DrawLines(pen, puntos);
                using var punto = new SolidBrush(color);
                for (int i = 0; i < n; i++)
                    if (valor(_actividadMensual[i]) > 0) g.FillEllipse(punto, puntos[i].X - 3, puntos[i].Y - 3, 6, 6);
            }

            g.SmoothingMode = SmoothingMode.AntiAlias;
            Serie(a => a.Prestamos, Accent);
            Serie(a => a.Devoluciones, Paleta.Verde);

            // Eje X cada 5 días (y el último)
            for (int i = 0; i < n; i++)
            {
                if (i % 5 != 0 && i != n - 1) continue;
                var p = Punto(i, 0);
                TextRenderer.DrawText(g, _actividadMensual[i].Fecha.ToString("dd/MM"), Fuente7,
                    new Rectangle((int)p.X - 20, area.Bottom + 6, 40, 14), FgSecondary, TextFormatFlags.HorizontalCenter);
            }
        }

        // ══════════════════════════════════════════════════════════
        // BARRAS POR DEPARTAMENTO
        // ══════════════════════════════════════════════════════════
        private void PintarDepartamentos(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            var card = PintarTarjeta(g, pnlDepartamentos);
            Texto(g, "Préstamos activos por departamento", FuenteTitulo, FgPrimary, new Rectangle(14, 12, card.Width - 28, 20));

            if (_departamentos.Count == 0) { Vacio(g, card, _error ?? "Sin préstamos activos"); return; }

            int max = Math.Max(1, _departamentos.Max(d => d.Total));
            int ancho = card.Width - 28 - 40;
            for (int i = 0; i < _departamentos.Count; i++)
            {
                var (depto, total) = _departamentos[i];
                int y = 46 + i * 42;
                if (y + 30 > card.Height) break;
                Texto(g, depto, Fuente8, FgPrimary, new Rectangle(14, y, ancho, 16));

                var fondo = new Rectangle(14, y + 20, ancho, 6);
                using (var path = RoundedRect(fondo, 3))
                using (var brush = new SolidBrush(Surface2)) g.FillPath(brush, path);
                int w = Math.Max(6, ancho * total / max);
                using (var path = RoundedRect(new Rectangle(fondo.X, fondo.Y, w, 6), 3))
                using (var brush = new SolidBrush(Accent)) g.FillPath(brush, path);

                TextRenderer.DrawText(g, total.ToString(), Fuente8B, new Rectangle(fondo.Right + 8, y + 14, 30, 18),
                    FgSecondary, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
            }
        }

        // ══════════════════════════════════════════════════════════
        // PRÓXIMAS DEVOLUCIONES
        // ══════════════════════════════════════════════════════════
        private void PintarProximas(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            var card = PintarTarjeta(g, pnlProximas);
            Texto(g, "Próximas devoluciones", FuenteTitulo, FgPrimary, new Rectangle(14, 12, card.Width - 28, 20));

            if (_proximasDevoluciones.Rows.Count == 0)
            {
                Vacio(g, card, _error ?? "Nada por devolver en los próximos 7 días");
                return;
            }

            int y = 42;
            int mostradas = 0;
            foreach (DataRow r in _proximasDevoluciones.Rows)
            {
                if (y + 46 > card.Height - 22 && mostradas < _proximasDevoluciones.Rows.Count - 1) break;
                int dias = Convert.ToInt32(r["DiasRestantes"]);
                var (texto, fondo, frente) = dias < 0 ? ("VENCIDO", Paleta.RojoSuave, Paleta.Rojo)
                                           : dias == 0 ? ("Hoy", Paleta.NaranjaSuave, Paleta.Naranja)
                                           : dias <= 2 ? ($"{dias} d", Paleta.AmarilloSuave, Paleta.Amarillo)
                                           : ($"{dias} d", Paleta.VerdeSuave, Paleta.Verde);
                var chip = Chip(g, texto, fondo, frente, card.Width - 14, y + 20);

                Texto(g, r["Empleado"]?.ToString() ?? "", Fuente8S, FgPrimary, new Rectangle(14, y + 4, chip.X - 22, 18));
                Texto(g, Truncar(r["Herramientas"]?.ToString(), 32), Fuente7, FgSecondary, new Rectangle(14, y + 23, chip.X - 22, 14));

                y += 46;
                mostradas++;
                if (mostradas < _proximasDevoluciones.Rows.Count)
                    using (var pen = new Pen(Border)) g.DrawLine(pen, 14, y - 3, card.Width - 14, y - 3);
            }

            int restantes = _proximasDevoluciones.Rows.Count - mostradas;
            if (restantes > 0)
                Texto(g, $"+{restantes} más", Fuente8, Accent, new Rectangle(14, card.Height - 22, card.Width - 28, 16));
        }

        // ══════════════════════════════════════════════════════════
        // SIN STOCK + ALERTAS
        // ══════════════════════════════════════════════════════════
        private void PintarStockAlertas(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            var card = PintarTarjeta(g, pnlStockAlertas);
            int mitad = card.Height / 2;

            // Mitad superior: herramientas agotadas
            Texto(g, "Sin stock", FuenteTitulo, FgPrimary, new Rectangle(14, 12, card.Width - 28, 20));
            if (_sinStockList.Rows.Count == 0)
            {
                Vacio(g, new Rectangle(14, 36, card.Width - 28, mitad - 44),
                    _error ?? "✓ Todas las herramientas tienen unidades disponibles", _error == null ? Paleta.Verde : null);
            }
            else
            {
                int y = 40;
                foreach (DataRow r in _sinStockList.Rows)
                {
                    if (y + 24 > mitad - 6) break;
                    var chip = Chip(g, "Agotado", Paleta.RojoSuave, Paleta.Rojo, card.Width - 14, y + 10);
                    Texto(g, $"{r["Nombre"]}  ({r["Total"]} u.)", Fuente8, FgPrimary, new Rectangle(14, y + 2, chip.X - 22, 16));
                    y += 26;
                }
            }

            using (var pen = new Pen(Border)) g.DrawLine(pen, 14, mitad, card.Width - 14, mitad);

            // Mitad inferior: alertas
            var alertas = new List<string>();
            if (_vencidos > 0) alertas.Add($"{Plural(_vencidos, "préstamo vencido", "préstamos vencidos")} sin devolver");
            if (_daniadas > 0) alertas.Add($"{Plural(_daniadas, "unidad dañada", "unidades dañadas")} registradas");
            if (_enMantenimiento > 0) alertas.Add($"{Plural(_enMantenimiento, "unidad", "unidades")} en reparación");

            var caja = new Rectangle(12, mitad + 12, card.Width - 24, card.Height - mitad - 24);
            bool hay = alertas.Count > 0;
            using (var path = RoundedRect(caja, 8))
            using (var brush = new SolidBrush(hay ? Paleta.RojoSuave : Paleta.VerdeSuave))
                g.FillPath(brush, path);

            if (!hay)
            {
                Vacio(g, caja, "✓ Sin alertas activas", Paleta.Verde);
                return;
            }
            Texto(g, "⚠  Alertas activas", Fuente9B, Paleta.Rojo, new Rectangle(caja.X + 12, caja.Y + 10, caja.Width - 24, 18));
            for (int i = 0; i < alertas.Count; i++)
                Texto(g, "•  " + alertas[i], Fuente8, Paleta.Rojo, new Rectangle(caja.X + 12, caja.Y + 34 + i * 20, caja.Width - 24, 16));
        }

        // ══════════════════════════════════════════════════════════
        // ACTIVIDAD DE HOY
        // ══════════════════════════════════════════════════════════
        private void ConstruirActividad()
        {
            flpActividad.SuspendLayout();
            foreach (Control c in flpActividad.Controls.Cast<Control>().ToList()) c.Dispose();
            flpActividad.Controls.Clear();

            if (_actividadReciente.Rows.Count == 0)
            {
                var vacio = new PanelGdi { Size = new Size(440, 80), Margin = new Padding(0), BackColor = BgPage };
                vacio.Paint += (s, e) =>
                {
                    var card = PintarTarjeta(e.Graphics, vacio);
                    Vacio(e.Graphics, card, _error ?? "Sin actividad registrada hoy");
                };
                flpActividad.Controls.Add(vacio);
            }
            else
            {
                foreach (DataRow r in _actividadReciente.Rows)
                    flpActividad.Controls.Add(TarjetaActividad(r));
            }
            flpActividad.ResumeLayout();
        }

        private static PanelGdi TarjetaActividad(DataRow r)
        {
            bool esPrestamo = r["Tipo"]?.ToString() == "Préstamo";
            string empleado = r["Empleado"]?.ToString() ?? "";
            string herramientas = Truncar(r["Herramientas"]?.ToString(), 28);
            string hora = r["Fecha"] is DateTime f ? f.ToString("HH:mm") : "";
            string codigo = r["CodigoEmpleado"]?.ToString() ?? "";

            var tarjeta = new PanelGdi { Size = new Size(230, 84), Margin = new Padding(0, 0, 12, 12), BackColor = BgPage };
            tarjeta.Paint += (s, e) =>
            {
                var g = e.Graphics;
                var card = PintarTarjeta(g, tarjeta);
                var tam = TextRenderer.MeasureText(g, esPrestamo ? "Préstamo" : "Devolución", Fuente8B, Size.Empty, TextFormatFlags.NoPadding);
                var chip = new Rectangle(12, 10, tam.Width + 16, 20);
                using (var path = RoundedRect(chip, 9))
                using (var brush = new SolidBrush(esPrestamo ? AccentSoft : Paleta.VerdeSuave))
                    g.FillPath(brush, path);
                TextRenderer.DrawText(g, esPrestamo ? "Préstamo" : "Devolución", Fuente8B, chip,
                    esPrestamo ? Accent : Paleta.Verde, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
                TextRenderer.DrawText(g, hora, Fuente8, new Rectangle(card.Width - 60, 12, 48, 16), FgSecondary, TextFormatFlags.Right);

                // Avatar + empleado y herramientas
                EmpleadoGrid.DibujarAvatar(g, new Rectangle(8, 36, 40, 40), codigo, empleado);
                Texto(g, empleado, Fuente9S, FgPrimary, new Rectangle(52, 38, card.Width - 64, 18));
                Texto(g, herramientas, Fuente8, FgSecondary, new Rectangle(52, 57, card.Width - 64, 16));
            };
            return tarjeta;
        }
    }
}
