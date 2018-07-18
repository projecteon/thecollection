import './css/site.css';
import 'bootstrap';
import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { AppContainer } from 'react-hot-loader';
import { Provider } from 'react-redux';
import { ConnectedRouter } from 'react-router-redux';
import { createBrowserHistory } from 'history';
import * as RoutesModule from './routes';
import configureStore from './configureStore';
import { IApplicationState }  from './store';


let routes = RoutesModule.routes;


// create browser history to use in the Redux store
const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href')!;
const history = createBrowserHistory({ basename: baseUrl });

// get the application-wide store instance, prepopulating with state from the server where available.
const initialState = (window as any).initialReduxState as IApplicationState;
const store = configureStore(history, initialState);

// this code starts up the React app when it runs in a browser. It sets up the routing configuration
// and injects the app into a DOM element.
function renderApp() {
  // this code starts up the React app when it runs in a browser. It sets up the routing configuration
  // and injects the app into a DOM element.
  ReactDOM.render(
      <AppContainer>
          <Provider store={ store }>
              <ConnectedRouter history={ history } children={ routes } />
          </Provider>
      </AppContainer>,
      document.getElementById('react-app'),
  );
}

renderApp();

// allow Hot Module Replacement
if (module.hot) {
    module.hot.accept('./routes', () => {
        // tslint:disable-next-line:no-require-imports
        routes = require<typeof RoutesModule>('./routes').routes;
        renderApp();
    });
}
