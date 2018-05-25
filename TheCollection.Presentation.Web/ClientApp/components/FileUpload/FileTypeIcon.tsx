import * as React from 'react';

type FileTypeIconProps = {
  filetype: string;
};

const FileTypeIcon: React.StatelessComponent<FileTypeIconProps> = props => {
  const filetype = '.' + props.filetype;
  return <div className='file-icon' data-filetype={filetype}></div>;
}

export default FileTypeIcon;
