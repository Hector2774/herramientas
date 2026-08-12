using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PromacoHerra
{
    // Esta clase crea un Panel personalizado con bordes redondeados suavizados.
    public class RoundedPanel : Panel
    {
        // Propiedad para ajustar el radio de la curvatura desde el diseñador
        public int CornerRadius { get; set; } = 20; // Default 20px

        public RoundedPanel()
        {
            this.DoubleBuffered = true; // Evita parpadeos al redibujar
            this.BackColor = ThemeManager.CardBackground; // Usamos el blanco por defecto del tema claro
            this.Padding = new Padding(15); // Padding interno por defecto para que el contenido no toque la curva
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias; // Activar suavizado para bordes perfectos

            // Crear el camino (path) con los bordes redondeados
            Rectangle rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            using (GraphicsPath path = GetRoundedPath(rect, CornerRadius))
            {
                // Pintar el fondo de la tarjeta (blanco corporativo)
                using (SolidBrush brush = new SolidBrush(this.BackColor))
                {
                    e.Graphics.FillPath(brush, path);
                }

                // Opcional: Dibujar un borde muy sutil
                using (Pen borderPen = new Pen(ThemeManager.BorderColor, 1))
                {
                    e.Graphics.DrawPath(borderPen, path);
                }
            }
        }

        // Método auxiliar para generar la forma redondeada
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