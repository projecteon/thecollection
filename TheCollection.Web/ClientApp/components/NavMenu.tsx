import * as React from 'react';
import { Link } from 'react-router';

export class NavMenu extends React.Component<any, void> {
    public render() {
        return  <div className='main-nav'>
                  <nav className='navbar flex-column navbar-toggleable-md navbar-inverse'>
                    <button className='navbar-toggler navbar-toggler-right' type='button' data-toggle='collapse' data-target='#navbarNav' aria-controls='navbarNav' aria-expanded='false' aria-label='Toggle navigation'>
                      <span className='navbar-toggler-icon'></span>
                    </button>
                    <Link className='navbar-brand' to={ '/' }><img src='/images/teapot.svg' width='30' height='30' alt='' className='d-inline-block align-top' />The Collection</Link>
                    <div className='collapse navbar-collapse' id='navbarNav'>
                      <ul className='nav navbar-nav flex-column'>
                        <li className='nav-item'>
                          <Link to={ '/' } activeClassName='active'>
                            <span className='fa fa-home'></span> Home
                          </Link>
                        </li>
                        <li className='nav-item'>
                          <Link to={ '/teabags' } activeClassName='active'>
                            <span className='fa fa-th-list'></span> Teabags
                          </Link>
                        </li>
                        <li className='nav-item'>
                          <Link to={ '/teabagform' } activeClassName='active'>
                            <span className='fa fa-plus'></span> New teabag
                          </Link>
                        </li>
                        <li className='nav-item'>
                          <Link to='/Account/LogOff'>
                            <span className='fa fa-sign-out'></span> Log off
                          </Link>
                        </li>
                      </ul>
                    </div>
                  </nav>
                </div>;
    }
}
