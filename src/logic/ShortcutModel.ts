import { ipcMain, ipcRenderer } from 'electron';
import Shortcut from './Shortcut';
import fs from 'fs-extra';
import path from 'path';
import { app } from 'electron';

export default class ShortcutModel {
  shortcuts: Array<Shortcut>;

  constructor() {
    this.shortcuts = new Array();

    this.initIpc();
  }

  initIpc() {
    if (ipcRenderer) {
      ipcRenderer.on('writeConfigRenderer', (event, data) => {
        this.writeConfigRenderer();
      });
    } else if (ipcMain) {
      ipcMain.on('writeConfigMain', (event, data) => {
        this.writeConfigMain(data);
        event.sender.send('writeConfigResponse', 'result');
      });

      ipcMain.on('readConfigMain', (event, data) => {
        event.sender.send('readConfigResponse', this.readConfigMain());
      });
    }
  }

  getConfigPath() {
    return path.join(app.getPath('userData'), 'config.json');
  }

  readConfig() {
    if(app) {
      return this.readConfigMain();
    } else {
      return this.readConfigRenderer();
    }
  }

  readConfigMain() {
    let data = fs.readFileSync(this.getConfigPath());
    let config = JSON.parse(data.toString());
    if(!config.shortcuts) {
      return this.shortcuts;
    }
    console.log(config);

    this.shortcuts = config.shortcuts.map((shortcut) => {

      let keys = new Map<number, string>();
      shortcut.keys.forEach((key) => {
        if(!key.code || !key.displayValue) {
          keys.set(-1, "%");
        }

        keys.set(key.code, key.displayValue);
      });

      return new Shortcut({
        name: shortcut.name,
        command: shortcut.command,
        keys: keys
      });
    });

    return this.shortcuts;
  }

  readConfigRenderer() {
    ipcRenderer.once('readConfigResponse', (event, response) => {
      if(!response || !response.length) {
        return;
      }

      this.shortcuts = response.map((shortcut: any) => {
        return new Shortcut(shortcut);
      });

      let configReadEvent = new CustomEvent('configRead');
      window.dispatchEvent(configReadEvent);
    });

    ipcRenderer.send('readConfigMain', '');
  }

  writeConfigMain(shortcuts: Array<Shortcut>) {
    this.shortcuts = shortcuts.map((shortcut) => {
      return new Shortcut(shortcut);
    });

    let config = {
      shortcuts: this.shortcuts.map((shortcut) => {
        return {
          name: shortcut.name,
          command: shortcut.command,
          keys: this.convertKeysForConfig(shortcut.keys),
        };
      }),
    };

    fs.writeFileSync(this.getConfigPath(), JSON.stringify(config, null, 2));
  }

  convertKeysForConfig(keys: Map<number, string>) {
    let codes = Array.from(keys.keys());
    let _keys = codes.map((code) => {
      return {
        code: code,
        displayValue: keys.get(code),
      };
    });

    return _keys;
  }

  writeConfigRenderer() {
    ipcRenderer.once('writeConfigResponse', (event, response) => {
      alert('Config written');
    });

    ipcRenderer.send('writeConfigMain', this.shortcuts);
  }

  getMatchingShortcut(keyCodes: Array<number>): Shortcut | null {
    let matchedShortcut = null;
    this.shortcuts.some((shortcut) => {
      if(shortcut.matches(keyCodes) == false) {
        return false;
      }

      matchedShortcut = shortcut;
      return true;
    });

    return matchedShortcut;
  }
}
