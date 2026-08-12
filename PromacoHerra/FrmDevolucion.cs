using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using PromacoHerra.Services;

namespace PromacoHerra
{
    public partial class FrmDevolucion : Form
    {
        // Guarda el PrestamoId seleccionado en el grid superior
        private int? _prestamoIdSeleccionado = null;

        public FrmDevolucion()
        {
            InitializeComponent();
            ThemeManager.ApplyTheme(this);

            this.Load += FrmDevolucion_Load;
            dgvPrestamos.CellClick += dgvPrestamos_CellClick;
            dgvDetalle.RowPrePaint += dgvDetalle_RowPrePaint;
            btnDevolver.Click += btnDevolver_Click;
            btnDevolverTodo.Click += btnDevolverTodo_Click;
            txtBuscar.TextChanged += txtBuscar_TextChanged;
            dgvPrestamos.RowPrePaint += dgvPrestamos_RowPrePaint;
        }

        private void FrmDevolucion_Load(object sender, EventArgs e)
        {
            CargarEstados();

            // Usa el nuevo método separado
            int vencidos = DevolucionService.MarcarVencidosYContar();
            CargarPrestamos();

            if (vencidos > 0)
                MessageBox.Show(
                    $"⚠ {vencidos} préstamo(s) marcado(s) como vencido(s) al abrir.",
                    "Préstamos vencidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        // ── Cargar grid de préstamos activos ──────────────────────
        private void CargarPrestamos()
        {
            var dt = DevolucionService.ObtenerPrestamosActivos();
            dgvPrestamos.DataSource = dt;

            OcultarColumna(dgvPrestamos, "PrestamoId");

            RenombrarColumna(dgvPrestamos, "Empleado", "Empleado");
            RenombrarColumna(dgvPrestamos, "CodigoEmpleado", "Código");
            RenombrarColumna(dgvPrestamos, "Departamento", "Departamento");
            RenombrarColumna(dgvPrestamos, "FechaPrestamo", "Fecha préstamo");
            RenombrarColumna(dgvPrestamos, "FechaDevolucionEsperada", "Fecha límite");
            RenombrarColumna(dgvPrestamos, "Estado", "Estado");
            RenombrarColumna(dgvPrestamos, "DiasAtraso", "Días atraso");
            RenombrarColumna(dgvPrestamos, "HerramientasPendientes", "Pendientes");

          
            _prestamoIdSeleccionado = null;
            dgvDetalle.DataSource = null;
        }

        // ── Seleccionar préstamo → cargar su detalle ───────────────
        private void dgvPrestamos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            _prestamoIdSeleccionado = Convert.ToInt32(
                dgvPrestamos.Rows[e.RowIndex].Cells["PrestamoId"].Value);

            CargarDetalle(_prestamoIdSeleccionado.Value);
        }

        private void CargarDetalle(int prestamoId)
        {
            var dt = DevolucionService.ObtenerDetallePendiente(prestamoId);
            dgvDetalle.DataSource = dt;

            OcultarColumna(dgvDetalle, "PrestamoDetalleId");
            OcultarColumna(dgvDetalle, "HerramientaId");

            RenombrarColumna(dgvDetalle, "CodigoHerramienta", "Código");
            RenombrarColumna(dgvDetalle, "Herramienta", "Herramienta");
            RenombrarColumna(dgvDetalle, "Marca", "Marca");
            RenombrarColumna(dgvDetalle, "Categoria", "Categoría");
            RenombrarColumna(dgvDetalle, "EstadoDevolucion", "Estado");
        }

        // ── Buscar en grid de préstamos ────────────────────────────
        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            var term = txtBuscar.Text.Trim().ToLower();
            foreach (DataGridViewRow row in dgvPrestamos.Rows)
            {
                var empleado = row.Cells["Empleado"].Value?.ToString().ToLower() ?? "";
                var codigo = row.Cells["CodigoEmpleado"].Value?.ToString().ToLower() ?? "";
                row.Visible = string.IsNullOrEmpty(term)
                               || empleado.Contains(term)
                               || codigo.Contains(term);
            }
        }

