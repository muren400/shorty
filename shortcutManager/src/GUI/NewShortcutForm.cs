using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace shortcutManager
{
    class NewShortcutForm :  Form
    {
        public Shortcut Shortcut { get; }
        private readonly Stack<Keys> inputs;

        public NewShortcutForm() : this(new Shortcut()) { }


        public NewShortcutForm(Shortcut shortcut)
        {
            Shortcut = shortcut;
            inputs = new Stack<Keys>();

            Width = 500;
            Height = 100;
            Text = "Press at least two Keys";

            TextBox textBoxKeys = new TextBox
            {
                //Dock = DockStyle.Fill
            };

            textBoxKeys.KeyPress += TextBoxKeys_KeyPress;
            textBoxKeys.KeyUp += TextBoxKeys_KeyUp;
            textBoxKeys.TextChanged += TextBoxKeys_TextChanged;
            textBoxKeys.SetBounds(12, 36, 372, 20);
            textBoxKeys.Anchor |= AnchorStyles.Right;

            //Controls.Add(textBoxKeys);

            Button buttonOk = new Button();
            buttonOk.Text = "OK";
            buttonOk.DialogResult = DialogResult.OK;
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            
            Button buttonCancel = new Button();
            buttonCancel.Text = "Cancel";
            buttonCancel.DialogResult = DialogResult.Cancel;
            buttonCancel.SetBounds(309, 72, 75, 23);
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            //ClientSize = new Size(396, 107);
            ClientSize = new System.Drawing.Size(396, 107);
            Controls.AddRange(new Control[] { textBoxKeys, buttonOk, buttonCancel });
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterScreen;
            MinimizeBox = false;
            MaximizeBox = false;
            AcceptButton = buttonOk;
            CancelButton = buttonCancel;
        }

        [DllImport("user32.dll")]
        static extern bool HideCaret(IntPtr hWnd);

        private void TextBoxKeys_TextChanged(object sender, EventArgs e)
        {
            HideCaret(((TextBox)sender).Handle);
        }

        private void TextBoxKeys_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void TextBoxKeys_KeyUp(object sender, KeyEventArgs e)
        {
            if (sender is TextBox == false)
            {
                return;
            }

            TextBox text = (TextBox)sender;

            if((e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back) && inputs.Count > 0)
            {
                Shortcut.DeleteKey(inputs.Pop());
            }
            else if (Shortcut.AddKey(e.KeyCode))
            {
                inputs.Push(e.KeyCode);
            }

            text.Text = Shortcut.GetKeysAsString();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // NewShortcutForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "NewShortcutForm";
            this.ResumeLayout(false);

        }
    }
}
