using shortcutManager.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace shortcutManager
{
    class Systray
    {
        private readonly NotifyIcon systrayIcon;
        private readonly ContextMenu systrayContextMenu;
        private readonly ShortcutManager shortcutManager;
        private readonly Dictionary<MenuItem, Shortcut> shortcutItems;

        public Systray(ShortcutManager shortcutManager)
        {
            systrayIcon = new NotifyIcon();
            this.shortcutManager = shortcutManager;

            shortcutItems = new Dictionary<MenuItem, Shortcut>();

            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Resources));

#if DEBUG
            systrayIcon.Icon = (Icon)resources.GetObject("Icon_debug.icon");
#else
            systrayIcon.Icon = (Icon)resources.GetObject("Icon.icon");
#endif

            systrayIcon.Text = "Customizable Shortcuts";

            systrayContextMenu = new ContextMenu();

            RepopulateMenu();

            this.shortcutManager.PropertyChanged += ShortcutsManagerPropertyChanged;

            systrayIcon.ContextMenu = systrayContextMenu;
            systrayIcon.Visible = true;
        }

        public void RepopulateMenu()
        {
            systrayContextMenu.MenuItems.Clear();

            int index = 0;

            foreach (Shortcut shortcut in shortcutManager.GetShortcuts())
            {
                AddShortcutItem(shortcut, index++);
            }

            systrayContextMenu.MenuItems.Add("-");

            MenuItem exitItem = new MenuItem();
            exitItem.Index = index++;
            exitItem.Text = "Exit";
            exitItem.Click += new EventHandler(MenuItemExit);

            MenuItem settingsItem = new MenuItem();
            settingsItem.Index = index++;
            settingsItem.Text = "Settings";
            settingsItem.Click += new EventHandler(MenuItemSettings);

            systrayContextMenu.MenuItems.AddRange(new MenuItem[]
            {
                settingsItem,
                exitItem
            });
        }

        private void AddShortcutItem(Shortcut shortcut, int index)
        {
            MenuItem item = new MenuItem();
            item.Index = index;
            item.Text = shortcut.ShortcutName + " (" + shortcut.GetKeysAsString() + ")";
            systrayContextMenu.MenuItems.Add(item);
            item.Click += new EventHandler(MenuItemShortcut);

            shortcutItems.Add(item, shortcut);
        }

        private void MenuItemShortcut(object Sender, EventArgs e)
        {
            if(Sender is MenuItem == false)
            {
                return;
            }

            if(shortcutItems.TryGetValue((MenuItem) Sender, out Shortcut shortcut) == false)
            {
                return;
            }

            shortcut.RunCommand();
        }

            private void MenuItemSettings(object Sender, EventArgs e)
        {
            SettingsForm settingsDialog = new SettingsForm(shortcutManager);

            if (settingsDialog.ShowDialog() == DialogResult.OK)
            {
            }
            else
            {
            }
            settingsDialog.Dispose();
        }

        private void MenuItemExit(object Sender, EventArgs e)
        {
            shortcutManager.Stop();
            Application.Exit();
            systrayIcon.Visible = false;
        }

        public void ShortcutsManagerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == ShortcutManager.SHORTCUTS_PROPERTY_CHANGED)
            {
                RepopulateMenu();
            }
        }
    }
}
