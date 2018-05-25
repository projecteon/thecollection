import { ICountry } from '../interfaces/ICountry';
import { ADD_COUNTRY, CHANGE_NAME, RECIEVE_COUNTRY, REQUEST_COUNTRY } from '../constants/country';

export type ReceiveCountryAction = {
  type: RECIEVE_COUNTRY;
  country: ICountry;
};

export type RequestCountryAction = {
  type: REQUEST_COUNTRY;
  countryid: string;
};

export type AddCountryAction = {
  type: ADD_COUNTRY;
  country: ICountry;
};

export type ChangeNameAction = {
  type: CHANGE_NAME;
  name: string;
};
