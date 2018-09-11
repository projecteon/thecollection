import produce from 'immer';
import { Reducer } from 'redux';
import { ITeabag } from '../../interfaces/tea/IBag';
import {
  BagActionTypes,
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
      case BagActionTypes.RequestBags:
        draft.searchedTerms = action.searchTerms;
        draft.teabags = [];
        draft.isLoading = true;
        break;
      case BagActionTypes.RecieveBags:
        draft.searchedTerms = action.searchTerms;
        draft.teabags = action.teabags;
        draft.resultCount = action.resultCount;
        draft.isLoading = false;
        break;
      case BagActionTypes.SearchTermsError:
        draft.searchError = action.searchError;
        break;
      case BagActionTypes.SearchTermsChanged:
        draft.searchedTerms = action.searchTerms;
        break;
      case BagActionTypes.ZoomImageToggle:
        draft.zoomImageId = action.imageid;
        break;
      default:
        const exhaustiveCheck: never = action;
    }
  });
