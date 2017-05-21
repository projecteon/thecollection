import * as React from 'react';
import { Link } from 'react-router';
import { ImageZoom } from "./ImageZoom";
import {ITeabag} from '../interfaces/ITeaBag';

import './Teabags.css';
import { ISearchResult } from "ClientApp/interfaces/ISearchResult";

export interface ITeabagsState {
  teabags: ITeabag[];
  resultCount?: number;
  isLoading: boolean;
  zoomUri?: string;
  searchTerms?: string;
  searchError?: string;
}

export class Teabags extends React.Component<void, ITeabagsState> {
  controls: {
      searchInput?: HTMLInputElement;
    } = {};

  constructor() {
    super();

    this.onSearch = this.onSearch.bind(this);
    this.onSearchTermChanged = this.onSearchTermChanged.bind(this);

    this.state = {isLoading: false, teabags: [], searchTerms: ''};
  }

  private validateSearch(): boolean {
    if (this.state.searchTerms === undefined || this.state.searchTerms.trim().length < 3) {
      this.setState({...this.state, ...{searchError: 'Requires at least 3 characters to search'}});
      this.controls.searchInput.focus();
      return false;
    }

    if (this.state.searchError !== undefined) {
      this.setState({...this.state, ...{searchError: undefined}});
    }

    return true;
  }

  onZoomClicked(teabag?: ITeabag) {
    if(teabag != undefined) {
      this.setState({...this.state, ...{zoomUri: teabag.image}});
    }
    else {
      this.setState({...this.state, ...{zoomUri: undefined}});
    }
  }

  onSearchTermChanged(event: React.FormEvent<HTMLInputElement>) {
    let searchError = {};
    if (this.state.searchError !== undefined && (event.target as any).value.trim().length > 2) {
      searchError = {searchError: undefined};
    }

    this.setState({...this.state, ...searchError, ...{searchTerms: (event.target as any).value}});
  }

  onSearch(event: React.MouseEvent<HTMLButtonElement>) {
    event.preventDefault();
    event.stopPropagation();
    if(this.validateSearch() === false) {
      return;
    }

    this.setState({ teabags: [], isLoading: true });
    let uri = this.state.searchTerms !== undefined && this.state.searchTerms.length > 0
      ? `/api/Bags/?searchterm=${encodeURIComponent(this.state.searchTerms)}`
      : `/api/Bags/`;

    fetch(uri)
      .then(response => response.json() as Promise<ISearchResult<ITeabag>>)
      .then(searchResult => {
        console.log(`loaded ${searchResult.count} rows`);
        this.setState({ teabags: searchResult.data, resultCount: searchResult.count, isLoading: false });
      });
  }

  renderSearchSuccess() {
    let brands = this.state.teabags.map(teabag => teabag.brand.name).filter((value, index, self) => self.indexOf(value) === index)
    let tags = brands.map((brand, index) => { return <span key={index} className='col-xs-6 col-sm-4 col-md-2 labelBadge' style={{whiteSpace: 'nowrap'}}>{brand} <span className="badge pull-right">{this.state.teabags.filter(teagbag => { return teagbag.brand.name === brand; }).length}</span></span>;});
    return <div style={{display: 'flex', flexWrap: 'wrap', paddingleft: 5, marginTop: 35}}><p style={{width: '100%'}}><span key={-1} className='col-xs-6 col-sm-4 col-md-2 labelBadge' style={{whiteSpace: 'nowrap'}}>Total <span className="badge pull-right">{this.state.resultCount}</span></span>{tags}</p></div>;
  }

  renderSearchBar() {
    let iconClassName = this.state.isLoading === true ? 'glyphicon glyphicon-refresh glyphicon-refresh-animate' : 'glyphicon glyphicon-search';
    let groupClassName = this.state.searchError !== undefined ? 'input-group has-error' : 'input-group';
    let btnClassName = this.state.searchError !== undefined ? 'btn btn-danger' : 'btn btn-default';
    let alert = this.state.searchError !== undefined ? <div className="alert alert-danger" role="alert">{this.state.searchError}</div> : undefined;
    return  <div style={{position: 'fixed', zIndex: 3, paddingTop: 10, paddingRight: 15, backgroundColor: '#ffffff'}}>
              {alert}
              <div className={groupClassName}>
                <input ref={input => this.controls.searchInput = input} type="text" className="form-control" placeholder="search terms" onChange={this.onSearchTermChanged} disabled={this.state.isLoading}/>
                <span className="input-group-btn">
                  <button type="button" className={btnClassName} onClick={this.onSearch} disabled={this.state.isLoading}>
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
                <img src={`/images/thumbnail/${teabag.image}`} style={{width: '100%', cursor: 'ponter'}} onClick={this.onZoomClicked.bind(this, teabag)}/>
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
    let contents = this.state.isLoading
      ? undefined
      : this.renderTeabags(this.state.teabags);

    return  <div>
              {this.state.zoomUri !== undefined ? <ImageZoom uri={this.state.zoomUri} onClick={this.onZoomClicked.bind(this)} /> : undefined}
              <div style={{marginBottom: 20, paddingBottom: 20}}>{this.renderSearchBar()}</div>

              {this.state.teabags.length === 0 ? undefined : this.renderSearchSuccess()}
              <div style={{display: 'flex', flexWrap: 'wrap', marginTop: this.state.teabags.length === 0 || this.state.searchError !== undefined ? 10 : 10}}>
                {contents}
              </div>
            </div>;
  }
}
