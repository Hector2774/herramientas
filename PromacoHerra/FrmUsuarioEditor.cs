using System;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using PromacoHerra.Controls;
using PromacoHerra.Services;

namespace PromacoHerra
{
    // Alta y edición de usuarios: empleado, nombre de usuario y rol (catálogo Rol).
    //   nuevo  → el sistema genera una contraseña temporal que el empleado cambia al primer ingreso
    //   editar → todo menos la contraseña (para eso está "Restablecer contraseña").
    //            El empleado no se cambia si el usuario ya registró mantenimientos, y un
    //            administrador no puede cambiarse su propio rol.
    public class FrmUsuarioEditor : Form
    {
        private static readonly Regex FormatoUsuario = new("^[A-Za-z0-9._-]{3,50}$");
        private const string EtiquetaSecundaria = "secundaria";

        private readonly DataRow? _usuario;          // null = usuario nuevo
        private readonly bool _esYo;
        private readonly bool _tieneMovimientos;
        private DataTable _roles = new();

        private readonly EmpleadoPickerControl pickerEmpleado = new();
        private readonly MaterialTextBox txtUsuario = new();
        private readonly MaterialComboBox cboRol = new();
        private readonly Label lblRolDescripcion = new();
        private readonly Label lblError = new();
        private readonly MaterialButton btnGuardar = new();
        private readonly MaterialButton btnCancelar = new();

        public int UsuarioId { get; private set; }
        public string Username { get; private set; } = "";
        /// <summary>Solo al crear: la contraseña temporal generada.</summary>
        public string PasswordTemporal { get; private set; } = "";

        private bool EsNuevo => _usuario == null;

        /// <param name="usuario">Fila de sp_Usuario_ObtenerTodos; null para crear uno nuevo.</param>
        public FrmUsuarioEditor(DataRow? usuario = null)
        {
            _usuario = usuario;
            if (usuario != null)
            {
                UsuarioId = Convert.ToInt32(usuario["UsuarioId"]);
                _esYo = UsuarioId == Sesion.UsuarioId;
                _tieneMovimientos = Convert.ToBoolean(usuario["TieneMovimientos"]);
            }

            ConstruirInterfaz();
            ThemeManager.ApplyTheme(this);
            btnGuardar.Variant = MaterialButtonVariant.Primary;
            btnCancelar.Variant = MaterialButtonVariant.Secondary;
            lblError.ForeColor = ThemeManager.DangerRed;
            foreach (Control c in Controls)
                if (c is Label { Tag: EtiquetaSecundaria }) c.ForeColor = ThemeManager.TextSecondary;

            Load += (_, _) => CargarDatos();
        }

        private void ConstruirInterfaz()
        {
            Text = EsNuevo ? "Nuevo usuario" : "Editar usuario";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = false;
            MaximizeBox = MinimizeBox = false;
            BackColor = ThemeManager.CardBackground;
            const int x = 24, ancho = 432;
            int y = 16;

            Agregar(new Label { Text = Text, Font = new Font("Segoe UI", 15F, FontStyle.Bold), AutoSize = true }, x, ref y, 36);
            Agregar(Secundaria(EsNuevo
                    ? "Cada persona que entrega o recibe herramientas debe tener su propio usuario: " +
                      "así cada préstamo queda registrado a nombre de quien lo aprobó."
                    : "La contraseña no se edita aquí. Si el empleado la olvidó, usa \"Restablecer contraseña\".",
                ancho, EsNuevo ? 58 : 40), x, ref y, EsNuevo ? 66 : 48);

            // ── Empleado ──
            Agregar(Etiqueta("Empleado"), x, ref y, 22);
            pickerEmpleado.Size = new Size(ancho, 38);
            pickerEmpleado.EmpleadoSeleccionado += (_, _, _, _) => lblError.Text = "";
            Agregar(pickerEmpleado, x, ref y, 44);
            if (_tieneMovimientos)
            {
                pickerEmpleado.Enabled = false;
                Agregar(Secundaria("No se puede cambiar: ya registró mantenimientos a su nombre.", ancho, 20), x, ref y, 28);
            }
            else y += 8;

            // ── Nombre de usuario ──
            Agregar(Etiqueta("Nombre de usuario"), x, ref y, 22);
            txtUsuario.Size = new Size(ancho, 38);
            txtUsuario.MaxLength = 50;
            txtUsuario.PlaceholderText = "Ej. jperez";
            txtUsuario.TextChanged += (_, _) => lblError.Text = "";
            Agregar(txtUsuario, x, ref y, 44);
            Agregar(Secundaria("Letras, números, punto, guion o guion bajo; sin espacios.", ancho, 20), x, ref y, 32);

            // ── Rol ──
            Agregar(Etiqueta("Rol"), x, ref y, 22);
            cboRol.Size = new Size(ancho, 38);
            cboRol.DropDownStyle = ComboBoxStyle.DropDownList;
            cboRol.SelectedIndexChanged += (_, _) => MostrarDescripcionRol();
            cboRol.Enabled = !_esYo;
            Agregar(cboRol, x, ref y, 44);
            lblRolDescripcion.Font = new Font("Segoe UI", 9.5F);
            lblRolDescripcion.Size = new Size(ancho, 38);
            lblRolDescripcion.Tag = EtiquetaSecundaria;
            Agregar(lblRolDescripcion, x, ref y, 42);

            if (EsNuevo)
                Agregar(Secundaria("El sistema generará una contraseña temporal. Entrégasela al empleado: " +
                                   "deberá cambiarla la primera vez que ingrese.", ancho, 40), x, ref y, 44);

            lblError.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblError.Size = new Size(ancho, 38);
            Agregar(lblError, x, ref y, 44);

            ClientSize = new Size(x * 2 + ancho, y + 56);
            btnGuardar.Text = EsNuevo ? "Crear usuario" : "Guardar cambios";
            btnGuardar.Icon = EsNuevo ? FontAwesome.Sharp.IconChar.UserPlus : FontAwesome.Sharp.IconChar.FloppyDisk;
            btnGuardar.IconSize = 16;
            btnGuardar.Size = new Size(180, 40);
            btnGuardar.Location = new Point(x + ancho - btnGuardar.Width, y);
            btnGuardar.Click += (_, _) => Guardar();

            btnCancelar.Text = "Cancelar";
            btnCancelar.Size = new Size(110, 40);
            btnCancelar.Location = new Point(btnGuardar.Left - 10 - btnCancelar.Width, y);
            btnCancelar.Click += (_, _) => { DialogResult = DialogResult.Cancel; Close(); };

            Controls.Add(btnGuardar);
            Controls.Add(btnCancelar);
            CancelButton = btnCancelar;
        }

