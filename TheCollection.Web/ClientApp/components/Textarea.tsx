import * as React from 'react';

type TextareaProps = {
  inputid: string;
  value: string;
  onChange: (newValue: string) => void;
  placeholder?: string;
}

export class Textarea extends React.Component<TextareaProps, {}> {

  constructor(props: TextareaProps) {
    super(props);

    this.onChange = this.onChange.bind(this);
  }

  getBindValue(value: string) {
    if (value === undefined || value === null) { return ''; }

    return value;
  }

  onChange(event: React.FormEvent<HTMLTextAreaElement>) {
    this.props.onChange(event.currentTarget.value);
  }

  render() {
    return <textarea className='form-control' id={this.props.inputid} placeholder={this.props.placeholder} value={this.getBindValue(this.props.value)} onChange={this.onChange}/>;
  }
}
