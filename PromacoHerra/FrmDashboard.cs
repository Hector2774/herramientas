using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using PromacoHerra.Data;

namespace PromacoHerra
{
    public partial class FrmDashboard : Form
    {
        private FlowLayoutPanel flowActividadPanel;
        private FlowLayoutPanel flowAlertasPanel;

        // Panel donde vive la gráfica dibujada a mano
        private Panel _graficaCanvas;

        // Datos para la gráfica
        private int _nDisponibles, _nPrestadas, _nDanadas, _nMantenimiento, _nBaja;

        public FrmDashboard()
        {
            InitializeComponent();
            ThemeManager.ApplyTheme(this);
            ConfigurarListasModernas();
            ConfigurarGrafica();
            this.Load += FrmDashboard_Load;
        }

        private void FrmDashboard_Load(object sender, EventArgs e)
        {
            // Fecha dinámica en el saludo
            string hora = DateTime.Now.Hour < 12 ? "Buenos días"
                         : DateTime.Now.Hour < 19 ? "Buenas tardes"
                         : "Buenas noches";
            lblSaludo.Text = $"{hora}, Admin";
            lblFecha.Text = DateTime.Now.ToString("dddd, d 'de' MMMM 'de' yyyy",
                             new System.Globalization.CultureInfo("es-HN"));

            CargarDashboard();
        }

        private void CargarDashboard()
        {
            CargarResumen();
            CargarAlertas();
            CargarActividad();
            CargarRanking();
            _graficaCanvas?.Invalidate();   // redibujar la gráfica
        }

        // ── Cards de resumen ───────────────────────────────────────
        private void CargarResumen()
        {
            using var cn = Db.GetConnection();
            cn.Open();

            int activos = Convert.ToInt32(
                new Microsoft.Data.SqlClient.SqlCommand(
                    "SELECT COUNT(*) FROM Prestamo WHERE Estado IN ('Activo','Vencido')", cn)
                .ExecuteScalar());

            int vencidos = Convert.ToInt32(
                new Microsoft.Data.SqlClient.SqlCommand(
                    "SELECT COUNT(*) FROM Prestamo WHERE Estado = 'Vencido'", cn)
                .ExecuteScalar());

            int disponibles = Convert.ToInt32(
                new Microsoft.Data.SqlClient.SqlCommand(
                    "SELECT COUNT(*) FROM Herramienta WHERE Estado = 'Disponible' AND Activa = 1", cn)
                .ExecuteScalar());

            int danadas = Convert.ToInt32(
                new Microsoft.Data.SqlClient.SqlCommand(
                    "SELECT COUNT(*) FROM Herramienta WHERE Estado = 'Dañada' AND Activa = 1", cn)
                .ExecuteScalar());

            int mant = Convert.ToInt32(
                new Microsoft.Data.SqlClient.SqlCommand(
                    "SELECT COUNT(*) FROM Mantenimiento WHERE FechaFin IS NULL", cn)
                .ExecuteScalar());

            // Datos para la gráfica de inventario
            _nDisponibles = disponibles;
            _nDanadas = danadas;
            _nMantenimiento = mant;

            _nPrestadas = Convert.ToInt32(
                new Microsoft.Data.SqlClient.SqlCommand(
                    "SELECT COUNT(*) FROM Herramienta WHERE Estado = 'Prestada' AND Activa = 1", cn)
                .ExecuteScalar());

            _nBaja = Convert.ToInt32(
                new Microsoft.Data.SqlClient.SqlCommand(
                    "SELECT COUNT(*) FROM Herramienta WHERE Activa = 0", cn)
                .ExecuteScalar());

            // Actualizar labels de las cards
            ActualizarCard(lblAtrasos, "🔴", "Vencidos", vencidos);
            ActualizarCard(lblDisponibles, "🟢", "Disponibles", disponibles);
            ActualizarCard(lblActivos, "🔵", "Préstamos activos", activos);
            ActualizarCard(lblDanadas, "🟡", "Herramientas dañadas", danadas);
            ActualizarCard(lblMantenimiento, "🔧", "En mantenimiento", mant);
        }

