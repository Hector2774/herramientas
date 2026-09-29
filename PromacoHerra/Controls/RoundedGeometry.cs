using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace PromacoHerra.Controls
{
    // Helper compartido para recortar esquinas redondeadas reales (GraphicsPath + Region)
    // en vez de solo dibujarlas. Usado por todos los controles Material y por RoundedPanel/ItemActividad.
    public static class RoundedGeometry
    {
        public static GraphicsPath RoundedRect(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();

            if (radius <= 0 || rect.Width <= 0 || rect.Height <= 0)
            {
                path.AddRectangle(rect);
                return path;
            }

            int d = radius * 2;
            d = Math.Min(d, Math.Min(rect.Width, rect.Height));

            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
