using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using FontAwesome.Sharp;

namespace PromacoHerra.Controls
{
    // Menú lateral dibujado con GDI+ (sin un botón por ítem):
    //   header con el nombre de la app · ítems con ícono y texto · píldora del ítem activo
    //   (redondeada, con margen lateral y barra de acento a la izquierda) que se desliza
    //   animada al cambiar de sección · hover con fondo blanco translúcido animado.
    // Usa los colores de ThemeManager (SidebarColor, AccentBlue) y no tiene hijos, así que
    // ThemeManager.ApplyTheme no lo modifica.
    public class SidebarControl : Control
    {
        public sealed record Item(string Clave, string Texto, IconChar Icono, bool AlFinal = false);

        private const int AltoHeader = 92;
        private const int AltoItem = 44;
        private const int Separacion = 4;
        private const int MargenLateral = 10;
        private const int TamIcono = 18;

        // Colores derivados de la paleta: blanco translúcido para hover/activo y un azul
        // más claro que AccentBlue para que la barra contraste sobre el fondo oscuro
        private static readonly Color AcentoClaro = Mezclar(ThemeManager.AccentBlue, Color.White, 0.35f);
        private static readonly Font FuenteApp = new("Segoe UI", 15F, FontStyle.Bold);
        private static readonly Font FuenteSub = new("Segoe UI", 8.5F);
        private static readonly Font FuenteItem = new("Segoe UI", 10F);
        private static readonly Font FuenteActivo = new("Segoe UI", 10F, FontStyle.Bold);

        private readonly List<Item> _items = new();
        private readonly Dictionary<string, float> _hover = new();        // 0..1 por ítem (animado)
        private readonly Dictionary<(IconChar, int), Bitmap> _iconos = new();
        private readonly System.Windows.Forms.Timer _timer = new() { Interval = 15 };

        private string? _activo;
        private string? _bajoMouse;
        private float _pildoraY = -1;     // posición animada de la píldora activa
        private float _pildoraAlfa;       // 0..1, aparece la primera vez

        public event EventHandler<string>? ItemSeleccionado;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string TituloApp { get; set; } = "PROMACO";

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SubtituloApp { get; set; } = "Control de herramientas";

        // Clave del ítem activo; al cambiar, la píldora se desliza hacia él
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string? Activo
        {
            get => _activo;
            set
            {
                if (_activo == value) return;
                _activo = value;
                if (_pildoraY < 0 && RectItem(value) is { } r) _pildoraY = r.Y;   // primera vez: sin deslizar
                _timer.Start();
            }
        }

        public SidebarControl()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            DoubleBuffered = true;
            BackColor = ThemeManager.SidebarColor;
            Width = 220;
            Cursor = Cursors.Default;
            _timer.Tick += (s, e) => Animar();
        }

        public void AgregarItem(string clave, string texto, IconChar icono, bool alFinal = false)
        {
            _items.Add(new Item(clave, texto, icono, alFinal));
            _hover[clave] = 0;
            Invalidate();
        }

        // ── Geometría ──────────────────────────────────────────────
        private Rectangle? RectItem(string? clave)
        {
            if (clave == null) return null;
            var arriba = _items.Where(i => !i.AlFinal).ToList();
            int idx = arriba.FindIndex(i => i.Clave == clave);
            if (idx >= 0)
                return new Rectangle(MargenLateral, AltoHeader + idx * (AltoItem + Separacion), Width - 2 * MargenLateral, AltoItem);

            var abajo = _items.Where(i => i.AlFinal).ToList();
            idx = abajo.FindIndex(i => i.Clave == clave);
            if (idx < 0) return null;
            int y = Height - 14 - (abajo.Count - idx) * (AltoItem + Separacion) + Separacion;
            return new Rectangle(MargenLateral, y, Width - 2 * MargenLateral, AltoItem);
        }

        private int YSeparador => (RectItem(_items.FirstOrDefault(i => i.AlFinal)?.Clave)?.Y ?? Height) - 12;

        private Item? ItemEn(Point p) => _items.FirstOrDefault(i => RectItem(i.Clave)?.Contains(p) == true);

        // ── Animación: la píldora se acerca a su destino y el hover sube/baja de opacidad ──
        private void Animar()
        {
            bool sigue = false;

            if (RectItem(_activo) is { } destino)
            {
                float dy = destino.Y - _pildoraY;
                if (Math.Abs(dy) > 0.5f) { _pildoraY += dy * 0.25f; sigue = true; }
                else _pildoraY = destino.Y;

                if (_pildoraAlfa < 1) { _pildoraAlfa = Math.Min(1, _pildoraAlfa + 0.12f); sigue = true; }
            }

            foreach (var clave in _hover.Keys.ToList())
            {
                float objetivo = clave == _bajoMouse && clave != _activo ? 1f : 0f;
                float actual = _hover[clave];
                if (Math.Abs(objetivo - actual) > 0.01f)
                {
                    _hover[clave] = actual + (objetivo - actual) * 0.25f;
                    sigue = true;
                }
                else _hover[clave] = objetivo;
            }

            Invalidate();
            if (!sigue) _timer.Stop();
        }

