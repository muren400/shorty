using shortcutManager.Properties;
using System;
using System.Collections.Generic;
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
        public Systray(ShortcutManager shortcutManager)
        {
            systrayIcon = new NotifyIcon();
            this.shortcutManager = shortcutManager;

            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Resources));
            systrayIcon.Icon = (Icon)resources.GetObject("Icon.icon");
            systrayIcon.Text = "Customizable Shortcuts";

            systrayContextMenu = new ContextMenu();

            MenuItem exitItem = new MenuItem();
            exitItem.Index = 0;
            exitItem.Text = "E&xit";
            exitItem.Click += new EventHandler(MenuItemExit);

            MenuItem settingsItem = new MenuItem();
            settingsItem.Index = 0;
            settingsItem.Text = "&Settings";
            settingsItem.Click += new EventHandler(MenuItemSettings);

            systrayContextMenu.MenuItems.AddRange(new MenuItem[]
            {
                settingsItem,
                exitItem 
            });

            systrayIcon.ContextMenu = systrayContextMenu;
            systrayIcon.Visible = true;
        }

        private void MenuItemSettings(object Sender, EventArgs e)
        {
            SettingsMain settingsDialog = new SettingsMain(shortcutManager);

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
            Application.Exit();
            systrayIcon.Visible = false;
        }
    }
}
