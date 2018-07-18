import * as React from 'react';
import { NavLink } from 'react-router-dom';

import './NavMenu.scss';

// tslint:disable-next-line:variable-name
const NavMenu: React.StatelessComponent<{}> = () => {
  return  <div className='main-nav'>
            <nav className='navbar flex-column navbar-toggleable-sm navbar-inverse'>
              <NavLink className='navbar-brand' to={ '/' }><img src='/images/teapot.svg' width='30' height='30' alt='' className='d-inline-block align-top' />The Collection</NavLink>
              <button className='navbar-toggler navbar-toggler-right' type='button' data-toggle='collapse' data-target='#navbarNav' aria-controls='navbarNav' aria-expanded='false' aria-label='Toggle navigation'>
                <span className='fa fa-bars'></span>
              </button>
              <div className='collapse navbar-collapse' id='navbarNav'>
                <ul className='nav navbar-nav flex-column'>
                  <li className='nav-item'>
                    <NavLink to={ '/' } activeClassName='active'>
                      <span className='fa fa-home'></span> Home
                    </NavLink>
                  </li>
                  <li className='nav-item'>
                    <NavLink to={ '/tea/dashboard' } activeClassName='active'>
                      <span className='fa fa-home'></span> Dashboard
                    </NavLink>
                  </li>
                  <li className='nav-item'>
                    <NavLink to={ '/tea/teabags' } activeClassName='active'>
                      <span className='fa fa-th-list'></span> Teabags
                    </NavLink>
                  </li>
                  <li className='nav-item'>
                    <NavLink to={ '/tea/teabagform' } activeClassName='active'>
                      <span className='fa fa-plus'></span> New teabag
                    </NavLink>
                  </li>
                  <li className='nav-item'>
                    <form action='/Account/LogOff/' method='post'>
                      <button className='btn btn-link' type='submit'><span className='fa fa-sign-out'></span> Log off</button>
                    </form>
                  </li>
                </ul>
              </div>
            </nav>
          </div>;
};

export default NavMenu;
