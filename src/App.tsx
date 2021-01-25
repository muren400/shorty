import React from 'react';
import { BrowserRouter as Router, Switch, Route } from 'react-router-dom';
import SettingsShortcuts from './gui/SettingsShortcuts';

export default function App() {
  return (
    <Router>
      <Switch>
        <Route path="/" component={SettingsShortcuts} />
      </Switch>
    </Router>
  );
}
