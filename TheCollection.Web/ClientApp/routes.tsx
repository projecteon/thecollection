import * as React from 'react';
import { Redirect, Router, Route, HistoryBase } from 'react-router';
import Layout from './components/Layout';
import Home from './views/Home';
import TeaDashboard from './views/tea/Dashboard';
import Teabags from './views/Teabags';
import TeabagForm from './views/TeabagForm';
import BrandForm from './viewComponents/BrandForm';
import BagtypeForm from './viewComponents/BagtypeForm';
import CountryForm from './viewComponents/CountryForm';

// http://stackoverflow.com/questions/32128978/react-router-no-not-found-route
// https://github.com/ReactTraining/react-router/issues/142
// https://reacttraining.com/react-router/web/example/recursive-paths
// https://css-tricks.com/learning-react-router/
// https://github.com/ReactTraining/react-router/issues/4105

export default <Route component={ Layout }>
    <Route path='/' components={{ body: Home }} />
    <Route path='/tea/dashboard' components={{ body: TeaDashboard }} />
    <Route path='/teabags' components={{ body: Teabags }} />
    <Route path='/teabagform' components={{ body: TeabagForm }}>
        <Route path='/teabagform/:id' components={{ body: TeabagForm }} />
    </Route>
    <Route path='/brandform' components={{ body: BrandForm }}>
        <Route path='/brandform/:id' components={{ body: BrandForm }} />
    </Route>
    <Route path='/bagtypeform' components={{ body: BagtypeForm }}>
        <Route path='/bagtypeform/:id' components={{ body: BagtypeForm }} />
    </Route>
    <Route path='/countryform' components={{ body: CountryForm }}>
        <Route path='/countryform/:id' components={{ body: CountryForm }} />
    </Route>
    {/* <Route path='Account/LogOff'><Redirect to='/Account/LogOff'/></Route> */}
</Route>;

// enable Hot Module Replacement (HMR)
if (module.hot) {
    module.hot.accept();
}
