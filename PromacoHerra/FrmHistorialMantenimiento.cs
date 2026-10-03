using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FontAwesome.Sharp;
using PromacoHerra.Controls;

namespace PromacoHerra
{
    // Historial de mantenimiento de una herramienta (todas sus unidades):
    //   encabezado → herramienta y cantidad de registros
    //   indicadores → mantenimientos (y en curso), costo total, duración promedio, correctivos
    //   grilla     → una fila por mantenimiento, con chips de tipo y resultado
    //   detalle    → lo que no cabe en la grilla: trabajo solicitado, notas de cierre,
    //                desglose del costo, garantía, factura y quién reportó el daño
    public class FrmHistorialMantenimiento : Form
    {
        private static readonly Font FuenteCodigo = new("Consolas", 9.5F);
        private static readonly Font FuenteTituloDetalle = new("Segoe UI", 8.5F, FontStyle.Bold);
        private static readonly Font FuenteDetalle = new("Segoe UI", 9.5F);

        private readonly DataTable _datos;

        private readonly KpiTile kpiTotal = new("Mantenimientos", IconChar.ScrewdriverWrench, ThemeManager.AccentBlue, ThemeManager.AccentBlueSoft);
        private readonly KpiTile kpiCosto = new("Costo total", IconChar.MoneyBillWave, Paleta.Verde, Paleta.VerdeSuave);
        private readonly KpiTile kpiDuracion = new("Duración promedio", IconChar.Clock, Paleta.Naranja, Paleta.NaranjaSuave);
        private readonly KpiTile kpiCorrectivos = new("Correctivos", IconChar.HeartCrack, Paleta.Rojo, Paleta.RojoSuave);

        private readonly DataGridView dgv = new();
        private readonly DataGridViewTextBoxColumn colUnidad = Columna("Unidad", "Unidad", 11);
        private readonly DataGridViewTextBoxColumn colTipo = Columna("Tipo", "Tipo", 11);
        private readonly DataGridViewTextBoxColumn colServicio = Columna("Servicio", "Servicio", 7);
        private readonly DataGridViewTextBoxColumn colRealizado = Columna("Realizado por", "Realizado por", 28);
        private readonly DataGridViewTextBoxColumn colInicio = Columna("Inicio", "Inicio", 9);
        private readonly DataGridViewTextBoxColumn colCierre = Columna("Cierre", "Cierre", 9);
        private readonly DataGridViewTextBoxColumn colDias = Columna("Días", "Duración", 9);
        private readonly DataGridViewTextBoxColumn colResultado = Columna("Resultado", "Resultado", 10);
        private readonly DataGridViewTextBoxColumn colCosto = Columna("Costo total", "Costo", 9);

        private readonly Label lblSinSeleccion = new();
        private readonly Label lblTrabajo = new();
        private readonly Label lblNotas = new();
        private readonly Label lblCostos = new();
        private readonly Label lblGarantia = new();
        private readonly Label lblReporto = new();
        private readonly TableLayoutPanel tlpDetalle = new();
        private readonly MaterialButton btnCerrar = new() { Text = "Cerrar", Size = new Size(120, 40) };

        /// <param name="nombreHerramienta">Nombre y código, para el encabezado.</param>
        /// <param name="datos">Resultado de sp_Mantenimiento_ObtenerPorHerramienta.</param>
        public FrmHistorialMantenimiento(string nombreHerramienta, DataTable datos)
        {
            _datos = datos;
            ConstruirInterfaz(nombreHerramienta);
            ThemeManager.ApplyTheme(this);
            AplicarEstilos();
            CargarIndicadores();
            dgv.DataSource = _datos;
            Shown += (_, _) => { dgv.ClearSelection(); MostrarDetalle(null); };
        }

        private static DataGridViewTextBoxColumn Columna(string propiedad, string encabezado, float peso) => new()
        {
            DataPropertyName = propiedad,
            HeaderText = encabezado,
            Name = "col" + propiedad.Replace(" ", ""),
            FillWeight = peso,
            ReadOnly = true
        };

