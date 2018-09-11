import produce from 'immer';
import { Reducer } from 'redux';
import { IBrand } from '../../interfaces/tea/IBrand';
import { AddBrandAction, BrandActionTypes, ChangeNameAction, ReceiveBrandAction, RequestBrandAction } from '../../actions/tea/brand';
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
      case BrandActionTypes.Request:
        draft.brand = {} as IBrand;
        draft.isLoading = true;
        break;
      case BrandActionTypes.Recieve:
        draft.brand = action.brand;
        draft.isLoading = false;
        break;
      case BrandActionTypes.Add:
        draft.brand = action.brand;
        draft.isLoading = false;
        break;
      case BrandActionTypes.ChangeName:
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


