import produce from 'immer';
import { Reducer } from 'redux';
import { IBagType } from '../../interfaces/tea/IBagType';
import { AddBagtypeAction, BagTypeActionTypes, ChangeNameAction, ReceiveBagtypeAction, RequestBagtypeAction } from '../../actions/tea/bagtype';
import { addBagtype, changeName, requestBagtype } from '../../thunks/tea/bagtype';

export interface IBagTypeState {
  bagtype: IBagType;
  isLoading: boolean;
}

export const actionCreators = {...addBagtype, ...changeName, ...requestBagtype};

const unloadedState: IBagTypeState = { isLoading: false, bagtype: {} as IBagType };
type BagtypeActions = AddBagtypeAction | ChangeNameAction | ReceiveBagtypeAction | RequestBagtypeAction;
export const reducer: Reducer<IBagTypeState, BagtypeActions> = (state = unloadedState, action) =>
  produce(state, draft => {
    switch (action.type) {
      case BagTypeActionTypes.Request:
        draft.bagtype = {} as IBagType;
        draft.isLoading = true;
        break;
      case BagTypeActionTypes.Recieve:
        draft.bagtype = action.bagtype;
        draft.isLoading = false;
        break;
      case BagTypeActionTypes.Add:
        draft.bagtype = action.bagtype;
        draft.isLoading = false;
        break;
      case BagTypeActionTypes.ChangeName:
        draft.isLoading = false;
        if (draft.bagtype === undefined) {
          break;
        }

        draft.bagtype.name = action.name;
        break;
      default:
        const exhaustiveCheck: never = action;
    }
  });


