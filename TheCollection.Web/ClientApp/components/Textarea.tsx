import * as React from 'react';

type TextareaProps = {
  inputid: string;
  value: string;
  isReadOnly: boolean;
  onChange: (newValue: string) => void;
  placeholder?: string;
};

// tslint:disable-next-line:variable-name
const Textarea: React.StatelessComponent<TextareaProps> = props => {
  const getBindValue = (value: string) => {
    if (value === undefined || value === null) { return ''; }

    return value;
  };

  const onChange = (event: React.FormEvent<HTMLTextAreaElement>) => {
    props.onChange(event.currentTarget.value);
  };

  return <textarea className='form-control' id={props.inputid} placeholder={props.placeholder} value={getBindValue(props.value)} disabled={props.isReadOnly} onChange={onChange}/>;
};

export default Textarea;