        // ── Botón: Devolver herramienta seleccionada ───────────────
        private void btnDevolver_Click(object sender, EventArgs e)
        {
            if (dgvDetalle.CurrentRow == null)
            {
                MessageBox.Show("Seleccione una herramienta del detalle.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cboEstado.SelectedIndex < 0)
            {
                MessageBox.Show("Seleccione el estado de devolución.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int detalleId = Convert.ToInt32(
                dgvDetalle.CurrentRow.Cells["PrestamoDetalleId"].Value);
            string estado = cboEstado.SelectedItem.ToString();
            string obs = txtObservacion.Text.Trim();

            // Confirmación extra si regresa dañada o perdida
            if (estado == "Dañado" || estado == "Perdido")
            {
                string msg = estado == "Dañado"
                    ? "La herramienta se marcará como Dañada y no volverá al stock hasta pasar por mantenimiento."
                    : "La herramienta se marcará como Perdida y se descontará del stock total permanentemente.";

                var confirm = MessageBox.Show(
                    msg + "\n\n¿Confirmar?", $"Devolución — {estado}",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirm != DialogResult.Yes) return;
            }

            try
            {
                DevolucionService.RegistrarUna(detalleId, estado, obs);

                MessageBox.Show("Devolución registrada correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarControlesDev();
                CargarPrestamos();

                // Si el préstamo sigue activo, recargar su detalle
                if (_prestamoIdSeleccionado.HasValue)
                    CargarDetalle(_prestamoIdSeleccionado.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al registrar devolución",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Botón: Devolver todas las herramientas del préstamo ────
        private void btnDevolverTodo_Click(object sender, EventArgs e)
        {
            if (_prestamoIdSeleccionado == null)
            {
                MessageBox.Show("Seleccione un préstamo de la lista.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cboEstado.SelectedIndex < 0)
            {
                MessageBox.Show("Seleccione el estado de devolución.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string estado = cboEstado.SelectedItem.ToString();
            string obs = txtObservacion.Text.Trim();

            string msg = estado == "Bueno"
                ? "Se devolverán todas las herramientas pendientes en estado Bueno."
                : $"Todas las herramientas se marcarán como {estado}. ¿Confirmar?";

            var confirm = MessageBox.Show(msg, "Devolver todo",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                DevolucionService.RegistrarTodas(
                    _prestamoIdSeleccionado.Value, estado, obs);

                MessageBox.Show("Todas las herramientas devueltas correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarControlesDev();
                _prestamoIdSeleccionado = null;
                CargarPrestamos();
                dgvDetalle.DataSource = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al devolver",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Color de filas del grid de préstamos ───────────────────
        private void dgvPrestamos_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var row = dgvPrestamos.Rows[e.RowIndex];

            // Verificar que las columnas ya existen antes de leerlas
            if (!dgvPrestamos.Columns.Contains("Estado") ||
                !dgvPrestamos.Columns.Contains("DiasAtraso")) return;

            var estado = row.Cells["Estado"].Value?.ToString() ?? "";
            int dias = 0;

            if (row.Cells["DiasAtraso"].Value != null &&
                row.Cells["DiasAtraso"].Value != DBNull.Value)
                dias = Convert.ToInt32(row.Cells["DiasAtraso"].Value);

            if (estado == "Vencido" || dias > 0)
            {
                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 200, 200);
                row.DefaultCellStyle.ForeColor = Color.DarkRed;
            }
            else if (dias == 0)
            {
                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 235, 180);
                row.DefaultCellStyle.ForeColor = Color.DarkOrange;
            }
            else
            {
                // A tiempo — restaurar color por defecto
                row.DefaultCellStyle.BackColor = Color.Empty;
                row.DefaultCellStyle.ForeColor = Color.Empty;
            }
        }

        private void dgvDetalle_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            // Sin color en el detalle — la información visual está en el grid de préstamos
        }

        // ── Cargar estados de devolución ───────────────────────────
        private void CargarEstados()
        {
            cboEstado.Items.Clear();
            cboEstado.Items.Add("Bueno");
            cboEstado.Items.Add("Dañado");
            cboEstado.Items.Add("Perdido");
            cboEstado.SelectedIndex = 0;
        }

        // ── Helpers ────────────────────────────────────────────────
        private void LimpiarControlesDev()
        {
            txtObservacion.Clear();
            cboEstado.SelectedIndex = 0;
        }

        private void OcultarColumna(DataGridView dgv, string nombre)
        {
            if (dgv.Columns.Contains(nombre))
                dgv.Columns[nombre].Visible = false;
        }

        private void RenombrarColumna(DataGridView dgv, string nombre, string header)
        {
            if (dgv.Columns.Contains(nombre))
                dgv.Columns[nombre].HeaderText = header;
        }
    }
}