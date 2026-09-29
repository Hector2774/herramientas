namespace PromacoHerra
{
    partial class FrmReportes
    {
        private System.ComponentModel.IContainer components = null;

        private void InitializeComponent()
        {
            panelFiltros = new RoundedPanel();
            label6 = new Label();
            label5 = new Label();
            btnGenerar = new PromacoHerra.Controls.MaterialButton();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            cboTipoReporte = new PromacoHerra.Controls.MaterialComboBox();
            pickerEmpleado = new PromacoHerra.Controls.EmpleadoPickerControl();
            btnEmpleadoTodos = new PromacoHerra.Controls.MaterialButton();
            cboHerramienta = new PromacoHerra.Controls.MaterialComboBox();
            dtpDesde = new DateTimePicker();
            dtpHasta = new DateTimePicker();
            lblTitulo = new Label();
            panelCards = new FlowLayoutPanel();
            cardAtrasos = new RoundedPanel();
            lblAtrasos = new Label();
            cardDisponibles = new RoundedPanel();
            lblDisponibles = new Label();
            cardActivos = new RoundedPanel();
            lblActivos = new Label();
            cardDanadas = new RoundedPanel();
            lblDanadas = new Label();
            panel1 = new RoundedPanel();
            dgvReporte = new DataGridView();
            lblReporte = new Label();
            btnExcel = new PromacoHerra.Controls.MaterialButton();
            btnPDF = new PromacoHerra.Controls.MaterialButton();
            panel2 = new RoundedPanel();
            lblExportar = new Label();
            panelFiltros.SuspendLayout();
            panelCards.SuspendLayout();
            cardAtrasos.SuspendLayout();
            cardDisponibles.SuspendLayout();
            cardActivos.SuspendLayout();
            cardDanadas.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvReporte).BeginInit();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panelFiltros
            // 
            panelFiltros.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panelFiltros.BackColor = Color.White;
            panelFiltros.Controls.Add(label6);
            panelFiltros.Controls.Add(label5);
            panelFiltros.Controls.Add(btnGenerar);
            panelFiltros.Controls.Add(label4);
            panelFiltros.Controls.Add(label3);
            panelFiltros.Controls.Add(label2);
            panelFiltros.Controls.Add(label1);
            panelFiltros.Controls.Add(cboTipoReporte);
            panelFiltros.Controls.Add(pickerEmpleado);
            panelFiltros.Controls.Add(btnEmpleadoTodos);
            panelFiltros.Controls.Add(cboHerramienta);
            panelFiltros.Controls.Add(dtpDesde);
            panelFiltros.Controls.Add(dtpHasta);
            panelFiltros.Location = new Point(13, 131);
            panelFiltros.Name = "panelFiltros";
            panelFiltros.Padding = new Padding(0, 0, 6, 6);
            panelFiltros.Size = new Size(878, 145);
            panelFiltros.TabIndex = 13;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            label6.Location = new Point(240, 95);
            label6.Name = "label6";
            label6.Size = new Size(43, 17);
            label6.TabIndex = 10;
            label6.Text = "Hasta";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            label5.Location = new Point(20, 95);
            label5.Name = "label5";
            label5.Size = new Size(46, 17);
            label5.TabIndex = 9;
            label5.Text = "Desde";
            // 
            // btnGenerar
            // 
            btnGenerar.CornerRadius = 8;
            btnGenerar.FlatStyle = FlatStyle.Flat;
            btnGenerar.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnGenerar.Icon = FontAwesome.Sharp.IconChar.BarChart;
            btnGenerar.IconColor = null;
            btnGenerar.IconSize = 22;
            btnGenerar.Location = new Point(460, 106);
            btnGenerar.Name = "btnGenerar";
            btnGenerar.Size = new Size(178, 36);
            btnGenerar.TabIndex = 20;
            btnGenerar.Text = "Generar reporte";
            btnGenerar.Variant = PromacoHerra.Controls.MaterialButtonVariant.Default;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            label4.ForeColor = SystemColors.ControlText;
            label4.Location = new Point(561, 31);
            label4.Name = "label4";
            label4.Size = new Size(85, 17);
            label4.TabIndex = 8;
            label4.Text = "Herramienta";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            label3.ForeColor = SystemColors.ControlText;
            label3.Location = new Point(240, 33);
            label3.Name = "label3";
            label3.Size = new Size(69, 17);
            label3.TabIndex = 7;
            label3.Text = "Empleado";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            label2.ForeColor = SystemColors.ControlText;
            label2.Location = new Point(20, 33);
            label2.Name = "label2";
            label2.Size = new Size(107, 17);
            label2.TabIndex = 6;
            label2.Text = "Tipo de Reporte";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            label1.ForeColor = SystemColors.ControlText;
            label1.Location = new Point(20, 5);
            label1.Name = "label1";
            label1.Size = new Size(58, 17);
            label1.TabIndex = 5;
            label1.Text = "FILTROS";
            // 
            // cboTipoReporte
            // 
            cboTipoReporte.BackColor = Color.WhiteSmoke;
            cboTipoReporte.DataSource = null;
            cboTipoReporte.DisplayMember = "";
            cboTipoReporte.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTipoReporte.Location = new Point(20, 51);
            cboTipoReporte.MinimumSize = new Size(0, 32);
            cboTipoReporte.Name = "cboTipoReporte";
            cboTipoReporte.Padding = new Padding(0, 0, 0, 4);
            cboTipoReporte.SelectedIndex = -1;
            cboTipoReporte.SelectedItem = null;
            cboTipoReporte.SelectedValue = null;
            cboTipoReporte.Size = new Size(200, 32);
            cboTipoReporte.TabIndex = 0;
            cboTipoReporte.ValueMember = "";
            // 
            // pickerEmpleado
            // 
            pickerEmpleado.Location = new Point(240, 53);
            pickerEmpleado.MinimumSize = new Size(200, 28);
            pickerEmpleado.Name = "pickerEmpleado";
            pickerEmpleado.Size = new Size(200, 28);
            pickerEmpleado.TabIndex = 21;
            // 
            // btnEmpleadoTodos
            // 
            btnEmpleadoTodos.CornerRadius = 8;
            btnEmpleadoTodos.FlatStyle = FlatStyle.Flat;
            btnEmpleadoTodos.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnEmpleadoTodos.Icon = null;
            btnEmpleadoTodos.IconColor = null;
            btnEmpleadoTodos.IconSize = 16;
            btnEmpleadoTodos.Location = new Point(446, 51);
            btnEmpleadoTodos.Name = "btnEmpleadoTodos";
            btnEmpleadoTodos.Size = new Size(55, 28);
            btnEmpleadoTodos.TabIndex = 21;
            btnEmpleadoTodos.Text = "Todos";
            btnEmpleadoTodos.Variant = PromacoHerra.Controls.MaterialButtonVariant.Default;
            // 
            // cboHerramienta
            // 
            cboHerramienta.BackColor = Color.WhiteSmoke;
            cboHerramienta.DataSource = null;
            cboHerramienta.DisplayMember = "";
            cboHerramienta.DropDownStyle = ComboBoxStyle.DropDownList;
            cboHerramienta.Location = new Point(561, 49);
            cboHerramienta.MinimumSize = new Size(0, 32);
            cboHerramienta.Name = "cboHerramienta";
            cboHerramienta.Padding = new Padding(0, 0, 0, 4);
            cboHerramienta.SelectedIndex = -1;
            cboHerramienta.SelectedItem = null;
            cboHerramienta.SelectedValue = null;
            cboHerramienta.Size = new Size(200, 32);
            cboHerramienta.TabIndex = 2;
            cboHerramienta.ValueMember = "";
            // 
            // dtpDesde
            // 
            dtpDesde.CustomFormat = "dd/MM/yyyy";
            dtpDesde.Format = DateTimePickerFormat.Custom;
            dtpDesde.Location = new Point(20, 113);
            dtpDesde.Name = "dtpDesde";
            dtpDesde.Size = new Size(200, 23);
            dtpDesde.TabIndex = 3;
            // 
            // dtpHasta
            // 
            dtpHasta.CustomFormat = "dd/MM/yyyy";
            dtpHasta.Format = DateTimePickerFormat.Custom;
            dtpHasta.Location = new Point(240, 113);
            dtpHasta.Name = "dtpHasta";
            dtpHasta.Size = new Size(200, 23);
            dtpHasta.TabIndex = 4;
            // 
            // lblTitulo
            // 
            lblTitulo.BackColor = Color.White;
            lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.Location = new Point(3, 18);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(147, 29);
            lblTitulo.TabIndex = 15;
            lblTitulo.Text = "Resultados";
            // 
            // panelCards
            // 
            panelCards.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panelCards.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panelCards.Controls.Add(cardAtrasos);
            panelCards.Controls.Add(cardDisponibles);
            panelCards.Controls.Add(cardActivos);
            panelCards.Controls.Add(cardDanadas);
            panelCards.Location = new Point(13, 3);
            panelCards.Name = "panelCards";
            panelCards.Padding = new Padding(0, 10, 0, 10);
            panelCards.Size = new Size(878, 122);
            panelCards.TabIndex = 16;
            // 
            // cardAtrasos
            // 
            cardAtrasos.BackColor = Color.Red;
            cardAtrasos.Controls.Add(lblAtrasos);
            cardAtrasos.Location = new Point(0, 10);
            cardAtrasos.Margin = new Padding(0, 0, 7, 0);
            cardAtrasos.Name = "cardAtrasos";
            cardAtrasos.Padding = new Padding(2, 0, 0, 0);
            cardAtrasos.Size = new Size(205, 108);
            cardAtrasos.TabIndex = 1;
            // 
            // lblAtrasos
            // 
            lblAtrasos.BackColor = Color.White;
            lblAtrasos.Dock = DockStyle.Fill;
            lblAtrasos.Font = new Font("Segoe UI", 13.5F);
            lblAtrasos.Location = new Point(2, 0);
            lblAtrasos.Name = "lblAtrasos";
            lblAtrasos.Size = new Size(203, 108);
            lblAtrasos.TabIndex = 0;
            lblAtrasos.Text = "Atrasos\n2";
            lblAtrasos.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cardDisponibles
            // 
            cardDisponibles.BackColor = Color.DarkBlue;
            cardDisponibles.Controls.Add(lblDisponibles);
            cardDisponibles.Location = new Point(212, 10);
            cardDisponibles.Margin = new Padding(0, 0, 7, 0);
            cardDisponibles.Name = "cardDisponibles";
            cardDisponibles.Padding = new Padding(2, 0, 0, 0);
            cardDisponibles.Size = new Size(205, 108);
            cardDisponibles.TabIndex = 2;
            // 
            // lblDisponibles
            // 
            lblDisponibles.BackColor = Color.White;
            lblDisponibles.Dock = DockStyle.Fill;
            lblDisponibles.Font = new Font("Segoe UI", 13.5F);
            lblDisponibles.Location = new Point(2, 0);
            lblDisponibles.Margin = new Padding(0);
            lblDisponibles.Name = "lblDisponibles";
            lblDisponibles.Size = new Size(203, 108);
            lblDisponibles.TabIndex = 0;
            lblDisponibles.Text = "Disponibles\n38";
            lblDisponibles.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cardActivos
            // 
            cardActivos.BackColor = Color.LimeGreen;
            cardActivos.Controls.Add(lblActivos);
            cardActivos.Location = new Point(424, 10);
            cardActivos.Margin = new Padding(0, 0, 7, 0);
            cardActivos.Name = "cardActivos";
            cardActivos.Padding = new Padding(2, 0, 0, 0);
            cardActivos.Size = new Size(205, 108);
            cardActivos.TabIndex = 0;
            // 
            // lblActivos
            // 
            lblActivos.BackColor = Color.White;
            lblActivos.Dock = DockStyle.Fill;
            lblActivos.Font = new Font("Segoe UI", 13.5F);
            lblActivos.Location = new Point(2, 0);
            lblActivos.Name = "lblActivos";
            lblActivos.Size = new Size(203, 108);
            lblActivos.TabIndex = 0;
            lblActivos.Text = "Préstamos activos\n14";
            lblActivos.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cardDanadas
            // 
            cardDanadas.BackColor = Color.DarkOrange;
            cardDanadas.Controls.Add(lblDanadas);
            cardDanadas.Location = new Point(636, 10);
            cardDanadas.Margin = new Padding(0, 0, 7, 0);
            cardDanadas.Name = "cardDanadas";
            cardDanadas.Padding = new Padding(2, 0, 0, 0);
            cardDanadas.Size = new Size(205, 108);
            cardDanadas.TabIndex = 3;
            // 
            // lblDanadas
            // 
            lblDanadas.BackColor = Color.White;
            lblDanadas.Dock = DockStyle.Fill;
            lblDanadas.Font = new Font("Segoe UI", 13.5F);
            lblDanadas.Location = new Point(2, 0);
            lblDanadas.Name = "lblDanadas";
            lblDanadas.Size = new Size(203, 108);
            lblDanadas.TabIndex = 0;
            lblDanadas.Text = "Dañadas\n3";
            lblDanadas.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.BackColor = Color.White;
            panel1.Controls.Add(dgvReporte);
            panel1.Controls.Add(lblReporte);
            panel1.Location = new Point(13, 354);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(0, 0, 6, 6);
            panel1.Size = new Size(878, 479);
            panel1.TabIndex = 17;
            // 
            // dgvReporte
            // 
            dgvReporte.AllowUserToAddRows = false;
            dgvReporte.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvReporte.Dock = DockStyle.Fill;
            dgvReporte.Location = new Point(0, 31);
            dgvReporte.Name = "dgvReporte";
            dgvReporte.ReadOnly = true;
            dgvReporte.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReporte.Size = new Size(872, 442);
            dgvReporte.TabIndex = 17;
            // 
            // lblReporte
            // 
            lblReporte.BackColor = Color.Transparent;
            lblReporte.Dock = DockStyle.Top;
            lblReporte.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblReporte.Location = new Point(0, 0);
            lblReporte.Name = "lblReporte";
            lblReporte.Size = new Size(872, 31);
            lblReporte.TabIndex = 16;
            // 
            // btnExcel
            // 
            btnExcel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnExcel.CornerRadius = 8;
            btnExcel.FlatStyle = FlatStyle.Flat;
            btnExcel.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnExcel.Icon = FontAwesome.Sharp.IconChar.FileExcel;
            btnExcel.IconColor = null;
            btnExcel.IconSize = 22;
            btnExcel.Location = new Point(451, 11);
            btnExcel.Name = "btnExcel";
            btnExcel.Size = new Size(75, 36);
            btnExcel.TabIndex = 18;
            btnExcel.Text = "Excel";
            btnExcel.Variant = PromacoHerra.Controls.MaterialButtonVariant.Default;
            // 
            // btnPDF
            // 
            btnPDF.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnPDF.CornerRadius = 8;
            btnPDF.FlatStyle = FlatStyle.Flat;
            btnPDF.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnPDF.Icon = FontAwesome.Sharp.IconChar.FilePdf;
            btnPDF.IconColor = null;
            btnPDF.IconSize = 22;
            btnPDF.Location = new Point(532, 11);
            btnPDF.Name = "btnPDF";
            btnPDF.Size = new Size(75, 36);
            btnPDF.TabIndex = 19;
            btnPDF.Text = "PDF";
            btnPDF.Variant = PromacoHerra.Controls.MaterialButtonVariant.Default;
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel2.BackColor = Color.White;
            panel2.Controls.Add(lblExportar);
            panel2.Controls.Add(lblTitulo);
            panel2.Controls.Add(btnExcel);
            panel2.Controls.Add(btnPDF);
            panel2.Location = new Point(13, 282);
            panel2.Name = "panel2";
            panel2.Padding = new Padding(0, 0, 6, 6);
            panel2.Size = new Size(878, 66);
            panel2.TabIndex = 20;
            // 
            // lblExportar
            // 
            lblExportar.BackColor = Color.White;
            lblExportar.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblExportar.Location = new Point(347, 18);
            lblExportar.Name = "lblExportar";
            lblExportar.Size = new Size(98, 29);
            lblExportar.TabIndex = 20;
            lblExportar.Text = "Exportar";
            lblExportar.Click += label7_Click;
            // 
            // FrmReportes
            // 
            ClientSize = new Size(990, 888);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(panelCards);
            Controls.Add(panelFiltros);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FrmReportes";
            Padding = new Padding(10, 0, 0, 0);
            Text = "Reportes";
            panelFiltros.ResumeLayout(false);
            panelFiltros.PerformLayout();
            panelCards.ResumeLayout(false);
            cardAtrasos.ResumeLayout(false);
            cardDisponibles.ResumeLayout(false);
            cardActivos.ResumeLayout(false);
            cardDanadas.ResumeLayout(false);
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvReporte).EndInit();
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }
        private PromacoHerra.RoundedPanel panelFiltros;
        private Label label6;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private PromacoHerra.Controls.MaterialComboBox cboTipoReporte;
        private PromacoHerra.Controls.EmpleadoPickerControl pickerEmpleado;
        private PromacoHerra.Controls.MaterialButton btnEmpleadoTodos;
        private PromacoHerra.Controls.MaterialComboBox cboHerramienta;
        private DateTimePicker dtpDesde;
        private DateTimePicker dtpHasta;
        private Label lblTitulo;
        private FlowLayoutPanel panelCards;
        private PromacoHerra.RoundedPanel cardAtrasos;
        private Label lblAtrasos;
        private PromacoHerra.RoundedPanel cardDisponibles;
        private Label lblDisponibles;
        private PromacoHerra.RoundedPanel cardActivos;
        private Label lblActivos;
        private PromacoHerra.RoundedPanel cardDanadas;
        private Label lblDanadas;
        private PromacoHerra.RoundedPanel panel1;
        private PromacoHerra.Controls.MaterialButton btnExcel;
        private PromacoHerra.Controls.MaterialButton btnPDF;
        private PromacoHerra.Controls.MaterialButton btnGenerar;
        private PromacoHerra.RoundedPanel panel2;
        private Label lblExportar;
        private Label lblReporte;
        private DataGridView dgvReporte;
    }
}