import * as React from 'react';
import { Link } from 'react-router';

export class NavMenu extends React.Component<any, void> {
    public render() {
        return <div className='main-nav'>
                <div className='navbar navbar-inverse'>
                <div className='navbar-header'>
                    <button type='button' className='navbar-toggle' data-toggle='collapse' data-target='.navbar-collapse'>
                        <span className='sr-only'>Toggle navigation</span>
                        <span className='icon-bar'></span>
                        <span className='icon-bar'></span>
                        <span className='icon-bar'></span>
                    </button>
                    <Link className='navbar-brand' to={ '/' }>The Collection</Link>
                </div>
                <div className='clearfix'></div>
                <div className='navbar-collapse collapse'>
                    <ul className='nav navbar-nav'>
                        <li>
                            <Link to={ '/' } activeClassName='active'>
                                <span className='glyphicon glyphicon-home'></span> Home
                            </Link>
                        </li>
                         <li>
                            <Link to={ '/teabags' } activeClassName='active'>
                                <span className='glyphicon glyphicon-th-list'></span> Teabags
                            </Link>
                        </li>
                        <li>
                            <Link to={ '/teabagform' } activeClassName='active'>
                                <span className='glyphicon glyphicon-plus'></span> New teabag
                            </Link>
                        </li>
                        <li>
                            <Link to='/Account/LogOff'>
                                <span className='glyphicon glyphicon-log-out'></span> Log off
                            </Link>
                        </li>
                    </ul>
                </div>
            </div>
        </div>;
    }
}
