import * as React from 'react';

type TextInputProps = {
  inputid: string;
  value: string;
  onChange: (newValue: string) => void;
  placeholder?: string;
}

export class TextInput extends React.Component<TextInputProps, {}> {

  constructor(props: TextInputProps) {
    super(props);

    this.onChange = this.onChange.bind(this);
  }

  getBindValue(value: string) {
    if (value === undefined || value === null) { return ''; }

    return value;
  }

  onChange(event: React.FormEvent<HTMLInputElement>) {
    this.props.onChange(event.currentTarget.value);
  }

  render() {
    return <input type='text' className='form-control' id={this.props.inputid} placeholder={this.props.placeholder} value={this.getBindValue(this.props.value)} onChange={this.onChange}/>;
  }
}
