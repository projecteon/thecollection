import * as React from 'react';
import {Component, ComponentClass} from 'react';

export function PopupHOC<T>(PopupComponent: ComponentClass<T>): ComponentClass<T> {
  return class extends Component<T, {}> {
   render() {
    return (
      <div style={{display: 'flex', flexDirection: 'row', alignItems: 'center', justifyContent: 'center',
                  position: 'fixed', top: 0, left: 0, width: '100%', height: '100%', zIndex: 1020, backgroundColor: 'rgba(255, 255, 255, 0.7)' }}
          onClick={(e) => { e.stopPropagation(); }}>
        <div style={{position: 'absolute', top: '30%', left: '50%', transform: 'translate(-50%,-50%)'}}>
          <div style={{backgroundColor: '#fff', borderRadius: 5, padding: 5, minWidth: 200, minHeight: 50}}>
            <PopupComponent {...this.props}/>
          </div>
        </div>
      </div>);
    }
  }
}
