import * as moment from 'moment';
import * as HttpClient from '../../util/HttpClient';
import { put, call, takeEvery } from 'redux-saga/effects';
import { ICountBy } from '../../interfaces/ICountBy';
import { IRefValue } from '../../interfaces/IRefValue';
import { ChartActionTypes, requestCountByRefValueAction, receiveCountByRefValueAction, requestCountByPeriodAction, receiveCountByPeriodAction } from '../../actions/dashboard/chart';

function* fetchCountByRefValue(action: ReturnType<typeof requestCountByRefValueAction>) {
  try {
    const result: ICountBy<IRefValue>[] = yield call(HttpClient.fetchJson, `${action.payload.apipath}${action.payload.top ? action.payload.top : ''}`);
    yield put(receiveCountByRefValueAction(action.payload.apipath, result));
  } catch (err) {
    yield put(receiveCountByRefValueAction(action.payload.apipath, []));
  }
}

function* fetchCountByPeriod(action: ReturnType<typeof requestCountByPeriodAction>) {
  try {
    const result: ICountBy<string>[] = yield call(HttpClient.fetchJson, `${action.payload.apipath}${action.payload.top ? action.payload.top : ''}`);
    const data = result.map(x => { return {count: x.count, value: moment(x.value)}; });
    yield put(receiveCountByPeriodAction(action.payload.apipath, data));
  } catch (err) {
    yield put(receiveCountByPeriodAction(action.payload.apipath, []));
  }
}

export function* watchDashboard() {
  yield takeEvery(ChartActionTypes.RequestCountByRefValue, fetchCountByRefValue);
  yield takeEvery(ChartActionTypes.RequestCountByPeriod, fetchCountByPeriod);
}
