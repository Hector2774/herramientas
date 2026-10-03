using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace PromacoHerra.Controls
{
    // Avatar circular con las iniciales de una persona. El color sale de una paleta fija
    // por índice, para que el mismo empleado tenga el mismo color en la lista y en el detalle.
    public class AvatarControl : Control
    {
        private string _nombre = string.Empty;
        private int _colorIndex;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Nombre
        {
            get => _nombre;
            set { _nombre = value ?? string.Empty; Invalidate(); }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ColorIndex
        {
            get => _colorIndex;
            set { _colorIndex = value; Invalidate(); }
        }

        public AvatarControl()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
            Size = new Size(48, 48);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(Parent?.BackColor ?? ThemeManager.CardBackground);
            Avatar.Dibujar(e.Graphics, new Rectangle(0, 0, Width - 1, Height - 1), _nombre, _colorIndex);
        }
    }

    public static class Avatar
    {
        private static readonly Color[] Paleta =
        {
            Color.FromArgb(37, 99, 235),   // azul
            Color.FromArgb(22, 163, 74),   // verde
            Color.FromArgb(202, 138, 4),   // amarillo
            Color.FromArgb(234, 88, 12),   // naranja
            Color.FromArgb(147, 51, 234),  // morado
            Color.FromArgb(6, 148, 162),   // teal
        };

        public static Color ColorPara(int index) => Paleta[Math.Abs(index) % Paleta.Length];

        // Índice estable a partir de un texto (código de empleado, nombre de departamento).
        // No usa string.GetHashCode(): en .NET cambia en cada ejecución.
        public static int IndicePorTexto(string texto)
        {
            unchecked
            {
                int h = 17;
                foreach (var c in texto ?? "") h = h * 31 + char.ToUpperInvariant(c);
                return Math.Abs(h % Paleta.Length);
            }
        }

        // Versión suave de un color de la paleta (fondo de chips): 14% del color sobre blanco
        public static Color Suave(Color c) =>
            Color.FromArgb(255 - (255 - c.R) * 14 / 100, 255 - (255 - c.G) * 14 / 100, 255 - (255 - c.B) * 14 / 100);

        // "NANCY PATRICIA MENDOZA HERNANDEZ" → "NM" (primer nombre + primer apellido)
        public static string Iniciales(string nombre)
        {
            var partes = (nombre ?? "").Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (partes.Length == 0) return "?";
            if (partes.Length == 1) return partes[0][..1].ToUpperInvariant();

            var apellido = partes.Length >= 3 ? partes[^2] : partes[^1];
            return (partes[0][..1] + apellido[..1]).ToUpperInvariant();
        }

        public static void Dibujar(Graphics g, Rectangle rect, string nombre, int colorIndex)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (var brush = new SolidBrush(ColorPara(colorIndex)))
                g.FillEllipse(brush, rect);

            using var fuente = new Font("Segoe UI", Math.Max(8f, rect.Height * 0.30f), FontStyle.Bold, GraphicsUnit.Pixel);
            TextRenderer.DrawText(g, Iniciales(nombre), fuente, rect, Color.White,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
        }
    }

    // Texto y colores del plazo de un préstamo. Nunca muestra días negativos.
    public static class Plazo
    {
        public static readonly Color VerdeFondo = Color.FromArgb(220, 252, 231);
        public static readonly Color VerdeTexto = Color.FromArgb(21, 128, 61);
        public static readonly Color AmarilloFondo = Color.FromArgb(254, 243, 199);
        public static readonly Color AmarilloTexto = Color.FromArgb(180, 83, 9);
        public static readonly Color RojoFondo = Color.FromArgb(254, 226, 226);
        public static readonly Color RojoTexto = Color.FromArgb(185, 28, 28);

        public static string Dias(int n) => n == 1 ? "1 día" : $"{n} días";

        public static (string Texto, Color Fondo, Color Frente) Describir(int diasRestantes)
        {
            if (diasRestantes < 0)
                return ($"Venció hace {Dias(-diasRestantes)}", RojoFondo, RojoTexto);
            if (diasRestantes == 0)
                return ("Vence hoy", AmarilloFondo, AmarilloTexto);
            if (diasRestantes <= 2)
                return ($"Vence en {Dias(diasRestantes)}", AmarilloFondo, AmarilloTexto);
            return ($"Vence en {Dias(diasRestantes)}", VerdeFondo, VerdeTexto);
        }

        // Chip redondeado con el texto del plazo; devuelve el ancho usado
        public static int DibujarChip(Graphics g, Point origen, int diasRestantes, Font fuente)
        {
            var (texto, fondo, frente) = Describir(diasRestantes);
            var tam = TextRenderer.MeasureText(g, texto, fuente, Size.Empty, TextFormatFlags.NoPadding);
            var rect = new Rectangle(origen.X, origen.Y, tam.Width + 16, tam.Height + 6);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (var path = RoundedGeometry.RoundedRect(rect, rect.Height / 2))
            using (var brush = new SolidBrush(fondo))
                g.FillPath(brush, path);

            TextRenderer.DrawText(g, texto, fuente, rect, frente,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
            return rect.Width;
        }
    }
}
