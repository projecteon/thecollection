import { ITeabag } from '../interfaces/ITeaBag';
import { IBrand } from '../interfaces/IBrand';
import { ICountry } from '../interfaces/ICountry';
import { IBagType } from '../interfaces/IBagType';
import { CHANGE_BAGTYPE, CHANGE_BRAND, CHANGE_COUNTRY, CLEAR_BAGTYPE, CLEAR_BRAND, CLEAR_COUNTRY, RECEIVE_TEABAG, REQUEST_TEABAG } from '../constants/teabag';

export type ReceiveTeabagAction = {
  type: RECEIVE_TEABAG;
  teabag: ITeabag;
};

export type RequestTeabagAction = {
  type: REQUEST_TEABAG;
  teabagid: string;
};

export type ChangeBrandAction = {
  type: CHANGE_BRAND;
  brand: IBrand;
};

export type ClearBrandAction = {
  type: CLEAR_BRAND;
};

export type ChangeBagTypeAction = {
  type: CHANGE_BAGTYPE;
  bagtype: IBagType;
};

export type ClearBagTypeAction = {
  type: CLEAR_BAGTYPE;
};

export type ChangeCountryAction = {
  type: CHANGE_COUNTRY;
  country: ICountry;
};

export type ClearCountryAction = {
  type: CLEAR_COUNTRY;
};
