import { fetch, addTask } from 'domain-task';
import { routerActions, RouterAction } from 'react-router-redux';
import { AppThunkAction } from '../store';
import { ICountry } from '../interfaces/ICountry';
import { ChangeNameAction, CountryActionTypes, ReceiveCountryAction, RequestCountryAction } from '../actions/country';

import { BagActionTypes, ChangeCountryAction } from '../actions/tea/bag';

export const requestCountry = {
   requestCountry: (countryid?: string): AppThunkAction<ReceiveCountryAction | RequestCountryAction> => (dispatch, getState) => {
    if (countryid === undefined) {
      dispatch({ type: CountryActionTypes.Recieve, country: {} as ICountry });
      return;
    }

    try {
      let fetchTask = fetch(`/api/Tea/Countries/${countryid}`, { credentials: 'same-origin' })
        .then(response => response.json() as Promise<ICountry>)
        .then(data => {
          dispatch({ type: CountryActionTypes.Recieve, country: data });
          addTask(fetchTask); // ensure server-side prerendering waits for this to complete
      });
    } catch (err) {
      dispatch({ type: CountryActionTypes.Recieve, country: {} as ICountry });
    }

    dispatch({ type: CountryActionTypes.Request, countryid: countryid });
  },
 };

export const addCountry = {
  addCountry: (country: ICountry): AppThunkAction<ChangeCountryAction | RouterAction> => (dispatch, getState) => {
    try {
      let addBrandTask = fetch(`/api/Tea/Countries/`, {
          headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
          },
          credentials: 'same-origin',
          method: 'post',
          body: JSON.stringify(country),
        })
        .then(response => response.json() as Promise<ICountry>)
        .then(data => {
          dispatch({type: BagActionTypes.ChangeCountry, country: {...data, ...{canaddnew: true}}});
          dispatch(routerActions.goBack());
          addTask(addBrandTask); // ensure server-side prerendering waits for this to complete
      });
    } catch (err) {
      console.log(err);
    }
  },
 };

export const changeName = {
  changeName: (name: string): AppThunkAction<ChangeNameAction> => (dispatch, getState) => {
    dispatch({type: CountryActionTypes.ChangeName, name: name});
  },
 };
