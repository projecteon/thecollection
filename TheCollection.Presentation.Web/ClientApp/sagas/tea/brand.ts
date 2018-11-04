import { requestAction, recieveAction, BrandActionTypes, addAction } from '../../actions/tea/brand';
import * as HttpClient from '../../util/HttpClient';
import { put, call, takeEvery } from 'redux-saga/effects';
import { IBrand } from '../../interfaces/tea/IBrand';
import { UserFeedbackType } from '../../types/ui';
import { addUserFeedback } from '../../actions/ui';
import { generateID } from '../../util';
import { EmptyRefRecord } from '../../types';
import { routerActions } from 'react-router-redux';
import { valueChangedAction } from '../../actions/tea/bag';
import { PropNames } from '../../interfaces/tea/IBag';


function* fetchBrand(action: ReturnType<typeof requestAction>) {
  if (action.payload === undefined) {
    yield put(recieveAction(EmptyRefRecord));
    return;
  }

  try {
    const result: IBrand = yield call(HttpClient.fetchJson, `/api/Tea/Brands/${action.payload}`);
    yield put(recieveAction(result));
  } catch (err) {
    yield put(recieveAction(EmptyRefRecord));
    yield put(addUserFeedback({type: UserFeedbackType.Error, id: generateID(), message: err.message}));
  }
}

function* addBrand(action: ReturnType<typeof addAction>) {
  try {
    const data: IBrand = yield call(HttpClient.postJson, `/api/Tea/Brands/`, action.payload);
    yield put(valueChangedAction(PropNames.brand, {...data, ...{canaddnew: true}}));
    yield put(routerActions.goBack());
  } catch (err) {
    yield put(addUserFeedback({type: UserFeedbackType.Error, id: generateID(), message: err.message}));
  }
}

export function* watchBrand() {
  yield takeEvery(BrandActionTypes.Request, fetchBrand);
  yield takeEvery(BrandActionTypes.Add, addBrand);
}
