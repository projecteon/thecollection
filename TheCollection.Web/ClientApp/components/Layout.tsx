import * as React from 'react';
import NavMenu from './NavMenu';

type LayoutProps = {
    body: React.ReactElement<any>;
}

const Layout: React.StatelessComponent<LayoutProps> = props => {
  return  <div>
            <div className='row' style={{position: 'relative'}}>
                <div className='col-sm-12 col-md-3' style={{zIndex: 5, position: 'sticky', top: 0}}>
                    <NavMenu />
                </div>
                <div className='col-sm-12 col-md-9'>
                    { props.body }
                </div>
            </div>
          </div>;
}

export default Layout;
