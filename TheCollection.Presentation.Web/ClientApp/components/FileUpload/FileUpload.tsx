import * as React from 'react';
import classNames from 'classnames';
import FileTypeIcon from './FileTypeIcon';
import * as Util from '../../util/FileUpload';
import {FILE_TYPE} from '../../enums/FileTypes';

import './FileUpload.scss';

export type FileUploadProps = {
  filetypes: FILE_TYPE[];
  allowMultiple: boolean;
  onDrop(files: FileList): void;
};

export type FileUploadState = {
  draggedCount: number;
  fileNames?: string[];
};

export default class FileUpload extends React.Component<FileUploadProps, FileUploadState> {
  public state: FileUploadState;

  controls: {
    fileinput?: HTMLInputElement | null;
  } = {};

  constructor(props: FileUploadProps) {
    super(props);
    this.state = {
      draggedCount: 0,
    };

    this.onDragStart = this.onDragStart.bind(this);
    this.onDragEnter = this.onDragEnter.bind(this);
    this.onDragOver = this.onDragOver.bind(this);
    this.onDrag = this.onDrag.bind(this);
    this.OnDragLeave = this.OnDragLeave.bind(this);
    this.onDragExit = this.onDragExit.bind(this);
    this.OnDragEnd = this.OnDragEnd.bind(this);
    this.onDrop = this.onDrop.bind(this);
    this.onFileInput = this.onFileInput.bind(this);
  }

  private createAcceptAttribute(): string {
    if (this.props.filetypes.length > 0) {
      let filetypes = Util.FileUpload.getUniqueFileTypeExtensions(this.props.filetypes);
      let acceptTypes = filetypes.concat(Util.FileUpload.getMimeTypes(this.props.filetypes));
      return acceptTypes.join(',');
    }

    return '*.*';
  }

  private onDrop(e: any) {
    e.stopPropagation();
    e.preventDefault();
    const droppedFiles: FileList = e.dataTransfer ? e.dataTransfer.files : e.target.files as FileList;
    this.setState({ draggedCount: 0 });
    this.props.onDrop(droppedFiles);
  }

  private onDragStart(e: React.DragEvent<HTMLDivElement>) {
    e.stopPropagation();
    e.preventDefault();
  }

  private onDragEnter(e: React.DragEvent<HTMLDivElement>) {
    this.setState({ draggedCount: this.state.draggedCount + 1 });
  }

  private onDragOver(e: React.DragEvent<HTMLDivElement>) {
    e.stopPropagation();
    e.preventDefault();
    e.dataTransfer.dropEffect = 'copy';
  }

  private onDrag(e: React.DragEvent<HTMLDivElement>) {
    e.stopPropagation();
    e.preventDefault();
  }

  private OnDragLeave(e: React.DragEvent<HTMLDivElement>) {
    this.setState({ draggedCount: this.state.draggedCount - 1 });
  }

  private onDragExit(e: React.DragEvent<HTMLDivElement>) {
    e.stopPropagation();
    e.preventDefault();

  }

  private OnDragEnd(e: React.DragEvent<HTMLDivElement>) {
    e.stopPropagation();
    e.preventDefault();
  }

  private onFileInput() {
    if (this.controls.fileinput) {
      this.controls.fileinput.click();
    }
  }

  private renderFileIcons() {
    if (this.props.filetypes.length) {
      return undefined;
    }

    let filetypes = Util.FileUpload.getUniqueFileTypeExtensions(this.props.filetypes);
    let icons = filetypes.map(function (filetype, index) {
      return <FileTypeIcon key={index}
                          filetype={filetype} />;
    });

    return <div className='valid-file-icons'>
             {icons}
           </div>;
  }

  render() {
    let className = classNames('filepicker', {dragHover: this.state.draggedCount > 0});
    return  <div className={className}
                 onDragStart={this.onDragStart}
                 onDragEnter={this.onDragEnter}
                 onDragOver={this.onDragOver}
                 onDrag={this.onDrag}
                 onDragLeave={this.OnDragLeave}
                 onDragExit={this.onDragExit}
                 onDragEnd={this.OnDragEnd}
                 onDrop={this.onDrop}
                 onClick={this.onFileInput}>
              {this.renderFileIcons()}
              <i className='fa fa-upload' role='button' title='File chooser button' style={{ fontSize: '1.3em' }} />
              <input style={{visibility: 'hidden', position: 'absolute', top: 0, left: 0, height: 0, width: 0}} ref={(input) => this.controls.fileinput = input} multiple={this.props.allowMultiple} type='file' onChange={this.onDrop} accept={this.createAcceptAttribute()}/>
            </div>;
  }
}
