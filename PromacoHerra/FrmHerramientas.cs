using System;
using System.Data;
using System.Windows.Forms;
using PromacoHerra.Services;

namespace PromacoHerra
{
    public partial class FrmHerramientas : Form
    {
        private int? _herramientaId = null;
        private int? _categoriaId = null;
        private int? _marcaId = null;
        private int? _mantenimientoId = null;   // seleccionado en el grid de activos

        public FrmHerramientas()
        {
            InitializeComponent();
            ThemeManager.ApplyTheme(this);

            this.Load += FrmHerramientas_Load;

            // Herramientas
            btnNuevo.Click += btnNuevo_Click;
            btnGuardar.Click += btnGuardar_Click;
            btnEditar.Click += btnEditar_Click;
            btnCancelar.Click += btnCancelar_Click;
            btnEliminar.Click += btnEliminar_Click;
            dgvHerramientas.CellClick += dgvHerramientas_CellClick;
            txtBuscar.TextChanged += txtBuscar_TextChanged;

            // Categorías
            btnNuevoCategoria.Click += btnNuevoCategoria_Click;
            btnGuardarCategoria.Click += btnGuardarCategoria_Click;
            btnEditarCategoria.Click += btnEditarCategoria_Click;
            btnCancelarCategoria.Click += btnCancelarCategoria_Click;
            btnEliminarCategoria.Click += btnEliminarCategoria_Click;
            dgvCategorias.CellClick += dgvCategorias_CellClick;

            // Marcas
            btnNuevoMarca.Click += btnNuevoMarca_Click;
            btnGuardarMarca.Click += btnGuardarMarca_Click;
            btnEditarMarca.Click += btnEditarMarca_Click;
            btnCancelarMarca.Click += btnCancelarMarca_Click;
            btnEliminarMarca.Click += btnEliminarMarca_Click;
            dgvMarcas.CellClick += dgvMarcas_CellClick;

            // Mantenimiento
            dgvMantenimientos.CellClick += dgvMantenimientos_CellClick;
            btnAbrirMantenimiento.Click += btnAbrirMantenimiento_Click;
            btnCerrarMantenimiento.Click += btnCerrarMantenimiento_Click;
            btnVerHistorial.Click += btnVerHistorial_Click;
        }

        // ══════════════════════════════════════════════════════════
        // CARGA INICIAL
        // ══════════════════════════════════════════════════════════
        private void FrmHerramientas_Load(object sender, EventArgs e)
        {
            CargarCombos();
            CargarHerramientas(); HabilitarControlesH(false);
            CargarCategorias(); HabilitarControlesC(false);
            CargarMarcas(); HabilitarControlesM(false);
            CargarMantenimientos();
            HabilitarCierre(false);
        }

