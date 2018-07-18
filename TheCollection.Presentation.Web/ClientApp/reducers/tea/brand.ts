import produce from 'immer';
import { Reducer } from 'redux';
import { IBrand } from '../../interfaces/tea/IBrand';
import { ADD_BRAND, CHANGE_NAME, RECIEVE_BRAND, REQUEST_BRAND } from '../../constants/tea/brand';
import { AddBrandAction, ChangeNameAction, ReceiveBrandAction, RequestBrandAction } from '../../actions/tea/brand';
import { addBrand, changeName, requestBrand } from '../../thunks/tea/brand';

export interface IBrandState {
  brand: IBrand;
  isLoading: boolean;
}

export const actionCreators = {...addBrand, ...changeName, ...requestBrand};

const unloadedState: IBrandState = { isLoading: false, brand: {} as IBrand };
type BrandActions = AddBrandAction | ChangeNameAction | ReceiveBrandAction | RequestBrandAction;
export const reducer: Reducer<IBrandState, BrandActions> = (state = unloadedState, action) =>
  produce(state, draft => {
    switch (action.type) {
      case REQUEST_BRAND:
        draft.brand = {} as IBrand;
        draft.isLoading = true;
        break;
      case RECIEVE_BRAND:
        draft.brand = action.brand;
        draft.isLoading = false;
        break;
      case ADD_BRAND:
        draft.brand = action.brand;
        draft.isLoading = false;
        break;
      case CHANGE_NAME:
        draft.isLoading = false;
        if (draft.brand === undefined) {
          break;
        }

        draft.brand.name = action.name;
        break;
      default:
        const exhaustiveCheck: never = action;
    }
  });


