import * as React from 'react';
import {  withRouter, RouteComponentProps } from 'react-router';
import { connect, Dispatch } from 'react-redux';
import { History } from 'history';
import { IApplicationState }  from '../../store';
import Loader from '../../components/Loader';
import { IBagType } from '../../interfaces/tea/IBagType';
import { addAction, valueChangedAction, requestAction } from '../../actions/tea/bagtype';

function mapDispatchToProps(dispatch: Dispatch) {
  return {
    onRequestBagType: (id: string) => dispatch(requestAction(id)),
    onAdd: (bagType: IBagType) => dispatch(addAction(bagType)),
    onValueChanged: <K extends keyof IBagType>(property: K, value: IBagType[K]) => dispatch(valueChangedAction(property, value)),
  };
}

function mapStateToProps(state: IApplicationState) {
  return {...state.bagtype, ...state.ui};
}

type BagtypeProps = ReturnType<typeof mapStateToProps> & ReturnType<typeof mapDispatchToProps> & RouteComponentProps<{id: string, history: History}>;

class BagtypeForm extends React.Component<BagtypeProps, {}> {

  constructor(props: BagtypeProps) {
    super(props);

    this.onNameChanged = this.onNameChanged.bind(this);
    this.onAdd = this.onAdd.bind(this);
    this.onUpdate = this.onUpdate.bind(this);
  }

  componentWillMount() {
    if (this.props.match.params && this.props.match.params.id && this.props.match.params.id.length > 0) {
      this.props.onRequestBagType(this.props.match.params.id);
    }
  }

  onNameChanged(event: React.FormEvent<HTMLInputElement>) {
    this.props.onValueChanged('name', event.currentTarget.value);
  }

  onAdd(event: React.MouseEvent<HTMLButtonElement>) {
    event.preventDefault();
    event.stopPropagation();
    this.props.onAdd(this.props.bagtype);
    // this.props.history.goBack();
  }

  onUpdate(event: React.MouseEvent<HTMLButtonElement>) {
    event.preventDefault();
    event.stopPropagation();
    this.props.onAdd(this.props.bagtype);
    // this.props.history.goBack();
  }

  renderSaveButton() {
    if (this.props.bagtype.id) {
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
                  <input type='text' className='form-control' id='inputName' placeholder='Name' value={this.props.bagtype.name} onChange={this.onNameChanged} />
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
  mapStateToProps,    // selects which state properties are merged into the component's props
  mapDispatchToProps, // selects which action creators are merged into the component's props
)(BagtypeForm));

