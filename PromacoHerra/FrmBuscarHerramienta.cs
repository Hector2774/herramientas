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
    // Modal para elegir una herramienta (lo abre HerramientaPickerControl). Mismo estilo y teclado
    // que FrmBuscarEmpleado: búsqueda sin acentos por nombre, código, categoría o marca, filtro por
    // categoría y grilla con miniatura, código, nombre, chip de categoría y marca.
    // Con incluirInactivas (reportes) también lista las dadas de baja, marcadas como tales.
    public class FrmBuscarHerramienta : Form
    {
        private sealed record Fila(int HerramientaId, string Codigo, string Nombre, string Categoria, string Marca, bool Activa);

        private const string TodasLasCategorias = "Todas las categorías";
        private const string SinCategoria = "Sin categoría";

        private readonly Panel pnlHeader = new();
        private readonly Label lblTitulo = new();
        private readonly Label lblSubtitulo = new();
        private readonly Panel pnlFiltros = new();
        private readonly IconPictureBox icoBuscar = new();
        private readonly MaterialTextBox txtBuscar = new();
        private readonly MaterialComboBox cboCategoria = new();
        private readonly Label lblResultados = new();
        private readonly Panel pnlGrid = new();
        private readonly Label lblSinResultados = new();
        private readonly Panel pnlFooter = new();
        private readonly Label lblSeleccion = new();
        private readonly MaterialButton btnSeleccionar = new();
        private readonly MaterialButton btnCancelar = new();

        // La grilla se crea después de ApplyTheme (ver EmpleadoGrid)
        private readonly DataGridView dgv = new();
        private readonly DataGridViewTextBoxColumn colMiniatura = EmpleadoGrid.Columna("", 52);
        private readonly DataGridViewTextBoxColumn colCodigo = EmpleadoGrid.Columna("CÓDIGO", 100);
        private readonly DataGridViewTextBoxColumn colNombre = EmpleadoGrid.Columna("NOMBRE");
        private readonly DataGridViewTextBoxColumn colCategoria = EmpleadoGrid.Columna("CATEGORÍA", 180);
        private readonly DataGridViewTextBoxColumn colMarca = EmpleadoGrid.Columna("MARCA", 150);

        private readonly bool _incluirInactivas;
        private List<Fila> _todas = new();

        public int HerramientaIdSeleccionada { get; private set; }
        public string NombreSeleccionado { get; private set; } = string.Empty;
        public string CodigoSeleccionado { get; private set; } = string.Empty;

        public FrmBuscarHerramienta(bool incluirInactivas = false)
        {
            _incluirInactivas = incluirInactivas;
            ConstruirUI();
            ThemeManager.ApplyTheme(this);
            ConstruirGrid();
            AplicarEstilos();

            Load += FrmBuscarHerramienta_Load;
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
            Text = "Seleccionar herramienta";

            // Encabezado
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 72;
            lblTitulo.Text = "Seleccionar herramienta";
            lblTitulo.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblTitulo.Location = new Point(22, 14);
            lblTitulo.AutoSize = true;
            lblSubtitulo.Text = "Busca por nombre, código, categoría o marca";
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
            txtBuscar.PlaceholderText = "Buscar por nombre, código o marca…";
            cboCategoria.Location = new Point(366, 8);
            cboCategoria.Size = new Size(220, 38);
            lblResultados.Font = new Font("Segoe UI", 9F);
            lblResultados.Size = new Size(220, 20);
            lblResultados.Location = new Point(618, 18);
            lblResultados.TextAlign = ContentAlignment.MiddleRight;
            pnlFiltros.Controls.AddRange(new Control[] { icoBuscar, txtBuscar, cboCategoria, lblResultados });

            // Grilla (se agrega en ConstruirGrid) + aviso sin resultados
            pnlGrid.Dock = DockStyle.Fill;
            pnlGrid.Padding = new Padding(20, 0, 20, 0);
            lblSinResultados.Dock = DockStyle.Fill;
            lblSinResultados.Text = "Ninguna herramienta coincide con la búsqueda.";
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
            cboCategoria.SelectedIndexChanged += (s, e) => FiltrarGrid();
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
            colMiniatura.Resizable = DataGridViewTriState.False;
            dgv.Dock = DockStyle.Fill;
            dgv.Columns.AddRange(colMiniatura, colCodigo, colNombre, colCategoria, colMarca);
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
        private void FrmBuscarHerramienta_Load(object? sender, EventArgs e)
        {
            AlinearDerecha();   // los paneles ya tienen su ancho final
            CargarHerramientas();
            txtBuscar.Focus();

            // Al mostrarse, la grilla preselecciona la primera fila: se quita para que un Enter
            // accidental no elija nada sin querer
            BeginInvoke(new Action(() => { dgv.ClearSelection(); ActualizarSeleccion(); }));
        }

        private void CargarHerramientas()
        {
            DataTable dt;
            try
            {
                dt = Db.Query($@"SELECT h.HerramientaId, h.Codigo, h.Nombre, h.Activa,
       ISNULL(c.Nombre, '{SinCategoria}') AS Categoria,
       ISNULL(m.NombreMarca, '')          AS Marca
FROM   Herramienta h
LEFT   JOIN CategoriaHerramienta c ON c.CategoriaId = h.CategoriaId
LEFT   JOIN Marca                m ON m.MarcaId     = h.MarcaId
{(_incluirInactivas ? "" : "WHERE  h.Activa = 1")}
ORDER  BY h.Activa DESC, h.Nombre ASC");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al cargar herramientas", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _todas = dt.AsEnumerable().Select(r => new Fila(
                r.Field<int>("HerramientaId"),
                r["Codigo"]?.ToString() ?? "",
                r["Nombre"]?.ToString() ?? "",
                r["Categoria"]?.ToString() ?? SinCategoria,
                r["Marca"]?.ToString() ?? "",
                r.Field<bool>("Activa"))).ToList();

            cboCategoria.Items.Clear();
            cboCategoria.Items.Add(TodasLasCategorias);
            foreach (var c in _todas.Select(f => f.Categoria).Distinct()
                                    .OrderBy(c => c == SinCategoria).ThenBy(c => c, StringComparer.CurrentCultureIgnoreCase))
                cboCategoria.Items.Add(c);
            cboCategoria.SelectedIndex = 0;

            FiltrarGrid();
        }

        // ── Filtro local (sin nuevas consultas a la BD) ────────────
        private void FiltrarGrid()
        {
            string texto = txtBuscar.Text.Trim();
            string? categoria = cboCategoria.SelectedIndex > 0 ? cboCategoria.SelectedItem as string : null;

            var visibles = _todas.Where(f =>
                (categoria == null || f.Categoria == categoria) &&
                (texto == "" || EmpleadoGrid.Contiene(f.Nombre, texto) || EmpleadoGrid.Contiene(f.Codigo, texto) ||
                 EmpleadoGrid.Contiene(f.Categoria, texto) || EmpleadoGrid.Contiene(f.Marca, texto))).ToList();

            dgv.SuspendLayout();
            dgv.Rows.Clear();
            foreach (var f in visibles)
            {
                int i = dgv.Rows.Add("", f.Codigo, f.Nombre, f.Categoria, f.Marca);
                dgv.Rows[i].Tag = f;
            }
            dgv.ResumeLayout();
            dgv.ClearSelection();
            dgv.CurrentCell = null;
            EmpleadoGrid.ReiniciarHover(dgv);

            lblResultados.Text = visibles.Count == 1 ? "1 herramienta encontrada" : $"{visibles.Count} herramientas encontradas";
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
                ? "Doble clic o Enter para elegir una herramienta"
                : string.Join("  ·  ", new[] { f.Codigo, f.Nombre, f.Categoria, f.Marca }.Where(s => s != ""));
            lblSeleccion.ForeColor = f == null ? ThemeManager.TextSecondary : ThemeManager.TextPrimary;
        }

        private void Dgv_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Graphics == null || dgv.Rows[e.RowIndex].Tag is not Fila f) return;

            EmpleadoGrid.PintarFondo(dgv, e);
            var g = e.Graphics;
            var b = e.CellBounds;
            if (e.ColumnIndex == colMiniatura.Index)
                HerramientaListItem.DibujarMiniatura(g, new Rectangle(b.X + (b.Width - 32) / 2, b.Y + (b.Height - 32) / 2, 32, 32),
                    f.Categoria, Paleta.Surface2, 16);
            else if (e.ColumnIndex == colCodigo.Index)
                EmpleadoGrid.DibujarTexto(g, b, f.Codigo, EmpleadoGrid.FuenteCodigo, ThemeManager.TextSecondary);
            else if (e.ColumnIndex == colNombre.Index)
                EmpleadoGrid.DibujarTexto(g, b, f.Activa ? f.Nombre : f.Nombre + "  (dada de baja)",
                    EmpleadoGrid.FuenteNombre, f.Activa ? ThemeManager.TextPrimary : ThemeManager.TextSecondary);
            else if (e.ColumnIndex == colCategoria.Index)
                EmpleadoGrid.DibujarChipDepartamento(g, b, f.Categoria);
            else if (e.ColumnIndex == colMarca.Index)
                EmpleadoGrid.DibujarTexto(g, b, f.Marca == "" ? "—" : f.Marca, dgv.DefaultCellStyle.Font!, ThemeManager.TextSecondary);
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

            HerramientaIdSeleccionada = f.HerramientaId;
            NombreSeleccionado = f.Nombre;
            CodigoSeleccionado = f.Codigo;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
