import * as React from 'react';

import './Loader.css';

// other options:
// https://projects.lukehaas.me/css-loaders/
// https://codepen.io/martinvd/pen/xbQJom
export const Loader : React.StatelessComponent<{}> = props =>
    <div style={{position: 'absolute', height: '100vh', width: '100%', backgroundColor: 'rgba(255, 255, 255, 0.7)', zIndex: 5}} onClick={event => event.stopPropagation()}><div className="loader"><div className="inner one" /><div className="inner two" /><div className="inner three" /></div></div>;
