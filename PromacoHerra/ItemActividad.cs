using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using PromacoHerra.Controls;

namespace PromacoHerra
{
    public class ItemActividad : UserControl
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Iniciales { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Titulo { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Subtitulo { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string TextoEstado { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
            Font fontAvatar = new Font("Segoe UI", 12.5f, FontStyle.Bold);
            using (SolidBrush textBrush = new SolidBrush(ThemeManager.SidebarColor))
            {
                StringFormat formatCentro = new StringFormat();
                formatCentro.Alignment = StringAlignment.Center;
                formatCentro.LineAlignment = StringAlignment.Center;
                g.DrawString(Iniciales, fontAvatar, textBrush, rectAvatar, formatCentro);
            }

            // 2. Calcular primero la Etiqueta de Estado (Badge) a la derecha,
            //    para poder reservarle su espacio y que el texto nunca se le monte encima.
            Font fontEstado = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            SizeF sizeTexto = g.MeasureString(TextoEstado, fontEstado);

            int badgeWidth = (int)sizeTexto.Width + 40;
            int badgeHeight = 26;
            int badgeX = this.Width - badgeWidth - 15; // Alineado a la derecha
            int badgeY = 24;

            Rectangle badgeRect = new Rectangle(badgeX, badgeY, badgeWidth, badgeHeight);

            // 3. Dibujar Textos (Título y Subtítulo), recortados para no invadir el badge
            const int textoX = 65;
            const int margenBadge = 12; // separación mínima entre el texto y el badge
            int anchoDisponible = Math.Max(0, badgeX - margenBadge - textoX);

            Font fontTitulo = new Font("Segoe UI", 11.5f, FontStyle.Bold);
            Font fontSub = new Font("Segoe UI", 9.5f, FontStyle.Regular);

            StringFormat formatRecortado = new StringFormat
            {
                Trimming = StringTrimming.EllipsisCharacter,
                FormatFlags = StringFormatFlags.NoWrap
            };

            using (SolidBrush brushTitulo = new SolidBrush(ThemeManager.TextPrimary))     // Texto casi negro
            using (SolidBrush brushSub = new SolidBrush(ThemeManager.TextSecondary))      // Texto gris medio
            {
                g.DrawString(Titulo, fontTitulo, brushTitulo,
                    new RectangleF(textoX, 15, anchoDisponible, fontTitulo.GetHeight(g) + 2), formatRecortado);
                g.DrawString(Subtitulo, fontSub, brushSub,
                    new RectangleF(textoX, 38, anchoDisponible, fontSub.GetHeight(g) + 2), formatRecortado);
            }

            // Usamos el color de estado con una transparencia muy sutil
            using (GraphicsPath path = RoundedGeometry.RoundedRect(badgeRect, 12))
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
    }
}