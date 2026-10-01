using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PromacoHerra.Controls
{
    // Tile de estadística: número grande coloreado + etiqueta pequeña en mayúsculas.
    // Fondo Surface2 con esquinas de 6px. Se usan 5 en fila en el detalle de herramienta.
    public class StockTile : Control
    {
        private static readonly Font FuenteValor = new("Segoe UI", 15F, FontStyle.Bold);
        private static readonly Font FuenteEtiqueta = new("Segoe UI", 7.5F, FontStyle.Bold);

        private int _valor;
        private string _etiqueta = "";
        private Color _color = ThemeManager.TextPrimary;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int Valor { get => _valor; set { _valor = value; Invalidate(); } }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Etiqueta { get => _etiqueta; set { _etiqueta = value ?? ""; Invalidate(); } }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color ColorValor { get => _color; set { _color = value; Invalidate(); } }

        public StockTile()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            Size = new Size(120, 70);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Parent?.BackColor ?? ThemeManager.AppBackground);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (var path = RoundedGeometry.RoundedRect(new Rectangle(0, 0, Width - 1, Height - 1), 6))
            using (var brush = new SolidBrush(Paleta.Surface2))
                g.FillPath(brush, path);

            TextRenderer.DrawText(g, _valor.ToString(), FuenteValor, new Rectangle(12, 8, Width - 20, 30),
                _color, TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
            TextRenderer.DrawText(g, _etiqueta.ToUpperInvariant(), FuenteEtiqueta, new Rectangle(12, 42, Width - 20, 18),
                ThemeManager.TextSecondary, TextFormatFlags.Left | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);
        }
    }
}
