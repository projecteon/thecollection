import * as React from 'react';
import { Link } from 'react-router';
import { ITeabag } from '../interfaces/tea/IBag';

import './TeabagCard.scss';

type TeabagCardProps = {
  teabag: ITeabag;
  onZoomClicked(imageId?: string): void;
};

// tslint:disable-next-line:variable-name
const TeabagCard: React.StatelessComponent<TeabagCardProps> = props => {
  return  <div style={{padding: 10, boxSizing: 'border-box', position: 'relative'}} className='col-xs-12 col-sm-4 col-md-3 col-lg-2'>
            <div className='card'>
              {props.teabag.imageid ? <div style={{position: 'relative'}} onClick={e => { props.onZoomClicked(props.teabag.imageid); }}><img src={`/thumbnails/${props.teabag.imageid}/teabag.png`} style={{width: '100%', cursor: 'pointer'}}/><p className='edit'><i className='text-info fa fa-search'/></p></div> : undefined}
              <div style={{padding: '5px 10px', backgroundColor: '#fff', width: '100%', position: 'relative', minHeight: 55}}>
                <strong style={{display: 'block', width: '100%', borderTop: '1px solid #959595', overflow: 'hidden'}}>{props.teabag.brand.name} - {props.teabag.flavour}</strong>
                <div style={{color: '#959595'}}><small>{props.teabag.serie}</small></div>
                <div style={{color: '#959595'}}><small>{props.teabag.hallmark}</small></div>
                <div style={{color: '#959595'}}><small>{props.teabag.serialnumber}</small></div>
                <div style={{color: '#959595'}}><small>{props.teabag.bagtype ? props.teabag.bagtype.name : ''}</small></div>
                <div style={{color: '#959595'}}><small>{props.teabag.country ? props.teabag.country.name : ''}</small></div>
                <p className='edit'>
                  <Link to={ `/tea/teabagform/${props.teabag.id}` } activeClassName='active' className='text-info'>
                    <span className='fa fa-pencil-square-o'></span>
                  </Link>
                </p>
              </div>
            </div>
          </div>;
};

export default TeabagCard;
