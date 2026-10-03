using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FontAwesome.Sharp;
using PromacoHerra.Controls;
using PromacoHerra.Data;

namespace PromacoHerra
{
    // Modal para elegir un empleado (lo abre EmpleadoPickerControl). Mismo estilo que FrmEmpleados:
    // búsqueda sin acentos, filtro por departamento y grilla con avatar y chip de departamento.
    // Teclado: ↓ o Enter en la búsqueda → primera fila · Enter en la grilla → elegir ·
    // escribir con la grilla enfocada → sigue buscando.
    // Con incluirInactivos (reportes) también lista a los inactivos, marcados como tales.
    public class FrmBuscarEmpleado : Form
    {
        private sealed record Fila(int EmpleadoId, string Codigo, string Nombre, string Departamento, bool Activo);

        private const string TodosLosDepartamentos = "Todos los departamentos";

        private readonly Panel pnlHeader = new();
        private readonly Label lblTitulo = new();
        private readonly Label lblSubtitulo = new();
        private readonly Panel pnlFiltros = new();
        private readonly IconPictureBox icoBuscar = new();
        private readonly MaterialTextBox txtBuscar = new();
        private readonly MaterialComboBox cboDepartamento = new();
        private readonly Label lblResultados = new();
        private readonly Panel pnlGrid = new();
        private readonly Label lblSinResultados = new();
        private readonly Panel pnlFooter = new();
        private readonly Label lblSeleccion = new();
        private readonly MaterialButton btnSeleccionar = new();
        private readonly MaterialButton btnCancelar = new();

        // La grilla se crea después de ApplyTheme (ver EmpleadoGrid)
        private readonly DataGridView dgv = new();
        private readonly DataGridViewTextBoxColumn colAvatar = EmpleadoGrid.Columna("", 44);
        private readonly DataGridViewTextBoxColumn colCodigo = EmpleadoGrid.Columna("CÓDIGO", 90);
        private readonly DataGridViewTextBoxColumn colNombre = EmpleadoGrid.Columna("NOMBRE");
        private readonly DataGridViewTextBoxColumn colDepartamento = EmpleadoGrid.Columna("DEPARTAMENTO", 220);

        private readonly bool _incluirInactivos;
        private List<Fila> _todos = new();

        public int EmpleadoIdSeleccionado { get; private set; }
        public string NombreSeleccionado { get; private set; } = string.Empty;
        public string DepartamentoSeleccionado { get; private set; } = string.Empty;
        public string CodigoSeleccionado { get; private set; } = string.Empty;

        public FrmBuscarEmpleado(bool incluirInactivos = false)
        {
            _incluirInactivos = incluirInactivos;
            ConstruirUI();
            ThemeManager.ApplyTheme(this);
            ConstruirGrid();
            AplicarEstilos();

            Load += FrmBuscarEmpleado_Load;
        }

