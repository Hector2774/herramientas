// FrmEmpleados.cs
using PromacoHerra.Models;
using PromacoHerra.Services;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PromacoHerra
{
    public partial class FrmEmpleados : Form
    {
        private readonly EmpleadoService _service = new EmpleadoService();

        public FrmEmpleados()
        {
            InitializeComponent();
            ThemeManager.ApplyTheme(this);

            btnSincronizar.Click += BtnSincronizar_Click;
            txtBuscar.TextChanged += TxtBuscar_TextChanged;
            this.Load += FrmEmpleados_Load;
        }

        // ── Al abrir el formulario carga lo que ya hay en la BD local
        private void FrmEmpleados_Load(object sender, EventArgs e)
        {
            CargarLocales();
        }

        // ── Sincronizar desde la API ───────────────────────────────
        private async void BtnSincronizar_Click(object sender, EventArgs e)
        {
            btnSincronizar.Enabled = false;
            btnSincronizar.Text = "Sincronizando...";
            lblSincronizacion.Text = "Conectando con la API de RRHH...";
            lblSincronizacion.ForeColor = Color.DarkOrange;

            try
            {
                var (insertados, actualizados) = await _service.SincronizarAsync();
                CargarLocales();

                var total = _service.ObtenerLocales().Count;
                lblSincronizacion.Text =
                    $"Última sincronización: {DateTime.Now:dd/MM/yyyy HH:mm}  |  " +
                    $"Nuevos: {insertados}  |  Actualizados: {actualizados}  |  " +
                    $"Total activos: {total}";
                lblSincronizacion.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                lblSincronizacion.Text = "Error al sincronizar con la API.";
                lblSincronizacion.ForeColor = Color.Red;
                MessageBox.Show(ex.Message, "Error de sincronización",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                btnSincronizar.Enabled = true;
                btnSincronizar.Text = "Sincronizar empleados";
            }
        }

        // ── Carga la tabla desde la base de datos local ────────────
        private void CargarLocales()
        {
            var empleados = _service.ObtenerLocales();

            dgvEmpleados.Rows.Clear();
            dgvEmpleados.Columns.Clear();
            dgvEmpleados.Columns.Add("Codigo", "Código");
            dgvEmpleados.Columns.Add("Nombre", "Nombre");
            dgvEmpleados.Columns.Add("Departamento", "Departamento");
            dgvEmpleados.Columns["Codigo"].FillWeight = 15;

            foreach (var emp in empleados)
                dgvEmpleados.Rows.Add(emp.Codigo, emp.Nombre, emp.Departamento);

            if (empleados.Count == 0)
            {
                lblSincronizacion.Text = "Sin datos locales. Presiona 'Sincronizar' para obtener empleados.";
                lblSincronizacion.ForeColor = Color.Gray;
            }
        }

        // ── Filtro en tiempo real ──────────────────────────────────
        private void TxtBuscar_TextChanged(object sender, EventArgs e)
        {
            var term = txtBuscar.Text.Trim().ToLower();
            foreach (DataGridViewRow fila in dgvEmpleados.Rows)
            {
                var codigo = fila.Cells["Codigo"].Value?.ToString().ToLower() ?? "";
                var nombre = fila.Cells["Nombre"].Value?.ToString().ToLower() ?? "";
                fila.Visible = string.IsNullOrEmpty(term)
                               || codigo.Contains(term)
                               || nombre.Contains(term);
            }
        }
    }
}