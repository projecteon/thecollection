import * as React from 'react';
import { Component, ComponentClass } from 'react';
import debounce from 'lodash/debounce';

import { ItemsList } from './ItemsList';

import './ComboBox.css';

export interface IComboBoxProps<T> {
  selectedItem?: T;
  displayProperty: keyof T;
  renderItem(item: T): JSX.Element;
  onAddNew?(): void;
  onClear(): void;
  onItemSelected(item: T): void;
  onSearch(searchTerm: string): Promise<T[]>;
}

export interface IComboBoxState<T> {
  results: T[];
  selectedItem?: T;
  searchedTerm: any;
  searchTerm: any;
  displayResults: boolean;
  isLoading: boolean;
}

export function ComboBox<T>(): ComponentClass<IComboBoxProps<T>> {
  let Items = ItemsList<T>(); // tslint:disable-line:variable-name

  return class extends Component<IComboBoxProps<T>, IComboBoxState<T>> {
    delayedSearch: ((searchedTerm: string) => void) & _.Cancelable;
    controls: {
      input?: HTMLInputElement;
    } = {};

    constructor(props: IComboBoxProps<T>) {
      super(props);

      this.delayedSearch = debounce(this.onSearch, 250); // http://stackoverflow.com/questions/23123138/perform-debounce-in-react-js, https://medium.com/@justintulk/debouncing-reacts-controlled-textareas-w-redux-lodash-4383084ca090

      this.onFocus = this.onFocus.bind(this);
      this.onBlur = this.onBlur.bind(this);
      this.onClear = this.onClear.bind(this);
      this.onSearch = this.onSearch.bind(this);
      this.onItemSelected = this.onItemSelected.bind(this);
      this.onSearchTermChanged = this.onSearchTermChanged.bind(this);

      this.state = {selectedItem: props.selectedItem, searchTerm: props.selectedItem === undefined || props.selectedItem === null ? '' : props.selectedItem[props.displayProperty], displayResults: false, searchedTerm: '', results: [], isLoading: false};
    }

    componentWillReceiveProps(props: IComboBoxProps<T>) {
      if (props.selectedItem === undefined || props.selectedItem === null) {
        this.setState({...this.state, ...{displayResults: false, searchTerm: '', searchedTerm: '', selectedItem: undefined, results: [] }});
      }
      else if (props.selectedItem !== this.props.selectedItem) {
        this.setState({...this.state, ...{displayResults: false, searchTerm: props.selectedItem[props.displayProperty], searchedTerm: props.selectedItem[props.displayProperty], selectedItem: props.selectedItem, results: [props.selectedItem] }});
      }
    }

    getSelectedItems() {
      if ( this.state.selectedItem !== undefined && this.state.searchTerm === this.state.selectedItem[this.props.displayProperty]) {
        return  [this.state.selectedItem];
      }

      return this.state.results.filter(item => { return item[this.props.displayProperty].toString().toLowerCase().indexOf(this.state.searchTerm.toString().toLowerCase()) > -1; });
    };

    onFocus() {
      this.setState({...this.state, ...{displayResults: true}});
    }

    onBlur() {
      if (this.state.selectedItem !== undefined && this.state.searchTerm === this.state.selectedItem[this.props.displayProperty] && this.state.selectedItem[this.props.displayProperty] !== this.props.selectedItem[this.props.displayProperty]) {
        this.setState({...this.state, ...{displayResults: false, selectedItem: this.props.selectedItem, searchTerm: this.props.selectedItem === undefined ? this.state.searchTerm : this.props.selectedItem[this.props.displayProperty]}});
        return;
      }
    }

    onClear() {
      this.setState({...this.state, ...{displayResults: false, selectedItem: undefined, searchTerm: '', searchedTerm: ''}});

      if (this.props.onClear) {
        this.props.onClear();
      }
    }

    onSearch(searchedTerm: string) {
      if (this.state.searchedTerm !== searchedTerm || searchedTerm.length === 0) {
        this.props.onSearch(searchedTerm)
            .then(data => {this.setState({...this.state, ...{ isLoading: false, results: data, displayResults: true, searchedTerm: searchedTerm }});
        });
      }
    }

    onItemSelected(item: T) {
      this.props.onItemSelected(item);
    }

    onSearchTermChanged() {
      let searchedTerm = this.controls.input.value;
      this.setState({...this.state, ...{searchTerm: this.controls.input.value, isLoading: true}}, () => this.delayedSearch(searchedTerm));
    }

    renderClearButton() {
      if (this.state.selectedItem === undefined) {
        return undefined;
      }

      return  <span className='input-group-btn'>
                <button type='button' className='btn btn-danger' onClick={this.onClear}>
                  <span className='fa fa-times' aria-hidden='true' />
                </button>
              </span>;
    }

    renderLoadingIcon() {
      if (this.state.isLoading !== true) {
        return undefined;
      }

      let className = `fa fa-refresh fa-spin form-control-feedback ${this.props.onAddNew ? 'has-new' : ''}`;
      return <span className={className} aria-hidden='true'></span>;
    }

    renderSearchIcon() {
      if (this.state.isLoading === true) {
        return undefined;
      }

      let className = `fa fa-search form-control-feedback ${this.props.onAddNew ? 'has-new' : ''}`;
      return <span className={className} aria-hidden='true'></span>;
    }

    renderAddButton() {
      if (this.props.onAddNew === undefined) {
        return undefined;
      }

      return  <span className='input-group-btn'>
                <button type='button' className='btn btn-success' onClick={this.props.onAddNew}>
                  <span className='fa fa-plus' aria-hidden='true' />
                </button>
              </span>;
    }

    renderResult() {
      if (this.state.results.length <= 0 || this.state.displayResults === false) {
        return undefined;
      }

      let items = this.getSelectedItems();
      return <Items items={items} renderItem={this.props.renderItem} onItemSelected={this.onItemSelected}/>;
    }

    render() {
      let className = `input-group has-feedback ${this.state.selectedItem === undefined && this.state.searchedTerm.length > 0 && this.state.displayResults === false ? 'has-error' : ''}`;
      return  <div style={{position: 'relative'}}>
                <div className={className}>
                  {this.renderClearButton()}
                  <input ref={input => this.controls.input = input} type='text' className='form-control' id='inputType' placeholder='Search for...' autoComplete='off' role='combobox' onChange={this.onSearchTermChanged} value={this.state.searchTerm} onFocus={this.onFocus} onBlur={this.onBlur}/>
                  {this.renderSearchIcon()}
                  {this.renderLoadingIcon()}
                  {this.renderAddButton()}
                </div>
                {this.renderResult()}
              </div>;
    }
  };
}
