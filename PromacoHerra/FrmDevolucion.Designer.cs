namespace PromacoHerra
{
    partial class FrmDevolucion
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.TableLayoutPanel tlpMain;

        // ── Panel izquierdo: préstamos activos ──
        private System.Windows.Forms.Panel pnlIzquierdo;
        private System.Windows.Forms.TableLayoutPanel tlpIzqHeader;
        private System.Windows.Forms.Label lblSecPrestamos;
        private PromacoHerra.Controls.MaterialTextBox txtBuscar;
        private System.Windows.Forms.FlowLayoutPanel flpPrestamos;
        private System.Windows.Forms.Label lblSinPrestamos;

        // ── Panel derecho: detalle ──
        private System.Windows.Forms.Panel pnlDerecho;
        private System.Windows.Forms.Label lblVacio;
        private System.Windows.Forms.TableLayoutPanel tlpDetalle;
        private PromacoHerra.RoundedPanel pnlResumen;
        private PromacoHerra.Controls.AvatarControl avatarEmpleado;
        private System.Windows.Forms.Label lblNombreEmpleado;
        private System.Windows.Forms.Label lblMetaEmpleado;
        private System.Windows.Forms.TableLayoutPanel tlpStats;
        private PromacoHerra.RoundedPanel pnlStatPrestadas;
        private System.Windows.Forms.Label lblStatPrestadas;
        private System.Windows.Forms.Label lblStatPrestadasCap;
        private PromacoHerra.RoundedPanel pnlStatSeleccionadas;
        private System.Windows.Forms.Label lblStatSeleccionadas;
        private System.Windows.Forms.Label lblStatSeleccionadasCap;
        private PromacoHerra.RoundedPanel pnlStatPlazo;
        private System.Windows.Forms.Label lblStatPlazo;
        private System.Windows.Forms.Label lblStatPlazoCap;
        private PromacoHerra.RoundedPanel pnlAlerta;
        private System.Windows.Forms.Label lblAlerta;
        private System.Windows.Forms.Panel pnlHerrHeader;
        private System.Windows.Forms.Label lblHerrTitulo;
        private System.Windows.Forms.LinkLabel lnkSeleccionarTodo;
        private System.Windows.Forms.FlowLayoutPanel flpHerramientas;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Label lblResumenSeleccion;
        private PromacoHerra.Controls.MaterialButton btnDevolverSeleccionadas;
        private PromacoHerra.Controls.MaterialButton btnDevolverTodo;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitulo = new Label();
            tlpMain = new TableLayoutPanel();
            pnlIzquierdo = new Panel();
            tlpIzqHeader = new TableLayoutPanel();
            lblSecPrestamos = new Label();
            txtBuscar = new PromacoHerra.Controls.MaterialTextBox();
            flpPrestamos = new FlowLayoutPanel();
            lblSinPrestamos = new Label();
            pnlDerecho = new Panel();
            lblVacio = new Label();
            tlpDetalle = new TableLayoutPanel();
            pnlResumen = new PromacoHerra.RoundedPanel();
            avatarEmpleado = new PromacoHerra.Controls.AvatarControl();
            lblNombreEmpleado = new Label();
            lblMetaEmpleado = new Label();
            tlpStats = new TableLayoutPanel();
            pnlStatPrestadas = new PromacoHerra.RoundedPanel();
            lblStatPrestadas = new Label();
            lblStatPrestadasCap = new Label();
            pnlStatSeleccionadas = new PromacoHerra.RoundedPanel();
            lblStatSeleccionadas = new Label();
            lblStatSeleccionadasCap = new Label();
            pnlStatPlazo = new PromacoHerra.RoundedPanel();
            lblStatPlazo = new Label();
            lblStatPlazoCap = new Label();
            pnlAlerta = new PromacoHerra.RoundedPanel();
            lblAlerta = new Label();
            pnlHerrHeader = new Panel();
            lblHerrTitulo = new Label();
            lnkSeleccionarTodo = new LinkLabel();
            flpHerramientas = new FlowLayoutPanel();
            pnlFooter = new Panel();
            lblResumenSeleccion = new Label();
            btnDevolverSeleccionadas = new PromacoHerra.Controls.MaterialButton();
            btnDevolverTodo = new PromacoHerra.Controls.MaterialButton();
            tlpMain.SuspendLayout();
            pnlIzquierdo.SuspendLayout();
            tlpIzqHeader.SuspendLayout();
            pnlDerecho.SuspendLayout();
            tlpDetalle.SuspendLayout();
            pnlResumen.SuspendLayout();
            tlpStats.SuspendLayout();
            pnlStatPrestadas.SuspendLayout();
            pnlStatSeleccionadas.SuspendLayout();
            pnlStatPlazo.SuspendLayout();
            pnlAlerta.SuspendLayout();
            pnlHerrHeader.SuspendLayout();
            pnlFooter.SuspendLayout();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(1200, 60);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Devolución de Herramientas";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            //
            // tlpMain
            //
            // Dos columnas: préstamos activos (320px fijos) | detalle (resto).
            // No se usa Dock=Left en el panel izquierdo porque ThemeManager
            // pinta los paneles acoplados a la izquierda como sidebar.
            tlpMain.ColumnCount = 2;
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 320F));
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpMain.Controls.Add(pnlIzquierdo, 0, 0);
            tlpMain.Controls.Add(pnlDerecho, 1, 0);
            tlpMain.Dock = DockStyle.Fill;
            tlpMain.Margin = new Padding(0);
            tlpMain.Name = "tlpMain";
            tlpMain.RowCount = 1;
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpMain.TabIndex = 1;
            //
            // pnlIzquierdo
            //
            pnlIzquierdo.BackColor = Color.White;
            pnlIzquierdo.Controls.Add(flpPrestamos);
            pnlIzquierdo.Controls.Add(lblSinPrestamos);
            pnlIzquierdo.Controls.Add(tlpIzqHeader);
            pnlIzquierdo.Dock = DockStyle.Fill;
            pnlIzquierdo.Margin = new Padding(0);
            pnlIzquierdo.Name = "pnlIzquierdo";
            pnlIzquierdo.Padding = new Padding(0, 0, 1, 0);
            pnlIzquierdo.TabIndex = 0;
            //
            // tlpIzqHeader
            //
            tlpIzqHeader.AutoSize = true;
            tlpIzqHeader.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpIzqHeader.BackColor = Color.White;
            tlpIzqHeader.ColumnCount = 1;
            tlpIzqHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpIzqHeader.Controls.Add(lblSecPrestamos, 0, 0);
            tlpIzqHeader.Controls.Add(txtBuscar, 0, 1);
            tlpIzqHeader.Dock = DockStyle.Top;
            tlpIzqHeader.Margin = new Padding(0);
            tlpIzqHeader.Name = "tlpIzqHeader";
            tlpIzqHeader.Padding = new Padding(16, 12, 16, 10);
            tlpIzqHeader.RowCount = 2;
            tlpIzqHeader.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpIzqHeader.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpIzqHeader.TabIndex = 0;
            //
            // lblSecPrestamos
            //
            lblSecPrestamos.Dock = DockStyle.Fill;
            lblSecPrestamos.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblSecPrestamos.Margin = new Padding(0, 0, 0, 6);
            lblSecPrestamos.Name = "lblSecPrestamos";
            lblSecPrestamos.Size = new Size(287, 20);
            lblSecPrestamos.TabIndex = 0;
            lblSecPrestamos.Text = "PRÉSTAMOS ACTIVOS";
            //
            // txtBuscar
            //
            txtBuscar.BackColor = Color.White;
            txtBuscar.Dock = DockStyle.Fill;
            txtBuscar.Margin = new Padding(0);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.PlaceholderText = "Buscar empleado o código (PR-0001)...";
            txtBuscar.Size = new Size(287, 38);
            txtBuscar.TabIndex = 1;
            //
            // flpPrestamos
            //
            flpPrestamos.AutoScroll = true;
            flpPrestamos.BackColor = Color.White;
            flpPrestamos.Dock = DockStyle.Fill;
            flpPrestamos.FlowDirection = FlowDirection.TopDown;
            flpPrestamos.Margin = new Padding(0);
            flpPrestamos.Name = "flpPrestamos";
            flpPrestamos.TabIndex = 1;
            flpPrestamos.WrapContents = false;
            //
            // lblSinPrestamos
            //
            lblSinPrestamos.Dock = DockStyle.Fill;
            lblSinPrestamos.Font = new Font("Segoe UI", 9.5F);
            lblSinPrestamos.Name = "lblSinPrestamos";
            lblSinPrestamos.Padding = new Padding(16);
            lblSinPrestamos.TabIndex = 2;
            lblSinPrestamos.Text = "No hay préstamos activos.";
            lblSinPrestamos.TextAlign = ContentAlignment.TopCenter;
            lblSinPrestamos.Visible = false;
            //
            // pnlDerecho
            //
            pnlDerecho.Controls.Add(tlpDetalle);
            pnlDerecho.Controls.Add(lblVacio);
            pnlDerecho.Dock = DockStyle.Fill;
            pnlDerecho.Margin = new Padding(0);
            pnlDerecho.Name = "pnlDerecho";
            pnlDerecho.Padding = new Padding(20, 4, 20, 12);
            pnlDerecho.TabIndex = 1;
            //
            // lblVacio
            //
            lblVacio.Dock = DockStyle.Fill;
            lblVacio.Font = new Font("Segoe UI", 11F);
            lblVacio.Name = "lblVacio";
            lblVacio.TabIndex = 0;
            lblVacio.Text = "Selecciona un préstamo para ver su detalle";
            lblVacio.TextAlign = ContentAlignment.MiddleCenter;
            //
            // tlpDetalle
            //
            tlpDetalle.ColumnCount = 1;
            tlpDetalle.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpDetalle.Controls.Add(pnlResumen, 0, 0);
            tlpDetalle.Controls.Add(pnlAlerta, 0, 1);
            tlpDetalle.Controls.Add(pnlHerrHeader, 0, 2);
            tlpDetalle.Controls.Add(flpHerramientas, 0, 3);
            tlpDetalle.Controls.Add(pnlFooter, 0, 4);
            tlpDetalle.Dock = DockStyle.Fill;
            tlpDetalle.Margin = new Padding(0);
            tlpDetalle.Name = "tlpDetalle";
            tlpDetalle.RowCount = 5;
            tlpDetalle.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpDetalle.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpDetalle.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpDetalle.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpDetalle.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpDetalle.TabIndex = 1;
            tlpDetalle.Visible = false;
            //
            // pnlResumen
            //
            pnlResumen.Controls.Add(avatarEmpleado);
            pnlResumen.Controls.Add(lblNombreEmpleado);
            pnlResumen.Controls.Add(lblMetaEmpleado);
            pnlResumen.Controls.Add(tlpStats);
            pnlResumen.Dock = DockStyle.Fill;
            pnlResumen.Margin = new Padding(0, 0, 0, 12);
            pnlResumen.Name = "pnlResumen";
            pnlResumen.Size = new Size(860, 158);
            pnlResumen.TabIndex = 0;
            //
            // avatarEmpleado
            //
            avatarEmpleado.Location = new Point(20, 18);
            avatarEmpleado.Name = "avatarEmpleado";
            avatarEmpleado.Size = new Size(48, 48);
            avatarEmpleado.TabIndex = 0;
            //
            // lblNombreEmpleado
            //
            lblNombreEmpleado.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblNombreEmpleado.AutoEllipsis = true;
            lblNombreEmpleado.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblNombreEmpleado.Location = new Point(80, 16);
            lblNombreEmpleado.Name = "lblNombreEmpleado";
            lblNombreEmpleado.Size = new Size(756, 28);
            lblNombreEmpleado.TabIndex = 1;
            lblNombreEmpleado.Text = "Empleado";
            //
            // lblMetaEmpleado
            //
            lblMetaEmpleado.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblMetaEmpleado.AutoEllipsis = true;
            lblMetaEmpleado.Font = new Font("Segoe UI", 9.5F);
            lblMetaEmpleado.Location = new Point(80, 44);
            lblMetaEmpleado.Name = "lblMetaEmpleado";
            lblMetaEmpleado.Size = new Size(756, 22);
            lblMetaEmpleado.TabIndex = 2;
            lblMetaEmpleado.Text = "Departamento · PR-0000";
            //
            // tlpStats
            //
            tlpStats.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tlpStats.ColumnCount = 3;
            tlpStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tlpStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tlpStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));
            tlpStats.Controls.Add(pnlStatPrestadas, 0, 0);
            tlpStats.Controls.Add(pnlStatSeleccionadas, 1, 0);
            tlpStats.Controls.Add(pnlStatPlazo, 2, 0);
            tlpStats.Location = new Point(20, 80);
            tlpStats.Margin = new Padding(0);
            tlpStats.Name = "tlpStats";
            tlpStats.RowCount = 1;
            tlpStats.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpStats.Size = new Size(816, 60);
            tlpStats.TabIndex = 3;
            //
            // pnlStatPrestadas
            //
            pnlStatPrestadas.Controls.Add(lblStatPrestadas);
            pnlStatPrestadas.Controls.Add(lblStatPrestadasCap);
            pnlStatPrestadas.Dock = DockStyle.Fill;
            pnlStatPrestadas.Margin = new Padding(0, 0, 10, 0);
            pnlStatPrestadas.Name = "pnlStatPrestadas";
            pnlStatPrestadas.TabIndex = 0;
            //
            // lblStatPrestadas
            //
            lblStatPrestadas.AutoSize = true;
            lblStatPrestadas.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblStatPrestadas.Location = new Point(12, 4);
            lblStatPrestadas.Name = "lblStatPrestadas";
            lblStatPrestadas.TabIndex = 0;
            lblStatPrestadas.Text = "0";
            //
            // lblStatPrestadasCap
            //
            lblStatPrestadasCap.AutoSize = true;
            lblStatPrestadasCap.Font = new Font("Segoe UI", 9F);
            lblStatPrestadasCap.Location = new Point(12, 34);
            lblStatPrestadasCap.Name = "lblStatPrestadasCap";
            lblStatPrestadasCap.TabIndex = 1;
            lblStatPrestadasCap.Text = "Prestadas";
            //
            // pnlStatSeleccionadas
            //
            pnlStatSeleccionadas.Controls.Add(lblStatSeleccionadas);
            pnlStatSeleccionadas.Controls.Add(lblStatSeleccionadasCap);
            pnlStatSeleccionadas.Dock = DockStyle.Fill;
            pnlStatSeleccionadas.Margin = new Padding(0, 0, 10, 0);
            pnlStatSeleccionadas.Name = "pnlStatSeleccionadas";
            pnlStatSeleccionadas.TabIndex = 1;
            //
            // lblStatSeleccionadas
            //
            lblStatSeleccionadas.AutoSize = true;
            lblStatSeleccionadas.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblStatSeleccionadas.Location = new Point(12, 4);
            lblStatSeleccionadas.Name = "lblStatSeleccionadas";
            lblStatSeleccionadas.TabIndex = 0;
            lblStatSeleccionadas.Text = "0";
            //
            // lblStatSeleccionadasCap
            //
            lblStatSeleccionadasCap.AutoSize = true;
            lblStatSeleccionadasCap.Font = new Font("Segoe UI", 9F);
            lblStatSeleccionadasCap.Location = new Point(12, 34);
            lblStatSeleccionadasCap.Name = "lblStatSeleccionadasCap";
            lblStatSeleccionadasCap.TabIndex = 1;
            lblStatSeleccionadasCap.Text = "Seleccionadas";
            //
            // pnlStatPlazo
            //
            pnlStatPlazo.Controls.Add(lblStatPlazo);
            pnlStatPlazo.Controls.Add(lblStatPlazoCap);
            pnlStatPlazo.Dock = DockStyle.Fill;
            pnlStatPlazo.Margin = new Padding(0);
            pnlStatPlazo.Name = "pnlStatPlazo";
            pnlStatPlazo.TabIndex = 2;
            //
            // lblStatPlazo
            //
            lblStatPlazo.AutoSize = true;
            lblStatPlazo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblStatPlazo.Location = new Point(12, 7);
            lblStatPlazo.Name = "lblStatPlazo";
            lblStatPlazo.TabIndex = 0;
            lblStatPlazo.Text = "Vence en 0 días";
            //
            // lblStatPlazoCap
            //
            lblStatPlazoCap.AutoSize = true;
            lblStatPlazoCap.Font = new Font("Segoe UI", 9F);
            lblStatPlazoCap.Location = new Point(12, 34);
            lblStatPlazoCap.Name = "lblStatPlazoCap";
            lblStatPlazoCap.TabIndex = 1;
            lblStatPlazoCap.Text = "Plazo";
            //
            // pnlAlerta
            //
            pnlAlerta.Controls.Add(lblAlerta);
            pnlAlerta.Dock = DockStyle.Fill;
            pnlAlerta.Margin = new Padding(0, 0, 0, 12);
            pnlAlerta.Name = "pnlAlerta";
            pnlAlerta.Size = new Size(860, 46);
            pnlAlerta.TabIndex = 1;
            pnlAlerta.Visible = false;
            //
            // lblAlerta
            //
            lblAlerta.Dock = DockStyle.Fill;
            lblAlerta.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblAlerta.Name = "lblAlerta";
            lblAlerta.Padding = new Padding(14, 0, 14, 0);
            lblAlerta.TabIndex = 0;
            lblAlerta.Text = "⚠  Este préstamo está vencido.";
            lblAlerta.TextAlign = ContentAlignment.MiddleLeft;
            //
            // pnlHerrHeader
            //
            pnlHerrHeader.Controls.Add(lblHerrTitulo);
            pnlHerrHeader.Controls.Add(lnkSeleccionarTodo);
            pnlHerrHeader.Dock = DockStyle.Fill;
            pnlHerrHeader.Margin = new Padding(0, 0, 0, 6);
            pnlHerrHeader.Name = "pnlHerrHeader";
            pnlHerrHeader.Size = new Size(860, 30);
            pnlHerrHeader.TabIndex = 2;
            //
            // lblHerrTitulo
            //
            lblHerrTitulo.AutoSize = true;
            lblHerrTitulo.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            lblHerrTitulo.Location = new Point(0, 4);
            lblHerrTitulo.Name = "lblHerrTitulo";
            lblHerrTitulo.TabIndex = 0;
            lblHerrTitulo.Text = "Herramientas en préstamo";
            //
            // lnkSeleccionarTodo
            //
            lnkSeleccionarTodo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lnkSeleccionarTodo.Font = new Font("Segoe UI", 9.5F);
            lnkSeleccionarTodo.LinkBehavior = LinkBehavior.HoverUnderline;
            lnkSeleccionarTodo.Location = new Point(660, 4);
            lnkSeleccionarTodo.Name = "lnkSeleccionarTodo";
            lnkSeleccionarTodo.Size = new Size(200, 22);
            lnkSeleccionarTodo.TabIndex = 1;
            lnkSeleccionarTodo.TabStop = true;
            lnkSeleccionarTodo.Text = "Seleccionar todo";
            lnkSeleccionarTodo.TextAlign = ContentAlignment.MiddleRight;
            //
            // flpHerramientas
            //
            flpHerramientas.AutoScroll = true;
            flpHerramientas.Dock = DockStyle.Fill;
            flpHerramientas.FlowDirection = FlowDirection.TopDown;
            flpHerramientas.Margin = new Padding(0);
            flpHerramientas.Name = "flpHerramientas";
            flpHerramientas.TabIndex = 3;
            flpHerramientas.WrapContents = false;
            //
            // pnlFooter
            //
            pnlFooter.Controls.Add(lblResumenSeleccion);
            pnlFooter.Controls.Add(btnDevolverSeleccionadas);
            pnlFooter.Controls.Add(btnDevolverTodo);
            pnlFooter.Dock = DockStyle.Fill;
            pnlFooter.Margin = new Padding(0);
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Size = new Size(860, 60);
            pnlFooter.TabIndex = 4;
            //
            // lblResumenSeleccion
            //
            lblResumenSeleccion.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblResumenSeleccion.AutoEllipsis = true;
            lblResumenSeleccion.Font = new Font("Segoe UI", 10F);
            lblResumenSeleccion.Location = new Point(0, 20);
            lblResumenSeleccion.Name = "lblResumenSeleccion";
            lblResumenSeleccion.Size = new Size(470, 24);
            lblResumenSeleccion.TabIndex = 0;
            lblResumenSeleccion.Text = "Selecciona las herramientas a devolver";
            lblResumenSeleccion.TextAlign = ContentAlignment.MiddleLeft;
            //
            // btnDevolverSeleccionadas
            //
            btnDevolverSeleccionadas.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDevolverSeleccionadas.Enabled = false;
            btnDevolverSeleccionadas.Location = new Point(480, 12);
            btnDevolverSeleccionadas.Name = "btnDevolverSeleccionadas";
            btnDevolverSeleccionadas.Size = new Size(210, 42);
            btnDevolverSeleccionadas.TabIndex = 1;
            btnDevolverSeleccionadas.Text = "Devolver seleccionadas";
            btnDevolverSeleccionadas.Variant = PromacoHerra.Controls.MaterialButtonVariant.Secondary;
            //
            // btnDevolverTodo
            //
            btnDevolverTodo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDevolverTodo.Location = new Point(700, 12);
            btnDevolverTodo.Name = "btnDevolverTodo";
            btnDevolverTodo.Size = new Size(160, 42);
            btnDevolverTodo.TabIndex = 2;
            btnDevolverTodo.Text = "Devolver todo";
            btnDevolverTodo.Variant = PromacoHerra.Controls.MaterialButtonVariant.Success;
            //
            // FrmDevolucion
            //
            ClientSize = new Size(1200, 720);
            Controls.Add(tlpMain);
            Controls.Add(lblTitulo);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FrmDevolucion";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Devoluciones";
            tlpMain.ResumeLayout(false);
            pnlIzquierdo.ResumeLayout(false);
            pnlIzquierdo.PerformLayout();
            tlpIzqHeader.ResumeLayout(false);
            pnlDerecho.ResumeLayout(false);
            tlpDetalle.ResumeLayout(false);
            pnlResumen.ResumeLayout(false);
            tlpStats.ResumeLayout(false);
            pnlStatPrestadas.ResumeLayout(false);
            pnlStatPrestadas.PerformLayout();
            pnlStatSeleccionadas.ResumeLayout(false);
            pnlStatSeleccionadas.PerformLayout();
            pnlStatPlazo.ResumeLayout(false);
            pnlStatPlazo.PerformLayout();
            pnlAlerta.ResumeLayout(false);
            pnlHerrHeader.ResumeLayout(false);
            pnlHerrHeader.PerformLayout();
            pnlFooter.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}
