import { ipcMain, ipcRenderer, app } from 'electron';
import fs from 'fs-extra';
import path from 'path';
import Shortcut from './Shortcut';

export default class ShortcutModel {
  shortcuts: Array<Shortcut>;

  server: {
    port: number;
    activated: boolean;
  };

  constructor() {
    this.shortcuts = new Array<Shortcut>();
    this.server = {
      port: 11337,
      activated: false,
    };

    this.initIpc();
  }

  initIpc() {
    if (ipcRenderer) {
      ipcRenderer.on('writeConfigRenderer', () => {
        this.writeConfigRenderer();
      });
    } else if (ipcMain) {
      ipcMain.on('writeConfigMain', (event, data) => {
        this.writeConfigMain(data);
        event.sender.send('writeConfigResponse', 'result');
      });

      ipcMain.on('readConfigMain', (event) => {
        event.sender.send('readConfigResponse', this.readConfigMain());
      });
    }
  }

  getConfigPath = () => {
    return path.join(app.getPath('userData'), 'config.json');
  };

  readConfig = () => {
    if (app) {
      return this.readConfigMain();
    }

    return this.readConfigRenderer();
  };

  readConfigMain() {
    const data = fs.readFileSync(this.getConfigPath());
    const config = JSON.parse(data.toString());
    if (!config.shortcuts) {
      return {
        shortcuts: this.shortcuts,
        server: this.server,
      };
    }
    console.log(config);

    this.shortcuts = config.shortcuts.map(
      (shortcut: {
        keys: { code: number; displayValue: string }[];
        name: string;
        command: string;
        request: string;
      }) => {
        const keys = new Map<number, string>();
        shortcut.keys.forEach((key: { code: number; displayValue: string }) => {
          if (!key.code || !key.displayValue) {
            keys.set(-1, '%');
          }

          keys.set(key.code, key.displayValue);
        });

        return new Shortcut({
          name: shortcut.name,
          command: shortcut.command,
          request: shortcut.request,
          keys,
        });
      }
    );

    this.server = config.server;

    return {
      shortcuts: this.shortcuts,
      server: this.server,
    };
  }

  readConfigRenderer() {
    ipcRenderer.once('readConfigResponse', (_event, response) => {
      // if (!response || !response.length) {
      //   return;
      // }

      this.shortcuts = response.shortcuts.map((shortcut: any) => {
        return new Shortcut(shortcut);
      });

      this.server = response.server;

      const configReadEvent = new CustomEvent('configRead');
      window.dispatchEvent(configReadEvent);
    });

    ipcRenderer.send('readConfigMain', '');
  }

  writeConfigMain(data: { shortcuts: Array<Shortcut>; server: any }) {
    this.shortcuts = data.shortcuts.map((shortcut) => {
      return new Shortcut(shortcut);
    });

    const config = {
      shortcuts: this.shortcuts.map((shortcut) => {
        return {
          name: shortcut.name,
          command: shortcut.command,
          request: shortcut.request,
          keys: this.convertKeysForConfig(shortcut.keys),
        };
      }),
      server: data.server,
    };

    fs.writeFileSync(this.getConfigPath(), JSON.stringify(config, null, 2));
  }

  convertKeysForConfig = (keys: Map<number, string>) => {
    const codes = Array.from(keys.keys());
    return codes.map((code) => {
      return {
        code,
        displayValue: keys.get(code),
      };
    });
  };

  writeConfigRenderer() {
    ipcRenderer.once('writeConfigResponse', () => {
      alert('Config written');
    });

    ipcRenderer.send('writeConfigMain', {
      shortcuts: this.shortcuts,
      server: this.server,
    });
  }

  getMatchingShortcut(keyCodes: Array<number>): Shortcut | null {
    let matchedShortcut = null;
    this.shortcuts.some((shortcut) => {
      if (shortcut.matches(keyCodes) === false) {
        return false;
      }

      matchedShortcut = shortcut;
      return true;
    });

    return matchedShortcut;
  }
}
