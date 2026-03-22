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
    public partial class FrmPrestamo : Form
    {
        public FrmPrestamo()
        {
            InitializeComponent();
            this.Load += new System.EventHandler(this.FrmPrestamo_Load);
            this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);
            this.btnQuitar.Click += new System.EventHandler(this.btnQuitar_Click);
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
        }

        private void FrmPrestamo_Load(object sender, EventArgs e)
        {
            CargarEmpleados();
            CargarHerramientasDisponibles();

            dgvSeleccionadas.DataSource = CrearTablaSeleccion();
            
        }


        private void CargarEmpleados()
        {
            var dt = Db.Query("SELECT EmpleadoId, Nombre FROM Empleado WHERE Activo = 1");

            cboEmpleado.DataSource = dt;
            cboEmpleado.DisplayMember = "Nombre";
            cboEmpleado.ValueMember = "EmpleadoId";
        }

        private void CargarHerramientasDisponibles()
        {
            dgvDisponibles.DataSource = Db.Query(@"
        SELECT HerramientaId, Codigo, Nombre
        FROM Herramienta
        WHERE Estado = 'Disponible' AND Activa = 1
    ");
            dgvDisponibles.Columns["HerramientaId"].Visible = false;
            
        }

        private DataTable CrearTablaSeleccion()
        {
            var dt = new DataTable();
            dt.Columns.Add("HerramientaId", typeof(int));
            dt.Columns.Add("Codigo");
            dt.Columns.Add("Nombre");
            return dt;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (dgvDisponibles.CurrentRow == null) return;

            var row = dgvDisponibles.CurrentRow;

            var dtDisponibles = (DataTable)dgvDisponibles.DataSource;
            var dtSeleccionadas = (DataTable)dgvSeleccionadas.DataSource;

            dtSeleccionadas.Rows.Add(
                row.Cells["HerramientaId"].Value,
                row.Cells["Codigo"].Value,
                row.Cells["Nombre"].Value
            );
            dgvSeleccionadas.Columns["HerramientaId"].Visible = false;


            dtDisponibles.Rows.RemoveAt(row.Index);
        }

        private void btnQuitar_Click(object sender, EventArgs e)
        {
            if (dgvSeleccionadas.CurrentRow == null) return;

            var row = dgvSeleccionadas.CurrentRow;

            var dtSeleccionadas = (DataTable)dgvSeleccionadas.DataSource;
            var dtDisponibles = (DataTable)dgvDisponibles.DataSource;
            

            dtDisponibles.Rows.Add(
                row.Cells["HerramientaId"].Value,
                row.Cells["Codigo"].Value,
                row.Cells["Nombre"].Value
            );

            dtSeleccionadas.Rows.RemoveAt(row.Index);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (cboEmpleado.SelectedValue == null)
            {
                MessageBox.Show("Seleccione un empleado");
                return;
            }

            var dt = (DataTable)dgvSeleccionadas.DataSource;

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Seleccione al menos una herramienta");
                return;
            }

            using var cn = Db.GetConnection();
            cn.Open();

            using var tx = cn.BeginTransaction();

            try
            {
                // 1. Insertar préstamo
                var cmdPrestamo = new Microsoft.Data.SqlClient.SqlCommand(@"
    INSERT INTO Prestamo (EmpleadoId, FechaPrestamo, FechaDevolucion, Observaciones)
    OUTPUT INSERTED.PrestamoId
    VALUES (@EmpleadoId, @Fecha, @FechaDev, @Obs)
", cn, tx);

                cmdPrestamo.Parameters.AddWithValue("@EmpleadoId", cboEmpleado.SelectedValue);
                cmdPrestamo.Parameters.AddWithValue("@Fecha", dtpFecha.Value);
                cmdPrestamo.Parameters.AddWithValue("@FechaDev", dtpFechaDevolucion.Value);
                cmdPrestamo.Parameters.AddWithValue("@Obs", txtObservaciones.Text);

                int prestamoId = (int)cmdPrestamo.ExecuteScalar();

                // 2. Insertar detalle + actualizar herramientas
                foreach (DataRow item in dt.Rows)
                {
                    int herramientaId = (int)item["HerramientaId"];

                    // detalle
                    var cmdDetalle = new Microsoft.Data.SqlClient.SqlCommand(@"
                INSERT INTO PrestamoDetalle (PrestamoId, HerramientaId)
                VALUES (@PrestamoId, @HerramientaId)
            ", cn, tx);

                    cmdDetalle.Parameters.AddWithValue("@PrestamoId", prestamoId);
                    cmdDetalle.Parameters.AddWithValue("@HerramientaId", herramientaId);
                    cmdDetalle.ExecuteNonQuery();

                    // actualizar estado herramienta
                    var cmdUpdate = new Microsoft.Data.SqlClient.SqlCommand(@"
                UPDATE Herramienta
                SET Estado = 'Prestada'
                WHERE HerramientaId = @Id
            ", cn, tx);

                    cmdUpdate.Parameters.AddWithValue("@Id", herramientaId);
                    cmdUpdate.ExecuteNonQuery();
                }

                tx.Commit();

                MessageBox.Show("Préstamo registrado correctamente");

                // refrescar
                CargarHerramientasDisponibles();
                dgvSeleccionadas.DataSource = CrearTablaSeleccion();
            }
            catch (Exception ex)
            {
                tx.Rollback();
                MessageBox.Show("Error: " + ex.Message);
            }
        }



        private void btnCancelar_Click(object sender, EventArgs e)
        {

        }

        
    }
}
