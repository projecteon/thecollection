import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { AppThunkAction } from '../../store';
import { IBagType } from '../../interfaces/tea/IBagType';
import { ADD_BAGTYPE, CHANGE_NAME, RECIEVE_BAGTYPE, REQUEST_BAGTYPE } from '../../constants/tea/bagtype';
import { AddBagtypeAction, ChangeNameAction, ReceiveBagtypeAction, RequestBagtypeAction } from '../../actions/tea/bagtype';
import { addBagtype, changeName, requestBagtype } from '../../thunks/tea/bagtype';

export interface IBagTypeState {
  bagtype?: IBagType;
  isLoading: boolean;
}

export const actionCreators = {...addBagtype, ...changeName, ...requestBagtype};

const unloadedState: IBagTypeState = { isLoading: false, bagtype: {} as IBagType };
type BagtypeActions = AddBagtypeAction | ChangeNameAction | ReceiveBagtypeAction | RequestBagtypeAction;
export const reducer: Reducer<IBagTypeState> = (state: IBagTypeState, action: BagtypeActions) => {
  switch (action.type) {
    case REQUEST_BAGTYPE:
      return  {...state, ...{ bagtype: {} as IBagType, isLoading: true }};
    case RECIEVE_BAGTYPE:
      return  {...state, ...{ bagtype: action.bagtype, isLoading: false }};
    case ADD_BAGTYPE:
      return {...state, ...{ bagtype: action.bagtype, isLoading: false }};
    case CHANGE_NAME:
      return {...state, ...{ bagtype: {...state.bagtype, ...{name: action.name}}, isLoading: false }};
    default:
      const exhaustiveCheck: never = action;
  }

  return state || unloadedState;
};


