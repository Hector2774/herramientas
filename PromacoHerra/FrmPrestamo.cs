using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using PromacoHerra.Controls;
using PromacoHerra.Models;
using PromacoHerra.Services;

namespace PromacoHerra
{
    // Registro de préstamos:
    //   izquierda → empleado, aprobación/fechas, resumen de lo seleccionado y guardar
    //   derecha   → catálogo de herramientas (una tarjeta por tipo) con búsqueda y filtro por categoría
    // Cada tarjeta tiene un stepper: "+" asigna la siguiente unidad disponible y "−" quita la última;
    // "Ver unidades ›" permite elegir exactamente cuáles.
    public partial class FrmPrestamo : Form
    {
        private const string SinCategoria = "Sin categoría";

        private List<HerramientaCatalogoItem> _catalogo = new();                          // herramientas disponibles para prestar
        private readonly Dictionary<int, HerramientaCardControl> _tarjetas = new();     // HerramientaId → su tarjeta en pantalla

        // HerramientaId → UnidadIds seleccionadas, en el orden en que se agregaron
        private readonly Dictionary<int, List<int>> _seleccion = new();

        // Categorías marcadas en los chips de filtro (vacío = mostrar todas)
        private readonly HashSet<string> _categoriasActivas = new(StringComparer.OrdinalIgnoreCase);

        // Aplica estilos y conecta los eventos: carga, botones Guardar/Cancelar, selección de
        // empleado, búsqueda y doble clic en la lista de seleccionadas
        public FrmPrestamo()
        {
            InitializeComponent();
            ThemeManager.ApplyTheme(this);
            AplicarEstilos();

            this.Load += FrmPrestamo_Load;
            btnGuardar.Click += btnGuardar_Click;
            btnCancelar.Click += btnCancelar_Click;
            pickerEmpleado.EmpleadoSeleccionado += (id, nombre, depto, codigo) => ActualizarResumen();
            txtBuscar.TextChanged += (s, e) => AplicarFiltros();
            lvSeleccion.DoubleClick += lvSeleccion_DoubleClick;
            pnlIzquierdo.Paint += pnlIzquierdo_Paint;
        }

        // ThemeManager deja todas las etiquetas en TextPrimary; aquí se ajustan las secundarias
        private void AplicarEstilos()
        {
            foreach (var lbl in new[] { lblSecEmpleado, lblSecAprobacion, lblSecSeleccion })
                lbl.ForeColor = ThemeManager.TextSecondary;

            foreach (var lbl in new[] { lblAprobadoPorTitulo, lblFechaPrestamo, lblFechaDevolucion,
                                        lblObservaciones, lblSeleccionVacia, lblCatalogoVacio })
                lbl.ForeColor = ThemeManager.TextSecondary;

            foreach (var sep in new[] { sepEmpleado, sepAprobacion })
                sep.BackColor = ThemeManager.BorderColor;

            lvSeleccion.ForeColor = ThemeManager.TextPrimary;
            btnGuardar.Variant = MaterialButtonVariant.Primary;
            btnCancelar.Variant = MaterialButtonVariant.Secondary;
        }

        // ── Carga inicial ──────────────────────────────────────────
        // Al abrir: muestra quién aprueba (el usuario en sesión), pone las fechas por defecto
        // (devolución en 7 días) y carga el catálogo de herramientas
        private void FrmPrestamo_Load(object? sender, EventArgs e)
        {
            // Lo aprueba el empleado del usuario en sesión
            lblAprobadoPor.Text = Sesion.NombreEmpleado;

            dtpFecha.Value = DateTime.Now;
            dtpFechaDevolucion.MinDate = DateTime.Today.AddDays(1);
            dtpFechaDevolucion.Value = DateTime.Today.AddDays(7);

            CargarCatalogo();
        }

        // Trae de la base de datos las herramientas con unidades disponibles y redibuja
        // tarjetas, chips de categoría, filtros y resumen
        private void CargarCatalogo()
        {
            try
            {
                _catalogo = HerramientaService.ObtenerCatalogoPrestamo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Errores.Mensaje(ex), "Error al cargar herramientas",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                _catalogo = new();
            }

            // Descarta selecciones de unidades que ya no están disponibles
            var disponibles = _catalogo.SelectMany(h => h.Unidades).Select(u => u.UnidadId).ToHashSet();
            foreach (var id in _seleccion.Keys.ToList())
            {
                _seleccion[id].RemoveAll(u => !disponibles.Contains(u));
                if (_seleccion[id].Count == 0) _seleccion.Remove(id);
            }

            ConstruirTarjetas();
            ConstruirChips();
            AplicarFiltros();
            ActualizarResumen();
        }

        // Crea una tarjeta por herramienta del catálogo y conecta sus botones "+", "−" y "Ver unidades"
        private void ConstruirTarjetas()
        {
            flpCatalogo.SuspendLayout();

            foreach (Control c in flpCatalogo.Controls.Cast<Control>().ToList())
                c.Dispose();
            flpCatalogo.Controls.Clear();
            _tarjetas.Clear();

            foreach (var item in _catalogo)
            {
                var card = new HerramientaCardControl();
                card.Cargar(item);
                card.MasClick += (s, e) => AgregarUnidad(item);
                card.MenosClick += (s, e) => QuitarUnidad(item);
                card.VerUnidadesClick += (s, e) => AbrirSelectorUnidades(item);

                _tarjetas[item.HerramientaId] = card;
                flpCatalogo.Controls.Add(card);
            }

            flpCatalogo.ResumeLayout();
        }

