
namespace shortcutManager
{
    partial class SettingsShortcutControl
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsShortcutControl));
            this.keystrokeCommandSplit = new System.Windows.Forms.SplitContainer();
            this.keystrokeBox = new System.Windows.Forms.GroupBox();
            this.listViewKeystrokes = new System.Windows.Forms.ListView();
            this.commandBox = new System.Windows.Forms.GroupBox();
            this.textBoxCommand = new System.Windows.Forms.TextBox();
            this.actionsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonAddKeystroke = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.buttonReloadConfig = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.keystrokeCommandSplit)).BeginInit();
            this.keystrokeCommandSplit.Panel1.SuspendLayout();
            this.keystrokeCommandSplit.Panel2.SuspendLayout();
            this.keystrokeCommandSplit.SuspendLayout();
            this.keystrokeBox.SuspendLayout();
            this.commandBox.SuspendLayout();
            this.actionsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // keystrokeCommandSplit
            // 
            resources.ApplyResources(this.keystrokeCommandSplit, "keystrokeCommandSplit");
            this.keystrokeCommandSplit.Name = "keystrokeCommandSplit";
            // 
            // keystrokeCommandSplit.Panel1
            // 
            resources.ApplyResources(this.keystrokeCommandSplit.Panel1, "keystrokeCommandSplit.Panel1");
            this.keystrokeCommandSplit.Panel1.Controls.Add(this.keystrokeBox);
            // 
            // keystrokeCommandSplit.Panel2
            // 
            resources.ApplyResources(this.keystrokeCommandSplit.Panel2, "keystrokeCommandSplit.Panel2");
            this.keystrokeCommandSplit.Panel2.Controls.Add(this.commandBox);
            // 
            // keystrokeBox
            // 
            resources.ApplyResources(this.keystrokeBox, "keystrokeBox");
            this.keystrokeBox.Controls.Add(this.listViewKeystrokes);
            this.keystrokeBox.Name = "keystrokeBox";
            this.keystrokeBox.TabStop = false;
            // 
            // listViewKeystrokes
            // 
            resources.ApplyResources(this.listViewKeystrokes, "listViewKeystrokes");
            this.listViewKeystrokes.HideSelection = false;
            this.listViewKeystrokes.MultiSelect = false;
            this.listViewKeystrokes.Name = "listViewKeystrokes";
            this.listViewKeystrokes.UseCompatibleStateImageBehavior = false;
            this.listViewKeystrokes.View = System.Windows.Forms.View.List;
            // 
            // commandBox
            // 
            resources.ApplyResources(this.commandBox, "commandBox");
            this.commandBox.Controls.Add(this.textBoxCommand);
            this.commandBox.Name = "commandBox";
            this.commandBox.TabStop = false;
            // 
            // textBoxCommand
            // 
            resources.ApplyResources(this.textBoxCommand, "textBoxCommand");
            this.textBoxCommand.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxCommand.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBoxCommand.HideSelection = false;
            this.textBoxCommand.Name = "textBoxCommand";
            // 
            // actionsPanel
            // 
            resources.ApplyResources(this.actionsPanel, "actionsPanel");
            this.actionsPanel.Controls.Add(this.buttonAddKeystroke);
            this.actionsPanel.Controls.Add(this.buttonRemove);
            this.actionsPanel.Controls.Add(this.buttonReloadConfig);
            this.actionsPanel.Controls.Add(this.buttonSave);
            this.actionsPanel.Name = "actionsPanel";
            // 
            // buttonAddKeystroke
            // 
            resources.ApplyResources(this.buttonAddKeystroke, "buttonAddKeystroke");
            this.buttonAddKeystroke.Name = "buttonAddKeystroke";
            this.buttonAddKeystroke.UseVisualStyleBackColor = true;
            this.buttonAddKeystroke.Click += new System.EventHandler(this.buttonAddKeystroke_Click);
            // 
            // buttonRemove
            // 
            resources.ApplyResources(this.buttonRemove, "buttonRemove");
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // buttonReloadConfig
            // 
            resources.ApplyResources(this.buttonReloadConfig, "buttonReloadConfig");
            this.buttonReloadConfig.Name = "buttonReloadConfig";
            this.buttonReloadConfig.UseVisualStyleBackColor = true;
            this.buttonReloadConfig.Click += new System.EventHandler(this.buttonReloadConfig_Click);
            // 
            // buttonSave
            // 
            resources.ApplyResources(this.buttonSave, "buttonSave");
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // SettingsShortcutControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.keystrokeCommandSplit);
            this.Controls.Add(this.actionsPanel);
            this.Name = "SettingsShortcutControl";
            this.keystrokeCommandSplit.Panel1.ResumeLayout(false);
            this.keystrokeCommandSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.keystrokeCommandSplit)).EndInit();
            this.keystrokeCommandSplit.ResumeLayout(false);
            this.keystrokeBox.ResumeLayout(false);
            this.commandBox.ResumeLayout(false);
            this.commandBox.PerformLayout();
            this.actionsPanel.ResumeLayout(false);
            this.actionsPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer keystrokeCommandSplit;
        private System.Windows.Forms.GroupBox keystrokeBox;
        private System.Windows.Forms.ListView listViewKeystrokes;
        private System.Windows.Forms.GroupBox commandBox;
        private System.Windows.Forms.TextBox textBoxCommand;
        private System.Windows.Forms.FlowLayoutPanel actionsPanel;
        private System.Windows.Forms.Button buttonAddKeystroke;
        private System.Windows.Forms.Button buttonReloadConfig;
        private System.Windows.Forms.Button buttonSave;

        public System.Windows.Forms.ListView GetKeystrokesListView()
        {
            return listViewKeystrokes;
        }

        public System.Windows.Forms.TextBox GetCommandTextBox()
        {
            return textBoxCommand;
        }

        private System.Windows.Forms.Button buttonRemove;
    }
}
