using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PromacoHerra.Controls
{
    // Control segmentado tipo iOS: "Categorías | Marcas | Ubicaciones". Un solo segmento activo.
    public class SegmentedControl : Control
    {
        private string[] _opciones = { "Opción 1", "Opción 2" };
        private int _seleccionado;
        private int _hover = -1;

        public event EventHandler? SeleccionCambiada;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string[] Opciones { get => _opciones; set { _opciones = value ?? Array.Empty<string>(); Invalidate(); } }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Seleccionado
        {
            get => _seleccionado;
            set
            {
                if (_seleccionado == value) return;
                _seleccionado = value;
                Invalidate();
                SeleccionCambiada?.Invoke(this, EventArgs.Empty);
            }
        }

        public SegmentedControl()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            Cursor = Cursors.Hand;
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            Size = new Size(260, 36);
        }

        private Rectangle Segmento(int i)
        {
            int w = (Width - 8) / Math.Max(1, _opciones.Length);
            return new Rectangle(4 + i * w, 4, w, Height - 8);
        }

        private int IndiceEn(Point p)
        {
            for (int i = 0; i < _opciones.Length; i++)
                if (Segmento(i).Contains(p)) return i;
            return -1;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            int h = IndiceEn(e.Location);
            if (h != _hover) { _hover = h; Invalidate(); }
        }

        protected override void OnMouseLeave(EventArgs e) { _hover = -1; Invalidate(); base.OnMouseLeave(e); }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            int i = IndiceEn(e.Location);
            if (i >= 0) Seleccionado = i;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Parent?.BackColor ?? ThemeManager.AppBackground);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (var path = RoundedGeometry.RoundedRect(new Rectangle(0, 0, Width - 1, Height - 1), 8))
            using (var brush = new SolidBrush(Paleta.Surface2))
                g.FillPath(brush, path);

            for (int i = 0; i < _opciones.Length; i++)
            {
                var r = Segmento(i);
                bool activo = i == _seleccionado;
                if (activo)
                {
                    using var path = RoundedGeometry.RoundedRect(new Rectangle(r.X, r.Y, r.Width - 1, r.Height - 1), 6);
                    using (var brush = new SolidBrush(Color.White)) g.FillPath(brush, path);
                    using (var pen = new Pen(ThemeManager.BorderColor)) g.DrawPath(pen, path);
                }
                TextRenderer.DrawText(g, _opciones[i], Font, r,
                    activo ? ThemeManager.AccentBlue : _hover == i ? ThemeManager.TextPrimary : ThemeManager.TextSecondary,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
        }
    }
}
