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



    }
}
