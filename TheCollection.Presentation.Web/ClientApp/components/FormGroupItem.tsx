import * as React from 'react';
import {Component, ComponentClass, StatelessComponent} from 'react';

type FormGroupItemProps = {
  inputid: string;
  label: string;
  responsiveInputComponentWidth: string;
};

// tslint:disable-next-line:variable-name
export function FormGroupItem<T>(InputComponent: ComponentClass<T> | StatelessComponent<T>): ComponentClass<T & FormGroupItemProps> {
  return class extends Component<T & FormGroupItemProps, {}> {
    render() {
      const { inputid, label, responsiveInputComponentWidth, ...props } = this.props as FormGroupItemProps;
      return (
        <div className='form-group'>
          <label htmlFor={inputid} className='col-sm-2 control-label'>{label}</label>
          <div className={responsiveInputComponentWidth}>
            <InputComponent {...props}/>
          </div>
        </div>);
    }
  };
}
