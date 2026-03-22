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
            ((System.ComponentModel.ISupportInitialize)dgvReporte).BeginInit();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(804, 60);
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
            dgvReporte.Location = new Point(20, 130);
            dgvReporte.Name = "dgvReporte";
            dgvReporte.ReadOnly = true;
            dgvReporte.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReporte.Size = new Size(740, 350);
            dgvReporte.TabIndex = 3;
            // 
            // FrmReportes
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(804, 514);
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
    }
}