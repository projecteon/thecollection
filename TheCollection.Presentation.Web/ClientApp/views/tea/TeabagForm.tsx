import * as React from 'react';
import { withRouter, RouteComponentProps } from 'react-router';
import { connect, Dispatch } from 'react-redux';
import { History } from 'history';
import { IApplicationState }  from '../../store';

import {ComboBox} from '../../components/ComboBox/ComboBox';
import Loader from '../../components/Loader';
import {FormGroupItem} from '../../components/FormGroupItem';
import TextInput from '../../components/TextInput';
import Textarea from '../../components/Textarea';
import {Image} from '../../components/Image';

import {IBrand} from '../../interfaces/tea/IBrand';
import {ICountry} from '../../interfaces/ICountry';
import {IBagType} from '../../interfaces/tea/IBagType';
import { ITeabag, PropNames } from '../../interfaces/tea/IBag';
import { requestAction, saveAction, valueChangedAction, clearRefValueAction, clearStringValueAction } from '../../actions/tea/bag';
import { RefValuePropertyNames, StringValuePropertyNames } from '../../types';
import { ISearchResult } from '../../interfaces/ISearchResult';

const InputFormGroupItem = FormGroupItem(TextInput); // tslint:disable-line:variable-name
const TextareaFormGroupItem = FormGroupItem(Textarea); // tslint:disable-line:variable-name
const BrandInputGroupItem = FormGroupItem(ComboBox<IBrand>()); // tslint:disable-line:variable-name
const CountryInputGroupItem = FormGroupItem(ComboBox<ICountry>()); // tslint:disable-line:variable-name
const BagtypeInputGroupItem = FormGroupItem(ComboBox<IBagType>()); // tslint:disable-line:variable-name

function mapDispatchToProps(dispatch: Dispatch) {
  return {
    onClearRefValue: (property: RefValuePropertyNames<ITeabag>) => dispatch(clearRefValueAction(property)),
    onClearStringValue: (property: StringValuePropertyNames<ITeabag>) => dispatch(clearStringValueAction(property)),
    onRequestTeabag: (id?: string) => dispatch(requestAction(id)),
    onSave: (teabag: ITeabag) => dispatch(saveAction(teabag)),
    onValueChanged: <K extends keyof ITeabag>(property: K, value: ITeabag[K]) => dispatch(valueChangedAction(property, value)),
  };
}

function mapStateToProps(state: IApplicationState) {
  return {...state.teabag, ...state.ui};
}

type TeabagProps = ReturnType<typeof mapStateToProps> & ReturnType<typeof mapDispatchToProps> & RouteComponentProps<{id: string, history: History}>;

class TeabagForm extends React.Component<TeabagProps, {}> {

  constructor(props: TeabagProps) {
    super(props);
    this.onAddNewBagType = this.onAddNewBagType.bind(this);
    this.onAddNewBrand = this.onAddNewBrand.bind(this);
    this.onAddNewCountry = this.onAddNewCountry.bind(this);
    this.onSearchCountry = this.onSearchCountry.bind(this);
    this.onSearchType = this.onSearchType.bind(this);
    this.onSave = this.onSave.bind(this);

    this.onBagTypeChanged = this.onBagTypeChanged.bind(this);
    this.onBrandChanged = this.onBrandChanged.bind(this);
    this.onCountryChanged = this.onCountryChanged.bind(this);
  }

  componentWillMount() {
    if (this.props.match.params && this.props.match.params.id && this.props.match.params.id.length > 0) {
      if (this.props.teabag !== undefined && this.props.teabag.id !== this.props.match.params.id) {
        console.log(this.props.match.params.id);
        this.props.onRequestTeabag(this.props.match.params.id);
      }
    } else if ((this.props.match.params === undefined || this.props.match.params.id === undefined) && this.props.teabag.id !== undefined && this.props.teabag.id.length > 0) {
      this.props.onRequestTeabag();
    }
  }

  componentWillReceiveProps(nextProps: TeabagProps) {
    if (nextProps.isLoading === false && nextProps.match.params && nextProps.match.params.id && nextProps.match.params.id.length > 0) {
      if (nextProps.teabag !== undefined && nextProps.teabag.id !== nextProps.match.params.id) {
        this.props.onRequestTeabag(nextProps.match.params.id);
      }
    } else if ((this.props.match.params === undefined || this.props.match.params.id === undefined) && this.props.teabag.id !== undefined && this.props.teabag.id.length > 0) {
      this.props.onRequestTeabag();
    }
  }

  onAddNewBrand() {
    this.props.history.push('/tea/brandform');
  }

  onSearchBrand(searchTerm: string) {
    let uri = `/api/Tea/Brands/?searchterm=${encodeURIComponent(searchTerm)}`;
    return fetch(uri, { credentials: 'same-origin' })
            .then(response => response.json() as Promise<ISearchResult<IBrand>>)
            .then(data => Promise.resolve(data.data));
  }

  onAddNewCountry() {
    this.props.history.push('/countryform');
  }

  onSearchCountry(searchTerm: string) {
    let uri = `/api/Tea/Countries/?searchterm=${encodeURIComponent(searchTerm)}`;
    return fetch(uri, { credentials: 'same-origin' })
            .then(response => response.json() as Promise<ISearchResult<ICountry>>)
            .then(data => Promise.resolve(data.data));
  }

