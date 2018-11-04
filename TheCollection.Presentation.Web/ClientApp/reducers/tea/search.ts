import produce from 'immer';
import { Reducer } from 'redux';
import { ITeabag } from '../../interfaces/tea/IBag';
import { ActionsUnion } from '../../util/Redux';
import { searchResult, searchTermChanged, searchTermError, SearchActionTypes, zoomImageToggle } from '../../actions/tea/search';

export interface ISearchState {
  result: ITeabag[];
  resultCount?: number;
  zoomImageId?: string;
  searchedTerms: string;
  searchError: string;
}

const unloadedState: ISearchState = { result: [], searchError: '', searchedTerms: '' };
const actionCreators = { searchResult, searchTermChanged, searchTermError, zoomImageToggle };
export type actionTypes = ActionsUnion<typeof actionCreators>;
export const reducer: Reducer<ISearchState, actionTypes> = (state = unloadedState, action) =>
  produce(state, draft => {
    switch (action.type) {
      case SearchActionTypes.SearchResult:
        draft.result = action.payload.data;
        draft.resultCount = action.payload.count;
        break;
      case SearchActionTypes.SearchTermsChanged:
        draft.searchedTerms = action.payload;
        break;
      case SearchActionTypes.SearchTermsError:
        draft.searchError = action.payload;
        break;
      case SearchActionTypes.ZoomImageToggle:
        draft.zoomImageId = action.payload;
        break;
      default:
        const exhaustiveCheck: never = action;
    }
  });
