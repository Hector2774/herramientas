using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PromacoHerra.Controls;
using PromacoHerra.Services;

namespace PromacoHerra
{
    // Abre un mantenimiento para una o más unidades (pueden ser de distintas herramientas):
    //   unidades → búsqueda + grilla con casillas; las dañadas (pendientes de reparación) van primero
    //              con quién reportó el daño
    //   detalle  → tipo, quién lo realiza (empleado si es interno, proveedor si es externo) y descripción
    // Se usa desde la pestaña Mantenimiento (nuevo o desde un pendiente) y desde FrmUnidades.
    public class FrmAbrirMantenimiento : Form
    {
        private static readonly Font FuenteCodigo = new("Consolas", 9.5F);

        private readonly HashSet<int> _preseleccion;
        private DataTable _unidades = new();
        private DataView? _vista;

        // Unidades
        private readonly MaterialTextBox txtBuscar = new();
        private readonly MaterialCheckBox chkSoloDañadas = new();
        private readonly Label lblSeleccion = new();
        private readonly DataGridView dgvUnidades = new();
        private readonly DataGridViewCheckBoxColumn colSel = new();
        private readonly DataGridViewTextBoxColumn colCodigo = new();
        private readonly DataGridViewTextBoxColumn colHerramienta = new();
        private readonly DataGridViewTextBoxColumn colEstado = new();
        private readonly DataGridViewTextBoxColumn colReporte = new();
        private readonly DataGridViewTextBoxColumn colUbicacion = new();

        // Detalle
        private readonly MaterialComboBox cboTipo = new();
        private readonly MaterialRadioButton rbInterno = new();
        private readonly MaterialRadioButton rbExterno = new();
        private readonly EmpleadoPickerControl pickEmpleado = new();
        private readonly MaterialComboBox cboProveedor = new();
        private readonly Label lblResponsable = new();
        private readonly Label lblSinProveedores = new();
        private readonly MaterialTextBox txtDescripcion = new();
        private readonly MaterialButton btnAbrir = new();
        private readonly MaterialButton btnCancelar = new();

        /// <summary>Cuántas unidades entraron a mantenimiento (0 si se canceló).</summary>
        public int UnidadesRegistradas { get; private set; }

        /// <param name="preseleccion">Unidades que llegan marcadas (desde un pendiente o desde FrmUnidades).</param>
        /// <param name="filtro">Texto inicial del buscador (p. ej. el código de la herramienta).</param>
        public FrmAbrirMantenimiento(IEnumerable<int>? preseleccion = null, string? filtro = null)
        {
            _preseleccion = new HashSet<int>(preseleccion ?? Enumerable.Empty<int>());
            ConstruirInterfaz();
            ThemeManager.ApplyTheme(this);
            AplicarEstilos();
            Load += (_, _) => Cargar(filtro);
        }

        // ══════════════════════════════════════════════════════════
        // INTERFAZ
        // ══════════════════════════════════════════════════════════
        private void ConstruirInterfaz()
        {
            Text = "Nuevo mantenimiento";
            Size = new Size(1040, 760);
            MinimumSize = new Size(900, 640);
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = false;
            MinimizeBox = false;

            // ── Encabezado ──
            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 78 };
            var lblTitulo = new Label
            {
                Text = "Nuevo mantenimiento",
                Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                Location = new Point(24, 12),
                AutoSize = true
            };
            var lblSub = new Label
            {
                Text = "Marque las unidades a reparar o revisar. Las dañadas que esperan reparación aparecen primero.",
                Font = new Font("Segoe UI", 10F),
                ForeColor = ThemeManager.TextSecondary,
                Location = new Point(24, 44),
                AutoSize = true
            };
            pnlHeader.Controls.AddRange(new Control[] { lblTitulo, lblSub });

