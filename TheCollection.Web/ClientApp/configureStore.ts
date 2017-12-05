import { createStore, applyMiddleware, compose, combineReducers, GenericStoreEnhancer } from 'redux';
import thunk from 'redux-thunk';
import { browserHistory } from 'react-router';
import { routerReducer, routerMiddleware } from 'react-router-redux';
import { createHistory } from 'history';
import * as Store from './store';

export default function configureStore(initialState?: Store.IApplicationState) {
  // create a history of your choosing (we're using a browser history in this case)
  // const history = createHistory();

  // build middleware. These are functions that can process the actions before they reach the store.
  const windowIfDefined = typeof window === 'undefined' ? null : window as any;
  // if devTools is installed, connect to it
  const devToolsExtension = windowIfDefined && windowIfDefined.devToolsExtension as () => GenericStoreEnhancer;
  const createStoreWithMiddleware = compose(
    applyMiddleware(thunk),
    applyMiddleware(thunk, routerMiddleware(browserHistory)),
    devToolsExtension ? devToolsExtension() : f => f,
  )(createStore);

  // combine all reducers and instantiate the app-wide store instance
  const allReducers = buildRootReducer(Store.reducers);
  const store = createStoreWithMiddleware(allReducers, initialState) as Redux.Store<Store.IApplicationState>;

  // enable Webpack hot module replacement for reducers
  if (module.hot) {
    module.hot.accept('./store', () => {
      const nextRootReducer = require<typeof Store>('./store');
      store.replaceReducer(buildRootReducer(nextRootReducer.reducers));
    });
  }

  return store;
}

function buildRootReducer(allReducers) {
  return combineReducers<Store.IApplicationState>(Object.assign({}, allReducers, { routing: routerReducer }));
}
