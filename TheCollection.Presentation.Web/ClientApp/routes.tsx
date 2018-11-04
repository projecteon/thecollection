import * as React from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
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
// https://stackoverflow.com/questions/37696391/multiple-params-with-react-router

export const routes = <Layout>
    <Route exact path='/' component={ Home } />
    <Route exact path='/tea/dashboard' component={ TeaDashboard } />
    <Route exact path='/tea/teabags' component={ Teabags } />
    <Route exact path='/tea/teabagform/' component={ TeabagForm } />
    <Route exact path='/tea/teabagform/:id' component={ TeabagForm } />
    <Route exact path='/tea/brandform/' component={ BrandForm } />
    <Route exact path='/tea/brandform/:id' component={ BrandForm } />
    <Route exact path='/tea/bagtypeform/:id' component={ BagtypeForm } />
    <Route exact path='/countryform/:id' component={ CountryForm } />
    {/* <Route path='Account/LogOff'><Redirect to='/Account/LogOff'/></Route> */}
</Layout>;
