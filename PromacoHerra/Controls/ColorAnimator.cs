using System;
using System.Drawing;
using System.Windows.Forms;

namespace PromacoHerra.Controls
{
    // Anima una transición de color suave (no abrupta) e invalida el control dueño en cada paso.
    // Lo comparten MaterialButton (hover), MaterialTextBox/MaterialComboBox (underline en foco)
    // y MaterialCheckBox/MaterialRadioButton (relleno al marcar).
    internal sealed class ColorAnimator : IDisposable
    {
        private const int DurationMs = 150;

        private readonly Control _owner;
        private readonly System.Windows.Forms.Timer _timer;
        private Color _from;
        private Color _to;
        private DateTime _start;

        public Color Current { get; private set; }

        public ColorAnimator(Control owner, Color initial)
        {
            _owner = owner;
            Current = initial;
            _from = initial;
            _to = initial;
            _timer = new System.Windows.Forms.Timer { Interval = 15 };
            _timer.Tick += (s, e) => Step();
        }

        public void AnimateTo(Color target)
        {
            if (Current == target && _to == target) return;
            _from = Current;
            _to = target;
            _start = DateTime.Now;
            if (!_timer.Enabled) _timer.Start();
        }

        private void Step()
        {
            double elapsed = (DateTime.Now - _start).TotalMilliseconds;
            double t = Math.Min(1.0, elapsed / DurationMs);
            Current = Lerp(_from, _to, t);

            if (!_owner.IsDisposed)
                _owner.Invalidate();

            if (t >= 1.0)
                _timer.Stop();
        }

        private static Color Lerp(Color a, Color b, double t)
        {
            int alpha = a.A + (int)((b.A - a.A) * t);
            int r = a.R + (int)((b.R - a.R) * t);
            int g = a.G + (int)((b.G - a.G) * t);
            int bl = a.B + (int)((b.B - a.B) * t);
            return Color.FromArgb(alpha, r, g, bl);
        }

        public void Dispose() => _timer.Dispose();
    }
}
