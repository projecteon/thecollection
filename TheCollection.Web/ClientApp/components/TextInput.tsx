import * as React from 'react';

type TextInputProps = {
  inputid: string;
  value: string;
  onChange: (newValue: string) => void;
  placeholder?: string;
}

const TextInput: React.StatelessComponent<TextInputProps> = props => {
  const getBindValue = (value: string) => {
    if (value === undefined || value === null) { return ''; }

    return value;
  }

  const onChange = (event: React.FormEvent<HTMLInputElement>) => {
    props.onChange(event.currentTarget.value);
  }

  return <input type='text' className='form-control' id={props.inputid} placeholder={props.placeholder} value={getBindValue(props.value)} onChange={onChange}/>;
}

export default TextInput;
