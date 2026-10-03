using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PromacoHerra.Controls
{
    // Interruptor on/off con su texto a la derecha ("Préstamos activos" / "Préstamos suspendidos").
    // No cambia solo al hacer click: dispara SolicitudCambio para que el formulario confirme
    // y, si la operación en BD sale bien, asigne Activo.
    public class ToggleSwitch : Control
    {
        private bool _activo = true;
        private string _textoActivo = "Activo";
        private string _textoInactivo = "Inactivo";

        public event EventHandler? SolicitudCambio;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Activo { get => _activo; set { _activo = value; Invalidate(); } }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string TextoActivo { get => _textoActivo; set { _textoActivo = value; Invalidate(); } }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string TextoInactivo { get => _textoInactivo; set { _textoInactivo = value; Invalidate(); } }

        public ToggleSwitch()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            Cursor = Cursors.Hand;
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            Size = new Size(220, 28);
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            if (Enabled) SolicitudCambio?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Parent?.BackColor ?? ThemeManager.CardBackground);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var pista = new Rectangle(0, (Height - 22) / 2, 40, 22);
            using (var path = RoundedGeometry.RoundedRect(pista, 11))
            using (var brush = new SolidBrush(_activo ? Paleta.Verde : Color.FromArgb(203, 208, 216)))
                g.FillPath(brush, path);

            int xPerilla = _activo ? pista.Right - 19 : pista.X + 3;
            using (var brush = new SolidBrush(Color.White))
                g.FillEllipse(brush, xPerilla, pista.Y + 3, 16, 16);

            TextRenderer.DrawText(g, _activo ? _textoActivo : _textoInactivo, Font,
                new Rectangle(pista.Right + 8, 0, Width - pista.Right - 8, Height),
                _activo ? Paleta.Verde : Paleta.Rojo,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }
    }
}
