using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PromacoHerra
{
    public static class ThemeManager
    {
        // ── Paleta PROMACO ─────────────────────────────────────────
        public static Color AppBackground = Color.FromArgb(245, 246, 250);
        public static Color CardBackground = Color.White;
        public static Color SidebarColor = Color.FromArgb(11, 11, 69);
        public static Color SidebarHover = Color.FromArgb(27, 27, 110);
        public static Color AccentRed = Color.FromArgb(211, 18, 18);
        public static Color AccentBlue = Color.FromArgb(0, 86, 179);
        public static Color AccentBlueSoft = Color.FromArgb(224, 235, 255);
        public static Color BorderColor = Color.FromArgb(225, 228, 235);
        public static Color TextPrimary = Color.FromArgb(25, 30, 50);
        public static Color TextSecondary = Color.FromArgb(100, 110, 130);
        public static Color TextOnDark = Color.White;
        public static Color SuccessGreen = Color.FromArgb(22, 163, 74);
        public static Color WarningOrange = Color.FromArgb(217, 119, 6);
        public static Color DangerRed = Color.FromArgb(185, 28, 28);

        // ── Entry point ────────────────────────────────────────────
        public static void ApplyTheme(Form form)
        {
            form.BackColor = AppBackground;
            form.ForeColor = TextPrimary;
            form.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);

            ApplyToControls(form.Controls);
            StylizeTabs(form);
        }

        // ── Recorrido recursivo ────────────────────────────────────
        private static void ApplyToControls(Control.ControlCollection controls)
        {
            foreach (Control ctrl in controls)
            {
                StyleControl(ctrl);
                if (ctrl.HasChildren)
                    ApplyToControls(ctrl.Controls);
            }
        }

        private static void StyleControl(Control ctrl)
        {
            switch (ctrl)
            {
                case Button btn:
                    StyleButton(btn);
                    break;

                case DataGridView dgv:
                    StyleDataGridView(dgv);
                    break;

                case TextBox txt:
                    txt.BackColor = CardBackground;
                    txt.ForeColor = TextPrimary;
                    txt.BorderStyle = BorderStyle.FixedSingle;
                    txt.Font = new Font("Segoe UI", 9.5f);
                    break;

                case ComboBox cmb:
                    cmb.ForeColor = TextPrimary;
                    cmb.BackColor = CardBackground;
                    cmb.FlatStyle = FlatStyle.Flat;
                    cmb.Font = new Font("Segoe UI", 9.5f);
                    break;

                case NumericUpDown nud:
                    nud.BackColor = CardBackground;
                    nud.ForeColor = TextPrimary;
                    nud.Font = new Font("Segoe UI", 9.5f);
                    break;

                case DateTimePicker dtp:
                    dtp.CalendarForeColor = TextPrimary;
                    dtp.CalendarMonthBackground = CardBackground;
                    dtp.Font = new Font("Segoe UI", 9.5f);
                    break;

                case GroupBox gb:
                    // Sin borde visible — solo el título en azul marino
                    gb.ForeColor = AccentBlue;
                    gb.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
                    gb.Paint += GroupBox_Paint;
                    break;

                case Label lbl:
                    bool enSidebar = lbl.Parent?.Dock == DockStyle.Left ||
                                     lbl.Parent?.BackColor == SidebarColor;
                    lbl.ForeColor = enSidebar ? TextOnDark : TextPrimary;
                    break;

                case Panel pnl:
                    if (pnl.Dock == DockStyle.Left)
                        pnl.BackColor = SidebarColor;
                    break;

                case TabControl tc:
                    tc.DrawMode = TabDrawMode.OwnerDrawFixed;
                    tc.DrawItem += TabControl_DrawItem;
                    tc.Appearance = TabAppearance.Normal;
                    tc.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
                    tc.ItemSize = new Size(0, 36);
                    //tc.SizeMode = TabSizeMode.Fixed;
                    tc.Padding = new Point(20, 8);
                    break;
            }
        }

        // ══════════════════════════════════════════════════════════
        // BOTONES — tres variantes automáticas
        // ══════════════════════════════════════════════════════════
        private static void StyleButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.Cursor = Cursors.Hand;
            btn.Font = new Font("Segoe UI", 9f, FontStyle.Bold);

            bool esSidebar = btn.Parent?.Dock == DockStyle.Left ||
                             btn.Parent?.BackColor == SidebarColor;

            void Apply()
            {
                if (esSidebar)
                {
                    // Botones del menú lateral
                    btn.BackColor = btn.Enabled ? SidebarColor : Color.FromArgb(40, 40, 100);
                    btn.ForeColor = TextOnDark;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.FlatAppearance.MouseOverBackColor = SidebarHover;
                    btn.FlatAppearance.MouseDownBackColor = AccentRed;
                }
                else if (btn.Text.Contains("Guardar") || btn.Text.Contains("Abrir") ||
                         btn.Text.Contains("Sincronizar") || btn.Text.Contains("Generar") ||
                         btn.Text.Contains("Registrar"))
                {
                    // Botón primario — azul sólido
                    btn.BackColor = btn.Enabled ? AccentBlue : Color.FromArgb(180, 200, 230);
                    btn.ForeColor = TextOnDark;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 65, 150);
                    btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(0, 50, 120);
                }
                else if (btn.Text.Contains("Eliminar") || btn.Text.Contains("Dar de Baja") ||
                         btn.Text.Contains("Cerrar"))
                {
                    // Botón de peligro — rojo suave
                    btn.BackColor = btn.Enabled ? Color.FromArgb(254, 242, 242) : Color.FromArgb(245, 235, 235);
                    btn.ForeColor = btn.Enabled ? DangerRed : Color.FromArgb(180, 100, 100);
                    btn.FlatAppearance.BorderSize = 1;
                    btn.FlatAppearance.BorderColor = Color.FromArgb(252, 165, 165);
                    btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(254, 226, 226);
                    btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(254, 202, 202);
                }
                else if (btn.Text.Contains("Cancelar") || btn.Text.Contains("Devolver"))
                {
                    // Botón secundario — gris neutro
                    btn.BackColor = btn.Enabled ? Color.FromArgb(248, 249, 250) : Color.FromArgb(230, 232, 236);
                    btn.ForeColor = btn.Enabled ? TextPrimary : TextSecondary;
                    btn.FlatAppearance.BorderSize = 1;
                    btn.FlatAppearance.BorderColor = BorderColor;
                    btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(237, 239, 244);
                    btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(220, 224, 232);
                }
                else
                {
                    // Botón neutro por defecto
                    btn.BackColor = btn.Enabled ? CardBackground : Color.FromArgb(235, 236, 240);
                    btn.ForeColor = btn.Enabled ? TextPrimary : TextSecondary;
                    btn.FlatAppearance.BorderSize = 1;
                    btn.FlatAppearance.BorderColor = BorderColor;
                    btn.FlatAppearance.MouseOverBackColor = AccentBlueSoft;
                    btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(200, 218, 245);
                }

                btn.Cursor = btn.Enabled ? Cursors.Hand : Cursors.Default;
            }

            Apply();
            btn.EnabledChanged += (s, e) => Apply();
        }

        // ══════════════════════════════════════════════════════════
        // GROUPBOX — reemplaza el borde viejo por una línea fina
        // ══════════════════════════════════════════════════════════
        private static void GroupBox_Paint(object sender, PaintEventArgs e)
        {
            var gb = (GroupBox)sender;
            var g = e.Graphics;
            g.Clear(gb.BackColor);

            // Medir el texto del título
            var titleSize = TextRenderer.MeasureText(gb.Text, gb.Font);
            int textLeft = 12;
            int textTop = 0;
            int lineY = titleSize.Height / 2;

            // Línea izquierda — pequeño acento azul
            using (var pen = new Pen(AccentBlue, 3))
            {
                g.DrawLine(pen, textLeft - 10, lineY, textLeft - 4, lineY);
            }

            // Línea completa inferior del título (separador sutil)
            using (var pen = new Pen(BorderColor, 1))
            {
                g.DrawLine(pen,
                    textLeft + titleSize.Width + 6, lineY,
                    gb.Width - 6, lineY);
            }

            // Título
            TextRenderer.DrawText(g, gb.Text, gb.Font,
                new Point(textLeft, textTop),
                AccentBlue,
                TextFormatFlags.Left | TextFormatFlags.Top);
        }

        // ══════════════════════════════════════════════════════════
        // TABCONTROL — pestañas modernas
        // ══════════════════════════════════════════════════════════
        private static void StylizeTabs(Form form)
        {
            foreach (Control ctrl in form.Controls)
                FindAndStyleTabs(ctrl);
        }

        private static void FindAndStyleTabs(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl is TabControl tc)
                {
                    tc.DrawMode = TabDrawMode.OwnerDrawFixed;
                    tc.DrawItem += TabControl_DrawItem;
                    tc.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
                    tc.ItemSize = new Size(140, 36);
                    tc.SizeMode = TabSizeMode.Fixed;
                    tc.Padding = new Point(16, 8);
                    tc.BackColor = AppBackground;

                    // Fondo de cada página de pestaña
                    foreach (TabPage tp in tc.TabPages)
                    {
                        tp.BackColor = AppBackground;
                        tp.Font = new Font("Segoe UI", 9.5f);
                    }
                }
                if (ctrl.HasChildren)
                    FindAndStyleTabs(ctrl);
            }
        }

        private static void TabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            var tc = (TabControl)sender;
            var tab = tc.TabPages[e.Index];
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            bool activa = e.Index == tc.SelectedIndex;
            var bounds = e.Bounds;

            if (activa)
            {
                // Fondo blanco con borde superior azul
                using (var brush = new SolidBrush(CardBackground))
                    g.FillRectangle(brush, bounds);

                // Línea superior azul como indicador activo
                using (var pen = new Pen(AccentBlue, 3))
                    g.DrawLine(pen, bounds.Left, bounds.Top + 1,
                                   bounds.Right, bounds.Top + 1);

                TextRenderer.DrawText(g, tab.Text, new Font("Segoe UI", 9.5f, FontStyle.Bold),
                    bounds, AccentBlue,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
            else
            {
                // Fondo gris suave para inactivas
                using (var brush = new SolidBrush(Color.FromArgb(235, 237, 243)))
                    g.FillRectangle(brush, bounds);

                TextRenderer.DrawText(g, tab.Text, new Font("Segoe UI", 9.5f),
                    bounds, TextSecondary,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
        }

        // ══════════════════════════════════════════════════════════
        // DATAGRIDVIEW — igual que antes pero más pulido
        // ══════════════════════════════════════════════════════════
        public static void StyleDataGridView(DataGridView dgv)
        {
            dgv.EnableHeadersVisualStyles = false;
            dgv.BackgroundColor = AppBackground;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgv.GridColor = BorderColor;
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToResizeRows = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;

            // Headers
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = SidebarColor;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = TextOnDark;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 0, 0, 0);
            dgv.ColumnHeadersHeight = 40;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            // Celdas
            dgv.DefaultCellStyle.BackColor = CardBackground;
            dgv.DefaultCellStyle.ForeColor = TextPrimary;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9.5f);
            dgv.DefaultCellStyle.SelectionBackColor = AccentBlueSoft;
            dgv.DefaultCellStyle.SelectionForeColor = TextPrimary;
            dgv.DefaultCellStyle.Padding = new Padding(10, 0, 10, 0);
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            // Filas alternas
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 253);

            // Altura de filas
            dgv.RowTemplate.Height = 38;

            // Separador horizontal entre filas
            dgv.AdvancedCellBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.Single;
            dgv.AdvancedCellBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            dgv.AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
            dgv.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
            dgv.GridColor = Color.FromArgb(235, 238, 245);

            // Hover effect
            dgv.CellMouseEnter += (s, e) =>
            {
                if (e.RowIndex >= 0)
                    dgv.Rows[e.RowIndex].DefaultCellStyle.BackColor =
                        Color.FromArgb(237, 242, 255);
            };

            dgv.CellMouseLeave += (s, e) =>
            {
                if (e.RowIndex >= 0)
                    dgv.Rows[e.RowIndex].DefaultCellStyle.BackColor =
                        e.RowIndex % 2 == 0
                        ? CardBackground
                        : Color.FromArgb(248, 250, 253);
            };
        }
    }
}