        private void ActualizarCard(Label lbl, string emoji, string titulo, int valor)
        {
            lbl.Text = $"{emoji}\n{titulo}";
            // Agregar el número en tamaño grande con un segundo label generado dinámicamente
            // Buscamos si ya existe el label del número en el parent
            var parent = lbl.Parent;
            Label lblNum = null;
            foreach (Control c in parent.Controls)
                if (c is Label l && l.Tag?.ToString() == "numero") lblNum = l;

            if (lblNum == null)
            {
                lblNum = new Label
                {
                    Tag = "numero",
                    Dock = DockStyle.Bottom,
                    Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Height = 45,
                    ForeColor = lbl.ForeColor
                };
                parent.Controls.Add(lblNum);
            }

            lblNum.Text = valor.ToString();
            lblNum.ForeColor = lbl.ForeColor;
        }

        // ── Alertas ────────────────────────────────────────────────
        private void CargarAlertas()
        {
            flowAlertasPanel.Controls.Clear();

            var dt = Db.Query(@"
                SELECT TOP 10
                    e.Nombre,
                    h.Nombre AS Herramienta,
                    p.FechaDevolucionEsperada,
                    DATEDIFF(DAY, p.FechaDevolucionEsperada, GETDATE()) AS DiasAtraso
                FROM PrestamoDetalle pd
                INNER JOIN Prestamo    p ON p.PrestamoId    = pd.PrestamoId
                INNER JOIN Empleado    e ON e.EmpleadoId    = p.EmpleadoId
                INNER JOIN Herramienta h ON h.HerramientaId = pd.HerramientaId
                WHERE pd.FechaDevuelta IS NULL
                  AND p.Estado IN ('Activo','Vencido')
                  AND p.FechaDevolucionEsperada < GETDATE()
                ORDER BY p.FechaDevolucionEsperada ASC");

            if (dt.Rows.Count == 0)
            {
                flowAlertasPanel.Controls.Add(new Label
                {
                    Text = "✅  Sin alertas activas",
                    Font = new Font("Segoe UI", 10F),
                    ForeColor = Color.FromArgb(22, 163, 74),
                    AutoSize = true,
                    Margin = new Padding(12, 16, 0, 0)
                });
                return;
            }

            foreach (DataRow row in dt.Rows)
            {
                int dias = Convert.ToInt32(row["DiasAtraso"]);
                string nombre = row["Nombre"].ToString();
                string herramienta = row["Herramienta"].ToString();
                var fecha = Convert.ToDateTime(row["FechaDevolucionEsperada"]);

                var item = new ItemActividad
                {
                    Iniciales = ObtenerIniciales(nombre),
                    Titulo = nombre,
                    Subtitulo = $"{herramienta}  •  Venció: {fecha:dd/MM/yyyy}",
                    TextoEstado = $"{dias} día(s) de atraso",
                    ColorEstado = ThemeManager.AccentRed
                };
                flowAlertasPanel.Controls.Add(item);
            }
        }

        // ── Actividad reciente ─────────────────────────────────────
        private void CargarActividad()
        {
            flowActividadPanel.Controls.Clear();

            var dt = Db.Query(@"
                SELECT TOP 10
                    e.Nombre,
                    h.Nombre AS Herramienta,
                    pd.FechaDevuelta,
                    p.FechaPrestamo,
                    pd.EstadoDevolucion
                FROM PrestamoDetalle pd
                INNER JOIN Prestamo    p ON p.PrestamoId    = pd.PrestamoId
                INNER JOIN Empleado    e ON e.EmpleadoId    = p.EmpleadoId
                INNER JOIN Herramienta h ON h.HerramientaId = pd.HerramientaId
                ORDER BY ISNULL(pd.FechaDevuelta, p.FechaPrestamo) DESC");

            Color azul = ThemeManager.SidebarColor;
            Color verde = Color.FromArgb(22, 163, 74);
            Color rojo = ThemeManager.AccentRed;

            foreach (DataRow row in dt.Rows)
            {
                string nombre = row["Nombre"].ToString();
                string herramienta = row["Herramienta"].ToString();
                bool devuelta = row["FechaDevuelta"] != DBNull.Value;
                string estado = row["EstadoDevolucion"]?.ToString() ?? "";

                DateTime fecha = devuelta
                    ? Convert.ToDateTime(row["FechaDevuelta"])
                    : Convert.ToDateTime(row["FechaPrestamo"]);

                Color colorEstado = devuelta
                    ? (estado == "Dañado" || estado == "Perdido" ? rojo : verde)
                    : azul;

                string textoEstado = devuelta ? (estado == "Pendiente" ? "Devuelto" : estado) : "Préstamo";

                var item = new ItemActividad
                {
                    Iniciales = ObtenerIniciales(nombre),
                    Titulo = devuelta ? $"{nombre} devolvió" : $"{nombre} prestó",
                    Subtitulo = $"{herramienta}  •  {fecha:dd/MMM HH:mm}",
                    TextoEstado = textoEstado,
                    ColorEstado = colorEstado
                };
                flowActividadPanel.Controls.Add(item);
            }
        }

        // ── Ranking ────────────────────────────────────────────────
        private void CargarRanking()
        {
            dgvRanking.DataSource = Db.Query(@"
                SELECT TOP 10
                    e.Nombre          AS Empleado,
                    d.Nombre          AS Departamento,
                    COUNT(pd.PrestamoDetalleId) AS TotalPréstamos
                FROM PrestamoDetalle pd
                INNER JOIN Prestamo    p ON p.PrestamoId   = pd.PrestamoId
                INNER JOIN Empleado    e ON e.EmpleadoId   = p.EmpleadoId
                LEFT  JOIN Departamento d ON d.DepartamentoId = e.DepartamentoId
                GROUP BY e.Nombre, d.Nombre
                ORDER BY TotalPréstamos DESC");
        }

        // ── Gráfica de barras del inventario ───────────────────────
        private void ConfigurarGrafica()
        {
            _graficaCanvas = new Panel
            {
                Location = new Point(10, 52),
                Size = new Size(575, 298),
                BackColor = Color.White
            };
            _graficaCanvas.Paint += GraficaInventario_Paint;
            panelGrafica.Controls.Add(_graficaCanvas);
        }

        private void GraficaInventario_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            int total = _nDisponibles + _nPrestadas + _nDanadas + _nMantenimiento + _nBaja;
            if (total == 0) { DrawNoData(g, _graficaCanvas.Width, _graficaCanvas.Height); return; }

            // Configuración de la gráfica
            int marginLeft = 55;
            int marginBottom = 55;
            int marginTop = 20;
            int marginRight = 20;
            int w = _graficaCanvas.Width - marginLeft - marginRight;
            int h = _graficaCanvas.Height - marginTop - marginBottom;

            string[] labels = { "Disponibles", "Prestadas", "Dañadas", "Mant.", "Baja" };
            int[] values = { _nDisponibles, _nPrestadas, _nDanadas, _nMantenimiento, _nBaja };
            Color[] colors =
            {
                Color.FromArgb(22,  163, 74),    // verde
                Color.FromArgb(0,   86,  179),   // azul
                Color.FromArgb(217, 119, 6),     // naranja
                Color.FromArgb(109, 40,  217),   // violeta
                Color.FromArgb(150, 150, 160)    // gris
            };

            int maxVal = 0;
            foreach (var v in values) if (v > maxVal) maxVal = v;
            if (maxVal == 0) maxVal = 1;

            int barCount = labels.Length;
            int barWidth = (int)(w / (barCount * 1.6));
            int gap = (w - barWidth * barCount) / (barCount + 1);
            int baseY = marginTop + h;

            // Líneas de guía horizontales
            using var penGuia = new Pen(Color.FromArgb(235, 238, 245), 1);
            int guias = 4;
            for (int i = 0; i <= guias; i++)
            {
                int yGuia = marginTop + (int)(h * i / (float)guias);
                g.DrawLine(penGuia, marginLeft, yGuia, marginLeft + w, yGuia);

                int labelVal = (int)(maxVal * (guias - i) / (float)guias);
                using var fntEje = new Font("Segoe UI", 7.5f);
                g.DrawString(labelVal.ToString(), fntEje, Brushes.Gray,
                    2, yGuia - 7);
            }

            // Eje X
            using var penEje = new Pen(Color.FromArgb(200, 205, 215), 1);
            g.DrawLine(penEje, marginLeft, baseY, marginLeft + w, baseY);

            // Barras
            using var fntBar = new Font("Segoe UI", 8f);
            using var fntNum = new Font("Segoe UI", 9f, FontStyle.Bold);

            for (int i = 0; i < barCount; i++)
            {
                int x = marginLeft + gap + i * (barWidth + gap);
                int barH = values[i] == 0 ? 0 : (int)(h * values[i] / (float)maxVal);
                int y = baseY - barH;

                // Barra con esquinas superiores redondeadas
                DrawRoundedBar(g, colors[i], x, y, barWidth, barH, 6);

                // Valor encima de la barra
                if (values[i] > 0)
                {
                    string numStr = values[i].ToString();
                    var sz = g.MeasureString(numStr, fntNum);
                    g.DrawString(numStr, fntNum,
                        new SolidBrush(colors[i]),
                        x + (barWidth - sz.Width) / 2, y - sz.Height - 2);
                }

                // Etiqueta debajo del eje
                var szLabel = g.MeasureString(labels[i], fntBar);
                g.DrawString(labels[i], fntBar, Brushes.Gray,
                    x + (barWidth - szLabel.Width) / 2, baseY + 8);
            }
        }

        private static void DrawRoundedBar(Graphics g, Color color, int x, int y, int w, int h, int r)
        {
            if (h <= 0) return;
            if (h < r * 2) r = h / 2;

            using var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(x, y, r * 2, r * 2, 180, 90);
            path.AddArc(x + w - r * 2, y, r * 2, r * 2, 270, 90);
            path.AddLine(x + w, y + r, x + w, y + h);
            path.AddLine(x + w, y + h, x, y + h);
            path.AddLine(x, y + h, x, y + r);
            path.CloseFigure();

            using var brush = new SolidBrush(color);
            g.FillPath(brush, path);

            // Brillo suave en la parte superior
            using var brushLight = new LinearGradientBrush(
                new Rectangle(x, y, w, h / 3),
                Color.FromArgb(60, Color.White), Color.Transparent,
                LinearGradientMode.Vertical);
            g.FillPath(brushLight, path);
        }

        private static void DrawNoData(Graphics g, int w, int h)
        {
            using var f = new Font("Segoe UI", 11F);
            var sz = g.MeasureString("Sin datos aún", f);
            g.DrawString("Sin datos aún", f, Brushes.LightGray,
                (w - sz.Width) / 2, (h - sz.Height) / 2);
        }

        // ── FlowPanels para alertas y actividad ────────────────────
        private void ConfigurarListasModernas()
        {
            lstAlertas.Visible = false;
            lstActividad.Visible = false;

            flowAlertasPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.White,
                Padding = new Padding(8, 4, 8, 4),
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };
            panelAlertas.Controls.Add(flowAlertasPanel);
            flowAlertasPanel.BringToFront();

            flowActividadPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.White,
                Padding = new Padding(8, 4, 8, 4),
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };
            panelActividad.Controls.Add(flowActividadPanel);
            flowActividadPanel.BringToFront();
        }

        private string ObtenerIniciales(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre)) return "--";
            var p = nombre.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return p.Length == 1
                ? p[0].Substring(0, Math.Min(2, p[0].Length)).ToUpper()
                : (p[0][0].ToString() + p[1][0].ToString()).ToUpper();
        }

        // Métodos de compatibilidad (no se usan pero evitan errores de compilación)
        public void ApplyCardStyle(Panel panel) { }
        public void StyleListItem(Panel item) { }
        private void lblActivos_Click(object sender, EventArgs e) { }
        private void FrmDashboard_Load_1(object sender, EventArgs e) { }
    }
}