        private void CargarCombos()
        {
            cboCategoria.DataSource = CategoriaService.ObtenerTodas();
            cboCategoria.DisplayMember = "Nombre";
            cboCategoria.ValueMember = "CategoriaId";
            cboCategoria.SelectedIndex = -1;

            cboMarca.DataSource = MarcaService.ObtenerTodas();
            cboMarca.DisplayMember = "Nombre";
            cboMarca.ValueMember = "MarcaId";
            cboMarca.SelectedIndex = -1;

            cboUbicacion.DataSource = UbicacionService.ObtenerTodas();
            cboUbicacion.DisplayMember = "Nombre";
            cboUbicacion.ValueMember = "UbicacionId";
            cboUbicacion.SelectedIndex = -1;

            // Combo de herramientas para abrir mantenimiento
            // Solo muestra disponibles y dañadas (no las que ya están en mantenimiento)
            var dtH = PromacoHerra.Data.Db.Query(@"
                SELECT HerramientaId,
                       Codigo + ' — ' + Nombre AS Display
                FROM   Herramienta
                WHERE  Activa = 1
                  AND  Estado <> 'En Mantenimiento'
                ORDER  BY Nombre");
            cboHerramientaMant.DataSource = dtH;
            cboHerramientaMant.DisplayMember = "Display";
            cboHerramientaMant.ValueMember = "HerramientaId";
            cboHerramientaMant.SelectedIndex = -1;

            // Tipo de mantenimiento
            if (cboTipoMant.Items.Count == 0)
            {
                cboTipoMant.Items.Add("Preventivo");
                cboTipoMant.Items.Add("Correctivo");
            }
            cboTipoMant.SelectedIndex = 0;
        }

        // ══════════════════════════════════════════════════════════
        // HERRAMIENTAS  (igual que antes)
        // ══════════════════════════════════════════════════════════
        private void CargarHerramientas()
        {
            dgvHerramientas.DataSource = HerramientaService.ObtenerTodas();
            OcultarCol(dgvHerramientas, "HerramientaId");
            OcultarCol(dgvHerramientas, "CategoriaId");
            OcultarCol(dgvHerramientas, "MarcaId");
            OcultarCol(dgvHerramientas, "UbicacionId");
            OcultarCol(dgvHerramientas, "Activa");
            RenombrarCol(dgvHerramientas, "Codigo", "Código");
            RenombrarCol(dgvHerramientas, "Categoria", "Categoría");
            RenombrarCol(dgvHerramientas, "Ubicacion", "Ubicación");
            RenombrarCol(dgvHerramientas, "Caracteristicas", "Características");
            RenombrarCol(dgvHerramientas, "StockTotal", "Stock Total");
            RenombrarCol(dgvHerramientas, "StockDisponible", "Disponible");
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            var t = txtBuscar.Text.Trim();
            dgvHerramientas.DataSource = string.IsNullOrEmpty(t)
                ? HerramientaService.ObtenerTodas()
                : HerramientaService.Buscar(t);
            OcultarCol(dgvHerramientas, "HerramientaId");
            OcultarCol(dgvHerramientas, "CategoriaId");
            OcultarCol(dgvHerramientas, "MarcaId");
            OcultarCol(dgvHerramientas, "UbicacionId");
        }

        private void dgvHerramientas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvHerramientas.Rows[e.RowIndex];
            _herramientaId = Convert.ToInt32(row.Cells["HerramientaId"].Value);
            txtCodigo.Text = row.Cells["Codigo"].Value?.ToString();
            txtNombre.Text = row.Cells["Nombre"].Value?.ToString();
            txtCaracteristicas.Text = row.Cells["Caracteristicas"].Value?.ToString();
            nudStockTotal.Value = Convert.ToDecimal(row.Cells["StockTotal"].Value ?? 1);
            cboCategoria.SelectedValue = row.Cells["CategoriaId"].Value ?? DBNull.Value;
            cboMarca.SelectedValue = row.Cells["MarcaId"].Value ?? DBNull.Value;
            cboUbicacion.SelectedValue = row.Cells["UbicacionId"].Value ?? DBNull.Value;
            btnEditar.Enabled = true;
            HabilitarControlesH(false);
        }

