using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace shortcutManager
{
    class KeyStrokeHandler
    {
        public const int WM_HOTKEY = 0x312;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool GetMessage(ref Message lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern UIntPtr SetTimer(IntPtr hWnd, UIntPtr nIDEvent, uint uElapse, TimerProc lpTimerFunc);
        private delegate void TimerProc(IntPtr hWnd, uint uMsg, IntPtr nIDEvent, uint dwTime);

        private Thread keyStrokeListenerThread;
        private static bool kill = false;

        private enum KeyModifier
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            WinKey = 8
        }

        private List<Shortcut> shortcuts;
        private static Dictionary<int, Shortcut> registeredKeyStrokes;

        public KeyStrokeHandler()
        {
            registeredKeyStrokes = new Dictionary<int, Shortcut>();
        }

        public void SetKeyStrokes(List<Shortcut> shortcuts)
        {
            this.shortcuts = shortcuts;
            StartListening();
        }

        private void UnregisterKeyStrokes()
        {
            foreach (int id in registeredKeyStrokes.Keys)
            {
                UnregisterHotKey(IntPtr.Zero, id);
            }

            registeredKeyStrokes.Clear();
        }

        private void RegisterKeyStrokes()
        {
            UnregisterKeyStrokes();

            int id = 0;

            foreach (Shortcut shortcut in shortcuts)
            {
                Tuple<int, int> keys = GetKeys(shortcut);
                RegisterHotKey(IntPtr.Zero, id, keys.Item1, keys.Item2);
                registeredKeyStrokes.Add(id, shortcut);
                id++;
            }
        }

        private Tuple<int, int> GetKeys(Shortcut shortcut)
        {
            int iMod = (int)KeyModifier.None;
            int iKey = 0;

            foreach (Keys key in shortcut.getKeys())
            {
                switch (key)
                {
                    case Keys.Alt:
                        iMod |= (int)KeyModifier.Alt;
                        break;
                    case Keys.Control:
                    case Keys.ControlKey:
                    case Keys.RControlKey:
                    case Keys.LControlKey:
                        iMod |= (int)KeyModifier.Control;
                        break;
                    case Keys.Shift:
                    case Keys.ShiftKey:
                    case Keys.LShiftKey:
                    case Keys.RShiftKey:
                        iMod |= (int)KeyModifier.Shift;
                        break;
                    case Keys.LWin:
                    case Keys.RWin:
                        iMod |= (int)KeyModifier.WinKey;
                        break;
                    default:
                        iKey = key.GetHashCode();
                        break;
                }
            }

            return new Tuple<int, int>(iMod, iKey);
        }

        public void StartListening()
        {
            if (keyStrokeListenerThread != null)
            {
                keyStrokeListenerThread.Abort();
            }

            keyStrokeListenerThread = new Thread(RegisterAndListen);
            kill = false;
            keyStrokeListenerThread.Start();
        }

        private void RegisterAndListen()
        {
            RegisterKeyStrokes();

            SetTimer(IntPtr.Zero, UIntPtr.Zero, 2000, null);

            Message msg = new Message();
            while (GetMessage(ref msg, IntPtr.Zero, 0, 0))
            {
                if(kill)
                {
                    return;
                }

                if (msg.Msg != WM_HOTKEY)
                {
                    continue;
                }

                HandleKeyStroke((int)msg.WParam);
            }
        }

        private void HandleKeyStroke(int id)
        {
            if (registeredKeyStrokes.TryGetValue(id, out Shortcut shortcut) == false)
            {
                return;
            }

            shortcut.RunCommand();
        }

        public void StopService()
        {
            kill = true;
        }
    }
}
