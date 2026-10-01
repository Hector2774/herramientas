using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PromacoHerra.Controls;
using PromacoHerra.Models;
using PromacoHerra.Services;

namespace PromacoHerra
{
    // Unidades físicas de una herramienta:
    //   encabezado → nombre, chips de stock y switch "Préstamos activos / suspendidos"
    //   grilla     → una fila por unidad con checkbox, estado (chip) y condición (Bueno / Dañado / Perdido)
    //   barra      → acción masiva sobre las unidades marcadas (aparece al marcar alguna)
    //   pie        → agregar unidades nuevas
    // La condición por fila cambia el estado de esa unidad: Bueno → Disponible, Dañado → Dañada,
    // Perdido → Perdida. No aplica a unidades prestadas, en mantenimiento o dadas de baja.
    public class FrmUnidades : Form
    {
        private sealed record Accion(string Texto, string Estado)
        {
            public override string ToString() => Texto;
        }

        private const string EstadoMantenimiento = "Mantenimiento";
        private static readonly Font FuenteCodigo = new("Consolas", 9.5F);

        private readonly int _herramientaId;
        private DataTable _unidades = new();

        // Encabezado
        private readonly Label lblTitulo = new();
        private readonly Label lblSubtitulo = new();
        private readonly FlowLayoutPanel flpChips = new();
        private readonly ChipLabel chipDisponibles = new();
        private readonly ChipLabel chipPrestadas = new();
        private readonly ChipLabel chipMantenimiento = new();
        private readonly ChipLabel chipDañadas = new();
        private readonly ToggleSwitch swPrestamos = new();

        // Grilla
        private readonly DataGridView dgvUnidades = new();
        private readonly DataGridViewCheckBoxColumn colSel = new();
        private readonly DataGridViewTextBoxColumn colCodigo = new();
        private readonly DataGridViewTextBoxColumn colEstado = new();
        private readonly DataGridViewTextBoxColumn colPrestadaA = new();
        private readonly DataGridViewTextBoxColumn colDevolucion = new();
        private readonly DataGridViewTextBoxColumn colCondicion = new();

        // Barra de acciones masivas
        private readonly Panel pnlAcciones = new();
        private readonly Label lblSeleccion = new();
        private readonly ComboBox cboAccion = new();
        private readonly ComboBox cboTipoMant = new();
        private readonly MaterialTextBox txtObservacion = new();
        private readonly MaterialButton btnAplicar = new();
        private readonly LinkLabel lnkLimpiar = new();

        // Pie
        private readonly NumericUpDown nudAgregar = new();
        private readonly MaterialButton btnAgregar = new();
        private readonly MaterialButton btnCerrar = new();

        public FrmUnidades(int herramientaId)
        {
            _herramientaId = herramientaId;
            ConstruirInterfaz();
            ThemeManager.ApplyTheme(this);
            AplicarEstilos();
            Load += (_, _) => Cargar();
        }

        // ══════════════════════════════════════════════════════════
        // INTERFAZ
        // ══════════════════════════════════════════════════════════
        private void ConstruirInterfaz()
        {
            Text = "Unidades de stock";
            Size = new Size(1080, 700);
            MinimumSize = new Size(960, 560);
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = false;
            MinimizeBox = false;

            // ── Encabezado ──
            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 118, Padding = new Padding(24, 14, 24, 10) };
            lblTitulo.Text = "Unidades de stock";
            lblTitulo.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblTitulo.Location = new Point(24, 12);
            lblTitulo.AutoSize = true;
            lblSubtitulo.Font = new Font("Segoe UI", 10F);
            lblSubtitulo.Location = new Point(24, 44);
            lblSubtitulo.Size = new Size(640, 22);
            lblSubtitulo.AutoEllipsis = true;
            lblSubtitulo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            flpChips.Location = new Point(22, 76);
            flpChips.Size = new Size(700, 30);
            flpChips.WrapContents = false;
            foreach (var chip in new[] { chipDisponibles, chipPrestadas, chipMantenimiento, chipDañadas })
            {
                chip.Margin = new Padding(0, 0, 8, 0);
                flpChips.Controls.Add(chip);
            }

            swPrestamos.TextoActivo = "Préstamos activos";
            swPrestamos.TextoInactivo = "Préstamos suspendidos";
            swPrestamos.Size = new Size(230, 30);
            swPrestamos.Location = new Point(760, 18);
            swPrestamos.SolicitudCambio += (_, _) => AlternarPrestamo();

