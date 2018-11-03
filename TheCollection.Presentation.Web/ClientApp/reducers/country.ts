import produce from 'immer';
import { Reducer } from 'redux';
import { ICountry } from '../interfaces/ICountry';
import { CountryActionTypes, addAction, valueChangedAction, recieveAction } from '../actions/country';
import { ActionsUnion } from '../util/Redux';

export interface ICountryState {
  country: ICountry;
}

export const actionCreators = {addAction, valueChangedAction, recieveAction};

const unloadedState: ICountryState = { country: {} as ICountry };
type CountryActions = ActionsUnion<typeof actionCreators>;
export const reducer: Reducer<ICountryState, CountryActions> = (state = unloadedState, action) =>
  produce(state, draft => {
    switch (action.type) {
      case CountryActionTypes.Recieve:
        draft.country = action.payload;
        break;
      case CountryActionTypes.Add:
        draft.country = action.payload;
        break;
      case CountryActionTypes.ChangeName:
        if (draft.country === undefined) {
          break;
        }

        draft.country[action.payload.property] = action.payload.value;
        break;
      default:
    }
  });


