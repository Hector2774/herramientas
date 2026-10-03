using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows.Forms;

namespace PromacoHerra.Controls
{
    // Wrapper de TextBox: el TextBox nativo es una ventana propia y no se puede recortar/pintar
    // alrededor, así que se envuelve sin borde y se dibuja un underline Material que anima su
    // color al recibir foco. Expone por delegación la superficie que ya usa el resto del proyecto
    // (Text, TextChanged, KeyDown, Focus, Clear, SelectionStart, PlaceholderText, PasswordChar...).
    public class MaterialTextBox : UserControl
    {
        private readonly TextBox _inner;
        private readonly ColorAnimator _underlineAnimator;
        private Label? _floatingLabel;
        private bool _useFloatingLabel;
        private string _floatingLabelText = string.Empty;

        public MaterialTextBox()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint |
                      ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            DoubleBuffered = true;

            BackColor = ThemeManager.CardBackground;
            Padding = new Padding(0, 0, 0, 4);
            Height = 38;
            MinimumSize = new Size(0, 38);

            _inner = new TextBox
            {
                BorderStyle = BorderStyle.None,
                Dock = DockStyle.Fill,
                BackColor = ThemeManager.CardBackground,
                ForeColor = ThemeManager.TextPrimary,
                Font = new Font("Segoe UI", ThemeManager.BaseFontSize)
            };
            _inner.TextChanged += (s, e) => { OnTextChanged(EventArgs.Empty); UpdateFloatingLabel(); };
            _inner.KeyDown += (s, e) => OnKeyDown(e);
            _inner.KeyPress += (s, e) => OnKeyPress(e);
            _inner.Enter += (s, e) => { _underlineAnimator.AnimateTo(ColorFoco); UpdateFloatingLabel(); };
            _inner.Leave += (s, e) => { _underlineAnimator.AnimateTo(ThemeManager.BorderColor); UpdateFloatingLabel(); };
            // El TextBox interno ocupa casi toda el área visible, así que un click ahí nunca
            // llegaría al Click del wrapper — se reenvía para que siga funcionando igual que antes.
            _inner.Click += (s, e) => OnClick(EventArgs.Empty);

            Controls.Add(_inner);

            _underlineAnimator = new ColorAnimator(this, ThemeManager.BorderColor);
        }

        // ── Superficie reenviada al TextBox interno ────────────────
        public override string Text
        {
            get => _inner?.Text ?? string.Empty;
            [param: AllowNull]
            set { if (_inner != null) _inner.Text = value ?? string.Empty; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string PlaceholderText
        {
            get => _inner.PlaceholderText;
            set => _inner.PlaceholderText = value;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public char PasswordChar
        {
            get => _inner.PasswordChar;
            set => _inner.PasswordChar = value;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool Multiline
        {
            get => _inner.Multiline;
            set => _inner.Multiline = value;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool ReadOnly
        {
            get => _inner.ReadOnly;
            set => _inner.ReadOnly = value;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int SelectionStart
        {
            get => _inner.SelectionStart;
            set => _inner.SelectionStart = value;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int MaxLength
        {
            get => _inner.MaxLength;
            set => _inner.MaxLength = value;
        }

        // Color del underline (y de la etiqueta flotante) con foco. null = ThemeManager.AccentBlue
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color? FocusColor { get; set; }

        private Color ColorFoco => FocusColor ?? ThemeManager.AccentBlue;

        public new bool Focus() => _inner.Focus();

        public void Clear() => _inner.Clear();

        // ── Etiqueta flotante opcional (apagada por defecto) ───────
        // La mayoría de los campos ya tienen un Label fijo al lado; se deja lista
        // para activarla puntualmente sin rehacer el layout de los formularios.
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool UseFloatingLabel
        {
            get => _useFloatingLabel;
            set { _useFloatingLabel = value; EnsureFloatingLabel(); UpdateFloatingLabel(); }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string FloatingLabelText
        {
            get => _floatingLabelText;
            set { _floatingLabelText = value; if (_floatingLabel != null) _floatingLabel.Text = value; }
        }

        private void EnsureFloatingLabel()
        {
            if (!_useFloatingLabel || _floatingLabel != null) return;

            _floatingLabel = new Label
            {
                AutoSize = false,
                BackColor = ThemeManager.CardBackground,
                Text = _floatingLabelText,
                Font = new Font("Segoe UI", ThemeManager.BaseFontSize),
                ForeColor = ThemeManager.TextSecondary,
                Location = new Point(2, 6),
                Size = new Size(Math.Max(0, Width - 4), 18)
            };
            Controls.Add(_floatingLabel);
            _floatingLabel.BringToFront();
        }

        private void UpdateFloatingLabel()
        {
            if (_floatingLabel == null) return;

            bool arriba = _inner.Focused || !string.IsNullOrEmpty(_inner.Text);
            if (arriba)
            {
                _floatingLabel.Font = new Font("Segoe UI", 7.5f, FontStyle.Bold);
                _floatingLabel.ForeColor = ColorFoco;
                _floatingLabel.Location = new Point(2, -2);
            }
            else
            {
                _floatingLabel.Font = new Font("Segoe UI", ThemeManager.BaseFontSize);
                _floatingLabel.ForeColor = ThemeManager.TextSecondary;
                _floatingLabel.Location = new Point(2, 6);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using var pen = new Pen(_underlineAnimator.Current, 2);
            e.Graphics.DrawLine(pen, 0, Height - 1, Width, Height - 1);
        }
    }
}
