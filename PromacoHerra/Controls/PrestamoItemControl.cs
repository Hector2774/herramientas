using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using PromacoHerra.Models;

namespace PromacoHerra.Controls
{
    // Un préstamo activo en la lista izquierda de Devoluciones. Todo se pinta con GDI+:
    //   [avatar]  Nombre del empleado
    //             Departamento                 N pendientes
    //             [PR-0001] [Vence en N días]
    // Seleccionado: borde izquierdo azul + fondo accent-light. Vencido: borde izquierdo rojo siempre.
    public class PrestamoItemControl : Control
    {
        private const int Alto = 92;
        private const int X0 = 64;

        private static readonly Font FuenteNombre = new("Segoe UI", 10F, FontStyle.Bold);
        private static readonly Font FuenteSecundaria = new("Segoe UI", 9F);
        private static readonly Font FuenteCodigo = new("Consolas", 8.5F);
        private static readonly Font FuenteChip = new("Segoe UI", 8.5F, FontStyle.Bold);
        private static readonly Color CodigoFondo = Color.FromArgb(241, 243, 246);
        private static readonly Color HoverFondo = Color.FromArgb(248, 250, 253);

        private bool _seleccionado;
        private bool _hover;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PrestamoActivo Prestamo { get; }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Seleccionado
        {
            get => _seleccionado;
            set { if (_seleccionado != value) { _seleccionado = value; Invalidate(); } }
        }

        public PrestamoItemControl(PrestamoActivo prestamo)
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            Prestamo = prestamo;
            Height = Alto;
            Margin = new Padding(0);
            Cursor = Cursors.Hand;
        }

        protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            var p = Prestamo;

            g.Clear(_seleccionado ? ThemeManager.AccentBlueSoft : _hover ? HoverFondo : ThemeManager.CardBackground);

            // Borde izquierdo: rojo si está vencido (siempre), azul si está seleccionado
            if (p.Vencido || _seleccionado)
                using (var brush = new SolidBrush(p.Vencido ? ThemeManager.DangerRed : ThemeManager.AccentBlue))
                    g.FillRectangle(brush, 0, 0, 3, Height);

            // Separador inferior
            using (var pen = new Pen(ThemeManager.BorderColor))
                g.DrawLine(pen, 0, Height - 1, Width, Height - 1);

            Avatar.Dibujar(g, new Rectangle(14, 14, 38, 38), p.Empleado, p.ColorIndex);

            int ancho = Width - X0 - 12;
            TextRenderer.DrawText(g, p.Empleado, FuenteNombre, new Rectangle(X0, 12, ancho, 20),
                ThemeManager.TextPrimary, TextFormatFlags.Left | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);

            // Departamento a la izquierda, pendientes a la derecha
            string pendientes = p.Pendientes == 1 ? "1 pendiente" : $"{p.Pendientes} pendientes";
            var tamPend = TextRenderer.MeasureText(g, pendientes, FuenteSecundaria, Size.Empty, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(g, pendientes, FuenteSecundaria,
                new Rectangle(Width - 12 - tamPend.Width, 34, tamPend.Width, 18),
                ThemeManager.TextSecondary, TextFormatFlags.Right | TextFormatFlags.NoPadding);
            TextRenderer.DrawText(g, string.IsNullOrEmpty(p.Departamento) ? "Sin departamento" : p.Departamento,
                FuenteSecundaria, new Rectangle(X0, 34, ancho - tamPend.Width - 8, 18),
                ThemeManager.TextSecondary, TextFormatFlags.Left | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);

            // Código del préstamo (monospace, fondo gris) + chip de plazo
            var tamCod = TextRenderer.MeasureText(g, p.Codigo, FuenteCodigo, Size.Empty, TextFormatFlags.NoPadding);
            var rectCod = new Rectangle(X0, 58, tamCod.Width + 12, tamCod.Height + 6);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (var path = RoundedGeometry.RoundedRect(rectCod, 4))
            using (var brush = new SolidBrush(CodigoFondo))
                g.FillPath(brush, path);
            TextRenderer.DrawText(g, p.Codigo, FuenteCodigo, rectCod, ThemeManager.TextPrimary,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);

            Plazo.DibujarChip(g, new Point(rectCod.Right + 6, 57), p.DiasRestantes, FuenteChip);
        }
    }
}
