using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using PromacoHerra.Controls;
using PromacoHerra.Services;

namespace PromacoHerra
{
    // Cierre de un mantenimiento activo:
    //   resultado → reparada (vuelve a Disponible) o irreparable (se da de baja)
    //   costo     → lo que le costó a la empresa: materiales/refacciones + mano de obra externa.
    //               La mano de obra interna no se registra (ya la cubre el salario) y en garantía
    //               no hay costo. El folio de factura solo aplica a servicios externos.
    public class FrmCerrarMantenimiento : Form
    {
        private const string Moneda = "Q.";

        private readonly int _mantenimientoId;
        private readonly bool _externo;

        private readonly MaterialRadioButton rbReparada = new();
        private readonly MaterialRadioButton rbBaja = new();
        private readonly MaterialCheckBox chkGarantia = new();
        private readonly NumericUpDown nudMateriales = new();
        private readonly NumericUpDown nudManoObra = new();
        private readonly MaterialTextBox txtFolio = new();
        private readonly Label lblTotal = new();
        private readonly MaterialTextBox txtNotas = new();
        private readonly MaterialButton btnCerrar = new();
        private readonly MaterialButton btnCancelar = new();

        /// <param name="mantenimiento">Fila de sp_Mantenimiento_ObtenerActivos.</param>
        public FrmCerrarMantenimiento(DataRow mantenimiento)
        {
            _mantenimientoId = mantenimiento.Field<int>("MantenimientoId");
            _externo = mantenimiento.Field<string>("TipoServicio") == MantenimientoService.Externo;
            ConstruirInterfaz(mantenimiento);
            ThemeManager.ApplyTheme(this);
            btnCerrar.Variant = MaterialButtonVariant.Primary;
            btnCancelar.Variant = MaterialButtonVariant.Secondary;
            ActualizarCostos();
        }

        private void ConstruirInterfaz(DataRow m)
        {
            Text = "Cerrar mantenimiento";
            ClientSize = new Size(600, 600);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = false;
            MaximizeBox = MinimizeBox = false;
            BackColor = ThemeManager.CardBackground;
            const int x = 24, ancho = 552;
            int y = 16;

            // ── Encabezado: qué unidad, quién la tiene y qué se pidió ──
            Agregar(new Label { Text = "Cerrar mantenimiento", Font = new Font("Segoe UI", 15F, FontStyle.Bold), AutoSize = true }, x, ref y, 34);
            Agregar(new Label
            {
                Text = $"{m["Herramienta"]}  ·  {m["CodigoHerramienta"]}",
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                Size = new Size(ancho, 22),
                AutoEllipsis = true
            }, x, ref y, 24);

            int dias = Convert.ToInt32(m["DiasEnMantenimiento"]);
            string servicio = _externo ? "Externo" : "Interno";
            Agregar(Secundaria($"{m["TipoMantenimiento"]}  ·  {servicio}: {m["RealizadoPor"]}  ·  " +
                               (dias == 1 ? "1 día" : $"{dias} días") + " en mantenimiento", ancho), x, ref y, 22);
            if (m["Descripcion"] is string desc && desc != "")
                Agregar(Secundaria("Trabajo solicitado: " + desc, ancho, 38), x, ref y, 42);
            y += 8;

            // ── Resultado ──
            Agregar(Titulo("Resultado"), x, ref y, 28);
            rbReparada.Text = "Reparada: vuelve al stock disponible";
            rbBaja.Text = "Irreparable: se da de baja y sale del stock";
            rbReparada.Checked = true;
            foreach (var rb in new[] { rbReparada, rbBaja })
            {
                rb.AutoSize = true;
                Agregar(rb, x, ref y, 28);
            }
            y += 12;

            // ── Costo para la empresa ──
            Agregar(Titulo("Costo para la empresa"), x, ref y, 28);
            chkGarantia.Text = "En garantía (el proveedor o fabricante cubre la reparación)";
            chkGarantia.AutoSize = true;
            chkGarantia.CheckedChanged += (_, _) => ActualizarCostos();
            Agregar(chkGarantia, x, ref y, 34);

            ConfigurarMonto(nudMateriales);
            ConfigurarMonto(nudManoObra);
            var lblMateriales = Etiqueta($"Materiales y refacciones ({Moneda})");
            var lblManoObra = Etiqueta($"Mano de obra ({Moneda})");
            Controls.Add(lblMateriales); lblMateriales.Location = new Point(x, y);
            Controls.Add(lblManoObra); lblManoObra.Location = new Point(x + 284, y);
            y += 22;
            nudMateriales.SetBounds(x, y, 260, 28);
            Controls.Add(nudMateriales);
            if (_externo)
            {
                nudManoObra.SetBounds(x + 284, y, 268, 28);
                Controls.Add(nudManoObra);
            }
            else
            {
                var lblSinManoObra = Secundaria("No se registra en servicios internos: ya la cubre el salario del empleado.", 268, 40);
                lblSinManoObra.Location = new Point(x + 284, y - 2);
                Controls.Add(lblSinManoObra);
            }
            y += 40;

            if (_externo)
            {
                Agregar(Etiqueta("Folio de factura del proveedor"), x, ref y, 22);
                txtFolio.Size = new Size(260, 38);
                txtFolio.MaxLength = 50;
                txtFolio.PlaceholderText = "Ej. FAC-10293";
                Agregar(txtFolio, x, ref y, 48);
            }

            lblTotal.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTotal.Size = new Size(ancho, 24);
            Agregar(lblTotal, x, ref y, 34);

            // ── Notas ──
            Agregar(Etiqueta("Notas de cierre"), x, ref y, 22);
            txtNotas.Multiline = true;
            txtNotas.MaxLength = 400;
            txtNotas.Size = new Size(ancho, 64);
            txtNotas.PlaceholderText = "Qué se hizo, piezas cambiadas, recomendaciones…";
            Agregar(txtNotas, x, ref y, 76);

            // ── Pie (el alto del formulario se ajusta a lo que haya arriba) ──
            ClientSize = new Size(600, y + 64);
            btnCerrar.Text = "Cerrar mantenimiento";
            btnCerrar.Icon = FontAwesome.Sharp.IconChar.CheckCircle;
            btnCerrar.IconSize = 16;
            btnCerrar.Size = new Size(220, 40);
            btnCerrar.Location = new Point(x + ancho - btnCerrar.Width, ClientSize.Height - 56);
            btnCerrar.Click += (_, _) => Cerrar();
            btnCancelar.Text = "Cancelar";
            btnCancelar.Size = new Size(120, 40);
            btnCancelar.Location = new Point(btnCerrar.Left - 10 - btnCancelar.Width, btnCerrar.Top);
            btnCancelar.Click += (_, _) => { DialogResult = DialogResult.Cancel; Close(); };
            Controls.Add(btnCerrar);
            Controls.Add(btnCancelar);
            CancelButton = btnCancelar;
        }

