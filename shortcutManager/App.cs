using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace shortcutManager
{
    static class App
    {
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ShortcutManager shortcutManager = new ShortcutManager();
            Systray systray = new Systray(shortcutManager);
            Application.Run();
        }
    }
}
