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
        public void AplicarDisenoDataGrid(DataGridView dgv)
        {
            // 1. Configuración general y bordes
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal; // Líneas divisorias solo horizontales
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            // 2. Estilo del encabezado (Columnas)
            // Desactivar los estilos visuales por defecto es OBLIGATORIO para que el color de fondo funcione
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(41, 128, 185); // Un azul profesional
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv.ColumnHeadersHeight = 35;

            // 3. Estilo de las celdas (Filas)
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = Color.FromArgb(64, 64, 64); // Texto gris oscuro para no cansar la vista
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219); // Azul más claro al seleccionar
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.DefaultCellStyle.Padding = new Padding(5, 0, 0, 0); // Un poco de espacio para que el texto no pegue al borde

            // 4. Filas alternas (Efecto cebra)
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245); // Gris muy claro

            // 5. Comportamientos visuales
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // Selecciona toda la fila, no solo una celda
            dgv.RowHeadersVisible = false; // Oculta la columna vacía de la izquierda (la de la flechita)
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Hace que las columnas ocupen todo el ancho
            dgv.AllowUserToResizeRows = false;
            dgv.AllowUserToAddRows = false; // Oculta la última fila vacía (si agregas datos por código)
        }
        private void FrmHerramientas_Load(object sender, EventArgs e)
        {
            CargarHerramientas();
            CargarEstados();
            HabilitarControles(false);
            AplicarDisenoDataGrid(dgvHerramientas);
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
