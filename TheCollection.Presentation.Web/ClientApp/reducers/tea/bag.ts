import produce from 'immer';
import { Reducer } from 'redux';
import { ITeabag } from '../../interfaces/tea/IBag';
import { BagActionTypes, valueChangedAction, recieveAction } from '../../actions/tea/bag';
import { ActionsUnion } from '../../util/Redux';
import { EmptyTeabagRecord } from '../../types';

export interface ITeabagState {
  teabag: ITeabag;
  isLoading: boolean;
}

const unloadedState: ITeabagState = { isLoading: false, teabag: EmptyTeabagRecord };
const actionCreators = { recieveAction, valueChangedAction };
export type actionTypes = ActionsUnion<typeof actionCreators>;
export const reducer: Reducer<ITeabagState, actionTypes> = (state = unloadedState, action) =>
  produce(state, draft => {
    switch (action.type) {
      case BagActionTypes.RecieveBag:
        draft.teabag = action.payload;
        draft.isLoading = false;
        break;
      case BagActionTypes.ValueChanged:
        draft.teabag[action.payload.property] = action.payload.value;
        break;
      default:
    }
  });
