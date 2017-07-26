import * as React from 'react';
import { Link } from 'react-router';
import { connect } from 'react-redux';
import { IApplicationState }  from '../store';
import * as TeabagReducer from '../reducers/teabag';

import {ComboBox} from '../components/ComboBox/ComboBox';
import {Loader} from '../components/Loader';

import {IBrand} from '../interfaces/IBrand';
import {ICountry} from '../interfaces/ICountry';
import {IBagType} from '../interfaces/IBagType';

const BrandComboBox = ComboBox<IBrand>(); // tslint:disable-line:variable-name
const CountryComboBox = ComboBox<ICountry>(); // tslint:disable-line:variable-name
const TypeComboBox = ComboBox<IBagType>(); // tslint:disable-line:variable-name

type TeabagsProps =
    TeabagReducer.ITeabagState     // ... state we've requested from the Redux store
    & typeof TeabagReducer.actionCreators   // ... plus action creators we've requested
    & { params?: { id?: string } };       // ... plus incoming routing parameters

class TeabagForm extends React.Component<TeabagsProps, void> {

  constructor() {
    super();

    this.onAddNewBagType = this.onAddNewBagType.bind(this);
    this.onAddNewBrand = this.onAddNewBrand.bind(this);
    this.onAddNewCountry = this.onAddNewCountry.bind(this);
    this.onBagTypeSelected = this.onBagTypeSelected.bind(this);
    this.onBrandSelected = this.onBrandSelected.bind(this);
    this.onClearBagType = this.onClearBagType.bind(this);
    this.onClearBrand = this.onClearBrand.bind(this);
    this.onClearCountry = this.onClearCountry.bind(this);
    this.onCountrySelected = this.onCountrySelected.bind(this);
    this.onSearchBrand = this.onSearchBrand.bind(this);
    this.onSearchCountry = this.onSearchCountry.bind(this);
    this.onSearchType = this.onSearchType.bind(this);
  }

  componentWillMount() {
    if (this.props.params && this.props.params.id && this.props.params.id.length > 0) {
      this.props.requestTeabag(this.props.params.id);
    }
  }

  componentWillReceiveProps(nextProps: TeabagsProps) {
    if (this.props.isLoading === false && this.props.params && this.props.params.id && this.props.params.id.length > 0 && this.props.teabag.id !== this.props.params.id) {
      this.props.requestTeabag(this.props.params.id);
    }
  }

  onAddNewBrand() {}

  onBrandSelected(brand: IBrand) {
    this.props.changeBrand(brand);
  }

  onClearBrand() {
    this.props.clearBrand();
  }

  onSearchBrand(searchTerm: string) {
    let uri = `/api/Brands/?searchterm=${encodeURIComponent(searchTerm)}`;
    return fetch(uri)
            .then(response => response.json() as Promise<IBrand[]>);
  }

  onAddNewCountry() {}
  onCountrySelected(country: ICountry) {
    this.props.changeCountry(country);
  }

  onClearCountry() {
    this.props.clearCountry();
  }

  onSearchCountry(searchTerm: string) {
    let uri = `/api/Countries/?searchterm=${encodeURIComponent(searchTerm)}`;
    return fetch(uri)
            .then(response => response.json() as Promise<ICountry[]>);
  }

  onAddNewBagType() {}

  onBagTypeSelected(bagtype: IBagType) {
    this.props.changeBagtype(bagtype);
  }

  onClearBagType() {
    this.props.clearBagtype();
  }

  onSearchType(searchTerm: string) {
    let uri = `/api/BagTypes/?searchterm=${encodeURIComponent(searchTerm)}`;
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
    return  <form className='form-horizontal' style={{marginTop: 10}}>
              <div className='form-group'>
                <div className='col-sm-offset-2 col-sm-10'>
                  {/*<img src={`/images/${this.state.teabag.image}`}  style={{maxWidth: 'calc(100vw - 30px)', maxHeight: '95vh', cursor: 'ponter'}} />*/}
                </div>
              </div>
              <div className='form-group'>
                <label htmlFor='inputBrand' className='col-sm-2 control-label'>Brand</label>
                <div className='col-sm-6'>
                  <BrandComboBox onAddNew={this.onAddNewBrand} onSearch={this.onSearchBrand} onItemSelected={this.onBrandSelected} renderItem={this.renderBrandComboBoxItem} displayProperty={'name'} selectedItem={this.props.teabag.brand} onClear={this.onClearBrand}/>
                </div>
              </div>
              <div className='form-group'>
                <label htmlFor='inputFlavour' className='col-sm-2 control-label'>Flavour</label>
                <div className='col-sm-6'>
                  <input type='text' className='form-control' id='inputFlavour' placeholder='Flavour' value={this.props.teabag.flavour} />
                </div>
              </div>
              <div className='form-group'>
                <label htmlFor='inputSeries' className='col-sm-2 control-label'>Series</label>
                <div className='col-sm-6'>
                  <input type='text' className='form-control' id='inputSeries' placeholder='Series' value={this.props.teabag.serie} />
                </div>
              </div>
              <div className='form-group'>
                <label htmlFor='inputSerialnumber' className='col-sm-2 control-label'>Serialnumber</label>
                <div className='col-sm-3'>
                  <input type='text' className='form-control' id='inputSerialnumber' placeholder='Serialnumber' value={this.props.teabag.serialnumber} />
                </div>
              </div>
              <div className='form-group'>
                <label htmlFor='inputHallmark' className='col-sm-2 control-label'>Hallmark</label>
                <div className='col-sm-6  '>
                  <textarea className='form-control' id='inputHallmark' placeholder='Hallmark' value={this.props.teabag.hallmark} />
                </div>
              </div>
              <div className='form-group'>
                <label htmlFor='inputType' className='col-sm-2 control-label'>Type</label>
                <div className='col-sm-6'>
                  <TypeComboBox onAddNew={this.onAddNewBagType} onSearch={this.onSearchType} onItemSelected={this.onBagTypeSelected} renderItem={this.renderBagTypeComboBoxItem} displayProperty={'name'} selectedItem={this.props.teabag.type}  onClear={this.onClearBagType} />
                </div>
              </div>
              <div className='form-group'>
                <label htmlFor='inputCountry' className='col-sm-2 control-label'>Country</label>
                <div className='col-sm-6'>
                  <CountryComboBox onAddNew={this.onAddNewCountry} onSearch={this.onSearchCountry} onItemSelected={this.onCountrySelected} renderItem={this.renderCountryComboBoxItem} displayProperty={'name'} selectedItem={this.props.teabag.country}  onClear={this.onClearCountry} />
                </div>
              </div>
              <div className='form-group'>
                <div className='col-sm-offset-2 col-sm-10'>
                  <button type='submit' className='btn btn-default'>
                    <span className='fa fa-floppy-o' aria-hidden='true'></span> Save
                  </button>
                </div>
              </div>
            </form>;
  }

  render() {
    return  <div>
              {this.props.isLoading === true ? <Loader /> : undefined}
              {this.renderForm()}
            </div>;
  }
}

export default connect(
    (state: IApplicationState) => state.teabag, // selects which state properties are merged into the component's props
    TeabagReducer.actionCreators                 // selects which action creators are merged into the component's props
)(TeabagForm);

