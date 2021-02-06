import React from 'react';
// import { Titlebar, Color } from 'custom-electron-titlebar';
import { ipcRenderer } from 'electron';
import { Form, Input, Tabs } from 'antd';
import Shortcut from '../logic/Shortcut';
import ShortcutModel from '../logic/ShortcutModel';
import styles from '../styles/SettingsShortcuts.module.css';

const { TabPane } = Tabs;

type PropsType = {
  shortcutModel: ShortcutModel;
};

type StateType = unknown;

export default class SettingsShortcuts extends React.Component<
  PropsType,
  StateType
> {
  shortcutModel: ShortcutModel;

  shortcutKey: number;

  keyToShortcut: Map<number, Shortcut>;

  constructor(props: PropsType) {
    super(props);

    this.shortcutModel = props.shortcutModel;

    this.shortcutKey = 0;
    this.keyToShortcut = new Map();

    this.state = {};
  }

  getNextKey(): number {
    const key = this.shortcutKey;
    this.shortcutKey += 1;
    return key;
  }

  onEdit = (targetKey: string | unknown, action: string) => {
    if (typeof action !== 'string') {
      return;
    }

    switch (action) {
      case 'add':
        this.add();
        break;
      case 'remove':
        if (typeof targetKey !== 'string') {
          break;
        }

        this.remove(targetKey);
        break;
      default:
        break;
    }
  };

  add = () => {
    this.shortcutModel.shortcuts.push(new Shortcut({}));
    this.setState({});
  };

  remove = (target: string) => {
    if (typeof target !== 'string') {
      return;
    }

    const key = Number.parseInt(target, 10);

    const shortcut = this.keyToShortcut.get(key);
    if (shortcut == null) {
      return;
    }

    this.keyToShortcut.delete(key);
    const index = this.shortcutModel.shortcuts.indexOf(shortcut, 0);
    if (index > -1) {
      this.shortcutModel.shortcuts.splice(index, 1);
    }

    this.setState({});
  };

  renderCommandForm = (shortcut: Shortcut) => {
    const nameBoxRef = React.createRef<Input>();
    const requestBoxRef = React.createRef<Input>();
    const commandBoxRef = React.createRef<Input>();
    return (
      <Form layout="vertical" name="commandForm">
        <Form.Item label="Name">
          <Input
            ref={nameBoxRef}
            placeholder="Enter any command"
            defaultValue={shortcut.name}
            onChange={() => {
              const value = nameBoxRef.current?.input.value;

              if (!value) {
                return;
              }

              shortcut.name = value;
            }}
          />
        </Form.Item>
        <Form.Item label="Request">
          <Input
            ref={requestBoxRef}
            placeholder="Enter the request"
            defaultValue={shortcut.request}
            spellCheck="false"
            onChange={() => {
              const value = requestBoxRef.current?.input.value;

              if (!value) {
                return;
              }

              shortcut.request = value;
            }}
          />
        </Form.Item>
        <Form.Item label="Command">
          <Input
            ref={commandBoxRef}
            placeholder="Enter any command"
            defaultValue={shortcut.command}
            spellCheck="false"
            onChange={() => {
              const value = commandBoxRef.current?.input.value;

              if (!value) {
                return;
              }

              shortcut.command = value;
            }}
          />
        </Form.Item>
      </Form>
    );
  };

  renderKeystrokeBox = (shortcut: Shortcut) => {
    const keystrokeBoxRef = React.createRef<Input>();
    return (
      <Input
        ref={keystrokeBoxRef}
        placeholder="Press any keys"
        defaultValue={shortcut.getDisplayString()}
        spellCheck="false"
        onKeyDown={(e) => {
          e.preventDefault();

          ipcRenderer.once('getLastPressedKeyResponse', (_event, response) => {
            if (typeof response !== 'number') {
              return;
            }

            if (e.keyCode === 8 || e.keyCode === 46) {
              shortcut.removeLastKey();
            } else {
              shortcut.addKey(response, e.key);
            }

            keystrokeBoxRef.current?.setValue(shortcut.getDisplayString());
          });

          ipcRenderer.send('getLastPressedKey');
        }}
      />
    );
  };

  renderTab = (shortcut: Shortcut) => {
    const tab = this.renderKeystrokeBox(shortcut);
    const form = this.renderCommandForm(shortcut);
    const key = this.getNextKey();
    this.keyToShortcut.set(key, shortcut);
    return (
      <TabPane className={styles.CommandForm} tab={tab} key={key}>
        {form}
      </TabPane>
    );
  };

  renderTabs = () => {
    const items = this.shortcutModel.shortcuts.map((shortcut) => {
      return this.renderTab(shortcut);
    });

    return items;
  };

  renderSplitter = () => {
    const splitterRef = React.createRef<HTMLDivElement>();
    let dragging = false;

    document.addEventListener('mousemove', (event) => {
      if (!dragging || event.buttons < 1) {
        return;
      }

      if (!splitterRef.current) {
        return;
      }

      splitterRef.current.style.left = `${event.pageX}px`;
    });

    return (
      <div
        ref={splitterRef}
        role="none"
        className="splitter"
        onMouseDown={() => {
          dragging = true;
        }}
        onMouseUp={() => {
          dragging = false;
        }}
      />
    );
  };

  render() {
    const tabs = this.renderTabs();

    // TODO: make width changeable
    return (
      <Tabs
        onEdit={this.onEdit}
        className={styles.TabsContainer}
        tabPosition="left"
        type="editable-card"
      >
        {tabs}
      </Tabs>
    );
  }
}
