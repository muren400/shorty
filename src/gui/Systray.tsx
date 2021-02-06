import SysTray from 'systray';
import fs from 'fs-extra';
import path from 'path';
import { app } from 'electron';
import ShortcutModel from '../logic/ShortcutModel';
import SettingsWindow from './SettingsWindow';
import Shortcut from '../logic/Shortcut';

const RESOURCES_PATH = app.isPackaged
  ? path.join(process.resourcesPath, 'assets')
  : path.join(__dirname, '../../assets');

export default class Systray {
  systray: SysTray;

  shortcutModel: ShortcutModel;

  settingsWindow: SettingsWindow;

  constructor(shortcutModel: ShortcutModel) {
    this.shortcutModel = shortcutModel;
    this.settingsWindow = new SettingsWindow(this.shortcutModel);

    let iconPath = path.join(
      RESOURCES_PATH,
      `icon.${process.platform === 'win32' ? 'ico' : 'png'}`
    );

    if (process.env.NODE_ENV === 'development') {
      iconPath = path.join(
        RESOURCES_PATH,
        `icon_debug.${process.platform === 'win32' ? 'ico' : 'png'}`
      );
    }

    const items = [];

    const idToItem = new Map<number, Shortcut | string>();

    let id = 0;

    this.shortcutModel.shortcuts.forEach((shortcut) => {
      items.push({
        title: `${shortcut.name} (${shortcut.getDisplayString()})`,
        tooltip: shortcut.command,
        // checked is implement by plain text in linux
        checked: false,
        enabled: true,
      });

      idToItem.set(id++, shortcut);
    });

    items.push({
      title: '-',
      tooltip: '',
      // checked is implement by plain text in linux
      checked: false,
      enabled: false,
    });

    id++;

    items.push({
      title: 'Settings',
      tooltip: 'Settings',
      // checked is implement by plain text in linux
      checked: false,
      enabled: true,
    });

    idToItem.set(id++, 'Settings');

    items.push({
      title: 'Exit',
      tooltip: 'Quit Shorty',
      // checked is implement by plain text in linux
      checked: false,
      enabled: true,
    });

    idToItem.set(id++, 'Exit');

    const icon = fs.readFileSync(iconPath);
    this.systray = new SysTray({
      menu: {
        // you should using .png icon in macOS/Linux, but .ico format in windows
        icon: icon.toString('base64'),
        title: 'Shorty',
        tooltip: 'Settings',
        items: items,
      },
      debug: false,
      copyDir: true, // copy go tray binary to outside directory, useful for packing tool like pkg.
    });

    this.systray.onClick((action) => {
      if (idToItem.get(action.seq_id) === 'Settings') {
        // systray.sendAction({
        //     type: 'update-item',
        //     item: {
        //     ...action.item,
        //     checked: !action.item.checked,
        //     },
        //     seq_id: action.seq_id,
        // })
        this.onSettingsClick();
      } else if (idToItem.get(action.seq_id) === 'Exit') {
        this.onCloseClick();
      } else {
        const shortcut = idToItem.get(action.seq_id);
        if (!shortcut || typeof shortcut === 'string') {
          return;
        }

        shortcut.executeCommand();
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
