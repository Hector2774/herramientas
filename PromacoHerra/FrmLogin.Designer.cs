namespace PromacoHerra
{
    partial class FrmLogin
    {
        private System.ComponentModel.IContainer components = null;

        // Campos y acciones
        private PromacoHerra.Controls.MaterialTextBox txtUsuario;
        private PromacoHerra.Controls.MaterialTextBox txtPassword;
        private PromacoHerra.Controls.MaterialButton btnLogin;
        private PromacoHerra.Controls.MaterialButton btnSalir;

        // Layout de dos paneles: marca (izquierda) | formulario (derecha)
        private PromacoHerra.Controls.PanelGdi pnlMarca;
        private System.Windows.Forms.Panel pnlFormulario;
        private System.Windows.Forms.Panel pnlCampos;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblSubtitulo;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.Label lblPassword;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pnlMarca = new PromacoHerra.Controls.PanelGdi();
            pnlFormulario = new System.Windows.Forms.Panel();
            pnlCampos = new System.Windows.Forms.Panel();
            lblTitulo = new System.Windows.Forms.Label();
            lblSubtitulo = new System.Windows.Forms.Label();
            lblUsuario = new System.Windows.Forms.Label();
            txtUsuario = new PromacoHerra.Controls.MaterialTextBox();
            lblPassword = new System.Windows.Forms.Label();
            txtPassword = new PromacoHerra.Controls.MaterialTextBox();
            btnLogin = new PromacoHerra.Controls.MaterialButton();
            btnSalir = new PromacoHerra.Controls.MaterialButton();
            pnlFormulario.SuspendLayout();
            pnlCampos.SuspendLayout();
            SuspendLayout();
            //
            // pnlMarca (degradado, logo, nombre de la app y línea roja se pintan en FrmLogin.cs)
            //
            pnlMarca.BackColor = System.Drawing.Color.FromArgb(14, 0, 77);
            pnlMarca.Dock = System.Windows.Forms.DockStyle.Left;
            pnlMarca.Location = new System.Drawing.Point(0, 0);
            pnlMarca.Name = "pnlMarca";
            pnlMarca.Size = new System.Drawing.Size(400, 520);
            pnlMarca.TabIndex = 0;
            //
            // pnlFormulario
            //
            pnlFormulario.BackColor = System.Drawing.Color.White;
            pnlFormulario.Controls.Add(pnlCampos);
            pnlFormulario.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlFormulario.Location = new System.Drawing.Point(400, 0);
            pnlFormulario.Name = "pnlFormulario";
            pnlFormulario.Size = new System.Drawing.Size(480, 520);
            pnlFormulario.TabIndex = 1;
            //
            // pnlCampos (bloque centrado vertical y horizontalmente al cambiar de tamaño)
            //
            pnlCampos.BackColor = System.Drawing.Color.White;
            pnlCampos.Controls.Add(lblTitulo);
            pnlCampos.Controls.Add(lblSubtitulo);
            pnlCampos.Controls.Add(lblUsuario);
            pnlCampos.Controls.Add(txtUsuario);
            pnlCampos.Controls.Add(lblPassword);
            pnlCampos.Controls.Add(txtPassword);
            pnlCampos.Controls.Add(btnLogin);
            pnlCampos.Controls.Add(btnSalir);
            pnlCampos.Location = new System.Drawing.Point(80, 75);
            pnlCampos.Name = "pnlCampos";
            pnlCampos.Size = new System.Drawing.Size(320, 370);
            pnlCampos.TabIndex = 0;
            //
            // lblTitulo
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            lblTitulo.Location = new System.Drawing.Point(-4, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new System.Drawing.Size(178, 37);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Iniciar sesión";
            //
            // lblSubtitulo
            //
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblSubtitulo.Location = new System.Drawing.Point(0, 42);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new System.Drawing.Size(250, 19);
            lblSubtitulo.TabIndex = 1;
            lblSubtitulo.Text = "Ingresa tus credenciales para continuar";
            //
            // lblUsuario
            //
            lblUsuario.AutoSize = true;
            lblUsuario.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            lblUsuario.Location = new System.Drawing.Point(0, 96);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new System.Drawing.Size(55, 17);
            lblUsuario.TabIndex = 2;
            lblUsuario.Text = "Usuario";
            //
            // txtUsuario
            //
            txtUsuario.Location = new System.Drawing.Point(0, 118);
            txtUsuario.Name = "txtUsuario";
            txtUsuario.Size = new System.Drawing.Size(320, 38);
            txtUsuario.TabIndex = 3;
            //
            // lblPassword
            //
            lblPassword.AutoSize = true;
            lblPassword.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            lblPassword.Location = new System.Drawing.Point(0, 174);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new System.Drawing.Size(78, 17);
            lblPassword.TabIndex = 4;
            lblPassword.Text = "Contraseña";
            //
            // txtPassword
            //
            txtPassword.Location = new System.Drawing.Point(0, 196);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '●';
            txtPassword.Size = new System.Drawing.Size(320, 38);
            txtPassword.TabIndex = 5;
            //
            // btnLogin (acción principal: rojo de marca)
            //
            btnLogin.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            btnLogin.Icon = FontAwesome.Sharp.IconChar.SignInAlt;
            btnLogin.IconSize = 22;
            btnLogin.Location = new System.Drawing.Point(0, 268);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new System.Drawing.Size(320, 48);
            btnLogin.TabIndex = 6;
            btnLogin.Text = "Ingresar";
            btnLogin.UseVisualStyleBackColor = true;
            //
            // btnSalir (acción secundaria: ghost)
            //
            btnSalir.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            btnSalir.Icon = FontAwesome.Sharp.IconChar.SignOutAlt;
            btnSalir.IconSize = 18;
            btnSalir.Location = new System.Drawing.Point(0, 328);
            btnSalir.Name = "btnSalir";
            btnSalir.Size = new System.Drawing.Size(320, 40);
            btnSalir.TabIndex = 7;
            btnSalir.Text = "Salir";
            btnSalir.UseVisualStyleBackColor = true;
            //
            // FrmLogin
            //
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(880, 520);
            Controls.Add(pnlFormulario);
            Controls.Add(pnlMarca);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Name = "FrmLogin";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Login - Promaco";
            pnlFormulario.ResumeLayout(false);
            pnlCampos.ResumeLayout(false);
            pnlCampos.PerformLayout();
            ResumeLayout(false);
        }
    }
}
