using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;

namespace PromacoHerra.Controls
{
    // Estilo y dibujo compartidos por las grillas de empleados (FrmEmpleados y FrmBuscarEmpleado):
    // encabezado claro en mayúsculas, filas de 44px alternadas, hover, avatar con iniciales,
    // código monospace, nombre en negrita y chip de color por departamento.
    // La grilla debe crearse DESPUÉS de ThemeManager.ApplyTheme para que este no le aplique
    // su encabezado oscuro ni su hover.
    public static class EmpleadoGrid
    {
        public static readonly Font FuenteCodigo = new("Consolas", 9F);
        public static readonly Font FuenteNombre = new("Segoe UI", 9.5F, FontStyle.Bold);
        private static readonly Font FuenteAvatar = new("Segoe UI", 8.5F, FontStyle.Bold);
        private static readonly Font FuenteChip = new("Segoe UI", 8.5F, FontStyle.Bold);

        public static void Configurar(DataGridView dgv)
        {
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.MultiSelect = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.RowHeadersVisible = false;
            dgv.BorderStyle = BorderStyle.None;
            dgv.BackgroundColor = Color.White;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.None;   // los separadores se dibujan a mano
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgv.RowTemplate.Height = 44;

            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.ColumnHeadersHeight = 38;
            var h = dgv.ColumnHeadersDefaultCellStyle;
            h.BackColor = h.SelectionBackColor = Paleta.Surface2;
            h.ForeColor = h.SelectionForeColor = ThemeManager.TextSecondary;
            h.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold);
            h.Padding = new Padding(8, 0, 0, 0);

            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
            dgv.DefaultCellStyle.ForeColor = ThemeManager.TextPrimary;

            // Hover: la fila bajo el mouse se guarda en Tag
            dgv.Tag = -1;
            dgv.CellMouseEnter += (s, e) => CambiarHover(dgv, e.RowIndex);
            dgv.MouseLeave += (s, e) => CambiarHover(dgv, -1);
            dgv.SelectionChanged += (s, e) => dgv.Invalidate();
        }

        public static DataGridViewTextBoxColumn Columna(string encabezado, int ancho = 0)
        {
            var c = new DataGridViewTextBoxColumn
            {
                HeaderText = encabezado,
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                AutoSizeMode = ancho > 0 ? DataGridViewAutoSizeColumnMode.None : DataGridViewAutoSizeColumnMode.Fill
            };
            if (ancho > 0) c.Width = ancho;
            return c;
        }

        public static int FilaHover(DataGridView dgv) => dgv.Tag is int i ? i : -1;

        public static void ReiniciarHover(DataGridView dgv) => dgv.Tag = -1;

        private static void CambiarHover(DataGridView dgv, int fila)
        {
            int anterior = FilaHover(dgv);
            if (fila == anterior) return;
            dgv.Tag = fila;
            if (anterior >= 0 && anterior < dgv.RowCount) dgv.InvalidateRow(anterior);
            if (fila >= 0 && fila < dgv.RowCount) dgv.InvalidateRow(fila);
        }

        // Fondo de la celda (alternado / hover / selección) + separador inferior
        public static void PintarFondo(DataGridView dgv, DataGridViewCellPaintingEventArgs e)
        {
            var b = e.CellBounds;
            bool resaltada = dgv.Rows[e.RowIndex].Selected || e.RowIndex == FilaHover(dgv);
            Color fondo = resaltada ? ThemeManager.AccentBlueSoft
                        : e.RowIndex % 2 == 1 ? Paleta.Surface2 : Color.White;

            using (var brush = new SolidBrush(fondo))
                e.Graphics!.FillRectangle(brush, b);
            using (var pen = new Pen(ThemeManager.BorderColor))
                e.Graphics!.DrawLine(pen, b.Left, b.Bottom - 1, b.Right, b.Bottom - 1);
        }

        public static void DibujarTexto(Graphics g, Rectangle celda, string texto, Font fuente, Color color) =>
            TextRenderer.DrawText(g, texto, fuente, Rectangle.Inflate(celda, -8, 0), color,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);

        // Círculo de 32px con iniciales; color fijo por código del empleado
        public static void DibujarAvatar(Graphics g, Rectangle celda, string codigo, string nombre)
        {
            var r = new Rectangle(celda.X + (celda.Width - 32) / 2, celda.Y + (celda.Height - 32) / 2, 32, 32);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (var brush = new SolidBrush(Avatar.ColorPara(Avatar.IndicePorTexto(codigo))))
                g.FillEllipse(brush, r);
            TextRenderer.DrawText(g, Avatar.Iniciales(nombre), FuenteAvatar, r, Color.White,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
        }

        // Chip con color fijo por departamento (mismo hash que el avatar), recortado al ancho de la celda
        public static void DibujarChipDepartamento(Graphics g, Rectangle celda, string depto)
        {
            if (string.IsNullOrWhiteSpace(depto) || depto == "—")
            {
                TextRenderer.DrawText(g, "—", FuenteChip, Rectangle.Inflate(celda, -10, 0), ThemeManager.TextSecondary,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
                return;
            }

            var color = Avatar.ColorPara(Avatar.IndicePorTexto(depto));
            var tam = TextRenderer.MeasureText(g, depto, FuenteChip, Size.Empty, TextFormatFlags.NoPadding);
            int ancho = Math.Min(tam.Width + 18, celda.Width - 16);
            var r = new Rectangle(celda.X + 8, celda.Y + (celda.Height - 24) / 2, ancho, 24);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (var path = RoundedGeometry.RoundedRect(r, 12))
            using (var brush = new SolidBrush(Avatar.Suave(color)))
                g.FillPath(brush, path);

            var oscuro = Color.FromArgb(color.R * 75 / 100, color.G * 75 / 100, color.B * 75 / 100);
            TextRenderer.DrawText(g, depto, FuenteChip, Rectangle.Inflate(r, -8, 0), oscuro,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);
        }

        // Sin distinguir mayúsculas ni acentos ("nunez" encuentra "NUÑEZ")
        public static bool Contiene(string texto, string termino) =>
            !string.IsNullOrEmpty(texto) &&
            CultureInfo.InvariantCulture.CompareInfo.IndexOf(texto, termino,
                CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0;
    }
}
