import * as React from 'react';
import {Component, ComponentClass, StatelessComponent} from 'react';

type FormGroupItemProps = {
  inputid: string;
  label: string;
  responsiveInputComponentWidth: string;
}

export function FormGroupItem<T>(InputComponent: ComponentClass<T> | StatelessComponent<T>): ComponentClass<T & FormGroupItemProps> {
  return class extends Component<T & FormGroupItemProps, {}> {
   render() {
    return (
      <div className='form-group'>
        <label htmlFor={this.props.inputid} className='col-sm-2 control-label'>{this.props.label}</label>
        <div className={this.props.responsiveInputComponentWidth}>
          <InputComponent {...this.props}/>
        </div>
      </div>);
    }
  }
}
