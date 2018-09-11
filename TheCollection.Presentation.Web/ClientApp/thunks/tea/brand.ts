import { fetch, addTask } from 'domain-task';
import { routerActions, RouterAction } from 'react-router-redux';
import { AppThunkAction } from '../../store';
import { IBrand } from '../../interfaces/tea/IBrand';
import { BrandActionTypes, ChangeNameAction, ReceiveBrandAction, RequestBrandAction } from '../../actions/tea/brand';

import { BagActionTypes, ChangeBrandAction } from '../../actions/tea/bag';

export const requestBrand = {
   requestBrand: (brandid?: string): AppThunkAction<ReceiveBrandAction | RequestBrandAction> => (dispatch, getState) => {
    if (brandid === undefined) {
      dispatch({ type: BrandActionTypes.Recieve, brand: {} as IBrand });
      return;
    }

    try {
      let fetchTask = fetch(`/api/Tea/Brands/${brandid}`, { credentials: 'same-origin' })
        .then(response => response.json() as Promise<IBrand>)
        .then(data => {
          dispatch({ type: BrandActionTypes.Recieve, brand: data });
          addTask(fetchTask); // ensure server-side prerendering waits for this to complete
      });
    } catch (err) {
      dispatch({ type: BrandActionTypes.Recieve, brand: {} as IBrand });
    }

    dispatch({ type: BrandActionTypes.Request, brandid: brandid });
  },
 };

export const addBrand = {
  addBrand: (brand: IBrand): AppThunkAction<ChangeBrandAction | RouterAction> => (dispatch, getState) => {
    try {
      let addBrandTask = fetch(`/api/Tea/Brands/`, {
          headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
          },
          credentials: 'same-origin',
          method: 'post',
          body: JSON.stringify(brand),
        })
        .then(response => response.json() as Promise<IBrand>)
        .then(data => {
          dispatch({type: BagActionTypes.ChangeBrand, brand: {...data, ...{canaddnew: true}}});
          dispatch(routerActions.goBack());
          addTask(addBrandTask); // ensure server-side prerendering waits for this to complete
      });
    } catch (err) {
      console.log(err);
    }
  },
 };

export const changeName = {
  changeName: (name: string): AppThunkAction<ChangeNameAction> => (dispatch, getState) => {
    dispatch({type: BrandActionTypes.ChangeName, name: name});
  },
 };