        // ── Chips de categoría ─────────────────────────────────────
        // Crea un chip "Todas" + un chip por cada categoría del catálogo. Clic en un chip lo
        // activa o desactiva como filtro (se pueden combinar varias categorías)
        private void ConstruirChips()
        {
            var categorias = _catalogo.Select(CategoriaDe).Distinct(StringComparer.OrdinalIgnoreCase)
                                      .OrderBy(c => c == SinCategoria).ThenBy(c => c).ToList();

            _categoriasActivas.RemoveWhere(c => !categorias.Contains(c, StringComparer.OrdinalIgnoreCase));

            flpCategorias.SuspendLayout();
            foreach (Control c in flpCategorias.Controls.Cast<Control>().ToList())
                c.Dispose();
            flpCategorias.Controls.Clear();

            var chipTodas = new ChipToggle("Todas", null);
            chipTodas.Click += (s, e) => { _categoriasActivas.Clear(); AplicarFiltros(); };
            flpCategorias.Controls.Add(chipTodas);

            foreach (var cat in categorias)
            {
                var chip = new ChipToggle(cat, cat);
                chip.Click += (s, e) =>
                {
                    if (!_categoriasActivas.Remove(cat)) _categoriasActivas.Add(cat);
                    AplicarFiltros();
                };
                flpCategorias.Controls.Add(chip);
            }
            flpCategorias.ResumeLayout();
        }

        // Categoría de una herramienta, o "Sin categoría" si no tiene
        private static string CategoriaDe(HerramientaCatalogoItem h) =>
            string.IsNullOrWhiteSpace(h.Categoria) ? SinCategoria : h.Categoria;

        // ── Búsqueda + filtro por categoría ────────────────────────
        // Muestra u oculta cada tarjeta según las categorías marcadas y el texto buscado
        // (nombre, código, marca, categoría o código de unidad)
        private void AplicarFiltros()
        {
            foreach (ChipToggle chip in flpCategorias.Controls)
                chip.Seleccionado = chip.Categoria == null
                    ? _categoriasActivas.Count == 0
                    : _categoriasActivas.Contains(chip.Categoria);

            string termino = txtBuscar.Text.Trim();
            int visibles = 0;

            flpCatalogo.SuspendLayout();
            foreach (var item in _catalogo)
            {
                bool porCategoria = _categoriasActivas.Count == 0 || _categoriasActivas.Contains(CategoriaDe(item));
                bool porTexto = termino == ""
                    || Contiene(item.Nombre, termino) || Contiene(item.Codigo, termino)
                    || Contiene(item.Marca, termino) || Contiene(item.Categoria, termino)
                    || item.Unidades.Any(u => Contiene(u.Codigo, termino));

                bool visible = porCategoria && porTexto;
                _tarjetas[item.HerramientaId].Visible = visible;
                if (visible) visibles++;
            }
            flpCatalogo.ResumeLayout();

            lblCatalogoVacio.Text = _catalogo.Count == 0
                ? "No hay herramientas habilitadas para préstamo."
                : "No hay herramientas que coincidan con la búsqueda.";
            lblCatalogoVacio.Visible = visibles == 0;
            flpCatalogo.Visible = visibles > 0;
        }

