using System;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;

namespace PromacoHerra.Controls
{
    public class EmpleadoPickerControl : UserControl
    {
        private MaterialTextBox txtNombre = null!;
        private MaterialButton btnBuscar = null!;

        public int EmpleadoId { get; private set; }
        public string Nombre { get; private set; } = string.Empty;
        public string Departamento { get; private set; } = string.Empty;
        public string Codigo { get; private set; } = string.Empty;

        public event Action<int, string, string, string>? EmpleadoSeleccionado;

        public EmpleadoPickerControl()
        {
            Height = 28;
            MinimumSize = new Size(200, 28);

            btnBuscar = new MaterialButton
            {
                Dock = DockStyle.Right,
                Width = 34,
                Text = "",
                Icon = IconChar.Search,
                IconSize = 18,
                Variant = MaterialButtonVariant.Primary,
                CornerRadius = 4,
                Cursor = Cursors.Hand
            };
            btnBuscar.Click += (s, e) => AbrirBusqueda();

            txtNombre = new MaterialTextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                PlaceholderText = "Seleccione un empleado...",
                Cursor = Cursors.Hand
            };
            txtNombre.Click += (s, e) => AbrirBusqueda();

            Controls.Add(txtNombre);
            Controls.Add(btnBuscar);
        }

        public void Limpiar()
        {
            EmpleadoId = 0;
            Nombre = null!;
            Departamento = null!;
            Codigo = null!;
            txtNombre.Text = string.Empty;
        }

        // Deja el picker en el estado "Todos los empleados" (EmpleadoId = 0).
        // Úsalo en filtros/reportes donde no seleccionar a nadie es una opción válida.
        public void EstablecerTodos(string texto = "Todos")
        {
            EmpleadoId = 0;
            Nombre = texto;
            Departamento = string.Empty;
            Codigo = string.Empty;
            txtNombre.Text = texto;
        }

        private void AbrirBusqueda()
        {
            using var frm = new FrmBuscarEmpleado();
            if (frm.ShowDialog(FindForm()) == DialogResult.OK)
            {
                EmpleadoId = frm.EmpleadoIdSeleccionado;
                Nombre = frm.NombreSeleccionado;
                Departamento = frm.DepartamentoSeleccionado;
                Codigo = frm.CodigoSeleccionado;
                txtNombre.Text = $"{frm.CodigoSeleccionado}  —  {frm.NombreSeleccionado}";
                EmpleadoSeleccionado?.Invoke(EmpleadoId, Nombre, Departamento, Codigo);
            }
        }
    }
}