  onAddNewBagType() {
    this.props.history.push('/tea/bagtypeform');
  }

  onSearchType(searchTerm: string) {
    let uri = `/api/Tea/BagTypes/?searchterm=${encodeURIComponent(searchTerm)}`;
    return fetch(uri, { credentials: 'same-origin' })
            .then(response => response.json() as Promise<ISearchResult<IBagType>>)
            .then(data => Promise.resolve(data.data));
  }

  onSave(event: React.MouseEvent<HTMLButtonElement>) {
    event.stopPropagation();
    event.preventDefault();
    this.props.onSave(this.props.teabag);
  }

  onBagTypeChanged(bagType: IBagType) {
    this.props.onValueChanged(PropNames.bagType, {...bagType, ...{canaddnew: this.props.teabag.bagType.canaddnew}});
  }

  onBrandChanged(brand: IBrand) {
    this.props.onValueChanged(PropNames.brand, {...brand, ...{canaddnew: this.props.teabag.brand.canaddnew}});
  }

  onCountryChanged(country: ICountry) {
    this.props.onValueChanged(PropNames.country, {...country, ...{canaddnew: this.props.teabag.country.canaddnew}});
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

    return  <button className='btn btn-default' style={{cursor: 'pointer'}} onClick={this.onSave}>
              <span className='fa fa-floppy-o' aria-hidden='true'></span> Save
            </button>;
  }

  renderForm() {
    return  <form className='form-horizontal' style={{marginTop: 10}}>
              <div className='row'>
                <div className='col-sm-6 flex-first'>
                  <div className='form-group'>
                    <Image imageid={this.props.teabag.imageId}/>
                  </div>
                </div>
                <div className='col-sm-6 flex-last'>
                  <BrandInputGroupItem  inputid='inputBrand'
                                        responsiveInputComponentWidth='col-sm-12'
                                        label='Brand'
                                        onAddNew={this.props.teabag.brand && this.props.teabag.brand.canaddnew ? this.onAddNewBrand : undefined}
                                        onSearch={this.onSearchBrand}
                                        onItemSelected={this.onBrandChanged}
                                        onClear={() => this.props.onClearRefValue(PropNames.brand)}
                                        renderItem={this.renderBrandComboBoxItem}
                                        displayProperty={'name'}
                                        selectedItem={this.props.teabag.brand}
                                        isReadOnly={this.props.teabag.iseditable === false} />
                  <InputFormGroupItem inputid='inputFlavour' responsiveInputComponentWidth='col-sm-12' label='Flavour' placeholder='Flavour' value={this.props.teabag.flavour} isReadOnly={this.props.teabag.iseditable === false} onChange={(newValue: string) => this.props.onValueChanged(PropNames.flavour, newValue)}/>
                  <InputFormGroupItem inputid='inputSeries' responsiveInputComponentWidth='col-sm-12' label='Series' placeholder='Series' value={this.props.teabag.serie} isReadOnly={this.props.teabag.iseditable === false} onChange={(newValue: string) => this.props.onValueChanged(PropNames.serie, newValue)}/>
                  <InputFormGroupItem inputid='inputSerialnumber' responsiveInputComponentWidth='col-sm-6' label='Serialnumber' placeholder='Serialnumber' value={this.props.teabag.serialNumber} isReadOnly={this.props.teabag.iseditable === false} onChange={(newValue: string) => this.props.onValueChanged(PropNames.serialNumber, newValue)}/>
                  <TextareaFormGroupItem inputid='inputHallmark' responsiveInputComponentWidth='col-sm-12' label='Hallmark' placeholder='Hallmark' value={this.props.teabag.hallmark} isReadOnly={this.props.teabag.iseditable === false} onChange={(newValue: string) => this.props.onValueChanged(PropNames.hallmark, newValue)}/>
                  <BagtypeInputGroupItem  inputid='inputBagType'
                                          responsiveInputComponentWidth='col-sm-12'
                                          label='Type'
                                          onAddNew={this.props.teabag.bagType && this.props.teabag.bagType.canaddnew ? this.onAddNewBagType : undefined}
                                          onSearch={this.onSearchType}
                                          onItemSelected={this.onBagTypeChanged}
                                          onClear={() => this.props.onClearRefValue(PropNames.bagType)}
                                          renderItem={this.renderBagTypeComboBoxItem}
                                          displayProperty={'name'}
                                          selectedItem={this.props.teabag.bagType}
                                          isReadOnly={this.props.teabag.iseditable === false} />
                  <CountryInputGroupItem  inputid='inputCountry'
                                          responsiveInputComponentWidth='col-sm-12'
                                          label='Country'
                                          onAddNew={this.props.teabag.country && this.props.teabag.country.canaddnew ? this.onAddNewCountry : undefined}
                                          onSearch={this.onSearchCountry}
                                          onItemSelected={this.onCountryChanged}
                                          onClear={() => this.props.onClearRefValue(PropNames.country)}
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
  mapStateToProps,
  mapDispatchToProps,               // selects which action creators are merged into the component's props
)(TeabagForm));

