using System;
using System.Drawing;
using System.Windows.Forms;
using PromacoHerra.Controls;
using PromacoHerra.Services;

namespace PromacoHerra
{
    // Cambio de contraseña del usuario en sesión:
    //   voluntario  → desde el menú "Mi contraseña"; pide la contraseña actual
    //   obligatorio → al entrar con una contraseña temporal (usuario nuevo o restablecido) o con
    //                 una que ya no cumple las reglas; no pide la actual (se acaba de verificar)
    //                 y cancelar significa no entrar al sistema
    public class FrmCambiarPassword : Form
    {
        private readonly bool _obligatorio;

        private readonly MaterialTextBox txtActual = new();
        private readonly MaterialTextBox txtNueva = new();
        private readonly MaterialTextBox txtConfirmar = new();
        private readonly Label lblError = new();
        private readonly MaterialButton btnGuardar = new();
        private readonly MaterialButton btnCancelar = new();

        public FrmCambiarPassword(bool obligatorio)
        {
            _obligatorio = obligatorio;
            ConstruirInterfaz();
            ThemeManager.ApplyTheme(this);
            btnGuardar.Variant = MaterialButtonVariant.Primary;
            btnCancelar.Variant = MaterialButtonVariant.Secondary;
            lblError.ForeColor = ThemeManager.DangerRed;
            // ApplyTheme deja todas las etiquetas en TextPrimary
            foreach (Control c in Controls)
                if (c is Label { Tag: EtiquetaSecundaria }) c.ForeColor = ThemeManager.TextSecondary;
        }

        private const string EtiquetaSecundaria = "secundaria";

        private void ConstruirInterfaz()
        {
            Text = "Cambiar contraseña";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = _obligatorio ? FormStartPosition.CenterScreen : FormStartPosition.CenterParent;
            ShowInTaskbar = _obligatorio;
            MaximizeBox = MinimizeBox = false;
            BackColor = ThemeManager.CardBackground;
            const int x = 24, ancho = 372;
            int y = 16;

            Agregar(new Label
            {
                Text = _obligatorio ? "Crea tu contraseña" : "Cambiar mi contraseña",
                Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                AutoSize = true
            }, x, ref y, 36);

            string intro = _obligatorio
                ? $"Hola, {Sesion.NombreEmpleado}. Antes de continuar, reemplaza tu contraseña temporal por una personal."
                : $"Usuario: {Sesion.Username}";
            Agregar(Secundaria(intro, ancho, _obligatorio ? 40 : 20), x, ref y, _obligatorio ? 48 : 30);

            if (!_obligatorio)
                Campo("Contraseña actual", txtActual, x, ancho, ref y);
            Campo("Nueva contraseña", txtNueva, x, ancho, ref y);
            Campo("Confirmar nueva contraseña", txtConfirmar, x, ancho, ref y);

            Agregar(Secundaria($"Mínimo {PasswordHasher.LongitudMinima} caracteres, con letras y números, " +
                               "y sin incluir tu nombre de usuario.", ancho, 36), x, ref y, 40);

            lblError.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblError.Size = new Size(ancho, 38);
            Agregar(lblError, x, ref y, 44);

            ClientSize = new Size(x * 2 + ancho, y + 56);
            btnGuardar.Text = "Guardar contraseña";
            btnGuardar.Icon = FontAwesome.Sharp.IconChar.Key;
            btnGuardar.IconSize = 16;
            btnGuardar.Size = new Size(200, 40);
            btnGuardar.Location = new Point(x + ancho - btnGuardar.Width, y);
            btnGuardar.Click += (_, _) => Guardar();

            btnCancelar.Text = _obligatorio ? "Salir" : "Cancelar";
            btnCancelar.Size = new Size(110, 40);
            btnCancelar.Location = new Point(btnGuardar.Left - 10 - btnCancelar.Width, y);
            btnCancelar.Click += (_, _) => { DialogResult = DialogResult.Cancel; Close(); };

            Controls.Add(btnGuardar);
            Controls.Add(btnCancelar);
            AcceptButton = btnGuardar;
            CancelButton = btnCancelar;

            Shown += (_, _) => (_obligatorio ? txtNueva : txtActual).Focus();
        }

        private void Campo(string etiqueta, MaterialTextBox txt, int x, int ancho, ref int y)
        {
            Agregar(new Label { Text = etiqueta, Font = new Font("Segoe UI", 9F, FontStyle.Bold), AutoSize = true }, x, ref y, 20);
            txt.PasswordChar = '●';
            txt.MaxLength = 128;
            txt.Size = new Size(ancho, 38);
            txt.TextChanged += (_, _) => lblError.Text = "";
            Agregar(txt, x, ref y, 50);
        }

        private void Agregar(Control c, int x, ref int y, int alto)
        {
            c.Location = new Point(x, y);
            Controls.Add(c);
            y += alto;
        }

        private static Label Secundaria(string texto, int ancho, int alto) => new()
        {
            Text = texto,
            Font = new Font("Segoe UI", 9.5F),
            Size = new Size(ancho, alto),
            Tag = EtiquetaSecundaria
        };

        private void Guardar()
        {
            // Sin Trim: los espacios forman parte de la contraseña
            string nueva = txtNueva.Text;

            if (nueva != txtConfirmar.Text) { MostrarError("La confirmación no coincide con la nueva contraseña."); return; }
            if (!_obligatorio && nueva == txtActual.Text) { MostrarError("La nueva contraseña debe ser distinta de la actual."); return; }

            string? regla = PasswordHasher.ValidarNueva(nueva, Sesion.Username);
            if (regla != null) { MostrarError(regla); return; }

            try
            {
                Cursor = Cursors.WaitCursor;
                UsuarioService.CambiarMiPassword(_obligatorio ? null : txtActual.Text, nueva);
            }
            catch (Exception ex)
            {
                MostrarError(ex.Message);
                return;
            }
            finally { Cursor = Cursors.Default; }

            MessageBox.Show("Tu contraseña se actualizó correctamente.", "Contraseña actualizada",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void MostrarError(string mensaje)
        {
            lblError.Text = mensaje;
            System.Media.SystemSounds.Beep.Play();
        }
    }
}
