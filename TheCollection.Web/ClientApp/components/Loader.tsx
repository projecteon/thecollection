import * as React from 'react';

import './Loader.scss';

// other options:
// https://projects.lukehaas.me/css-loaders/
// https://codepen.io/martinvd/pen/xbQJom
const Loader : React.StatelessComponent<{isInternalLoader?: boolean}> = props => {
    const style = (): React.CSSProperties => {
      let componentStyle: React.CSSProperties = {backgroundColor: 'rgba(255, 255, 255, 0.7)', zIndex: 5, width: '100%'};
      if (props.isInternalLoader === true) {
        return {...style, ...{position: 'relative', minHeight: 200, height: '100%', display: 'flex', alignItems: 'center'}};
      }

      return {...style, ...{position: 'absolute', height: '100vh'}};
    };

    return  <div style={style()} onClick={event => event.stopPropagation()}>
              <div className="loader">
                <div className="inner one" />
                <div className="inner two" />
                <div className="inner three" />
              </div>
            </div>;
}

export default Loader;
