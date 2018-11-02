import { ICountry } from '../interfaces/ICountry';
import { createAction } from '../util/Redux';

export enum CountryActionTypes {
  Add = '[Country] Add',
  ChangeName = '[Country] ChangeName',
  Recieve = '[Country] Recieve',
  Request = '[Country] Request',
}

export const requestAction = (id?: string) => createAction(CountryActionTypes.Request, {id});
export const recieveAction = (country: ICountry) => createAction(CountryActionTypes.Recieve, country);
export const addAction = (country: ICountry) => createAction(CountryActionTypes.Add, country);
export const valueChangedAction = <K extends keyof ICountry>(property: K, value: ICountry[K]) => createAction(CountryActionTypes.ChangeName, {property, value});

