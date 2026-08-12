using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using PromacoHerra.Data;
using PromacoHerra.Services;

namespace PromacoHerra
{
    public partial class FrmReportes : Form
    {
        public FrmReportes()
        {
            InitializeComponent();
            ThemeManager.ApplyTheme(this);

            this.Load += FrmReportes_Load;
            btnGenerar.Click += btnGenerar_Click;
            btnExcel.Click += btnExcel_Click;
            btnPDF.Click += btnPDF_Click;
        }

        // ── Carga inicial ──────────────────────────────────────────
        private void FrmReportes_Load(object sender, EventArgs e)
        {
            CargarTiposReporte();
            CargarEmpleados();
            CargarHerramientas();
            CargarResumen();

            // Fechas por defecto: último mes
            dtpDesde.Value = DateTime.Now.AddMonths(-1);
            dtpHasta.Value = DateTime.Now;
        }

        // ── Tipos de reporte — ahora incluye todos los del módulo ──
        private void CargarTiposReporte()
        {
            cboTipoReporte.Items.Clear();
            cboTipoReporte.Items.Add("Historial de préstamos");
            cboTipoReporte.Items.Add("Préstamos vencidos");
            cboTipoReporte.Items.Add("Herramientas más prestadas");
            cboTipoReporte.Items.Add("Empleados con atrasos");
            cboTipoReporte.Items.Add("Herramientas dañadas");
            cboTipoReporte.Items.Add("Costos de mantenimiento");
            cboTipoReporte.SelectedIndex = 0;
        }

        private void CargarEmpleados()
        {
            var dt = Db.Query(
                "SELECT EmpleadoId, Nombre FROM Empleado WHERE Activo = 1 ORDER BY Nombre");

            var dtFinal = dt.Clone();
            var filaTodos = dtFinal.NewRow();
            filaTodos["EmpleadoId"] = 0;
            filaTodos["Nombre"] = "Todos";
            dtFinal.Rows.Add(filaTodos);
            foreach (DataRow row in dt.Rows) dtFinal.ImportRow(row);

            cboEmpleado.DataSource = dtFinal;
            cboEmpleado.DisplayMember = "Nombre";
            cboEmpleado.ValueMember = "EmpleadoId";
        }

        private void CargarHerramientas()
        {
            var dt = Db.Query(
                "SELECT HerramientaId, Nombre FROM Herramienta WHERE Activa = 1 ORDER BY Nombre");

            var dtFinal = dt.Clone();
            var filaTodas = dtFinal.NewRow();
            filaTodas["HerramientaId"] = 0;
            filaTodas["Nombre"] = "Todas";
            dtFinal.Rows.Add(filaTodas);
            foreach (DataRow row in dt.Rows) dtFinal.ImportRow(row);

            cboHerramienta.DataSource = dtFinal;
            cboHerramienta.DisplayMember = "Nombre";
            cboHerramienta.ValueMember = "HerramientaId";
        }

        // ── Cards de resumen ───────────────────────────────────────
        private void CargarResumen()
        {
            var activos = Db.Query("SELECT COUNT(*) AS N FROM Prestamo WHERE Estado = 'Activo'");
            var vencidos = Db.Query("SELECT COUNT(*) AS N FROM Prestamo WHERE Estado = 'Vencido'");
            var disponibles = Db.Query("SELECT COUNT(*) AS N FROM Herramienta WHERE Estado = 'Disponible' AND Activa = 1");
            var danadas = Db.Query("SELECT COUNT(*) AS N FROM Herramienta WHERE Estado = 'Dañada' AND Activa = 1");

            lblActivos.Text = $"Préstamos Activos\n{activos.Rows[0]["N"]}";
            lblAtrasos.Text = $"Vencidos\n{vencidos.Rows[0]["N"]}";
            lblDisponibles.Text = $"Herramientas Disponibles\n{disponibles.Rows[0]["N"]}";
            lblDanadas.Text = $"Herramientas Dañadas\n{danadas.Rows[0]["N"]}";
        }