        private void btnNuevo_Click(object sender, EventArgs e) { _herramientaId = null; LimpiarH(); btnEditar.Enabled = false; HabilitarControlesH(true); txtCodigo.Focus(); }
        private void btnEditar_Click(object sender, EventArgs e) { if (_herramientaId == null) { Aviso("Seleccione una herramienta."); return; } btnEditar.Enabled = false; HabilitarControlesH(true); }
        private void btnCancelar_Click(object sender, EventArgs e) { _herramientaId = null; LimpiarH(); HabilitarControlesH(false); }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodigo.Text) || string.IsNullOrWhiteSpace(txtNombre.Text))
            { Aviso("Código y Nombre son obligatorios."); return; }
            int? catId = cboCategoria.SelectedValue as int?;
            int? marId = cboMarca.SelectedValue as int?;
            int? ubiId = cboUbicacion.SelectedValue as int?;
            int stock = (int)nudStockTotal.Value;
            try
            {
                if (_herramientaId == null)
                    HerramientaService.Insertar(txtCodigo.Text.Trim(), txtNombre.Text.Trim(), txtCaracteristicas.Text.Trim(), catId, marId, ubiId, stock);
                else
                    HerramientaService.Actualizar(_herramientaId.Value, txtCodigo.Text.Trim(), txtNombre.Text.Trim(), txtCaracteristicas.Text.Trim(), catId, marId, ubiId, stock);
                OK("Herramienta guardada."); CargarHerramientas(); LimpiarH(); HabilitarControlesH(false); _herramientaId = null;
            }
            catch (Exception ex) { Error(ex.Message); }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_herramientaId == null) { Aviso("Seleccione una herramienta."); return; }
            if (Confirmar("¿Dar de baja esta herramienta?") != DialogResult.Yes) return;
            try { HerramientaService.DarDeBaja(_herramientaId.Value); OK("Dada de baja."); CargarHerramientas(); LimpiarH(); HabilitarControlesH(false); _herramientaId = null; }
            catch (Exception ex) { Error(ex.Message); }
        }

        private void LimpiarH() { txtCodigo.Clear(); txtNombre.Clear(); txtCaracteristicas.Clear(); nudStockTotal.Value = 1; cboCategoria.SelectedIndex = -1; cboMarca.SelectedIndex = -1; cboUbicacion.SelectedIndex = -1; }
        private void HabilitarControlesH(bool on) { txtCodigo.Enabled = txtNombre.Enabled = txtCaracteristicas.Enabled = nudStockTotal.Enabled = cboCategoria.Enabled = cboMarca.Enabled = cboUbicacion.Enabled = on; btnGuardar.Enabled = on; btnCancelar.Enabled = on; btnNuevo.Enabled = !on; btnEliminar.Enabled = !on && _herramientaId != null; }

        // ══════════════════════════════════════════════════════════
        // CATEGORÍAS
        // ══════════════════════════════════════════════════════════
        private void CargarCategorias() { dgvCategorias.DataSource = CategoriaService.ObtenerTodas(); OcultarCol(dgvCategorias, "CategoriaId"); RenombrarCol(dgvCategorias, "Descripcion", "Descripción"); }
        private void dgvCategorias_CellClick(object sender, DataGridViewCellEventArgs e) { if (e.RowIndex < 0) return; var r = dgvCategorias.Rows[e.RowIndex]; _categoriaId = Convert.ToInt32(r.Cells["CategoriaId"].Value); txtNombreCategoria.Text = r.Cells["Nombre"].Value?.ToString(); txtDescCategoria.Text = r.Cells["Descripcion"].Value?.ToString(); btnEditarCategoria.Enabled = true; HabilitarControlesC(false); }
        private void btnNuevoCategoria_Click(object sender, EventArgs e) { _categoriaId = null; txtNombreCategoria.Clear(); txtDescCategoria.Clear(); btnEditarCategoria.Enabled = false; HabilitarControlesC(true); txtNombreCategoria.Focus(); }
        private void btnEditarCategoria_Click(object sender, EventArgs e) { if (_categoriaId == null) { Aviso("Seleccione una categoría."); return; } btnEditarCategoria.Enabled = false; HabilitarControlesC(true); }
        private void btnCancelarCategoria_Click(object sender, EventArgs e) { _categoriaId = null; txtNombreCategoria.Clear(); txtDescCategoria.Clear(); HabilitarControlesC(false); }
        private void btnGuardarCategoria_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombreCategoria.Text)) { Aviso("El nombre es obligatorio."); return; }
            try { if (_categoriaId == null) CategoriaService.Insertar(txtNombreCategoria.Text.Trim(), txtDescCategoria.Text.Trim()); else CategoriaService.Actualizar(_categoriaId.Value, txtNombreCategoria.Text.Trim(), txtDescCategoria.Text.Trim()); OK("Categoría guardada."); CargarCategorias(); CargarCombos(); txtNombreCategoria.Clear(); txtDescCategoria.Clear(); HabilitarControlesC(false); _categoriaId = null; }
            catch (Exception ex) { Error(ex.Message); }
        }
        private void btnEliminarCategoria_Click(object sender, EventArgs e)
        {
            if (_categoriaId == null) { Aviso("Seleccione una categoría."); return; }
            if (Confirmar("¿Eliminar esta categoría?") != DialogResult.Yes) return;
            try { PromacoHerra.Data.Db.Execute("UPDATE Herramienta SET CategoriaId = NULL WHERE CategoriaId = @Id", new Microsoft.Data.SqlClient.SqlParameter("@Id", _categoriaId)); PromacoHerra.Data.Db.Execute("DELETE FROM CategoriaHerramienta WHERE CategoriaId = @Id", new Microsoft.Data.SqlClient.SqlParameter("@Id", _categoriaId)); OK("Categoría eliminada."); CargarCategorias(); CargarCombos(); txtNombreCategoria.Clear(); txtDescCategoria.Clear(); HabilitarControlesC(false); _categoriaId = null; }
            catch (Exception ex) { Error(ex.Message); }
        }
        private void HabilitarControlesC(bool on) { txtNombreCategoria.Enabled = txtDescCategoria.Enabled = on; btnGuardarCategoria.Enabled = on; btnCancelarCategoria.Enabled = on; btnNuevoCategoria.Enabled = !on; btnEliminarCategoria.Enabled = !on && _categoriaId != null; }

        // ══════════════════════════════════════════════════════════
        // MARCAS
        // ══════════════════════════════════════════════════════════
        private void CargarMarcas() { dgvMarcas.DataSource = MarcaService.ObtenerTodas(); OcultarCol(dgvMarcas, "MarcaId"); RenombrarCol(dgvMarcas, "Descripcion", "Descripción"); }
        private void dgvMarcas_CellClick(object sender, DataGridViewCellEventArgs e) { if (e.RowIndex < 0) return; var r = dgvMarcas.Rows[e.RowIndex]; _marcaId = Convert.ToInt32(r.Cells["MarcaId"].Value); txtNombreMarca.Text = r.Cells["Nombre"].Value?.ToString(); txtDescMarca.Text = r.Cells["Descripcion"].Value?.ToString(); btnEditarMarca.Enabled = true; HabilitarControlesM(false); }
        private void btnNuevoMarca_Click(object sender, EventArgs e) { _marcaId = null; txtNombreMarca.Clear(); txtDescMarca.Clear(); btnEditarMarca.Enabled = false; HabilitarControlesM(true); txtNombreMarca.Focus(); }
        private void btnEditarMarca_Click(object sender, EventArgs e) { if (_marcaId == null) { Aviso("Seleccione una marca."); return; } btnEditarMarca.Enabled = false; HabilitarControlesM(true); }
        private void btnCancelarMarca_Click(object sender, EventArgs e) { _marcaId = null; txtNombreMarca.Clear(); txtDescMarca.Clear(); HabilitarControlesM(false); }
        private void btnGuardarMarca_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombreMarca.Text)) { Aviso("El nombre es obligatorio."); return; }
            try { if (_marcaId == null) MarcaService.Insertar(txtNombreMarca.Text.Trim(), txtDescMarca.Text.Trim()); else MarcaService.Actualizar(_marcaId.Value, txtNombreMarca.Text.Trim(), txtDescMarca.Text.Trim()); OK("Marca guardada."); CargarMarcas(); CargarCombos(); txtNombreMarca.Clear(); txtDescMarca.Clear(); HabilitarControlesM(false); _marcaId = null; }
            catch (Exception ex) { Error(ex.Message); }
        }
        private void btnEliminarMarca_Click(object sender, EventArgs e)
        {
            if (_marcaId == null) { Aviso("Seleccione una marca."); return; }
            if (Confirmar("¿Eliminar esta marca?") != DialogResult.Yes) return;
            try { PromacoHerra.Data.Db.Execute("UPDATE Herramienta SET MarcaId = NULL WHERE MarcaId = @Id", new Microsoft.Data.SqlClient.SqlParameter("@Id", _marcaId)); PromacoHerra.Data.Db.Execute("DELETE FROM Marca WHERE MarcaId = @Id", new Microsoft.Data.SqlClient.SqlParameter("@Id", _marcaId)); OK("Marca eliminada."); CargarMarcas(); CargarCombos(); txtNombreMarca.Clear(); txtDescMarca.Clear(); HabilitarControlesM(false); _marcaId = null; }
            catch (Exception ex) { Error(ex.Message); }
        }
        private void HabilitarControlesM(bool on) { txtNombreMarca.Enabled = txtDescMarca.Enabled = on; btnGuardarMarca.Enabled = on; btnCancelarMarca.Enabled = on; btnNuevoMarca.Enabled = !on; btnEliminarMarca.Enabled = !on && _marcaId != null; }

        // ══════════════════════════════════════════════════════════
        // MANTENIMIENTO
        // ══════════════════════════════════════════════════════════
        private void CargarMantenimientos()
        {
            dgvMantenimientos.DataSource = MantenimientoService.ObtenerActivos();

            OcultarCol(dgvMantenimientos, "MantenimientoId");
            OcultarCol(dgvMantenimientos, "HerramientaId");

            RenombrarCol(dgvMantenimientos, "CodigoHerramienta", "Código");
            RenombrarCol(dgvMantenimientos, "Herramienta", "Herramienta");
            RenombrarCol(dgvMantenimientos, "Categoria", "Categoría");
            RenombrarCol(dgvMantenimientos, "Marca", "Marca");
            RenombrarCol(dgvMantenimientos, "Ubicacion", "Ubicación");
            RenombrarCol(dgvMantenimientos, "TipoMantenimiento", "Tipo");
            RenombrarCol(dgvMantenimientos, "FechaInicio", "Fecha inicio");
            RenombrarCol(dgvMantenimientos, "RealizadoPor", "Realizado por");
            RenombrarCol(dgvMantenimientos, "DiasEnMantenimiento", "Días");
            RenombrarCol(dgvMantenimientos, "Descripcion", "Descripción");

            // Resaltar en naranja los que llevan más de 7 días
            foreach (DataGridViewRow row in dgvMantenimientos.Rows)
            {
                if (!dgvMantenimientos.Columns.Contains("DiasEnMantenimiento")) break;
                if (row.Cells["DiasEnMantenimiento"].Value == DBNull.Value) continue;
                int dias = Convert.ToInt32(row.Cells["DiasEnMantenimiento"].Value);
                if (dias > 7)
                {
                    row.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, 235, 180);
                    row.DefaultCellStyle.ForeColor = System.Drawing.Color.DarkOrange;
                }
            }

            _mantenimientoId = null;
            HabilitarCierre(false);
        }

        // Seleccionar un mantenimiento activo → habilita sección cerrar
        private void dgvMantenimientos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvMantenimientos.Rows[e.RowIndex];
            _mantenimientoId = Convert.ToInt32(row.Cells["MantenimientoId"].Value);
            HabilitarCierre(true);
            txtCosto.Clear();
            txtDescCierre.Clear();
        }

        // Abrir nuevo mantenimiento
        private void btnAbrirMantenimiento_Click(object sender, EventArgs e)
        {
            if (cboHerramientaMant.SelectedValue == null)
            { Aviso("Seleccione una herramienta."); return; }
            if (cboTipoMant.SelectedIndex < 0)
            { Aviso("Seleccione el tipo de mantenimiento."); return; }

            try
            {
                int id = MantenimientoService.RegistrarEntrada(
                    herramientaId: Convert.ToInt32(cboHerramientaMant.SelectedValue),
                    tipoMantenimiento: cboTipoMant.SelectedItem.ToString(),
                    descripcion: txtDescMant.Text.Trim(),
                    realizadoPor: txtRealizadoPor.Text.Trim());

                OK($"Mantenimiento #{id} abierto. La herramienta quedó fuera de stock.");
                LimpiarAbrir();
                CargarMantenimientos();
                CargarCombos();         // actualizar combo de herramientas disponibles
                CargarHerramientas();   // reflejar nuevo estado en la pestaña principal
            }
            catch (Exception ex) { Error(ex.Message); }
        }

        // Cerrar mantenimiento seleccionado
        private void btnCerrarMantenimiento_Click(object sender, EventArgs e)
        {
            if (_mantenimientoId == null)
            { Aviso("Seleccione un mantenimiento de la lista."); return; }

            decimal? costo = null;
            if (!string.IsNullOrWhiteSpace(txtCosto.Text))
            {
                if (!decimal.TryParse(txtCosto.Text.Trim(), out decimal c))
                { Aviso("El costo debe ser un número válido (ej: 250.00)."); return; }
                costo = c;
            }

            if (Confirmar("¿Cerrar este mantenimiento? La herramienta volverá al stock disponible.") != DialogResult.Yes) return;

            try
            {
                MantenimientoService.RegistrarSalida(
                    mantenimientoId: _mantenimientoId.Value,
                    descripcion: txtDescCierre.Text.Trim(),
                    costo: costo);

                OK("Mantenimiento cerrado. La herramienta volvió a estado Disponible.");
                txtCosto.Clear(); txtDescCierre.Clear();
                CargarMantenimientos();
                CargarCombos();
                CargarHerramientas();
            }
            catch (Exception ex) { Error(ex.Message); }
        }

        // Ver historial de la herramienta seleccionada en el grid
        private void btnVerHistorial_Click(object sender, EventArgs e)
        {
            if (_mantenimientoId == null)
            { Aviso("Seleccione un mantenimiento de la lista para ver el historial de esa herramienta."); return; }

            // Obtener HerramientaId desde la fila seleccionada
            int herramientaId = 0;
            foreach (DataGridViewRow row in dgvMantenimientos.Rows)
            {
                if (row.Cells["MantenimientoId"].Value != null &&
                    Convert.ToInt32(row.Cells["MantenimientoId"].Value) == _mantenimientoId)
                {
                    herramientaId = Convert.ToInt32(row.Cells["HerramientaId"].Value);
                    break;
                }
            }

            if (herramientaId == 0) return;

            var dt = MantenimientoService.ObtenerPorHerramienta(herramientaId);

            // Mostrar en un formulario simple de solo lectura
            var frmHistorial = new Form
            {
                Text = "Historial de Mantenimiento",
                Size = new System.Drawing.Size(800, 450),
                StartPosition = FormStartPosition.CenterParent
            };
            var dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                DataSource = dt,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            frmHistorial.Controls.Add(dgv);
            frmHistorial.ShowDialog(this);
        }

        private void LimpiarAbrir()
        {
            cboHerramientaMant.SelectedIndex = -1;
            cboTipoMant.SelectedIndex = 0;
            txtRealizadoPor.Clear();
            txtDescMant.Clear();
        }

        private void HabilitarCierre(bool on)
        {
            txtCosto.Enabled = on;
            txtDescCierre.Enabled = on;
            btnCerrarMantenimiento.Enabled = on;
            btnVerHistorial.Enabled = on;
        }

        // ══════════════════════════════════════════════════════════
        // HELPERS COMPARTIDOS
        // ══════════════════════════════════════════════════════════
        private void OcultarCol(DataGridView dgv, string col) { if (dgv.Columns.Contains(col)) dgv.Columns[col].Visible = false; }
        private void RenombrarCol(DataGridView dgv, string col, string h) { if (dgv.Columns.Contains(col)) dgv.Columns[col].HeaderText = h; }
        private void Aviso(string m) => MessageBox.Show(m, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        private void OK(string m) => MessageBox.Show(m, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        private void Error(string m) => MessageBox.Show(m, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        private DialogResult Confirmar(string m) => MessageBox.Show(m, "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
    }
}