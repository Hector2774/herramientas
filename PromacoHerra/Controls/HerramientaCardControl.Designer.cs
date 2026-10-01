namespace PromacoHerra.Controls
{
    partial class HerramientaCardControl
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.FlowLayoutPanel flpTags;
        private System.Windows.Forms.Label lblCategoria;
        private System.Windows.Forms.Label lblMarca;
        private System.Windows.Forms.Label lblStockDot;
        private System.Windows.Forms.Label lblStock;
        private PromacoHerra.Controls.MaterialButton btnMenos;
        private System.Windows.Forms.Label lblCantidad;
        private PromacoHerra.Controls.MaterialButton btnMas;
        private System.Windows.Forms.LinkLabel lnkUnidades;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblNombre = new Label();
            flpTags = new FlowLayoutPanel();
            lblCategoria = new Label();
            lblMarca = new Label();
            lblStockDot = new Label();
            lblStock = new Label();
            btnMenos = new PromacoHerra.Controls.MaterialButton();
            lblCantidad = new Label();
            btnMas = new PromacoHerra.Controls.MaterialButton();
            lnkUnidades = new LinkLabel();
            flpTags.SuspendLayout();
            SuspendLayout();
            //
            // lblNombre
            //
            lblNombre.AutoEllipsis = true;
            lblNombre.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            lblNombre.Location = new Point(12, 90);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(196, 42);
            lblNombre.TabIndex = 0;
            lblNombre.Text = "Herramienta";
            //
            // flpTags
            //
            flpTags.Controls.Add(lblCategoria);
            flpTags.Controls.Add(lblMarca);
            flpTags.Location = new Point(12, 134);
            flpTags.Name = "flpTags";
            flpTags.Size = new Size(196, 24);
            flpTags.TabIndex = 1;
            flpTags.WrapContents = false;
            //
            // lblCategoria
            //
            lblCategoria.AutoSize = true;
            lblCategoria.Font = new Font("Segoe UI", 8.5F);
            lblCategoria.Margin = new Padding(0, 0, 6, 0);
            lblCategoria.Name = "lblCategoria";
            lblCategoria.Padding = new Padding(6, 2, 6, 2);
            lblCategoria.TabIndex = 0;
            lblCategoria.Text = "Categoría";
            //
            // lblMarca
            //
            lblMarca.AutoSize = true;
            lblMarca.Font = new Font("Segoe UI", 8.5F);
            lblMarca.Margin = new Padding(0);
            lblMarca.Name = "lblMarca";
            lblMarca.Padding = new Padding(6, 2, 6, 2);
            lblMarca.TabIndex = 1;
            lblMarca.Text = "Marca";
            //
            // lblStockDot
            //
            lblStockDot.Font = new Font("Segoe UI", 10F);
            lblStockDot.Location = new Point(10, 162);
            lblStockDot.Name = "lblStockDot";
            lblStockDot.Size = new Size(16, 20);
            lblStockDot.TabIndex = 2;
            lblStockDot.Text = "●";
            lblStockDot.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblStock
            //
            lblStock.Font = new Font("Segoe UI", 9F);
            lblStock.Location = new Point(26, 162);
            lblStock.Name = "lblStock";
            lblStock.Size = new Size(182, 20);
            lblStock.TabIndex = 3;
            lblStock.Text = "0 disponibles";
            lblStock.TextAlign = ContentAlignment.MiddleLeft;
            //
            // btnMenos
            //
            btnMenos.CornerRadius = 6;
            btnMenos.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnMenos.Location = new Point(12, 198);
            btnMenos.Name = "btnMenos";
            btnMenos.Size = new Size(26, 26);
            btnMenos.TabIndex = 4;
            btnMenos.Text = "−";
            btnMenos.Variant = PromacoHerra.Controls.MaterialButtonVariant.Default;
            //
            // lblCantidad
            //
            lblCantidad.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            lblCantidad.Location = new Point(40, 198);
            lblCantidad.Name = "lblCantidad";
            lblCantidad.Size = new Size(30, 26);
            lblCantidad.TabIndex = 5;
            lblCantidad.Text = "0";
            lblCantidad.TextAlign = ContentAlignment.MiddleCenter;
            //
            // btnMas
            //
            btnMas.CornerRadius = 6;
            btnMas.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnMas.Location = new Point(72, 198);
            btnMas.Name = "btnMas";
            btnMas.Size = new Size(26, 26);
            btnMas.TabIndex = 6;
            btnMas.Text = "+";
            btnMas.Variant = PromacoHerra.Controls.MaterialButtonVariant.Default;
            //
            // lnkUnidades
            //
            lnkUnidades.Font = new Font("Segoe UI", 9F);
            lnkUnidades.LinkBehavior = LinkBehavior.HoverUnderline;
            lnkUnidades.Location = new Point(104, 198);
            lnkUnidades.Name = "lnkUnidades";
            lnkUnidades.Size = new Size(104, 26);
            lnkUnidades.TabIndex = 7;
            lnkUnidades.TabStop = true;
            lnkUnidades.Text = "Ver unidades ›";
            lnkUnidades.TextAlign = ContentAlignment.MiddleRight;
            lnkUnidades.Visible = false;
            //
            // HerramientaCardControl
            //
            BackColor = Color.White;
            Controls.Add(lblNombre);
            Controls.Add(flpTags);
            Controls.Add(lblStockDot);
            Controls.Add(lblStock);
            Controls.Add(btnMenos);
            Controls.Add(lblCantidad);
            Controls.Add(btnMas);
            Controls.Add(lnkUnidades);
            Font = new Font("Segoe UI", 9.5F);
            Margin = new Padding(0, 0, 12, 12);
            Name = "HerramientaCardControl";
            Size = new Size(224, 240);
            flpTags.ResumeLayout(false);
            flpTags.PerformLayout();
            ResumeLayout(false);
        }
    }
}
