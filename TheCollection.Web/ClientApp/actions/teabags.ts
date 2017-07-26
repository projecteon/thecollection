import { ITeabag } from '../interfaces/ITeaBag';
import { IBrand } from '../interfaces/IBrand';
import { ICountry } from '../interfaces/ICountry';
import { IBagType } from '../interfaces/IBagType';
import { RECEIVE_TEABAGS, REQUEST_TEABAGS, SEARCH_TERMS_ERROR, ZOOM_IMAGE_TOGGLE, CHANGE_BAGTYPE, CHANGE_BRAND, CHANGE_COUNTRY, CLEAR_BAGTYPE, CLEAR_BRAND, CLEAR_COUNTRY, RECEIVE_TEABAG, REQUEST_TEABAG } from '../constants/teabags';

export type ReceiveTeabagsAction = {
  type: RECEIVE_TEABAGS,
  searchTerms: string;
  teabags: ITeabag[];
  resultCount: number;
};

export type RequestTeabagsAction = {
  type: REQUEST_TEABAGS,
  searchTerms: string;
};

export type SearchTermsError = {
  type: SEARCH_TERMS_ERROR,
  searchError: string;
};

export type ZoomImage = {
  type: ZOOM_IMAGE_TOGGLE,
  imageid: string;
};

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
