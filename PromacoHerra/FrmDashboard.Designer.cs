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
        private System.Windows.Forms.Panel cardAtrasos;
        private System.Windows.Forms.Panel cardDisponibles;
        private System.Windows.Forms.Panel cardActivos;
        private System.Windows.Forms.Panel cardDanadas;
        private System.Windows.Forms.Panel cardMantenimiento;
        private System.Windows.Forms.Label lblAtrasos;
        private System.Windows.Forms.Label lblDisponibles;
        private System.Windows.Forms.Label lblActivos;
        private System.Windows.Forms.Label lblDanadas;
        private System.Windows.Forms.Label lblMantenimiento;

        // Contenido medio (alertas + actividad)
        private System.Windows.Forms.Panel panelContenido;
        private System.Windows.Forms.Panel panelAlertas;
        private System.Windows.Forms.Panel panelActividad;
        private System.Windows.Forms.ListBox lstAlertas;
        private System.Windows.Forms.ListBox lstActividad;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;

        // Bottom (gráfica + ranking)
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Panel panelGrafica;
        private System.Windows.Forms.Panel panelRanking;
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
            cardAtrasos = new System.Windows.Forms.Panel();
            lblAtrasos = new System.Windows.Forms.Label();
            cardDisponibles = new System.Windows.Forms.Panel();
            lblDisponibles = new System.Windows.Forms.Label();
            cardActivos = new System.Windows.Forms.Panel();
            lblActivos = new System.Windows.Forms.Label();
            cardDanadas = new System.Windows.Forms.Panel();
            lblDanadas = new System.Windows.Forms.Label();
            cardMantenimiento = new System.Windows.Forms.Panel();
            lblMantenimiento = new System.Windows.Forms.Label();
            panelContenido = new System.Windows.Forms.Panel();
            panelActividad = new System.Windows.Forms.Panel();
            panel2 = new System.Windows.Forms.Panel();
            label2 = new System.Windows.Forms.Label();
            lstActividad = new System.Windows.Forms.ListBox();
            panelAlertas = new System.Windows.Forms.Panel();
            panel1 = new System.Windows.Forms.Panel();
            label1 = new System.Windows.Forms.Label();
            lstAlertas = new System.Windows.Forms.ListBox();
            panelBottom = new System.Windows.Forms.Panel();
            panelGrafica = new System.Windows.Forms.Panel();
            panelGraficaHeader = new System.Windows.Forms.Panel();
            lblGraficaTitulo = new System.Windows.Forms.Label();
            panelRanking = new System.Windows.Forms.Panel();
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
            panelTop.Controls.Add(lblSaludo);
            panelTop.Controls.Add(lblFecha);
            panelTop.Location = new System.Drawing.Point(20, 20);
            panelTop.Name = "panelTop";
            panelTop.Size = new System.Drawing.Size(1220, 55);

            lblSaludo.AutoSize = true;
            lblSaludo.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            lblSaludo.Location = new System.Drawing.Point(0, 5);
            lblSaludo.Name = "lblSaludo";
            lblSaludo.Text = "Buen día, Admin";

            lblFecha.AutoSize = true;
            lblFecha.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblFecha.Location = new System.Drawing.Point(2, 38);
            lblFecha.Name = "lblFecha";
            lblFecha.ForeColor = System.Drawing.Color.FromArgb(100, 110, 130);
            lblFecha.Text = "lunes, 1 de enero de 2026";

            // ── panelCards ─────────────────────────────────────────
            panelCards.Controls.Add(cardAtrasos);
            panelCards.Controls.Add(cardDisponibles);
            panelCards.Controls.Add(cardActivos);
            panelCards.Controls.Add(cardDanadas);
            panelCards.Controls.Add(cardMantenimiento);
            panelCards.Location = new System.Drawing.Point(20, 85);
            panelCards.Name = "panelCards";
            panelCards.Size = new System.Drawing.Size(1220, 115);
            panelCards.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            panelCards.BackColor = System.Drawing.Color.Transparent;

            BuildCard(cardAtrasos, lblAtrasos, "🔴", "Vencidos", System.Drawing.Color.FromArgb(185, 28, 28), System.Drawing.Color.FromArgb(254, 242, 242));
            BuildCard(cardDisponibles, lblDisponibles, "🟢", "Disponibles", System.Drawing.Color.FromArgb(22, 163, 74), System.Drawing.Color.FromArgb(240, 253, 244));
            BuildCard(cardActivos, lblActivos, "🔵", "Préstamos activos", System.Drawing.Color.FromArgb(0, 86, 179), System.Drawing.Color.FromArgb(239, 246, 255));
            BuildCard(cardDanadas, lblDanadas, "🟡", "Herramientas dañadas", System.Drawing.Color.FromArgb(180, 110, 0), System.Drawing.Color.FromArgb(255, 251, 235));
            BuildCard(cardMantenimiento, lblMantenimiento, "🔧", "En mantenimiento", System.Drawing.Color.FromArgb(109, 40, 217), System.Drawing.Color.FromArgb(245, 243, 255));

            // ── panelContenido (alertas + actividad) ───────────────
            panelContenido.Controls.Add(panelAlertas);
            panelContenido.Controls.Add(panelActividad);
            panelContenido.Location = new System.Drawing.Point(20, 210);
            panelContenido.Name = "panelContenido";
            panelContenido.Size = new System.Drawing.Size(1220, 320);

            // panelAlertas
            panelAlertas.BackColor = System.Drawing.Color.White;
            panelAlertas.Controls.Add(panel1);
            panelAlertas.Controls.Add(lstAlertas);
            panelAlertas.Location = new System.Drawing.Point(0, 0);
            panelAlertas.Name = "panelAlertas";
            panelAlertas.Size = new System.Drawing.Size(595, 320);
            panelAlertas.Paint += Card_Paint;

            panel1.BackColor = System.Drawing.Color.White;
            panel1.Controls.Add(label1);
            panel1.Dock = System.Windows.Forms.DockStyle.Top;
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(595, 44);

            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            label1.Location = new System.Drawing.Point(16, 12);
            label1.Name = "label1";
            label1.Text = "⚠  Alertas Principales";

            lstAlertas.Location = new System.Drawing.Point(0, 44);
            lstAlertas.Name = "lstAlertas";
            lstAlertas.Size = new System.Drawing.Size(595, 272);

            // panelActividad
            panelActividad.BackColor = System.Drawing.Color.White;
            panelActividad.Controls.Add(panel2);
            panelActividad.Controls.Add(lstActividad);
            panelActividad.Location = new System.Drawing.Point(615, 0);
            panelActividad.Name = "panelActividad";
            panelActividad.Size = new System.Drawing.Size(605, 320);
            panelActividad.Paint += Card_Paint;

            panel2.BackColor = System.Drawing.Color.White;
            panel2.Controls.Add(label2);
            panel2.Dock = System.Windows.Forms.DockStyle.Top;
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(605, 44);

            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            label2.Location = new System.Drawing.Point(16, 12);
            label2.Name = "label2";
            label2.Text = "🕐  Actividad Reciente";

            lstActividad.Location = new System.Drawing.Point(0, 44);
            lstActividad.Name = "lstActividad";
            lstActividad.Size = new System.Drawing.Size(605, 272);

            // ── panelBottom (gráfica + ranking) ────────────────────
            panelBottom.Controls.Add(panelGrafica);
            panelBottom.Controls.Add(panelRanking);
            panelBottom.Location = new System.Drawing.Point(20, 545);
            panelBottom.Name = "panelBottom";
            panelBottom.Size = new System.Drawing.Size(1220, 360);

            // panelGrafica
            panelGrafica.BackColor = System.Drawing.Color.White;
            panelGrafica.Controls.Add(panelGraficaHeader);
            panelGrafica.Location = new System.Drawing.Point(0, 0);
            panelGrafica.Name = "panelGrafica";
            panelGrafica.Size = new System.Drawing.Size(595, 360);
            panelGrafica.Paint += Card_Paint;

            panelGraficaHeader.BackColor = System.Drawing.Color.White;
            panelGraficaHeader.Controls.Add(lblGraficaTitulo);
            panelGraficaHeader.Dock = System.Windows.Forms.DockStyle.Top;
            panelGraficaHeader.Name = "panelGraficaHeader";
            panelGraficaHeader.Size = new System.Drawing.Size(595, 44);

            lblGraficaTitulo.AutoSize = true;
            lblGraficaTitulo.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            lblGraficaTitulo.Location = new System.Drawing.Point(16, 12);
            lblGraficaTitulo.Name = "lblGraficaTitulo";
            lblGraficaTitulo.Text = "📊  Estado del inventario";

            // panelRanking
            panelRanking.BackColor = System.Drawing.Color.White;
            panelRanking.Controls.Add(panelRankingHeader);
            panelRanking.Controls.Add(dgvRanking);
            panelRanking.Location = new System.Drawing.Point(615, 0);
            panelRanking.Name = "panelRanking";
            panelRanking.Size = new System.Drawing.Size(605, 360);
            panelRanking.Paint += Card_Paint;

            panelRankingHeader.BackColor = System.Drawing.Color.White;
            panelRankingHeader.Controls.Add(lblRankingTitulo);
            panelRankingHeader.Dock = System.Windows.Forms.DockStyle.Top;
            panelRankingHeader.Name = "panelRankingHeader";
            panelRankingHeader.Size = new System.Drawing.Size(605, 44);

            lblRankingTitulo.AutoSize = true;
            lblRankingTitulo.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            lblRankingTitulo.Location = new System.Drawing.Point(16, 12);
            lblRankingTitulo.Name = "lblRankingTitulo";
            lblRankingTitulo.Text = "🏆  Empleados con más préstamos";

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
            ClientSize = new System.Drawing.Size(1273, 940);
            WindowState = System.Windows.Forms.FormWindowState.Maximized;
            Controls.Add(panelTop);
            Controls.Add(panelCards);
            Controls.Add(panelContenido);
            Controls.Add(panelBottom);
            Controls.Add(panelInventario);
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
            System.Windows.Forms.Panel panel,
            System.Windows.Forms.Label lbl,
            string emoji,
            string titulo,
            System.Drawing.Color colorAccent,
            System.Drawing.Color colorFondo)
        {
            lbl.Dock = System.Windows.Forms.DockStyle.Fill;
            lbl.Font = new System.Drawing.Font("Segoe UI", 11F);
            lbl.ForeColor = colorAccent;
            lbl.Text = $"{emoji}\n{titulo}\n—";
            lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lbl.Name = lbl.Name;

            panel.BackColor = colorFondo;
            panel.Controls.Add(lbl);
            panel.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            panel.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            panel.Size = new System.Drawing.Size(223, 99);
            panel.Paint += (s, e) =>
            {
                // Línea izquierda de acento
                using var pen = new System.Drawing.Pen(colorAccent, 4);
                e.Graphics.DrawLine(pen, 0, 8, 0, panel.Height - 8);
                // Borde sutil
                using var borderPen = new System.Drawing.Pen(
                    System.Drawing.Color.FromArgb(220, 225, 235), 1);
                e.Graphics.DrawRectangle(borderPen,
                    0, 0, panel.Width - 1, panel.Height - 1);
            };
        }

        private static void Card_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            var panel = (System.Windows.Forms.Panel)sender;
            using var pen = new System.Drawing.Pen(
                System.Drawing.Color.FromArgb(220, 225, 235), 1);
            e.Graphics.DrawRectangle(pen, 0, 0, panel.Width - 1, panel.Height - 1);
        }
    }
}