            pnlHeader.Controls.AddRange(new Control[] { lblTitulo, lblSubtitulo, flpChips, swPrestamos });
            pnlHeader.Paint += (_, e) => LineaInferior(e, pnlHeader);

            // ── Grilla ──
            colSel.HeaderText = "";
            colSel.DataPropertyName = "Sel";
            colSel.Width = 44;
            colSel.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colCodigo.HeaderText = "Código unidad";
            colCodigo.DataPropertyName = "CodigoUnidad";
            colCodigo.FillWeight = 16;
            colEstado.HeaderText = "Estado";
            colEstado.DataPropertyName = "Estado";
            colEstado.FillWeight = 16;
            colPrestadaA.HeaderText = "Prestada a";
            colPrestadaA.DataPropertyName = "PrestadaATexto";
            colPrestadaA.FillWeight = 22;
            colDevolucion.HeaderText = "Devolución esperada";
            colDevolucion.DataPropertyName = "DevolucionTexto";
            colDevolucion.FillWeight = 16;
            colCondicion.HeaderText = "Condición";
            colCondicion.Width = CondicionSelector.AnchoPreferido() + 24;
            colCondicion.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            foreach (var c in new DataGridViewColumn[] { colCodigo, colEstado, colPrestadaA, colDevolucion, colCondicion })
                c.ReadOnly = true;

            dgvUnidades.AutoGenerateColumns = false;
            dgvUnidades.Columns.AddRange(colSel, colCodigo, colEstado, colPrestadaA, colDevolucion, colCondicion);
            dgvUnidades.Dock = DockStyle.Fill;
            dgvUnidades.AllowUserToAddRows = false;
            dgvUnidades.AllowUserToDeleteRows = false;
            dgvUnidades.CellPainting += dgvUnidades_CellPainting;
            dgvUnidades.CellMouseClick += dgvUnidades_CellMouseClick;
            dgvUnidades.CellMouseMove += (_, e) =>
                dgvUnidades.Cursor = e.ColumnIndex == colCondicion.Index && e.RowIndex >= 0 ? Cursors.Hand : Cursors.Default;
            dgvUnidades.CurrentCellDirtyStateChanged += (_, _) =>
            {
                if (dgvUnidades.IsCurrentCellDirty) dgvUnidades.CommitEdit(DataGridViewDataErrorContexts.Commit);
            };
            dgvUnidades.CellValueChanged += (_, e) => { if (e.ColumnIndex == colSel.Index) ActualizarSeleccion(); };

            var pnlGrid = new Panel { Dock = DockStyle.Fill, Padding = new Padding(24, 12, 24, 12) };
            pnlGrid.Controls.Add(dgvUnidades);

            // ── Barra de acciones (visible solo con unidades marcadas) ──
            pnlAcciones.Dock = DockStyle.Bottom;
            pnlAcciones.Height = 62;
            pnlAcciones.Visible = false;

            lblSeleccion.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblSeleccion.Location = new Point(24, 20);
            lblSeleccion.Size = new Size(200, 24);

            cboAccion.DropDownStyle = ComboBoxStyle.DropDownList;
            cboAccion.Location = new Point(230, 17);
            cboAccion.Width = 220;
            cboAccion.Items.AddRange(new object[]
            {
                new Accion("Marcar disponible", "Disponible"),
                new Accion("Enviar a mantenimiento", EstadoMantenimiento),
                new Accion("Marcar dañada", "Dañada"),
                new Accion("Dar de baja (sale del stock)", "Baja"),
            });
            cboAccion.SelectedIndexChanged += (_, _) =>
                cboTipoMant.Visible = (cboAccion.SelectedItem as Accion)?.Estado == EstadoMantenimiento;

            cboTipoMant.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTipoMant.Location = new Point(460, 17);
            cboTipoMant.Width = 130;
            cboTipoMant.Items.AddRange(new object[] { "Correctivo", "Preventivo", "Calibración" });
            cboTipoMant.SelectedIndex = 0;
            cboTipoMant.Visible = false;
            cboAccion.SelectedIndex = 0;

            txtObservacion.PlaceholderText = "Observación (opcional)";
            txtObservacion.Location = new Point(600, 12);
            txtObservacion.Size = new Size(250, 38);

            btnAplicar.Text = "Aplicar";
            btnAplicar.Icon = FontAwesome.Sharp.IconChar.Check;
            btnAplicar.IconSize = 14;
            btnAplicar.Size = new Size(110, 38);
            btnAplicar.Location = new Point(862, 12);
            btnAplicar.Click += (_, _) => Aplicar();

