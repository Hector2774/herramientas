using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PromacoHerra.Controls
{
    // CheckBox plano dibujado a mano (owner-draw), coherente con el resto de la familia Material.
    // Hoy ningún formulario usa CheckBox todavía; queda listo para cuando se necesite.
    public class MaterialCheckBox : CheckBox
    {
        private const int BoxSize = 18;
        private readonly ColorAnimator _boxAnimator;

        public MaterialCheckBox()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint |
                      ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            DoubleBuffered = true;

            Font = new Font("Segoe UI", ThemeManager.BaseFontSize);
            ForeColor = ThemeManager.TextPrimary;
            Cursor = Cursors.Hand;
            MinimumSize = new Size(0, 24);

            _boxAnimator = new ColorAnimator(this, Checked ? ThemeManager.AccentBlue : ThemeManager.CardBackground);
            CheckedChanged += (s, e) => _boxAnimator.AnimateTo(Checked ? ThemeManager.AccentBlue : ThemeManager.CardBackground);
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            var g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Parent?.BackColor ?? ThemeManager.AppBackground);

            int y = (Height - BoxSize) / 2;
            var boxRect = new Rectangle(0, y, BoxSize, BoxSize);

            using (var path = RoundedGeometry.RoundedRect(boxRect, 4))
            {
                using (var brush = new SolidBrush(_boxAnimator.Current))
                    g.FillPath(brush, path);

                using (var pen = new Pen(Checked ? ThemeManager.AccentBlue : ThemeManager.BorderColor, 1.5f))
                    g.DrawPath(pen, path);
            }

            if (Checked)
            {
                using var checkPen = new Pen(Color.White, 2f)
                {
                    StartCap = LineCap.Round,
                    EndCap = LineCap.Round,
                    LineJoin = LineJoin.Round
                };
                g.DrawLines(checkPen, new[]
                {
                    new Point(4, y + 9),
                    new Point(7, y + 13),
                    new Point(14, y + 5)
                });
            }

            var textRect = new Rectangle(BoxSize + 8, 0, Math.Max(0, Width - BoxSize - 8), Height);
            TextRenderer.DrawText(g, Text, Font, textRect, ForeColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
        }
    }
}
