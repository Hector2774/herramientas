namespace PromacoHerra
{
    partial class FrmDashboard
    {
        private System.ComponentModel.IContainer components = null;

        private PromacoHerra.Controls.PanelGdi pnlTopbar;
        private System.Windows.Forms.Panel pnlScroll;
        private System.Windows.Forms.TableLayoutPanel tlpContenido;
        private System.Windows.Forms.TableLayoutPanel tlpKpis;
        private System.Windows.Forms.TableLayoutPanel tlpGraficos;
        private PromacoHerra.Controls.PanelGdi pnlDona;
        private PromacoHerra.Controls.PanelGdi pnlLineas;
        private System.Windows.Forms.TableLayoutPanel tlpWidgets;
        private PromacoHerra.Controls.PanelGdi pnlDepartamentos;
        private PromacoHerra.Controls.PanelGdi pnlProximas;
        private PromacoHerra.Controls.PanelGdi pnlStockAlertas;
        private System.Windows.Forms.Label lblActividadTitulo;
        private System.Windows.Forms.FlowLayoutPanel flpActividad;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pnlTopbar = new PromacoHerra.Controls.PanelGdi();
            pnlScroll = new Panel();
            tlpContenido = new TableLayoutPanel();
            tlpKpis = new TableLayoutPanel();
            tlpGraficos = new TableLayoutPanel();
            pnlDona = new PromacoHerra.Controls.PanelGdi();
            pnlLineas = new PromacoHerra.Controls.PanelGdi();
            tlpWidgets = new TableLayoutPanel();
            pnlDepartamentos = new PromacoHerra.Controls.PanelGdi();
            pnlProximas = new PromacoHerra.Controls.PanelGdi();
            pnlStockAlertas = new PromacoHerra.Controls.PanelGdi();
            lblActividadTitulo = new Label();
            flpActividad = new FlowLayoutPanel();
            pnlScroll.SuspendLayout();
            tlpContenido.SuspendLayout();
            tlpGraficos.SuspendLayout();
            tlpWidgets.SuspendLayout();
            SuspendLayout();
            // 
            // pnlTopbar
            // 
            pnlTopbar.BackColor = Color.White;
            pnlTopbar.Dock = DockStyle.Top;
            pnlTopbar.Location = new Point(0, 0);
            pnlTopbar.Name = "pnlTopbar";
            pnlTopbar.Size = new Size(1240, 64);
            pnlTopbar.TabIndex = 0;
            // 
            // pnlScroll
            // 
            pnlScroll.AutoScroll = true;
            pnlScroll.Controls.Add(tlpContenido);
            pnlScroll.Dock = DockStyle.Fill;
            pnlScroll.Location = new Point(0, 64);
            pnlScroll.Name = "pnlScroll";
            pnlScroll.Size = new Size(1240, 821);
            pnlScroll.TabIndex = 1;
            // 
            // tlpContenido
            // 
            tlpContenido.AutoSize = true;
            tlpContenido.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpContenido.ColumnCount = 1;
            tlpContenido.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpContenido.Controls.Add(tlpKpis, 0, 0);
            tlpContenido.Controls.Add(tlpGraficos, 0, 1);
            tlpContenido.Controls.Add(tlpWidgets, 0, 2);
            tlpContenido.Controls.Add(lblActividadTitulo, 0, 3);
            tlpContenido.Controls.Add(flpActividad, 0, 4);
            tlpContenido.Dock = DockStyle.Top;
            tlpContenido.Location = new Point(0, 0);
            tlpContenido.Name = "tlpContenido";
            tlpContenido.Padding = new Padding(20, 16, 20, 20);
            tlpContenido.RowCount = 5;
            tlpContenido.RowStyles.Add(new RowStyle());
            tlpContenido.RowStyles.Add(new RowStyle());
            tlpContenido.RowStyles.Add(new RowStyle());
            tlpContenido.RowStyles.Add(new RowStyle());
            tlpContenido.RowStyles.Add(new RowStyle());
            tlpContenido.Size = new Size(1240, 810);
            tlpContenido.TabIndex = 0;
            // 
            // tlpKpis
            // 
            tlpKpis.ColumnCount = 5;
            tlpKpis.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpKpis.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpKpis.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpKpis.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpKpis.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpKpis.Dock = DockStyle.Fill;
            tlpKpis.Location = new Point(20, 16);
            tlpKpis.Margin = new Padding(0, 0, 0, 12);
            tlpKpis.Name = "tlpKpis";
            tlpKpis.RowCount = 1;
            tlpKpis.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpKpis.Size = new Size(1200, 94);
            tlpKpis.TabIndex = 0;
            // 
            // tlpGraficos
            // 
            tlpGraficos.ColumnCount = 2;
            tlpGraficos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32F));
            tlpGraficos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 68F));
            tlpGraficos.Controls.Add(pnlDona, 0, 0);
            tlpGraficos.Controls.Add(pnlLineas, 1, 0);
            tlpGraficos.Dock = DockStyle.Fill;
            tlpGraficos.Location = new Point(20, 122);
            tlpGraficos.Margin = new Padding(0, 0, 0, 6);
            tlpGraficos.Name = "tlpGraficos";
            tlpGraficos.RowCount = 1;
            tlpGraficos.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpGraficos.Size = new Size(1200, 300);
            tlpGraficos.TabIndex = 1;
            // 
            // pnlDona
            // 
            pnlDona.Dock = DockStyle.Fill;
            pnlDona.Location = new Point(0, 0);
            pnlDona.Margin = new Padding(0, 0, 6, 6);
            pnlDona.Name = "pnlDona";
            pnlDona.Size = new Size(378, 294);
            pnlDona.TabIndex = 0;
            // 
            // pnlLineas
            // 
            pnlLineas.Dock = DockStyle.Fill;
            pnlLineas.Location = new Point(390, 0);
            pnlLineas.Margin = new Padding(6, 0, 0, 6);
            pnlLineas.Name = "pnlLineas";
            pnlLineas.Size = new Size(810, 294);
            pnlLineas.TabIndex = 1;
            // 
            // tlpWidgets
            // 
            tlpWidgets.ColumnCount = 3;
            tlpWidgets.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tlpWidgets.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tlpWidgets.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));
            tlpWidgets.Controls.Add(pnlDepartamentos, 0, 0);
            tlpWidgets.Controls.Add(pnlProximas, 1, 0);
            tlpWidgets.Controls.Add(pnlStockAlertas, 2, 0);
            tlpWidgets.Dock = DockStyle.Fill;
            tlpWidgets.Location = new Point(20, 428);
            tlpWidgets.Margin = new Padding(0, 0, 0, 14);
            tlpWidgets.Name = "tlpWidgets";
            tlpWidgets.RowCount = 1;
            tlpWidgets.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpWidgets.Size = new Size(1200, 320);
            tlpWidgets.TabIndex = 2;
            // 
            // pnlDepartamentos
            // 
            pnlDepartamentos.Dock = DockStyle.Fill;
            pnlDepartamentos.Location = new Point(0, 6);
            pnlDepartamentos.Margin = new Padding(0, 6, 6, 0);
            pnlDepartamentos.Name = "pnlDepartamentos";
            pnlDepartamentos.Size = new Size(393, 314);
            pnlDepartamentos.TabIndex = 0;
            // 
            // pnlProximas
            // 
            pnlProximas.Dock = DockStyle.Fill;
            pnlProximas.Location = new Point(405, 6);
            pnlProximas.Margin = new Padding(6, 6, 6, 0);
            pnlProximas.Name = "pnlProximas";
            pnlProximas.Size = new Size(387, 314);
            pnlProximas.TabIndex = 1;
            // 
            // pnlStockAlertas
            // 
            pnlStockAlertas.Dock = DockStyle.Fill;
            pnlStockAlertas.Location = new Point(804, 6);
            pnlStockAlertas.Margin = new Padding(6, 6, 0, 0);
            pnlStockAlertas.Name = "pnlStockAlertas";
            pnlStockAlertas.Size = new Size(396, 314);
            pnlStockAlertas.TabIndex = 2;
            // 
            // lblActividadTitulo
            // 
            lblActividadTitulo.AutoSize = true;
            lblActividadTitulo.Font = new Font("Segoe UI Semibold", 11F);
            lblActividadTitulo.Location = new Point(20, 762);
            lblActividadTitulo.Margin = new Padding(0, 0, 0, 8);
            lblActividadTitulo.Name = "lblActividadTitulo";
            lblActividadTitulo.Size = new Size(124, 20);
            lblActividadTitulo.TabIndex = 3;
            lblActividadTitulo.Text = "Actividad de hoy";
            // 
            // flpActividad
            // 
            flpActividad.AutoSize = true;
            flpActividad.Dock = DockStyle.Fill;
            flpActividad.Location = new Point(20, 790);
            flpActividad.Margin = new Padding(0);
            flpActividad.Name = "flpActividad";
            flpActividad.Size = new Size(1200, 1);
            flpActividad.TabIndex = 4;
            // 
            // FrmDashboard
            // 
            ClientSize = new Size(1240, 885);
            Controls.Add(pnlScroll);
            Controls.Add(pnlTopbar);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FrmDashboard";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Inicio";
            pnlScroll.ResumeLayout(false);
            pnlScroll.PerformLayout();
            tlpContenido.ResumeLayout(false);
            tlpContenido.PerformLayout();
            tlpGraficos.ResumeLayout(false);
            tlpWidgets.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}
