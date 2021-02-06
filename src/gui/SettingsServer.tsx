import { Form, InputNumber, Switch } from 'antd';
import { Content } from 'antd/lib/layout/layout';
import React from 'react';
import ShortcutModel from '../logic/ShortcutModel';
import styles from '../styles/SettingsServer.module.css';

export default class SettingsServer extends React.Component {
  shortcutModel: ShortcutModel;

  constructor(props: any, shortcutModel: ShortcutModel) {
    super(props);

    this.shortcutModel = shortcutModel;
  }

  renderServerForm = () => {
    return (
      <Form layout="horizontal" name="serverForm">
        <Form.Item label="Listen for Requests">
          <Switch
            defaultChecked={this.shortcutModel.server.activated}
            size="small"
            onChange={(value) => {
              this.shortcutModel.server.activated = Boolean(value);
            }}
          />
        </Form.Item>
        <Form.Item label="Port">
          <InputNumber
            placeholder="port"
            defaultValue={this.shortcutModel.server.port}
            onChange={(value) => {
              if (!value) {
                return;
              }

              this.shortcutModel.server.port = Number(value);
            }}
          />
        </Form.Item>
      </Form>
    );
  };

  render() {
    return (
      <Content style={{ padding: '0 24px' }}>
        <this.renderServerForm />
      </Content>
    );
  }
}
