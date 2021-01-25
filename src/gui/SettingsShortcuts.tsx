import React from 'react';
import Shortcut from '../logic/Shortcut';
import ShortcutModel from '../logic/ShortcutModel';
import { Form, Input, Tabs } from 'antd';
import { ipcRenderer } from 'electron';
import styles from '../styles/SettingsShortcuts.module.css';

const { TabPane } = Tabs;

export default class SettingsShortcuts extends React.Component {
  shortcutModel: ShortcutModel;
  shortcutKey: number;
  keyToShortcut: Map<number, Shortcut>

  constructor(props: {}) {
    super(props);

    this.shortcutModel = new ShortcutModel();
    window.addEventListener('configRead', (event) => {
      this.setState({
        shortcut: null
      });
    });
    this.shortcutModel.readConfig();

    this.shortcutKey = 0;
    this.keyToShortcut = new Map();

    this.state = {
      shortcut: null,
    };

    this.renderCommandForm = this.renderCommandForm.bind(this);
    this.renderTab = this.renderTab.bind(this);
    this.renderTabs = this.renderTabs.bind(this);
  }

  renderCommandForm(shortcut: Shortcut) {
    let nameBoxRef = React.createRef<Input>();
    let commandBoxRef = React.createRef<Input>();
    return (
      <Form
        layout="vertical"
        name="commandForm"
      >
        <Form.Item
          label="Name"
        >
          <Input
            ref={nameBoxRef}
            placeholder="Enter any command"
            defaultValue={shortcut.name}
            onChange={(e) => {
              let value = nameBoxRef.current?.input.value;

              if(!value) {
                return;
              }

              shortcut.name = value;
            }}>
          </Input>
        </Form.Item>
        <Form.Item
          label="Command"
        >
          <Input
            ref={commandBoxRef}
            placeholder="Enter any command"
            defaultValue={shortcut.command}
            onChange={(e) => {
              let value = commandBoxRef.current?.input.value;

              if(!value) {
                return;
              }

              shortcut.command = value;
            }}>
          </Input>
        </Form.Item>
      </Form>
    );
  }

  renderKeystrokeBox(shortcut: Shortcut) {
    let keystrokeBoxRef = React.createRef<Input>();
    return (
      <Input
        ref={keystrokeBoxRef}
        placeholder="Press any keys"
        defaultValue={shortcut.getDisplayString()}
        onKeyDown={(e) => {
          e.preventDefault();

          ipcRenderer.once('getLastPressedKeyResponse', (event, response) => {
            if(typeof response != 'number') {
              return;
            }

            if(e.keyCode == 8 || e.keyCode == 46) {
              shortcut.removeLastKey();
            } else {
              shortcut.addKey(response, e.key);
            }

            keystrokeBoxRef.current?.setValue(shortcut.getDisplayString());
          });

          ipcRenderer.send('getLastPressedKey');
        }}>

        </Input>
    );
  }

  renderTab(shortcut: Shortcut) {
    let tab = this.renderKeystrokeBox(shortcut);
    let form = this.renderCommandForm(shortcut);
    this.keyToShortcut.set(this.shortcutKey, shortcut);
    return (
        <TabPane className={styles.CommandForm} tab={tab} key={this.shortcutKey++}>
            {form}
        </TabPane>
    );
  }

  onEdit = (targetKey: any, action: string) => {
    switch (action) {
      case 'add':
        this.add();
        break;
      case 'remove':
        this.remove(targetKey);
        break;
      default:
        break;
    }
  };

  add = () => {
    this.shortcutModel.shortcuts.push(new Shortcut({}));
    this.setState({
      shortcut: null
    });
  }

  remove = (target: any) => {
    if(typeof target != 'string') {
      return;
    }

    let key = Number.parseInt(target);

    let shortcut = this.keyToShortcut.get(key);
    if(shortcut == null) {
      return;
    }

    this.keyToShortcut.delete(key);
    const index = this.shortcutModel.shortcuts.indexOf(shortcut, 0);
    if (index > -1) {
      this.shortcutModel.shortcuts.splice(index, 1);
    }

    this.setState({
      shortcut: null
    });
  }

  renderTabs() {
    let items = this.shortcutModel.shortcuts.map(shortcut => {
      return this.renderTab(shortcut);
    });

    return items;
  }

  render() {
    let tabs = this.renderTabs();

    // TODO: make width changeable
    return (
      <Tabs
        onTabClick={(e) => {

        }}
        onEdit={this.onEdit}
        className={styles.TabsContainer}
        tabPosition="left"
        type="editable-card">
        {tabs}
      </Tabs>
    );
  }
}
