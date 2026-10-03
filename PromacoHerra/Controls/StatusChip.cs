using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PromacoHerra.Controls
{
    // Colores semánticos que complementan a ThemeManager (fondos suaves para chips y tiles)
    public static class Paleta
    {
        public static readonly Color Surface2 = Color.FromArgb(240, 242, 246);
        public static readonly Color Verde = Color.FromArgb(22, 163, 74);
        public static readonly Color VerdeSuave = Color.FromArgb(220, 252, 231);
        public static readonly Color Amarillo = Color.FromArgb(202, 138, 4);
        public static readonly Color AmarilloSuave = Color.FromArgb(254, 249, 195);
        public static readonly Color Naranja = Color.FromArgb(234, 88, 12);
        public static readonly Color NaranjaSuave = Color.FromArgb(255, 237, 213);
        public static readonly Color Rojo = Color.FromArgb(220, 38, 38);
        public static readonly Color RojoSuave = Color.FromArgb(254, 226, 226);
        public static readonly Color Gris = Color.FromArgb(107, 114, 128);
        public static readonly Color GrisSuave = Color.FromArgb(243, 244, 246);
        public static readonly Color Violeta = Color.FromArgb(124, 58, 237);
        public static readonly Color VioletaSuave = Color.FromArgb(237, 233, 254);
    }

    // Chip redondeado (texto sobre fondo suave) dibujado con GDI+.
    // Se usa en celdas de DataGridView (CellPainting) y en controles propios.
    public static class StatusChip
    {
        public static readonly Font FuenteChip = new("Segoe UI", 8.5F, FontStyle.Bold);

        // Estado de una unidad física
        public static (Color Fondo, Color Texto) ColoresEstadoUnidad(string? estado) => estado switch
        {
            "Disponible" => (Paleta.VerdeSuave, Paleta.Verde),
            "Prestada" => (Paleta.AmarilloSuave, Paleta.Amarillo),
            "En Mantenimiento" => (Paleta.NaranjaSuave, Paleta.Naranja),
            "Dañada" => (Paleta.RojoSuave, Paleta.Rojo),
            "Perdida" => (Paleta.RojoSuave, Paleta.Rojo),
            _ => (Paleta.GrisSuave, Paleta.Gris)          // Baja u otros
        };

        public static (Color Fondo, Color Texto) ColoresTipoMantenimiento(string? tipo) => tipo switch
        {
            "Preventivo" => (ThemeManager.AccentBlueSoft, ThemeManager.AccentBlue),
            "Correctivo" => (Paleta.NaranjaSuave, Paleta.Naranja),
            "Calibración" => (Paleta.VioletaSuave, Paleta.Violeta),
            _ => (Paleta.GrisSuave, Paleta.Gris)
        };

        public static Size Medir(Graphics g, string texto, Font? fuente = null)
        {
            var tam = TextRenderer.MeasureText(g, texto, fuente ?? FuenteChip, Size.Empty, TextFormatFlags.NoPadding);
            return new Size(tam.Width + 16, tam.Height + 6);
        }

        // Dibuja el chip con su esquina superior izquierda en "origen"; devuelve su rectángulo
        public static Rectangle Dibujar(Graphics g, Point origen, string texto, Color fondo, Color frente, Font? fuente = null)
        {
            fuente ??= FuenteChip;
            var rect = new Rectangle(origen, Medir(g, texto, fuente));

            var suavizado = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (var path = RoundedGeometry.RoundedRect(rect, rect.Height / 2))
            using (var brush = new SolidBrush(fondo))
                g.FillPath(brush, path);
            g.SmoothingMode = suavizado;

            TextRenderer.DrawText(g, texto, fuente, rect, frente,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
            return rect;
        }

        // Chip centrado verticalmente dentro de una celda, con margen izquierdo
        public static Rectangle DibujarEnCelda(Graphics g, Rectangle celda, string texto, Color fondo, Color frente)
        {
            var tam = Medir(g, texto);
            return Dibujar(g, new Point(celda.X + 10, celda.Y + (celda.Height - tam.Height) / 2), texto, fondo, frente);
        }
    }

    // Chip como control (para barras de herramientas: "3 activos", "5 Disponibles"...)
    public class ChipLabel : Control
    {
        private Color _fondo = Paleta.GrisSuave;
        private Color _frente = Paleta.Gris;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color Fondo { get => _fondo; set { _fondo = value; Invalidate(); } }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color Frente { get => _frente; set { _frente = value; Invalidate(); } }

        public ChipLabel()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            Size = new Size(90, 26);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            using var g = CreateGraphics();
            var tam = StatusChip.Medir(g, Text, Font);
            Size = new Size(tam.Width + 4, Math.Max(24, tam.Height + 2));
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(Parent?.BackColor ?? ThemeManager.AppBackground);
            var tam = StatusChip.Medir(e.Graphics, Text, Font);
            StatusChip.Dibujar(e.Graphics, new Point(1, (Height - tam.Height) / 2), Text, _fondo, _frente, Font);
        }
    }
}
