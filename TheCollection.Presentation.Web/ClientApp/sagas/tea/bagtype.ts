import { requestAction, recieveAction, BagTypeActionTypes, addAction } from '../../actions/tea/bagtype';
import * as HttpClient from '../../util/HttpClient';
import { put, call, takeEvery } from 'redux-saga/effects';
import { IBagType } from '../../interfaces/tea/IBagType';
import { UserFeedbackType } from '../../types/ui';
import { addUserFeedback } from '../../actions/ui';
import { generateID } from '../../util';
import { EmptyRefRecord } from '../../types';
import { routerActions } from 'react-router-redux';
import { valueChangedAction } from '../../actions/tea/bag';
import { PropNames } from '../../interfaces/tea/IBag';


function* fetchBagType(action: ReturnType<typeof requestAction>) {
  if (action.payload === undefined) {
    yield put(recieveAction(EmptyRefRecord));
    return;
  }

  try {
    const result: IBagType = yield call(HttpClient.fetchJson, `/api/Tea/BagTypes/${action.payload}`);
    yield put(recieveAction(result));
  } catch (err) {
    yield put(recieveAction(EmptyRefRecord));
    yield put(addUserFeedback({type: UserFeedbackType.Error, id: generateID(), message: err.message}));
  }
}

function* addBagType(action: ReturnType<typeof addAction>) {
  try {
    const data: IBagType = yield call(HttpClient.postJson, `/api/Tea/BagTypes/`, action.payload);
    yield put(valueChangedAction(PropNames.bagType, {...data, ...{canaddnew: true}}));
    yield put(routerActions.goBack());
  } catch (err) {
    yield put(addUserFeedback({type: UserFeedbackType.Error, id: generateID(), message: err.message}));
  }
}

export function* watchBagType() {
  yield takeEvery(BagTypeActionTypes.Request, fetchBagType);
  yield takeEvery(BagTypeActionTypes.Add, addBagType);
}
