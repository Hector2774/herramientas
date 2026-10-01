using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;
using FontAwesome.Sharp;
using PromacoHerra.Models;

namespace PromacoHerra.Controls
{
    // Dibujo owner-draw de una fila de la lista de herramientas (ListBox con DrawMode.OwnerDrawFixed):
    //   [ícono]  Nombre de la herramienta                 (3/5)
    //            HER-0001 · Eléctricas · DeWalt
    // No es un control: el ListBox pinta cientos de filas sin crear un control por cada una.
    public static class HerramientaListItem
    {
        public const int Alto = 64;

        private static readonly Font FuenteNombre = new("Segoe UI", 10F, FontStyle.Bold);
        private static readonly Font FuenteSub = new("Segoe UI", 8.5F);
        private static readonly Font FuenteBadge = new("Segoe UI", 8.5F, FontStyle.Bold);
        private static readonly Dictionary<(IconChar, Color, int), Bitmap> _iconos = new();

        public static void Dibujar(Graphics g, Rectangle b, HerramientaResumen h, bool seleccionado)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (var brush = new SolidBrush(seleccionado ? ThemeManager.AccentBlueSoft : ThemeManager.CardBackground))
                g.FillRectangle(brush, b);
            if (seleccionado)
                using (var brush = new SolidBrush(ThemeManager.AccentBlue))
                    g.FillRectangle(brush, b.X, b.Y, 3, b.Height);
            using (var pen = new Pen(ThemeManager.BorderColor))
                g.DrawLine(pen, b.X, b.Bottom - 1, b.Right, b.Bottom - 1);

            // Miniatura 40x40 (espacio para foto futura): ícono según la categoría
            var thumb = new Rectangle(b.X + 12, b.Y + 12, 40, 40);
            DibujarMiniatura(g, thumb, h.Categoria, seleccionado ? Color.White : Paleta.Surface2, 20);

            // Badge disponible/total a la derecha
            var (fondo, frente) = ColoresStock(h.StockDisponible, h.StockTotal);
            string badge = $"{h.StockDisponible}/{h.StockTotal}";
            var tamBadge = StatusChip.Medir(g, badge, FuenteBadge);
            var rBadge = StatusChip.Dibujar(g, new Point(b.Right - tamBadge.Width - 12, b.Y + (b.Height - tamBadge.Height) / 2),
                badge, fondo, frente, FuenteBadge);

            int x = thumb.Right + 12;
            int ancho = Math.Max(20, rBadge.X - x - 8);
            TextRenderer.DrawText(g, h.Nombre, FuenteNombre, new Rectangle(x, b.Y + 12, ancho, 20),
                ThemeManager.TextPrimary, TextFormatFlags.Left | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);

            string sub = string.Join(" · ", new[] { h.Codigo, h.Categoria, h.Marca }.Where(s => !string.IsNullOrEmpty(s)));
            TextRenderer.DrawText(g, sub, FuenteSub, new Rectangle(x, b.Y + 35, ancho, 18),
                ThemeManager.TextSecondary, TextFormatFlags.Left | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);
        }

        // Verde si hay más del 40% disponible, amarillo si queda algo, rojo si no queda nada
        public static (Color Fondo, Color Frente) ColoresStock(int disponibles, int total)
        {
            if (disponibles <= 0) return (Paleta.RojoSuave, Paleta.Rojo);
            if (total > 0 && disponibles > total * 0.4) return (Paleta.VerdeSuave, Paleta.Verde);
            return (Paleta.AmarilloSuave, Paleta.Amarillo);
        }

        public static void DibujarMiniatura(Graphics g, Rectangle r, string categoria, Color fondo, int tamIcono)
        {
            using (var path = RoundedGeometry.RoundedRect(r, 8))
            using (var brush = new SolidBrush(fondo))
                g.FillPath(brush, path);

            var icono = Icono(IconoCategoria(categoria), ThemeManager.TextSecondary, tamIcono);
            g.DrawImage(icono, r.X + (r.Width - tamIcono) / 2, r.Y + (r.Height - tamIcono) / 2, tamIcono, tamIcono);
        }

        // Ícono representativo por nombre de categoría (sin distinguir acentos)
        public static IconChar IconoCategoria(string categoria)
        {
            string c = Normalizar(categoria);
            if (c.Contains("inalambric")) return IconChar.BatteryFull;
            if (c.Contains("electric")) return IconChar.Bolt;
            if (c.Contains("medicion")) return IconChar.RulerCombined;
            if (c.Contains("neumatic")) return IconChar.Wind;
            if (c.Contains("jardin")) return IconChar.Leaf;
            if (c.Contains("segur")) return IconChar.HelmetSafety;
            if (c.Contains("manual")) return IconChar.Hammer;
            return IconChar.Wrench;
        }

        private static string Normalizar(string s)
        {
            var d = (s ?? "").ToLowerInvariant().Normalize(System.Text.NormalizationForm.FormD);
            var sb = new System.Text.StringBuilder();
            foreach (var ch in d)
                if (CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark) sb.Append(ch);
            return sb.ToString();
        }

        private static Bitmap Icono(IconChar icono, Color color, int tam)
        {
            if (!_iconos.TryGetValue((icono, color, tam), out var bmp))
                _iconos[(icono, color, tam)] = bmp = icono.ToBitmap(color, tam);
            return bmp;
        }
    }
}
