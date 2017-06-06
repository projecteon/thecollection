import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { AppThunkAction } from './';
import { ITeabag } from '../interfaces/ITeaBag';
import { ISearchResult } from '../interfaces/ISearchResult';

export interface TeabagsState {
  teabags: ITeabag[];
  resultCount?: number;
  isLoading: boolean;
  zoomImageId?: string;
  searchedTerms?: string;
  searchError?: string;
}

interface RequestTeabagsAction {
  type: 'REQUEST_TEABAGS';
  searchTerms: string;
}

interface ReceiveTeabagsAction {
  type: 'RECEIVE_TEABAGS';
  searchTerms: string;
  teabags: ITeabag[];
  resultCount: number;
}

interface SearchTermsError {
  type: 'SEARCH_TERMS_ERROR';
  searchError: string;
}

interface ZoomImage {
  type: 'ZOOM_IMAGE_TOGGLE';
  imageid: string;
}

type KnownAction = RequestTeabagsAction | ReceiveTeabagsAction | SearchTermsError | ZoomImage;

export const actionCreators = {
  requestTeabags: (searchTerms?: string): AppThunkAction<RequestTeabagsAction | ReceiveTeabagsAction | SearchTermsError> => (dispatch, getState) => {
     if (searchTerms.trim().length < 3) {
      dispatch({ type: 'SEARCH_TERMS_ERROR', searchError: 'Requires at least 3 characters to search' });
      return;
    }

    // only load data if it's something we don't already have (and are not already loading)
    if (searchTerms !== getState().teabags.searchedTerms || true) {
      let uri = searchTerms !== undefined && searchTerms.length > 0
        ? `/api/Bags/?searchterm=${encodeURIComponent(searchTerms)}`
        : `/api/Bags/`;
      try {
        let fetchTask = fetch(uri)
          .then(response => response.json() as Promise<ISearchResult<ITeabag>>)
          .then(data => {
            console.log(`loaded ${data.count} rows`);
            dispatch({ type: 'RECEIVE_TEABAGS', searchTerms: searchTerms, teabags: data.data, resultCount: data.count });
            addTask(fetchTask); // Ensure server-side prerendering waits for this to complete
        });
      } catch (err) {
        dispatch({ type: 'RECEIVE_TEABAGS', searchTerms: searchTerms, teabags: [], resultCount: 0 });
      }

      dispatch({ type: 'REQUEST_TEABAGS', searchTerms: searchTerms });
    }
  },
  validateSearchTerms: (searchTerms: string): AppThunkAction<SearchTermsError> => (dispatch, getState) => {
    if (getState().teabags.searchError !== '' && searchTerms.trim().length > 2) {
      dispatch({ type: 'SEARCH_TERMS_ERROR', searchError: '' });
    }
  },
  zoomImage: (imageid: string): AppThunkAction<ZoomImage> => (dispatch, getState) => {
    dispatch({ type: 'ZOOM_IMAGE_TOGGLE', imageid: imageid });
  },
};


const unloadedState: TeabagsState = { isLoading: false, teabags: [], searchError: '' };
export const reducer: Reducer<TeabagsState> = (state: TeabagsState, action: KnownAction) => {
  switch (action.type) {
    case 'REQUEST_TEABAGS':
      return  {...state, ...{
        searchTerms: action.searchTerms,
        teabags: [],
        isLoading: true
      }};
    case 'RECEIVE_TEABAGS':
      // Only accept the incoming data if it matches the most recent request. This ensures we correctly
      // handle out-of-order responses.
      // if (action.searchTerms === state.searchedTerms) {
      //   return  {...state, ...{
      //     searchTerms: action.searchTerms,
      //     teabags: action.teabags,
      //     resultCount: action.resultCount,
      //     isLoading: false
      //   }};
      // }
      // break;

      return  {...state, ...{
          searchTerms: action.searchTerms,
          teabags: action.teabags,
          resultCount: action.resultCount,
          isLoading: false
        }};
    case 'SEARCH_TERMS_ERROR':
      return {...state, ...{searchError: action.searchError}};
    case 'ZOOM_IMAGE_TOGGLE':
      return {...state, ...{zoomImageId: action.imageid}};
    default:
      const exhaustiveCheck: never = action;
  }

  return state || unloadedState;
};
