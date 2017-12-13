import * as React from 'react';
import { Redirect, Router, Route, HistoryBase } from 'react-router';
import Layout from './components/Layout';
import Home from './views/Home';
import TeaDashboard from './views/tea/Dashboard';
import Teabags from './views/tea/Teabags';
import TeabagForm from './views/tea/TeabagForm';
import BrandForm from './viewComponents/tea/BrandForm';
import BagtypeForm from './viewComponents/tea/BagtypeForm';
import CountryForm from './viewComponents/CountryForm';

// http://stackoverflow.com/questions/32128978/react-router-no-not-found-route
// https://github.com/ReactTraining/react-router/issues/142
// https://reacttraining.com/react-router/web/example/recursive-paths
// https://css-tricks.com/learning-react-router/
// https://github.com/ReactTraining/react-router/issues/4105

export default <Route component={ Layout }>
    <Route path='/' components={{ body: Home }} />
    <Route path='/tea/dashboard' components={{ body: TeaDashboard }} />
    <Route path='/tea/teabags' components={{ body: Teabags }} />
    <Route path='/tea/teabagform' components={{ body: TeabagForm }}>
        <Route path='/tea/teabagform/:id' components={{ body: TeabagForm }} />
    </Route>
    <Route path='/tea/brandform' components={{ body: BrandForm }}>
        <Route path='/tea/brandform/:id' components={{ body: BrandForm }} />
    </Route>
    <Route path='/tea/bagtypeform' components={{ body: BagtypeForm }}>
        <Route path='/tea/bagtypeform/:id' components={{ body: BagtypeForm }} />
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
