import { ITeabag } from '../interfaces/ITeaBag';
import { IRefValue } from '../interfaces/IRefValue';
import * as Constant from '../constants/teabags';

export type ReceiveTeabagsAction = {
  type: Constant.RECEIVE_TEABAGS,
  searchTerms: string;
  teabags: ITeabag[];
  resultCount: number;
};

export type RequestTeabagsAction = {
  type: Constant.REQUEST_TEABAGS,
  searchTerms: string;
};

export type SearchTermsError = {
  type: Constant.SEARCH_TERMS_ERROR,
  searchError: string;
};

export type ZoomImage = {
  type: Constant.ZOOM_IMAGE_TOGGLE,
  imageid: string;
};

export type ReceiveTeabagAction = {
  type: Constant.RECEIVE_TEABAG;
  teabag: ITeabag;
};

export type RequestTeabagAction = {
  type: Constant.REQUEST_TEABAG;
  teabagid: string;
};

export type ChangeBrandAction = {
  type: Constant.CHANGE_BRAND;
  brand: IRefValue;
};

export type ClearBrandAction = {
  type: Constant.CLEAR_BRAND;
};

export type ChangeBagTypeAction = {
  type: Constant.CHANGE_BAGTYPE;
  bagtype: IRefValue;
};

export type ClearBagTypeAction = {
  type: Constant.CLEAR_BAGTYPE;
};

export type ChangeCountryAction = {
  type: Constant.CHANGE_COUNTRY;
  country: IRefValue;
};

export type ClearCountryAction = {
  type: Constant.CLEAR_COUNTRY;
};

export type ChangeSerieAction = {
  type: Constant.CHANGE_SERIE;
  serie: string;
};

export type ChangeFlavourAction = {
  type: Constant.CHANGE_FLAVOUR;
  flavour: string;
};

export type ChangeHallmarkAction = {
  type: Constant.CHANGE_HALLMARK;
  hallmark: string;
};

export type ChangeSerialNumberAction = {
  type: Constant.CHANGE_SERIALNUMBER;
  serialnumber: string;
};

export type SaveTeabag = {
  type: Constant.SAVE_TEABAG;
};
