import produce from 'immer';
import { Reducer } from 'redux';
import { IBagType } from '../../interfaces/tea/IBagType';
import { BagTypeActionTypes, addAction, valueChangedAction, recieveAction } from '../../actions/tea/bagtype';
import { ActionsUnion } from '../../util/Redux';

export interface IBagTypeState {
  bagtype: IBagType;
}

export const actionCreators = {addAction, valueChangedAction, recieveAction};

const unloadedState: IBagTypeState = { bagtype: {} as IBagType };
type BagtypeActions = ActionsUnion<typeof actionCreators>;
export const reducer: Reducer<IBagTypeState, BagtypeActions> = (state = unloadedState, action) =>
  produce(state, draft => {
    switch (action.type) {
      case BagTypeActionTypes.Recieve:
        draft.bagtype = action.payload;
        break;
      case BagTypeActionTypes.Add:
        draft.bagtype = action.payload;
        break;
      case BagTypeActionTypes.ChangeName:
        if (draft.bagtype === undefined) {
          break;
        }

        draft.bagtype[action.payload.property] = action.payload.value;
        break;
      default:
    }
  });


