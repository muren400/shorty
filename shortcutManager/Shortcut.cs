using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace shortcutManager
{
    public class Shortcut : ListViewItem
    {
        private char[] keySeparators = new char[] {'+'};

        private ISet<Keys> keys;

        public string Command { get; }

        public Shortcut()
        {
            keys = new HashSet<Keys>();
            Command = "";
        }

        public Shortcut(string strKeys, string strCommand) : base(strKeys)
        {
            if(strKeys != null)
            {
                keys = new HashSet<Keys>();

                String[] aKeys = strKeys.Replace(" ", "").Split(new char[] { '+' });
                foreach(String strKey in aKeys)
                {
                    try
                    {
                        Keys key = (Keys)Enum.Parse(typeof(Keys), strKey, true);
                        keys.Add(key);
                    }
                    catch (Exception)
                    {}
                }
            }

            if(strCommand != null)
            {
                Command = strCommand;
            }
        }

        public ISet<Keys> getKeys()
        {
            return keys;
        }

        public string GetKeysAsString()
        {
            string strKeys = null;

            foreach(Keys key in keys)
            {
                if (strKeys == null)
                    strKeys = "";
                else
                    strKeys += "+";

                strKeys += key.ToString();
            }

            return strKeys;
        }

        public bool AddKey(Keys key)
        {
            bool result = keys.Add(key);
            Text = GetKeysAsString();
            return result;
        }

        public bool deleteKey(Keys key)
        {
            return keys.Remove(key);
        }
    }
}
