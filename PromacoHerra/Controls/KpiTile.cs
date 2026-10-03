using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using FontAwesome.Sharp;

namespace PromacoHerra.Controls
{
    // Tile de indicador: borde izquierdo de color, ícono en cuadro suave, número grande,
    // etiqueta y un texto de cambio ("↑ 2 vencieron hoy"). Si Clickeable, se resalta al pasar el mouse.
    public class KpiTile : Control
    {
        private static readonly Font FuenteValor = new("Segoe UI", 18F, FontStyle.Bold);
        private static readonly Font FuenteEtiqueta = new("Segoe UI", 9F, FontStyle.Bold);
        private static readonly Font FuenteDelta = new("Segoe UI", 8F);

        private int _valor;
        private string _etiqueta = "";
        private string _delta = "";
        private Color _color = ThemeManager.AccentBlue;
        private Color _suave = ThemeManager.AccentBlueSoft;
        private IconChar _icono = IconChar.ChartColumn;
        private Bitmap? _iconoBmp;
        private bool _clickeable;
        private bool _hover;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Valor { get => _valor; set { _valor = value; Invalidate(); } }

        // Valor como texto (montos, promedios). null = mostrar Valor como número entero
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string? Texto { get => _texto; set { _texto = value; Invalidate(); } }
        private string? _texto;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Etiqueta { get => _etiqueta; set { _etiqueta = value ?? ""; Invalidate(); } }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Delta { get => _delta; set { _delta = value ?? ""; Invalidate(); } }

        // Color del texto de cambio; por defecto gris
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color ColorDelta { get; set; } = ThemeManager.TextSecondary;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Clickeable
        {
            get => _clickeable;
            set { _clickeable = value; Cursor = value ? Cursors.Hand : Cursors.Default; }
        }

        public KpiTile(string etiqueta, IconChar icono, Color color, Color suave)
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            _etiqueta = etiqueta;
            _icono = icono;
            _color = color;
            _suave = suave;
            Height = 92;
        }

        protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Parent?.BackColor ?? ThemeManager.AppBackground);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var rect = new Rectangle(0, 0, Width - 1, Height - 1);
            using (var path = RoundedGeometry.RoundedRect(rect, 8))
            {
                using (var brush = new SolidBrush(_clickeable && _hover ? Color.FromArgb(250, 251, 253) : Color.White))
                    g.FillPath(brush, path);
                using (var pen = new Pen(_clickeable && _hover ? _color : ThemeManager.BorderColor))
                    g.DrawPath(pen, path);

                // Borde izquierdo de color (3px), recortado a las esquinas redondeadas
                var clip = g.Clip;
                g.SetClip(path);
                using (var brush = new SolidBrush(_color))
                    g.FillRectangle(brush, 0, 0, 3, Height);
                g.Clip = clip;
            }

            // Ícono en cuadro suave
            var cuadro = new Rectangle(16, 16, 36, 36);
            using (var path = RoundedGeometry.RoundedRect(cuadro, 8))
            using (var brush = new SolidBrush(_suave))
                g.FillPath(brush, path);
            _iconoBmp ??= _icono.ToBitmap(_color, 18);
            g.DrawImage(_iconoBmp, cuadro.X + 9, cuadro.Y + 9, 18, 18);

            int x = cuadro.Right + 12;
            int ancho = Math.Max(10, Width - x - 10);
            TextRenderer.DrawText(g, _texto ?? _valor.ToString("N0"), FuenteValor, new Rectangle(x, 8, ancho, 34), _color,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
            TextRenderer.DrawText(g, _etiqueta, FuenteEtiqueta, new Rectangle(x, 44, ancho, 18), ThemeManager.TextPrimary,
                TextFormatFlags.Left | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);
            TextRenderer.DrawText(g, _delta, FuenteDelta, new Rectangle(x, 64, ancho, 16), ColorDelta,
                TextFormatFlags.Left | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _iconoBmp?.Dispose();
            base.Dispose(disposing);
        }
    }

    // Tarjeta para elegir un tipo de reporte: ícono, nombre y descripción.
    // Seleccionada: borde azul 2px y fondo accent-soft.
    public class ReporteCard : Control
    {
        private static readonly Font FuenteNombre = new("Segoe UI", 9F, FontStyle.Bold);
        private static readonly Font FuenteDescripcion = new("Segoe UI", 8F);

        private readonly IconChar _icono;
        private readonly string _descripcion;
        private Bitmap? _iconoNormal;
        private Bitmap? _iconoActivo;
        private bool _seleccionada;
        private bool _hover;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Seleccionada
        {
            get => _seleccionada;
            set { if (_seleccionada != value) { _seleccionada = value; Invalidate(); } }
        }

        public ReporteCard(string nombre, string descripcion, IconChar icono)
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            Text = nombre;
            _descripcion = descripcion;
            _icono = icono;
            Cursor = Cursors.Hand;
            Size = new Size(158, 84);
            Margin = new Padding(0, 0, 8, 8);
        }

        protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Parent?.BackColor ?? ThemeManager.AppBackground);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var rect = new Rectangle(1, 1, Width - 3, Height - 3);
            using (var path = RoundedGeometry.RoundedRect(rect, 8))
            {
                using (var brush = new SolidBrush(_seleccionada ? ThemeManager.AccentBlueSoft : Color.White))
                    g.FillPath(brush, path);
                using var pen = new Pen(_seleccionada ? ThemeManager.AccentBlue
                                        : _hover ? Color.FromArgb(190, 200, 215) : ThemeManager.BorderColor, 2f);
                g.DrawPath(pen, path);
            }

            var color = _seleccionada ? ThemeManager.AccentBlue : ThemeManager.TextSecondary;
            var icono = _seleccionada ? (_iconoActivo ??= _icono.ToBitmap(ThemeManager.AccentBlue, 18))
                                      : (_iconoNormal ??= _icono.ToBitmap(ThemeManager.TextSecondary, 18));
            g.DrawImage(icono, 12, 13, 18, 18);

            // Nombre al lado del ícono (hasta 2 líneas) y descripción debajo (hasta 2 líneas)
            var flags = TextFormatFlags.Left | TextFormatFlags.WordBreak | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding;
            TextRenderer.DrawText(g, Text, FuenteNombre, new Rectangle(38, 12, Width - 48, 32),
                _seleccionada ? ThemeManager.AccentBlue : ThemeManager.TextPrimary, flags);
            TextRenderer.DrawText(g, _descripcion, FuenteDescripcion, new Rectangle(12, 48, Width - 22, 28), color, flags);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) { _iconoNormal?.Dispose(); _iconoActivo?.Dispose(); }
            base.Dispose(disposing);
        }
    }
}
