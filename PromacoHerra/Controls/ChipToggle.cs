using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PromacoHerra.Controls
{
    // Chip de filtro estilo toggle (pastilla redondeada). Categoria null = chip "Todas".
    // Usado en el catálogo de FrmPrestamo y en la lista de FrmHerramientas.
    public class ChipToggle : Control
    {
        private bool _seleccionado;
        private bool _hover;

        // null = chip "Todas"
        public string? Categoria { get; }

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public bool Seleccionado
        {
            get => _seleccionado;
            set { if (_seleccionado != value) { _seleccionado = value; Invalidate(); } }
        }

        public ChipToggle(string texto, string? categoria)
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            Text = texto;
            Categoria = categoria;
            Font = new Font("Segoe UI", 9.5F);
            Cursor = Cursors.Hand;
            Margin = new Padding(0, 0, 8, 8);

            var tam = TextRenderer.MeasureText(texto, Font);
            Size = new Size(tam.Width + 28, 30);
        }

        protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Parent?.BackColor ?? ThemeManager.AppBackground);

            var rect = new Rectangle(0, 0, Width - 1, Height - 1);
            using var path = RoundedGeometry.RoundedRect(rect, Height / 2);

            Color fondo = _seleccionado ? ThemeManager.AccentBlue
                        : _hover ? ThemeManager.AccentBlueSoft
                        : ThemeManager.CardBackground;
            using (var brush = new SolidBrush(fondo))
                g.FillPath(brush, path);

            if (!_seleccionado)
                using (var pen = new Pen(ThemeManager.BorderColor, 1))
                    g.DrawPath(pen, path);

            TextRenderer.DrawText(g, Text, Font, ClientRectangle,
                _seleccionado ? Color.White : ThemeManager.TextPrimary,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }
    }
}
