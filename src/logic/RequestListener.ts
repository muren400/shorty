import http from 'http';
import ShortcutModel from './ShortcutModel';

export default class RequestListener {
  shortcutModel: ShortcutModel;

  constructor(shortcutModel: ShortcutModel) {
    this.shortcutModel = shortcutModel;
  }

  startListening() {
    if (this.shortcutModel.server.activated === false) {
      return;
    }

    const server = http.createServer((req, res) => {
      const requestCommand = req.url?.substring(1);
      if (!requestCommand) {
        return;
      }

      let success = false;

      this.shortcutModel.shortcuts.some((shortcut) => {
        if (shortcut.request !== requestCommand) {
          return false;
        }

        shortcut.executeCommand();

        success = true;

        return true;
      });

      res.writeHead(200);
      res.end(success ? 'done' : 'no command found');
    });

    server.listen(this.shortcutModel.server.port, '0.0.0.0', () => {
      console.log(`Server is running`);
    });
  }
}
