using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace shortcutManager
{

    public class SettingsShortcuts : SettingsItem
    {
        private SettingsShortcutControl settingsShortcutControl;
        private ListView shortcutsListView;
        private readonly ShortcutManager shortcutManager;

        public SettingsShortcuts(ShortcutManager shortcutManager, string text, int imageIndex) : base(text, imageIndex) {
            this.shortcutManager = shortcutManager;
            InitItem();
            this.shortcutManager.PropertyChanged += ShortcutsManagerPropertyChanged;
        }

        public override Control getSettingsControl()
        {
            return settingsShortcutControl;
        }

        protected override void InitItem()
        {
            settingsShortcutControl = new SettingsShortcutControl(shortcutManager)
            {
                Dock = DockStyle.Fill
            };

            shortcutsListView = settingsShortcutControl.GetKeystrokesListView();
            shortcutsListView.ItemSelectionChanged += ShortcutSelectedEvent;

            ReloadShortcus();
        }

        public void ReloadShortcus() {
            shortcutsListView.Clear();

            List<Shortcut> shortcuts = shortcutManager.GetShortcuts();
            foreach (Shortcut shortcut in shortcuts)
            {
                shortcutsListView.Items.Add(shortcut);
            }

            shortcuts.First<Shortcut>().Selected = true;
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            NewShortcutForm newShortcutForm = new NewShortcutForm();
            DialogResult result = newShortcutForm.ShowDialog();

            if(result == DialogResult.OK)
            {
                shortcutManager.GetShortcuts().Add(newShortcutForm.Shortcut);
                shortcutsListView.Items.Add(newShortcutForm.Shortcut);
                shortcutsListView.Refresh();
            }
        }

        private void Confirmation_KeyPress(object sender, KeyPressEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void KeyInputForm_KeyUp(object sender, KeyEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ShortcutSelectedEvent(Object sender, ListViewItemSelectionChangedEventArgs args)
        {
            var item = args.Item;

            if (item is Shortcut == false)
                return;

            Shortcut shortcut = (Shortcut)item;

            settingsShortcutControl.GetNameTextBox().Text = shortcut.ShortcutName;
            settingsShortcutControl.GetCommandTextBox().Text = shortcut.Command;
        }

        public void ShortcutsManagerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == ShortcutManager.SHORTCUTS_PROPERTY_CHANGED)
            {
                ReloadShortcus();
            }
        }
    }
}
