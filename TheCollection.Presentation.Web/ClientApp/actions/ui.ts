import { IUserFeedback } from '../types/ui';
import { createAction } from '../util/Redux';

export enum UIActionTypes {
  AddUserFeedback = '[UI] AddUserFeedback',
  RemoveUserFeedback = '[UI] RemoveUserFeedback',
  StartWorking = '[UI] StartWorking',
  EndWorking = '[UI] EndWorking',
}

export const addUserFeedback = (feedback: IUserFeedback) => createAction( UIActionTypes.AddUserFeedback, feedback);
export const removeUserFeedback = (id: number) => createAction( UIActionTypes.RemoveUserFeedback, id);
export const startWorkingAction = () => createAction( UIActionTypes.StartWorking);
export const endWorkingAction = () => createAction( UIActionTypes.EndWorking);
