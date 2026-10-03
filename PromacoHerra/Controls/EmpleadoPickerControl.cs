using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;

namespace PromacoHerra.Controls
{
    public class EmpleadoPickerControl : UserControl
    {
        private MaterialTextBox txtNombre = null!;
        private MaterialButton btnBuscar = null!;
        private MaterialButton btnLimpiar = null!;
        private string? _textoTodos;

        public int EmpleadoId { get; private set; }
        public string Nombre { get; private set; } = string.Empty;
        public string Departamento { get; private set; } = string.Empty;
        public string Codigo { get; private set; } = string.Empty;

        /// <summary>El buscador también lista empleados inactivos (reportes e historial).</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IncluirInactivos { get; set; }

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

            // Solo en modo filtro (EstablecerTodos): vuelve a "Todos" cuando hay alguien elegido
            btnLimpiar = new MaterialButton
            {
                Dock = DockStyle.Right,
                Width = 34,
                Text = "",
                Icon = IconChar.Xmark,
                IconSize = 14,
                Variant = MaterialButtonVariant.Default,
                CornerRadius = 4,
                Cursor = Cursors.Hand,
                Visible = false
            };
            btnLimpiar.Click += (s, e) => EstablecerTodos(_textoTodos ?? "Todos");

            txtNombre = new MaterialTextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                PlaceholderText = "Seleccione un empleado...",
                Cursor = Cursors.Hand
            };
            txtNombre.Click += (s, e) => AbrirBusqueda();

            // El último en agregarse se acopla primero: la lupa queda a la derecha y la X junto a ella
            Controls.Add(txtNombre);
            Controls.Add(btnLimpiar);
            Controls.Add(btnBuscar);
        }

        public void Limpiar()
        {
            EmpleadoId = 0;
            Nombre = null!;
            Departamento = null!;
            Codigo = null!;
            txtNombre.Text = string.Empty;
            btnLimpiar.Visible = false;
        }

        // Precarga un empleado ya elegido (p. ej. al editar un registro existente)
        public void Establecer(int empleadoId, string nombre, string codigo, string departamento = "")
        {
            EmpleadoId = empleadoId;
            Nombre = nombre;
            Codigo = codigo;
            Departamento = departamento;
            txtNombre.Text = $"{codigo}  —  {nombre}";
            btnLimpiar.Visible = _textoTodos != null;
        }

        // Deja el picker en el estado "Todos los empleados" (EmpleadoId = 0).
        // Úsalo en filtros/reportes donde no seleccionar a nadie es una opción válida;
        // a partir de ahí, la X junto a la lupa vuelve a este estado.
        public void EstablecerTodos(string texto = "Todos")
        {
            _textoTodos = texto;
            EmpleadoId = 0;
            Nombre = texto;
            Departamento = string.Empty;
            Codigo = string.Empty;
            txtNombre.Text = texto;
            btnLimpiar.Visible = false;
        }

        private void AbrirBusqueda()
        {
            using var frm = new FrmBuscarEmpleado(IncluirInactivos);
            if (frm.ShowDialog(FindForm()) == DialogResult.OK)
            {
                EmpleadoId = frm.EmpleadoIdSeleccionado;
                Nombre = frm.NombreSeleccionado;
                Departamento = frm.DepartamentoSeleccionado;
                Codigo = frm.CodigoSeleccionado;
                txtNombre.Text = $"{frm.CodigoSeleccionado}  —  {frm.NombreSeleccionado}";
                btnLimpiar.Visible = _textoTodos != null;
                EmpleadoSeleccionado?.Invoke(EmpleadoId, Nombre, Departamento, Codigo);
            }
        }
    }
}