        private void CargarDatos()
        {
            try { _roles = RolService.ObtenerTodos(); }
            catch (Exception ex) { lblError.Text = ex.Message; btnGuardar.Enabled = false; return; }

            cboRol.DataSource = _roles;
            cboRol.DisplayMember = "Nombre";
            cboRol.ValueMember = "RolId";

            if (_usuario == null)
            {
                // Por defecto el rol con menos permisos
                cboRol.SelectedIndex = _roles.Rows.Count > 0 ? 0 : -1;
            }
            else
            {
                pickerEmpleado.Establecer(Convert.ToInt32(_usuario["EmpleadoId"]),
                    _usuario["Empleado"].ToString() ?? "", _usuario["CodigoEmpleado"].ToString() ?? "");
                txtUsuario.Text = _usuario["Username"].ToString() ?? "";
                cboRol.SelectedValue = Convert.ToInt32(_usuario["RolId"]);
            }
            MostrarDescripcionRol();
            if (_esYo) lblRolDescripcion.Text = "No puedes cambiar tu propio rol: otro administrador debe hacerlo.";
        }

        private void MostrarDescripcionRol()
        {
            lblRolDescripcion.Text = cboRol.SelectedItem is DataRowView r ? r["Descripcion"] as string ?? "" : "";
        }

        private void Agregar(Control c, int x, ref int y, int alto)
        {
            c.Location = new Point(x, y);
            Controls.Add(c);
            y += alto;
        }

        private static Label Etiqueta(string texto) =>
            new() { Text = texto, Font = new Font("Segoe UI", 9F, FontStyle.Bold), AutoSize = true };

        private static Label Secundaria(string texto, int ancho, int alto) => new()
        {
            Text = texto,
            Font = new Font("Segoe UI", 9.5F),
            Size = new Size(ancho, alto),
            Tag = EtiquetaSecundaria
        };

        private void Guardar()
        {
            string username = txtUsuario.Text.Trim();

            if (pickerEmpleado.EmpleadoId == 0) { lblError.Text = "Seleccione el empleado."; return; }
            if (!FormatoUsuario.IsMatch(username))
            {
                lblError.Text = "El nombre de usuario debe tener de 3 a 50 caracteres válidos.";
                return;
            }
            if (cboRol.SelectedValue is not int rolId) { lblError.Text = "Seleccione el rol."; return; }

            try
            {
                Cursor = Cursors.WaitCursor;
                if (_usuario == null)
                {
                    string temporal = PasswordHasher.GenerarTemporal();
                    UsuarioId = UsuarioService.Crear(username, pickerEmpleado.EmpleadoId, rolId, temporal);
                    PasswordTemporal = temporal;
                }
                else
                {
                    UsuarioService.Actualizar(UsuarioId, username, pickerEmpleado.EmpleadoId, rolId,
                        activo: Convert.ToBoolean(_usuario["Activo"]));
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                return;
            }
            finally { Cursor = Cursors.Default; }

            Username = username;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
