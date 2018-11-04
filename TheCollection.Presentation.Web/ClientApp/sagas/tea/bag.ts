import { call, put, takeEvery } from 'redux-saga/effects';
import * as HttpClient from '../../util/HttpClient';
import { BagActionTypes, saveAction, clearRefValueAction, valueChangedAction, clearStringValueAction, requestAction, recieveAction } from '../../actions/tea/bag';
import { generateID } from '../../util';
import { UserFeedbackType } from '../../types/ui';
import { addUserFeedback } from '../../actions/ui';
import { ITeabag } from '../../interfaces/tea/IBag';
import { EmptyRefRecord, EmptyTeabagRecord } from '../../types';


function* clearRefValue(action: ReturnType<typeof clearRefValueAction>) {
  yield put(valueChangedAction(action.payload, EmptyRefRecord));
}

function* clearStringValue(action: ReturnType<typeof clearStringValueAction>) {
  yield put(valueChangedAction(action.payload, ''));
}

function* fetchBag(action: ReturnType<typeof requestAction>) {
  if (action.payload.id === undefined) {
    yield put(recieveAction(EmptyTeabagRecord));
    return;
  }

  try {
    const result: ITeabag = yield call(HttpClient.fetchJson, `/api/Tea/Bags/${action.payload.id}`);
    yield put(recieveAction(result));
  } catch (err) {
    yield put(recieveAction(EmptyTeabagRecord));
  }
}

function* save(action: ReturnType<typeof saveAction>) {
  try {
    const result: ITeabag = yield call(HttpClient.putJson, `/api/Tea/Bags/`, action.payload.id, action.payload);
  } catch (err) {
    yield put(addUserFeedback({type: UserFeedbackType.Error, id: generateID(), message: err.message}));
  }
}

export function* watchBag() {
  yield takeEvery(BagActionTypes.ClearRefValue, clearRefValue);
  yield takeEvery(BagActionTypes.ClearStringValue, clearStringValue);
  yield takeEvery(BagActionTypes.RequestBag, fetchBag);
  yield takeEvery(BagActionTypes.Save, save);
}
