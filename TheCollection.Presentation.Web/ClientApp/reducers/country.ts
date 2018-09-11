import produce from 'immer';
import { Reducer } from 'redux';
import { ICountry } from '../interfaces/ICountry';
import { AddCountryAction, ChangeNameAction, CountryActionTypes, ReceiveCountryAction, RequestCountryAction } from '../actions/country';
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
      case CountryActionTypes.Request:
        draft.country = {} as ICountry;
        draft.isLoading = true;
        break;
      case CountryActionTypes.Recieve:
        draft.country = action.country;
        draft.isLoading = false;
        break;
      case CountryActionTypes.Add:
        draft.country = action.country;
        draft.isLoading = false;
        break;
      case CountryActionTypes.ChangeName:
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


