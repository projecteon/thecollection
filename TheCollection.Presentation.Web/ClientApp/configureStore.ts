import { createStore, applyMiddleware, compose, combineReducers, StoreEnhancer, Store, StoreEnhancerStoreCreator, ReducersMapObject } from 'redux';
import thunk from 'redux-thunk';
import { routerReducer, routerMiddleware } from 'react-router-redux';
import * as StoreModule from './store';
import { IApplicationState, reducers } from './store';
import { History } from 'history';

export default function configureStore(history: History, initialState?: IApplicationState) {
  // build middleware. These are functions that can process the actions before they reach the store.
  const windowIfDefined = typeof window === 'undefined' ? undefined : window as any;
  // if devTools is installed, connect to it
  const devToolsExtension = windowIfDefined && windowIfDefined.__REDUX_DEVTOOLS_EXTENSION__ as () => StoreEnhancer<any>;
  const createStoreWithMiddleware = compose<StoreEnhancerStoreCreator<IApplicationState>>(
      applyMiddleware(thunk, routerMiddleware(history)),
      devToolsExtension ? devToolsExtension() : <S>(next: StoreEnhancerStoreCreator<S>) => next,
  )(createStore);

  // combine all reducers and instantiate the app-wide store instance
  const allReducers = buildRootReducer(reducers as any);
  const store = createStoreWithMiddleware(allReducers, initialState) as Store<IApplicationState>;

  // enable Webpack hot module replacement for reducers
  if (module.hot) {
    module.hot.accept('./store', () => {
      // tslint:disable-next-line:no-require-imports
      const nextRootReducer = require<typeof StoreModule>('./store');
      store.replaceReducer(buildRootReducer(nextRootReducer.reducers as any));
    });
  }

  return store;
}

function buildRootReducer(allReducers: ReducersMapObject) {
  return combineReducers<IApplicationState>(Object.assign({}, allReducers, { routing: routerReducer }) as any);
}