        // ── Botón Generar ──────────────────────────────────────────
        private void btnGenerar_Click(object sender, EventArgs e)
        {
            int? empleadoId = Convert.ToInt32(cboEmpleado.SelectedValue) == 0 ? null : Convert.ToInt32(cboEmpleado.SelectedValue);
            int? herramientaId = Convert.ToInt32(cboHerramienta.SelectedValue) == 0 ? null : Convert.ToInt32(cboHerramienta.SelectedValue);
            DateTime desde = dtpDesde.Value.Date;
            DateTime hasta = dtpHasta.Value.Date;

            DataTable dt = null;

            try
            {
                switch (cboTipoReporte.Text)
                {
                    case "Historial de préstamos":
                        dt = ReporteService.HistorialPrestamos(
                            empleadoId, herramientaId, desde, hasta);
                        break;

                    case "Préstamos vencidos":
                        dt = ReporteService.PrestamosVencidos();
                        break;

                    case "Herramientas más prestadas":
                        dt = ReporteService.HerramientasMasPrestadas(desde, hasta);
                        break;

                    case "Empleados con atrasos":
                        dt = ReporteService.EmpleadosConAtrasos(desde, hasta);
                        break;

                    case "Herramientas dañadas":
                        dt = ReporteService.HerramientasDañadas();
                        break;

                    case "Costos de mantenimiento":
                        dt = ReporteService.CostosMantenimiento(
                            desde, hasta, herramientaId);
                        break;
                }

                if (dt == null || dt.Rows.Count == 0)
                {
                    dgvReporte.DataSource = null;
                    lblReporte.Text = cboTipoReporte.Text + " — Sin resultados";
                    MessageBox.Show("No se encontraron resultados para los filtros seleccionados.",
                        "Sin datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                dgvReporte.DataSource = dt;
                lblReporte.Text = cboTipoReporte.Text;

                // Actualizar cards cada vez que se genera un reporte
                CargarResumen();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al generar reporte",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Exportar Excel ─────────────────────────────────────────
        private void btnExcel_Click(object sender, EventArgs e) =>
            ExportarExcel(dgvReporte);

        private void ExportarExcel(DataGridView dgv)
        {
            if (dgv.Rows.Count == 0)
            {
                MessageBox.Show("Genera un reporte primero.", "Sin datos",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var sfd = new SaveFileDialog
            {
                Filter = "Excel (*.xlsx)|*.xlsx",
                FileName = cboTipoReporte.Text
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;

            using var wb = new ClosedXML.Excel.XLWorkbook();
            var ws = wb.Worksheets.Add("Reporte");

            // Título
            ws.Cell(1, 1).Value = cboTipoReporte.Text.ToUpper();
            ws.Cell(1, 1).Style.Font.Bold = true;
            ws.Cell(1, 1).Style.Font.FontSize = 16;
            ws.Cell(1, 1).Style.Font.FontColor =
                ClosedXML.Excel.XLColor.FromHtml("#001f3f");
            ws.Range(1, 1, 1, dgv.Columns.Count).Merge();

            ws.Cell(2, 1).Value = "Generado el: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            ws.Cell(3, 1).Value = $"Período: {dtpDesde.Value:dd/MM/yyyy} — {dtpHasta.Value:dd/MM/yyyy}";

            // Encabezados
            int filaInicio = 5;
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                var celda = ws.Cell(filaInicio, i + 1);
                celda.Value = dgv.Columns[i].HeaderText;
                celda.Style.Fill.BackgroundColor =
                    ClosedXML.Excel.XLColor.FromHtml("#0056b3");
                celda.Style.Font.FontColor = ClosedXML.Excel.XLColor.White;
                celda.Style.Font.Bold = true;
                celda.Style.Alignment.Horizontal =
                    ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
            }

            // Datos
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                for (int j = 0; j < dgv.Columns.Count; j++)
                {
                    var celda = ws.Cell(i + filaInicio + 1, j + 1);
                    celda.Value = dgv.Rows[i].Cells[j].Value?.ToString();
                    celda.Style.Border.OutsideBorder =
                        ClosedXML.Excel.XLBorderStyleValues.Thin;
                    celda.Style.Border.OutsideBorderColor =
                        ClosedXML.Excel.XLColor.LightGray;

                    // Filas alternas
                    if (i % 2 != 0)
                        celda.Style.Fill.BackgroundColor =
                            ClosedXML.Excel.XLColor.FromHtml("#f2f2f2");
                }
            }

            ws.Columns().AdjustToContents();
            wb.SaveAs(sfd.FileName);
            MessageBox.Show("Exportado a Excel correctamente.", "Éxito",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ── Exportar PDF ───────────────────────────────────────────
        private void btnPDF_Click(object sender, EventArgs e) =>
            ExportarPDF(dgvReporte);

        private void ExportarPDF(DataGridView dgv)
        {
            if (dgv.Rows.Count == 0)
            {
                MessageBox.Show("Genera un reporte primero.", "Sin datos",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var sfd = new SaveFileDialog
            {
                Filter = "PDF (*.pdf)|*.pdf",
                FileName = cboTipoReporte.Text
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;

            using var writer = new PdfWriter(sfd.FileName);
            var pdf = new PdfDocument(writer);
            var doc = new Document(pdf,
                iText.Kernel.Geom.PageSize.A4.Rotate());
            doc.SetMargins(30, 30, 30, 30);

            // Título
            doc.Add(new Paragraph(cboTipoReporte.Text.ToUpper())
                .SetFontSize(16)
                .SetFontColor(new DeviceRgb(0, 31, 63))
                .SetTextAlignment(TextAlignment.CENTER));

            doc.Add(new Paragraph(
                $"Generado el: {DateTime.Now:dd/MM/yyyy HH:mm}   |   " +
                $"Período: {dtpDesde.Value:dd/MM/yyyy} — {dtpHasta.Value:dd/MM/yyyy}")
                .SetFontSize(9)
                .SetMarginBottom(15));

            // Tabla
            var table = new Table(
                UnitValue.CreatePercentArray(dgv.Columns.Count))
                .UseAllAvailableWidth();

            // Encabezados
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                table.AddHeaderCell(
                    new Cell()
                        .Add(new Paragraph(col.HeaderText))
                        .SetBackgroundColor(new DeviceRgb(0, 86, 179))
                        .SetFontColor(ColorConstants.WHITE)
                        .SetPadding(5)
                        .SetTextAlignment(TextAlignment.CENTER));
            }

            // Filas
            int idx = 0;
            foreach (DataGridViewRow row in dgv.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    var pdfCell = new Cell()
                        .Add(new Paragraph(cell.Value?.ToString() ?? ""))
                        .SetPadding(3)
                        .SetFontSize(8);

                    if (idx % 2 == 0)
                        pdfCell.SetBackgroundColor(new DeviceRgb(242, 242, 242));

                    table.AddCell(pdfCell);
                }
                idx++;
            }

            doc.Add(table);
            doc.Close();

            MessageBox.Show("Reporte PDF generado correctamente.", "Éxito",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Requerido por el Designer (evento del label)
        private void label7_Click(object sender, EventArgs e) { }
    }
}