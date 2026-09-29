namespace PromacoHerra
{
    partial class FrmDashboard
    {
        private System.ComponentModel.IContainer components = null;

        // Top
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblSaludo;
        private System.Windows.Forms.Label lblFecha;

        // Cards
        private System.Windows.Forms.FlowLayoutPanel panelCards;
        private PromacoHerra.RoundedPanel cardAtrasos;
        private PromacoHerra.RoundedPanel cardDisponibles;
        private PromacoHerra.RoundedPanel cardActivos;
        private PromacoHerra.RoundedPanel cardDanadas;
        private PromacoHerra.RoundedPanel cardMantenimiento;
        private System.Windows.Forms.Label lblAtrasos;
        private System.Windows.Forms.Label lblDisponibles;
        private System.Windows.Forms.Label lblActivos;
        private System.Windows.Forms.Label lblDanadas;
        private System.Windows.Forms.Label lblMantenimiento;

        // Contenido medio (alertas + actividad)
        private System.Windows.Forms.Panel panelContenido;
        private PromacoHerra.RoundedPanel panelAlertas;
        private PromacoHerra.RoundedPanel panelActividad;
        private System.Windows.Forms.ListBox lstAlertas;
        private System.Windows.Forms.ListBox lstActividad;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;

        // Bottom (gráfica + ranking)
        private System.Windows.Forms.Panel panelBottom;
        private PromacoHerra.RoundedPanel panelGrafica;
        private PromacoHerra.RoundedPanel panelRanking;
        private System.Windows.Forms.Panel panelGraficaHeader;
        private System.Windows.Forms.Label lblGraficaTitulo;
        private System.Windows.Forms.Panel panelRankingHeader;
        private System.Windows.Forms.Label lblRankingTitulo;
        private System.Windows.Forms.DataGridView dgvRanking;

        // Inventario (mantenido para compatibilidad, se usa internamente)
        private System.Windows.Forms.Panel panelInventario;
        private System.Windows.Forms.DataGridView dgvInventario;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label4;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            panelTop = new System.Windows.Forms.Panel();
            lblSaludo = new System.Windows.Forms.Label();
            lblFecha = new System.Windows.Forms.Label();
            panelCards = new System.Windows.Forms.FlowLayoutPanel();
            cardAtrasos = new PromacoHerra.RoundedPanel();
            lblAtrasos = new System.Windows.Forms.Label();
            cardDisponibles = new PromacoHerra.RoundedPanel();
            lblDisponibles = new System.Windows.Forms.Label();
            cardActivos = new PromacoHerra.RoundedPanel();
            lblActivos = new System.Windows.Forms.Label();
            cardDanadas = new PromacoHerra.RoundedPanel();
            lblDanadas = new System.Windows.Forms.Label();
            cardMantenimiento = new PromacoHerra.RoundedPanel();
            lblMantenimiento = new System.Windows.Forms.Label();
            panelContenido = new System.Windows.Forms.Panel();
            panelActividad = new PromacoHerra.RoundedPanel();
            panel2 = new System.Windows.Forms.Panel();
            label2 = new System.Windows.Forms.Label();
            lstActividad = new System.Windows.Forms.ListBox();
            panelAlertas = new PromacoHerra.RoundedPanel();
            panel1 = new System.Windows.Forms.Panel();
            label1 = new System.Windows.Forms.Label();
            lstAlertas = new System.Windows.Forms.ListBox();
            panelBottom = new System.Windows.Forms.Panel();
            panelGrafica = new PromacoHerra.RoundedPanel();
            panelGraficaHeader = new System.Windows.Forms.Panel();
            lblGraficaTitulo = new System.Windows.Forms.Label();
            panelRanking = new PromacoHerra.RoundedPanel();
            panelRankingHeader = new System.Windows.Forms.Panel();
            lblRankingTitulo = new System.Windows.Forms.Label();
            dgvRanking = new System.Windows.Forms.DataGridView();
            // compatibilidad
            panelInventario = new System.Windows.Forms.Panel();
            dgvInventario = new System.Windows.Forms.DataGridView();
            panel3 = new System.Windows.Forms.Panel();
            label3 = new System.Windows.Forms.Label();
            panel4 = new System.Windows.Forms.Panel();
            label4 = new System.Windows.Forms.Label();

            panelTop.SuspendLayout();
            panelCards.SuspendLayout();
            panelContenido.SuspendLayout();
            panelBottom.SuspendLayout();
            panelGrafica.SuspendLayout();
            panelRanking.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRanking).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvInventario).BeginInit();
            SuspendLayout();

            // ── panelTop ───────────────────────────────────────────
            panelTop.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            panelTop.Controls.Add(lblSaludo);
            panelTop.Controls.Add(lblFecha);
            panelTop.Location = new System.Drawing.Point(20, 20);
            panelTop.Name = "panelTop";
            panelTop.Size = new System.Drawing.Size(1220, 55);

            lblSaludo.AutoSize = true;
            lblSaludo.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            lblSaludo.Location = new System.Drawing.Point(0, 5);
            lblSaludo.Name = "lblSaludo";
            lblSaludo.Text = "Buen día, Admin";

            lblFecha.AutoSize = true;
            lblFecha.Font = new System.Drawing.Font("Segoe UI", 11.5F);
            lblFecha.Location = new System.Drawing.Point(2, 38);
            lblFecha.Name = "lblFecha";
            lblFecha.ForeColor = System.Drawing.Color.FromArgb(100, 110, 130);
            lblFecha.Text = "lunes, 1 de enero de 2026";

            // ── panelCards ─────────────────────────────────────────
            panelCards.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            panelCards.Controls.Add(cardAtrasos);
            panelCards.Controls.Add(cardDisponibles);
            panelCards.Controls.Add(cardActivos);
            panelCards.Controls.Add(cardDanadas);
            panelCards.Controls.Add(cardMantenimiento);
            panelCards.Location = new System.Drawing.Point(20, 85);
            panelCards.Name = "panelCards";
            panelCards.Size = new System.Drawing.Size(1220, 128);
            panelCards.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            panelCards.BackColor = System.Drawing.Color.Transparent;

            BuildCard(cardAtrasos, lblAtrasos, "🔴", "Vencidos", System.Drawing.Color.FromArgb(185, 28, 28), System.Drawing.Color.FromArgb(254, 242, 242));
            BuildCard(cardDisponibles, lblDisponibles, "🟢", "Disponibles", System.Drawing.Color.FromArgb(22, 163, 74), System.Drawing.Color.FromArgb(240, 253, 244));
            BuildCard(cardActivos, lblActivos, "🔵", "Préstamos activos", System.Drawing.Color.FromArgb(0, 86, 179), System.Drawing.Color.FromArgb(239, 246, 255));
            BuildCard(cardDanadas, lblDanadas, "🟡", "Herramientas dañadas", System.Drawing.Color.FromArgb(180, 110, 0), System.Drawing.Color.FromArgb(255, 251, 235));
            BuildCard(cardMantenimiento, lblMantenimiento, "🔧", "En mantenimiento", System.Drawing.Color.FromArgb(109, 40, 217), System.Drawing.Color.FromArgb(245, 243, 255));

            // ── panelContenido (alertas + actividad) ───────────────
            // Alto fijo: las listas de adentro ya tienen scroll propio (AutoScroll),
            // así que no necesitan crecer en alto — solo a lo ancho.
            panelContenido.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            panelContenido.Controls.Add(panelAlertas);
            panelContenido.Controls.Add(panelActividad);
            panelContenido.Location = new System.Drawing.Point(20, 223);
            panelContenido.Name = "panelContenido";
            panelContenido.Size = new System.Drawing.Size(1220, 320);

            // panelAlertas
            panelAlertas.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            panelAlertas.BackColor = System.Drawing.Color.White;
            panelAlertas.CornerRadius = ThemeManager.CornerRadiusCard;
            panelAlertas.Controls.Add(panel1);
            panelAlertas.Controls.Add(lstAlertas);
            panelAlertas.Location = new System.Drawing.Point(0, 0);
            panelAlertas.Name = "panelAlertas";
            panelAlertas.Size = new System.Drawing.Size(595, 320);

            panel1.BackColor = System.Drawing.Color.White;
            panel1.Controls.Add(label1);
            panel1.Dock = System.Windows.Forms.DockStyle.Top;
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(595, 44);

            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Segoe UI", 12.5F, System.Drawing.FontStyle.Bold);
            label1.Location = new System.Drawing.Point(16, 12);
            label1.Name = "label1";
            label1.Text = "⚠  Alertas Principales";

            lstAlertas.Location = new System.Drawing.Point(0, 44);
            lstAlertas.Name = "lstAlertas";
            lstAlertas.Size = new System.Drawing.Size(595, 272);

            // panelActividad
            panelActividad.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            panelActividad.BackColor = System.Drawing.Color.White;
            panelActividad.CornerRadius = ThemeManager.CornerRadiusCard;
            panelActividad.Controls.Add(panel2);
            panelActividad.Controls.Add(lstActividad);
            panelActividad.Location = new System.Drawing.Point(615, 0);
            panelActividad.Name = "panelActividad";
            panelActividad.Size = new System.Drawing.Size(605, 320);

            panel2.BackColor = System.Drawing.Color.White;
            panel2.Controls.Add(label2);
            panel2.Dock = System.Windows.Forms.DockStyle.Top;
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(605, 44);

            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Segoe UI", 12.5F, System.Drawing.FontStyle.Bold);
            label2.Location = new System.Drawing.Point(16, 12);
            label2.Name = "label2";
            label2.Text = "🕐  Actividad Reciente";

            lstActividad.Location = new System.Drawing.Point(0, 44);
            lstActividad.Name = "lstActividad";
            lstActividad.Size = new System.Drawing.Size(605, 272);

            // ── panelBottom (gráfica + ranking) ────────────────────
            // Es la sección que absorbe el alto extra del formulario (panelContenido de
            // arriba se queda con su alto fijo, ver comentario ahí).
            panelBottom.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            panelBottom.Controls.Add(panelGrafica);
            panelBottom.Controls.Add(panelRanking);
            panelBottom.Location = new System.Drawing.Point(20, 558);
            panelBottom.Name = "panelBottom";
            panelBottom.Size = new System.Drawing.Size(1220, 360);

            // panelGrafica
            panelGrafica.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            panelGrafica.BackColor = System.Drawing.Color.White;
            panelGrafica.CornerRadius = ThemeManager.CornerRadiusCard;
            panelGrafica.Controls.Add(panelGraficaHeader);
            panelGrafica.Location = new System.Drawing.Point(0, 0);
            panelGrafica.Name = "panelGrafica";
            panelGrafica.Size = new System.Drawing.Size(595, 360);

            panelGraficaHeader.BackColor = System.Drawing.Color.White;
            panelGraficaHeader.Controls.Add(lblGraficaTitulo);
            panelGraficaHeader.Dock = System.Windows.Forms.DockStyle.Top;
            panelGraficaHeader.Name = "panelGraficaHeader";
            panelGraficaHeader.Size = new System.Drawing.Size(595, 44);

            lblGraficaTitulo.AutoSize = true;
            lblGraficaTitulo.Font = new System.Drawing.Font("Segoe UI", 12.5F, System.Drawing.FontStyle.Bold);
            lblGraficaTitulo.Location = new System.Drawing.Point(16, 12);
            lblGraficaTitulo.Name = "lblGraficaTitulo";
            lblGraficaTitulo.Text = "📊  Estado del inventario";

            // panelRanking
            panelRanking.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            panelRanking.BackColor = System.Drawing.Color.White;
            panelRanking.CornerRadius = ThemeManager.CornerRadiusCard;
            panelRanking.Controls.Add(panelRankingHeader);
            panelRanking.Controls.Add(dgvRanking);
            panelRanking.Location = new System.Drawing.Point(615, 0);
            panelRanking.Name = "panelRanking";
            panelRanking.Size = new System.Drawing.Size(605, 360);

            panelRankingHeader.BackColor = System.Drawing.Color.White;
            panelRankingHeader.Controls.Add(lblRankingTitulo);
            panelRankingHeader.Dock = System.Windows.Forms.DockStyle.Top;
            panelRankingHeader.Name = "panelRankingHeader";
            panelRankingHeader.Size = new System.Drawing.Size(605, 44);

            lblRankingTitulo.AutoSize = true;
            lblRankingTitulo.Font = new System.Drawing.Font("Segoe UI", 12.5F, System.Drawing.FontStyle.Bold);
            lblRankingTitulo.Location = new System.Drawing.Point(16, 12);
            lblRankingTitulo.Name = "lblRankingTitulo";
            lblRankingTitulo.Text = "🏆  Empleados con más préstamos";

            dgvRanking.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            dgvRanking.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvRanking.Location = new System.Drawing.Point(10, 52);
            dgvRanking.Name = "dgvRanking";
            dgvRanking.Size = new System.Drawing.Size(585, 298);
            dgvRanking.TabIndex = 0;

            // compatibilidad — ocultos
            panelInventario.Visible = false;
            dgvInventario.Name = "dgvInventario";
            panel3.Name = "panel3";
            label3.Name = "label3";
            panel4.Name = "panel4";
            label4.Name = "label4";

            // ── Form ───────────────────────────────────────────────
            ClientSize = new System.Drawing.Size(1273, 953);
            Controls.Add(panelTop);
            Controls.Add(panelCards);
            Controls.Add(panelContenido);
            Controls.Add(panelBottom);
            Controls.Add(panelInventario);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FrmDashboard";
            Padding = new System.Windows.Forms.Padding(20);
            Text = "Dashboard";

            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            panelCards.ResumeLayout(false);
            panelContenido.ResumeLayout(false);
            panelBottom.ResumeLayout(false);
            panelGrafica.ResumeLayout(false);
            panelRanking.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvRanking).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvInventario).EndInit();
            ResumeLayout(false);
        }

        // ── Helpers del Designer ───────────────────────────────────
        private static void BuildCard(
            PromacoHerra.RoundedPanel panel,
            System.Windows.Forms.Label lbl,
            string emoji,
            string titulo,
            System.Drawing.Color colorAccent,
            System.Drawing.Color colorFondo)
        {
            lbl.Dock = System.Windows.Forms.DockStyle.Fill;
            lbl.Font = new System.Drawing.Font("Segoe UI", 12.5F);
            lbl.ForeColor = colorAccent;
            lbl.Text = $"{emoji}\n{titulo}\n—";
            lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lbl.Name = lbl.Name;

            panel.BackColor = colorFondo;
            panel.CornerRadius = ThemeManager.CornerRadiusCard;
            panel.Controls.Add(lbl);
            panel.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            panel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            panel.Size = new System.Drawing.Size(223, 112);
            panel.Paint += (s, e) =>
            {
                // Línea izquierda de acento, recortada para no invadir las esquinas curvas
                // (el borde y la sombra ya los dibuja RoundedPanel).
                int r = panel.CornerRadius;
                using var pen = new System.Drawing.Pen(colorAccent, 4);
                e.Graphics.DrawLine(pen, 2, r + 4, 2, panel.Height - r - 4);
            };
        }
    }
}