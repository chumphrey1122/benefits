import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';

import './custom.css'
import { Employees } from './components/Employees';
import { EmployeeAndDependents } from './components/EmployeeAndDependents';

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
            <Route path='/employees' render={({ history }) => <Employees history={history} />} />
            <Route path='/employee/:id' render={({ match }) => <EmployeeAndDependents match={match} />} />
      </Layout>
    );
  }
}
