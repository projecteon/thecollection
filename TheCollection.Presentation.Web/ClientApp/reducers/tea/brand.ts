import produce from 'immer';
import { Reducer } from 'redux';
import { IBrand } from '../../interfaces/tea/IBrand';
import { addAction, BrandActionTypes, valueChangedAction, recieveAction } from '../../actions/tea/brand';
import { ActionsUnion } from '../../util/Redux';

export interface IBrandState {
  brand: IBrand;
}

export const actionCreators = {addAction, valueChangedAction, recieveAction};

const unloadedState: IBrandState = { brand: {} as IBrand };
export type BrandActions = ActionsUnion<typeof actionCreators>;
export const reducer: Reducer<IBrandState, BrandActions> = (state = unloadedState, action) =>
  produce(state, draft => {
    switch (action.type) {
      case BrandActionTypes.Recieve:
        draft.brand = action.payload;
        break;
      case BrandActionTypes.Add:
        draft.brand = action.payload;
        break;
      case BrandActionTypes.ChangeName:
        if (draft.brand === undefined) {
          break;
        }

        draft.brand[action.payload.property] = action.payload.value;
        break;
      default:
        const exhaustiveCheck: never = action;
    }
  });


