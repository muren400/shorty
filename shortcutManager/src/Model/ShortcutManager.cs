using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace shortcutManager
{
    public class ShortcutManager : INotifyPropertyChanged
    {
        public static String SHORTCUTS_PROPERTY_CHANGED = "SHORTCUTS_PROPERTY_CHANGED";

        private const string CONFIG_FILE_NAME = "shortcuts.json";
        private List<Shortcut> shortcuts;
        private KeyStrokeHandler keyStrokeHandler;


        public ShortcutManager()
        {
            ReloadConfig();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public List<Shortcut> GetShortcuts()
        {
            return shortcuts;
        }

        public void ReloadConfig()
        {
            ReadShortcuts();
            keyStrokeHandler = new KeyStrokeHandler();
            keyStrokeHandler.SetKeyStrokes(shortcuts);
            keyStrokeHandler.StartListening();

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(SHORTCUTS_PROPERTY_CHANGED));
        }

        public void WriteShortcuts()
        {
            JObject config = new JObject();
            JArray jsonShortcuts = new JArray();

            foreach(Shortcut shortcut in shortcuts)
            {
                JObject jsonShortcut = new JObject();
                jsonShortcut.Add("keybinding", shortcut.GetKeysAsString());
                jsonShortcut.Add("command", shortcut.Command);
                jsonShortcuts.Add(jsonShortcut);
            }

            config.Add("shortcuts", jsonShortcuts);
            config.ToString();

            using (StreamWriter file = File.CreateText(GetConfigPath()))
            using (JsonTextWriter writer = new JsonTextWriter(file))
            {
                writer.Formatting = Formatting.Indented;

                config.WriteTo(writer);
            }

            ReloadConfig();
        }

        private void ReadShortcuts()
        {
            shortcuts = new List<Shortcut>();

            var appDataDir = Directory.GetParent(Application.LocalUserAppDataPath);

            string jsonPath = GetConfigPath();

            string strJson = File.ReadAllText(jsonPath);
            var jsonShortcutConfig = JObject.Parse(strJson);

            List<JObject> jsonShortcuts = jsonShortcutConfig.SelectToken("$.shortcuts").Values<JObject>().ToList();

            if (jsonShortcuts == null)
                return;

            foreach (JObject jsonShortcut in jsonShortcuts)
            {
                string keybinding = jsonShortcut.SelectToken("$.keybinding").Value<string>().ToString();
                string command = jsonShortcut.SelectToken("$.command").Value<string>().ToString();

                shortcuts.Add(new Shortcut(keybinding, command));
            }
        }

        private String GetConfigPath()
        {
            var appDataDir = Directory.GetParent(Application.LocalUserAppDataPath);

            string configPath = appDataDir.FullName;

            if (Directory.Exists(configPath) == false)
                Directory.CreateDirectory(configPath);

            configPath += Path.DirectorySeparatorChar + CONFIG_FILE_NAME;

            if (File.Exists(configPath) == false)
                File.Create(configPath);

            return configPath;
        }
    }
}
