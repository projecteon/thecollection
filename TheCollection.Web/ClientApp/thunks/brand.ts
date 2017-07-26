import { fetch, addTask } from 'domain-task';
import { AppThunkAction } from '../store';
import { IBrand } from '../interfaces/IBrand';
import { ADD_BRAND, RECIEVE_BRAND, REQUEST_BRAND } from '../constants/brand';
import { AddBrandAction, ReceiveBrandAction, RequestBrandAction } from '../actions/brand';

export const requestBrand = {
   requestBrand: (brandid?: string): AppThunkAction<ReceiveBrandAction | RequestBrandAction> => (dispatch, getState) => {
    if (brandid === undefined) {
      dispatch({ type: RECIEVE_BRAND, brand: {} as IBrand });
      return;
    }

    try {
      let fetchTask = fetch(`/api/Brands/${brandid}`)
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
  addBrand: (brand: IBrand): AppThunkAction<AddBrandAction> => (dispatch, getState) => {
    dispatch({type: ADD_BRAND, brand: brand});
  },
 };
