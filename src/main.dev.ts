/* eslint global-require: off, no-console: off */

/**
 * This module executes inside of electron's main process. You can start
 * electron renderer process from here and communicate with the other processes
 * through IPC.
 *
 * When running `yarn build` or `yarn build-main`, this file is compiled to
 * `./src/main.prod.js` using webpack. This gives us some performance wins.
 */
import 'core-js/stable';
import 'regenerator-runtime/runtime';
import path from 'path';
import { app } from 'electron';
import { autoUpdater } from 'electron-updater';
import log from 'electron-log';
import KeystrokeHandler from './logic/KeystrokeHandler';
import ShortcutModel from './logic/ShortcutModel';
import Systray from './gui/Systray';

export default class AppUpdater {
  constructor() {
    log.transports.file.level = 'info';
    autoUpdater.logger = log;
    autoUpdater.checkForUpdatesAndNotify();
  }
}

if (
  process.env.NODE_ENV === 'development' ||
  process.env.DEBUG_PROD === 'true'
) {
  const {build} = require('../package.json')
  const appData = app.getPath('appData')
  app.setPath('userData', path.join(appData, build.productName))
}

let shortcutModel = new ShortcutModel();
shortcutModel.readConfigMain();

let systray = new Systray(shortcutModel);

try {
  let keystrokeHandler = new KeystrokeHandler(shortcutModel);
  keystrokeHandler.startListening();
} catch (error) {
  console.log(error);
}

/**
 * Add event listeners...
 */

app.on('window-all-closed', () => {
  // Respect the OSX convention of having the application in memory even
  // after all windows have been closed
  // if (process.platform !== 'darwin') {
  //   app.quit();
  // }
});

// app.whenReady().then(createWindow).catch(console.log);

app.on('activate', () => {
  // On macOS it's common to re-create a window in the app when the
  // dock icon is clicked and there are no other windows open.
  // if (mainWindow === null) createWindow();
});
