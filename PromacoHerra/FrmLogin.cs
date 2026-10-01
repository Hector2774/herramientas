using PromacoHerra.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PromacoHerra
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
            ThemeManager.ApplyTheme(this);
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
        }

        private void BtnLogin_Click(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string user = txtUsuario.Text.Trim();
            string pass = txtPassword.Text.Trim();

            if (user == "" || pass == "")
            {
                MessageBox.Show("Ingrese usuario y contraseña");
                return;
            }

            if (UsuarioService.Login(user, pass))
            {
                // abrir sistema
                FrmPrincipal frm = new FrmPrincipal();
                frm.Show();

                this.Hide();
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos");
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        

    }
}
