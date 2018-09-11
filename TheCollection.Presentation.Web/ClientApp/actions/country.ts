import { ICountry } from '../interfaces/ICountry';

export enum CountryActionTypes {
  Add = '[Country] Add',
  ChangeName = '[Country] ChangeName',
  Recieve = '[Country] Recieve',
  Request = '[Country] Request',
}

export type ReceiveCountryAction = {
  type: CountryActionTypes.Recieve;
  country: ICountry;
};

export type RequestCountryAction = {
  type: CountryActionTypes.Request;
  countryid: string;
};

export type AddCountryAction = {
  type: CountryActionTypes.Add;
  country: ICountry;
};

export type ChangeNameAction = {
  type: CountryActionTypes.ChangeName;
  name: string;
};