            lnkLimpiar.Text = "Quitar selección";
            lnkLimpiar.AutoSize = true;
            lnkLimpiar.Location = new Point(982, 22);
            lnkLimpiar.LinkClicked += (_, _) => MarcarTodas(false);

            pnlAcciones.Controls.AddRange(new Control[] { lblSeleccion, cboAccion, cboTipoMant, txtObservacion, btnAplicar, lnkLimpiar });

            // ── Pie ──
            var pnlFooter = new Panel { Dock = DockStyle.Bottom, Height = 64 };
            var lblAgregar = new Label { Text = "Agregar unidades:", Location = new Point(24, 22), AutoSize = true };
            nudAgregar.Location = new Point(160, 18);
            nudAgregar.Width = 70;
            nudAgregar.Minimum = 1;
            nudAgregar.Maximum = 999;
            btnAgregar.Text = "Agregar";
            btnAgregar.Icon = FontAwesome.Sharp.IconChar.Plus;
            btnAgregar.IconSize = 14;
            btnAgregar.Size = new Size(120, 38);
            btnAgregar.Location = new Point(240, 13);
            btnAgregar.Click += (_, _) => AgregarUnidades();
            btnCerrar.Text = "Cerrar";
            btnCerrar.Size = new Size(120, 38);
            btnCerrar.Location = new Point(900, 13);
            btnCerrar.Click += (_, _) => { DialogResult = DialogResult.OK; Close(); };
            pnlFooter.Controls.AddRange(new Control[] { lblAgregar, nudAgregar, btnAgregar, btnCerrar });
            pnlFooter.Paint += (_, e) => LineaSuperior(e, pnlFooter);

            // Lo alineado a la derecha se ubica al cambiar de tamaño (las anclas fallan cuando
            // el panel aún no tiene su ancho final al construirse)
            pnlHeader.Resize += (_, _) => swPrestamos.Left = pnlHeader.Width - 24 - swPrestamos.Width;
            pnlFooter.Resize += (_, _) => btnCerrar.Left = pnlFooter.Width - 24 - btnCerrar.Width;
            pnlAcciones.Resize += (_, _) =>
            {
                lnkLimpiar.Left = pnlAcciones.Width - 24 - lnkLimpiar.Width;
                btnAplicar.Left = lnkLimpiar.Left - 12 - btnAplicar.Width;
                txtObservacion.Width = Math.Max(120, btnAplicar.Left - 12 - txtObservacion.Left);
            };

            Controls.Add(pnlGrid);
            Controls.Add(pnlAcciones);
            Controls.Add(pnlFooter);
            Controls.Add(pnlHeader);
            CancelButton = btnCerrar;
        }

        private void AplicarEstilos()
        {
            BackColor = ThemeManager.CardBackground;
            lblSubtitulo.ForeColor = ThemeManager.TextSecondary;
            pnlAcciones.BackColor = ThemeManager.AccentBlueSoft;
            lblSeleccion.ForeColor = ThemeManager.AccentBlue;
            lnkLimpiar.LinkColor = lnkLimpiar.ActiveLinkColor = ThemeManager.AccentBlue;

            chipDisponibles.Fondo = Paleta.VerdeSuave; chipDisponibles.Frente = Paleta.Verde;
            chipPrestadas.Fondo = Paleta.AmarilloSuave; chipPrestadas.Frente = Paleta.Amarillo;
            chipMantenimiento.Fondo = Paleta.NaranjaSuave; chipMantenimiento.Frente = Paleta.Naranja;
            chipDañadas.Fondo = Paleta.RojoSuave; chipDañadas.Frente = Paleta.Rojo;

            btnAplicar.Variant = MaterialButtonVariant.Primary;
            btnAgregar.Variant = MaterialButtonVariant.Default;
            btnCerrar.Variant = MaterialButtonVariant.Secondary;

            // Grilla clara con filas de alto fijo
            var dgv = dgvUnidades;
            dgv.BackgroundColor = ThemeManager.CardBackground;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = ThemeManager.CardBackground;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = ThemeManager.TextSecondary;
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = ThemeManager.CardBackground;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.ColumnHeadersHeight = 40;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = ThemeManager.CardBackground;
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgv.RowTemplate.Height = 46;
            dgv.MultiSelect = false;
            colCodigo.DefaultCellStyle.Font = FuenteCodigo;
        }

