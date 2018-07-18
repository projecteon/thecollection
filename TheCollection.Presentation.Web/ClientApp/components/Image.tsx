import * as React from 'react';

type ImageProps = {
  imageid: string;
}

// tslint:disable-next-line:variable-name
export const Image: React.StatelessComponent<ImageProps> = props => {

  const hasValidImageId = () => {
    return props.imageid !== undefined && props.imageid.length > 0;
  };

  const renderImage = () => {
    return <img src={`/images/${props.imageid}/teabag.png`}  style={{maxWidth: '100%', cursor: 'ponter'}} />;
  };

  const renderPlaceholder = () => {
    return <div />;
  };


  return  <div>
            {hasValidImageId() ? renderImage() : renderPlaceholder()}
          </div>;
};
