using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using PromacoHerra.Controls;
using PromacoHerra.Services;

namespace PromacoHerra
{
    // Administración de usuarios (solo administradores):
    //   encabezado → título y "Nuevo usuario"
    //   grilla     → usuario, empleado, rol, estado y situación de la contraseña
    //   barra      → acciones sobre el usuario seleccionado: editar (todo menos la contraseña),
    //                restablecer contraseña, activar / desactivar. Doble clic en la fila = editar.
    // Los usuarios no se eliminan: préstamos y mantenimientos guardan quién los registró.
    public class FrmUsuarios : Form
    {
        private readonly Label lblTitulo = new();
        private readonly Label lblSubtitulo = new();
        private readonly MaterialButton btnNuevo = new();

        private readonly DataGridView dgvUsuarios = new();
        private readonly DataGridViewTextBoxColumn colUsuario = Columna("Username", "Usuario", 18);
        private readonly DataGridViewTextBoxColumn colEmpleado = Columna("Empleado", "Empleado", 36);
        private readonly DataGridViewTextBoxColumn colRol = Columna("Rol", "Rol", 16);
        private readonly DataGridViewTextBoxColumn colEstado = Columna("Activo", "Estado", 14);
        private readonly DataGridViewTextBoxColumn colPassword = Columna("DebeCambiarPassword", "Contraseña", 22);

        private readonly Panel pnlAcciones = new();
        private readonly Label lblSeleccion = new();
        private readonly MaterialButton btnRestablecer = new();
        private readonly MaterialButton btnEditar = new();
        private readonly MaterialButton btnEstado = new();

        private DataRow? _seleccionado;

        public FrmUsuarios()
        {
            ConstruirInterfaz();
            ThemeManager.ApplyTheme(this);
            AplicarEstilos();
            Load += (_, _) => Cargar(null);
        }

        // ══════════════════════════════════════════════════════════
        // INTERFAZ
        // ══════════════════════════════════════════════════════════
        private static DataGridViewTextBoxColumn Columna(string propiedad, string encabezado, float peso) => new()
        {
            DataPropertyName = propiedad,
            HeaderText = encabezado,
            Name = "col" + propiedad,
            FillWeight = peso,
            ReadOnly = true
        };

        private void ConstruirInterfaz()
        {
            Text = "Usuarios";
            Padding = new Padding(24, 16, 24, 24);

            // ── Encabezado ──
            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 84 };
            lblTitulo.Text = "Usuarios";
            lblTitulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitulo.Location = new Point(0, 4);
            lblTitulo.AutoSize = true;
            lblSubtitulo.Text = "Quién puede ingresar al sistema y con qué rol. Los préstamos quedan a nombre del usuario que los aprueba.";
            lblSubtitulo.Font = new Font("Segoe UI", 10F);
            lblSubtitulo.Location = new Point(0, 44);
            lblSubtitulo.AutoSize = true;

            btnNuevo.Text = "Nuevo usuario";
            btnNuevo.Icon = FontAwesome.Sharp.IconChar.UserPlus;
            btnNuevo.IconSize = 16;
            btnNuevo.Size = new Size(170, 40);
            btnNuevo.Click += (_, _) => NuevoUsuario();
            pnlHeader.Controls.AddRange(new Control[] { lblTitulo, lblSubtitulo, btnNuevo });
            pnlHeader.Resize += (_, _) => btnNuevo.Location = new Point(pnlHeader.Width - btnNuevo.Width, 6);

            // ── Grilla ──
            dgvUsuarios.Dock = DockStyle.Fill;
            dgvUsuarios.AutoGenerateColumns = false;
            dgvUsuarios.Columns.AddRange(colUsuario, colEmpleado, colRol, colEstado, colPassword);
            dgvUsuarios.ReadOnly = true;
            dgvUsuarios.AllowUserToAddRows = dgvUsuarios.AllowUserToDeleteRows = false;
            dgvUsuarios.AllowUserToResizeRows = false;
            dgvUsuarios.RowHeadersVisible = false;
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.BorderStyle = BorderStyle.None;
            dgvUsuarios.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvUsuarios.SelectionChanged += (_, _) => ActualizarSeleccion();
            dgvUsuarios.CellDoubleClick += (_, e) => { if (e.RowIndex >= 0) EditarUsuario(); };
            dgvUsuarios.CellPainting += dgvUsuarios_CellPainting;
            dgvUsuarios.CellFormatting += dgvUsuarios_CellFormatting;

