namespace PromacoHerra
{
    partial class FrmReportes
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblSubtitulo;
        private System.Windows.Forms.TableLayoutPanel tlpKpis;
        private System.Windows.Forms.FlowLayoutPanel flpTarjetas;
        private PromacoHerra.RoundedPanel pnlFiltros;
        private System.Windows.Forms.FlowLayoutPanel flpFiltros;
        private PromacoHerra.Controls.MaterialButton btnGenerar;
        private System.Windows.Forms.Panel pnlResultadosHeader;
        private System.Windows.Forms.Label lblReporte;
        private System.Windows.Forms.Label lblConteo;
        private PromacoHerra.Controls.MaterialButton btnExcel;
        private PromacoHerra.Controls.MaterialButton btnPDF;
        private System.Windows.Forms.Label lblResumen;
        private System.Windows.Forms.Panel pnlGridHost;
        private System.Windows.Forms.Label lblSinResultados;
        private System.Windows.Forms.Panel pnlSinIncidentes;
        private FontAwesome.Sharp.IconPictureBox icoSinIncidentes;
        private System.Windows.Forms.Label lblSinIncidentes;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            tlpMain = new TableLayoutPanel();
            pnlHeader = new Panel();
            lblTitulo = new Label();
            lblSubtitulo = new Label();
            tlpKpis = new TableLayoutPanel();
            flpTarjetas = new FlowLayoutPanel();
            pnlFiltros = new PromacoHerra.RoundedPanel();
            flpFiltros = new FlowLayoutPanel();
            btnGenerar = new PromacoHerra.Controls.MaterialButton();
            pnlResultadosHeader = new Panel();
            lblReporte = new Label();
            lblConteo = new Label();
            btnExcel = new PromacoHerra.Controls.MaterialButton();
            btnPDF = new PromacoHerra.Controls.MaterialButton();
            lblResumen = new Label();
            pnlGridHost = new Panel();
            lblSinResultados = new Label();
            pnlSinIncidentes = new Panel();
            icoSinIncidentes = new FontAwesome.Sharp.IconPictureBox();
            lblSinIncidentes = new Label();
            tlpMain.SuspendLayout();
            pnlHeader.SuspendLayout();
            pnlFiltros.SuspendLayout();
            pnlResultadosHeader.SuspendLayout();
            pnlGridHost.SuspendLayout();
            pnlSinIncidentes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)icoSinIncidentes).BeginInit();
            SuspendLayout();
            //
            // tlpMain
            //
            tlpMain.ColumnCount = 1;
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpMain.Controls.Add(pnlHeader, 0, 0);
            tlpMain.Controls.Add(tlpKpis, 0, 1);
            tlpMain.Controls.Add(flpTarjetas, 0, 2);
            tlpMain.Controls.Add(pnlFiltros, 0, 3);
            tlpMain.Controls.Add(pnlResultadosHeader, 0, 4);
            tlpMain.Controls.Add(lblResumen, 0, 5);
            tlpMain.Controls.Add(pnlGridHost, 0, 6);
            tlpMain.Dock = DockStyle.Fill;
            tlpMain.Name = "tlpMain";
            tlpMain.Padding = new Padding(20, 8, 20, 14);
            tlpMain.RowCount = 7;
            tlpMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpMain.TabIndex = 0;
            //
            // pnlHeader
            //
            pnlHeader.Controls.Add(lblTitulo);
            pnlHeader.Controls.Add(lblSubtitulo);
            pnlHeader.Dock = DockStyle.Fill;
            pnlHeader.Margin = new Padding(0, 0, 0, 6);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1160, 46);
            pnlHeader.TabIndex = 0;
            //
            // lblTitulo
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblTitulo.Location = new Point(0, 4);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Reportes";
            //
            // lblSubtitulo
            //
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.Font = new Font("Segoe UI", 9F);
            lblSubtitulo.Location = new Point(124, 14);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.TabIndex = 1;
            lblSubtitulo.Text = "Indicadores actualizados";
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
            tlpKpis.Margin = new Padding(0, 0, 0, 12);
            tlpKpis.Name = "tlpKpis";
            tlpKpis.RowCount = 1;
            tlpKpis.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpKpis.Size = new Size(1160, 92);
            tlpKpis.TabIndex = 1;
            //
            // flpTarjetas
            //
            flpTarjetas.AutoSize = true;
            flpTarjetas.Dock = DockStyle.Fill;
            flpTarjetas.Margin = new Padding(0, 0, 0, 4);
            flpTarjetas.Name = "flpTarjetas";
            flpTarjetas.TabIndex = 2;
            //
            // pnlFiltros
            //
            pnlFiltros.Controls.Add(flpFiltros);
            pnlFiltros.Controls.Add(btnGenerar);
            pnlFiltros.Dock = DockStyle.Fill;
            pnlFiltros.Margin = new Padding(0, 0, 0, 10);
            pnlFiltros.Name = "pnlFiltros";
            pnlFiltros.Size = new Size(1160, 84);
            pnlFiltros.TabIndex = 3;
            //
            // flpFiltros
            //
            flpFiltros.Location = new Point(14, 10);
            flpFiltros.Name = "flpFiltros";
            flpFiltros.Size = new Size(950, 64);
            flpFiltros.TabIndex = 0;
            flpFiltros.WrapContents = false;
            //
            // btnGenerar
            //
            btnGenerar.Icon = FontAwesome.Sharp.IconChar.Play;
            btnGenerar.IconSize = 14;
            btnGenerar.Location = new Point(980, 28);
            btnGenerar.Name = "btnGenerar";
            btnGenerar.Size = new Size(160, 40);
            btnGenerar.TabIndex = 1;
            btnGenerar.Text = "Generar reporte";
            btnGenerar.Variant = PromacoHerra.Controls.MaterialButtonVariant.Primary;
            //
            // pnlResultadosHeader
            //
            pnlResultadosHeader.Controls.Add(lblReporte);
            pnlResultadosHeader.Controls.Add(lblConteo);
            pnlResultadosHeader.Controls.Add(btnExcel);
            pnlResultadosHeader.Controls.Add(btnPDF);
            pnlResultadosHeader.Dock = DockStyle.Fill;
            pnlResultadosHeader.Margin = new Padding(0, 0, 0, 6);
            pnlResultadosHeader.Name = "pnlResultadosHeader";
            pnlResultadosHeader.Size = new Size(1160, 40);
            pnlResultadosHeader.TabIndex = 4;
            //
            // lblReporte
            //
            lblReporte.AutoSize = true;
            lblReporte.Font = new Font("Segoe UI", 11.5F, FontStyle.Bold);
            lblReporte.Location = new Point(0, 8);
            lblReporte.Name = "lblReporte";
            lblReporte.TabIndex = 0;
            lblReporte.Text = "Historial de préstamos";
            //
            // lblConteo
            //
            lblConteo.AutoSize = true;
            lblConteo.Font = new Font("Segoe UI", 9F);
            lblConteo.Location = new Point(220, 13);
            lblConteo.Name = "lblConteo";
            lblConteo.TabIndex = 1;
            lblConteo.Text = "";
            //
            // btnExcel
            //
            btnExcel.Icon = FontAwesome.Sharp.IconChar.FileExcel;
            btnExcel.IconSize = 16;
            btnExcel.Location = new Point(920, 0);
            btnExcel.Name = "btnExcel";
            btnExcel.Size = new Size(110, 38);
            btnExcel.TabIndex = 2;
            btnExcel.Text = "Excel";
            btnExcel.Variant = PromacoHerra.Controls.MaterialButtonVariant.Default;
            //
            // btnPDF
            //
            btnPDF.Icon = FontAwesome.Sharp.IconChar.FilePdf;
            btnPDF.IconSize = 16;
            btnPDF.Location = new Point(1040, 0);
            btnPDF.Name = "btnPDF";
            btnPDF.Size = new Size(110, 38);
            btnPDF.TabIndex = 3;
            btnPDF.Text = "PDF";
            btnPDF.Variant = PromacoHerra.Controls.MaterialButtonVariant.Default;
            //
            // lblResumen
            //
            lblResumen.AutoSize = true;
            lblResumen.Font = new Font("Segoe UI", 9.5F);
            lblResumen.Margin = new Padding(0, 0, 0, 8);
            lblResumen.Name = "lblResumen";
            lblResumen.TabIndex = 5;
            lblResumen.Text = "";
            lblResumen.Visible = false;
            //
            // pnlGridHost
            //
            pnlGridHost.BackColor = Color.White;
            pnlGridHost.Controls.Add(lblSinResultados);
            pnlGridHost.Controls.Add(pnlSinIncidentes);
            pnlGridHost.Dock = DockStyle.Fill;
            pnlGridHost.Margin = new Padding(0);
            pnlGridHost.Name = "pnlGridHost";
            pnlGridHost.TabIndex = 6;
            //
            // lblSinResultados
            //
            lblSinResultados.Dock = DockStyle.Fill;
            lblSinResultados.Font = new Font("Segoe UI", 10.5F);
            lblSinResultados.Name = "lblSinResultados";
            lblSinResultados.TabIndex = 0;
            lblSinResultados.Text = "Sin resultados para los filtros seleccionados.";
            lblSinResultados.TextAlign = ContentAlignment.MiddleCenter;
            lblSinResultados.Visible = false;
            //
            // pnlSinIncidentes
            //
            pnlSinIncidentes.Controls.Add(icoSinIncidentes);
            pnlSinIncidentes.Controls.Add(lblSinIncidentes);
            pnlSinIncidentes.Dock = DockStyle.Fill;
            pnlSinIncidentes.Name = "pnlSinIncidentes";
            pnlSinIncidentes.TabIndex = 1;
            pnlSinIncidentes.Visible = false;
            //
            // icoSinIncidentes
            //
            icoSinIncidentes.IconChar = FontAwesome.Sharp.IconChar.CircleCheck;
            icoSinIncidentes.IconSize = 56;
            icoSinIncidentes.Name = "icoSinIncidentes";
            icoSinIncidentes.Size = new Size(60, 56);
            icoSinIncidentes.TabIndex = 0;
            icoSinIncidentes.TabStop = false;
            //
            // lblSinIncidentes
            //
            lblSinIncidentes.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblSinIncidentes.Name = "lblSinIncidentes";
            lblSinIncidentes.Size = new Size(500, 30);
            lblSinIncidentes.TabIndex = 1;
            lblSinIncidentes.Text = "Sin incidentes en este período";
            lblSinIncidentes.TextAlign = ContentAlignment.MiddleCenter;
            //
            // FrmReportes
            //
            ClientSize = new Size(1200, 760);
            Controls.Add(tlpMain);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FrmReportes";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Reportes";
            tlpMain.ResumeLayout(false);
            tlpMain.PerformLayout();
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlFiltros.ResumeLayout(false);
            pnlResultadosHeader.ResumeLayout(false);
            pnlResultadosHeader.PerformLayout();
            pnlGridHost.ResumeLayout(false);
            pnlSinIncidentes.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)icoSinIncidentes).EndInit();
            ResumeLayout(false);
        }
    }
}
