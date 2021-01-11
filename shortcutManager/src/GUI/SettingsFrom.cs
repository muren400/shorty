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
    public partial class SettingsForm : Form
    {
        private readonly ShortcutManager shortcutManager;

        public SettingsForm(ShortcutManager shortcutManager)
        {
            this.shortcutManager = shortcutManager;

            InitializeComponent();

            SettingsShortcuts itemShortcuts = new SettingsShortcuts(shortcutManager, "Shortcuts", 0);
            tabPageShortcuts.Controls.Add(itemShortcuts.getSettingsControl());
        }
    }
}
