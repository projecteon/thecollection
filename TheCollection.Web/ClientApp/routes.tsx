import * as React from 'react';
import { Router, Route, HistoryBase } from 'react-router';
import { Layout } from './components/Layout';
import Home from './components/Home';
import Teabags from './components/Teabags';
import TeabagForm from './components/TeabagForm';

// http://stackoverflow.com/questions/32128978/react-router-no-not-found-route
// https://github.com/ReactTraining/react-router/issues/142
// https://reacttraining.com/react-router/web/example/recursive-paths
// https://css-tricks.com/learning-react-router/
// https://github.com/ReactTraining/react-router/issues/4105

export default <Route component={ Layout }>
    <Route path='/' components={{ body: Home }} />
    <Route path='/teabags' components={{ body: Teabags }} />
    <Route path='/teabagform' components={{ body: TeabagForm }}>
        <Route path='/teabagform/:id' components={{ body: TeabagForm }} />
    </Route>
</Route>;

// enable Hot Module Replacement (HMR)
if (module.hot) {
    module.hot.accept();
}
