using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using PromacoHerra.Controls;

namespace PromacoHerra
{
    // Panel tipo "tarjeta" Material: esquinas redondeadas recortadas de verdad (Region, no solo
    // dibujadas) y una sombra sutil simulada con capas concéntricas semitransparentes pintadas en
    // un margen reservado (ShadowSize) alrededor de la tarjeta.
    public class RoundedPanel : Panel
    {
        private int _cornerRadius = 20;
        private int _shadowSize = 6;
        private bool _showShadow = true;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CornerRadius
        {
            get => _cornerRadius;
            set { _cornerRadius = value; AplicarRegion(); Invalidate(); }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ShadowSize
        {
            get => _shadowSize;
            set { _shadowSize = value; AplicarPadding(); AplicarRegion(); Invalidate(); }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ShowShadow
        {
            get => _showShadow;
            set { _showShadow = value; AplicarPadding(); Invalidate(); }
        }

        public RoundedPanel()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint |
                      ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            DoubleBuffered = true; // Evita parpadeos al redibujar
            BackColor = ThemeManager.CardBackground; // Usamos el blanco por defecto del tema claro
            AplicarPadding(); // Padding interno para que el contenido no toque la curva ni la sombra
        }

        // Solo reserva el margen de la sombra (derecha/abajo); no impone un padding de
        // contenido fijo porque cada formulario ya maneja el suyo (algunos posicionan
        // hijos con Location absoluto, otros los acoplan con Dock a todo el ancho).
        private void AplicarPadding()
        {
            int extra = _showShadow ? _shadowSize : 0;
            Padding = new Padding(0, 0, extra, extra);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            AplicarRegion();
        }

        // El Region cubre TODO el control (no solo la tarjeta) para no recortar la sombra que se
        // pinta en el margen reservado; el recorte real de esquinas cuadradas ocurre igual en el
        // borde exterior del control, que es lo que ven los hijos y el hit-testing del mouse.
        private void AplicarRegion()
        {
            if (Width <= 0 || Height <= 0) return;
            using var path = RoundedGeometry.RoundedRect(new Rectangle(0, 0, Width, Height), _cornerRadius);
            Region = new Region(path);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int shadow = _showShadow ? _shadowSize : 0;
            var cardRect = new Rectangle(0, 0, Width - shadow - 1, Height - shadow - 1);

            if (_showShadow)
            {
                for (int i = shadow; i >= 1; i--)
                {
                    var ringRect = new Rectangle(i, i, cardRect.Width, cardRect.Height);
                    using var ringPath = RoundedGeometry.RoundedRect(ringRect, _cornerRadius);
                    int alpha = (int)(12 * ((shadow - i + 1) / (float)shadow));
                    using var ringBrush = new SolidBrush(Color.FromArgb(alpha, 15, 23, 42));
                    g.FillPath(ringBrush, ringPath);
                }
            }

            using (var path = RoundedGeometry.RoundedRect(cardRect, _cornerRadius))
            {
                using (var brush = new SolidBrush(BackColor))
                    g.FillPath(brush, path);

                using (var borderPen = new Pen(ThemeManager.BorderColor, 1))
                    g.DrawPath(borderPen, path);
            }

            base.OnPaint(e);
        }
    }
}