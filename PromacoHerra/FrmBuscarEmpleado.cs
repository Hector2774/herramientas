using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FontAwesome.Sharp;
using PromacoHerra.Controls;
using PromacoHerra.Data;

namespace PromacoHerra
{
    public class FrmBuscarEmpleado : Form
    {
        private MaterialTextBox txtBuscar = null!;
        private Label lblResultados = null!;
        private DataGridView dgv = null!;
        private MaterialButton btnSeleccionar = null!;
        private MaterialButton btnCancelar = null!;

        private DataTable _todos = new DataTable();

        public int EmpleadoIdSeleccionado { get; private set; }
        public string NombreSeleccionado { get; private set; } = string.Empty;
        public string DepartamentoSeleccionado { get; private set; } = string.Empty;
        public string CodigoSeleccionado { get; private set; } = string.Empty;

        public FrmBuscarEmpleado()
        {
            ConstruirUI();
            ThemeManager.ApplyTheme(this);

            Load += FrmBuscarEmpleado_Load;
        }

        // ── Construcción de la UI (sin Designer) ───────────────────
        private void ConstruirUI()
        {
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Size = new Size(700, 500);
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            Text = "Buscar Empleado";

            var pnlTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 65,
                BackColor = Color.White
            };

            var lblBuscar = new Label
            {
                Text = "Buscar:",
                Location = new Point(15, 24),
                AutoSize = true
            };

            txtBuscar = new MaterialTextBox
            {
                Location = new Point(75, 20),
                Width = 340,
                PlaceholderText = "Nombre, código o departamento..."
            };

            lblResultados = new Label
            {
                Location = new Point(430, 24),
                Size = new Size(240, 23),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                TextAlign = ContentAlignment.MiddleRight
            };

            pnlTop.Controls.Add(lblBuscar);
            pnlTop.Controls.Add(txtBuscar);
            pnlTop.Controls.Add(lblResultados);

            dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoGenerateColumns = true
            };

            var pnlBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 55,
                BackColor = Color.White
            };

            btnCancelar = new MaterialButton
            {
                Text = "Cancelar",
                Icon = IconChar.Times,
                IconSize = 22,
                Size = new Size(110, 34),
                Location = new Point(575, 10),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            btnSeleccionar = new MaterialButton
            {
                Text = "Seleccionar",
                Icon = IconChar.Check,
                IconSize = 22,
                Size = new Size(110, 34),
                Location = new Point(455, 10),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            pnlBottom.Controls.Add(btnSeleccionar);
            pnlBottom.Controls.Add(btnCancelar);

            Controls.Add(dgv);
            Controls.Add(pnlBottom);
            Controls.Add(pnlTop);

            CancelButton = btnCancelar;

            txtBuscar.TextChanged += (s, e) => FiltrarGrid();
            txtBuscar.KeyDown += TxtBuscar_KeyDown;
            dgv.KeyDown += Dgv_KeyDown;
            dgv.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) ConfirmarSeleccion(); };
            btnSeleccionar.Click += (s, e) => ConfirmarSeleccion();
            btnCancelar.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
        }

        // ── Carga inicial ───────────────────────────────────────────
        private void FrmBuscarEmpleado_Load(object? sender, EventArgs e)
        {
            CargarEmpleados();
            txtBuscar.Focus();
        }

        private void CargarEmpleados()
        {
            _todos = Db.Query(@"SELECT e.EmpleadoId, e.Codigo, e.Nombre,
       ISNULL(d.Nombre, '—') AS Departamento,
       ISNULL(e.Cargo, '—')  AS Cargo
FROM   Empleado e
LEFT   JOIN Departamento d ON e.DepartamentoId = d.DepartamentoId
WHERE  e.Activo = 1
ORDER  BY e.Nombre ASC");

            dgv.DataSource = _todos;
            ConfigurarColumnas();
            ActualizarContador(_todos.Rows.Count);
        }

        private void ConfigurarColumnas()
        {
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (dgv.Columns.Contains("EmpleadoId"))
                dgv.Columns["EmpleadoId"].Visible = false;

            AjustarColumna("Codigo", "Código", 15);
            AjustarColumna("Nombre", "Nombre", 35);
            AjustarColumna("Departamento", "Departamento", 30);
            AjustarColumna("Cargo", "Cargo", 20);
        }

        private void AjustarColumna(string nombre, string header, int fillWeight)
        {
            if (!dgv.Columns.Contains(nombre)) return;
            dgv.Columns[nombre].HeaderText = header;
            dgv.Columns[nombre].FillWeight = fillWeight;
        }

        // ── Filtro local (sin nuevas consultas a la BD) ────────────
        private void FiltrarGrid()
        {
            string texto = txtBuscar.Text.Trim();

            if (string.IsNullOrEmpty(texto))
            {
                dgv.DataSource = _todos;
            }
            else
            {
                string filtro = texto.Replace("'", "''");
                var vista = new DataView(_todos)
                {
                    RowFilter = $"Nombre LIKE '%{filtro}%' OR Codigo LIKE '%{filtro}%' " +
                                $"OR Departamento LIKE '%{filtro}%' OR Cargo LIKE '%{filtro}%'"
                };
                dgv.DataSource = vista;
            }

            ConfigurarColumnas();
            ActualizarContador(dgv.Rows.Count);
        }

        private void ActualizarContador(int cantidad)
        {
            lblResultados.Text = cantidad == 1
                ? "1 empleado encontrado"
                : $"{cantidad} empleados encontrados";
        }

        // ── Teclado ──────────────────────────────────────────────────
        private void TxtBuscar_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Enter)
            {
                SeleccionarPrimeraFila();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void SeleccionarPrimeraFila()
        {
            if (dgv.Rows.Count == 0) return;

            var primeraColumnaVisible = dgv.Columns
                .Cast<DataGridViewColumn>()
                .FirstOrDefault(c => c.Visible);
            if (primeraColumnaVisible == null) return;

            dgv.Focus();
            dgv.ClearSelection();
            dgv.Rows[0].Selected = true;
            dgv.CurrentCell = dgv.Rows[0].Cells[primeraColumnaVisible.Index];
        }

        private void Dgv_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ConfirmarSeleccion();
                e.Handled = true;
                e.SuppressKeyPress = true;
                return;
            }

            if (e.KeyCode >= Keys.A && e.KeyCode <= Keys.Z)
            {
                txtBuscar.Focus();
                txtBuscar.Text += char.ToLower((char)e.KeyCode);
                txtBuscar.SelectionStart = txtBuscar.Text.Length;
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        // ── Confirmación de selección ───────────────────────────────
        private void ConfirmarSeleccion()
        {
            if (dgv.CurrentRow == null) return;

            var row = dgv.CurrentRow;
            EmpleadoIdSeleccionado = Convert.ToInt32(row.Cells["EmpleadoId"].Value);
            NombreSeleccionado = row.Cells["Nombre"].Value?.ToString() ?? string.Empty;
            DepartamentoSeleccionado = row.Cells["Departamento"].Value?.ToString() ?? string.Empty;
            CodigoSeleccionado = row.Cells["Codigo"].Value?.ToString() ?? string.Empty;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
