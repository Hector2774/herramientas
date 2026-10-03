using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using PromacoHerra.Controls;
using PromacoHerra.Models;
using PromacoHerra.Services;

namespace PromacoHerra
{
    // Devolución de herramientas:
    //   izquierda → préstamos con herramientas pendientes (búsqueda por empleado o código)
    //   derecha   → resumen del empleado, alerta si está vencido y una fila por unidad pendiente
    //               con su condición (Bueno / Dañado / Perdido) y nota
    // Se puede devolver solo lo seleccionado o todo; el préstamo se cierra solo al quedar sin pendientes.
    public partial class FrmDevolucion : Form
    {
        private static readonly Color StatFondo = Color.FromArgb(248, 249, 251);

        private List<PrestamoActivo> _prestamos = new();
        private PrestamoItemControl? _itemSeleccionado;
        private readonly List<HerramientaDevolucionRow> _filas = new();

        private PrestamoActivo? Actual => _itemSeleccionado?.Prestamo;

        public FrmDevolucion()
        {
            InitializeComponent();
            ThemeManager.ApplyTheme(this);
            AplicarEstilos();

            this.Load += FrmDevolucion_Load;
            txtBuscar.TextChanged += (s, e) => AplicarFiltro();
            lnkSeleccionarTodo.LinkClicked += lnkSeleccionarTodo_LinkClicked;
            btnDevolverSeleccionadas.Click += btnDevolverSeleccionadas_Click;
            btnDevolverTodo.Click += btnDevolverTodo_Click;
            pnlIzquierdo.Paint += pnlIzquierdo_Paint;

            flpPrestamos.ClientSizeChanged += (s, e) => AjustarAnchos(flpPrestamos);
            flpHerramientas.ClientSizeChanged += (s, e) => AjustarAnchos(flpHerramientas);
        }

        // ThemeManager deja todas las etiquetas en TextPrimary; aquí se ajustan las demás
        private void AplicarEstilos()
        {
            foreach (var lbl in new[] { lblSecPrestamos, lblSinPrestamos, lblVacio, lblMetaEmpleado,
                                        lblStatPrestadasCap, lblStatSeleccionadasCap, lblStatPlazoCap,
                                        lblResumenSeleccion })
                lbl.ForeColor = ThemeManager.TextSecondary;

            lblStatSeleccionadas.ForeColor = ThemeManager.AccentBlue;
            lnkSeleccionarTodo.LinkColor = lnkSeleccionarTodo.ActiveLinkColor = ThemeManager.AccentBlue;

            pnlResumen.CornerRadius = 12;
            foreach (var stat in new[] { pnlStatPrestadas, pnlStatSeleccionadas, pnlStatPlazo })
            {
                stat.ShowShadow = false;
                stat.CornerRadius = 8;
                stat.BackColor = StatFondo;
            }

            pnlAlerta.ShowShadow = false;
            pnlAlerta.CornerRadius = 8;
            pnlAlerta.BackColor = Plazo.RojoFondo;
            lblAlerta.ForeColor = Plazo.RojoTexto;

            btnDevolverSeleccionadas.Variant = MaterialButtonVariant.Secondary;
            btnDevolverTodo.Variant = MaterialButtonVariant.Success;
        }

        private void FrmDevolucion_Load(object? sender, EventArgs e)
        {
            CargarPrestamos(null);

            // Sin foco inicial en el buscador, para que se vea su placeholder
            BeginInvoke(new Action(() => ActiveControl = null));
        }

        // ══════════════════════════════════════════════════════════
        // LISTA IZQUIERDA
        // ══════════════════════════════════════════════════════════

        // Recarga los préstamos activos; si mantenerId sigue en la lista, lo vuelve a seleccionar
        private void CargarPrestamos(int? mantenerId)
        {
            try
            {
                _prestamos = DevolucionService.ListarPrestamosActivos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Errores.Mensaje(ex), "Error al cargar préstamos",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                _prestamos = new();
            }

            _itemSeleccionado = null;

            flpPrestamos.SuspendLayout();
            foreach (Control c in flpPrestamos.Controls.Cast<Control>().ToList())
                c.Dispose();
            flpPrestamos.Controls.Clear();

            foreach (var p in _prestamos)
            {
                var item = new PrestamoItemControl(p);
                item.Click += (s, e) => SeleccionarPrestamo((PrestamoItemControl)s!);
                flpPrestamos.Controls.Add(item);
            }
            flpPrestamos.ResumeLayout();

            AplicarFiltro();

            var mantener = flpPrestamos.Controls.OfType<PrestamoItemControl>()
                                       .FirstOrDefault(i => i.Prestamo.PrestamoId == mantenerId);
            if (mantener != null) SeleccionarPrestamo(mantener);
            else MostrarVacio();
        }

