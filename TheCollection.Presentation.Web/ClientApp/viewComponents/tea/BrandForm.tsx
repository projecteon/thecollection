import * as React from 'react';
import { withRouter, RouteComponentProps } from 'react-router';
import { connect } from 'react-redux';
import { History } from 'history';
import { IApplicationState }  from '../../store';
import * as BrandReducer from '../../reducers/tea/brand';
import Loader from '../../components/Loader';

type BrandProps =
    BrandReducer.IBrandState     // ... state we've requested from the Redux store
    & typeof BrandReducer.actionCreators   // ... plus action creators we've requested
    & { params?: { id?: string } }        // ... plus incoming routing parameters
    & { history: History; };              // ... plus naviation through react router

class BrandForm extends React.Component<BrandProps, {}> {

  constructor(props: BrandProps) {
    super(props);

    this.onNameChanged = this.onNameChanged.bind(this);
    this.onAdd = this.onAdd.bind(this);
    this.onUpdate = this.onUpdate.bind(this);
  }

  componentWillMount() {
    if (this.props.params && this.props.params.id && this.props.params.id.length > 0) {
      this.props.requestBrand(this.props.params.id);
    }
  }

  componentWillReceiveProps(nextProps: BrandProps) {
    if (this.props.isLoading === false && this.props.params && this.props.params.id && this.props.params.id.length > 0 && this.props.brand.id !== this.props.params.id) {
      this.props.requestBrand(this.props.params.id);
    }
  }

  onNameChanged(event: React.FormEvent<HTMLInputElement>) {
    this.props.changeName(event.currentTarget.value);
  }

  onAdd(event: React.MouseEvent<HTMLButtonElement>) {
    event.preventDefault();
    event.stopPropagation();
    this.props.addBrand(this.props.brand);
    // this.props.history.goBack();
  }

  onUpdate(event: React.MouseEvent<HTMLButtonElement>) {
    event.preventDefault();
    event.stopPropagation();
    // this.props.addBrand(this.props.brand);
    // this.props.history.goBack();
  }

  renderSaveButton() {
    if (this.props.brand.id) {
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
                  <input type='text' className='form-control' id='inputName' placeholder='Name' value={this.props.brand.name} onChange={this.onNameChanged} />
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
    (state: IApplicationState, routerProps: RouteComponentProps<{id: string}>) => state.brand, // selects which state properties are merged into the component's props
    BrandReducer.actionCreators,               // selects which action creators are merged into the component's props
)(BrandForm));

