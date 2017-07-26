import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { AppThunkAction } from '../store';
import { IBrand } from '../interfaces/IBrand';
import { ADD_BRAND, RECIEVE_BRAND, REQUEST_BRAND } from '../constants/brand';
import { AddBrandAction, ReceiveBrandAction, RequestBrandAction } from '../actions/brand';
import { addBrand, requestBrand } from '../thunks/brand';

export interface IBrandState {
  brand?: IBrand;
  isLoading: boolean;
}

export const actionCreators = {...addBrand, ...requestBrand};

const unloadedState: IBrandState = { isLoading: false, brand: {} as IBrand };
type BrandActions = AddBrandAction | ReceiveBrandAction | RequestBrandAction;
export const reducer: Reducer<IBrandState> = (state: IBrandState, action: BrandActions) => {
  switch (action.type) {
    case REQUEST_BRAND:
      return  {...state, ...{ brand: {} as IBrand, isLoading: true }};
    case RECIEVE_BRAND:
      return  {...state, ...{ brand: action.brand, isLoading: false }};
    case ADD_BRAND:
      return {...state, ...{ brand: action.brand, isLoading: false }};
    default:
      const exhaustiveCheck: never = action;
  }

  return state || unloadedState;
};


