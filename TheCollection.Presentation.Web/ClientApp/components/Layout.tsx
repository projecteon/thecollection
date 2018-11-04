import React from 'react';
import NavMenu from './NavMenu';

// tslint:disable-next-line:variable-name
export const Layout: React.StatelessComponent<{}> = props => {
  return  <div>
            <div className='row' style={{position: 'relative', margin: 0}}>
                <div className='col-sm-12 col-md-3' style={{zIndex: 5, position: 'sticky', top: 0, padding: 0}}>
                    <NavMenu />
                </div>
                <div className='col-sm-12 col-md-9'>
                    { props.children }
                </div>
            </div>
          </div>;
};
