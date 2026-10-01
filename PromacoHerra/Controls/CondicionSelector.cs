using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using PromacoHerra.Models;

namespace PromacoHerra.Controls
{
    // Tres botones toggle: ✓ Bueno (verde) / ⚠ Dañado (amarillo) / ✕ Perdido (rojo). Solo uno activo.
    // El dibujo y el hit-test son estáticos para poder usarlos también dentro de una celda de
    // DataGridView (FrmUnidades), donde no se pueden alojar controles por fila.
    public class CondicionSelector : Control
    {
        private static readonly Font Fuente = new("Segoe UI", 9F, FontStyle.Bold);
        private static readonly string[] Textos = { "✓ Bueno", "⚠ Dañado", "✕ Perdido" };
        private const int Separacion = 6;

        private CondicionDevolucion? _condicion = CondicionDevolucion.Bueno;
        private int _hover = -1;

        public event EventHandler? CondicionCambiada;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CondicionDevolucion? Condicion
        {
            get => _condicion;
            set { if (_condicion != value) { _condicion = value; Invalidate(); } }
        }

        public CondicionSelector()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            Cursor = Cursors.Hand;
            Size = new Size(AnchoPreferido(), 30);
        }

        public static int AnchoPreferido()
        {
            int total = 0;
            foreach (var t in Textos) total += TextRenderer.MeasureText(t, Fuente).Width + 14;
            return total + Separacion * (Textos.Length - 1);
        }

        // Rectángulos de los 3 botones dentro de "area", alineados a la izquierda
        public static Rectangle[] Segmentos(Rectangle area)
        {
            var rects = new Rectangle[Textos.Length];
            int x = area.X;
            int alto = Math.Min(30, area.Height);
            int y = area.Y + (area.Height - alto) / 2;
            for (int i = 0; i < Textos.Length; i++)
            {
                int w = TextRenderer.MeasureText(Textos[i], Fuente).Width + 14;
                rects[i] = new Rectangle(x, y, w, alto);
                x += w + Separacion;
            }
            return rects;
        }

        public static CondicionDevolucion? SegmentoEn(Rectangle area, Point p)
        {
            var rects = Segmentos(area);
            for (int i = 0; i < rects.Length; i++)
                if (rects[i].Contains(p)) return (CondicionDevolucion)i;
            return null;
        }

        private static (Color Fuerte, Color Suave) Colores(int i) => i switch
        {
            0 => (Paleta.Verde, Paleta.VerdeSuave),
            1 => (Paleta.Amarillo, Paleta.AmarilloSuave),
            _ => (Paleta.Rojo, Paleta.RojoSuave)
        };

        // habilitado = false: se dibuja atenuado (p. ej. unidades prestadas o en mantenimiento)
        public static void Dibujar(Graphics g, Rectangle area, CondicionDevolucion? activa, bool habilitado = true, int hover = -1)
        {
            var suavizado = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var rects = Segmentos(area);
            for (int i = 0; i < rects.Length; i++)
            {
                bool activo = activa.HasValue && (int)activa.Value == i;
                var (fuerte, suave) = Colores(i);
                var r = new Rectangle(rects[i].X, rects[i].Y, rects[i].Width - 1, rects[i].Height - 1);

                Color fondo = !habilitado ? Paleta.GrisSuave
                            : activo ? suave
                            : hover == i ? Color.FromArgb(248, 249, 250) : Color.White;
                Color borde = habilitado && activo ? fuerte : ThemeManager.BorderColor;
                Color texto = !habilitado ? Color.FromArgb(180, 185, 195)
                            : activo ? fuerte : ThemeManager.TextSecondary;

                using var path = RoundedGeometry.RoundedRect(r, 6);
                using (var brush = new SolidBrush(fondo)) g.FillPath(brush, path);
                using (var pen = new Pen(borde, habilitado && activo ? 1.5f : 1f)) g.DrawPath(pen, path);
                TextRenderer.DrawText(g, Textos[i], Fuente, rects[i], texto,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }

            g.SmoothingMode = suavizado;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            int h = SegmentoEn(ClientRectangle, e.Location) is { } c ? (int)c : -1;
            if (h != _hover) { _hover = h; Invalidate(); }
        }

        protected override void OnMouseLeave(EventArgs e) { _hover = -1; Invalidate(); base.OnMouseLeave(e); }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (!Enabled || SegmentoEn(ClientRectangle, e.Location) is not { } c) return;
            _condicion = c;
            Invalidate();
            CondicionCambiada?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(Parent?.BackColor ?? ThemeManager.CardBackground);
            Dibujar(e.Graphics, ClientRectangle, _condicion, Enabled, _hover);
        }
    }
}
