using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace PromacoHerra
{
    // Detecta la inactividad del usuario en toda la aplicación: cualquier tecla, clic, rueda o
    // movimiento real del mouse sobre una ventana del sistema cuenta como actividad.
    // FrmPrincipal la consulta para cerrar la sesión si nadie usa el equipo (puesto de bodega
    // compartido: evita que otra persona preste herramientas a nombre de quien dejó la sesión abierta).
    internal sealed class Inactividad : IMessageFilter
    {
        public static readonly TimeSpan Limite = TimeSpan.FromMinutes(15);

        private static readonly Inactividad _filtro = new();
        private static bool _instalado;
        private static long _ultimaActividad = Environment.TickCount64;
        private static Point _ultimaPosicion = Cursor.Position;

        public static TimeSpan TiempoInactivo => TimeSpan.FromMilliseconds(Environment.TickCount64 - _ultimaActividad);

        // Se llama al iniciar sesión: instala el filtro (una sola vez) y reinicia el contador
        public static void Iniciar()
        {
            if (!_instalado)
            {
                Application.AddMessageFilter(_filtro);
                _instalado = true;
            }
            Reiniciar();
        }

        public static void Reiniciar()
        {
            _ultimaActividad = Environment.TickCount64;
            _ultimaPosicion = Cursor.Position;
        }

        private const int WM_KEYDOWN = 0x0100, WM_SYSKEYDOWN = 0x0104;
        private const int WM_MOUSEMOVE = 0x0200, WM_NCMOUSEMOVE = 0x00A0;
        private const int WM_LBUTTONDOWN = 0x0201, WM_RBUTTONDOWN = 0x0204, WM_MBUTTONDOWN = 0x0207, WM_MOUSEWHEEL = 0x020A;
        private const int WM_NCLBUTTONDOWN = 0x00A1, WM_NCRBUTTONDOWN = 0x00A4;

        public bool PreFilterMessage(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_KEYDOWN: case WM_SYSKEYDOWN:
                case WM_LBUTTONDOWN: case WM_RBUTTONDOWN: case WM_MBUTTONDOWN: case WM_MOUSEWHEEL:
                case WM_NCLBUTTONDOWN: case WM_NCRBUTTONDOWN:
                    _ultimaActividad = Environment.TickCount64;
                    break;
                case WM_MOUSEMOVE: case WM_NCMOUSEMOVE:
                    // Windows envía WM_MOUSEMOVE también cuando cambia la ventana bajo el cursor
                    // sin que el mouse se mueva: solo cuenta si la posición cambió
                    var pos = Cursor.Position;
                    if (pos != _ultimaPosicion)
                    {
                        _ultimaPosicion = pos;
                        _ultimaActividad = Environment.TickCount64;
                    }
                    break;
            }
            return false;   // solo observa: el mensaje sigue su curso normal
        }

        // ── Cerrar los diálogos que estén abiertos sobre la ventana principal ──

        // Formularios modales (ShowDialog) y cuadros de MessageBox del hilo de la interfaz.
        // Devuelve true si había alguno (hay que esperar a que terminen de cerrarse).
        public static bool CerrarDialogos()
        {
            bool habia = false;

            foreach (var f in Application.OpenForms.Cast<Form>().Where(f => f.Modal).ToList())
            {
                f.DialogResult = DialogResult.Cancel;
                habia = true;
            }

            // MessageBox no es un Form: se buscan las ventanas de diálogo de Windows (#32770)
            EnumThreadWindows(GetCurrentThreadId(), (hwnd, _) =>
            {
                var clase = new StringBuilder(16);
                GetClassName(hwnd, clase, clase.Capacity);
                if (clase.ToString() == "#32770" && IsWindowVisible(hwnd))
                {
                    PostMessage(hwnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                    habia = true;
                }
                return true;
            }, IntPtr.Zero);

            return habia;
        }

        private const int WM_CLOSE = 0x0010;
        private delegate bool EnumWindowsProc(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32.dll")] private static extern bool EnumThreadWindows(uint threadId, EnumWindowsProc callback, IntPtr lParam);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)] private static extern int GetClassName(IntPtr hwnd, StringBuilder clase, int max);
        [DllImport("user32.dll")] private static extern bool IsWindowVisible(IntPtr hwnd);
        [DllImport("user32.dll")] private static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam);
        [DllImport("kernel32.dll")] private static extern uint GetCurrentThreadId();
    }
}
