import * as React from 'react';
import * as Mousetrap from 'mousetrap';
import { Link } from 'react-router';
import { connect } from 'react-redux';
import { IApplicationState }  from '../store';
import * as TeaBagsReducer from '../reducers/teabags';
import { ImageZoom } from '../components/ImageZoom';
import { TeabagCard } from '../components/TeabagCard';
import { ITeabag } from '../interfaces/ITeaBag';

import './Teabags.scss';

type TeabagsProps =
    TeaBagsReducer.ITeabagsState     // ... state we've requested from the Redux store
    & typeof TeaBagsReducer.actionCreators   // ... plus action creators we've requested

class Teabags extends React.Component<TeabagsProps, {}> {
  static SearchKeyboardShortcut = 'enter';
  controls: {
      searchInput?: HTMLInputElement;
    } = {};

  constructor(props: TeabagsProps) {
    super();

    this.onKeyboardSearch = this.onKeyboardSearch.bind(this);
    this.onSearch = this.onSearch.bind(this);
    this.onSearchTermsChanged = this.onSearchTermsChanged.bind(this);
    this.onZoomClicked = this.onZoomClicked.bind(this);

    // var mousetrap = new Mousetrap(/*add ref to searchbar dom*/);
    // mousetrap.stopCallback = function(){ return false; };
  }

  componentDidMount() {
    Mousetrap.prototype.stopCallback = () => { return false; };
    Mousetrap.bind(Teabags.SearchKeyboardShortcut, this.onKeyboardSearch);
  }

  componentWillUnmount() {
    Mousetrap.unbind(Teabags.SearchKeyboardShortcut);
  }

  onSearchTermsChanged() {
    event.preventDefault();
    event.stopPropagation();
    this.props.validateSearchTerms(this.controls.searchInput.value.trim());
  }

  onKeyboardSearch(event: ExtendedKeyboardEvent) {
    event.preventDefault();
    event.stopPropagation();
    this.props.requestTeabags(this.controls.searchInput.value.trim());
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
    return  <div style={{position: 'sticky', top: -1, zIndex: 4, paddingTop: 10, paddingBottom: 8, backgroundColor: '#ffffff', width: '100%' }}>
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
    return  <TeabagCard key={key} teabag={teabag} onZoomClicked={this.onZoomClicked} />;
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
