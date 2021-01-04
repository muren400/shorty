﻿using System;
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
        private SplitContainer splitContainer;
        private ListView shortcutsListView;
        private TextBox commandTextBox;
        private ShortcutManager shortcutManager;

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
            //splitContainer = new SplitContainer();
            //splitContainer.Dock = DockStyle.Fill;

            //Label labelShortcuts = new Label
            //{
            //    AutoSize = true,
            //    Text = "Shortcuts",
            //    Dock = DockStyle.Top
            //};

            //shortcutsListView = new ListView
            //{
            //    FullRowSelect = true,
            //    Dock = DockStyle.Fill,
            //    LabelEdit = true
            //};

            shortcutsListView = settingsShortcutControl.GetKeystrokesListView();
            shortcutsListView.ItemSelectionChanged += ShortcutSelectedEvent;

            //TableLayoutPanel actionsTable = new TableLayoutPanel
            //{
            //    Dock = DockStyle.Bottom
            //};
            //splitContainer.Panel1.Controls.Add(actionsTable);

            //Button addButton = new Button
            //{
            //    Text = "Add Shortcut",
            //    Dock = DockStyle.Bottom
            //};

            //addButton.Click += AddButton_Click;

            //actionsTable.Controls.Add(addButton);

            //splitContainer.Panel1.Controls.Add(shortcutsListView);
            //splitContainer.Panel1.Controls.Add(labelShortcuts);

            //Label labelCommand = new Label
            //{
            //    AutoSize = true,
            //    Text = "Command",
            //    Dock = DockStyle.Top
            //};

            //commandTextBox = new TextBox
            //{
            //    Multiline = true,
            //    Dock = DockStyle.Fill
            //};

            commandTextBox = settingsShortcutControl.GetCommandTextBox();

            //splitContainer.Panel2.Controls.Add(commandTextBox);
            //splitContainer.Panel2.Controls.Add(labelCommand);

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

            commandTextBox.Text = shortcut.Command;
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