import { Action, Reducer, ActionCreator } from 'redux';
import { fetch, addTask } from 'domain-task';
import { AppThunkAction } from '../store';
import { ISearchResult } from '../interfaces/ISearchResult';
import { ITeabag } from '../interfaces/ITeaBag';
import {
  RECEIVE_TEABAGS,
  REQUEST_TEABAGS,
  SEARCH_TERMS_CHANGED,
  SEARCH_TERMS_ERROR,
  ZOOM_IMAGE_TOGGLE,
} from '../constants/teabags';
import {
  ReceiveTeabagsAction,
  RequestTeabagsAction,
  SearchTermsChanged,
  SearchTermsError,
  ZoomImage,
} from '../actions/teabags';
import { requestTeabags, validateSearchTerms, zoomImage } from '../thunks/teabags';

export interface ITeabagsState {
  teabags: ITeabag[];
  resultCount?: number;
  isLoading: boolean;
  zoomImageId?: string;
  searchedTerms?: string;
  searchError?: string;
}

export const actionCreators = {...requestTeabags, ...validateSearchTerms, ...zoomImage};

const unloadedState: ITeabagsState = { isLoading: false, teabags: [], searchError: '' };
type KnownActions = RequestTeabagsAction | ReceiveTeabagsAction | SearchTermsError | SearchTermsChanged | ZoomImage;
export const reducer: Reducer<ITeabagsState> = (state: ITeabagsState, action: KnownActions) => {
  switch (action.type) {
    case REQUEST_TEABAGS:
      return {...state, ...{
        searchedTerms: action.searchTerms,
        teabags: [],
        isLoading: true,
      }};
    case RECEIVE_TEABAGS:
      return {...state, ...{
          searchedTerms: action.searchTerms,
          teabags: action.teabags,
          resultCount: action.resultCount,
          isLoading: false,
        }};
    case SEARCH_TERMS_ERROR:
      return {...state, ...{searchError: action.searchError}};
    case SEARCH_TERMS_CHANGED:
        return {...state, ...{searchedTerms: action.searchTerms}};
    case ZOOM_IMAGE_TOGGLE:
      return {...state, ...{zoomImageId: action.imageid}};
    default:
      const exhaustiveCheck: never = action;
  }

  return state || unloadedState;
};
