using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PromacoHerra.Data;

namespace PromacoHerra
{
    public partial class FrmHerramientas : Form
    {
        private int? herramientaId = null;
        public FrmHerramientas()
        {
            InitializeComponent();
            this.Load += new System.EventHandler(this.FrmHerramientas_Load);
            this.btnNuevo.Click += new System.EventHandler(this.btnNuevo_Click);
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            this.dgvHerramientas.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvHerramientas_CellClick);
            this.txtBuscar.TextChanged += new System.EventHandler(this.txtBuscar_TextChanged);
            this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);
        }
        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            BuscarHerramientas(txtBuscar.Text);
        }


        private void BuscarHerramientas(string texto)
        {
            dgvHerramientas.DataSource = Db.Query(@"
        SELECT 
            HerramientaId,
            Codigo,
            Nombre,
            Estado,
            Caracteristicas
        FROM Herramienta
        WHERE Activa = 1
        AND (
            Codigo LIKE '%" + texto + @"%' OR
            Nombre LIKE '%" + texto + @"%'
        )
        ORDER BY Nombre
    ");
        }
        private void FrmHerramientas_Load(object sender, EventArgs e)
        {
            CargarHerramientas();
            CargarEstados();
            HabilitarControles(false);
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            herramientaId = null;

            txtCodigo.Clear();
            txtNombre.Clear();
            txtCaracteristicas.Clear();
            cboEstado.SelectedIndex = 0;
            btnEditar.Enabled = false;
            HabilitarControles(true);

            txtCodigo.Focus();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            herramientaId = null;

            txtCodigo.Clear();
            txtNombre.Clear();
            txtCaracteristicas.Clear();
            cboEstado.SelectedIndex = 0;

            HabilitarControles(false);
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (herramientaId == null)
            {
                MessageBox.Show("Seleccione una herramienta");
                return;
            }
            btnEditar.Enabled = false;
            HabilitarControles(true);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodigo.Text) ||
                string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Complete los campos obligatorios");
                return;
            }

            if (herramientaId == null)
            {
                // INSERT
                Db.Execute(@"
            INSERT INTO Herramienta (Codigo, Nombre, Caracteristicas, Estado)
            VALUES (@Codigo, @Nombre, @Caracteristicas, @Estado)
        ",
                new Microsoft.Data.SqlClient.SqlParameter("@Codigo", txtCodigo.Text),
                new Microsoft.Data.SqlClient.SqlParameter("@Nombre", txtNombre.Text),
                new Microsoft.Data.SqlClient.SqlParameter("@Caracteristicas", txtCaracteristicas.Text),
                new Microsoft.Data.SqlClient.SqlParameter("@Estado", cboEstado.Text)
                );
            }
            else
            {
                // UPDATE
                Db.Execute(@"
            UPDATE Herramienta
            SET Codigo = @Codigo,
                Nombre = @Nombre,
                Caracteristicas = @Caracteristicas,
                Estado = @Estado
            WHERE HerramientaId = @Id
        ",
                new Microsoft.Data.SqlClient.SqlParameter("@Codigo", txtCodigo.Text),
                new Microsoft.Data.SqlClient.SqlParameter("@Nombre", txtNombre.Text),
                new Microsoft.Data.SqlClient.SqlParameter("@Caracteristicas", txtCaracteristicas.Text),
                new Microsoft.Data.SqlClient.SqlParameter("@Estado", cboEstado.Text),
                new Microsoft.Data.SqlClient.SqlParameter("@Id", herramientaId)
                );
            }

            MessageBox.Show("Guardado correctamente");

            CargarHerramientas();
            btnNuevo_Click(null, null);
            CargarHerramientas();
            btnCancelar_Click(null, null);
        }


        private void dgvHerramientas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvHerramientas.Rows[e.RowIndex];

            herramientaId = Convert.ToInt32(row.Cells["HerramientaId"].Value);
            btnEditar.Enabled = true;
            txtCodigo.Text = row.Cells["Codigo"].Value.ToString();
            txtNombre.Text = row.Cells["Nombre"].Value.ToString();
            txtCaracteristicas.Text = row.Cells["Caracteristicas"].Value.ToString();
            cboEstado.Text = row.Cells["Estado"].Value.ToString();

            HabilitarControles(false);
        }


        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (herramientaId == null)
            {
                MessageBox.Show("Seleccione un registro");
                return;
            }

            var confirm = MessageBox.Show("¿Eliminar herramienta?", "Confirmar", MessageBoxButtons.YesNo);

            if (confirm == DialogResult.Yes)
            {
                Db.Execute(@"
            UPDATE Herramienta
            SET Activa = 0
            WHERE HerramientaId = @Id
        ",
                new Microsoft.Data.SqlClient.SqlParameter("@Id", herramientaId)
                );

                MessageBox.Show("Eliminado correctamente");

                CargarHerramientas();
                btnNuevo_Click(null, null);
            }
        }



        private void CargarHerramientas()
        {
            dgvHerramientas.DataSource = Db.Query(@"
        SELECT 
            HerramientaId,
            Codigo,
            Nombre,
            Estado,
            Caracteristicas
        FROM Herramienta
        WHERE Activa = 1
        ORDER BY Nombre
    ");
            dgvHerramientas.Columns["HerramientaId"].Visible = false;
        }


        private void CargarEstados()
        {
            cboEstado.Items.Clear();
            cboEstado.Items.Add("Disponible");
            cboEstado.Items.Add("Prestada");
            cboEstado.Items.Add("Dañada");
            cboEstado.Items.Add("Baja");

            cboEstado.SelectedIndex = 0;
        }


        private void HabilitarControles(bool habilitar)
        {
            txtCodigo.Enabled = habilitar;
            txtNombre.Enabled = habilitar;
            txtCaracteristicas.Enabled = habilitar;
            cboEstado.Enabled = habilitar;

            btnGuardar.Enabled = habilitar;
            btnCancelar.Enabled = habilitar;

            btnEliminar.Enabled = !habilitar && herramientaId != null;

            btnNuevo.Enabled = !habilitar;
        }

    }



}
