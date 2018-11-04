import * as React from 'react';
import { withRouter, RouteComponentProps } from 'react-router';
import { connect, Dispatch } from 'react-redux';
import { History } from 'history';
import { IApplicationState }  from '../../store';
import Loader from '../../components/Loader';
import { IBrand } from '../../interfaces/tea/IBrand';
import { addAction, requestAction, valueChangedAction } from '../../actions/tea/brand';

function mapDispatchToProps(dispatch: Dispatch) {
  return {
    onRequestBrand: (id: string) => dispatch(requestAction(id)),
    onAdd: (brand: IBrand) => dispatch(addAction(brand)),
    onValueChanged: <K extends keyof IBrand>(property: K, value: IBrand[K]) => dispatch(valueChangedAction(property, value)),
  };
}

function mapStateToProps(state: IApplicationState) {
  return {...state.brand, ...state.ui};
}

type BrandProps = ReturnType<typeof mapStateToProps> & ReturnType<typeof mapDispatchToProps> & RouteComponentProps<{id: string, history: History}>;

class BrandForm extends React.Component<BrandProps, {}> {

  constructor(props: BrandProps) {
    super(props);

    this.onNameChanged = this.onNameChanged.bind(this);
    this.onAdd = this.onAdd.bind(this);
    this.onUpdate = this.onUpdate.bind(this);
  }

  componentWillMount() {
    if (this.props.match.params && this.props.match.params.id && this.props.match.params.id.length > 0) {
      this.props.onRequestBrand(this.props.match.params.id);
    }
  }

  onNameChanged(event: React.FormEvent<HTMLInputElement>) {
    this.props.onValueChanged('name', event.currentTarget.value);
  }

  onAdd(event: React.MouseEvent<HTMLButtonElement>) {
    event.preventDefault();
    event.stopPropagation();
    this.props.onAdd(this.props.brand);
    // this.props.history.goBack();
  }

  onUpdate(event: React.MouseEvent<HTMLButtonElement>) {
    event.preventDefault();
    event.stopPropagation();
    this.props.onAdd(this.props.brand);
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
  mapStateToProps, // selects which state properties are merged into the component's props
  mapDispatchToProps,               // selects which action creators are merged into the component's props
)(BrandForm));