        private static void LineaSuperior(PaintEventArgs e, Control c)
        {
            using var pen = new Pen(ThemeManager.BorderColor);
            e.Graphics.DrawLine(pen, 0, 0, c.Width, 0);
        }

        private static void LineaInferior(PaintEventArgs e, Control c)
        {
            using var pen = new Pen(ThemeManager.BorderColor);
            e.Graphics.DrawLine(pen, 0, c.Height - 1, c.Width, c.Height - 1);
        }

        // ══════════════════════════════════════════════════════════
        // DATOS
        // ══════════════════════════════════════════════════════════
        private void Cargar()
        {
            DataTable dtH;
            try { dtH = HerramientaService.ObtenerPorId(_herramientaId); }
            catch (Exception ex) { Error(ex.Message); return; }

            if (dtH.Rows.Count == 0 || !Convert.ToBoolean(dtH.Rows[0]["Activa"]))
            {
                DialogResult = DialogResult.OK;
                Close();
                return;
            }

            var h = dtH.Rows[0];
            lblSubtitulo.Text = $"{h["Nombre"]}  ·  {h["Codigo"]}";
            chipDisponibles.Text = $"{h["StockDisponible"]} Disponibles";
            chipPrestadas.Text = $"{h["StockPrestado"]} Prestadas";
            chipMantenimiento.Text = $"{h["StockMantenimiento"]} En mant.";
            chipDañadas.Text = $"{h["StockDañado"]} Dañadas";
            swPrestamos.Activo = Convert.ToBoolean(h["PrestamoHabilitado"]);

            _unidades = HerramientaService.ObtenerUnidades(_herramientaId);
            _unidades.Columns.Add("Sel", typeof(bool)).DefaultValue = false;
            _unidades.Columns.Add("PrestadaATexto", typeof(string));
            _unidades.Columns.Add("DevolucionTexto", typeof(string));
            foreach (DataRow r in _unidades.Rows)
            {
                r["Sel"] = false;
                r["PrestadaATexto"] = r["PrestadaA"] is string emp && emp != "" ? emp : "—";
                r["DevolucionTexto"] = r["FechaDevolucionEsperada"] is DateTime f ? f.ToString("dd/MM/yyyy") : "—";
            }

            dgvUnidades.DataSource = _unidades;
            dgvUnidades.ClearSelection();
            ActualizarSeleccion();
        }

        private List<int> UnidadesMarcadas() =>
            _unidades.AsEnumerable().Where(r => r.Field<bool>("Sel")).Select(r => r.Field<int>("UnidadId")).ToList();

        private void ActualizarSeleccion()
        {
            int n = UnidadesMarcadas().Count;
            pnlAcciones.Visible = n > 0;
            lblSeleccion.Text = n == 1 ? "1 unidad seleccionada" : $"{n} unidades seleccionadas";
        }

        private void MarcarTodas(bool valor)
        {
            foreach (DataRow r in _unidades.Rows) r["Sel"] = valor;
            ActualizarSeleccion();
        }

        // Condición visible según el estado actual; null = no aplica (prestada, en mantenimiento, baja)
        private static CondicionDevolucion? CondicionDe(string? estado) => estado switch
        {
            "Disponible" => CondicionDevolucion.Bueno,
            "Dañada" => CondicionDevolucion.Dañado,
            "Perdida" => CondicionDevolucion.Perdido,
            _ => null
        };

        private static Rectangle AreaCondicion(Rectangle celda) =>
            new(celda.X + 10, celda.Y + 7, celda.Width - 14, celda.Height - 14);

