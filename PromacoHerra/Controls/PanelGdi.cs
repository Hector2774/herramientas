using System.Windows.Forms;

namespace PromacoHerra.Controls
{
    // Panel para dibujo propio con GDI+ (evento Paint): doble buffer para evitar parpadeo
    // y redibujado completo al cambiar de tamaño.
    public class PanelGdi : Panel
    {
        public PanelGdi()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
        }
    }
}
