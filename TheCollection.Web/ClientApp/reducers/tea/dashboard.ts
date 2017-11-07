import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { AppThunkAction } from '../../store';
import { IRefValue } from '../../interfaces/IRefValue';
import { ICountBy } from '../../interfaces/ICountBy';
import { RECIEVE_BAGTYPECOUNT, REQUEST_BAGTYPECOUNT } from '../../constants/tea/dashboard';
import { ReceiveBagTypeCountAction, RequestBagTypeCountAction } from '../../actions/tea/dashboard';
import { requestBagTypeCount } from '../../thunks/tea/dashboard';

export interface IDashboardState {
  bagtypecount?: ICountBy<IRefValue>[];
  isLoading: boolean;
}

export const actionCreators = {...requestBagTypeCount};

const unloadedState: IDashboardState = { isLoading: false };
type DashboardActions = ReceiveBagTypeCountAction | RequestBagTypeCountAction;
export const reducer: Reducer<IDashboardState> = (state: IDashboardState, action: DashboardActions) => {
  switch (action.type) {
    case REQUEST_BAGTYPECOUNT:
      return  {...state, ...{ bagtypecount: undefined, isLoading: true }};
    case RECIEVE_BAGTYPECOUNT:
      return  {...state, ...{ bagtypecount: action.bagtypecount, isLoading: false }};
    default:
      const exhaustiveCheck: never = action;
  }

  return state || unloadedState;
};


