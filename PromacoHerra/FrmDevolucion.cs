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
    public partial class FrmDevolucion : Form
    {
        public FrmDevolucion()
        {
            
            InitializeComponent();
            this.Load += new System.EventHandler(this.FrmDevolucion_Load);
            this.dgvPrestamos.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPrestamos_CellClick);
            this.btnDevolver.Click += new System.EventHandler(this.btnDevolver_Click);
            this.btnDevolverTodo.Click += new System.EventHandler(this.btnDevolverTodo_Click);
            this.dgvDetalle.RowPrePaint += dgvDetalle_RowPrePaint;
        }

        private void CargarPrestamos()
        {
            dgvPrestamos.DataSource = Db.Query(@"
        SELECT 
            e.EmpleadoId,
            e.Nombre AS Empleado,
            COUNT(p.PrestamoId) AS PrestamosActivos
        FROM Prestamo p
        INNER JOIN Empleado e ON e.EmpleadoId = p.EmpleadoId
        WHERE p.FechaCierre IS NULL
        GROUP BY e.EmpleadoId, e.Nombre
        ORDER BY e.Nombre
    ");

            dgvPrestamos.Columns["EmpleadoId"].Visible = false;
        }

        private void CargarDetallePorEmpleado(int empleadoId)
        {
            dgvDetalle.DataSource = Db.Query(@"
        SELECT 
            d.PrestamoDetalleId,
            d.PrestamoId,
            h.HerramientaId,
            h.Codigo,
            h.Nombre,
            p.FechaDevolucion,
            d.FechaDevuelta
        FROM PrestamoDetalle d
        INNER JOIN Herramienta h ON h.HerramientaId = d.HerramientaId
        INNER JOIN Prestamo p ON p.PrestamoId = d.PrestamoId
        WHERE FechaDevuelta IS NULL AND p.EmpleadoId = " + empleadoId + @"
          AND p.FechaCierre IS NULL
    ");

            dgvDetalle.Columns["PrestamoDetalleId"].Visible = false;
            dgvDetalle.Columns["HerramientaId"].Visible = false;
        }



        private void FrmDevolucion_Load(object sender, EventArgs e)
        {
            CargarPrestamos();
            CargarEstados();
        }

        private void dgvPrestamos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int empleadoId = Convert.ToInt32(
                dgvPrestamos.Rows[e.RowIndex].Cells["EmpleadoId"].Value
            );

            CargarDetallePorEmpleado(empleadoId);
        }


        private void btnDevolver_Click(object sender, EventArgs e)
        {
            if (dgvDetalle.CurrentRow == null)
            {
                MessageBox.Show("Seleccione una herramienta");
                return;
            }

            var row = dgvDetalle.CurrentRow;

            int detalleId = Convert.ToInt32(row.Cells["PrestamoDetalleId"].Value);
            int herramientaId = Convert.ToInt32(row.Cells["HerramientaId"].Value);
            int prestamoId = Convert.ToInt32(row.Cells["PrestamoId"].Value);

            using var cn = Db.GetConnection();
            cn.Open();

            using var tx = cn.BeginTransaction();

            try
            {
                // 1. marcar devolución
                var cmd1 = new Microsoft.Data.SqlClient.SqlCommand(@"
            UPDATE PrestamoDetalle
            SET 
                FechaDevuelta = GETDATE(),
                ObservacionDevolucion = @Obs
            WHERE PrestamoDetalleId = @Id
              AND FechaDevuelta IS NULL
        ", cn, tx);

                cmd1.Parameters.AddWithValue("@Id", detalleId);
                cmd1.Parameters.AddWithValue("@Obs", txtObservacion.Text);

                int filas = cmd1.ExecuteNonQuery();

                if (filas == 0)
                {
                    tx.Rollback();
                    MessageBox.Show("Ya fue devuelta");
                    return;
                }

                // 2. actualizar estado herramienta
                var cmd2 = new Microsoft.Data.SqlClient.SqlCommand(@"
            UPDATE Herramienta
            SET Estado = @Estado
            WHERE HerramientaId = @Id
        ", cn, tx);

                cmd2.Parameters.AddWithValue("@Estado", cboEstado.Text);
                cmd2.Parameters.AddWithValue("@Id", herramientaId);
                cmd2.ExecuteNonQuery();

                tx.Commit();

                MessageBox.Show("Devolución registrada");

                txtObservacion.Clear();
                cboEstado.SelectedIndex = 0;

                // 🔥 recargar detalle por empleado
                int empleadoId = Convert.ToInt32(dgvPrestamos.CurrentRow.Cells["EmpleadoId"].Value);
                CargarDetallePorEmpleado(empleadoId);

                CargarPrestamos();
            }
            catch (Exception ex)
            {
                tx.Rollback();
                MessageBox.Show("Error: " + ex.Message);
            }

            var cmd3 = new Microsoft.Data.SqlClient.SqlCommand(@"
            SELECT COUNT(*)
            FROM PrestamoDetalle
            WHERE PrestamoId = @PrestamoId
              AND FechaDevuelta IS NULL
        ", cn, tx);

            cmd3.Parameters.AddWithValue("@PrestamoId", prestamoId);
            int pendientes = Convert.ToInt32(cmd3.ExecuteScalar());

            if (pendientes == 0)
            {
                var cmd4 = new Microsoft.Data.SqlClient.SqlCommand(@"
                UPDATE Prestamo
                SET FechaCierre = GETDATE()
                WHERE PrestamoId = @PrestamoId
            ", cn, tx);

                cmd4.Parameters.AddWithValue("@PrestamoId", prestamoId);
                cmd4.ExecuteNonQuery();
            }

            CargarPrestamos();
        }


        private void btnDevolverTodo_Click(object sender, EventArgs e)
        {
            if (dgvPrestamos.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un préstamo");
                return;
            }

            int prestamoId = Convert.ToInt32(dgvPrestamos.CurrentRow.Cells["PrestamoId"].Value);

            using var cn = Db.GetConnection();
            cn.Open();

            using var tx = cn.BeginTransaction();

            try
            {
                // devolver todos los detalles
                var cmd1 = new Microsoft.Data.SqlClient.SqlCommand(@"
            UPDATE PrestamoDetalle
            SET FechaDevuelta = GETDATE()
            WHERE PrestamoId = @Id AND FechaDevuelta IS NULL
        ", cn, tx);

                cmd1.Parameters.AddWithValue("@Id", prestamoId);
                cmd1.ExecuteNonQuery();

                // actualizar herramientas
                var cmd2 = new Microsoft.Data.SqlClient.SqlCommand(@"
            UPDATE Herramienta
            SET Estado = 'Disponible'
            WHERE HerramientaId IN (
                SELECT HerramientaId 
                FROM PrestamoDetalle 
                WHERE PrestamoId = @Id
            )
        ", cn, tx);

                cmd2.Parameters.AddWithValue("@Id", prestamoId);
                cmd2.ExecuteNonQuery();

                // cerrar préstamo
                var cmd3 = new Microsoft.Data.SqlClient.SqlCommand(@"
            UPDATE Prestamo
            SET FechaCierre = GETDATE()
            WHERE PrestamoId = @Id
        ", cn, tx);

                cmd3.Parameters.AddWithValue("@Id", prestamoId);
                cmd3.ExecuteNonQuery();

                tx.Commit();

                MessageBox.Show("Préstamo cerrado");

                CargarPrestamos();
                dgvDetalle.DataSource = null;
            }
            catch (Exception ex)
            {
                tx.Rollback();
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void CargarEstados()
        {
            cboEstado.Items.Clear();
            cboEstado.Items.Add("Disponible");
            cboEstado.Items.Add("Dañada");
            cboEstado.Items.Add("Baja");

            cboEstado.SelectedIndex = 0;
        }

        private void dgvDetalle_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var row = dgvDetalle.Rows[e.RowIndex];

            if (row.Cells["FechaDevolucion"].Value == null) return;

            DateTime fechaLimite = Convert.ToDateTime(row.Cells["FechaDevolucion"].Value);

            if (DateTime.Now.Date > fechaLimite.Date)
            {
                // ATRASADO
                row.DefaultCellStyle.BackColor = Color.Red;
                row.DefaultCellStyle.ForeColor = Color.White;
            }
            else if (DateTime.Now.Date == fechaLimite.Date)
            {
                // VENCE HOY
                row.DefaultCellStyle.BackColor = Color.Orange;
            }
            else
            {
                // A TIEMPO
                row.DefaultCellStyle.BackColor = Color.LightGreen;
            }
        }


    }
}
