using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace shortcutManager
{
    public abstract class SettingsItem : ListViewItem
    {
        public SettingsItem(string text, int imageIndex) : base(text, imageIndex)
        {
            //InitItem();
        }

        abstract protected void InitItem();

        abstract public Control getSettingsControl();
    }
}
