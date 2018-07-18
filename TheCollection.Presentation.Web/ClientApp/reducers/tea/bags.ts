import produce from 'immer';
import { Reducer } from 'redux';
import { ITeabag } from '../../interfaces/tea/IBag';
import {
  RECEIVE_TEABAGS,
  REQUEST_TEABAGS,
  SEARCH_TERMS_CHANGED,
  SEARCH_TERMS_ERROR,
  ZOOM_IMAGE_TOGGLE,
} from '../../constants/tea/bag';
import {
  ReceiveTeabagsAction,
  RequestTeabagsAction,
  SearchTermsChanged,
  SearchTermsError,
  ZoomImage,
} from '../../actions/tea/bag';
import { requestTeabags, validateSearchTerms, zoomImage } from '../../thunks/tea/bag';

export interface ITeabagsState {
  teabags: ITeabag[];
  resultCount?: number;
  isLoading: boolean;
  zoomImageId?: string;
  searchedTerms: string;
  searchError: string;
}

export const actionCreators = {...requestTeabags, ...validateSearchTerms, ...zoomImage};

const unloadedState: ITeabagsState = { isLoading: false, teabags: [], searchError: '', searchedTerms: '' };
type BagsActions = RequestTeabagsAction | ReceiveTeabagsAction | SearchTermsError | SearchTermsChanged | ZoomImage;
export const reducer: Reducer<ITeabagsState, BagsActions> = (state = unloadedState, action) =>
  produce(state, draft => {
    switch (action.type) {
      case REQUEST_TEABAGS:
        draft.searchedTerms = action.searchTerms;
        draft.teabags = [];
        draft.isLoading = true;
        break;
      case RECEIVE_TEABAGS:
        draft.searchedTerms = action.searchTerms;
        draft.teabags = action.teabags;
        draft.resultCount = action.resultCount;
        draft.isLoading = false;
        break;
      case SEARCH_TERMS_ERROR:
        draft.searchError = action.searchError;
        break;
      case SEARCH_TERMS_CHANGED:
        draft.searchedTerms = action.searchTerms;
        break;
      case ZOOM_IMAGE_TOGGLE:
        draft.zoomImageId = action.imageid;
        break;
      default:
        const exhaustiveCheck: never = action;
    }
  });
