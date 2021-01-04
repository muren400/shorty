using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace shortcutManager
{
    public partial class SettingsShortcutControl : UserControl
    {
        private ShortcutManager shortcutManager;
        public SettingsShortcutControl(ShortcutManager shortcutManager)
        {
            InitializeComponent();
            this.shortcutManager = shortcutManager;
        }

        private void buttonReloadConfig_Click(object sender, EventArgs e)
        {
            shortcutManager.ReloadConfig();
        }

        private void buttonAddKeystroke_Click(object sender, EventArgs e)
        {
            NewShortcutForm newShortcutForm = new NewShortcutForm();
            DialogResult result = newShortcutForm.ShowDialog();

            if (result == DialogResult.OK)
            {
                shortcutManager.GetShortcuts().Add(newShortcutForm.Shortcut);
                listViewKeystrokes.Items.Add(newShortcutForm.Shortcut);
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            Shortcut shortcut = listViewKeystrokes.SelectedItems.OfType<Shortcut>().First();
            if(shortcut == null)
            {
                return;
            }

            shortcutManager.GetShortcuts().Remove(shortcut);
            listViewKeystrokes.Items.Remove(shortcut);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if(listViewKeystrokes.SelectedItems.Count > 0)
            {
                Shortcut shortcut = listViewKeystrokes.SelectedItems.OfType<Shortcut>().First();
                shortcut.Command = textBoxCommand.Text;
            }

            shortcutManager.WriteShortcuts();
        }
    }
}