        // Sin distinguir mayúsculas ni acentos ("electrica" encuentra "Eléctricas")
        private static bool Contiene(string texto, string termino) =>
            !string.IsNullOrEmpty(texto) &&
            CultureInfo.InvariantCulture.CompareInfo.IndexOf(texto, termino,
                CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0;

        // ── Selección de unidades ──────────────────────────────────
        // Unidades ya seleccionadas de una herramienta (lista vacía si ninguna)
        private List<int> SeleccionDe(HerramientaCatalogoItem item) =>
            _seleccion.TryGetValue(item.HerramientaId, out var lista) ? lista : new List<int>();

        // "+" → la siguiente unidad disponible que aún no esté seleccionada
        private void AgregarUnidad(HerramientaCatalogoItem item)
        {
            var actual = SeleccionDe(item);
            var siguiente = item.Unidades.FirstOrDefault(u => !actual.Contains(u.UnidadId));
            if (siguiente == null) return;

            actual.Add(siguiente.UnidadId);
            _seleccion[item.HerramientaId] = actual;
            ActualizarResumen();
        }

        // "−" → quita la última unidad seleccionada
        private void QuitarUnidad(HerramientaCatalogoItem item)
        {
            if (!_seleccion.TryGetValue(item.HerramientaId, out var actual) || actual.Count == 0) return;

            actual.RemoveAt(actual.Count - 1);
            if (actual.Count == 0) _seleccion.Remove(item.HerramientaId);
            ActualizarResumen();
        }

        // "Ver unidades ›" → abre FrmSeleccionarUnidades para elegir exactamente qué unidades prestar
        private void AbrirSelectorUnidades(HerramientaCatalogoItem item)
        {
            using var frm = new FrmSeleccionarUnidades(item, SeleccionDe(item));
            if (frm.ShowDialog(FindForm()) != DialogResult.OK) return;

            var elegidas = frm.UnidadesSeleccionadas;
            if (elegidas.Count == 0) _seleccion.Remove(item.HerramientaId);
            else _seleccion[item.HerramientaId] = elegidas;
            ActualizarResumen();
        }

        // Doble clic en la lista de seleccionadas → abre el selector de unidades de esa herramienta
        private void lvSeleccion_DoubleClick(object? sender, EventArgs e)
        {
            if (lvSeleccion.SelectedItems.Count == 0) return;
            int herramientaId = (int)lvSeleccion.SelectedItems[0].Tag!;
            var item = _catalogo.FirstOrDefault(h => h.HerramientaId == herramientaId);
            if (item != null) AbrirSelectorUnidades(item);
        }

        // Refresca tarjetas, lista de seleccionadas, total y el botón Guardar
        private void ActualizarResumen()
        {
            lvSeleccion.BeginUpdate();
            lvSeleccion.Items.Clear();
            int total = 0;

            foreach (var item in _catalogo)
            {
                var ids = SeleccionDe(item);
                if (_tarjetas.TryGetValue(item.HerramientaId, out var card) && card.Cantidad != ids.Count)
                    card.Cantidad = ids.Count;
                if (ids.Count == 0) continue;

                var codigos = item.Unidades.Where(u => ids.Contains(u.UnidadId)).Select(u => u.Codigo).ToList();
                var lvi = new ListViewItem(item.Nombre)
                {
                    Tag = item.HerramientaId,
                    ToolTipText = $"{item.Nombre}\n{string.Join(", ", codigos)}"
                };
                lvi.SubItems.Add(ids.Count.ToString());
                lvi.SubItems.Add(CodigosCompactos(codigos));
                lvSeleccion.Items.Add(lvi);
                total += ids.Count;
            }
            lvSeleccion.EndUpdate();

            lblSeleccionVacia.Visible = total == 0;
            lblTotal.Text = total == 1 ? "Total: 1 unidad" : $"Total: {total} unidades";
            btnGuardar.Enabled = pickerEmpleado.EmpleadoId != 0 && total > 0;
        }

        // "HER-0006-01, HER-0006-02" → "HER-0006-01, -02" para que quepa en la columna
        private static string CodigosCompactos(List<string> codigos)
        {
            if (codigos.Count == 0) return "";
            return codigos[0] + string.Concat(codigos.Skip(1).Select(c =>
            {
                int guion = c.LastIndexOf('-');
                return guion > 0 ? ", " + c[guion..] : ", " + c;
            }));
        }

        // ── Guardar ────────────────────────────────────────────────
        // Botón "Guardar": valida (empleado elegido, que no se apruebe a sí mismo, al menos una
        // herramienta y fecha de devolución futura) y registra el préstamo en la base de datos
        private void btnGuardar_Click(object? sender, EventArgs e)
        {
            if (pickerEmpleado.EmpleadoId == 0)
            {
                MessageBox.Show("Seleccione el empleado solicitante.", "Campo requerido",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (pickerEmpleado.EmpleadoId == Sesion.EmpleadoId)
            {
                MessageBox.Show("El empleado no puede aprobar su propio préstamo.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // UnidadIds en el orden del catálogo
            var ids = _catalogo.SelectMany(SeleccionDe).ToList();
            if (ids.Count == 0)
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

            try
            {
                int prestamoId = PrestamoService.Registrar(
                    empleadoId: pickerEmpleado.EmpleadoId,
                    fechaDevolucionEsperada: dtpFechaDevolucion.Value,
                    observaciones: txtObservaciones.Text.Trim(),
                    unidadIds: ids);

                MessageBox.Show(
                    $"Préstamo #{prestamoId} registrado correctamente.",
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Errores.Mensaje(ex), "Error al registrar préstamo",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Puede que otra persona haya prestado alguna unidad mientras tanto
                CargarCatalogo();
            }
        }

        // Botón "Cancelar": descarta lo que se estaba llenando
        private void btnCancelar_Click(object? sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        // Deja el formulario en blanco para un nuevo préstamo y recarga el catálogo
        private void LimpiarFormulario()
        {
            pickerEmpleado.Limpiar();
            txtObservaciones.Clear();
            dtpFecha.Value = DateTime.Now;
            dtpFechaDevolucion.Value = DateTime.Today.AddDays(7);

            _seleccion.Clear();
            CargarCatalogo();
        }

        // Línea gris a la derecha del panel izquierdo
        private void pnlIzquierdo_Paint(object? sender, PaintEventArgs e)
        {
            using var pen = new Pen(ThemeManager.BorderColor, 1);
            e.Graphics.DrawLine(pen, pnlIzquierdo.Width - 1, 0, pnlIzquierdo.Width - 1, pnlIzquierdo.Height);
        }
    }
}