            var pnlGrid = new RoundedPanel { Dock = DockStyle.Fill, Padding = new Padding(12), CornerRadius = 12, BackColor = ThemeManager.CardBackground };
            pnlGrid.Controls.Add(dgvUsuarios);

            // ── Barra de acciones del usuario seleccionado ──
            pnlAcciones.Dock = DockStyle.Bottom;
            pnlAcciones.Height = 72;
            pnlAcciones.Padding = new Padding(0, 16, 0, 0);
            lblSeleccion.Font = new Font("Segoe UI", 10F);
            lblSeleccion.AutoSize = false;
            lblSeleccion.AutoEllipsis = true;
            lblSeleccion.TextAlign = ContentAlignment.MiddleLeft;

            btnRestablecer.Text = "Restablecer contraseña";
            btnRestablecer.Icon = FontAwesome.Sharp.IconChar.Key;
            btnRestablecer.IconSize = 15;
            btnRestablecer.Size = new Size(220, 40);
            btnRestablecer.Click += (_, _) => RestablecerPassword();

            btnEditar.Text = "Editar";
            btnEditar.Icon = FontAwesome.Sharp.IconChar.UserPen;
            btnEditar.IconSize = 15;
            btnEditar.Size = new Size(120, 40);
            btnEditar.Click += (_, _) => EditarUsuario();

            btnEstado.IconSize = 15;
            btnEstado.Size = new Size(140, 40);
            btnEstado.Click += (_, _) => CambiarEstado();

            pnlAcciones.Controls.AddRange(new Control[] { lblSeleccion, btnEditar, btnRestablecer, btnEstado });
            pnlAcciones.Resize += (_, _) => AlinearAcciones();

            // Acoplado: el último agregado se acopla primero
            Controls.Add(pnlGrid);
            Controls.Add(pnlAcciones);
            Controls.Add(pnlHeader);
        }

        private void AlinearAcciones()
        {
            int y = pnlAcciones.Padding.Top;
            int x = pnlAcciones.Width;
            foreach (var btn in new[] { btnEstado, btnRestablecer, btnEditar })
            {
                x -= btn.Width;
                btn.Location = new Point(x, y);
                x -= 10;
            }
            lblSeleccion.SetBounds(0, y, Math.Max(0, x - 10), 40);
        }

        private void AplicarEstilos()
        {
            BackColor = ThemeManager.AppBackground;
            foreach (var lbl in new[] { lblSubtitulo, lblSeleccion })
                lbl.ForeColor = ThemeManager.TextSecondary;

            btnNuevo.Variant = MaterialButtonVariant.Primary;
            btnRestablecer.Variant = MaterialButtonVariant.Default;
            btnEditar.Variant = MaterialButtonVariant.Default;

            dgvUsuarios.BackgroundColor = ThemeManager.CardBackground;
            dgvUsuarios.GridColor = ThemeManager.BorderColor;
            dgvUsuarios.EnableHeadersVisualStyles = false;
            dgvUsuarios.ColumnHeadersDefaultCellStyle.BackColor = ThemeManager.CardBackground;
            dgvUsuarios.ColumnHeadersDefaultCellStyle.ForeColor = ThemeManager.TextSecondary;
            dgvUsuarios.ColumnHeadersDefaultCellStyle.SelectionBackColor = ThemeManager.CardBackground;
            dgvUsuarios.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvUsuarios.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvUsuarios.ColumnHeadersHeight = 40;
            dgvUsuarios.DefaultCellStyle.BackColor = ThemeManager.CardBackground;
            dgvUsuarios.DefaultCellStyle.ForeColor = ThemeManager.TextPrimary;
            dgvUsuarios.DefaultCellStyle.SelectionBackColor = ThemeManager.AccentBlueSoft;
            dgvUsuarios.DefaultCellStyle.SelectionForeColor = ThemeManager.TextPrimary;
            dgvUsuarios.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvUsuarios.AlternatingRowsDefaultCellStyle.BackColor = ThemeManager.CardBackground;
            dgvUsuarios.RowTemplate.Height = 48;
            dgvUsuarios.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsuarios.MultiSelect = false;
            colUsuario.DefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        }

