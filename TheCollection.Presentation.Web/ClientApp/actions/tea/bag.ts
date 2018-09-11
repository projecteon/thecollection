import { ITeabag } from '../../interfaces/tea/IBag';
import { IRefValue } from '../../interfaces/IRefValue';

export enum BagActionTypes {
  ChangeBrand = '[Bag] ChangeBrand',
  ClearBrand = '[Bag] ClearBrand',
  ChangeBagType = '[Bag] ChangeBagType',
  ClearBagType = '[Bag] ClearBagType',
  ChangeCountry = '[Bag] ChangeCountry',
  ClearCountry = '[Bag] ClearCountry',
  ChangeFlavor = '[Bag] ChangeFlavor',
  ChangeHallmark = '[Bag] ChangeHallmark',
  ChangeSerialNumber = '[Bag] ChangeSerialNumber',
  ChangeSerie = '[Bag] ChangeSerie',
  RecieveBag = '[Bag] RecieveBag',
  RequestBag = '[Bag] RequestBag',
  RecieveBags = '[Bag] RecieveBags',
  RequestBags = '[Bag] RequestBags',
  SearchTermsError = '[Bag] SearchTermsError',
  SearchTermsChanged = '[Bag] SearchTermsChanged',
  ZoomImageToggle = '[Bag] ZoomImageToggle',
  Save = '[Bag] Save',
}

export type ReceiveTeabagsAction = {
  type: BagActionTypes.RecieveBags,
  searchTerms: string;
  teabags: ITeabag[];
  resultCount: number;
};

export type RequestTeabagsAction = {
  type: BagActionTypes.RequestBags,
  searchTerms: string;
};

export type SearchTermsError = {
  type: BagActionTypes.SearchTermsError,
  searchError: string;
};

export type SearchTermsChanged = {
  type: BagActionTypes.SearchTermsChanged,
  searchTerms: string;
};

export type ZoomImage = {
  type: BagActionTypes.ZoomImageToggle,
  imageid: string;
};

export type ReceiveTeabagAction = {
  type: BagActionTypes.RecieveBag;
  teabag: ITeabag;
};

export type RequestTeabagAction = {
  type: BagActionTypes.RequestBag;
  teabagid: string;
};

export type ChangeBrandAction = {
  type: BagActionTypes.ChangeBrand;
  brand: IRefValue;
};

export type ClearBrandAction = {
  type: BagActionTypes.ClearBrand;
};

export type ChangeBagTypeAction = {
  type: BagActionTypes.ChangeBagType;
  bagtype: IRefValue;
};

export type ClearBagTypeAction = {
  type: BagActionTypes.ClearBagType;
};

export type ChangeCountryAction = {
  type: BagActionTypes.ChangeCountry;
  country: IRefValue;
};

export type ClearCountryAction = {
  type: BagActionTypes.ClearCountry;
};

export type ChangeSerieAction = {
  type: BagActionTypes.ChangeSerie;
  serie: string;
};

export type ChangeFlavourAction = {
  type: BagActionTypes.ChangeFlavor;
  flavour: string;
};

export type ChangeHallmarkAction = {
  type: BagActionTypes.ChangeHallmark;
  hallmark: string;
};

export type ChangeSerialNumberAction = {
  type: BagActionTypes.ChangeSerialNumber;
  serialnumber: string;
};

export type SaveTeabag = {
  type: BagActionTypes.Save;
};
