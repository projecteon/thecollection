import * as React from 'react';
import { Link } from 'react-router';
import { connect } from 'react-redux';
import { IApplicationState }  from '../store';
import * as TeaBagsReducer from '../reducers/teabags';
import { ImageZoom } from '../components/ImageZoom';
import { ITeabag } from '../interfaces/ITeaBag';

import './Teabags.css';

type TeabagsProps =
    TeaBagsReducer.ITeabagsState     // ... state we've requested from the Redux store
    & typeof TeaBagsReducer.actionCreators   // ... plus action creators we've requested

class Teabags extends React.Component<TeabagsProps, void> {
  controls: {
      searchInput?: HTMLInputElement;
    } = {};

  constructor(props: TeabagsProps) {
    super();

    this.onSearch = this.onSearch.bind(this);
    this.onSearchTermsChanged = this.onSearchTermsChanged.bind(this);
  }

  onSearchTermsChanged() {
    event.preventDefault();
    event.stopPropagation();
    this.props.validateSearchTerms(this.controls.searchInput.value.trim());
  }

  onSearch(event: React.MouseEvent<HTMLButtonElement>) {
    event.preventDefault();
    event.stopPropagation();
    this.props.requestTeabags(this.controls.searchInput.value.trim());
  }

  onZoomClicked(imageId?: string) {
    this.props.zoomImage(imageId);
  }

  renderSuccessTag(brand: string, index: number) {
    return <span key={index} className='col-xs-6 col-sm-4 col-md-2 labelBadge justify-content-between align-items-center' style={{whiteSpace: 'nowrap'}}>{brand} <span className='badge badge-default'>{this.props.teabags.filter(teagbag => { return teagbag.brand.name === brand; }).length}</span></span>;
  }

  renderSearchSuccesses() {
    let brands = this.props.teabags.map(teabag => teabag.brand.name).filter((value, index, self) => self.indexOf(value) === index);
    let tags = brands.map((brand, index) => { return this.renderSuccessTag(brand, index); });
    return  <div style={{display: 'flex', flexWrap: 'wrap', paddingleft: 5}}>
              <p style={{width: '100%'}}>
                <span key={-1} className='col-xs-6 col-sm-4 col-md-2 labelBadge justify-content-between align-items-center' style={{whiteSpace: 'nowrap'}}>Total <span className='badge badge-default'>{this.props.resultCount}</span></span>
                {tags}
              </p>
            </div>;
  }

  renderSearchBar() {
    let iconClassName = this.props.isLoading === true ? 'fa fa-refresh fa-spin' : 'fa fa-search';
    let groupClassName = this.props.searchError.length > 0 ? 'input-group has-error' : 'input-group';
    let btnClassName = this.props.searchError.length > 0 ? 'btn btn-danger' : 'btn btn-default';
    let alert = this.props.searchError.length > 0 ? <div className='alert alert-danger' role='alert'>{this.props.searchError}</div> : undefined;
    return  <div style={{position: 'sticky', top: -1, zIndex: 1, paddingTop: 10, paddingBottom: 3, backgroundColor: '#ffffff', width: '100%', marginBottom: 5}}>
              {alert}
              <div className={groupClassName}>
                <input ref={input => this.controls.searchInput = input} type='text' className='form-control' placeholder='Search for...' disabled={this.props.isLoading} onChange={this.onSearchTermsChanged}/>
                <span className='input-group-btn'>
                  <button type='button' className={btnClassName} onClick={this.onSearch} disabled={this.props.isLoading}>
                    <span className={iconClassName} aria-hidden='true' />
                  </button>
                </span>
              </div>
            </div>;
  }

  private renderTeabag(teabag: ITeabag, key: number) {
    return  <div key={key} style={{padding: 10, boxSizing: 'border-box', position: 'relative'}} className='col-xs-12 col-sm-4 col-md-3 col-lg-2'>
              <div>
                {teabag.imageid ? <img src={`/thumbnails/${teabag.imageid}/teabag.png`} style={{width: '100%', cursor: 'pointer'}} onClick={this.onZoomClicked.bind(this, teabag.imageid)}/> : undefined}
                <div style={{padding: '5px 10px', backgroundColor: '#fff', width: '100%', position: 'relative', minHeight: 55}} className='details'>
                  <strong style={{display: 'block', width: '100%', borderTop: '1px solid #959595', overflow: 'hidden'}}>{teabag.brand.name} - {teabag.flavour}</strong>
                  <div style={{color: '#959595'}}><small>{teabag.serie}</small></div>
                  <div style={{color: '#959595'}}><small>{teabag.hallmark}</small></div>
                  <div style={{color: '#959595'}}><small>{teabag.serialnumber}</small></div>
                  <div style={{color: '#959595'}}><small>{teabag.type ? teabag.type.name : ''}</small></div>
                  <div style={{color: '#959595'}}><small>{teabag.country ? teabag.country.name : ''}</small></div>
                  <p className='edit'>
                    <Link to={ `/teabagform/${teabag.id}` } activeClassName='active' className='text-info'>
                      <span className='fa fa-pencil'></span>
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
    let contents = this.props.isLoading
      ? undefined
      : this.renderTeabags(this.props.teabags);

    return  <div style={{position: 'relative'}}>
              {this.props.zoomImageId !== undefined ? <ImageZoom imageid={this.props.zoomImageId} onClick={this.onZoomClicked.bind(this, undefined)} /> : undefined}
              {this.renderSearchBar()}
              {this.props.teabags.length === 0 ? undefined : this.renderSearchSuccesses()}
              <div style={{display: 'flex', flexWrap: 'wrap', marginTop: this.props.teabags.length === 0 || this.props.searchError !== undefined ? 10 : 10}}>
                {contents}
              </div>
            </div>;
  }
}


export default connect(
    (state: IApplicationState) => state.teabags,
    TeaBagsReducer.actionCreators
)(Teabags);
