import ioHook from 'iohook';
import { BrowserWindow, ipcMain } from 'electron';
import ShortcutModel from './ShortcutModel';

export default class KeystrokeHandler {
  private pressedKeys: Set<number>;
  private lastKey: number;
  private registeredKeys: Set<number>;
  private released: boolean;
  private shortcutModel: ShortcutModel;

  public constructor(shortcutModel: ShortcutModel) {
    this.shortcutModel = shortcutModel;
    this.pressedKeys = new Set();
    this.registeredKeys = new Set();
    this.released = false;
    this.lastKey = -1;

    ipcMain.on('getLastPressedKey', (event) => {
      event.sender.send('getLastPressedKeyResponse', this.lastKey);
    });
  }

  startListening() {
    ioHook.on('keydown', (event) => {
      if (this.released) {
        return;
      }

      this.lastKey = event.keycode;
      this.pressedKeys.add(event.keycode);
    });

    ioHook.on('keyup', (event) => {
      // console.log(event.keycode + " - " + String.fromCharCode(event.rawcode));

      if (this.pressedKeys.delete(event.keycode) == false) {
        return;
      }

      if (this.isSetup()) {
        this.registeredKeys.clear();
        return;
      }

      this.registeredKeys.add(event.keycode);

      if (this.pressedKeys.size == 0) {
        console.log(this.registeredKeys);

        let shortcut = this.shortcutModel.getMatchingShortcut(Array.from(this.registeredKeys));
        this.registeredKeys.clear();

        if(shortcut == null) {
          return;
        }

        shortcut.executeCommand();

        // if(this.registeredKeys.has(3613) && this.registeredKeys.has(40)) {
        //     exec("python c:/Users/erzae/Nextcloud/python/lazyRemote/lazyRemoteOn.py", (error, stdout, stderr) => {
        //         if (error) {
        //             console.log(`error: ${error.message}`);
        //             return;
        //         }
        //         if (stderr) {
        //             console.log(`stderr: ${stderr}`);
        //             return;
        //         }
        //         console.log(`stdout: ${stdout}`);
        //     });
        // }

        // if(this.registeredKeys.has(3613) && this.registeredKeys.has(41)) {
        //     exec("python c:/Users/erzae/Nextcloud/python/lazyRemote/lazyRemoteOff.py", (error, stdout, stderr) => {
        //         if (error) {
        //             console.log(`error: ${error.message}`);
        //             return;
        //         }
        //         if (stderr) {
        //             console.log(`stderr: ${stderr}`);
        //             return;
        //         }
        //         console.log(`stdout: ${stdout}`);
        //     });
        // }
      }
    });
    ioHook.start();
  }

  isSetup() {
    return (
      BrowserWindow.getAllWindows().filter((b) => {
        return b.isVisible();
      }).length > 0
    );
  }
}
