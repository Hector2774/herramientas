using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PromacoHerra.Controls
{
    // RadioButton plano dibujado a mano (owner-draw), mismo lenguaje visual que MaterialCheckBox.
    // Hoy ningún formulario usa RadioButton todavía; queda listo para cuando se necesite.
    public class MaterialRadioButton : RadioButton
    {
        private const int CircleSize = 18;
        private readonly ColorAnimator _dotAnimator;

        public MaterialRadioButton()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint |
                      ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            DoubleBuffered = true;

            Font = new Font("Segoe UI", ThemeManager.BaseFontSize);
            ForeColor = ThemeManager.TextPrimary;
            Cursor = Cursors.Hand;
            MinimumSize = new Size(0, 24);

            _dotAnimator = new ColorAnimator(this, Checked ? ThemeManager.AccentBlue : Color.Transparent);
            CheckedChanged += (s, e) => _dotAnimator.AnimateTo(Checked ? ThemeManager.AccentBlue : Color.Transparent);
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            var g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Parent?.BackColor ?? ThemeManager.AppBackground);

            int y = (Height - CircleSize) / 2;
            var circleRect = new Rectangle(0, y, CircleSize, CircleSize);

            using (var pen = new Pen(Checked ? ThemeManager.AccentBlue : ThemeManager.BorderColor, 1.5f))
                g.DrawEllipse(pen, circleRect);

            if (_dotAnimator.Current.A > 0)
            {
                var innerRect = Rectangle.Inflate(circleRect, -5, -5);
                using var brush = new SolidBrush(_dotAnimator.Current);
                g.FillEllipse(brush, innerRect);
            }

            var textRect = new Rectangle(CircleSize + 8, 0, Math.Max(0, Width - CircleSize - 8), Height);
            TextRenderer.DrawText(g, Text, Font, textRect, ForeColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
        }
    }
}
