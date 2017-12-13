import * as React from 'react';
import { Link, withRouter } from 'react-router';
import { connect } from 'react-redux';
import { History } from 'history';
import { IApplicationState }  from '../../store';
import * as TeabagReducer from '../../reducers/tea/bag';
import * as BrandReducer from '../../reducers/tea/brand';

import {ComboBox} from '../../components/ComboBox/ComboBox';
import Loader from '../../components/Loader';
import {FormGroupItem} from '../../components/FormGroupItem';
import TextInput from '../../components/TextInput';
import Textarea from '../../components/Textarea';
import {Image} from '../../components/Image';

import {IBrand} from '../../interfaces/tea/IBrand';
import {ICountry} from '../../interfaces/ICountry';
import {IBagType} from '../../interfaces/tea/IBagType';

const CountryComboBox = ComboBox<ICountry>(); // tslint:disable-line:variable-name
const TypeComboBox = ComboBox<IBagType>(); // tslint:disable-line:variable-name

const InputFormGroupItem = FormGroupItem(TextInput); // tslint:disable-line:variable-name
const TextareaFormGroupItem = FormGroupItem(Textarea); // tslint:disable-line:variable-name
const BrandInputGroupItem = FormGroupItem(ComboBox<IBrand>()); // tslint:disable-line:variable-name
const CountryInputGroupItem = FormGroupItem(ComboBox<ICountry>()); // tslint:disable-line:variable-name
const BagtypeInputGroupItem = FormGroupItem(ComboBox<IBagType>()); // tslint:disable-line:variable-name

type TeabagProps =
    TeabagReducer.ITeabagState            // ... state we've requested from the Redux store
    & typeof TeabagReducer.actionCreators // ... plus action creators we've requested
    & { params?: { id?: string } }        // ... plus incoming routing parameters
    & { history: History; };              // ... plus naviation through react router

class TeabagForm extends React.Component<TeabagProps, {}> {

  constructor(props: TeabagProps) {
    super();
    this.onAddNewBagType = this.onAddNewBagType.bind(this);
    this.onAddNewBrand = this.onAddNewBrand.bind(this);
    this.onAddNewCountry = this.onAddNewCountry.bind(this);
    this.onSearchCountry = this.onSearchCountry.bind(this);
    this.onSearchType = this.onSearchType.bind(this);
    this.onSave = this.onSave.bind(this);
  }

  componentWillMount() {
    if (this.props.params && this.props.params.id && this.props.params.id.length > 0) {
      if (this.props.teabag !== undefined && this.props.teabag.id !== this.props.params.id) {
        this.props.requestTeabag(this.props.params.id);
      }
    } else if (this.props.params.id === undefined && this.props.teabag.id !== undefined && this.props.teabag.id.length > 0) {
      this.props.requestTeabag();
    }
  }

  componentWillReceiveProps(nextProps: TeabagProps) {
    if (nextProps.isLoading === false && nextProps.params && nextProps.params.id && nextProps.params.id.length > 0) {
      if (nextProps.teabag !== undefined && nextProps.teabag.id !== nextProps.params.id) {
        this.props.requestTeabag(nextProps.params.id);
      }
    } else if (nextProps.params.id === undefined && this.props.teabag.id !== undefined && this.props.teabag.id.length > 0) {
      this.props.requestTeabag();
    }
  }

  onAddNewBrand() {
    this.props.history.push('/tea/brandform');
  }

  onSearchBrand(searchTerm: string) {
    let uri = `/api/Tea/Brands/?searchterm=${encodeURIComponent(searchTerm)}`;
    return fetch(uri, { credentials: 'same-origin' })
            .then(response => response.json() as Promise<IBrand[]>);
  }

  onAddNewCountry() {
    this.props.history.push('/countryform');
  }

  onSearchCountry(searchTerm: string) {
    let uri = `/api/Tea/Countries/?searchterm=${encodeURIComponent(searchTerm)}`;
    return fetch(uri, { credentials: 'same-origin' })
            .then(response => response.json() as Promise<ICountry[]>);
  }

  onAddNewBagType() {
    this.props.history.push('/tea/bagtypeform');
  }

  onSearchType(searchTerm: string) {
    let uri = `/api/Tea/BagTypes/?searchterm=${encodeURIComponent(searchTerm)}`;
    return fetch(uri, { credentials: 'same-origin' })
            .then(response => response.json() as Promise<IBagType[]>);
  }