        // ── Construcción de la UI (sin Designer) ───────────────────
        private void ConstruirUI()
        {
            FormBorderStyle = FormBorderStyle.FixedDialog;
            ClientSize = new Size(860, 580);
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            Text = "Seleccionar empleado";

            // Encabezado
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 72;
            lblTitulo.Text = "Seleccionar empleado";
            lblTitulo.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblTitulo.Location = new Point(22, 14);
            lblTitulo.AutoSize = true;
            lblSubtitulo.Text = "Busca por nombre, código o departamento";
            lblSubtitulo.Font = new Font("Segoe UI", 9.5F);
            lblSubtitulo.Location = new Point(24, 42);
            lblSubtitulo.AutoSize = true;
            pnlHeader.Controls.AddRange(new Control[] { lblTitulo, lblSubtitulo });

            // Búsqueda y filtro
            pnlFiltros.Dock = DockStyle.Top;
            pnlFiltros.Height = 58;
            icoBuscar.IconChar = IconChar.MagnifyingGlass;
            icoBuscar.IconSize = 18;
            icoBuscar.Size = new Size(22, 22);
            icoBuscar.Location = new Point(22, 17);
            txtBuscar.Location = new Point(50, 8);
            txtBuscar.Size = new Size(300, 38);
            txtBuscar.PlaceholderText = "Buscar por nombre o código…";
            cboDepartamento.Location = new Point(366, 8);
            cboDepartamento.Size = new Size(220, 38);
            lblResultados.Font = new Font("Segoe UI", 9F);
            lblResultados.Size = new Size(220, 20);
            lblResultados.Location = new Point(618, 18);
            lblResultados.TextAlign = ContentAlignment.MiddleRight;
            pnlFiltros.Controls.AddRange(new Control[] { icoBuscar, txtBuscar, cboDepartamento, lblResultados });

            // Grilla (se agrega en ConstruirGrid) + aviso sin resultados
            pnlGrid.Dock = DockStyle.Fill;
            pnlGrid.Padding = new Padding(20, 0, 20, 0);
            lblSinResultados.Dock = DockStyle.Fill;
            lblSinResultados.Text = "Ningún empleado coincide con la búsqueda.";
            lblSinResultados.TextAlign = ContentAlignment.MiddleCenter;
            lblSinResultados.Font = new Font("Segoe UI", 10.5F);
            lblSinResultados.Visible = false;
            pnlGrid.Controls.Add(lblSinResultados);

            // Pie
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Height = 66;
            lblSeleccion.Font = new Font("Segoe UI", 9.5F);
            lblSeleccion.Location = new Point(22, 23);
            lblSeleccion.Size = new Size(540, 22);
            lblSeleccion.AutoEllipsis = true;
            btnSeleccionar.Text = "Seleccionar";
            btnSeleccionar.Icon = IconChar.Check;
            btnSeleccionar.IconSize = 16;
            btnSeleccionar.Size = new Size(140, 40);
            btnSeleccionar.Enabled = false;
            btnCancelar.Text = "Cancelar";
            btnCancelar.Size = new Size(120, 40);
            pnlFooter.Controls.AddRange(new Control[] { lblSeleccion, btnSeleccionar, btnCancelar });

            Controls.Add(pnlGrid);
            Controls.Add(pnlFooter);
            Controls.Add(pnlFiltros);
            Controls.Add(pnlHeader);
            CancelButton = btnCancelar;

            // Lo alineado a la derecha se ubica al cambiar de tamaño (sin Anchor)
            pnlFiltros.Resize += (s, e) => AlinearDerecha();
            pnlFooter.Resize += (s, e) => AlinearDerecha();
            pnlHeader.Paint += (s, e) => Linea(e, pnlHeader, abajo: true);
            pnlFooter.Paint += (s, e) => Linea(e, pnlFooter, abajo: false);

            txtBuscar.TextChanged += (s, e) => FiltrarGrid();
            txtBuscar.KeyDown += TxtBuscar_KeyDown;
            cboDepartamento.SelectedIndexChanged += (s, e) => FiltrarGrid();
            btnSeleccionar.Click += (s, e) => ConfirmarSeleccion();
            btnCancelar.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
        }

        private void AlinearDerecha()
        {
            lblResultados.Left = pnlFiltros.Width - 22 - lblResultados.Width;
            btnCancelar.Location = new Point(pnlFooter.Width - 22 - btnCancelar.Width, 13);
            btnSeleccionar.Location = new Point(btnCancelar.Left - 10 - btnSeleccionar.Width, 13);
            lblSeleccion.Width = Math.Max(100, btnSeleccionar.Left - 20 - lblSeleccion.Left);
        }

        private void ConstruirGrid()
        {
            colAvatar.Resizable = DataGridViewTriState.False;
            dgv.Dock = DockStyle.Fill;
            dgv.Columns.AddRange(colAvatar, colCodigo, colNombre, colDepartamento);
            EmpleadoGrid.Configurar(dgv);
            dgv.CellPainting += Dgv_CellPainting;
            dgv.SelectionChanged += (s, e) => ActualizarSeleccion();
            dgv.KeyDown += Dgv_KeyDown;
            dgv.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) ConfirmarSeleccion(); };

