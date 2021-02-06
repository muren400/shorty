import React from 'react';
import { BrowserRouter as Router, Switch, Route } from 'react-router-dom';
import SettingsMain from './gui/SettingsMain';

export default function App() {
  window.addEventListener('DOMContentLoaded', () => {
    const customTitlebar = require('custom-electron-titlebar');
    const MyTitleBar = new customTitlebar.Titlebar({
      backgroundColor: customTitlebar.Color.fromHex('#29292e'),
      icon: './res/images/icon.png',
    });
    MyTitleBar.updateTitle('shorty - Settings');
  });

  return (
    <Router>
      <Switch>
        <Route path="/" component={SettingsMain} />
      </Switch>
    </Router>
  );
}
