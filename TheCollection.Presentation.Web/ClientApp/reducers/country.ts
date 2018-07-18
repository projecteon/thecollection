import produce from 'immer';
import { Reducer } from 'redux';
import { ICountry } from '../interfaces/ICountry';
import { ADD_COUNTRY, CHANGE_NAME, RECIEVE_COUNTRY, REQUEST_COUNTRY } from '../constants/country';
import { AddCountryAction, ChangeNameAction, ReceiveCountryAction, RequestCountryAction } from '../actions/country';
import { addCountry, changeName, requestCountry } from '../thunks/country';

export interface ICountryState {
  country: ICountry;
  isLoading: boolean;
}

export const actionCreators = {...addCountry, ...changeName, ...requestCountry};

const unloadedState: ICountryState = { isLoading: false, country: {} as ICountry };
type CountryActions = AddCountryAction | ChangeNameAction | ReceiveCountryAction | RequestCountryAction;
export const reducer: Reducer<ICountryState, CountryActions> = (state = unloadedState, action) =>
  produce(state, draft => {
    switch (action.type) {
      case REQUEST_COUNTRY:
        draft.country = {} as ICountry;
        draft.isLoading = true;
        break;
      case RECIEVE_COUNTRY:
        draft.country = action.country;
        draft.isLoading = false;
        break;
      case ADD_COUNTRY:
        draft.country = action.country;
        draft.isLoading = false;
        break;
      case CHANGE_NAME:
        draft.isLoading = false;
        if (draft.country === undefined) {
          break;
        }

        draft.country.name = action.name;
        break;
      default:
        const exhaustiveCheck: never = action;
    }
  });


