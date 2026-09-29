using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using PromacoHerra.Services;

namespace PromacoHerra
{
    public partial class FrmPrestamo : Form
    {
        public FrmPrestamo()
        {
            InitializeComponent();
            ThemeManager.ApplyTheme(this);

            this.Load += FrmPrestamo_Load;
            btnAgregar.Click += btnAgregar_Click;
            btnQuitar.Click += btnQuitar_Click;
            btnGuardar.Click += btnGuardar_Click;
            btnCancelar.Click += btnCancelar_Click;
        }

        // ── Carga inicial ──────────────────────────────────────────
        private void FrmPrestamo_Load(object sender, EventArgs e)
        {
            CargarHerramientasDisponibles();
            InicializarGridSeleccionadas();

            // Fecha préstamo = hoy (solo lectura)
            dtpFecha.Value = DateTime.Now;

            // Fecha devolución por defecto = 7 días adelante
            dtpFechaDevolucion.Value = DateTime.Now.AddDays(7);
            dtpFechaDevolucion.MinDate = DateTime.Now.AddDays(1);
        }

        private void CargarHerramientasDisponibles()
        {
            dgvDisponibles.DataSource = HerramientaService.ObtenerDisponibles();
            OcultarColumna(dgvDisponibles, "HerramientaId");
            OcultarColumna(dgvDisponibles, "StockDisponible");

            RenombrarColumna(dgvDisponibles, "Codigo", "Código");
            RenombrarColumna(dgvDisponibles, "Nombre", "Herramienta");
            RenombrarColumna(dgvDisponibles, "Categoria", "Categoría");
            RenombrarColumna(dgvDisponibles, "Marca", "Marca");
            RenombrarColumna(dgvDisponibles, "Ubicacion", "Ubicación");
        }

        // ── Grid de seleccionadas — tabla manual en memoria ────────
        private void InicializarGridSeleccionadas()
        {
            var dt = new DataTable();
            dt.Columns.Add("HerramientaId", typeof(int));
            dt.Columns.Add("Codigo");
            dt.Columns.Add("Nombre");
            dt.Columns.Add("Categoria");
            dt.Columns.Add("Marca");

            dgvSeleccionadas.DataSource = dt;
            OcultarColumna(dgvSeleccionadas, "HerramientaId");
            RenombrarColumna(dgvSeleccionadas, "Codigo", "Código");
            RenombrarColumna(dgvSeleccionadas, "Nombre", "Herramienta");
            RenombrarColumna(dgvSeleccionadas, "Categoria", "Categoría");
            RenombrarColumna(dgvSeleccionadas, "Marca", "Marca");
        }

        // ── Botón >> Agregar herramienta a seleccionadas ───────────
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (dgvDisponibles.CurrentRow == null) return;

            var row = dgvDisponibles.CurrentRow;
            var dtDisponibles = (DataTable)dgvDisponibles.DataSource;
            var dtSeleccionadas = (DataTable)dgvSeleccionadas.DataSource;

            // Evitar duplicados
            int id = Convert.ToInt32(row.Cells["HerramientaId"].Value);
            foreach (DataRow r in dtSeleccionadas.Rows)
            {
                if (Convert.ToInt32(r["HerramientaId"]) == id)
                {
                    MessageBox.Show("Esta herramienta ya fue agregada.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            dtSeleccionadas.Rows.Add(
                row.Cells["HerramientaId"].Value,
                row.Cells["Codigo"].Value,
                row.Cells["Nombre"].Value,
                row.Cells["Categoria"].Value,
                row.Cells["Marca"].Value
            );

            dtDisponibles.Rows.RemoveAt(row.Index);
        }

        // ── Botón << Quitar herramienta de seleccionadas ───────────
        private void btnQuitar_Click(object sender, EventArgs e)
        {
            if (dgvSeleccionadas.CurrentRow == null) return;

            var row = dgvSeleccionadas.CurrentRow;
            var dtSeleccionadas = (DataTable)dgvSeleccionadas.DataSource;
            var dtDisponibles = (DataTable)dgvDisponibles.DataSource;

            dtDisponibles.Rows.Add(
                row.Cells["HerramientaId"].Value,
                row.Cells["Codigo"].Value,
                row.Cells["Nombre"].Value,
                row.Cells["Categoria"].Value,
                row.Cells["Marca"].Value
            );

            dtSeleccionadas.Rows.RemoveAt(row.Index);
        }

        // ── Botón Guardar ──────────────────────────────────────────
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Validaciones
            if (pickerEmpleado.EmpleadoId == 0)
            {
                MessageBox.Show("Seleccione el empleado solicitante.", "Campo requerido",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (pickerAprobador.EmpleadoId == 0)
            {
                MessageBox.Show("Seleccione quién aprueba el préstamo.", "Campo requerido",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (pickerEmpleado.EmpleadoId == pickerAprobador.EmpleadoId)
            {
                MessageBox.Show("El empleado no puede aprobar su propio préstamo.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dtSeleccionadas = (DataTable)dgvSeleccionadas.DataSource;
            if (dtSeleccionadas.Rows.Count == 0)
            {
                MessageBox.Show("Seleccione al menos una herramienta.", "Campo requerido",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dtpFechaDevolucion.Value.Date <= DateTime.Now.Date)
            {
                MessageBox.Show("La fecha de devolución debe ser posterior a hoy.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Armar lista de IDs
            var ids = new List<int>();
            foreach (DataRow r in dtSeleccionadas.Rows)
                ids.Add(Convert.ToInt32(r["HerramientaId"]));

            try
            {
                int prestamoId = PrestamoService.Registrar(
                    empleadoId: pickerEmpleado.EmpleadoId,
                    aprobadoPorId: pickerAprobador.EmpleadoId,
                    fechaDevolucionEsperada: dtpFechaDevolucion.Value,
                    observaciones: txtObservaciones.Text.Trim(),
                    herramientaIds: ids);

                MessageBox.Show(
                    $"Préstamo #{prestamoId} registrado correctamente.",
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Limpiar para un nuevo préstamo
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al registrar préstamo",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Botón Cancelar ─────────────────────────────────────────
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        // ── Helpers ────────────────────────────────────────────────
        private void LimpiarFormulario()
        {
            pickerEmpleado.Limpiar();
            pickerAprobador.Limpiar();
            txtObservaciones.Clear();
            dtpFecha.Value = DateTime.Now;
            dtpFechaDevolucion.Value = DateTime.Now.AddDays(7);

            CargarHerramientasDisponibles();
            InicializarGridSeleccionadas();
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

        private void dgvSeleccionadas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void grpPrestamo_Enter(object sender, EventArgs e)
        {

        }
    }
}