            // ── Barra de búsqueda ──
            var pnlBuscar = new Panel { Dock = DockStyle.Top, Height = 50, Padding = new Padding(24, 6, 24, 6) };
            txtBuscar.PlaceholderText = "Buscar por código, herramienta, categoría, ubicación o quién reportó…";
            txtBuscar.Location = new Point(24, 6);
            txtBuscar.Size = new Size(420, 38);
            txtBuscar.TextChanged += (_, _) => Filtrar();
            chkSoloDañadas.Text = "Solo dañadas";
            chkSoloDañadas.AutoSize = true;
            chkSoloDañadas.Location = new Point(460, 14);
            chkSoloDañadas.CheckedChanged += (_, _) => Filtrar();
            lblSeleccion.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblSeleccion.TextAlign = ContentAlignment.MiddleRight;
            lblSeleccion.Size = new Size(260, 38);
            lblSeleccion.Location = new Point(700, 6);
            pnlBuscar.Controls.AddRange(new Control[] { txtBuscar, chkSoloDañadas, lblSeleccion });
            pnlBuscar.Resize += (_, _) => lblSeleccion.Left = pnlBuscar.Width - 24 - lblSeleccion.Width;

            // ── Grilla de unidades ──
            colSel.DataPropertyName = "Sel";
            colSel.HeaderText = "";
            colSel.Width = 44;
            colSel.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            Columna(colCodigo, "CodigoUnidad", "Código unidad", 15);
            Columna(colHerramienta, "Herramienta", "Herramienta", 28);
            Columna(colEstado, "Estado", "Estado", 13);
            Columna(colReporte, "ReporteTexto", "Daño reportado", 30);
            Columna(colUbicacion, "Ubicacion", "Ubicación", 16);

            dgvUnidades.AutoGenerateColumns = false;
            dgvUnidades.Columns.AddRange(colSel, colCodigo, colHerramienta, colEstado, colReporte, colUbicacion);
            dgvUnidades.Dock = DockStyle.Fill;
            dgvUnidades.AllowUserToAddRows = false;
            dgvUnidades.AllowUserToDeleteRows = false;
            dgvUnidades.CellPainting += dgvUnidades_CellPainting;
            dgvUnidades.CurrentCellDirtyStateChanged += (_, _) =>
            {
                if (dgvUnidades.IsCurrentCellDirty) dgvUnidades.CommitEdit(DataGridViewDataErrorContexts.Commit);
            };
            dgvUnidades.CellValueChanged += (_, e) => { if (e.ColumnIndex == colSel.Index) ActualizarSeleccion(); };
            // Clic en cualquier parte de la fila marca/desmarca la unidad
            dgvUnidades.CellClick += (_, e) =>
            {
                if (e.RowIndex < 0 || e.ColumnIndex == colSel.Index ||
                    dgvUnidades.Rows[e.RowIndex].DataBoundItem is not DataRowView r) return;
                r["Sel"] = !(bool)r["Sel"];
                ActualizarSeleccion();
            };

            var pnlGrid = new Panel { Dock = DockStyle.Fill, Padding = new Padding(24, 4, 24, 12) };
            pnlGrid.Controls.Add(dgvUnidades);

