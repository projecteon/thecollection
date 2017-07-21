import { ISearchResult } from '../interfaces/ISearchResult';
import { ITeabag } from '../interfaces/ITeaBag';
import { RECEIVE_TEABAGS, REQUEST_TEABAGS, SEARCH_TERMS_ERROR, ZOOM_IMAGE_TOGGLE } from '../constants/teabags';
import { ReceiveTeabagsAction, RequestTeabagsAction, SearchTermsError, ZoomImage } from '../actions/teabags';
import { Action, Reducer, ActionCreator } from 'redux';
import { fetch, addTask } from 'domain-task';
import { AppThunkAction } from '../store';

export interface ITeabagsState {
  teabags: ITeabag[];
  resultCount?: number;
  isLoading: boolean;
  zoomImageId?: string;
  searchedTerms?: string;
  searchError?: string;
}

type KnownAction = RequestTeabagsAction | ReceiveTeabagsAction | SearchTermsError | ZoomImage;

export const actionCreators = {
  requestTeabags: (searchTerms?: string): AppThunkAction<RequestTeabagsAction | ReceiveTeabagsAction | SearchTermsError> => (dispatch, getState) => {
    if (searchTerms.trim().length < 3) {
      dispatch({ type: SEARCH_TERMS_ERROR, searchError: 'Requires at least 3 characters to search' });
      return;
    }

    let uri = searchTerms !== undefined && searchTerms.length > 0 ? `/api/Bags/?searchterm=${encodeURIComponent(searchTerms)}` : `/api/Bags/`;
    try {
      let fetchTask = fetch(uri)
        .then(response => response.json() as Promise<ISearchResult<ITeabag>>)
        .then(data => {
          dispatch({ type: RECEIVE_TEABAGS, searchTerms: searchTerms, teabags: data.data, resultCount: data.count });
          addTask(fetchTask); // ensure server-side prerendering waits for this to complete
      });
    } catch (err) {
      dispatch({ type: RECEIVE_TEABAGS, searchTerms: searchTerms, teabags: [], resultCount: 0 });
    }

    dispatch({ type: REQUEST_TEABAGS, searchTerms: searchTerms });
  },
  validateSearchTerms: (searchTerms: string): AppThunkAction<SearchTermsError> => (dispatch, getState) => {
    if (getState().teabags.searchError !== '' && searchTerms.trim().length > 2) {
      dispatch({ type: SEARCH_TERMS_ERROR, searchError: '' });
    }
  },
  zoomImage: (imageid: string): AppThunkAction<ZoomImage> => (dispatch, getState) => {
    dispatch({ type: ZOOM_IMAGE_TOGGLE, imageid: imageid });
  },
};

const unloadedState: ITeabagsState = { isLoading: false, teabags: [], searchError: '' };
export const reducer: Reducer<ITeabagsState> = (state: ITeabagsState, action: KnownAction) => {
  switch (action.type) {
    case REQUEST_TEABAGS:
      return {...state, ...{
        searchedTerms: action.searchTerms,
        teabags: [],
        isLoading: true
      }};
    case RECEIVE_TEABAGS:
      return {...state, ...{
          searchedTerms: action.searchTerms,
          teabags: action.teabags,
          resultCount: action.resultCount,
          isLoading: false
        }};
    case SEARCH_TERMS_ERROR:
      return {...state, ...{searchError: action.searchError}};
    case ZOOM_IMAGE_TOGGLE:
      return {...state, ...{zoomImageId: action.imageid}};
    default:
      const exhaustiveCheck: never = action;
  }

  return state || unloadedState;
};
