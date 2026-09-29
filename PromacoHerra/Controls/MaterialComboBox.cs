using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows.Forms;

namespace PromacoHerra.Controls
{
    // Wrapper de ComboBox (mismo motivo que MaterialTextBox: el desplegable nativo no se puede
    // recortar). Expone por delegación la superficie real que usa el proyecto para enlazar datos
    // (DataSource/DisplayMember/ValueMember/SelectedValue/Items...) y dibuja un underline animado.
    public class MaterialComboBox : UserControl
    {
        private readonly ComboBox _inner;
        private readonly ColorAnimator _underlineAnimator;

        public MaterialComboBox()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint |
                      ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            DoubleBuffered = true;

            BackColor = ThemeManager.CardBackground;
            Padding = new Padding(0, 0, 0, 4);
            Height = 38;
            MinimumSize = new Size(0, 38);

            _inner = new ComboBox
            {
                Dock = DockStyle.Fill,
                FlatStyle = FlatStyle.Flat,
                BackColor = ThemeManager.CardBackground,
                ForeColor = ThemeManager.TextPrimary,
                Font = new Font("Segoe UI", ThemeManager.BaseFontSize),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            _inner.Enter += (s, e) => _underlineAnimator.AnimateTo(ThemeManager.AccentBlue);
            _inner.Leave += (s, e) => _underlineAnimator.AnimateTo(ThemeManager.BorderColor);
            _inner.SelectedIndexChanged += (s, e) => OnSelectedIndexChanged(e);
            _inner.SelectedValueChanged += (s, e) => OnSelectedValueChanged(e);

            Controls.Add(_inner);

            _underlineAnimator = new ColorAnimator(this, ThemeManager.BorderColor);
        }

        // ── Superficie reenviada al ComboBox interno ───────────────
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public object? DataSource { get => _inner.DataSource; set => _inner.DataSource = value; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string DisplayMember { get => _inner.DisplayMember; set => _inner.DisplayMember = value; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string ValueMember { get => _inner.ValueMember; set => _inner.ValueMember = value; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public object? SelectedValue { get => _inner.SelectedValue; set => _inner.SelectedValue = value; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int SelectedIndex { get => _inner.SelectedIndex; set => _inner.SelectedIndex = value; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public object? SelectedItem { get => _inner.SelectedItem; set => _inner.SelectedItem = value; }

        public ComboBox.ObjectCollection Items => _inner.Items;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public ComboBoxStyle DropDownStyle { get => _inner.DropDownStyle; set => _inner.DropDownStyle = value; }

        public override string Text
        {
            get => _inner?.Text ?? string.Empty;
            [param: AllowNull]
            set { if (_inner != null) _inner.Text = value ?? string.Empty; }
        }

        public event EventHandler? SelectedIndexChanged;
        public event EventHandler? SelectedValueChanged;

        protected virtual void OnSelectedIndexChanged(EventArgs e) => SelectedIndexChanged?.Invoke(this, e);
        protected virtual void OnSelectedValueChanged(EventArgs e) => SelectedValueChanged?.Invoke(this, e);

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using var pen = new Pen(_underlineAnimator.Current, 2);
            e.Graphics.DrawLine(pen, 0, Height - 1, Width, Height - 1);
        }
    }
}
