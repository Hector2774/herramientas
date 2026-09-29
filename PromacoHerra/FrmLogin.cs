using PromacoHerra.Data;
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

            using var cn = Db.GetConnection();
            cn.Open();

            var cmd = new Microsoft.Data.SqlClient.SqlCommand(@"
        SELECT COUNT(*) 
        FROM Usuario 
        WHERE Username = @user 
        AND Password = @pass 
        AND Activo = 1
    ", cn);

            cmd.Parameters.AddWithValue("@user", user);
            cmd.Parameters.AddWithValue("@pass", pass);

            int existe = Convert.ToInt32(cmd.ExecuteScalar());

            if (existe > 0)
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
