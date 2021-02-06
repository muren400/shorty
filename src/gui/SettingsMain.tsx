import { Tabs } from 'antd';
import React from 'react';
import ShortcutModel from '../logic/ShortcutModel';
import SettingsServer from './SettingsServer';
import SettingsShortcuts from './SettingsShortcuts';

const { TabPane } = Tabs;

export default class SettingsMain extends React.Component {
  shortcutModel: ShortcutModel;

  constructor(props: any) {
    super(props);

    this.shortcutModel = new ShortcutModel();
    window.addEventListener('configRead', () => {
      this.setState({});
    });
    this.shortcutModel.readConfig();
  }

  renderShortcutSettings = () => {
    return (
      <TabPane tab="Shortcuts" key="Shortcuts">
        <SettingsShortcuts shortcutModel={this.shortcutModel} />
      </TabPane>
    );
  };

  renderServerSettings = () => {
    const settingsServer = new SettingsServer({}, this.shortcutModel);
    return (
      <TabPane tab="Server" key="Server">
        {settingsServer.render()}
      </TabPane>
    );
  };

  render() {
    return (
      <Tabs type="card">
        {this.renderShortcutSettings()}
        {this.renderServerSettings()}
      </Tabs>
    );
  }
}
