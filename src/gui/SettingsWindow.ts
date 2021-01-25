import { app, BrowserWindow, shell } from "electron";
import ShortcutModel from "../logic/ShortcutModel";
import MenuBuilder from "../menu";
import path from 'path';

export default class SettingsWindow {
  window: BrowserWindow | null;
  shortcutModel: ShortcutModel;

  constructor(shortcutModel: ShortcutModel) {
    this.shortcutModel = shortcutModel;
    this.window = null;

    if (process.env.NODE_ENV === 'production') {
      const sourceMapSupport = require('source-map-support');
      sourceMapSupport.install();
    }

    if (
      process.env.NODE_ENV === 'development' ||
      process.env.DEBUG_PROD === 'true'
    ) {
      require('electron-debug')();
    }
  }

  async installExtensions() {
    const installer = require('electron-devtools-installer');
    const forceDownload = !!process.env.UPGRADE_EXTENSIONS;
    const extensions = ['REACT_DEVELOPER_TOOLS'];

    return installer
      .default(
        extensions.map((name) => installer[name]),
        forceDownload
      )
      .catch(console.log);
  }

  async createWindow() {
    if(this.window != null) {
      return;
    }

    if (
      process.env.NODE_ENV === 'development' ||
      process.env.DEBUG_PROD === 'true'
    ) {
      await this.installExtensions();
    }

    const RESOURCES_PATH = app.isPackaged
      ? path.join(process.resourcesPath, 'resources')
      : path.join(__dirname, '../../resources');

    const getAssetPath = (...paths: string[]): string => {
      return path.join(RESOURCES_PATH, ...paths);
    };

    this.window = new BrowserWindow({
      show: false,
      width: 1024,
      height: 728,
      icon: getAssetPath('icon.png'),
      webPreferences: {
        nodeIntegration: true,
      },
    });

    this.window.loadFile('index.html');

    this.window.once('ready-to-show', () => {
      if (!this.window) {
        throw new Error('"mainWindow" is not defined');
      }

      this.window.show();
    });

    this.window.on('closed', () => {
      this.window = null;
    });

    const menuBuilder = new MenuBuilder(this.window, this.shortcutModel);
    menuBuilder.buildMenu();

    // // Open urls in the user's browser
    // this.window.webContents.on('new-window', (event, url) => {
    //   event.preventDefault();
    //   shell.openExternal(url);
    // });

    // Remove this if your app does not use auto updates
    // eslint-disable-next-line
    // new AppUpdater();
  };
}
