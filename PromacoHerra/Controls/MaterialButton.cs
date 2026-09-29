using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using FontAwesome.Sharp;

namespace PromacoHerra.Controls
{
    public enum MaterialButtonVariant
    {
        Default,
        Primary,
        Secondary,
        Danger,
        Sidebar
    }

    // Button plano, sin relieve 3D, con esquinas redondeadas recortadas de verdad (Region)
    // y transición de color animada en hover/press. La variante determina la paleta,
    // siempre tomada de ThemeManager (nunca colores nuevos inventados aquí).
    public class MaterialButton : Button
    {
        private MaterialButtonVariant _variant = MaterialButtonVariant.Default;
        private int _cornerRadius = ThemeManager.CornerRadiusButton;
        private readonly ColorAnimator _bgAnimator;

        private IconChar? _icon;
        private Color? _iconColor;
        private int _iconSize = 16;
        private Bitmap? _iconBitmap;
        private Color _iconBitmapColor;
        private int _iconBitmapSize;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public MaterialButtonVariant Variant
        {
            get => _variant;
            set { _variant = value; _bgAnimator.AnimateTo(BackColorFor(false)); Invalidate(); }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int CornerRadius
        {
            get => _cornerRadius;
            set { _cornerRadius = value; AplicarRegion(); Invalidate(); }
        }

        // Ícono FontAwesome opcional, dibujado a la izquierda del texto. null = sin ícono
        // (comportamiento idéntico al de antes de agregar esta propiedad).
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public IconChar? Icon
        {
            get => _icon;
            set { _icon = value; Invalidate(); }
        }

        // Color del ícono. null (default) = usa el mismo color que el texto en cada estado
        // (incluye el gris automático cuando el botón está deshabilitado).
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color? IconColor
        {
            get => _iconColor;
            set { _iconColor = value; Invalidate(); }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int IconSize
        {
            get => _iconSize;
            set { _iconSize = value; Invalidate(); }
        }

        public MaterialButton()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint |
                      ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            DoubleBuffered = true;

            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            FlatAppearance.MouseOverBackColor = Color.Transparent;
            FlatAppearance.MouseDownBackColor = Color.Transparent;
            Cursor = Cursors.Hand;
            Font = new Font("Segoe UI", ThemeManager.SmallBoldFontSize, FontStyle.Bold);
            if (Height < 38) Height = 38;

            _bgAnimator = new ColorAnimator(this, BackColorFor(false));
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            _bgAnimator.AnimateTo(BackColorFor(false));
            Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if (Enabled) _bgAnimator.AnimateTo(BackColorFor(true));
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _bgAnimator.AnimateTo(BackColorFor(false));
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            AplicarRegion();
        }

        private void AplicarRegion()
        {
            if (Width <= 0 || Height <= 0) return;
            using var path = RoundedGeometry.RoundedRect(new Rectangle(0, 0, Width, Height), _cornerRadius);
            Region = new Region(path);
        }

        private (Color bg, Color fg, Color border, Color hover) Palette()
        {
            switch (_variant)
            {
                case MaterialButtonVariant.Sidebar:
                    return (ThemeManager.SidebarColor, ThemeManager.TextOnDark,
                            Color.Transparent, ThemeManager.SidebarHover);

                case MaterialButtonVariant.Primary:
                    return (ThemeManager.AccentBlue, ThemeManager.TextOnDark,
                            Color.Transparent, Color.FromArgb(0, 65, 150));

                case MaterialButtonVariant.Danger:
                    return (Color.FromArgb(254, 242, 242), ThemeManager.DangerRed,
                            Color.FromArgb(252, 165, 165), Color.FromArgb(254, 226, 226));

                case MaterialButtonVariant.Secondary:
                    return (Color.FromArgb(248, 249, 250), ThemeManager.TextPrimary,
                            ThemeManager.BorderColor, Color.FromArgb(237, 239, 244));

                default:
                    return (ThemeManager.CardBackground, ThemeManager.TextPrimary,
                            ThemeManager.BorderColor, ThemeManager.AccentBlueSoft);
            }
        }

        private Color BackColorFor(bool hover)
        {
            if (!Enabled) return Color.FromArgb(235, 236, 240);
            var p = Palette();
            return hover ? p.hover : p.bg;
        }

        private Color ForeColorForState()
        {
            if (!Enabled) return ThemeManager.TextSecondary;
            return Palette().fg;
        }

        // Cachea el bitmap del ícono; solo lo regenera si cambió el color o el tamaño
        // (evita crear un bitmap nuevo en cada frame de la animación de hover).
        private Bitmap? GetIconBitmap(Color color)
        {
            if (_icon == null) return null;

            if (_iconBitmap == null || _iconBitmapColor != color || _iconBitmapSize != _iconSize)
            {
                _iconBitmap?.Dispose();
                _iconBitmap = _icon.Value.ToBitmap(color, _iconSize);
                _iconBitmapColor = color;
                _iconBitmapSize = _iconSize;
            }

            return _iconBitmap;
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            var g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Parent?.BackColor ?? ThemeManager.AppBackground);

            var rect = new Rectangle(0, 0, Width - 1, Height - 1);
            var palette = Palette();

            using (var path = RoundedGeometry.RoundedRect(rect, _cornerRadius))
            {
                using (var bgBrush = new SolidBrush(_bgAnimator.Current))
                    g.FillPath(bgBrush, path);

                if (Enabled && palette.border != Color.Transparent)
                    using (var pen = new Pen(palette.border, 1))
                        g.DrawPath(pen, path);
            }

            var textColor = ForeColorForState();
            var iconBitmap = GetIconBitmap(_iconColor ?? textColor);

            if (iconBitmap == null)
            {
                TextRenderer.DrawText(g, Text, Font, ClientRectangle, textColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
            }
            else if (string.IsNullOrEmpty(Text))
            {
                // Botón solo-ícono (sin texto): centrado simple, sin espaciado de sobra.
                int iconX = (ClientRectangle.Width - _iconSize) / 2;
                int iconY = (ClientRectangle.Height - _iconSize) / 2;
                g.DrawImage(iconBitmap, iconX, iconY, _iconSize, _iconSize);
            }
            else
            {
                const int gap = 8;
                var textSize = TextRenderer.MeasureText(g, Text, Font);
                int blockWidth = _iconSize + gap + textSize.Width;
                int startX = Math.Max(0, (ClientRectangle.Width - blockWidth) / 2);
                int iconY = (ClientRectangle.Height - _iconSize) / 2;

                g.DrawImage(iconBitmap, startX, iconY, _iconSize, _iconSize);

                var textRect = new Rectangle(startX + _iconSize + gap, 0,
                    ClientRectangle.Width - (startX + _iconSize + gap), ClientRectangle.Height);
                TextRenderer.DrawText(g, Text, Font, textRect, textColor,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _iconBitmap?.Dispose();
            base.Dispose(disposing);
        }
    }
}
