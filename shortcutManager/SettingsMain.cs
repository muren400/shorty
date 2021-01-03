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
    public partial class SettingsMain : Form
    {
        private readonly ListView settingsListView;
        private readonly ShortcutManager shortcutManager;

        public SettingsMain(ShortcutManager shortcutManager)
        {
            this.shortcutManager = shortcutManager;

            InitializeComponent();

            settingsListView = new ListView();
            settingsListView.FullRowSelect = true;
            settingsListView.Dock = DockStyle.Fill;

            ListViewItem itemShortcuts = new SettingsShortcuts(shortcutManager, "Shortcuts", 0);

            settingsListView.Items.AddRange(new ListViewItem[] { itemShortcuts });
            this.splitContainerMain.Panel1.Controls.Add(settingsListView);

            settingsListView.ItemSelectionChanged += SettingsView_ItemSelectionChanged;

            itemShortcuts.Selected = true;
        }

        private void SettingsView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ListViewItem selectedItem = e.Item;

            if (selectedItem is SettingsItem == false)
                return;

            SettingsItem settingsItem = (SettingsItem)selectedItem;
            Control settingsControl = settingsItem.getSettingsControl();

            if (settingsControl == null)
                return;

            this.splitContainerMain.Panel2.Controls.Clear();
            this.splitContainerMain.Panel2.Controls.Add(settingsControl);
        }

        private void ReloadConfigClicked(object sender, EventArgs e)
        {
            shortcutManager.ReloadConfig();
        }
    }
}
