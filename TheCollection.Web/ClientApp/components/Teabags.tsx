import * as React from 'react';
import { Link } from 'react-router';
import { connect } from 'react-redux';
import { ApplicationState }  from '../store';
import * as TeabagsState from '../store/TeabagsStore';
import { ImageZoom } from "./ImageZoom";
import { ITeabag } from '../interfaces/ITeaBag';

import './Teabags.css';

// At runtime, Redux will merge together...
type TeabagsProps =
    TeabagsState.TeabagsState     // ... state we've requested from the Redux store
    & typeof TeabagsState.actionCreators   // ... plus action creators we've requested

class Teabags extends React.Component<TeabagsProps, void> {
  controls: {
      searchInput?: HTMLInputElement;
    } = {};

  constructor() {
    super();

    this.onSearch = this.onSearch.bind(this);
  }

  private validateSearch(searchTerms: string): boolean {
    if (searchTerms.trim().length < 3) {
      this.props.setSearchError('Requires at least 3 characters to search');
      this.controls.searchInput.focus();
      return false;
    }

    if (this.props.searchError.length > 0) {
      this.props.setSearchError('');
    }

    return true;
  }

  onSearch(event: React.MouseEvent<HTMLButtonElement>) {
    event.preventDefault();
    event.stopPropagation();
    if(this.validateSearch(this.controls.searchInput.value.trim())) {
      this.props.requestTeabags(this.controls.searchInput.value.trim());
    }
  }

  renderSearchSuccess() {
    let brands = this.props.teabags.map(teabag => teabag.brand.name).filter((value, index, self) => self.indexOf(value) === index)
    let tags = brands.map((brand, index) => { return <span key={index} className='col-xs-6 col-sm-4 col-md-2 labelBadge' style={{whiteSpace: 'nowrap'}}>{brand} <span className="badge pull-right">{this.props.teabags.filter(teagbag => { return teagbag.brand.name === brand; }).length}</span></span>;});
    return <div style={{display: 'flex', flexWrap: 'wrap', paddingleft: 5, marginTop: 35}}><p style={{width: '100%'}}><span key={-1} className='col-xs-6 col-sm-4 col-md-2 labelBadge' style={{whiteSpace: 'nowrap'}}>Total <span className="badge pull-right">{this.props.resultCount}</span></span>{tags}</p></div>;
  }

  renderSearchBar() {
    let iconClassName = this.props.isLoading === true ? 'glyphicon glyphicon-refresh glyphicon-refresh-animate' : 'glyphicon glyphicon-search';
    let groupClassName = this.props.searchError.length > 0 ? 'input-group has-error' : 'input-group';
    let btnClassName = this.props.searchError.length > 0 ? 'btn btn-danger' : 'btn btn-default';
    let alert = this.props.searchError.length > 0 ? <div className="alert alert-danger" role="alert">{this.props.searchError}</div> : undefined;
    return  <div style={{position: 'fixed', zIndex: 3, paddingTop: 10, paddingRight: 15, backgroundColor: '#ffffff'}}>
              {alert}
              <div className={groupClassName}>
                <input ref={input => this.controls.searchInput = input} type="text" className="form-control" placeholder="search terms" disabled={this.props.isLoading}/>
                <span className="input-group-btn">
                  <button type="button" className={btnClassName} onClick={this.onSearch} disabled={this.props.isLoading}>
                    <span className={iconClassName} aria-hidden="true" />
                  </button>
                </span>
              </div>
            </div>;
  }

  private renderTeabag(teabag: ITeabag, key: number) {
    return  <div key={key} style={{padding: 10, boxSizing: 'border-box', position: 'relative'}} className='col-xs-12 col-sm-4 col-md-2'>
              <div>
              {/*<div style={{boxShadow: '0px 13px 5px -10px rgba(0,0,0,0.75)'}}>*/}
                <img src={`/thumbnails/${teabag.imageid}/teabag.png`} style={{width: '100%', cursor: 'ponter'}} />
                <div style={{padding: '5px 10px', backgroundColor: '#fff', width: '100%', position: 'relative', minHeight: 55}}>
                  <strong style={{display: 'block', width: '100%', borderTop: '1px solid #959595'}}>{teabag.brand.name} - {teabag.flavour}</strong>
                  <div style={{color: '#959595'}}><small>{teabag.serie}</small></div>
                  <div style={{color: '#959595'}}><small>{teabag.hallmark}</small></div>
                  <div style={{color: '#959595'}}><small>{teabag.serialnumber}</small></div>
                  <div style={{color: '#959595'}}><small>{teabag.type.name}</small></div>
                  <div style={{color: '#959595'}}><small>{teabag.country.name}</small></div>
                  <p style={{position: 'absolute', bottom: -5, right: 15}}>
                    {/*<button type="button" className='btn btn-default'>
                      <span className='glyphicon glyphicon-pencil' aria-hidden="true" />
                    </button>*/}
                  <Link to={ `/teabagform/${teabag.id}` } activeClassName='active'>
                        <span className='glyphicon glyphicon-pencil'></span>
                    </Link>
                  </p>
                </div>
              </div>
            </div>;
  }

  private renderTeabags(teabags: ITeabag[]) {
    return teabags.map((teabag, index) => {
      return this.renderTeabag(teabag, index);
    });
  }

  render() {
    console.log(this.props);
    let contents = this.props.isLoading
      ? undefined
      : this.renderTeabags(this.props.teabags);

    return  <div>
              <div style={{marginBottom: 20, paddingBottom: 20}}>{this.renderSearchBar()}</div>
              {this.props.teabags.length === 0 ? undefined : this.renderSearchSuccess()}
              <div style={{display: 'flex', flexWrap: 'wrap', marginTop: this.props.teabags.length === 0 || this.props.searchError !== undefined ? 10 : 10}}>
                {contents}
              </div>
            </div>;
  }
}


export default connect(
    (state: ApplicationState) => state.teabags, // Selects which state properties are merged into the component's props
    TeabagsState.actionCreators                 // Selects which action creators are merged into the component's props
)(Teabags);
