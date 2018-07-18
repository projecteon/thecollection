import { fetch, addTask } from 'domain-task';
import { routerActions, RouterAction } from 'react-router-redux';
import { AppThunkAction } from '../../store';
import { IBrand } from '../../interfaces/tea/IBrand';
import { ADD_BRAND, CHANGE_NAME, RECIEVE_BRAND, REQUEST_BRAND } from '../../constants/tea/brand';
import { AddBrandAction, ChangeNameAction, ReceiveBrandAction, RequestBrandAction } from '../../actions/tea/brand';

import { CHANGE_BRAND } from '../../constants/tea/bag';
import { ChangeBrandAction } from '../../actions/tea/bag';

export const requestBrand = {
   requestBrand: (brandid?: string): AppThunkAction<ReceiveBrandAction | RequestBrandAction> => (dispatch, getState) => {
    if (brandid === undefined) {
      dispatch({ type: RECIEVE_BRAND, brand: {} as IBrand });
      return;
    }

    try {
      let fetchTask = fetch(`/api/Tea/Brands/${brandid}`, { credentials: 'same-origin' })
        .then(response => response.json() as Promise<IBrand>)
        .then(data => {
          dispatch({ type: RECIEVE_BRAND, brand: data });
          addTask(fetchTask); // ensure server-side prerendering waits for this to complete
      });
    } catch (err) {
      dispatch({ type: RECIEVE_BRAND, brand: {} as IBrand });
    }

    dispatch({ type: REQUEST_BRAND, brandid: brandid });
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
          dispatch({type: CHANGE_BRAND, brand: {...data, ...{canaddnew: true}}});
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
    dispatch({type: CHANGE_NAME, name: name});
  },
 };
