import * as React from 'react';
import { withRouter, RouteComponentProps } from 'react-router';
import { connect, Dispatch } from 'react-redux';
import { History } from 'history';
import { IApplicationState }  from '../store';
import Loader from '../components/Loader';
import { addAction, requestAction, valueChangedAction } from '../actions/country';
import { ICountry } from '../interfaces/ICountry';

function mapDispatchToProps(dispatch: Dispatch) {
  return {
    onRequestCountry: (id?: string) => dispatch(requestAction(id)),
    onAdd: (country: ICountry) => dispatch(addAction(country)),
    onValueChanged: <K extends keyof ICountry>(property: K, value: ICountry[K]) => dispatch(valueChangedAction(property, value)),
  };
}

function mapStateToProps(state: IApplicationState) {
  return {...state.country, ...state.ui};
}

type CountryProps = ReturnType<typeof mapStateToProps> & ReturnType<typeof mapDispatchToProps> & RouteComponentProps<{id: string, history: History}>;

class CountryForm extends React.Component<CountryProps, {}> {

  constructor(props: CountryProps) {
    super(props);

    this.onNameChanged = this.onNameChanged.bind(this);
    this.onAdd = this.onAdd.bind(this);
    this.onUpdate = this.onUpdate.bind(this);
  }

  componentWillMount() {
    if (this.props.match.params && this.props.match.params.id && this.props.match.params.id.length > 0) {
      this.props.onRequestCountry(this.props.match.params.id);
    }
  }

  onNameChanged(event: React.FormEvent<HTMLInputElement>) {
    this.props.onValueChanged('name', event.currentTarget.value);
  }

  onAdd(event: React.MouseEvent<HTMLButtonElement>) {
    event.preventDefault();
    event.stopPropagation();
    this.props.onAdd(this.props.country);
    // this.props.history.goBack();
  }

  onUpdate(event: React.MouseEvent<HTMLButtonElement>) {
    event.preventDefault();
    event.stopPropagation();
    // this.props.addCountry(this.props.country);
    // this.props.history.goBack();
  }

  renderSaveButton() {
    if (this.props.country.id) {
      return  <button className='btn btn-default' onClick={this.onUpdate}>
                <span className='fa fa-floppy-o' aria-hidden='true'></span> Update
              </button>;
    }

    return  <button className='btn btn-default' onClick={this.onAdd}>
                <span className='fa fa-floppy-o' aria-hidden='true'></span> Add
              </button>;
  }

  renderForm() {
    return  <form className='form-horizontal' style={{marginTop: 10}}>
              <div className='form-group'>
                <label htmlFor='inputName' className='col-sm-2 control-label'>Name</label>
                <div className='col-sm-6'>
                  <input type='text' className='form-control' id='inputName' placeholder='Name' value={this.props.country.name} onChange={this.onNameChanged} />
                </div>
              </div>
              <div className='form-group'>
                <div className='col-sm-offset-2 col-sm-10'>
                  {this.renderSaveButton()}
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
  mapStateToProps, // selects which state properties are merged into the component's props
  mapDispatchToProps,                // selects which action creators are merged into the component's props
)(CountryForm));

