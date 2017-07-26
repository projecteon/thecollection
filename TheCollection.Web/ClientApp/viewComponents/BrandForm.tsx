import * as React from 'react';
import { Link } from 'react-router';
import { connect } from 'react-redux';
import { IApplicationState }  from '../store';
import * as BrandReducer from '../reducers/brand';
import {Loader} from '../components/Loader';
import {IBrand} from '../interfaces/IBrand';

type BrandProps =
    BrandReducer.IBrandState     // ... state we've requested from the Redux store
    & typeof BrandReducer.actionCreators   // ... plus action creators we've requested
    & { params?: { id?: string } };       // ... plus incoming routing parameters

class BrandForm extends React.Component<BrandProps, void> {

  constructor() {
    super();
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

  renderForm() {
    return  <form className='form-horizontal' style={{marginTop: 10}}>
              <div className='form-group'>
                <label htmlFor='inputName' className='col-sm-2 control-label'>Name</label>
                <div className='col-sm-6'>
                  <input type='text' className='form-control' id='inputName' placeholder='Name' value={this.props.brand.name} />
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
    (state: IApplicationState) => state.brand, // selects which state properties are merged into the component's props
    BrandReducer.actionCreators                 // selects which action creators are merged into the component's props
)(BrandForm);

