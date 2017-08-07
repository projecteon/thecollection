import { fetch, addTask } from 'domain-task';
import { routerActions, RouterAction } from 'react-router-redux';
import { AppThunkAction } from '../store';
import { ICountry } from '../interfaces/ICountry';
import { ADD_COUNTRY, CHANGE_NAME, RECIEVE_COUNTRY, REQUEST_COUNTRY } from '../constants/country';
import { AddCountryAction, ChangeNameAction, ReceiveCountryAction, RequestCountryAction } from '../actions/country';

import { CHANGE_COUNTRY } from '../constants/teabags';
import { ChangeCountryAction } from '../actions/teabags';

export const requestCountry = {
   requestCountry: (countryid?: string): AppThunkAction<ReceiveCountryAction | RequestCountryAction> => (dispatch, getState) => {
    if (countryid === undefined) {
      dispatch({ type: RECIEVE_COUNTRY, country: {} as ICountry });
      return;
    }

    try {
      let fetchTask = fetch(`/api/Countries/${countryid}`)
        .then(response => response.json() as Promise<ICountry>)
        .then(data => {
          dispatch({ type: RECIEVE_COUNTRY, country: data });
          addTask(fetchTask); // ensure server-side prerendering waits for this to complete
      });
    } catch (err) {
      dispatch({ type: RECIEVE_COUNTRY, country: {} as ICountry });
    }

    dispatch({ type: REQUEST_COUNTRY, countryid: countryid });
  },
 };

 export const addCountry = {
  addCountry: (country: ICountry): AppThunkAction<ChangeCountryAction | RouterAction> => (dispatch, getState) => {
    try {
      let addBrandTask = fetch(`/api/Countries/`, {
          headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
          },
          method: 'post',
          body: JSON.stringify(country),
        })
        .then(response => response.json() as Promise<ICountry>)
        .then(data => {
          dispatch({type: CHANGE_COUNTRY, country: data});
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
    dispatch({type: CHANGE_NAME, name: name});
  },
 };
