import SysTray from 'systray';
import fs from 'fs-extra';
import path from 'path';
import ShortcutModel from '../logic/ShortcutModel';
import SettingsWindow from './SettingsWindow';

export default class Systray {
  systray: SysTray;
  shortcutModel: ShortcutModel;
  settingsWindow: SettingsWindow;

  constructor(shortcutModel: ShortcutModel) {
    this.shortcutModel = shortcutModel;
    this.settingsWindow = new SettingsWindow(this.shortcutModel);

    let iconPath = path.join(
      __dirname,
      `../../assets/icon.${process.platform === 'win32' ? 'ico' : 'png'}`
    );
    iconPath = path.join(
      __dirname,
      `../../assets/icon_debug.${process.platform === 'win32' ? 'ico' : 'png'}`
    );

    let icon = fs.readFileSync(iconPath);
    this.systray = new SysTray({
      menu: {
        // you should using .png icon in macOS/Linux, but .ico format in windows
        icon: icon.toString('base64'),
        title: 'Shorty',
        tooltip: 'Settings',
        items: [
          {
            title: 'Settings',
            tooltip: 'Settings',
            // checked is implement by plain text in linux
            checked: false,
            enabled: true,
          },
          {
            title: 'Exit',
            tooltip: 'bb',
            checked: false,
            enabled: true,
          },
        ],
      },
      debug: false,
      copyDir: true, // copy go tray binary to outside directory, useful for packing tool like pkg.
    });

    this.systray.onClick((action) => {
      if (action.seq_id === 0) {
        // systray.sendAction({
        //     type: 'update-item',
        //     item: {
        //     ...action.item,
        //     checked: !action.item.checked,
        //     },
        //     seq_id: action.seq_id,
        // })
        this.onSettingsClick();
      } else if (action.seq_id === 1) {
        this.onCloseClick();
      }
    });
  }

  onSettingsClick() {
    this.settingsWindow.createWindow();
  }

  onCloseClick() {
    this.systray.kill();
  }
}