            // ── Detalle ──
            var tlpDetalle = new TableLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 196,
                Padding = new Padding(24, 14, 24, 8),
                ColumnCount = 3,
                RowCount = 4
            };
            tlpDetalle.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220F));
            tlpDetalle.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 230F));
            tlpDetalle.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpDetalle.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tlpDetalle.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            tlpDetalle.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tlpDetalle.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            cboTipo.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTipo.Items.AddRange(MantenimientoService.Tipos);
            cboTipo.Dock = DockStyle.Fill;
            cboTipo.Margin = new Padding(0, 0, 14, 0);
            // La calibración la hace casi siempre un laboratorio externo que emite certificado
            cboTipo.SelectedIndexChanged += (_, _) =>
            {
                if (cboTipo.SelectedItem as string == "Calibración") rbExterno.Checked = true;
            };

            var flpServicio = new FlowLayoutPanel { Dock = DockStyle.Fill, Margin = new Padding(0), WrapContents = false };
            rbInterno.Text = "Interno";
            rbExterno.Text = "Externo";
            foreach (var rb in new[] { rbInterno, rbExterno })
            {
                rb.AutoSize = true;
                rb.Margin = new Padding(0, 10, 18, 0);
                rb.CheckedChanged += (_, _) => ActualizarResponsable();
                flpServicio.Controls.Add(rb);
            }

            // Empleado y proveedor comparten la celda; solo uno está visible
            var pnlResponsable = new Panel { Dock = DockStyle.Fill, Margin = new Padding(0) };
            pickEmpleado.Dock = DockStyle.Top;
            pickEmpleado.Height = 38;
            cboProveedor.Dock = DockStyle.Top;
            cboProveedor.DropDownStyle = ComboBoxStyle.DropDownList;
            lblSinProveedores.Dock = DockStyle.Fill;
            lblSinProveedores.Text = "No hay proveedores registrados. Agréguelos en Herramientas → Catálogos → Proveedores.";
            lblSinProveedores.TextAlign = ContentAlignment.MiddleLeft;
            pnlResponsable.Controls.AddRange(new Control[] { pickEmpleado, cboProveedor, lblSinProveedores });

            txtDescripcion.Multiline = true;
            txtDescripcion.Dock = DockStyle.Fill;
            txtDescripcion.MaxLength = 400;
            txtDescripcion.Margin = new Padding(0);
            txtDescripcion.PlaceholderText = "Falla detectada o trabajo a realizar…";

            tlpDetalle.Controls.Add(Etiqueta("Tipo"), 0, 0);
            tlpDetalle.Controls.Add(Etiqueta("Servicio"), 1, 0);
            tlpDetalle.Controls.Add(lblResponsable, 2, 0);
            tlpDetalle.Controls.Add(cboTipo, 0, 1);
            tlpDetalle.Controls.Add(flpServicio, 1, 1);
            tlpDetalle.Controls.Add(pnlResponsable, 2, 1);
            var lblDescripcion = Etiqueta("Descripción");
            tlpDetalle.Controls.Add(lblDescripcion, 0, 2);
            tlpDetalle.SetColumnSpan(lblDescripcion, 3);
            tlpDetalle.Controls.Add(txtDescripcion, 0, 3);
            tlpDetalle.SetColumnSpan(txtDescripcion, 3);
            lblResponsable.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblResponsable.AutoSize = true;
            tlpDetalle.Paint += (_, e) => LineaSuperior(e, tlpDetalle);

            // ── Pie ──
            var pnlFooter = new Panel { Dock = DockStyle.Bottom, Height = 64 };
            btnAbrir.Text = "Abrir mantenimiento";
            btnAbrir.Icon = FontAwesome.Sharp.IconChar.Wrench;
            btnAbrir.IconSize = 14;
            btnAbrir.Size = new Size(210, 40);
            btnAbrir.Click += (_, _) => Abrir();
            btnCancelar.Text = "Cancelar";
            btnCancelar.Size = new Size(120, 40);
            btnCancelar.Click += (_, _) => { DialogResult = DialogResult.Cancel; Close(); };
            pnlFooter.Controls.AddRange(new Control[] { btnAbrir, btnCancelar });
            pnlFooter.Resize += (_, _) =>
            {
                btnAbrir.Location = new Point(pnlFooter.Width - 24 - btnAbrir.Width, 12);
                btnCancelar.Location = new Point(btnAbrir.Left - 10 - btnCancelar.Width, 12);
            };
            pnlFooter.Paint += (_, e) => LineaSuperior(e, pnlFooter);

            Controls.Add(pnlGrid);
            Controls.Add(pnlBuscar);
            Controls.Add(pnlHeader);
            Controls.Add(tlpDetalle);
            Controls.Add(pnlFooter);
            CancelButton = btnCancelar;
        }

        private static void Columna(DataGridViewTextBoxColumn c, string propiedad, string encabezado, float peso)
        {
            c.DataPropertyName = propiedad;
            c.HeaderText = encabezado;
            c.FillWeight = peso;
            c.ReadOnly = true;
        }

        private static Label Etiqueta(string texto) => new()
        {
            Text = texto,
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            AutoSize = true
        };

        private void AplicarEstilos()
        {
            BackColor = ThemeManager.CardBackground;
            lblSeleccion.ForeColor = ThemeManager.AccentBlue;
            lblSinProveedores.ForeColor = ThemeManager.TextSecondary;
            btnAbrir.Variant = MaterialButtonVariant.Primary;
            btnCancelar.Variant = MaterialButtonVariant.Secondary;

            var dgv = dgvUnidades;
            dgv.BackgroundColor = ThemeManager.CardBackground;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = ThemeManager.CardBackground;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = ThemeManager.TextSecondary;
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = ThemeManager.CardBackground;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.ColumnHeadersHeight = 38;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = ThemeManager.CardBackground;
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.RowTemplate.Height = 40;
            dgv.MultiSelect = false;
            colCodigo.DefaultCellStyle.Font = FuenteCodigo;
        }

        private static void LineaSuperior(PaintEventArgs e, Control c)
        {
            using var pen = new Pen(ThemeManager.BorderColor);
            e.Graphics.DrawLine(pen, 0, 0, c.Width, 0);
        }

        // ══════════════════════════════════════════════════════════
        // DATOS
        // ══════════════════════════════════════════════════════════
        private void Cargar(string? filtro)
        {
            try
            {
                _unidades = MantenimientoService.UnidadesElegibles();
                var proveedores = ProveedorService.ObtenerTodos();
                cboProveedor.DataSource = proveedores;
                cboProveedor.DisplayMember = "Nombre";
                cboProveedor.ValueMember = "ProveedorId";
                cboProveedor.SelectedIndex = -1;
            }
            catch (Exception ex) { Error(ex.Message); Close(); return; }

            _unidades.Columns.Add("Sel", typeof(bool));
            _unidades.Columns.Add("ReporteTexto", typeof(string));
            foreach (DataRow r in _unidades.Rows)
            {
                r["Sel"] = _preseleccion.Contains(r.Field<int>("UnidadId"));
                r["ReporteTexto"] = TextoReporte(r);
            }

            _vista = _unidades.DefaultView;
            dgvUnidades.DataSource = _vista;
            if (!string.IsNullOrEmpty(filtro)) txtBuscar.Text = filtro;
            Filtrar();

            // Valores sugeridos a partir de lo preseleccionado
            var marcadas = Marcadas().ToList();
            if (marcadas.Any(r => r.Field<string>("Estado") == "Dañada"))
                cboTipo.SelectedItem = "Correctivo";
            if (marcadas.Count == 1 && marcadas[0].Field<string>("NotaReporte") is { Length: > 0 } nota)
                txtDescripcion.Text = "Daño reportado: " + nota;

            rbInterno.Checked = true;
            ActualizarResponsable();
            ActualizarSeleccion();
            DesplazarAPrimeraMarcada();
        }

        // "Juan Pérez · 28/09/2026 · cable pelado" (o la observación si se marcó dañada a mano)
        private static string TextoReporte(DataRow r)
        {
            if (r.Field<string>("Estado") != "Dañada") return "";
            var partes = new List<string>();
            if (r["ReportadoPor"] is string quien) partes.Add(quien);
            if (r["FechaReporte"] is DateTime f) partes.Add(f.ToString("dd/MM/yyyy"));
            if (r["NotaReporte"] is string nota && nota != "") partes.Add(nota);
            return partes.Count > 0 ? string.Join("  ·  ", partes) : "Marcada como dañada";
        }

        private IEnumerable<DataRow> Marcadas() =>
            _unidades.AsEnumerable().Where(r => r.Field<bool>("Sel"));

        private void Filtrar()
        {
            if (_vista == null) return;
            var condiciones = new List<string>();
            string t = txtBuscar.Text.Trim();
            if (t != "")
            {
                // Escapar comillas y comodines para RowFilter
                string esc = t.Replace("'", "''").Replace("[", "[[]").Replace("*", "[*]").Replace("%", "[%]");
                condiciones.Add("(" + string.Join(" OR ",
                    new[] { "CodigoUnidad", "Herramienta", "Categoria", "Ubicacion", "ReportadoPor" }
                    .Select(c => $"{c} LIKE '%{esc}%'")) + ")");
            }
            if (chkSoloDañadas.Checked) condiciones.Add("Estado = 'Dañada'");
            _vista.RowFilter = string.Join(" AND ", condiciones);
            dgvUnidades.ClearSelection();
        }

        private void ActualizarSeleccion()
        {
            int n = Marcadas().Count();
            lblSeleccion.Text = n == 0 ? "Ninguna unidad marcada"
                              : n == 1 ? "1 unidad marcada" : $"{n} unidades marcadas";
            dgvUnidades.InvalidateColumn(colSel.Index);
        }

        private void DesplazarAPrimeraMarcada()
        {
            foreach (DataGridViewRow fila in dgvUnidades.Rows)
                if (fila.DataBoundItem is DataRowView r && (bool)r["Sel"])
                {
                    dgvUnidades.FirstDisplayedScrollingRowIndex = fila.Index;
                    return;
                }
        }

        private void ActualizarResponsable()
        {
            bool externo = rbExterno.Checked;
            bool hayProveedores = cboProveedor.Items.Count > 0;
            lblResponsable.Text = externo ? "Proveedor (taller, servicio autorizado, laboratorio)" : "Empleado que lo realiza";
            pickEmpleado.Visible = !externo;
            cboProveedor.Visible = externo && hayProveedores;
            lblSinProveedores.Visible = externo && !hayProveedores;
        }

        // Estado como chip
        private void dgvUnidades_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != colEstado.Index || e.Graphics == null) return;
            e.PaintBackground(e.CellBounds, (e.State & DataGridViewElementStates.Selected) != 0);
            string estado = e.Value?.ToString() ?? "";
            var (fondo, frente) = StatusChip.ColoresEstadoUnidad(estado);
            StatusChip.DibujarEnCelda(e.Graphics, e.CellBounds, estado, fondo, frente);
            e.Handled = true;
        }

        // ══════════════════════════════════════════════════════════
        // ABRIR
        // ══════════════════════════════════════════════════════════
        private void Abrir()
        {
            var ids = Marcadas().Select(r => r.Field<int>("UnidadId")).ToList();
            if (ids.Count == 0) { Aviso("Marque al menos una unidad de la lista."); return; }
            if (cboTipo.SelectedItem is not string tipo) { Aviso("Seleccione el tipo de mantenimiento."); return; }

            bool externo = rbExterno.Checked;
            int? empleadoId = null, proveedorId = null;
            if (externo)
            {
                if (cboProveedor.SelectedValue is not int p) { Aviso("Seleccione el proveedor que realizará el mantenimiento."); return; }
                proveedorId = p;
            }
            else
            {
                if (pickEmpleado.EmpleadoId <= 0) { Aviso("Seleccione el empleado que realizará el mantenimiento."); return; }
                empleadoId = pickEmpleado.EmpleadoId;
            }

            try
            {
                UnidadesRegistradas = MantenimientoService.RegistrarEntrada(
                    ids, tipo,
                    externo ? MantenimientoService.Externo : MantenimientoService.Interno,
                    empleadoId, proveedorId, txtDescripcion.Text.Trim());
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex) { Error(ex.Message); }
        }

        private static void Aviso(string m) => MessageBox.Show(m, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        private static void Error(string m) => MessageBox.Show(m, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
