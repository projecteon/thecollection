import { ITeabag } from '../interfaces/ITeaBag';
import { RECEIVE_TEABAGS, REQUEST_TEABAGS, SEARCH_TERMS_ERROR, ZOOM_IMAGE_TOGGLE } from '../constants/teabags';

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
