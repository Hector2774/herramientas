using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;

namespace PromacoHerra.Controls
{
    // Igual que EmpleadoPickerControl, pero abre FrmBuscarHerramienta
    public class HerramientaPickerControl : UserControl
    {
        private readonly MaterialTextBox txtNombre;
        private readonly MaterialButton btnBuscar;
        private readonly MaterialButton btnLimpiar;
        private string? _textoTodas;

        public int HerramientaId { get; private set; }
        public string Nombre { get; private set; } = string.Empty;
        public string Codigo { get; private set; } = string.Empty;

        /// <summary>El buscador también lista herramientas dadas de baja (reportes e historial).</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IncluirInactivas { get; set; }

        public event Action<int, string, string>? HerramientaSeleccionada;

        public HerramientaPickerControl()
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

            // Solo en modo filtro (EstablecerTodas): vuelve a "Todas" cuando hay una elegida
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
            btnLimpiar.Click += (s, e) => EstablecerTodas(_textoTodas ?? "Todas");

            txtNombre = new MaterialTextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                PlaceholderText = "Seleccione una herramienta...",
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
            HerramientaId = 0;
            Nombre = string.Empty;
            Codigo = string.Empty;
            txtNombre.Text = string.Empty;
            btnLimpiar.Visible = false;
        }

        // Deja el picker en el estado "Todas las herramientas" (HerramientaId = 0), para filtros;
        // a partir de ahí, la X junto a la lupa vuelve a este estado.
        public void EstablecerTodas(string texto = "Todas")
        {
            _textoTodas = texto;
            HerramientaId = 0;
            Nombre = texto;
            Codigo = string.Empty;
            txtNombre.Text = texto;
            btnLimpiar.Visible = false;
        }

        private void AbrirBusqueda()
        {
            using var frm = new FrmBuscarHerramienta(IncluirInactivas);
            if (frm.ShowDialog(FindForm()) == DialogResult.OK)
            {
                HerramientaId = frm.HerramientaIdSeleccionada;
                Nombre = frm.NombreSeleccionado;
                Codigo = frm.CodigoSeleccionado;
                txtNombre.Text = $"{frm.CodigoSeleccionado}  —  {frm.NombreSeleccionado}";
                btnLimpiar.Visible = _textoTodas != null;
                HerramientaSeleccionada?.Invoke(HerramientaId, Nombre, Codigo);
            }
        }
    }
}
