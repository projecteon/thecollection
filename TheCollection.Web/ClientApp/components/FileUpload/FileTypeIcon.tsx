import * as React from 'react';

export type FileTypeIconProps = {
  filetype: string;
};

export default class FileTypeIcon extends React.Component<FileTypeIconProps, {}> {

  constructor(props: FileTypeIconProps) {
    super(props);
  }

  render() {
    let filetype = '.' + this.props.filetype;
    return  <div className='file-icon' data-filetype={filetype}></div>;
  }
}
