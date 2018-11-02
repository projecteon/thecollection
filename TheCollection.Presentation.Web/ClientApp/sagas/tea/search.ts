import { call, put, select, takeEvery } from 'redux-saga/effects';
import { IApplicationState } from '../../store';
import { ISearchState } from '../../reducers/tea/search';
import * as HttpClient from '../../util/HttpClient';
import { ISearchResult } from '../../interfaces/ISearchResult';
import { ITeabag } from '../../interfaces/tea/IBag';
import { addUserFeedback } from '../../actions/ui';
import { UserFeedbackType } from '../../types/ui';
import { generateID } from '../../util';
import { SearchActionTypes, searchResult } from '../../actions/tea/search';
import { EmptySearchResult } from '../../types/search';


const getBagSearch = (state: IApplicationState) => state.searchteabag;

function* searchBags() {
  try {
    const {searchedTerms} = (yield select(getBagSearch)) as ISearchState;
    if (searchedTerms.trim().length < 3) {
      yield put(addUserFeedback({type: UserFeedbackType.Error, id: generateID(), message: 'Requires at least 3 characters to search' }));
      return;
    }

    yield put(searchResult(EmptySearchResult));
    const result: ISearchResult<ITeabag> = yield call(HttpClient.fetchJson, `/api/Tea/Bags/?searchterm=${encodeURIComponent(searchedTerms)}`);
    if (result.data.length === 0) {
      yield put(addUserFeedback({type: UserFeedbackType.Information, id: generateID(), message: 'None found'}));
    } else {
      yield put(searchResult(result));
    }
  } catch (err) {
    yield put(addUserFeedback({type: UserFeedbackType.Error, id: generateID(), message: err.message}));
  }
}

export function* watchBagSearch() {
  yield takeEvery(SearchActionTypes.Search, searchBags);
}


