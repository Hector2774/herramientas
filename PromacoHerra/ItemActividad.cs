using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PromacoHerra
{
    public class ItemActividad : UserControl
    {
        public string Iniciales { get; set; }
        public string Titulo { get; set; }
        public string Subtitulo { get; set; }
        public string TextoEstado { get; set; }
        public Color ColorEstado { get; set; }

        public ItemActividad()
        {
            this.Size = new Size(395, 75); // Tamaño de cada fila

            // Usamos el fondo blanco de las tarjetas del nuevo tema
            this.BackColor = ThemeManager.CardBackground;

            this.Margin = new Padding(0, 0, 0, 5); // Espacio entre elementos
            this.DoubleBuffered = true; // Evita parpadeos al dibujar
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias; // Bordes suaves

            // Opcional: Dibujar un borde muy sutil alrededor de la tarjeta
            using (Pen borderPen = new Pen(ThemeManager.BorderColor, 1))
            {
                g.DrawRectangle(borderPen, 0, 0, this.Width - 1, this.Height - 1);
            }

            // 1. Dibujar el círculo del Avatar (Fondo gris clarito)
            Rectangle rectAvatar = new Rectangle(10, 15, 42, 42);
            using (SolidBrush brushAvatar = new SolidBrush(ThemeManager.AppBackground))
            {
                g.FillEllipse(brushAvatar, rectAvatar);
            }

            // Dibujar iniciales centradas en el círculo (Texto en Azul Marino corporativo)
            Font fontAvatar = new Font("Segoe UI", 11, FontStyle.Bold);
            using (SolidBrush textBrush = new SolidBrush(ThemeManager.SidebarColor))
            {
                StringFormat formatCentro = new StringFormat();
                formatCentro.Alignment = StringAlignment.Center;
                formatCentro.LineAlignment = StringAlignment.Center;
                g.DrawString(Iniciales, fontAvatar, textBrush, rectAvatar, formatCentro);
            }

            // 2. Dibujar Textos (Título y Subtítulo)
            Font fontTitulo = new Font("Segoe UI", 10, FontStyle.Bold);
            Font fontSub = new Font("Segoe UI", 8.5f, FontStyle.Regular);

            using (SolidBrush brushTitulo = new SolidBrush(ThemeManager.TextPrimary))     // Texto casi negro
            using (SolidBrush brushSub = new SolidBrush(ThemeManager.TextSecondary))      // Texto gris medio
            {
                g.DrawString(Titulo, fontTitulo, brushTitulo, 65, 15);
                g.DrawString(Subtitulo, fontSub, brushSub, 65, 38);
            }

            // 3. Dibujar la Etiqueta de Estado (Badge) a la derecha
            Font fontEstado = new Font("Segoe UI", 8.5f, FontStyle.Bold);
            SizeF sizeTexto = g.MeasureString(TextoEstado, fontEstado);

            int badgeWidth = (int)sizeTexto.Width + 40;
            int badgeHeight = 26;
            int badgeX = this.Width - badgeWidth - 15; // Alineado a la derecha
            int badgeY = 24;

            Rectangle badgeRect = new Rectangle(badgeX, badgeY, badgeWidth, badgeHeight);

            // Usamos el color de estado con una transparencia muy sutil
            using (GraphicsPath path = GetRoundedPath(badgeRect, 12))
            using (SolidBrush badgeBg = new SolidBrush(Color.FromArgb(25, ColorEstado))) // 25 de opacidad sobre fondo blanco se ve genial
            using (Pen badgePen = new Pen(ColorEstado, 1.5f))
            using (SolidBrush textEstadoBrush = new SolidBrush(ColorEstado))
            {
                g.FillPath(badgeBg, path);
                g.DrawPath(badgePen, path);

                StringFormat formatCentro = new StringFormat();
                formatCentro.Alignment = StringAlignment.Center;
                formatCentro.LineAlignment = StringAlignment.Center;
                g.DrawString(TextoEstado, fontEstado, textEstadoBrush, badgeRect, formatCentro);
            }
        }

        // Método auxiliar para dibujar rectángulos con bordes redondeados
        private GraphicsPath GetRoundedPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int d = radius * 2;
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}