        // ══════════════════════════════════════════════════════════
        // INTERFAZ
        // ══════════════════════════════════════════════════════════
        private void ConstruirInterfaz(string nombreHerramienta)
        {
            Text = "Historial de mantenimiento";
            ClientSize = new Size(1180, 820);
            MinimumSize = new Size(980, 660);
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = false;
            MinimizeBox = false;
            Padding = new Padding(24, 16, 24, 16);

            // ── Encabezado ──
            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 66 };
            var lblTitulo = new Label { Text = "Historial de mantenimiento", Font = new Font("Segoe UI", 15F, FontStyle.Bold), AutoSize = true, Location = new Point(0, 0) };
            int unidades = _datos.AsEnumerable().Select(r => r["Unidad"]?.ToString()).Distinct().Count();
            var lblSub = new Label
            {
                Text = $"{nombreHerramienta}  ·  {Cantidad(_datos.Rows.Count, "registro")} en {Cantidad(unidades, "unidad", "unidades")}",
                Font = new Font("Segoe UI", 10F),
                AutoSize = true,
                Location = new Point(1, 36),
                Tag = "secundaria"
            };
            pnlHeader.Controls.AddRange(new Control[] { lblTitulo, lblSub });

            // ── Indicadores ──
            var tlpKpis = new TableLayoutPanel { Dock = DockStyle.Top, Height = 106, ColumnCount = 4, RowCount = 1, Padding = new Padding(0, 0, 0, 14) };
            for (int i = 0; i < 4; i++) tlpKpis.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            var kpis = new[] { kpiTotal, kpiCosto, kpiDuracion, kpiCorrectivos };
            for (int i = 0; i < kpis.Length; i++)
            {
                kpis[i].Dock = DockStyle.Fill;
                kpis[i].Margin = new Padding(0, 0, i < kpis.Length - 1 ? 12 : 0, 0);
                tlpKpis.Controls.Add(kpis[i], i, 0);
            }

            // ── Grilla ──
            dgv.Dock = DockStyle.Fill;
            dgv.AutoGenerateColumns = false;
            dgv.Columns.AddRange(colUnidad, colTipo, colServicio, colRealizado, colInicio, colCierre, colDias, colResultado, colCosto);
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = dgv.AllowUserToDeleteRows = dgv.AllowUserToResizeRows = false;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.CellFormatting += dgv_CellFormatting;
            dgv.CellPainting += dgv_CellPainting;
            dgv.SelectionChanged += (_, _) =>
                MostrarDetalle(dgv.SelectedRows.Count > 0 && dgv.SelectedRows[0].DataBoundItem is DataRowView v ? v.Row : null);

            var pnlGrid = new RoundedPanel { Dock = DockStyle.Fill, Padding = new Padding(12), CornerRadius = 12, BackColor = ThemeManager.CardBackground };
            pnlGrid.Controls.Add(dgv);

            // ── Detalle del mantenimiento seleccionado ──
            var pnlDetalleMarco = new Panel { Dock = DockStyle.Bottom, Height = 196, Padding = new Padding(0, 14, 0, 0) };
            var pnlDetalle = new RoundedPanel { Dock = DockStyle.Fill, Padding = new Padding(20, 14, 20, 14), CornerRadius = 12, BackColor = ThemeManager.CardBackground };

            lblSinSeleccion.Text = "Selecciona un mantenimiento para ver el trabajo realizado, el desglose del costo y la factura.";
            lblSinSeleccion.Dock = DockStyle.Fill;
            lblSinSeleccion.TextAlign = ContentAlignment.MiddleCenter;
            lblSinSeleccion.Font = FuenteDetalle;
            lblSinSeleccion.Tag = "secundaria";

