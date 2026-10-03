using PromacoHerra.Controls;
using PromacoHerra.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace PromacoHerra
{
    public partial class FrmLogin : Form
    {
        private const string NombreApp = "Gestión de Herramientas";
        private static readonly Font FuenteNombreApp = new("Segoe UI Semibold", 15F);

        // Logo en blanco + rojo sobre transparente, generado del logo original (ver LogoParaFondoOscuro)
        private readonly Bitmap? _logo = LogoParaFondoOscuro();

        public FrmLogin()
        {
            InitializeComponent();
            ThemeManager.ApplyTheme(this);
            AplicarEstiloMarca();
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
        }

        // ══════════════════════════════════════════════════════════
        // APARIENCIA — dos paneles con los colores de marca
        // ══════════════════════════════════════════════════════════

        // Después de ApplyTheme: su heurística pinta de sidebar el panel Dock.Left y elige
        // la variante de los botones por su texto.
        private void AplicarEstiloMarca()
        {
            pnlMarca.BackColor = ThemeManager.BrandNavy;
            pnlMarca.Paint += pnlMarca_Paint;

            lblTitulo.ForeColor = ThemeManager.BrandNavy;
            foreach (var lbl in new[] { lblSubtitulo, lblUsuario, lblPassword })
                lbl.ForeColor = ThemeManager.TextSecondary;

            txtUsuario.FocusColor = ThemeManager.BrandNavy;
            txtPassword.FocusColor = ThemeManager.BrandNavy;

            btnLogin.Variant = MaterialButtonVariant.Brand;
            btnSalir.Variant = MaterialButtonVariant.Ghost;

            pnlFormulario.Resize += (s, e) => CentrarCampos();
            CentrarCampos();
        }

        private void CentrarCampos()
        {
            pnlCampos.Location = new Point(
                Math.Max(0, (pnlFormulario.ClientSize.Width - pnlCampos.Width) / 2),
                Math.Max(0, (pnlFormulario.ClientSize.Height - pnlCampos.Height) / 2));
        }

        // Degradado diagonal + bloque centrado: logo, nombre de la app y línea roja
        private void pnlMarca_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            var area = pnlMarca.ClientRectangle;
            if (area.Width <= 0 || area.Height <= 0) return;

            using (var fondo = new LinearGradientBrush(area, ThemeManager.BrandNavyLight, ThemeManager.BrandNavy, 60f))
                g.FillRectangle(fondo, area);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            int anchoLogo = (int)(area.Width * 0.75);
            int altoLogo = _logo == null ? 0 : anchoLogo * _logo.Height / _logo.Width;
            var tamTexto = TextRenderer.MeasureText(g, NombreApp, FuenteNombreApp);
            const int separacionLogo = 36, separacionLinea = 14, altoLinea = 3, anchoLinea = 56;

            int altoBloque = altoLogo + separacionLogo + tamTexto.Height + separacionLinea + altoLinea;
            int y = (area.Height - altoBloque) / 2;

            if (_logo != null)
                g.DrawImage(_logo, (area.Width - anchoLogo) / 2, y, anchoLogo, altoLogo);
            y += altoLogo + separacionLogo;

            TextRenderer.DrawText(g, NombreApp, FuenteNombreApp,
                new Rectangle(0, y, area.Width, tamTexto.Height), ThemeManager.TextOnDark,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.Top);
            y += tamTexto.Height + separacionLinea;

            using (var rojo = new SolidBrush(ThemeManager.BrandRed))
                g.FillRectangle(rojo, (area.Width - anchoLinea) / 2, y, anchoLinea, altoLinea);
        }

        // El logo original es azul marino y rojo sobre fondo blanco opaco: sobre el panel azul el
        // texto desaparece. Se recorta al contenido y se recolorea: azul → blanco, rojo → rojo de
        // marca, blanco → transparente. El alfa sale de cuánto se aleja cada píxel del blanco, así
        // los bordes suavizados (antialias) del original se conservan.
        private static Bitmap? LogoParaFondoOscuro()
        {
            using var stream = typeof(FrmLogin).Assembly.GetManifestResourceStream("PromacoHerra.Recursos.logo.png");
            if (stream == null) return null;

            using var original = new Bitmap(stream);
            using var src = original.Clone(new Rectangle(0, 0, original.Width, original.Height), PixelFormat.Format32bppArgb);

            const int umbralBlanco = 245;
            int w = src.Width, h = src.Height;
            var datos = src.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            var px = new int[w * h];
            Marshal.Copy(datos.Scan0, px, 0, px.Length);
            src.UnlockBits(datos);

            // Recorte al contenido (todo lo que no es casi blanco), con un margen pequeño
            int minX = w, minY = h, maxX = -1, maxY = -1;
            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                {
                    var c = Color.FromArgb(px[y * w + x]);
                    if (Math.Min(c.R, Math.Min(c.G, c.B)) >= umbralBlanco) continue;
                    minX = Math.Min(minX, x); maxX = Math.Max(maxX, x);
                    minY = Math.Min(minY, y); maxY = Math.Max(maxY, y);
                }
            if (maxX < 0) return null;

            const int margen = 4;
            minX = Math.Max(0, minX - margen); minY = Math.Max(0, minY - margen);
            maxX = Math.Min(w - 1, maxX + margen); maxY = Math.Min(h - 1, maxY + margen);
            int cw = maxX - minX + 1, ch = maxY - minY + 1;

            var salida = new int[cw * ch];
            for (int y = 0; y < ch; y++)
                for (int x = 0; x < cw; x++)
                {
                    var c = Color.FromArgb(px[(y + minY) * w + (x + minX)]);
                    int min = Math.Min(c.R, Math.Min(c.G, c.B));
                    bool esRojo = c.R - c.B > 40;

                    // "Tinta" pura: azul (14,0,77) tiene mínimo 0; rojo (227,28,35) tiene mínimo 28
                    int minTinta = esRojo ? ThemeManager.BrandRed.G : 0;
                    int alfa = Math.Clamp((255 - min) * 255 / (255 - minTinta), 0, 255);
                    if (min >= umbralBlanco) alfa = 0;

                    var tinta = esRojo ? ThemeManager.BrandRed : Color.White;
                    salida[y * cw + x] = Color.FromArgb(alfa, tinta).ToArgb();
                }

            var logo = new Bitmap(cw, ch, PixelFormat.Format32bppArgb);
            var destino = logo.LockBits(new Rectangle(0, 0, cw, ch), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            Marshal.Copy(salida, 0, destino.Scan0, salida.Length);
            logo.UnlockBits(destino);
            return logo;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _logo?.Dispose();
            base.OnFormClosed(e);
        }

        private void BtnLogin_Click(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string user = txtUsuario.Text.Trim();
            string pass = txtPassword.Text;   // sin Trim: los espacios forman parte de la contraseña

            if (user == "" || pass == "")
            {
                MessageBox.Show("Ingrese usuario y contraseña");
                return;
            }

            bool correcto;
            Cursor = Cursors.WaitCursor;   // verificar el hash toma una fracción de segundo
            try { correcto = UsuarioService.Login(user, pass); }
            finally { Cursor = Cursors.Default; }

            if (correcto)
            {
                // Contraseña temporal (o que ya no cumple las reglas): cambiarla antes de entrar
                if (Sesion.DebeCambiarPassword)
                {
                    using var cambio = new FrmCambiarPassword(obligatorio: true);
                    if (cambio.ShowDialog(this) != DialogResult.OK)
                    {
                        Sesion.Cerrar();
                        txtPassword.Clear();
                        return;
                    }
                }

                // abrir sistema
                FrmPrincipal frm = new FrmPrincipal();
                frm.Show();

                this.Hide();
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos");
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        

    }
}
