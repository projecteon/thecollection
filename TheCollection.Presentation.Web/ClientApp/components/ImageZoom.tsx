import * as React from 'react';

import './ImageZoom.scss';

export interface IImageZoomProps extends React.Props<any> {
  imageid: string;
  onClick(): void;
}

export interface IImageZoomState {
  imageLoadFailed: boolean;
}

export class ImageZoom extends React.Component<IImageZoomProps, IImageZoomState> {

  constructor(props: IImageZoomProps) {
    super(props);
    this.onImageLoadError = this.onImageLoadError.bind(this);

    this.state = {imageLoadFailed: false};
  }

  onImageLoadError(event: React.SyntheticEvent<HTMLImageElement>) {
    this.setState({imageLoadFailed: true});
  }

  renderIcon() {
    return <div style={{backgroundColor: '#ffffff', padding: 20}}><i className='fa fa-ban' style={{fontSize: '4em', color: '#000000'}}/></div>;
  }

  renderImage() {
    return  <div className='image' onClick={this.props.onClick}>
              <p className='overlay'><i className='text-danger fa fa-times'/></p>
              <img src={`/images/${this.props.imageid}/Image.png`} style={{maxWidth: '90vw', maxHeight: '95vh', cursor: 'pointer'}}/>
            </div>;
  }

  render() {
    let element = this.state.imageLoadFailed ? this.renderIcon() : this.renderImage();
    return  <div style={{zIndex: 10, position: 'fixed', top: 0, left: 0, width: '100%', height: '100%', display: 'flex', justifyContent: 'center', alignItems: 'center', backgroundColor: 'rgba(0, 0, 0, 0.7)'}}>
              {element}
            </div>;
  }
}
