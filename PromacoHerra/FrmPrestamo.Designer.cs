namespace PromacoHerra
{
    partial class FrmPrestamo
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.TableLayoutPanel tlpMain;

        // ── Panel izquierdo: datos del préstamo ──
        private System.Windows.Forms.Panel pnlIzquierdo;
        private System.Windows.Forms.TableLayoutPanel tlpIzquierdo;
        private System.Windows.Forms.Label lblSecEmpleado;
        private PromacoHerra.Controls.EmpleadoPickerControl pickerEmpleado;
        private System.Windows.Forms.Label sepEmpleado;
        private System.Windows.Forms.Label lblSecAprobacion;
        private System.Windows.Forms.Label lblAprobadoPorTitulo;
        private System.Windows.Forms.Label lblAprobadoPor;
        private System.Windows.Forms.Label lblFechaPrestamo;
        private System.Windows.Forms.DateTimePicker dtpFecha;
        private System.Windows.Forms.Label lblFechaDevolucion;
        private System.Windows.Forms.DateTimePicker dtpFechaDevolucion;
        private System.Windows.Forms.Label lblObservaciones;
        private PromacoHerra.Controls.MaterialTextBox txtObservaciones;
        private System.Windows.Forms.Label sepAprobacion;
        private System.Windows.Forms.Label lblSecSeleccion;
        private System.Windows.Forms.Panel pnlSeleccion;
        private System.Windows.Forms.ListView lvSeleccion;
        private System.Windows.Forms.ColumnHeader colSelHerramienta;
        private System.Windows.Forms.ColumnHeader colSelUnidades;
        private System.Windows.Forms.ColumnHeader colSelCodigos;
        private System.Windows.Forms.Label lblSeleccionVacia;
        private System.Windows.Forms.Label lblTotal;
        private PromacoHerra.Controls.MaterialButton btnGuardar;
        private PromacoHerra.Controls.MaterialButton btnCancelar;

        // ── Panel derecho: catálogo ──
        private System.Windows.Forms.Panel pnlDerecho;
        private System.Windows.Forms.TableLayoutPanel tlpToolbar;
        private PromacoHerra.Controls.MaterialTextBox txtBuscar;
        private System.Windows.Forms.FlowLayoutPanel flpCategorias;
        private System.Windows.Forms.FlowLayoutPanel flpCatalogo;
        private System.Windows.Forms.Label lblCatalogoVacio;

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
            tlpIzquierdo = new TableLayoutPanel();
            lblSecEmpleado = new Label();
            pickerEmpleado = new PromacoHerra.Controls.EmpleadoPickerControl();
            sepEmpleado = new Label();
            lblSecAprobacion = new Label();
            lblAprobadoPorTitulo = new Label();
            lblAprobadoPor = new Label();
            lblFechaPrestamo = new Label();
            dtpFecha = new DateTimePicker();
            lblFechaDevolucion = new Label();
            dtpFechaDevolucion = new DateTimePicker();
            lblObservaciones = new Label();
            txtObservaciones = new PromacoHerra.Controls.MaterialTextBox();
            sepAprobacion = new Label();
            lblSecSeleccion = new Label();
            pnlSeleccion = new Panel();
            lvSeleccion = new ListView();
            colSelHerramienta = new ColumnHeader();
            colSelUnidades = new ColumnHeader();
            colSelCodigos = new ColumnHeader();
            lblSeleccionVacia = new Label();
            lblTotal = new Label();
            btnGuardar = new PromacoHerra.Controls.MaterialButton();
            btnCancelar = new PromacoHerra.Controls.MaterialButton();
            pnlDerecho = new Panel();
            tlpToolbar = new TableLayoutPanel();
            txtBuscar = new PromacoHerra.Controls.MaterialTextBox();
            flpCategorias = new FlowLayoutPanel();
            flpCatalogo = new FlowLayoutPanel();
            lblCatalogoVacio = new Label();
            tlpMain.SuspendLayout();
            pnlIzquierdo.SuspendLayout();
            tlpIzquierdo.SuspendLayout();
            pnlSeleccion.SuspendLayout();
            pnlDerecho.SuspendLayout();
            tlpToolbar.SuspendLayout();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(1200, 60);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Registro de Préstamos";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            //
            // tlpMain
            //
            // Dos columnas: datos del préstamo (300px fijos) | catálogo (resto).
            // No se usa Dock=Left en el panel izquierdo porque ThemeManager
            // pinta los paneles acoplados a la izquierda como sidebar.
            tlpMain.ColumnCount = 2;
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300F));
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
            pnlIzquierdo.Controls.Add(tlpIzquierdo);
            pnlIzquierdo.Dock = DockStyle.Fill;
            pnlIzquierdo.Margin = new Padding(0);
            pnlIzquierdo.Name = "pnlIzquierdo";
            pnlIzquierdo.Padding = new Padding(16, 12, 17, 16);
            pnlIzquierdo.TabIndex = 0;
            //
            // tlpIzquierdo
            //
            tlpIzquierdo.BackColor = Color.White;
            tlpIzquierdo.ColumnCount = 1;
            tlpIzquierdo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpIzquierdo.Controls.Add(lblSecEmpleado, 0, 0);
            tlpIzquierdo.Controls.Add(pickerEmpleado, 0, 1);
            tlpIzquierdo.Controls.Add(sepEmpleado, 0, 2);
            tlpIzquierdo.Controls.Add(lblSecAprobacion, 0, 3);
            tlpIzquierdo.Controls.Add(lblAprobadoPorTitulo, 0, 4);
            tlpIzquierdo.Controls.Add(lblAprobadoPor, 0, 5);
            tlpIzquierdo.Controls.Add(lblFechaPrestamo, 0, 6);
            tlpIzquierdo.Controls.Add(dtpFecha, 0, 7);
            tlpIzquierdo.Controls.Add(lblFechaDevolucion, 0, 8);
            tlpIzquierdo.Controls.Add(dtpFechaDevolucion, 0, 9);
            tlpIzquierdo.Controls.Add(lblObservaciones, 0, 10);
            tlpIzquierdo.Controls.Add(txtObservaciones, 0, 11);
            tlpIzquierdo.Controls.Add(sepAprobacion, 0, 12);
            tlpIzquierdo.Controls.Add(lblSecSeleccion, 0, 13);
            tlpIzquierdo.Controls.Add(pnlSeleccion, 0, 14);
            tlpIzquierdo.Controls.Add(lblTotal, 0, 15);
            tlpIzquierdo.Controls.Add(btnGuardar, 0, 16);
            tlpIzquierdo.Controls.Add(btnCancelar, 0, 17);
            tlpIzquierdo.Dock = DockStyle.Fill;
            tlpIzquierdo.Margin = new Padding(0);
            tlpIzquierdo.Name = "tlpIzquierdo";
            tlpIzquierdo.RowCount = 18;
            tlpIzquierdo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpIzquierdo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpIzquierdo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpIzquierdo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpIzquierdo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpIzquierdo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpIzquierdo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpIzquierdo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpIzquierdo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpIzquierdo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpIzquierdo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpIzquierdo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpIzquierdo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpIzquierdo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpIzquierdo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpIzquierdo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpIzquierdo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpIzquierdo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpIzquierdo.TabIndex = 0;
            //
            // lblSecEmpleado
            //
            lblSecEmpleado.Dock = DockStyle.Fill;
            lblSecEmpleado.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblSecEmpleado.Margin = new Padding(0, 0, 0, 4);
            lblSecEmpleado.Name = "lblSecEmpleado";
            lblSecEmpleado.Size = new Size(267, 20);
            lblSecEmpleado.TabIndex = 0;
            lblSecEmpleado.Text = "EMPLEADO";
            //
            // pickerEmpleado
            //
            pickerEmpleado.Dock = DockStyle.Fill;
            pickerEmpleado.Margin = new Padding(0);
            pickerEmpleado.MinimumSize = new Size(200, 38);
            pickerEmpleado.Name = "pickerEmpleado";
            pickerEmpleado.Size = new Size(267, 38);
            pickerEmpleado.TabIndex = 1;
            //
            // sepEmpleado
            //
            sepEmpleado.Dock = DockStyle.Fill;
            sepEmpleado.Margin = new Padding(0, 14, 0, 14);
            sepEmpleado.Name = "sepEmpleado";
            sepEmpleado.Size = new Size(267, 1);
            sepEmpleado.TabIndex = 2;
            //
            // lblSecAprobacion
            //
            lblSecAprobacion.Dock = DockStyle.Fill;
            lblSecAprobacion.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblSecAprobacion.Margin = new Padding(0, 0, 0, 6);
            lblSecAprobacion.Name = "lblSecAprobacion";
            lblSecAprobacion.Size = new Size(267, 20);
            lblSecAprobacion.TabIndex = 3;
            lblSecAprobacion.Text = "APROBACIÓN Y FECHAS";
            //
            // lblAprobadoPorTitulo
            //
            lblAprobadoPorTitulo.Dock = DockStyle.Fill;
            lblAprobadoPorTitulo.Font = new Font("Segoe UI", 9F);
            lblAprobadoPorTitulo.Margin = new Padding(0);
            lblAprobadoPorTitulo.Name = "lblAprobadoPorTitulo";
            lblAprobadoPorTitulo.Size = new Size(267, 20);
            lblAprobadoPorTitulo.TabIndex = 4;
            lblAprobadoPorTitulo.Text = "Aprobado por";
            //
            // lblAprobadoPor
            //
            lblAprobadoPor.AutoEllipsis = true;
            lblAprobadoPor.Dock = DockStyle.Fill;
            lblAprobadoPor.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblAprobadoPor.Margin = new Padding(0, 0, 0, 8);
            lblAprobadoPor.Name = "lblAprobadoPor";
            lblAprobadoPor.Size = new Size(267, 24);
            lblAprobadoPor.TabIndex = 5;
            lblAprobadoPor.Text = "—";
            //
            // lblFechaPrestamo
            //
            lblFechaPrestamo.Dock = DockStyle.Fill;
            lblFechaPrestamo.Font = new Font("Segoe UI", 9F);
            lblFechaPrestamo.Margin = new Padding(0);
            lblFechaPrestamo.Name = "lblFechaPrestamo";
            lblFechaPrestamo.Size = new Size(267, 20);
            lblFechaPrestamo.TabIndex = 6;
            lblFechaPrestamo.Text = "Fecha de préstamo";
            //
            // dtpFecha
            //
            dtpFecha.CustomFormat = "dd/MM/yyyy";
            dtpFecha.Dock = DockStyle.Fill;
            dtpFecha.Enabled = false;
            dtpFecha.Format = DateTimePickerFormat.Custom;
            dtpFecha.Margin = new Padding(0, 2, 0, 8);
            dtpFecha.Name = "dtpFecha";
            dtpFecha.Size = new Size(267, 27);
            dtpFecha.TabIndex = 7;
            //
            // lblFechaDevolucion
            //
            lblFechaDevolucion.Dock = DockStyle.Fill;
            lblFechaDevolucion.Font = new Font("Segoe UI", 9F);
            lblFechaDevolucion.Margin = new Padding(0);
            lblFechaDevolucion.Name = "lblFechaDevolucion";
            lblFechaDevolucion.Size = new Size(267, 20);
            lblFechaDevolucion.TabIndex = 8;
            lblFechaDevolucion.Text = "Devolución esperada";
            //
            // dtpFechaDevolucion
            //
            dtpFechaDevolucion.CustomFormat = "dd/MM/yyyy";
            dtpFechaDevolucion.Dock = DockStyle.Fill;
            dtpFechaDevolucion.Format = DateTimePickerFormat.Custom;
            dtpFechaDevolucion.Margin = new Padding(0, 2, 0, 8);
            dtpFechaDevolucion.Name = "dtpFechaDevolucion";
            dtpFechaDevolucion.Size = new Size(267, 27);
            dtpFechaDevolucion.TabIndex = 9;
            //
            // lblObservaciones
            //
            lblObservaciones.Dock = DockStyle.Fill;
            lblObservaciones.Font = new Font("Segoe UI", 9F);
            lblObservaciones.Margin = new Padding(0);
            lblObservaciones.Name = "lblObservaciones";
            lblObservaciones.Size = new Size(267, 20);
            lblObservaciones.TabIndex = 10;
            lblObservaciones.Text = "Observación";
            //
            // txtObservaciones
            //
            txtObservaciones.BackColor = Color.White;
            txtObservaciones.Dock = DockStyle.Fill;
            txtObservaciones.Margin = new Padding(0);
            txtObservaciones.Name = "txtObservaciones";
            txtObservaciones.PlaceholderText = "Opcional";
            txtObservaciones.Size = new Size(267, 38);
            txtObservaciones.TabIndex = 11;
            //
            // sepAprobacion
            //
            sepAprobacion.Dock = DockStyle.Fill;
            sepAprobacion.Margin = new Padding(0, 14, 0, 14);
            sepAprobacion.Name = "sepAprobacion";
            sepAprobacion.Size = new Size(267, 1);
            sepAprobacion.TabIndex = 12;
            //
            // lblSecSeleccion
            //
            lblSecSeleccion.Dock = DockStyle.Fill;
            lblSecSeleccion.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblSecSeleccion.Margin = new Padding(0, 0, 0, 6);
            lblSecSeleccion.Name = "lblSecSeleccion";
            lblSecSeleccion.Size = new Size(267, 20);
            lblSecSeleccion.TabIndex = 13;
            lblSecSeleccion.Text = "HERRAMIENTAS SELECCIONADAS";
            //
            // pnlSeleccion
            //
            pnlSeleccion.Controls.Add(lblSeleccionVacia);
            pnlSeleccion.Controls.Add(lvSeleccion);
            pnlSeleccion.Dock = DockStyle.Fill;
            pnlSeleccion.Margin = new Padding(0);
            pnlSeleccion.MinimumSize = new Size(0, 90);
            pnlSeleccion.Name = "pnlSeleccion";
            pnlSeleccion.TabIndex = 14;
            //
            // lvSeleccion
            //
            lvSeleccion.BorderStyle = BorderStyle.None;
            lvSeleccion.Columns.AddRange(new ColumnHeader[] { colSelHerramienta, colSelUnidades, colSelCodigos });
            lvSeleccion.Dock = DockStyle.Fill;
            lvSeleccion.Font = new Font("Segoe UI", 9F);
            lvSeleccion.FullRowSelect = true;
            lvSeleccion.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            lvSeleccion.MultiSelect = false;
            lvSeleccion.Name = "lvSeleccion";
            lvSeleccion.ShowItemToolTips = true;
            lvSeleccion.TabIndex = 0;
            lvSeleccion.UseCompatibleStateImageBehavior = false;
            lvSeleccion.View = View.Details;
            //
            // colSelHerramienta
            //
            colSelHerramienta.Text = "Herramienta";
            colSelHerramienta.Width = 112;
            //
            // colSelUnidades
            //
            colSelUnidades.Text = "Unidades";
            colSelUnidades.TextAlign = HorizontalAlignment.Center;
            colSelUnidades.Width = 66;
            //
            // colSelCodigos
            //
            colSelCodigos.Text = "Códigos";
            colSelCodigos.Width = 86;
            //
            // lblSeleccionVacia
            //
            lblSeleccionVacia.BackColor = Color.White;
            lblSeleccionVacia.Dock = DockStyle.Fill;
            lblSeleccionVacia.Font = new Font("Segoe UI", 9.5F);
            lblSeleccionVacia.Name = "lblSeleccionVacia";
            lblSeleccionVacia.TabIndex = 1;
            lblSeleccionVacia.Text = "Selecciona herramientas del catálogo";
            lblSeleccionVacia.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblTotal
            //
            lblTotal.Dock = DockStyle.Fill;
            lblTotal.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTotal.Margin = new Padding(0, 6, 0, 10);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(267, 24);
            lblTotal.TabIndex = 15;
            lblTotal.Text = "Total: 0 unidades";
            lblTotal.TextAlign = ContentAlignment.MiddleLeft;
            //
            // btnGuardar
            //
            btnGuardar.Dock = DockStyle.Fill;
            btnGuardar.Enabled = false;
            btnGuardar.Icon = FontAwesome.Sharp.IconChar.Save;
            btnGuardar.IconSize = 20;
            btnGuardar.Margin = new Padding(0, 0, 0, 8);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(267, 42);
            btnGuardar.TabIndex = 16;
            btnGuardar.Text = "Guardar préstamo";
            btnGuardar.Variant = PromacoHerra.Controls.MaterialButtonVariant.Primary;
            //
            // btnCancelar
            //
            btnCancelar.Dock = DockStyle.Fill;
            btnCancelar.Icon = FontAwesome.Sharp.IconChar.Close;
            btnCancelar.IconSize = 20;
            btnCancelar.Margin = new Padding(0);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(267, 40);
            btnCancelar.TabIndex = 17;
            btnCancelar.Text = "Cancelar";
            btnCancelar.Variant = PromacoHerra.Controls.MaterialButtonVariant.Secondary;
            //
            // pnlDerecho
            //
            pnlDerecho.Controls.Add(flpCatalogo);
            pnlDerecho.Controls.Add(lblCatalogoVacio);
            pnlDerecho.Controls.Add(tlpToolbar);
            pnlDerecho.Dock = DockStyle.Fill;
            pnlDerecho.Margin = new Padding(0);
            pnlDerecho.Name = "pnlDerecho";
            pnlDerecho.Padding = new Padding(20, 4, 8, 0);
            pnlDerecho.TabIndex = 1;
            //
            // tlpToolbar
            //
            tlpToolbar.AutoSize = true;
            tlpToolbar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpToolbar.ColumnCount = 1;
            tlpToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpToolbar.Controls.Add(txtBuscar, 0, 0);
            tlpToolbar.Controls.Add(flpCategorias, 0, 1);
            tlpToolbar.Dock = DockStyle.Top;
            tlpToolbar.Margin = new Padding(0);
            tlpToolbar.Name = "tlpToolbar";
            tlpToolbar.Padding = new Padding(0, 0, 12, 8);
            tlpToolbar.RowCount = 2;
            tlpToolbar.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpToolbar.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpToolbar.TabIndex = 0;
            //
            // txtBuscar
            //
            txtBuscar.BackColor = Color.White;
            txtBuscar.Dock = DockStyle.Fill;
            txtBuscar.Margin = new Padding(0, 0, 0, 10);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.PlaceholderText = "Buscar por nombre, código, marca o categoría...";
            txtBuscar.Size = new Size(860, 38);
            txtBuscar.TabIndex = 0;
            //
            // flpCategorias
            //
            flpCategorias.AutoSize = true;
            flpCategorias.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpCategorias.Dock = DockStyle.Fill;
            flpCategorias.Margin = new Padding(0);
            flpCategorias.Name = "flpCategorias";
            flpCategorias.TabIndex = 1;
            //
            // flpCatalogo
            //
            flpCatalogo.AutoScroll = true;
            flpCatalogo.Dock = DockStyle.Fill;
            flpCatalogo.Name = "flpCatalogo";
            flpCatalogo.Padding = new Padding(0, 4, 0, 12);
            flpCatalogo.TabIndex = 1;
            //
            // lblCatalogoVacio
            //
            lblCatalogoVacio.Dock = DockStyle.Fill;
            lblCatalogoVacio.Font = new Font("Segoe UI", 11F);
            lblCatalogoVacio.Name = "lblCatalogoVacio";
            lblCatalogoVacio.TabIndex = 2;
            lblCatalogoVacio.Text = "No hay herramientas que coincidan con la búsqueda.";
            lblCatalogoVacio.TextAlign = ContentAlignment.MiddleCenter;
            lblCatalogoVacio.Visible = false;
            //
            // FrmPrestamo
            //
            ClientSize = new Size(1200, 720);
            Controls.Add(tlpMain);
            Controls.Add(lblTitulo);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FrmPrestamo";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Préstamos";
            tlpMain.ResumeLayout(false);
            pnlIzquierdo.ResumeLayout(false);
            tlpIzquierdo.ResumeLayout(false);
            pnlSeleccion.ResumeLayout(false);
            pnlDerecho.ResumeLayout(false);
            pnlDerecho.PerformLayout();
            tlpToolbar.ResumeLayout(false);
            tlpToolbar.PerformLayout();
            ResumeLayout(false);
        }
    }
}
