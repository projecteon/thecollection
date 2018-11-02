import produce from 'immer';
import { Reducer } from 'redux';
import { IUserFeedback } from '../types/ui';
import { UIActionTypes, removeUserFeedback, addUserFeedback, startWorkingAction, endWorkingAction } from '../actions/ui';
import { ActionsUnion } from '../util/Redux';

export interface IUIState {
  userFeedbacks: IUserFeedback[];
  isLoading: boolean;
}

const unloadedState: IUIState = { isLoading: false, userFeedbacks: [] };
const actionCreators = { addUserFeedback, removeUserFeedback, startWorkingAction, endWorkingAction};
export type actionTypes = ActionsUnion<typeof actionCreators>;
export const reducer: Reducer<IUIState, actionTypes> = (state = unloadedState, action) =>
  produce(state, draft => {
    switch (action.type) {
      case UIActionTypes.AddUserFeedback:
        draft.userFeedbacks.push(action.payload);
        draft.isLoading = false;
        break;
      case UIActionTypes.RemoveUserFeedback:
        draft.isLoading = false;
        draft.userFeedbacks = draft.userFeedbacks.filter(userFeedback => userFeedback.id !== action.payload);
        break;
      case UIActionTypes.StartWorking:
        draft.isLoading = true;
        break;
      case UIActionTypes.EndWorking:
        draft.isLoading = false;
        break;
      default:
        const exhaustiveCheck: never = action;
    }
  });