        // ── Dibujo de chips de estado y selector de condición ──
        private void dgvUnidades_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Graphics == null || dgvUnidades.Rows[e.RowIndex].DataBoundItem is not DataRowView r) return;
            bool sel = (e.State & DataGridViewElementStates.Selected) != 0;
            string estado = r["Estado"]?.ToString() ?? "";

            if (e.ColumnIndex == colEstado.Index)
            {
                e.PaintBackground(e.CellBounds, sel);
                var (fondo, frente) = StatusChip.ColoresEstadoUnidad(estado);
                StatusChip.DibujarEnCelda(e.Graphics, e.CellBounds, estado, fondo, frente);
                e.Handled = true;
            }
            else if (e.ColumnIndex == colCondicion.Index)
            {
                e.PaintBackground(e.CellBounds, sel);
                var condicion = CondicionDe(estado);
                CondicionSelector.Dibujar(e.Graphics, AreaCondicion(e.CellBounds), condicion, habilitado: condicion != null);
                e.Handled = true;
            }
        }

        // Click en Bueno / Dañado / Perdido de una fila → cambia el estado de esa unidad
        private void dgvUnidades_CellMouseClick(object? sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != colCondicion.Index ||
                dgvUnidades.Rows[e.RowIndex].DataBoundItem is not DataRowView r) return;

            string estado = r["Estado"]?.ToString() ?? "";
            var actual = CondicionDe(estado);
            if (actual == null)
            {
                Aviso($"La unidad está {estado.ToLower()}: su condición se registra al devolverla, " +
                      "cerrar el mantenimiento o no aplica si fue dada de baja.");
                return;
            }

            var celda = dgvUnidades.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var punto = new Point(celda.X + e.X, celda.Y + e.Y);
            if (CondicionSelector.SegmentoEn(AreaCondicion(celda), punto) is not { } nueva || nueva == actual) return;

            var (nuevoEstado, texto) = nueva switch
            {
                CondicionDevolucion.Bueno => ("Disponible", "disponible (en buen estado)"),
                CondicionDevolucion.Dañado => ("Dañada", "dañada"),
                _ => ("Perdida", "perdida (se descuenta del stock)")
            };
            if (Confirmar($"¿Marcar la unidad {r["CodigoUnidad"]} como {texto}?") != DialogResult.Yes) return;

            try
            {
                HerramientaService.CambiarEstadoUnidades(_herramientaId, new[] { Convert.ToInt32(r["UnidadId"]) }, nuevoEstado);
                Cargar();
            }
            catch (Exception ex) { Error(ex.Message); }
        }

        // ══════════════════════════════════════════════════════════
        // ACCIONES
        // ══════════════════════════════════════════════════════════
        private void Aplicar()
        {
            if (cboAccion.SelectedItem is not Accion accion) return;

            var ids = UnidadesMarcadas();
            if (ids.Count == 0) { Aviso("Marque una o más unidades de la lista."); return; }

            string alcance = ids.Count == 1 ? "la unidad seleccionada" : $"las {ids.Count} unidades seleccionadas";
            if (Confirmar($"¿{accion.Texto}: {alcance}?") != DialogResult.Yes) return;

            try
            {
                string obs = txtObservacion.Text.Trim();

                if (accion.Estado == EstadoMantenimiento)
                {
                    int n = MantenimientoService.RegistrarEntrada(
                        _herramientaId, ids, cboTipoMant.SelectedItem!.ToString()!, obs);
                    OK($"{n} unidad(es) enviada(s) a mantenimiento.");
                }
                else
                {
                    var (afectadas, omitidas) = HerramientaService.CambiarEstadoUnidades(
                        _herramientaId, ids, accion.Estado, obs);

                    OK(omitidas > 0
                        ? $"{afectadas} unidad(es) actualizada(s). {omitidas} se omitieron por estar prestadas, en mantenimiento o ya en ese estado."
                        : $"{afectadas} unidad(es) actualizada(s).");
                }

                txtObservacion.Clear();
                Cargar();
            }
            catch (Exception ex) { Error(ex.Message); }
        }

        private void AlternarPrestamo()
        {
            bool habilitado = swPrestamos.Activo;
            string msg = habilitado
                ? "¿Suspender el préstamo de TODAS las unidades de esta herramienta?\n\nLas unidades ya prestadas se pueden devolver normalmente."
                : "¿Volver a permitir el préstamo de esta herramienta?";

            if (Confirmar(msg) != DialogResult.Yes) return;

            try
            {
                HerramientaService.HabilitarPrestamo(_herramientaId, !habilitado);
                Cargar();
            }
            catch (Exception ex) { Error(ex.Message); }
        }

        private void AgregarUnidades()
        {
            int cantidad = (int)nudAgregar.Value;
            if (Confirmar($"¿Agregar {cantidad} unidad(es) nueva(s) al stock?") != DialogResult.Yes) return;

            try
            {
                HerramientaService.AgregarUnidades(_herramientaId, cantidad);
                nudAgregar.Value = 1;
                Cargar();
            }
            catch (Exception ex) { Error(ex.Message); }
        }

        // ══════════════════════════════════════════════════════════
        // HELPERS
        // ══════════════════════════════════════════════════════════
        private void Aviso(string m) => MessageBox.Show(m, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        private void OK(string m) => MessageBox.Show(m, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        private void Error(string m) => MessageBox.Show(m, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        private DialogResult Confirmar(string m) => MessageBox.Show(m, "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
    }
}
