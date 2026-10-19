// FrmEmpleados.cs
using PromacoHerra.Controls;
using PromacoHerra.Models;
using PromacoHerra.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PromacoHerra
{
    // Lista de empleados (solo lectura). Se sincronizan desde la API de RRHH; aquí no se crean
    // ni editan. La búsqueda y el filtro por departamento trabajan sobre la lista en memoria.
    public partial class FrmEmpleados : Form
    {
        // ══════════════════════════════════════════════════════════
        // CAMPOS E INICIO
        // ══════════════════════════════════════════════════════════

        private const string TodosLosDepartamentos = "Todos los departamentos";
        private const string TextoSincronizar = "Sincronizar Empleados";

        private readonly EmpleadoService _service = new EmpleadoService();
        private List<EmpleadoDTO> _empleados = new();
        private ResultadoSincronizacion? _ultimoResultado;   // solo de esta sesión

        // La grilla se crea en código: así ThemeManager no le aplica su encabezado oscuro
        // ni su hover, y el diseñador de Visual Studio no puede perder sus columnas.
        private readonly DataGridView dgvEmpleados = new();
        private readonly DataGridViewTextBoxColumn colAvatar = EmpleadoGrid.Columna("", 44);
        private readonly DataGridViewTextBoxColumn colCodigo = EmpleadoGrid.Columna("CÓDIGO", 80);
        private readonly DataGridViewTextBoxColumn colNombre = EmpleadoGrid.Columna("NOMBRE");
        private readonly DataGridViewTextBoxColumn colDepartamento = EmpleadoGrid.Columna("DEPARTAMENTO", 200);

        // Arma la grilla, aplica colores y conecta los eventos: botón Sincronizar, caja de
        // búsqueda, combo de departamento y reacomodo de controles al cambiar el tamaño
        public FrmEmpleados()
        {
            InitializeComponent();
            ThemeManager.ApplyTheme(this);
            ConstruirGrid();
            AplicarEstilos();

            btnSincronizar.Click += BtnSincronizar_Click;
            txtBuscar.TextChanged += (s, e) => AplicarFiltros();
            cboDepartamento.SelectedIndexChanged += (s, e) => AplicarFiltros();
            this.Load += FrmEmpleados_Load;

            // Lo alineado a la derecha o centrado se ubica al cambiar de tamaño (sin Anchor)
            pnlHeader.Resize += (s, e) => btnSincronizar.Left = pnlHeader.Width - 24 - btnSincronizar.Width;
            pnlFiltros.Resize += (s, e) => lblMostrando.Left = pnlFiltros.Width - 24 - lblMostrando.Width;
            pnlVacio.Resize += (s, e) => CentrarEstadoVacio();
        }

        // Colores de fondo, textos secundarios e íconos según la paleta del sistema
        private void AplicarEstilos()
        {
            pnlHeader.BackColor = ThemeManager.AppBackground;
            pnlFiltros.BackColor = ThemeManager.AppBackground;
            lblSubtitulo.ForeColor = ThemeManager.TextSecondary;
            lblMostrando.ForeColor = ThemeManager.TextSecondary;
            lblVacioSubtitulo.ForeColor = ThemeManager.TextSecondary;
            icoBuscar.BackColor = ThemeManager.AppBackground;
            icoBuscar.IconColor = ThemeManager.TextSecondary;
            icoVacio.BackColor = Color.White;
            icoVacio.IconColor = Color.FromArgb(180, 188, 204);
            btnSincronizar.Variant = MaterialButtonVariant.Primary;
        }

        // ══════════════════════════════════════════════════════════
        // GRILLA
        // ══════════════════════════════════════════════════════════

        // Crea la tabla de empleados con sus 4 columnas (avatar, código, nombre, departamento)
        // y la agrega al panel de contenido
        private void ConstruirGrid()
        {
            colAvatar.Resizable = DataGridViewTriState.False;
            dgvEmpleados.Dock = DockStyle.Fill;
            dgvEmpleados.Columns.AddRange(colAvatar, colCodigo, colNombre, colDepartamento);
            EmpleadoGrid.Configurar(dgvEmpleados);
            dgvEmpleados.CellPainting += dgvEmpleados_CellPainting;

            pnlContenido.Controls.Add(dgvEmpleados);
            dgvEmpleados.BringToFront();
        }

        // Todo el contenido de las celdas se dibuja aquí (ver EmpleadoGrid)
        private void dgvEmpleados_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Graphics == null) return;
            if (dgvEmpleados.Rows[e.RowIndex].Tag is not EmpleadoDTO emp) return;

            EmpleadoGrid.PintarFondo(dgvEmpleados, e);
            var g = e.Graphics;
            if (e.ColumnIndex == colAvatar.Index)
                EmpleadoGrid.DibujarAvatar(g, e.CellBounds, emp.Codigo, emp.Nombre);
            else if (e.ColumnIndex == colCodigo.Index)
                EmpleadoGrid.DibujarTexto(g, e.CellBounds, emp.Codigo, EmpleadoGrid.FuenteCodigo, ThemeManager.TextSecondary);
            else if (e.ColumnIndex == colNombre.Index)
                EmpleadoGrid.DibujarTexto(g, e.CellBounds, emp.Nombre, EmpleadoGrid.FuenteNombre, ThemeManager.TextPrimary);
            else if (e.ColumnIndex == colDepartamento.Index)
                EmpleadoGrid.DibujarChipDepartamento(g, e.CellBounds, emp.Departamento);
            e.Handled = true;
        }

        // ══════════════════════════════════════════════════════════
        // CARGA Y FILTROS
        // ══════════════════════════════════════════════════════════

        // Al abrir el formulario se cargan los empleados guardados en la base de datos
        private void FrmEmpleados_Load(object? sender, EventArgs e)
        {
            CargarLocales();
            BeginInvoke(new Action(() => ActiveControl = null));   // que se vea el placeholder
        }

        // Lee los empleados de la base de datos local y refresca el combo, el subtítulo y la tabla
        private void CargarLocales()
        {
            try
            {
                _empleados = _service.ObtenerLocales();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Errores.Mensaje(ex), "Error al cargar empleados", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _empleados = new();
            }

            CargarDepartamentos();
            ActualizarSubtitulo();
            AplicarFiltros();
        }

        // Departamentos únicos de la lista cargada; conserva la selección si sigue existiendo
        private void CargarDepartamentos()
        {
            string? anterior = cboDepartamento.SelectedItem as string;
            cboDepartamento.Items.Clear();
            cboDepartamento.Items.Add(TodosLosDepartamentos);
            foreach (var d in _empleados.Select(e => e.Departamento)
                                        .Where(d => !string.IsNullOrWhiteSpace(d) && d != "—")
                                        .Distinct().OrderBy(d => d, StringComparer.CurrentCultureIgnoreCase))
                cboDepartamento.Items.Add(d);

            int idx = anterior == null ? 0 : cboDepartamento.Items.IndexOf(anterior);
            cboDepartamento.SelectedIndex = idx < 0 ? 0 : idx;
        }

        // Texto bajo el título: cantidad de empleados, fecha de la última sincronización y,
        // si se sincronizó en esta sesión, cuántos fueron nuevos o actualizados
        private void ActualizarSubtitulo()
        {
            var partes = new List<string> { _empleados.Count == 1 ? "1 empleado" : $"{_empleados.Count} empleados" };

            DateTime? ultima = null;
            try { ultima = _service.ObtenerUltimaSincronizacion(); } catch { /* el subtítulo no debe romper la carga */ }
            partes.Add(ultima is DateTime f ? $"Última sincronización: {f:dd/MM/yyyy HH:mm}" : "Sin sincronizar");

            if (_ultimoResultado != null)
                partes.Add($"{_ultimoResultado.Nuevos} nuevos, {_ultimoResultado.Actualizados} actualizados");

            lblSubtitulo.Text = string.Join("  ·  ", partes);
        }

        // Filtra la lista según el texto buscado (nombre o código) y el departamento elegido,
        // vuelve a llenar la tabla y muestra el estado "vacío" si no hay empleados
        private void AplicarFiltros()
        {
            string termino = txtBuscar.Text.Trim();
            string? depto = cboDepartamento.SelectedIndex > 0 ? cboDepartamento.SelectedItem as string : null;

            var visibles = _empleados.Where(e =>
                (depto == null || e.Departamento == depto) &&
                (termino == "" || EmpleadoGrid.Contiene(e.Nombre, termino) || EmpleadoGrid.Contiene(e.Codigo, termino))).ToList();

            dgvEmpleados.SuspendLayout();
            dgvEmpleados.Rows.Clear();
            foreach (var emp in visibles)
            {
                int i = dgvEmpleados.Rows.Add("", emp.Codigo, emp.Nombre, emp.Departamento);
                dgvEmpleados.Rows[i].Tag = emp;
            }
            dgvEmpleados.ResumeLayout();
            dgvEmpleados.ClearSelection();
            EmpleadoGrid.ReiniciarHover(dgvEmpleados);

            lblMostrando.Text = $"Mostrando {visibles.Count} de {_empleados.Count} empleados";

            // Estado vacío solo cuando no hay ningún empleado cargado (no cuando el filtro no encuentra nada)
            bool sinDatos = _empleados.Count == 0;
            pnlVacio.Visible = sinDatos;
            dgvEmpleados.Visible = !sinDatos;
            txtBuscar.Enabled = cboDepartamento.Enabled = !sinDatos;
            if (sinDatos) CentrarEstadoVacio();
        }

        // Centra el ícono y los textos del mensaje "no hay empleados"
        private void CentrarEstadoVacio()
        {
            int centroX = pnlVacio.Width / 2;
            int top = Math.Max(20, pnlVacio.Height / 2 - 80);
            icoVacio.Location = new Point(centroX - icoVacio.Width / 2, top);
            lblVacioTitulo.SetBounds(20, icoVacio.Bottom + 12, pnlVacio.Width - 40, 30);
            lblVacioSubtitulo.SetBounds(20, lblVacioTitulo.Bottom + 4, pnlVacio.Width - 40, 24);
        }

        // ══════════════════════════════════════════════════════════
        // SINCRONIZACIÓN
        // ══════════════════════════════════════════════════════════

        // Botón "Sincronizar Empleados": descarga los empleados de la API de Recursos Humanos
        // (en segundo plano, sin congelar la ventana), los guarda y recarga la lista
        private async void BtnSincronizar_Click(object? sender, EventArgs e)
        {
            btnSincronizar.Enabled = false;
            btnSincronizar.Text = "Sincronizando…";
            prgSincronizacion.Visible = true;

            try
            {
                _ultimoResultado = await _service.SincronizarAsync();
                CargarLocales();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Errores.Mensaje(ex), "Error de sincronización",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                prgSincronizacion.Visible = false;
                btnSincronizar.Text = TextoSincronizar;
                btnSincronizar.Enabled = true;
            }
        }
    }
}