        private void Agregar(Control c, int x, ref int y, int alto)
        {
            c.Location = new Point(x, y);
            Controls.Add(c);
            y += alto;
        }

        private static Label Titulo(string texto) =>
            new() { Text = texto, Font = new Font("Segoe UI", 11F, FontStyle.Bold), AutoSize = true };

        private static Label Etiqueta(string texto) =>
            new() { Text = texto, Font = new Font("Segoe UI", 9F, FontStyle.Bold), AutoSize = true };

        private static Label Secundaria(string texto, int ancho, int alto = 20) => new()
        {
            Text = texto,
            Font = new Font("Segoe UI", 9.5F),
            ForeColor = ThemeManager.TextSecondary,
            Size = new Size(ancho, alto),
            AutoEllipsis = true
        };

        private void ConfigurarMonto(NumericUpDown nud)
        {
            nud.DecimalPlaces = 2;
            nud.Maximum = 9999999;
            nud.ThousandsSeparator = true;
            nud.Font = new Font("Segoe UI", 10F);
            nud.ValueChanged += (_, _) => ActualizarCostos();
        }

        // En garantía no hay costo: se ponen en cero y se bloquean
        private void ActualizarCostos()
        {
            bool garantia = chkGarantia.Checked;
            if (garantia) nudMateriales.Value = nudManoObra.Value = 0;
            nudMateriales.Enabled = nudManoObra.Enabled = !garantia;

            decimal total = nudMateriales.Value + (_externo ? nudManoObra.Value : 0);
            lblTotal.Text = garantia ? "Total: sin costo (garantía)" : $"Total: {Moneda} {total:N2}";
        }

        private void Cerrar()
        {
            bool reparada = rbReparada.Checked;
            if (!reparada &&
                MessageBox.Show("La unidad se dará de baja y saldrá del stock de forma definitiva.\n\n¿Continuar?",
                    "Confirmar baja", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                MantenimientoService.RegistrarSalida(
                    _mantenimientoId,
                    reparada,
                    costoMateriales: nudMateriales.Value,
                    costoManoObra: _externo ? nudManoObra.Value : null,
                    enGarantia: chkGarantia.Checked,
                    folioFactura: _externo ? txtFolio.Text.Trim() : null,
                    notasCierre: txtNotas.Text.Trim());
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Errores.Mensaje(ex), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
