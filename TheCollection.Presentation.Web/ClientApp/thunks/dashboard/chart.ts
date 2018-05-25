import * as moment from 'moment';
import { fetch, addTask } from 'domain-task';
import { routerActions, RouterAction } from 'react-router-redux';
import {IRefValue} from '../../interfaces/IRefValue';
import {ICountBy} from '../../interfaces/ICountBy';
import { AppThunkAction } from '../../store';
import { ChartType } from '../../types/Chart';
import {
  CHANGE_CHARTPERIOD,
  CHANGE_CHARTTYPE,
  RECIEVE_COUNTBYREFVALUE,
  RECIEVE_COUNTBYPERIOD,
  REQUEST_COUNTBYPERIOD,
  REQUEST_COUNTBYREFVALUE,
  UPDATE_COUNTBYPERIOD,
  UPDATE_COUNTBYREFVALUE,
} from '../../constants/dashboard/chart';
import { ChangeChartType, ChangeChartPeriod, ReceiveCountByRefValueAction, ReceiveCountByPeriodAction, RequestCountByPeriodAction, RequestCountByRefValueAction, UpdateCountByPeriodAction, UpdateCountByRefValueAction } from '../../actions/dashboard/chart';

export const updateCountByRefValue = {
  updateCountByRefValue: (apipath: string, top?: number): AppThunkAction<ReceiveCountByRefValueAction | UpdateCountByRefValueAction> => (dispatch, getState) => {
   try {
    let path = top === undefined ? apipath : apipath + top;
    let fetchTask = fetch(path, {
        credentials: 'same-origin',
        method: 'put',
        body: JSON.stringify(''),
      })
      .then(response => response.json() as Promise<ICountBy<IRefValue>[]>)
      .then(data => {
        dispatch({ type: RECIEVE_COUNTBYREFVALUE, data: data, apipath: apipath });
        addTask(fetchTask); // ensure server-side prerendering waits for this to complete
      })
      .catch(() => dispatch({ type: RECIEVE_COUNTBYREFVALUE, data: undefined, apipath: apipath }));
   } catch (err) {
     dispatch({ type: RECIEVE_COUNTBYREFVALUE, data: undefined, apipath: apipath });
   }

   dispatch({ type: REQUEST_COUNTBYREFVALUE, apipath: apipath });
 },
};

export const updateCountByPeriod = {
  updateCountByPeriod: (apipath: string, top?: number): AppThunkAction<ReceiveCountByPeriodAction | UpdateCountByPeriodAction> => (dispatch, getState) => {
   try {
    let path = top === undefined ? apipath : apipath + top;
    let fetchTask = fetch(path, {
        credentials: 'same-origin',
        method: 'put',
        body: JSON.stringify(''),
      })
      .then(response => response.json() as Promise<ICountBy<string>[]>)
      .then(data => data.map(x => { return {count: x.count, value: moment(x.value)}; }))
      .then(data => {
        dispatch({ type: RECIEVE_COUNTBYPERIOD, data: data, apipath: apipath });
        addTask(fetchTask); // ensure server-side prerendering waits for this to complete
      })
      .catch(() => dispatch({ type: RECIEVE_COUNTBYPERIOD, data: undefined, apipath: apipath }));
   } catch (err) {
     dispatch({ type: RECIEVE_COUNTBYPERIOD, data: undefined, apipath: apipath });
   }

   dispatch({ type: REQUEST_COUNTBYPERIOD, apipath: apipath });
 },
};

export const requestCountByPeriod = {
  requestCountByPeriod: (apipath: string): AppThunkAction<ReceiveCountByPeriodAction | RequestCountByPeriodAction> => (dispatch, getState) => {
   try {
    let fetchTask = fetch(apipath, { credentials: 'same-origin' })
      .then(response => response.json() as Promise<ICountBy<string>[]>)
      .then(data => data.map(x => { return {count: x.count, value: moment(x.value)}; }))
      .then(data => {
        dispatch({ type: RECIEVE_COUNTBYPERIOD, data: data, apipath: apipath });
        addTask(fetchTask); // ensure server-side prerendering waits for this to complete
      })
      .catch(() => dispatch({ type: RECIEVE_COUNTBYPERIOD, data: undefined, apipath: apipath }));
   } catch (err) {
     dispatch({ type: RECIEVE_COUNTBYPERIOD, data: undefined, apipath: apipath });
   }

   dispatch({ type: REQUEST_COUNTBYPERIOD, apipath: apipath });
 },
};

export const requestCountByRefValue = {
  requestCountByRefValue: (apipath: string, top?: number): AppThunkAction<ReceiveCountByRefValueAction | RequestCountByRefValueAction> => (dispatch, getState) => {
   try {
    let path = top === undefined ? apipath : apipath + top;
    let fetchTask = fetch(path, { credentials: 'same-origin' })
      .then(response => response.json() as Promise<ICountBy<IRefValue>[]>)
      .then(data => {
        dispatch({ type: RECIEVE_COUNTBYREFVALUE, data: data, apipath: apipath });
        addTask(fetchTask); // ensure server-side prerendering waits for this to complete
      })
      .catch(() => dispatch({ type: RECIEVE_COUNTBYREFVALUE, data: undefined, apipath: apipath }));
   } catch (err) {
     dispatch({ type: RECIEVE_COUNTBYREFVALUE, data: undefined, apipath: apipath });
   }

   dispatch({ type: REQUEST_COUNTBYREFVALUE, apipath: apipath });
 },
};

export const changeChartType = {
  changeChartType: (charttype: ChartType, chartId: string): AppThunkAction<ChangeChartType> => (dispatch, getState) => {
    dispatch({ type: CHANGE_CHARTTYPE, charttype: charttype, chartId: chartId });
  },
};

export const changeChartPeriod = {
  changeChartPeriod: (startPeriod: moment.Moment, chartId: string): AppThunkAction<ChangeChartPeriod> => (dispatch, getState) => {
    dispatch({ type: CHANGE_CHARTPERIOD, startPeriod: startPeriod, chartId: chartId });
  },
};