  onSave(event: React.MouseEvent<HTMLButtonElement>) {
    event.stopPropagation();
    event.preventDefault();
    this.props.saveTeabag();
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

  renderSaveButton() {
    if (this.props.teabag.iseditable === false) {
      return undefined;
    }

    return  <button className='btn btn-default' style={{cusor: 'pointer'}} onClick={this.onSave}>
              <span className='fa fa-floppy-o' aria-hidden='true'></span> Save
            </button>;
  }

  renderForm() {
    return  <form className='form-horizontal' style={{marginTop: 10}}>
              <div className='row'>
                <div className='col-sm-6 flex-first'>
                  <div className='form-group'>
                    <Image imageid={this.props.teabag.imageid}/>
                  </div>
                </div>
                <div className='col-sm-6 flex-last'>
                  <BrandInputGroupItem  inputid='inputBrand'
                                        responsiveInputComponentWidth='col-sm-12'
                                        label='Brand'
                                        onAddNew={this.props.teabag.brand && this.props.teabag.brand.canaddnew ? this.onAddNewBrand : undefined}
                                        onSearch={this.onSearchBrand}
                                        onItemSelected={this.props.changeBrand}
                                        onClear={this.props.clearBrand}
                                        renderItem={this.renderBrandComboBoxItem}
                                        displayProperty={'name'}
                                        selectedItem={this.props.teabag.brand}
                                        isReadOnly={this.props.teabag.iseditable === false} />
                  <InputFormGroupItem inputid='inputFlavour' responsiveInputComponentWidth='col-sm-12' label='Flavour' placeholder='Flavour' value={this.props.teabag.flavour} isReadOnly={this.props.teabag.iseditable === false} onChange={this.props.changeFlavour}/>
                  <InputFormGroupItem inputid='inputSeries' responsiveInputComponentWidth='col-sm-12' label='Series' placeholder='Series' value={this.props.teabag.serie} isReadOnly={this.props.teabag.iseditable === false} onChange={this.props.changeSerie}/>
                  <InputFormGroupItem inputid='inputSerialnumber' responsiveInputComponentWidth='col-sm-6' label='Serialnumber' placeholder='Serialnumber' value={this.props.teabag.serialnumber} isReadOnly={this.props.teabag.iseditable === false} onChange={this.props.changeSerialNumber}/>
                  <TextareaFormGroupItem inputid='inputHallmark' responsiveInputComponentWidth='col-sm-12' label='Hallmark' placeholder='Hallmark' value={this.props.teabag.hallmark} isReadOnly={this.props.teabag.iseditable === false} onChange={this.props.changeHallmark}/>
                  <BagtypeInputGroupItem  inputid='inputBagType'
                                          responsiveInputComponentWidth='col-sm-12'
                                          label='Type'
                                          onAddNew={this.props.teabag.bagtype && this.props.teabag.bagtype.canaddnew ? this.onAddNewBagType : undefined}
                                          onSearch={this.onSearchType}
                                          onItemSelected={this.props.changeBagtype}
                                          onClear={this.props.teabag.iseditable ? this.props.clearBagtype : undefined}
                                          renderItem={this.renderBagTypeComboBoxItem}
                                          displayProperty={'name'}
                                          selectedItem={this.props.teabag.bagtype}
                                        isReadOnly={this.props.teabag.iseditable === false} />
                  <CountryInputGroupItem  inputid='inputCountry'
                                          responsiveInputComponentWidth='col-sm-12'
                                          label='Country'
                                          onAddNew={this.props.teabag.country && this.props.teabag.country.canaddnew ? this.onAddNewCountry : undefined}
                                          onSearch={this.onSearchCountry}
                                          onItemSelected={this.props.changeCountry}
                                          onClear={this.props.teabag.iseditable ? this.props.clearCountry : undefined}
                                          renderItem={this.renderCountryComboBoxItem}
                                          displayProperty={'name'}
                                          selectedItem={this.props.teabag.country}
                                        isReadOnly={this.props.teabag.iseditable === false} />
                  <div className='form-group'>
                    <div className='col-sm-offset-2 col-sm-10'>
                      { this.renderSaveButton() }
                    </div>
                  </div>
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

export default withRouter(connect(
    (state: IApplicationState) => state.teabag, // selects which state properties are merged into the component's props
    TeabagReducer.actionCreators,               // selects which action creators are merged into the component's props
)(TeabagForm));