        // ── Mouse ──────────────────────────────────────────────────
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            var item = ItemEn(e.Location);
            if (item?.Clave != _bajoMouse)
            {
                _bajoMouse = item?.Clave;
                Cursor = item != null ? Cursors.Hand : Cursors.Default;
                _timer.Start();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _bajoMouse = null;
            Cursor = Cursors.Default;
            _timer.Start();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (e.Button == MouseButtons.Left && ItemEn(e.Location) is { } item)
                ItemSeleccionado?.Invoke(this, item.Clave);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (RectItem(_activo) is { } r) _pildoraY = r.Y;   // sin animar al redimensionar
        }

        // ── Dibujo ─────────────────────────────────────────────────
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(BackColor);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Header
            TextRenderer.DrawText(g, TituloApp, FuenteApp, new Point(22, 26), Color.White);
            TextRenderer.DrawText(g, SubtituloApp, FuenteSub, new Point(24, 56), Atenuado(0.6f));

            // Píldora activa (deslizándose) con barra de acento
            if (_activo != null && _pildoraY >= 0)
            {
                var pildora = new Rectangle(MargenLateral, (int)Math.Round(_pildoraY), Width - 2 * MargenLateral, AltoItem);
                int alfa = (int)(46 * _pildoraAlfa);   // ~18 %: algo más fuerte que el hover (~12 %)
                using (var path = RoundedGeometry.RoundedRect(pildora, 8))
                using (var brush = new SolidBrush(Color.FromArgb(alfa, 255, 255, 255)))
                    g.FillPath(brush, path);
                var barra = new Rectangle(pildora.X, pildora.Y + 10, 4, pildora.Height - 20);
                using (var path = RoundedGeometry.RoundedRect(barra, 2))
                using (var brush = new SolidBrush(Color.FromArgb((int)(255 * _pildoraAlfa), AcentoClaro)))
                    g.FillPath(brush, path);
            }

            // Separador antes de los ítems del final (p. ej. Salir)
            if (_items.Any(i => i.AlFinal))
                using (var pen = new Pen(Color.FromArgb(28, 255, 255, 255)))
                    g.DrawLine(pen, 18, YSeparador, Width - 18, YSeparador);

            foreach (var item in _items)
            {
                if (RectItem(item.Clave) is not { } r) continue;
                bool activo = item.Clave == _activo;

                // Hover: blanco translúcido (hasta ~12 %) con opacidad animada
                float h = _hover[item.Clave];
                if (h > 0.01f)
                    using (var path = RoundedGeometry.RoundedRect(r, 8))
                    using (var brush = new SolidBrush(Color.FromArgb((int)(30 * h), 255, 255, 255)))
                        g.FillPath(brush, path);

                // TextRenderer no respeta transparencia: el blanco atenuado se mezcla con el fondo
                var color = activo ? Color.White : Atenuado(0.72f + 0.28f * h);
                g.DrawImage(Icono(item.Icono, color), r.X + 18, r.Y + (r.Height - TamIcono) / 2, TamIcono, TamIcono);
                TextRenderer.DrawText(g, item.Texto, activo ? FuenteActivo : FuenteItem,
                    new Rectangle(r.X + 18 + TamIcono + 14, r.Y, r.Width - 60, r.Height), color,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
            }
        }

        // Íconos en caché por color (el alfa del hover se agrupa en pasos para no crear demasiados)
        private Bitmap Icono(IconChar icono, Color color)
        {
            int clave = (color.A / 16) << 24 | color.R << 16 | color.G << 8 | color.B;
            if (!_iconos.TryGetValue((icono, clave), out var bmp))
                _iconos[(icono, clave)] = bmp = icono.ToBitmap(Color.FromArgb(color.A / 16 * 16 + 15, color), TamIcono);
            return bmp;
        }

        // Blanco con "opacidad" t sobre el fondo del sidebar, como color opaco
        private Color Atenuado(float t) => Mezclar(BackColor, Color.White, Math.Clamp(t, 0f, 1f));

        private static Color Mezclar(Color a, Color b, float t) => Color.FromArgb(
            (int)(a.R + (b.R - a.R) * t), (int)(a.G + (b.G - a.G) * t), (int)(a.B + (b.B - a.B) * t));

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _timer.Dispose();
                foreach (var b in _iconos.Values) b.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
