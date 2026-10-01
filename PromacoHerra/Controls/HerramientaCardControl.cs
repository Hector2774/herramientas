using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using PromacoHerra.Models;

namespace PromacoHerra.Controls
{
    // Tarjeta del catálogo de préstamos: un tipo de herramienta con su stock disponible
    // y un stepper (− cantidad +) para elegir cuántas unidades prestar.
    // La tarjeta solo muestra; la lógica de qué unidades se asignan vive en FrmPrestamo,
    // que escucha MasClick / MenosClick / VerUnidadesClick y luego actualiza Cantidad.
    public partial class HerramientaCardControl : UserControl
    {
        private const int ShadowSize = 4;
        private const int CornerRadius = 10;
        private const int ImagenAltura = 80;
        private const int DivisorY = 190;

        private static readonly Color ImagenFondo = Color.FromArgb(243, 244, 246);
        private static readonly Color TagGrisFondo = Color.FromArgb(243, 244, 246);

        private readonly ToolTip _tip = new();
        private int _cantidad;
        private Image? _imagen;

        public event EventHandler? MasClick;
        public event EventHandler? MenosClick;
        public event EventHandler? VerUnidadesClick;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HerramientaCatalogoItem? Item { get; private set; }

        // Unidades seleccionadas de esta herramienta
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Cantidad
        {
            get => _cantidad;
            set { _cantidad = value; ActualizarVista(); }
        }

        // Reservado para la foto de la herramienta; null = placeholder "Sin imagen"
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Image? Imagen
        {
            get => _imagen;
            set { _imagen = value; Invalidate(); }
        }

        public HerramientaCardControl()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint |
                      ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            InitializeComponent();

            lblNombre.ForeColor = ThemeManager.TextPrimary;
            lblCategoria.BackColor = ThemeManager.AccentBlueSoft;
            lblCategoria.ForeColor = ThemeManager.AccentBlue;
            lblMarca.BackColor = TagGrisFondo;
            lblMarca.ForeColor = ThemeManager.TextSecondary;
            lblStock.ForeColor = ThemeManager.TextSecondary;
            lblCantidad.ForeColor = ThemeManager.TextPrimary;
            lnkUnidades.LinkColor = ThemeManager.AccentBlue;
            lnkUnidades.ActiveLinkColor = ThemeManager.AccentBlue;

            btnMas.Click += (s, e) => MasClick?.Invoke(this, EventArgs.Empty);
            btnMenos.Click += (s, e) => MenosClick?.Invoke(this, EventArgs.Empty);
            lnkUnidades.LinkClicked += (s, e) => VerUnidadesClick?.Invoke(this, EventArgs.Empty);
        }

        public void Cargar(HerramientaCatalogoItem item)
        {
            Item = item;
            lblNombre.Text = item.Nombre;

            lblCategoria.Text = string.IsNullOrEmpty(item.Categoria) ? "Sin categoría" : item.Categoria;
            lblMarca.Text = item.Marca;
            lblMarca.Visible = !string.IsNullOrEmpty(item.Marca);

            _tip.SetToolTip(lblNombre, $"{item.Codigo} — {item.Nombre}");

            _cantidad = 0;
            ActualizarVista();
        }

        private void ActualizarVista()
        {
            int disponibles = Item?.Disponibles ?? 0;
            int restantes = disponibles - _cantidad;
            bool seleccionada = _cantidad > 0;

            lblCantidad.Text = _cantidad.ToString();
            lblCantidad.ForeColor = seleccionada ? ThemeManager.AccentBlue : ThemeManager.TextSecondary;

            // Stock restante: verde (3+), naranja (1-2), rojo (sin unidades)
            lblStockDot.ForeColor = restantes >= 3 ? ThemeManager.SuccessGreen
                                  : restantes > 0 ? ThemeManager.WarningOrange
                                  : ThemeManager.DangerRed;
            lblStock.Text = restantes switch
            {
                0 when disponibles == 0 => "Sin disponibles",
                0 => "Todas seleccionadas",
                1 => "1 disponible",
                _ => $"{restantes} disponibles"
            };

            btnMenos.Enabled = seleccionada;
            btnMas.Enabled = restantes > 0;
            btnMenos.Variant = seleccionada ? MaterialButtonVariant.Primary : MaterialButtonVariant.Default;
            btnMas.Variant = seleccionada ? MaterialButtonVariant.Primary : MaterialButtonVariant.Default;
            lnkUnidades.Visible = seleccionada;

            Invalidate();
        }

        // ── Pintado: sombra + tarjeta redondeada + área de imagen + badge ──

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Fuera de la tarjeta se ve el fondo del contenedor (la sombra se pinta encima)
            e.Graphics.Clear(Parent?.BackColor ?? ThemeManager.AppBackground);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var cardRect = new Rectangle(0, 0, Width - ShadowSize - 1, Height - ShadowSize - 1);

            // Sombra ligera: capas concéntricas semitransparentes desplazadas abajo/derecha
            for (int i = ShadowSize; i >= 1; i--)
            {
                using var ringPath = RoundedGeometry.RoundedRect(
                    new Rectangle(i, i, cardRect.Width, cardRect.Height), CornerRadius);
                int alpha = (int)(14 * ((ShadowSize - i + 1) / (float)ShadowSize));
                using var ringBrush = new SolidBrush(Color.FromArgb(alpha, 15, 23, 42));
                g.FillPath(ringBrush, ringPath);
            }

            using var cardPath = RoundedGeometry.RoundedRect(cardRect, CornerRadius);
            using (var brush = new SolidBrush(BackColor))
                g.FillPath(brush, cardPath);

            // Área de imagen (recortada a las esquinas superiores redondeadas)
            var imagenRect = new Rectangle(cardRect.X, cardRect.Y, cardRect.Width, ImagenAltura);
            var clipAnterior = g.Clip;
            g.SetClip(cardPath, CombineMode.Intersect);
            if (_imagen != null)
            {
                g.DrawImage(_imagen, imagenRect);
            }
            else
            {
                using (var brush = new SolidBrush(ImagenFondo))
                    g.FillRectangle(brush, imagenRect);
                using var fuente = new Font("Segoe UI", 8.5F);
                TextRenderer.DrawText(g, "Sin imagen", fuente, imagenRect, ThemeManager.TextSecondary,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
            g.Clip = clipAnterior;

            // Divisor sobre el stepper
            using (var pen = new Pen(ThemeManager.BorderColor, 1))
                g.DrawLine(pen, cardRect.X + 12, DivisorY, cardRect.Right - 12, DivisorY);

            // Borde: azul si hay unidades seleccionadas
            using (var pen = new Pen(_cantidad > 0 ? ThemeManager.AccentBlue : ThemeManager.BorderColor,
                                     _cantidad > 0 ? 2 : 1))
                g.DrawPath(pen, cardPath);

            // Badge circular con la cantidad seleccionada, esquina superior derecha de la imagen
            if (_cantidad > 0)
            {
                const int d = 26;
                var badge = new Rectangle(cardRect.Right - d - 8, cardRect.Y + 8, d, d);
                using (var brush = new SolidBrush(ThemeManager.AccentBlue))
                    g.FillEllipse(brush, badge);
                using var fuente = new Font("Segoe UI", 9F, FontStyle.Bold);
                TextRenderer.DrawText(g, _cantidad.ToString(), fuente, badge, Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
            }

            base.OnPaint(e);
        }
    }
}
