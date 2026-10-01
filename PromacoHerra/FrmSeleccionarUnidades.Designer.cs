namespace PromacoHerra
{
    partial class FrmSeleccionarUnidades
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.Label lblMeta;
        private System.Windows.Forms.Panel pnlFiltro;
        private PromacoHerra.Controls.MaterialTextBox txtFiltro;
        private System.Windows.Forms.Panel pnlLista;
        private System.Windows.Forms.ListView lvUnidades;
        private System.Windows.Forms.ColumnHeader colCodigo;
        private System.Windows.Forms.ColumnHeader colUbicacion;
        private System.Windows.Forms.ColumnHeader colEstado;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Label lblSeleccionadas;
        private PromacoHerra.Controls.MaterialButton btnConfirmar;
        private PromacoHerra.Controls.MaterialButton btnCancelar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pnlHeader = new Panel();
            lblNombre = new Label();
            lblMeta = new Label();
            pnlFiltro = new Panel();
            txtFiltro = new PromacoHerra.Controls.MaterialTextBox();
            pnlLista = new Panel();
            lvUnidades = new ListView();
            colCodigo = new ColumnHeader();
            colUbicacion = new ColumnHeader();
            colEstado = new ColumnHeader();
            pnlFooter = new Panel();
            lblSeleccionadas = new Label();
            btnConfirmar = new PromacoHerra.Controls.MaterialButton();
            btnCancelar = new PromacoHerra.Controls.MaterialButton();
            pnlHeader.SuspendLayout();
            pnlFiltro.SuspendLayout();
            pnlLista.SuspendLayout();
            pnlFooter.SuspendLayout();
            SuspendLayout();
            //
            // pnlHeader
            //
            pnlHeader.BackColor = Color.White;
            pnlHeader.Controls.Add(lblNombre);
            pnlHeader.Controls.Add(lblMeta);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(540, 78);
            pnlHeader.TabIndex = 0;
            //
            // lblNombre
            //
            lblNombre.AutoEllipsis = true;
            lblNombre.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblNombre.Location = new Point(20, 12);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(500, 30);
            lblNombre.TabIndex = 0;
            lblNombre.Text = "Herramienta";
            //
            // lblMeta
            //
            lblMeta.AutoEllipsis = true;
            lblMeta.Font = new Font("Segoe UI", 9.5F);
            lblMeta.Location = new Point(20, 44);
            lblMeta.Name = "lblMeta";
            lblMeta.Size = new Size(500, 22);
            lblMeta.TabIndex = 1;
            lblMeta.Text = "Código · Categoría · Marca";
            //
            // pnlFiltro
            //
            pnlFiltro.Controls.Add(txtFiltro);
            pnlFiltro.Dock = DockStyle.Top;
            pnlFiltro.Name = "pnlFiltro";
            pnlFiltro.Padding = new Padding(20, 12, 20, 8);
            pnlFiltro.Size = new Size(540, 58);
            pnlFiltro.TabIndex = 1;
            //
            // txtFiltro
            //
            txtFiltro.BackColor = Color.White;
            txtFiltro.Dock = DockStyle.Fill;
            txtFiltro.Name = "txtFiltro";
            txtFiltro.PlaceholderText = "Filtrar por código...";
            txtFiltro.TabIndex = 0;
            //
            // pnlLista
            //
            pnlLista.Controls.Add(lvUnidades);
            pnlLista.Dock = DockStyle.Fill;
            pnlLista.Name = "pnlLista";
            pnlLista.Padding = new Padding(20, 0, 20, 0);
            pnlLista.TabIndex = 2;
            //
            // lvUnidades
            //
            lvUnidades.BorderStyle = BorderStyle.FixedSingle;
            lvUnidades.CheckBoxes = true;
            lvUnidades.Columns.AddRange(new ColumnHeader[] { colCodigo, colUbicacion, colEstado });
            lvUnidades.Dock = DockStyle.Fill;
            lvUnidades.Font = new Font("Segoe UI", 10F);
            lvUnidades.FullRowSelect = true;
            lvUnidades.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            lvUnidades.MultiSelect = false;
            lvUnidades.Name = "lvUnidades";
            lvUnidades.TabIndex = 0;
            lvUnidades.UseCompatibleStateImageBehavior = false;
            lvUnidades.View = View.Details;
            //
            // colCodigo
            //
            colCodigo.Text = "Código";
            colCodigo.Width = 170;
            //
            // colUbicacion
            //
            colUbicacion.Text = "Ubicación";
            colUbicacion.Width = 200;
            //
            // colEstado
            //
            colEstado.Text = "Estado";
            colEstado.Width = 110;
            //
            // pnlFooter
            //
            pnlFooter.Controls.Add(lblSeleccionadas);
            pnlFooter.Controls.Add(btnCancelar);
            pnlFooter.Controls.Add(btnConfirmar);
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Size = new Size(540, 68);
            pnlFooter.TabIndex = 3;
            //
            // lblSeleccionadas
            //
            lblSeleccionadas.Anchor = AnchorStyles.Left;
            lblSeleccionadas.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            lblSeleccionadas.Location = new Point(20, 22);
            lblSeleccionadas.Name = "lblSeleccionadas";
            lblSeleccionadas.Size = new Size(180, 24);
            lblSeleccionadas.TabIndex = 0;
            lblSeleccionadas.Text = "Seleccionadas: 0";
            lblSeleccionadas.TextAlign = ContentAlignment.MiddleLeft;
            //
            // btnCancelar
            //
            btnCancelar.Anchor = AnchorStyles.Right;
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(270, 14);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(120, 40);
            btnCancelar.TabIndex = 2;
            btnCancelar.Text = "Cancelar";
            btnCancelar.Variant = PromacoHerra.Controls.MaterialButtonVariant.Secondary;
            //
            // btnConfirmar
            //
            btnConfirmar.Anchor = AnchorStyles.Right;
            btnConfirmar.Location = new Point(400, 14);
            btnConfirmar.Name = "btnConfirmar";
            btnConfirmar.Size = new Size(120, 40);
            btnConfirmar.TabIndex = 1;
            btnConfirmar.Text = "Confirmar";
            btnConfirmar.Variant = PromacoHerra.Controls.MaterialButtonVariant.Primary;
            //
            // FrmSeleccionarUnidades
            //
            CancelButton = btnCancelar;
            ClientSize = new Size(540, 520);
            Controls.Add(pnlLista);
            Controls.Add(pnlFooter);
            Controls.Add(pnlFiltro);
            Controls.Add(pnlHeader);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmSeleccionarUnidades";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Seleccionar unidades";
            pnlHeader.ResumeLayout(false);
            pnlFiltro.ResumeLayout(false);
            pnlLista.ResumeLayout(false);
            pnlFooter.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}
