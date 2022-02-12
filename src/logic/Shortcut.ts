import { exec } from 'child_process';

export default class Shortcut {
  name: string;

  keys: Map<number, string>;

  command: string;

  request: string;

  constructor(config: any) {
    this.name = config.name ? config.name : '';
    this.keys = config.keys ? config.keys : new Map();
    this.command = config.command ? config.command : '';
    this.request = config.request ? config.request : '';
  }

  addKey(keyCode: number, displayValue: string) {
    this.keys.set(keyCode, displayValue);
  }

  removeLastKey() {
    const lastCode = Array.from(this.keys.keys()).pop();

    if (lastCode != null) {
      this.keys.delete(lastCode);
    }
  }

  getDisplayString() {
    return Array.from(this.keys.values()).join('+');
  }

  matches(keyCodes: Array<number>) {
    if (this.keys.size === 0) {
      return false;
    }

    let count = 0;

    keyCodes.forEach((keyCode) => {
      if (this.keys.get(keyCode) != null) {
        count += 1;
      }
    });

    return count === this.keys.size;
  }

  executeCommand() {
    exec(this.command, (error, stdout, stderr) => {
      if (error) {
        console.log(`error: ${error.message}`);
        return;
      }
      if (stderr) {
        console.log(`stderr: ${stderr}`);
        return;
      }
      console.log(`stdout: ${stdout}`);
    });
  }
}
