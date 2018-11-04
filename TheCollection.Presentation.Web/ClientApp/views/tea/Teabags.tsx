import * as React from 'react';
import * as Mousetrap from 'mousetrap';
import { connect, Dispatch } from 'react-redux';
import { IApplicationState }  from '../../store';
import { ImageZoom } from '../../components/ImageZoom';
import TeabagCard from '../../components/TeabagCard';
import { ITeabag } from '../../interfaces/tea/IBag';
import { search, searchTermChanged, zoomImageToggle } from '../../actions/tea/search';

import './Teabags.scss';

const mapDispatchToProps = (dispatch: Dispatch) => {
  return {
    onSearch: () => dispatch(search()),
    onSearchTermChanged: (searchTerm: string) => dispatch(searchTermChanged(searchTerm)),
    onZoomImageToggle: (imageId: string) => dispatch(zoomImageToggle(imageId)),
  };
};

function mapStateToProps(state: IApplicationState) {
  return {...state.searchteabag, ...state.ui};
}

type TeabagsProps = ReturnType<typeof mapStateToProps> & ReturnType<typeof mapDispatchToProps>;

class Teabags extends React.Component<TeabagsProps, {}> {
  searchKeyboardShortcut = 'enter';
  searchinputMouseTrap: MousetrapInstance | undefined = undefined;

  controls: {
      searchInput?: HTMLInputElement | null;
    } = {};

  constructor(props: TeabagsProps) {
    super(props);

    this.onKeyboardSearch = this.onKeyboardSearch.bind(this);
    this.onSearch = this.onSearch.bind(this);
    this.onSearchTermsChanged = this.onSearchTermsChanged.bind(this);
    this.onZoomClicked = this.onZoomClicked.bind(this);
  }

  componentDidMount() {
    if (!this.controls.searchInput) {
      return;
    }

    this.searchinputMouseTrap = new Mousetrap(this.controls.searchInput);
    this.searchinputMouseTrap!.stopCallback = function() { return false; };
    this.searchinputMouseTrap!.bind(this.searchKeyboardShortcut, this.onKeyboardSearch);
  }

  componentWillUnmount() {
    if (!this.searchinputMouseTrap) {
      return;
    }

    this.searchinputMouseTrap.unbind(this.searchKeyboardShortcut);
    this.searchinputMouseTrap = undefined;
  }

  onSearchTermsChanged(event: React.ChangeEvent<HTMLInputElement>) {
    event.preventDefault();
    event.stopPropagation();
    if (!this.controls.searchInput) {
      return;
    }

    this.props.onSearchTermChanged(this.controls.searchInput.value);
  }

  onKeyboardSearch(event: ExtendedKeyboardEvent) {
    event.preventDefault();
    event.stopPropagation();
    this.props.onSearch();
  }

  onSearch(event: React.MouseEvent<HTMLButtonElement>) {
    event.preventDefault();
    event.stopPropagation();
    this.props.onSearch();
  }

  onZoomClicked(imageId: string) {
    this.props.onZoomImageToggle(imageId);
  }

  renderSuccessTag(brand: string, index: number) {
    return  <span key={index} className='labelBadge justify-content-between align-items-center' style={{whiteSpace: 'nowrap'}}>
              <span>{brand}</span>
              <span className='badge badge-default'>{this.props.result.filter(teagbag => { return teagbag.brand.name === brand; }).length}</span>
            </span>;
  }

  renderSearchSuccesses() {
    let brands = this.props.result.map(teabag => teabag.brand.name).filter((value, index, self) => self.indexOf(value) === index);
    let tags = brands.map((brand, index) => { return this.renderSuccessTag(brand, index); });
    return  <div style={{display: 'flex', flexWrap: 'wrap', paddingLeft: 5}}>
              <span key={-1} className='labelBadge justify-content-between align-items-center' style={{whiteSpace: 'nowrap'}}>
                <span>Total</span>
                <span className='badge badge-default'>{this.props.resultCount}</span>
              </span>
              {tags}
            </div>;
  }

  renderSearchBar() {
    let iconClassName = this.props.isLoading === true ? 'fa fa-refresh fa-spin' : 'fa fa-search';
    let groupClassName = this.props.searchError.length > 0 ? 'input-group has-error' : 'input-group';
    let btnClassName = this.props.searchError.length > 0 ? 'btn btn-danger' : 'btn btn-default';
    let alert = this.props.searchError.length > 0 ? <div className='alert alert-danger' role='alert'>{this.props.searchError}</div> : undefined;
    return  <div style={{position: 'sticky', zIndex: 4, paddingTop: 10, paddingBottom: 8, backgroundColor: '#ffffff', width: '100%' }} className='searchBar'>
              {alert}
              <div className={groupClassName}>
                <input ref={input => this.controls.searchInput = input} type='text' className='form-control mousetrap' placeholder='Search for...' value={this.props.searchedTerms} disabled={this.props.isLoading} onChange={this.onSearchTermsChanged}/>
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
      : this.renderTeabags(this.props.result);

    return  <div style={{position: 'relative'}}>
              {this.props.zoomImageId !== undefined ? <ImageZoom imageid={this.props.zoomImageId} onClick={this.onZoomClicked.bind(this, undefined)} /> : undefined}
              {this.renderSearchBar()}
              {this.props.result.length === 0 ? undefined : this.renderSearchSuccesses()}
              <div style={{display: 'flex', flexWrap: 'wrap', marginTop: this.props.result.length === 0 || this.props.searchError !== undefined ? 10 : 10}}>
                {contents}
              </div>
            </div>;
  }
}

export default connect(
  mapStateToProps,
  mapDispatchToProps,
)(Teabags);