        // ══════════════════════════════════════════════════════════
        // DATOS
        // ══════════════════════════════════════════════════════════
        private void Cargar(int? seleccionarId)
        {
            try
            {
                dgvUsuarios.DataSource = UsuarioService.ObtenerTodos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al cargar usuarios", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            dgvUsuarios.ClearSelection();
            foreach (DataGridViewRow fila in dgvUsuarios.Rows)
            {
                if (fila.DataBoundItem is DataRowView v && Convert.ToInt32(v["UsuarioId"]) == seleccionarId)
                {
                    fila.Selected = true;
                    dgvUsuarios.CurrentCell = fila.Cells[0];
                    break;
                }
            }
            ActualizarSeleccion();
        }

        private void ActualizarSeleccion()
        {
            _seleccionado = dgvUsuarios.SelectedRows.Count > 0 && dgvUsuarios.SelectedRows[0].DataBoundItem is DataRowView v
                ? v.Row : null;

            if (_seleccionado == null)
            {
                lblSeleccion.Text = "Selecciona un usuario para editarlo, restablecer su contraseña o desactivarlo.";
                btnEditar.Enabled = btnRestablecer.Enabled = btnEstado.Enabled = false;
                btnEstado.Text = "Desactivar";
                btnEstado.Icon = FontAwesome.Sharp.IconChar.UserSlash;
                btnEstado.Variant = MaterialButtonVariant.Default;
                return;
            }

            bool esYo = UsuarioId(_seleccionado) == Sesion.UsuarioId;
            bool activo = _seleccionado.Field<bool>("Activo");

            lblSeleccion.Text = esYo
                ? $"{_seleccionado["Username"]} es tu usuario: cambia tu contraseña desde \"Mi contraseña\" en el menú."
                : $"Seleccionado: {_seleccionado["Username"]} · {_seleccionado["Empleado"]}";

            btnEstado.Text = activo ? "Desactivar" : "Activar";
            btnEstado.Icon = activo ? FontAwesome.Sharp.IconChar.UserSlash : FontAwesome.Sharp.IconChar.UserCheck;
            btnEstado.Variant = activo ? MaterialButtonVariant.Danger : MaterialButtonVariant.Success;

            // Sobre sí mismo solo editar (sin cambiar su rol): evita quedarse fuera o sin permisos por error
            btnEditar.Enabled = true;
            btnRestablecer.Enabled = !esYo && activo;
            btnEstado.Enabled = !esYo;
        }

        private static int UsuarioId(DataRow r) => Convert.ToInt32(r["UsuarioId"]);

        // ══════════════════════════════════════════════════════════
        // ACCIONES
        // ══════════════════════════════════════════════════════════
        private void NuevoUsuario()
        {
            using var frm = new FrmUsuarioEditor();
            if (frm.ShowDialog(FindForm()) != DialogResult.OK) return;

            MostrarPasswordTemporal(frm.Username, frm.PasswordTemporal, "Usuario creado");
            Cargar(frm.UsuarioId);
        }

        private void EditarUsuario()
        {
            if (_seleccionado == null) return;
            using var frm = new FrmUsuarioEditor(_seleccionado);
            if (frm.ShowDialog(FindForm()) != DialogResult.OK) return;
            Cargar(frm.UsuarioId);
        }

        private void RestablecerPassword()
        {
            if (_seleccionado == null) return;
            string username = _seleccionado["Username"].ToString() ?? "";

            if (MessageBox.Show($"Se generará una contraseña temporal para \"{username}\" y su contraseña actual dejará de funcionar.\n\n" +
                                "Úsalo cuando el empleado olvidó su contraseña. ¿Continuar?",
                    "Restablecer contraseña", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            string temporal = PasswordHasher.GenerarTemporal();
            int id = UsuarioId(_seleccionado);
            try { UsuarioService.RestablecerPassword(id, temporal); }
            catch (Exception ex) { Error(ex.Message); return; }

            MostrarPasswordTemporal(username, temporal, "Contraseña restablecida");
            Cargar(id);
        }

        private void CambiarEstado()
        {
            if (_seleccionado == null) return;
            bool activo = _seleccionado.Field<bool>("Activo");
            string mensaje = activo
                ? $"\"{_seleccionado["Username"]}\" ya no podrá ingresar al sistema. Su historial de préstamos se conserva.\n\n¿Desactivar el usuario?"
                : $"\"{_seleccionado["Username"]}\" podrá volver a ingresar con su contraseña actual.\n\n¿Activar el usuario?";

            if (MessageBox.Show(mensaje, activo ? "Desactivar usuario" : "Activar usuario", MessageBoxButtons.YesNo,
                    activo ? MessageBoxIcon.Warning : MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            int id = UsuarioId(_seleccionado);
            try
            {
                UsuarioService.Actualizar(id, _seleccionado["Username"].ToString() ?? "",
                    Convert.ToInt32(_seleccionado["EmpleadoId"]), Convert.ToInt32(_seleccionado["RolId"]), !activo);
            }
            catch (Exception ex) { Error(ex.Message); return; }
            Cargar(id);
        }

        // Se copia al portapapeles: el administrador se la entrega al empleado en persona
        private void MostrarPasswordTemporal(string username, string temporal, string titulo)
        {
            bool copiada = false;
            try { Clipboard.SetText(temporal); copiada = true; }
            catch (System.Runtime.InteropServices.ExternalException) { }

            MessageBox.Show(
                $"Contraseña temporal de \"{username}\":\n\n        {temporal}\n\n" +
                (copiada ? "Ya se copió al portapapeles. " : "") +
                "Entrégasela al empleado en persona; el sistema le pedirá cambiarla la primera vez que ingrese.",
                titulo, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static void Error(string mensaje) =>
            MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        // ══════════════════════════════════════════════════════════
        // GRILLA: textos y chips
        // ══════════════════════════════════════════════════════════
        private void dgvUsuarios_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || dgvUsuarios.Rows[e.RowIndex].DataBoundItem is not DataRowView v) return;

            if (e.ColumnIndex == colEmpleado.Index)
            {
                e.Value = $"{v["Empleado"]}  ·  {v["CodigoEmpleado"]}";
                e.FormattingApplied = true;
            }
            else if (e.ColumnIndex == colPassword.Index)
            {
                e.Value = !Convert.ToBoolean(v["PasswordMigrada"]) ? "Se protegerá al ingresar"
                        : Convert.ToBoolean(v["DebeCambiarPassword"]) ? "Temporal: debe cambiarla"
                        : "Personal";
                e.FormattingApplied = true;
            }
        }

        private void dgvUsuarios_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Graphics == null) return;
            if (e.ColumnIndex != colRol.Index && e.ColumnIndex != colEstado.Index) return;
            if (dgvUsuarios.Rows[e.RowIndex].DataBoundItem is not DataRowView v) return;

            string texto;
            Color fondo, frente;
            if (e.ColumnIndex == colRol.Index)
            {
                texto = v["Rol"].ToString() ?? "";
                (fondo, frente) = Convert.ToBoolean(v["AdministraUsuarios"])
                    ? (Paleta.VioletaSuave, Paleta.Violeta)
                    : (Paleta.GrisSuave, Paleta.Gris);
            }
            else if (!Convert.ToBoolean(v["Activo"]))
                (texto, fondo, frente) = ("Inactivo", Paleta.RojoSuave, Paleta.Rojo);
            else if (!Convert.ToBoolean(v["EmpleadoActivo"]))
                (texto, fondo, frente) = ("Empleado inactivo", Paleta.AmarilloSuave, Paleta.Amarillo);
            else
                (texto, fondo, frente) = ("Activo", Paleta.VerdeSuave, Paleta.Verde);

            e.PaintBackground(e.CellBounds, (e.State & DataGridViewElementStates.Selected) != 0);
            StatusChip.DibujarEnCelda(e.Graphics, e.CellBounds, texto, fondo, frente);
            e.Handled = true;
        }
    }
}
