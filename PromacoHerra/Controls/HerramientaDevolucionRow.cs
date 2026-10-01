using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using FontAwesome.Sharp;
using PromacoHerra.Models;

namespace PromacoHerra.Controls
{
    // Una unidad prestada en el detalle de Devoluciones:
    //   [☐] [img]  Nombre                         [✓ Bueno] [⚠ Dañado] [✕ Perdido]  ← CondicionSelector
    //              HER-0001-02  Marca · Categoría
    //              [Nota de condición...]   ← solo cuando la fila está seleccionada
    // Click en la fila = seleccionar/deseleccionar. Elegir una condición también la selecciona.
    public class HerramientaDevolucionRow : Panel
    {
        private const int AltoCerrado = 68;
        private const int AltoAbierto = 116;
        private const int XTexto = 96;
        private const int Radio = 8;

        private static readonly Font FuenteNombre = new("Segoe UI", 10F, FontStyle.Bold);
        private static readonly Font FuenteMeta = new("Segoe UI", 9F);
        private static readonly Font FuenteCodigo = new("Consolas", 8.5F);
        private static readonly Color GrisSuave = Color.FromArgb(243, 244, 246);
        private static readonly Color CheckBorde = Color.FromArgb(156, 163, 175);
        private static Bitmap? _iconoHerramienta;

        private readonly CondicionSelector _selector;
        private readonly MaterialTextBox _txtNota;

        private bool _seleccionado;
        private CondicionDevolucion _condicion = CondicionDevolucion.Bueno;

        public event EventHandler? SeleccionCambiada;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DetallePendiente Detalle { get; }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Seleccionado
        {
            get => _seleccionado;
            set
            {
                if (_seleccionado == value) return;
                _seleccionado = value;
                BackColor = value ? ThemeManager.AccentBlueSoft : ThemeManager.CardBackground;
                Height = value ? AltoAbierto : AltoCerrado;
                _txtNota.Visible = value;
                Invalidate(true);
                SeleccionCambiada?.Invoke(this, EventArgs.Empty);
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CondicionDevolucion Condicion
        {
            get => _condicion;
            set
            {
                _condicion = value;
                _selector.Condicion = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Nota => _txtNota.Text.Trim();

        public HerramientaDevolucionRow(DetallePendiente detalle)
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            Detalle = detalle;
            BackColor = ThemeManager.CardBackground;
            Height = AltoCerrado;
            Margin = new Padding(0, 0, 0, 8);
            Cursor = Cursors.Hand;

            _selector = new CondicionSelector();
            _selector.CondicionCambiada += (s, e) => ElegirCondicion(_selector.Condicion ?? CondicionDevolucion.Bueno);

            _txtNota = new MaterialTextBox
            {
                PlaceholderText = "Nota de condición...",
                Visible = false,
                Cursor = Cursors.IBeam
            };

            Controls.AddRange(new Control[] { _selector, _txtNota });
            Condicion = CondicionDevolucion.Bueno;
        }

        private void ElegirCondicion(CondicionDevolucion condicion)
        {
            Condicion = condicion;
            Seleccionado = true;
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            Seleccionado = !Seleccionado;
            if (Seleccionado) _txtNota.Focus();
        }

        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
            if (_selector == null) return;

            _selector.Location = new Point(Width - 14 - _selector.Width, 19);
            _txtNota.SetBounds(XTexto, 66, Math.Max(100, Width - XTexto - 14), 38);
        }

        // Fuera del rectángulo redondeado se ve el fondo del contenedor
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            e.Graphics.Clear(Parent?.BackColor ?? ThemeManager.AppBackground);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var rect = new RectangleF(0.75f, 0.75f, Width - 2.5f, Height - 2.5f);
            using (var path = RoundedGeometry.RoundedRect(Rectangle.Round(rect), Radio))
            {
                using (var brush = new SolidBrush(BackColor))
                    g.FillPath(brush, path);
                using (var pen = new Pen(_seleccionado ? ThemeManager.AccentBlue : ThemeManager.BorderColor,
                                         _seleccionado ? 1.5f : 1f))
                    g.DrawPath(pen, path);
            }

            DibujarCheckbox(g, new Rectangle(14, 25, 18, 18));
            DibujarMiniatura(g, new Rectangle(44, 14, 40, 40));

            // Texto: hasta donde empiezan los botones de condición
            int anchoTexto = Math.Max(40, _selector.Left - XTexto - 10);
            TextRenderer.DrawText(g, Detalle.Herramienta, FuenteNombre, new Rectangle(XTexto, 13, anchoTexto, 20),
                ThemeManager.TextPrimary, TextFormatFlags.Left | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);

            var tamCod = TextRenderer.MeasureText(g, Detalle.Codigo, FuenteCodigo, Size.Empty, TextFormatFlags.NoPadding);
            var rectCod = new Rectangle(XTexto, 36, tamCod.Width + 12, tamCod.Height + 6);
            using (var path = RoundedGeometry.RoundedRect(rectCod, 4))
            using (var brush = new SolidBrush(_seleccionado ? Color.White : GrisSuave))
                g.FillPath(brush, path);
            TextRenderer.DrawText(g, Detalle.Codigo, FuenteCodigo, rectCod, ThemeManager.TextPrimary,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);

            string meta = string.Join(" · ", new[] { Detalle.Marca, Detalle.Categoria }.Where(s => !string.IsNullOrEmpty(s)));
            TextRenderer.DrawText(g, meta, FuenteMeta,
                new Rectangle(rectCod.Right + 8, 37, Math.Max(0, XTexto + anchoTexto - rectCod.Right - 8), 18),
                ThemeManager.TextSecondary, TextFormatFlags.Left | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);
        }

        // Checkbox cuadrado 18x18 con radio 5
        private void DibujarCheckbox(Graphics g, Rectangle r)
        {
            using var path = RoundedGeometry.RoundedRect(r, 5);
            if (_seleccionado)
            {
                using (var brush = new SolidBrush(ThemeManager.AccentBlue))
                    g.FillPath(brush, path);
                using var pen = new Pen(Color.White, 2f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round };
                g.DrawLines(pen, new[]
                {
                    new PointF(r.X + 4.5f, r.Y + 9.5f),
                    new PointF(r.X + 7.8f, r.Y + 12.8f),
                    new PointF(r.X + 13.5f, r.Y + 5.5f)
                });
            }
            else
            {
                using (var brush = new SolidBrush(Color.White))
                    g.FillPath(brush, path);
                using var pen = new Pen(CheckBorde, 1.5f);
                g.DrawPath(pen, path);
            }
        }

        // Miniatura reservada para la foto futura: fondo gris + ícono de herramienta
        private void DibujarMiniatura(Graphics g, Rectangle r)
        {
            using (var path = RoundedGeometry.RoundedRect(r, 6))
            using (var brush = new SolidBrush(_seleccionado ? Color.White : GrisSuave))
                g.FillPath(brush, path);

            _iconoHerramienta ??= IconChar.Wrench.ToBitmap(ThemeManager.TextSecondary, 20);
            g.DrawImage(_iconoHerramienta, r.X + (r.Width - 20) / 2, r.Y + (r.Height - 20) / 2, 20, 20);
        }
    }
}
