import { createAction } from '../../util/Redux';
import { ITeabag } from '../../interfaces/tea/IBag';
import { ISearchResult } from '../../interfaces/ISearchResult';

export enum SearchActionTypes {
  SearchResult = '[Tea][Search] SearchResult',
  Search = '[Tea][Search] Search',
  SearchTermsError = '[Tea][Search] SearchTermsError',
  SearchTermsChanged = '[Tea][Search] SearchTermsChanged',
  ZoomImageToggle = '[Tea][Search] ZoomImageToggle',
}

export const search = () => createAction(SearchActionTypes.Search);
export const searchResult = (result: ISearchResult<ITeabag>) => createAction(SearchActionTypes.SearchResult, result);
export const searchTermChanged = (searchTerm: string) => createAction(SearchActionTypes.SearchTermsChanged, searchTerm);
export const searchTermError = (errorMessage: string) => createAction(SearchActionTypes.SearchTermsError, errorMessage);
export const zoomImageToggle = (imageid: string) => createAction(SearchActionTypes.ZoomImageToggle, imageid);