            pnlGrid.Controls.Add(dgv);
            dgv.BringToFront();
        }

        private void AplicarEstilos()
        {
            BackColor = Color.White;
            foreach (var p in new Control[] { pnlHeader, pnlFiltros, pnlGrid, pnlFooter })
                p.BackColor = Color.White;
            foreach (var l in new[] { lblSubtitulo, lblResultados, lblSinResultados, lblSeleccion })
                l.ForeColor = ThemeManager.TextSecondary;
            icoBuscar.BackColor = Color.White;
            icoBuscar.IconColor = ThemeManager.TextSecondary;
            btnSeleccionar.Variant = MaterialButtonVariant.Primary;
            btnCancelar.Variant = MaterialButtonVariant.Secondary;
        }

        private static void Linea(PaintEventArgs e, Control c, bool abajo)
        {
            using var pen = new Pen(ThemeManager.BorderColor);
            int y = abajo ? c.Height - 1 : 0;
            e.Graphics.DrawLine(pen, 0, y, c.Width, y);
        }

        // ── Carga inicial ───────────────────────────────────────────
        private void FrmBuscarEmpleado_Load(object? sender, EventArgs e)
        {
            AlinearDerecha();   // los paneles ya tienen su ancho final
            CargarEmpleados();
            txtBuscar.Focus();

            // Al mostrarse, la grilla preselecciona la primera fila: se quita para que un Enter
            // accidental no elija a nadie sin querer
            BeginInvoke(new Action(() => { dgv.ClearSelection(); ActualizarSeleccion(); }));
        }

        private void CargarEmpleados()
        {
            DataTable dt;
            try
            {
                dt = Db.Query($@"SELECT e.EmpleadoId, e.Codigo, e.Nombre, e.Activo,
       ISNULL(d.Nombre, '—') AS Departamento
FROM   Empleado e
LEFT   JOIN Departamento d ON e.DepartamentoId = d.DepartamentoId
{(_incluirInactivos ? "" : "WHERE  e.Activo = 1")}
ORDER  BY e.Activo DESC, e.Nombre ASC");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al cargar empleados", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _todos = dt.AsEnumerable().Select(r => new Fila(
                r.Field<int>("EmpleadoId"),
                r["Codigo"]?.ToString() ?? "",
                r["Nombre"]?.ToString() ?? "",
                r["Departamento"]?.ToString() ?? "—",
                r.Field<bool>("Activo"))).ToList();

            cboDepartamento.Items.Clear();
            cboDepartamento.Items.Add(TodosLosDepartamentos);
            foreach (var d in _todos.Select(f => f.Departamento).Where(d => d != "—")
                                    .Distinct().OrderBy(d => d, StringComparer.CurrentCultureIgnoreCase))
                cboDepartamento.Items.Add(d);
            cboDepartamento.SelectedIndex = 0;

            FiltrarGrid();
        }

        // ── Filtro local (sin nuevas consultas a la BD) ────────────
        private void FiltrarGrid()
        {
            string texto = txtBuscar.Text.Trim();
            string? depto = cboDepartamento.SelectedIndex > 0 ? cboDepartamento.SelectedItem as string : null;

            var visibles = _todos.Where(f =>
                (depto == null || f.Departamento == depto) &&
                (texto == "" || EmpleadoGrid.Contiene(f.Nombre, texto) || EmpleadoGrid.Contiene(f.Codigo, texto) ||
                 EmpleadoGrid.Contiene(f.Departamento, texto))).ToList();

            dgv.SuspendLayout();
            dgv.Rows.Clear();
            foreach (var f in visibles)
            {
                int i = dgv.Rows.Add("", f.Codigo, f.Nombre, f.Departamento);
                dgv.Rows[i].Tag = f;
            }
            dgv.ResumeLayout();
            dgv.ClearSelection();
            dgv.CurrentCell = null;
            EmpleadoGrid.ReiniciarHover(dgv);

            lblResultados.Text = visibles.Count == 1 ? "1 empleado encontrado" : $"{visibles.Count} empleados encontrados";
            lblSinResultados.Visible = visibles.Count == 0;
            dgv.Visible = visibles.Count > 0;
            ActualizarSeleccion();
        }

        private Fila? FilaSeleccionada =>
            dgv.SelectedRows.Count > 0 ? dgv.SelectedRows[0].Tag as Fila : null;

        private void ActualizarSeleccion()
        {
            var f = FilaSeleccionada;
            btnSeleccionar.Enabled = f != null;
            lblSeleccion.Text = f == null
                ? "Doble clic o Enter para elegir un empleado"
                : $"{f.Codigo}  ·  {f.Nombre}  ·  {f.Departamento}";
            lblSeleccion.ForeColor = f == null ? ThemeManager.TextSecondary : ThemeManager.TextPrimary;
        }

        private void Dgv_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Graphics == null || dgv.Rows[e.RowIndex].Tag is not Fila f) return;

            EmpleadoGrid.PintarFondo(dgv, e);
            var g = e.Graphics;
            if (e.ColumnIndex == colAvatar.Index)
                EmpleadoGrid.DibujarAvatar(g, e.CellBounds, f.Codigo, f.Nombre);
            else if (e.ColumnIndex == colCodigo.Index)
                EmpleadoGrid.DibujarTexto(g, e.CellBounds, f.Codigo, EmpleadoGrid.FuenteCodigo, ThemeManager.TextSecondary);
            else if (e.ColumnIndex == colNombre.Index)
                EmpleadoGrid.DibujarTexto(g, e.CellBounds, f.Activo ? f.Nombre : f.Nombre + "  (inactivo)",
                    EmpleadoGrid.FuenteNombre, f.Activo ? ThemeManager.TextPrimary : ThemeManager.TextSecondary);
            else if (e.ColumnIndex == colDepartamento.Index)
                EmpleadoGrid.DibujarChipDepartamento(g, e.CellBounds, f.Departamento);
            e.Handled = true;
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
            dgv.Focus();
            dgv.CurrentCell = dgv.Rows[0].Cells[colNombre.Index];
            dgv.Rows[0].Selected = true;
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
            if (FilaSeleccionada is not Fila f) return;

            EmpleadoIdSeleccionado = f.EmpleadoId;
            NombreSeleccionado = f.Nombre;
            DepartamentoSeleccionado = f.Departamento;
            CodigoSeleccionado = f.Codigo;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
