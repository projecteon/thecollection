import { requestAction, recieveAction, CountryActionTypes, addAction } from '../actions/country';
import * as HttpClient from '../util/HttpClient';
import { put, call, takeEvery } from 'redux-saga/effects';
import { ICountry } from '../interfaces/ICountry';
import { UserFeedbackType } from '../types/ui';
import { addUserFeedback } from '../actions/ui';
import { generateID } from '../util';
import { EmptyRefRecord } from '../types';
import { routerActions } from 'react-router-redux';
import { valueChangedAction } from '../actions/tea/bag';
import { PropNames } from '../interfaces/tea/IBag';


function* fetchCountry(action: ReturnType<typeof requestAction>) {
  if (action.payload === undefined) {
    yield put(recieveAction(EmptyRefRecord));
    return;
  }

  try {
    const result: ICountry = yield call(HttpClient.fetchJson, `/api/Tea/Countries/${action.payload}`);
    yield put(recieveAction(result));
  } catch (err) {
    yield put(recieveAction(EmptyRefRecord));
    yield put(addUserFeedback({type: UserFeedbackType.Error, id: generateID(), message: err.message}));
  }
}

function* addCountry(action: ReturnType<typeof addAction>) {
  try {
    const data: ICountry = yield call(HttpClient.postJson, `/api/Tea/Countries/`, action.payload);
    yield put(valueChangedAction(PropNames.country, {...data, ...{canaddnew: true}}));
    yield put(routerActions.goBack());
  } catch (err) {
    yield put(addUserFeedback({type: UserFeedbackType.Error, id: generateID(), message: err.message}));
  }
}

export function* watchCountry() {
  yield takeEvery(CountryActionTypes.Request, fetchCountry);
  yield takeEvery(CountryActionTypes.Add, addCountry);
}
