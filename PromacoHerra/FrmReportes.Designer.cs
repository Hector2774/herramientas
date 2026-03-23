namespace PromacoHerra
{
    partial class FrmReportes
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.ComboBox cboReporte;
        private System.Windows.Forms.Button btnGenerar;
        private System.Windows.Forms.DataGridView dgvReporte;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblTitulo = new Label();
            cboReporte = new ComboBox();
            btnGenerar = new Button();
            dgvReporte = new DataGridView();
            dtpDesde = new DateTimePicker();
            dtpHasta = new DateTimePicker();
            btnGenerarAtrasados = new Button();
            btnReporteEmpleado = new Button();
            cboEmpleado = new ComboBox();
            btnReporteHerramienta = new Button();
            cboHerramienta = new ComboBox();
            btnReporteDanadas = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvReporte).BeginInit();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(1501, 60);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Reportes del Sistema";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cboReporte
            // 
            cboReporte.DropDownStyle = ComboBoxStyle.DropDownList;
            cboReporte.Location = new Point(20, 80);
            cboReporte.Name = "cboReporte";
            cboReporte.Size = new Size(300, 23);
            cboReporte.TabIndex = 1;
            // 
            // btnGenerar
            // 
            btnGenerar.Location = new Point(340, 80);
            btnGenerar.Name = "btnGenerar";
            btnGenerar.Size = new Size(150, 23);
            btnGenerar.TabIndex = 2;
            btnGenerar.Text = "Generar Reporte";
            // 
            // dgvReporte
            // 
            dgvReporte.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvReporte.Location = new Point(24, 295);
            dgvReporte.Name = "dgvReporte";
            dgvReporte.ReadOnly = true;
            dgvReporte.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReporte.Size = new Size(947, 444);
            dgvReporte.TabIndex = 3;
            // 
            // dtpDesde
            // 
            dtpDesde.Location = new Point(24, 134);
            dtpDesde.Name = "dtpDesde";
            dtpDesde.Size = new Size(200, 23);
            dtpDesde.TabIndex = 4;
            // 
            // dtpHasta
            // 
            dtpHasta.Location = new Point(258, 134);
            dtpHasta.Name = "dtpHasta";
            dtpHasta.Size = new Size(200, 23);
            dtpHasta.TabIndex = 5;
            // 
            // btnGenerarAtrasados
            // 
            btnGenerarAtrasados.Location = new Point(478, 133);
            btnGenerarAtrasados.Name = "btnGenerarAtrasados";
            btnGenerarAtrasados.Size = new Size(138, 23);
            btnGenerarAtrasados.TabIndex = 6;
            btnGenerarAtrasados.Text = "Prestamos atrasados";
            btnGenerarAtrasados.UseVisualStyleBackColor = true;
            // 
            // btnReporteEmpleado
            // 
            btnReporteEmpleado.Location = new Point(24, 177);
            btnReporteEmpleado.Name = "btnReporteEmpleado";
            btnReporteEmpleado.Size = new Size(143, 23);
            btnReporteEmpleado.TabIndex = 7;
            btnReporteEmpleado.Text = "Reporte por empleado";
            btnReporteEmpleado.UseVisualStyleBackColor = true;
            // 
            // cboEmpleado
            // 
            cboEmpleado.FormattingEnabled = true;
            cboEmpleado.Location = new Point(183, 178);
            cboEmpleado.Name = "cboEmpleado";
            cboEmpleado.Size = new Size(121, 23);
            cboEmpleado.TabIndex = 8;
            // 
            // btnReporteHerramienta
            // 
            btnReporteHerramienta.Location = new Point(28, 224);
            btnReporteHerramienta.Name = "btnReporteHerramienta";
            btnReporteHerramienta.Size = new Size(154, 23);
            btnReporteHerramienta.TabIndex = 9;
            btnReporteHerramienta.Text = "Reporte por herramienta";
            btnReporteHerramienta.UseVisualStyleBackColor = true;
            // 
            // cboHerramienta
            // 
            cboHerramienta.FormattingEnabled = true;
            cboHerramienta.Location = new Point(188, 225);
            cboHerramienta.Name = "cboHerramienta";
            cboHerramienta.Size = new Size(121, 23);
            cboHerramienta.TabIndex = 10;
            // 
            // btnReporteDanadas
            // 
            btnReporteDanadas.Location = new Point(403, 204);
            btnReporteDanadas.Name = "btnReporteDanadas";
            btnReporteDanadas.Size = new Size(133, 23);
            btnReporteDanadas.TabIndex = 11;
            btnReporteDanadas.Text = "Reporte de dañadas";
            btnReporteDanadas.UseVisualStyleBackColor = true;
            // 
            // FrmReportes
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1501, 823);
            Controls.Add(btnReporteDanadas);
            Controls.Add(cboHerramienta);
            Controls.Add(btnReporteHerramienta);
            Controls.Add(cboEmpleado);
            Controls.Add(btnReporteEmpleado);
            Controls.Add(btnGenerarAtrasados);
            Controls.Add(dtpHasta);
            Controls.Add(dtpDesde);
            Controls.Add(lblTitulo);
            Controls.Add(cboReporte);
            Controls.Add(btnGenerar);
            Controls.Add(dgvReporte);
            Name = "FrmReportes";
            Text = "FrmReportes";
            ((System.ComponentModel.ISupportInitialize)dgvReporte).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DateTimePicker dtpDesde;
        private DateTimePicker dtpHasta;
        private Button btnGenerarAtrasados;
        private Button btnReporteEmpleado;
        private ComboBox cboEmpleado;
        private Button btnReporteHerramienta;
        private ComboBox cboHerramienta;
        private Button btnReporteDanadas;
    }
}