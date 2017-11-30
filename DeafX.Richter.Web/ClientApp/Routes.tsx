import * as React from 'react';
import { Route, Switch } from 'react-router-dom';
import App from './Components/App';
import HomePage from './Components/Home/HomePage';
import ConfigurationPage from './Components/Configuration/ConfigurationPage';

export const routes = <App>
    <Switch>
        <Route exact path="/" component={HomePage} />
        <Route path="/config/:id" component={ConfigurationPage} />
    </Switch>
</App>;