        private void AplicarFiltro()
        {
            string termino = txtBuscar.Text.Trim();
            int visibles = 0;

            flpPrestamos.SuspendLayout();
            foreach (PrestamoItemControl item in flpPrestamos.Controls)
            {
                var p = item.Prestamo;
                bool visible = termino == ""
                    || Contiene(p.Empleado, termino) || Contiene(p.CodigoEmpleado, termino)
                    || Contiene(p.Codigo, termino) || p.PrestamoId.ToString() == termino.TrimStart('#');
                item.Visible = visible;
                if (visible) visibles++;
            }
            flpPrestamos.ResumeLayout();
            AjustarAnchos(flpPrestamos);

            lblSinPrestamos.Text = _prestamos.Count == 0
                ? "No hay préstamos activos."
                : "Ningún préstamo coincide con la búsqueda.";
            lblSinPrestamos.Visible = visibles == 0;
            flpPrestamos.Visible = visibles > 0;
        }

        // Sin distinguir mayúsculas ni acentos
        private static bool Contiene(string texto, string termino) =>
            !string.IsNullOrEmpty(texto) &&
            CultureInfo.InvariantCulture.CompareInfo.IndexOf(texto, termino,
                CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0;

        // En un FlowLayoutPanel vertical los hijos no se estiran solos: ocupan el ancho útil
        private static void AjustarAnchos(FlowLayoutPanel flp)
        {
            int ancho = flp.ClientSize.Width - flp.Padding.Horizontal;
            foreach (Control c in flp.Controls)
            {
                int w = Math.Max(100, ancho - c.Margin.Horizontal);
                if (c.Width != w) c.Width = w;
            }
        }

        private void SeleccionarPrestamo(PrestamoItemControl item)
        {
            if (_itemSeleccionado != null) _itemSeleccionado.Seleccionado = false;
            _itemSeleccionado = item;
            item.Seleccionado = true;
            CargarDetalle(item.Prestamo);
        }

        private void pnlIzquierdo_Paint(object? sender, PaintEventArgs e)
        {
            using var pen = new Pen(ThemeManager.BorderColor, 1);
            e.Graphics.DrawLine(pen, pnlIzquierdo.Width - 1, 0, pnlIzquierdo.Width - 1, pnlIzquierdo.Height);
        }

        // ══════════════════════════════════════════════════════════
        // DETALLE
        // ══════════════════════════════════════════════════════════
        private void MostrarVacio()
        {
            LimpiarFilas();
            tlpDetalle.Visible = false;
            lblVacio.Visible = true;
        }

        private void CargarDetalle(PrestamoActivo p)
        {
            List<DetallePendiente> detalle;
            try
            {
                detalle = DevolucionService.ListarDetallePendiente(p.PrestamoId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(Errores.Mensaje(ex), "Error al cargar el detalle",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Resumen del empleado
            avatarEmpleado.Nombre = p.Empleado;
            avatarEmpleado.ColorIndex = p.ColorIndex;
            lblNombreEmpleado.Text = p.Empleado;
            lblMetaEmpleado.Text = string.Join("  ·  ", new[]
            {
                string.IsNullOrEmpty(p.Departamento) ? "Sin departamento" : p.Departamento,
                p.Codigo,
                $"Prestado el {p.FechaPrestamo:dd/MM/yyyy}"
            });

            lblStatPrestadas.Text = detalle.Count.ToString();
            var (textoPlazo, _, colorPlazo) = Plazo.Describir(p.DiasRestantes);
            lblStatPlazo.Text = textoPlazo;
            lblStatPlazo.ForeColor = colorPlazo;
            lblStatPlazoCap.Text = $"Fecha límite {p.FechaDevolucionEsperada:dd/MM/yyyy}";

            // Alerta de vencido
            pnlAlerta.Visible = p.Vencido;
            if (p.Vencido)
                lblAlerta.Text = $"⚠  Este préstamo está vencido hace {Plazo.Dias(-p.DiasRestantes)}. " +
                                 "Registra la devolución lo antes posible.";

            // Una fila por unidad pendiente
            LimpiarFilas();
            flpHerramientas.SuspendLayout();
            foreach (var d in detalle)
            {
                var fila = new HerramientaDevolucionRow(d);
                fila.SeleccionCambiada += (s, e) => ActualizarSeleccion();
                _filas.Add(fila);
                flpHerramientas.Controls.Add(fila);
            }
            flpHerramientas.ResumeLayout();

            lblVacio.Visible = false;
            tlpDetalle.Visible = true;
            AjustarAnchos(flpHerramientas);
            ActualizarSeleccion();
        }

        private void LimpiarFilas()
        {
            foreach (var f in _filas) f.Dispose();
            _filas.Clear();
            flpHerramientas.Controls.Clear();
        }

        private void ActualizarSeleccion()
        {
            int n = _filas.Count(f => f.Seleccionado);
            int total = _filas.Count;

            lblStatSeleccionadas.Text = n.ToString();
            lblResumenSeleccion.Text = n == 0
                ? "Selecciona las herramientas a devolver"
                : $"{n} de {total} herramienta{(total == 1 ? "" : "s")} seleccionada{(n == 1 ? "" : "s")}";
            lblResumenSeleccion.ForeColor = n == 0 ? ThemeManager.TextSecondary : ThemeManager.TextPrimary;

            btnDevolverSeleccionadas.Enabled = n > 0;
            lnkSeleccionarTodo.Text = total > 0 && n == total ? "Deseleccionar todo" : "Seleccionar todo";
        }

        private void lnkSeleccionarTodo_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
        {
            bool seleccionar = !_filas.All(f => f.Seleccionado);
            foreach (var f in _filas) f.Seleccionado = seleccionar;
        }

        // ══════════════════════════════════════════════════════════
        // DEVOLVER
        // ══════════════════════════════════════════════════════════
        private void btnDevolverSeleccionadas_Click(object? sender, EventArgs e)
        {
            Devolver(_filas.Where(f => f.Seleccionado).ToList());
        }

        private void btnDevolverTodo_Click(object? sender, EventArgs e)
        {
            foreach (var f in _filas) f.Seleccionado = true;
            Devolver(_filas.ToList());
        }

        private void Devolver(List<HerramientaDevolucionRow> filas)
        {
            var prestamo = Actual;
            if (prestamo == null || filas.Count == 0) return;

            var items = filas.Select(f => new DevolucionItem
            {
                PrestamoDetalleId = f.Detalle.PrestamoDetalleId,
                Condicion = f.Condicion,
                Nota = f.Nota
            }).ToList();

            int buenas = items.Count(i => i.Condicion == CondicionDevolucion.Bueno);
            int dañadas = items.Count(i => i.Condicion == CondicionDevolucion.Dañado);
            int perdidas = items.Count(i => i.Condicion == CondicionDevolucion.Perdido);

            string confirmacion = $"Se devolverán {Cantidad(items.Count, "herramienta")} del préstamo {prestamo.Codigo}:\n\n"
                                + DesgloseCondiciones(buenas, dañadas, perdidas);
            if (dañadas > 0)
                confirmacion += "\n\nLas unidades dañadas no volverán al stock hasta pasar por mantenimiento.";
            if (perdidas > 0)
                confirmacion += "\n\nLas unidades perdidas se descontarán del stock.";
            confirmacion += "\n\n¿Confirmar la devolución?";

            if (MessageBox.Show(confirmacion, "Confirmar devolución", MessageBoxButtons.YesNo,
                    dañadas + perdidas > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            ResultadoDevolucion r;
            try
            {
                r = DevolucionService.RegistrarVarias(prestamo.PrestamoId, items);
            }
            catch (Exception ex)
            {
                MessageBox.Show(Errores.Mensaje(ex), "Error al registrar la devolución",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                CargarPrestamos(prestamo.PrestamoId);
                return;
            }

            string resumen = $"Se registró la devolución de {Cantidad(r.Total, "herramienta")}:\n\n"
                           + DesgloseCondiciones(r.Buenas, r.Dañadas, r.Perdidas) + "\n\n"
                           + (r.PrestamoCerrado
                               ? $"El préstamo {prestamo.Codigo} quedó cerrado."
                               : $"Quedan {Cantidad(r.Pendientes, "herramienta")} pendiente{(r.Pendientes == 1 ? "" : "s")} en el préstamo {prestamo.Codigo}.");

            MessageBox.Show(resumen, "Devolución registrada", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Si el préstamo se cerró desaparece de la lista; si no, se vuelve a mostrar con lo pendiente
            CargarPrestamos(r.PrestamoCerrado ? null : prestamo.PrestamoId);
        }

        private static string Cantidad(int n, string palabra) => n == 1 ? $"1 {palabra}" : $"{n} {palabra}s";

        private static string DesgloseCondiciones(int buenas, int dañadas, int perdidas)
        {
            var lineas = new List<string>();
            if (buenas > 0) lineas.Add($"  ✓ {buenas} en buen estado");
            if (dañadas > 0) lineas.Add($"  ⚠ {dañadas} dañada{(dañadas == 1 ? "" : "s")}");
            if (perdidas > 0) lineas.Add($"  ✕ {perdidas} perdida{(perdidas == 1 ? "" : "s")}");
            return string.Join("\n", lineas);
        }
    }
}
