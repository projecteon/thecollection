import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { AppThunkAction } from '../store';
import { ICountry } from '../interfaces/ICountry';
import { ADD_COUNTRY, CHANGE_NAME, RECIEVE_COUNTRY, REQUEST_COUNTRY } from '../constants/country';
import { AddCountryAction, ChangeNameAction, ReceiveCountryAction, RequestCountryAction } from '../actions/country';
import { addCountry, changeName, requestCountry } from '../thunks/country';

export interface ICountryState {
  country?: ICountry;
  isLoading: boolean;
}

export const actionCreators = {...addCountry, ...changeName, ...requestCountry};

const unloadedState: ICountryState = { isLoading: false, country: {} as ICountry };
type CountryActions = AddCountryAction | ChangeNameAction | ReceiveCountryAction | RequestCountryAction;
export const reducer: Reducer<ICountryState> = (state: ICountryState, action: CountryActions) => {
  switch (action.type) {
    case REQUEST_COUNTRY:
      return  {...state, ...{ country: {} as ICountry, isLoading: true }};
    case RECIEVE_COUNTRY:
      return  {...state, ...{ country: action.country, isLoading: false }};
    case ADD_COUNTRY:
      return {...state, ...{ country: action.country, isLoading: false }};
    case CHANGE_NAME:
      return {...state, ...{ country: {...state.country, ...{name: action.name}}, isLoading: false }};
    default:
      const exhaustiveCheck: never = action;
  }

  return state || unloadedState;
};


