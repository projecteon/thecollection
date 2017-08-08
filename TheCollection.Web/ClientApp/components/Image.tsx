import * as React from 'react';

export type ImageProps = {
  imageid: string;
}

export class Image extends React.Component<ImageProps, {}> {
  constructor(props: ImageProps) {
    super();
  }

  hasValidImageId() {
    return this.props.imageid !== undefined && this.props.imageid.length > 0;
  }

  renderImage() {
    return <img src={`/images/${this.props.imageid}/teabag.png`}  style={{maxWidth: 'calc(100vw - 30px)', maxHeight: '95vh', cursor: 'ponter'}} />;
  }

  renderPlaceholder() {
    return <div />
  }

  render() {
    return  <div>
              {this.hasValidImageId() ? this.renderImage() : this.renderPlaceholder()}
            </div>;
  }
}
