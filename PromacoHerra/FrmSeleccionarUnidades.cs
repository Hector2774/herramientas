using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PromacoHerra.Models;

namespace PromacoHerra
{
    // Modal para elegir exactamente qué unidades físicas de una herramienta se prestan.
    // Solo lista unidades en estado "Disponible". El resultado queda en UnidadesSeleccionadas.
    public partial class FrmSeleccionarUnidades : Form
    {
        private readonly List<UnidadDisponible> _unidades;
        private readonly HashSet<int> _seleccion;
        private bool _cargando;

        // UnidadIds elegidas, en el orden de la lista (por número de unidad)
        public List<int> UnidadesSeleccionadas =>
            _unidades.Where(u => _seleccion.Contains(u.UnidadId)).Select(u => u.UnidadId).ToList();

        public FrmSeleccionarUnidades(HerramientaCatalogoItem item, IEnumerable<int> seleccionadas)
        {
            InitializeComponent();
            ThemeManager.ApplyTheme(this);

            _unidades = item.Unidades.Where(u => u.Estado == "Disponible").ToList();
            _seleccion = new HashSet<int>(seleccionadas);

            lblNombre.Text = item.Nombre;
            lblMeta.Text = string.Join("  ·  ", new[] { item.Codigo, item.Categoria, item.Marca }
                .Where(s => !string.IsNullOrWhiteSpace(s)));
            lblMeta.ForeColor = ThemeManager.TextSecondary;
            lblSeleccionadas.ForeColor = ThemeManager.AccentBlue;
            btnConfirmar.Variant = PromacoHerra.Controls.MaterialButtonVariant.Primary;

            txtFiltro.TextChanged += (s, e) => CargarLista();
            lvUnidades.ItemChecked += lvUnidades_ItemChecked;
            btnConfirmar.Click += (s, e) => { DialogResult = DialogResult.OK; Close(); };

            CargarLista();
        }

        private void CargarLista()
        {
            string filtro = txtFiltro.Text.Trim();

            _cargando = true;
            lvUnidades.BeginUpdate();
            lvUnidades.Items.Clear();
            foreach (var u in _unidades)
            {
                if (filtro != "" && !u.Codigo.Contains(filtro, StringComparison.OrdinalIgnoreCase))
                    continue;

                var lvi = new ListViewItem(u.Codigo) { Tag = u.UnidadId, Checked = _seleccion.Contains(u.UnidadId) };
                lvi.SubItems.Add(u.Ubicacion);
                lvi.SubItems.Add(u.Estado);
                lvUnidades.Items.Add(lvi);
            }
            lvUnidades.EndUpdate();
            _cargando = false;

            ActualizarContador();
        }

        private void lvUnidades_ItemChecked(object? sender, ItemCheckedEventArgs e)
        {
            if (_cargando) return;

            int id = (int)e.Item.Tag!;
            if (e.Item.Checked) _seleccion.Add(id);
            else _seleccion.Remove(id);

            ActualizarContador();
        }

        private void ActualizarContador()
        {
            lblSeleccionadas.Text = $"Seleccionadas: {_seleccion.Count}";
        }
    }
}