            tlpDetalle.Dock = DockStyle.Fill;
            tlpDetalle.ColumnCount = 2;
            tlpDetalle.RowCount = 1;
            tlpDetalle.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58F));
            tlpDetalle.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42F));
            tlpDetalle.Controls.Add(ColumnaDetalle(("TRABAJO SOLICITADO", lblTrabajo), ("NOTAS DE CIERRE", lblNotas)), 0, 0);
            tlpDetalle.Controls.Add(ColumnaDetalle(("COSTO", lblCostos), ("GARANTÍA Y FACTURA", lblGarantia), ("DAÑO REPORTADO POR", lblReporto)), 1, 0);

            pnlDetalle.Controls.Add(tlpDetalle);
            pnlDetalle.Controls.Add(lblSinSeleccion);
            pnlDetalleMarco.Controls.Add(pnlDetalle);

            // ── Pie ──
            var pnlPie = new Panel { Dock = DockStyle.Bottom, Height = 58 };
            btnCerrar.Click += (_, _) => Close();
            pnlPie.Controls.Add(btnCerrar);
            pnlPie.Resize += (_, _) => btnCerrar.Location = new Point(pnlPie.Width - btnCerrar.Width, pnlPie.Height - btnCerrar.Height);
            CancelButton = btnCerrar;

            // Acoplado: el último agregado se acopla primero (encabezado arriba de todo, pie abajo de todo)
            Controls.Add(pnlGrid);
            Controls.Add(pnlDetalleMarco);
            Controls.Add(pnlPie);
            Controls.Add(tlpKpis);
            Controls.Add(pnlHeader);
        }

        // Una columna del panel de detalle: pares título / valor apilados
        private static FlowLayoutPanel ColumnaDetalle(params (string Titulo, Label Valor)[] campos)
        {
            var flp = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false, Margin = new Padding(0, 0, 16, 0) };
            foreach (var (titulo, valor) in campos)
            {
                flp.Controls.Add(new Label { Text = titulo, Font = FuenteTituloDetalle, AutoSize = true, Margin = new Padding(0, 0, 0, 2), Tag = "secundaria" });
                valor.Font = FuenteDetalle;
                valor.AutoSize = true;
                valor.Margin = new Padding(0, 0, 0, 10);
                flp.Controls.Add(valor);
            }
            flp.Resize += (_, _) =>
            {
                foreach (Control c in flp.Controls) c.MaximumSize = new Size(Math.Max(100, flp.ClientSize.Width - 4), 0);
            };
            return flp;
        }

        private void AplicarEstilos()
        {
            BackColor = ThemeManager.AppBackground;
            // ThemeManager pinta "Cerrar" como acción destructiva; aquí solo cierra la ventana
            btnCerrar.Variant = MaterialButtonVariant.Secondary;
            foreach (var lbl in Todos(this).OfType<Label>())
                if (Equals(lbl.Tag, "secundaria")) lbl.ForeColor = ThemeManager.TextSecondary;

            dgv.BackgroundColor = ThemeManager.CardBackground;
            dgv.GridColor = ThemeManager.BorderColor;
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = ThemeManager.CardBackground;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = ThemeManager.TextSecondary;
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = ThemeManager.CardBackground;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.ColumnHeadersHeight = 40;
            dgv.DefaultCellStyle.BackColor = ThemeManager.CardBackground;
            dgv.DefaultCellStyle.ForeColor = ThemeManager.TextPrimary;
            dgv.DefaultCellStyle.SelectionBackColor = ThemeManager.AccentBlueSoft;
            dgv.DefaultCellStyle.SelectionForeColor = ThemeManager.TextPrimary;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = ThemeManager.CardBackground;
            dgv.RowTemplate.Height = 44;
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.MultiSelect = false;
            colUnidad.DefaultCellStyle.Font = FuenteCodigo;
            colCosto.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colCosto.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        private static System.Collections.Generic.IEnumerable<Control> Todos(Control raiz)
        {
            foreach (Control c in raiz.Controls)
            {
                yield return c;
                foreach (var hijo in Todos(c)) yield return hijo;
            }
        }

        // ══════════════════════════════════════════════════════════
        // DATOS
        // ══════════════════════════════════════════════════════════
        private void CargarIndicadores()
        {
            var filas = _datos.AsEnumerable().ToList();
            int enCurso = filas.Count(r => r["Cierre"] == DBNull.Value);
            var cerrados = filas.Where(r => r["Cierre"] != DBNull.Value).ToList();

            kpiTotal.Valor = filas.Count;
            kpiTotal.Delta = enCurso == 0 ? "ninguno en curso" : $"{enCurso} en curso";

            decimal materiales = cerrados.Sum(r => Monto(r["Materiales"]));
            decimal manoObra = cerrados.Sum(r => Monto(r["Mano de obra"]));
            kpiCosto.Texto = Dinero.Texto(cerrados.Sum(r => Monto(r["Costo total"])));
            decimal suma = materiales + manoObra;
            kpiCosto.Delta = suma == 0 ? "sin costo registrado"
                           : $"{materiales / suma:P0} materiales · {manoObra / suma:P0} mano de obra";

            double promedio = cerrados.Count == 0 ? 0 : cerrados.Average(r => Convert.ToDouble(r["Días"]));
            kpiDuracion.Texto = cerrados.Count == 0 ? "—" : promedio < 1 ? "< 1 día" : Dias(promedio);
            kpiDuracion.Delta = cerrados.Count == 0 ? "sin mantenimientos cerrados" : "de los ya cerrados";

            kpiCorrectivos.Valor = filas.Count(r => Equals(r["Tipo"], "Correctivo"));
            int garantia = filas.Count(r => Equals(r["Garantía"], "Sí"));
            int bajas = filas.Count(r => Equals(r["Resultado"], "Baja"));
            kpiCorrectivos.Delta = $"{garantia} en garantía · {Cantidad(bajas, "baja")}";
        }

        private void MostrarDetalle(DataRow? r)
        {
            lblSinSeleccion.Visible = r == null;
            tlpDetalle.Visible = r != null;
            if (r == null) return;

            lblTrabajo.Text = Texto(r["Descripción"], "Sin descripción");
            lblNotas.Text = r["Cierre"] == DBNull.Value ? "El mantenimiento sigue en curso." : Texto(r["Notas de cierre"], "Sin notas");

            if (r["Cierre"] == DBNull.Value)
                lblCostos.Text = "Se registra al cerrar el mantenimiento.";
            else
            {
                string desglose = $"Materiales {Dinero.Texto(Monto(r["Materiales"]))}";
                if (Equals(r["Servicio"], "Externo")) desglose += $"  ·  Mano de obra {Dinero.Texto(Monto(r["Mano de obra"]))}";
                lblCostos.Text = $"{desglose}\nTotal: {Dinero.Texto(Monto(r["Costo total"]))}";
            }

            string garantia = Equals(r["Garantía"], "Sí") ? "En garantía (sin costo para la empresa)" : "Sin garantía";
            lblGarantia.Text = r["Factura"] is string f && f != "" ? $"{garantia}  ·  Factura {f}" : garantia;
            lblReporto.Text = Texto(r["Daño reportado por"], Equals(r["Tipo"], "Correctivo") ? "No vino de una devolución" : "No aplica (mantenimiento programado)");
        }

        // ══════════════════════════════════════════════════════════
        // GRILLA: textos y chips
        // ══════════════════════════════════════════════════════════
        private void dgv_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || dgv.Rows[e.RowIndex].DataBoundItem is not DataRowView v) return;

            if (e.ColumnIndex == colInicio.Index || e.ColumnIndex == colCierre.Index)
                e.Value = e.Value is DateTime d ? d.ToString("dd/MM/yyyy") : "—";
            else if (e.ColumnIndex == colDias.Index)
                e.Value = Dias(Convert.ToDouble(v["Días"]));
            else if (e.ColumnIndex == colCosto.Index)
                e.Value = v["Cierre"] == DBNull.Value ? "Pendiente" : Dinero.Texto(Monto(v["Costo total"]));
            else if (e.ColumnIndex == colRealizado.Index)
                e.Value = Texto(v["Realizado por"], "—");
            else return;

            if (e.ColumnIndex == colCosto.Index && v["Cierre"] == DBNull.Value)
                e.CellStyle!.ForeColor = ThemeManager.TextSecondary;
            e.FormattingApplied = true;
        }

        private void dgv_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Graphics == null) return;
            if (e.ColumnIndex != colTipo.Index && e.ColumnIndex != colResultado.Index) return;

            string texto = e.Value?.ToString() ?? "";
            var (fondo, frente) = e.ColumnIndex == colTipo.Index
                ? StatusChip.ColoresTipoMantenimiento(texto)
                : texto switch
                {
                    "En curso" => (Paleta.NaranjaSuave, Paleta.Naranja),
                    "Reparada" => (Paleta.VerdeSuave, Paleta.Verde),
                    "Baja" => (Paleta.RojoSuave, Paleta.Rojo),
                    _ => (Paleta.GrisSuave, Paleta.Gris)
                };

            e.PaintBackground(e.CellBounds, (e.State & DataGridViewElementStates.Selected) != 0);
            StatusChip.DibujarEnCelda(e.Graphics, e.CellBounds, texto, fondo, frente);
            e.Handled = true;
        }

        // ── Auxiliares ──
        private static decimal Monto(object valor) => valor == DBNull.Value ? 0 : Convert.ToDecimal(valor);

        private static string Texto(object valor, string siVacio) =>
            valor is string s && !string.IsNullOrWhiteSpace(s) ? s : siVacio;

        // Los días vienen de DATEDIFF(DAY): 0 = empezó y terminó el mismo día
        private static string Dias(double dias) =>
            dias < 0.5 ? "mismo día" :
            Math.Abs(dias - Math.Round(dias)) < 0.05
                ? (Math.Round(dias) == 1 ? "1 día" : $"{Math.Round(dias):0} días")
                : $"{dias:0.0} días";

        private static string Cantidad(int n, string singular, string? plural = null) =>
            n == 1 ? $"1 {singular}" : $"{n} {plural ?? singular + "s"}";
    }
}
