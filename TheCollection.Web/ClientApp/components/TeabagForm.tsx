import * as React from 'react';

import {ComboBox} from './ComboBox/ComboBox';
import {Loader} from './Loader';

import {ITeabag} from '../interfaces/ITeaBag';
import {IBrand} from '../interfaces/IBrand';
import {ICountry} from '../interfaces/ICountry';
import {IBagType} from '../interfaces/IBagType';

const BrandComboBox = ComboBox<IBrand>();
const CountryComboBox = ComboBox<ICountry>();
const TypeComboBox = ComboBox<IBagType>();

export interface ITeabagFormProps extends React.Props<any> {
  teabag?: ITeabag;
  params?: {id?: string}
}

export interface ITeabagFormState extends React.Props<any> {
  teabag: ITeabag;
  isLoading: boolean;
  searchedBagTypes: IBagType[];
  searchedCountries: ICountry[];
}

export class TeabagForm extends React.Component<ITeabagFormProps, ITeabagFormState> {

  constructor(props: ITeabagFormProps) {
    super(props);

    this.onAddNewBrand = this.onAddNewBrand.bind(this);
    this.onBrandSelected = this.onBrandSelected.bind(this);
    this.onClearBrand = this.onClearBrand.bind(this);
    this.onSearchBrand = this.onSearchBrand.bind(this);
    this.onSearchCountry = this.onSearchCountry.bind(this);
    this.onSearchType = this.onSearchType.bind(this);

    let state = {isLoading: false, searchedBagTypes: [], searchedCountries: [], teabag: {} as ITeabag};

    if (this.props.params && this.props.params.id && this.props.params.id.length > 0) {
      state.isLoading = true;
      fetch(`/api/Bags/${this.props.params.id}`)
        .then(response => response.json() as Promise<ITeabag>)
        .then(data => {
          this.setState({...this.state, ...{ teabag: data, isLoading: false }});
      });
    }

    this.state = state;
  }

  onAddNewBrand() {}
  onBrandSelected(brand: IBrand) {
    let teabag = {...this.state.teabag, ...{brand: brand}};
    this.setState({...this.state, ...{teabag: teabag}});
  }

  onClearBrand() {
    let teabag = {...this.state.teabag, ...{brand: undefined}};
    this.setState({...this.state, ...{teabag: teabag}});
  }

  onSearchBrand(searchTerm: string) {
    let uri = `/api/Brands/?searchterm=${encodeURIComponent(searchTerm)}`
    return fetch(uri)
            .then(response => response.json() as Promise<IBrand[]>);
  }

  onAddNewCountry() {}
  onCountrySelected(country: ICountry) {
    let teabag = {...this.state.teabag, ...{country: country}};
    this.setState({...this.state, ...{teabag: teabag}});
  }

  onSearchCountry(searchTerm: string) {
    let uri = `/api/Countries/?searchterm=${encodeURIComponent(searchTerm)}`
    return fetch(uri)
            .then(response => response.json() as Promise<ICountry[]>);
  }

  onAddNewBagType() {}
  onBagTypeSelected(bagtype: IBagType) {
    let teabag = {...this.state.teabag, ...{type: bagtype}};
    this.setState({...this.state, ...{teabag: teabag}});
  }
  onSearchType(searchTerm: string) {
    let uri = `/api/BagTypes/?searchterm=${encodeURIComponent(searchTerm)}`
    return fetch(uri)
            .then(response => response.json() as Promise<IBagType[]>);
  }

  renderBrandComboBoxItem(item: IBrand) {
    return <div>{item.name}</div>;
  }

  renderCountryComboBoxItem(item: ICountry) {
    return <div>{item.name}</div>;
  }

  renderBagTypeComboBoxItem(item: IBagType) {
    return <div>{item.name}</div>;
  }

  renderForm() {
    return  <form className="form-horizontal" style={{marginTop: 10}}>
              <div className="form-group">
                <div className="col-sm-offset-2 col-sm-10">
                  {/*<img src={`/images/${this.state.teabag.image}`}  style={{maxWidth: 'calc(100vw - 30px)', maxHeight: '95vh', cursor: 'ponter'}} />*/}
                </div>
              </div>
              <div className="form-group">
                <label htmlFor="inputBrand" className="col-sm-2 control-label">Brand</label>
                <div className="col-sm-6">
                  <BrandComboBox onAddNew={this.onAddNewBrand} onSearch={this.onSearchBrand} onItemSelected={this.onBrandSelected} renderItem={this.renderBrandComboBoxItem} displayProperty={'name'} selectedItem={this.state.teabag.brand} onClear={this.onClearBrand}/>
                </div>
              </div>
              <div className="form-group">
                <label htmlFor="inputFlavour" className="col-sm-2 control-label">Flavour</label>
                <div className="col-sm-6">
                  <input type="text" className="form-control" id="inputFlavour" placeholder="Flavour" value={this.state.teabag.flavour} />
                </div>
              </div>
              <div className="form-group">
                <label htmlFor="inputSeries" className="col-sm-2 control-label">Series</label>
                <div className="col-sm-6">
                  <input type="text" className="form-control" id="inputSeries" placeholder="Series" value={this.state.teabag.serie} />
                </div>
              </div>
              <div className="form-group">
                <label htmlFor="inputSerialnumber" className="col-sm-2 control-label">Serialnumber</label>
                <div className="col-sm-3">
                  <input type="text" className="form-control" id="inputSerialnumber" placeholder="Serialnumber" value={this.state.teabag.serialnumber} />
                </div>
              </div>
              <div className="form-group">
                <label htmlFor="inputHallmark" className="col-sm-2 control-label">Hallmark</label>
                <div className="col-sm-6  ">
                  <textarea className="form-control" id="inputHallmark" placeholder="Hallmark" value={this.state.teabag.hallmark} />
                </div>
              </div>
              <div className="form-group">
                <label htmlFor="inputType" className="col-sm-2 control-label">Type</label>
                <div className="col-sm-3">
                  <TypeComboBox onAddNew={this.onAddNewCountry} onSearch={this.onSearchType} renderItem={this.renderBagTypeComboBoxItem} displayProperty={'name'} selectedItem={this.state.teabag.type} />
                </div>
              </div>
              <div className="form-group">
                <label htmlFor="inputCountry" className="col-sm-2 control-label">Country</label>
                <div className="col-sm-3">
                  <CountryComboBox onAddNew={this.onAddNewBagType} onSearch={this.onSearchCountry} renderItem={this.renderCountryComboBoxItem} displayProperty={'name'} selectedItem={this.state.teabag.country} />
                </div>
              </div>
              <div className="form-group">
                <div className="col-sm-offset-2 col-sm-10">
                  <button type="submit" className="btn btn-default">
                    <span className="glyphicon glyphicon-floppy-disk" aria-hidden="true"></span> Save
                  </button>
                </div>
              </div>
            </form>;
  }

  render() {
    return  <div>
              {this.state.isLoading === true ? <Loader /> : undefined}
              {this.renderForm()}
            </div>
  }
}
