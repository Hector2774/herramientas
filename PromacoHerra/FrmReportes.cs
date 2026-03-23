using PromacoHerra.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PromacoHerra
{
    public partial class FrmReportes : Form
    {
        public FrmReportes()
        {
            InitializeComponent();
            this.Load += new System.EventHandler(this.FrmReportes_Load);
            this.btnGenerar.Click += new System.EventHandler(this.btnGenerar_Click);
            this.btnGenerarAtrasados.Click += new System.EventHandler(this.btnGenerarAtrasados_Click);
            this.btnReporteEmpleado.Click += new System.EventHandler(this.btnReporteEmpleado_Click);
            this.btnReporteHerramienta.Click += new System.EventHandler(this.btnReporteHerramienta_Click);
            this.btnReporteDanadas.Click += new System.EventHandler(this.btnReporteDanadas_Click);
            CargarEmpleados();
            CargarHerramientas();
        }

        private void CargarTiposReporte()
        {
            cboReporte.Items.Clear();

            cboReporte.Items.Add("Herramientas disponibles");
            cboReporte.Items.Add("Herramientas prestadas");
            cboReporte.Items.Add("Historial de préstamos");
            cboReporte.Items.Add("Préstamos activos");
            cboReporte.Items.Add("Préstamos por empleado");

            cboReporte.SelectedIndex = 0;
        }

        private void FrmReportes_Load(object sender, EventArgs e)
        {
            CargarTiposReporte();
        }

        
        private void btnGenerar_Click(object sender, EventArgs e)
        {
            string opcion = cboReporte.Text;

            switch (opcion)
            {
                case "Herramientas disponibles":
                    ReporteHerramientasDisponibles();
                    break;

                case "Herramientas prestadas":
                    ReporteHerramientasPrestadas();
                    break;

                case "Historial de préstamos":
                    ReporteHistorial();
                    break;

                case "Préstamos activos":
                    ReportePrestamosActivos();
                    break;

                case "Préstamos por empleado":
                    ReportePorEmpleado();
                    break;
                
            }
        }

        private void ReporteHerramientasDisponibles()
        {
            dgvReporte.DataSource = Db.Query(@"
        SELECT Codigo, Nombre, Caracteristicas
        FROM Herramienta
        WHERE Estado = 'Disponible' AND Activa = 1
    ");
        }


        private void ReporteHerramientasPrestadas()
        {
            dgvReporte.DataSource = Db.Query(@"
        SELECT Codigo, Nombre
        FROM Herramienta
        WHERE Estado = 'Prestada'
    ");
        }

        private void ReporteHistorial()
        {
            dgvReporte.DataSource = Db.Query(@"
        SELECT 
            p.PrestamoId,
            e.Nombre AS Empleado,
            p.FechaPrestamo,
            p.FechaCierre
        FROM Prestamo p
        INNER JOIN Empleado e ON e.EmpleadoId = p.EmpleadoId
        ORDER BY p.FechaPrestamo DESC
    ");
        }

        private void ReportePrestamosActivos()
        {
            dgvReporte.DataSource = Db.Query(@"
        SELECT 
            p.PrestamoId,
            e.Nombre AS Empleado,
            p.FechaPrestamo
        FROM Prestamo p
        INNER JOIN Empleado e ON e.EmpleadoId = p.EmpleadoId
        WHERE p.FechaCierre IS NULL
    ");
        }


        private void ReportePorEmpleado()
        {
            dgvReporte.DataSource = Db.Query(@"
        SELECT 
            e.Nombre,
            COUNT(p.PrestamoId) AS TotalPrestamos
        FROM Empleado e
        LEFT JOIN Prestamo p ON p.EmpleadoId = e.EmpleadoId
        GROUP BY e.Nombre
    ");
        }
        private void btnGenerarAtrasados_Click(object sender, EventArgs e)
        {
            ReporteAtrasados();
        }

        private void ReporteAtrasados()
        {
            using var cn = Db.GetConnection();
            cn.Open();

            var cmd = new Microsoft.Data.SqlClient.SqlCommand(@"
        SELECT 
            e.Nombre AS Empleado,
            h.Codigo,
            h.Nombre AS Herramienta,
            p.FechaPrestamo,
            p.FechaDevolucion,
            DATEDIFF(DAY, p.FechaDevolucion, GETDATE()) AS DiasAtraso
        FROM PrestamoDetalle d
        INNER JOIN Prestamo p ON p.PrestamoId = d.PrestamoId
        INNER JOIN Empleado e ON e.EmpleadoId = p.EmpleadoId
        INNER JOIN Herramienta h ON h.HerramientaId = d.HerramientaId
        WHERE 
            d.FechaDevuelta IS NULL
            AND p.FechaDevolucion < GETDATE()
            AND p.FechaPrestamo BETWEEN @Desde AND @Hasta
        ORDER BY DiasAtraso DESC
    ", cn);

            cmd.Parameters.AddWithValue("@Desde", dtpDesde.Value.Date);
            cmd.Parameters.AddWithValue("@Hasta", dtpHasta.Value.Date);

            var da = new Microsoft.Data.SqlClient.SqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);

            dgvReporte.DataSource = dt;
        }


        private void ReporteEmpleado()
        {
            if (cboEmpleado.SelectedValue == null)
            {
                MessageBox.Show("Seleccione un empleado");
                return;
            }

            using var cn = Db.GetConnection();
            cn.Open();

            var cmd = new Microsoft.Data.SqlClient.SqlCommand(@"
        SELECT 
            e.Nombre AS Empleado,
            h.Codigo,
            h.Nombre AS Herramienta,
            p.FechaPrestamo,
            p.FechaDevolucion,
            d.FechaDevuelta,
            CASE 
                WHEN d.FechaDevuelta IS NULL THEN 'Pendiente'
                ELSE 'Devuelto'
            END AS Estado,
            CASE 
                WHEN d.FechaDevuelta IS NULL AND p.FechaDevolucion < GETDATE()
                    THEN DATEDIFF(DAY, p.FechaDevolucion, GETDATE())
                ELSE 0
            END AS DiasAtraso
        FROM PrestamoDetalle d
        INNER JOIN Prestamo p ON p.PrestamoId = d.PrestamoId
        INNER JOIN Empleado e ON e.EmpleadoId = p.EmpleadoId
        INNER JOIN Herramienta h ON h.HerramientaId = d.HerramientaId
        WHERE p.EmpleadoId = @EmpleadoId
        ORDER BY p.FechaPrestamo DESC
    ", cn);

            cmd.Parameters.AddWithValue("@EmpleadoId", cboEmpleado.SelectedValue);

            var da = new Microsoft.Data.SqlClient.SqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);

            dgvReporte.DataSource = dt;
        }

        private void btnReporteEmpleado_Click(object sender, EventArgs e)
        {
            ReporteEmpleado();
        }

        private void CargarEmpleados()
        {
            var dt = Db.Query("SELECT EmpleadoId, Nombre FROM Empleado WHERE Activo = 1");

            cboEmpleado.DataSource = dt;
            cboEmpleado.DisplayMember = "Nombre";
            cboEmpleado.ValueMember = "EmpleadoId";
        }


        private void ReportePorHerramienta()
        {
            if (cboHerramienta.SelectedValue == null)
            {
                MessageBox.Show("Seleccione una herramienta");
                return;
            }

            using var cn = Db.GetConnection();
            cn.Open();

            var cmd = new Microsoft.Data.SqlClient.SqlCommand(@"
        SELECT 
            h.Codigo,
            h.Nombre AS Herramienta,
            e.Nombre AS Empleado,
            p.FechaPrestamo,
            p.FechaDevolucion,
            d.FechaDevuelta,
            CASE 
                WHEN d.FechaDevuelta IS NULL THEN 'Pendiente'
                ELSE 'Devuelto'
            END AS Estado,
            CASE 
                WHEN d.FechaDevuelta IS NULL AND p.FechaDevolucion < GETDATE()
                    THEN DATEDIFF(DAY, p.FechaDevolucion, GETDATE())
                ELSE 0
            END AS DiasAtraso
        FROM PrestamoDetalle d
        INNER JOIN Prestamo p ON p.PrestamoId = d.PrestamoId
        INNER JOIN Empleado e ON e.EmpleadoId = p.EmpleadoId
        INNER JOIN Herramienta h ON h.HerramientaId = d.HerramientaId
        WHERE h.HerramientaId = @HerramientaId
        ORDER BY p.FechaPrestamo DESC
    ", cn);

            cmd.Parameters.AddWithValue("@HerramientaId", cboHerramienta.SelectedValue);

            var da = new Microsoft.Data.SqlClient.SqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);

            dgvReporte.DataSource = dt;
        }

        private void btnReporteHerramienta_Click(object sender, EventArgs e)
        {
            ReportePorHerramienta();
        }

        private void CargarHerramientas()
        {
            var dt = Db.Query("SELECT HerramientaId, Nombre FROM Herramienta WHERE Activa = 1");

            cboHerramienta.DataSource = dt;
            cboHerramienta.DisplayMember = "Nombre";
            cboHerramienta.ValueMember = "HerramientaId";
        }

        private void ReporteHerramientasDanadas()
        {
            using var cn = Db.GetConnection();
            cn.Open();

            var cmd = new Microsoft.Data.SqlClient.SqlCommand(@"
        SELECT 
    h.Codigo,
    h.Nombre AS Herramienta,
    e.Nombre AS Empleado,
    p.FechaPrestamo,
    d.FechaDevuelta,
    d.ObservacionDevolucion
FROM Herramienta h
INNER JOIN PrestamoDetalle d ON d.HerramientaId = h.HerramientaId
INNER JOIN Prestamo p ON p.PrestamoId = d.PrestamoId
INNER JOIN Empleado e ON e.EmpleadoId = p.EmpleadoId
INNER JOIN (
    SELECT 
        HerramientaId, 
        MAX(FechaDevuelta) AS UltimaFecha
    FROM PrestamoDetalle
    WHERE FechaDevuelta IS NOT NULL
    GROUP BY HerramientaId
) ult ON ult.HerramientaId = d.HerramientaId 
      AND ult.UltimaFecha = d.FechaDevuelta
WHERE h.Estado = 'Dañada'
ORDER BY d.FechaDevuelta DESC;
    ", cn);

            var da = new Microsoft.Data.SqlClient.SqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);

            dgvReporte.DataSource = dt;
        }

        private void btnReporteDanadas_Click(object sender, EventArgs e)
        {
            ReporteHerramientasDanadas();
        }

    }


}
