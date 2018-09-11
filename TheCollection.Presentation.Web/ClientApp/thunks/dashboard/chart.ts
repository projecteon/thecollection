import * as moment from 'moment';
import { fetch, addTask } from 'domain-task';
import {IRefValue} from '../../interfaces/IRefValue';
import {ICountBy} from '../../interfaces/ICountBy';
import { AppThunkAction } from '../../store';
import { ChartType } from '../../types/Chart';
import { ChangeChartType, ChangeChartPeriod, ChartActionTypes, ReceiveCountByRefValueAction, ReceiveCountByPeriodAction, RequestCountByPeriodAction, RequestCountByRefValueAction } from '../../actions/dashboard/chart';
import { CountByPeriodTypes, CountByRefValueTypes } from '../../reducers/tea/dashboard';

export const updateCountByRefValue = {
  updateCountByRefValue: (apipath: string, top?: number): AppThunkAction<ReceiveCountByRefValueAction | RequestCountByRefValueAction> => (dispatch, getState) => {
   try {
    let path = top === undefined ? apipath : apipath + top;
    let fetchTask = fetch(path, {
        credentials: 'same-origin',
        method: 'put',
        body: JSON.stringify(''),
      })
      .then(response => response.json() as Promise<ICountBy<IRefValue>[]>)
      .then(data => {
        dispatch({ type: ChartActionTypes.RecieveCountByRefValue, data: data, apipath: apipath });
        addTask(fetchTask); // ensure server-side prerendering waits for this to complete
      })
      .catch(() => dispatch({ type: ChartActionTypes.RecieveCountByRefValue, data: [], apipath: apipath }));
   } catch (err) {
     dispatch({ type: ChartActionTypes.RecieveCountByRefValue, data: [], apipath: apipath });
   }

   dispatch({ type: ChartActionTypes.RequestCountByRefValue, apipath: apipath });
 },
};

export const updateCountByPeriod = {
  updateCountByPeriod: (apipath: string, top?: number): AppThunkAction<ReceiveCountByPeriodAction | RequestCountByPeriodAction> => (dispatch, getState) => {
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
        dispatch({ type: ChartActionTypes.RecieveCountByCountPeriod, data: data, apipath: apipath });
        addTask(fetchTask); // ensure server-side prerendering waits for this to complete
      })
      .catch(() => dispatch({ type: ChartActionTypes.RecieveCountByCountPeriod, data: [], apipath: apipath }));
   } catch (err) {
     dispatch({ type: ChartActionTypes.RecieveCountByCountPeriod, data: [], apipath: apipath });
   }

   dispatch({ type: ChartActionTypes.RequestCountByCountPeriod, apipath: apipath });
 },
};

export const requestCountByPeriod = {
  requestCountByPeriod: (apipath: string): AppThunkAction<ReceiveCountByPeriodAction | RequestCountByPeriodAction> => (dispatch, getState) => {
   try {
    let fetchTask = fetch(apipath, { credentials: 'same-origin' })
      .then(response => response.json() as Promise<ICountBy<string>[]>)
      .then(data => data.map(x => { return {count: x.count, value: moment(x.value)}; }))
      .then(data => {
        dispatch({ type: ChartActionTypes.RecieveCountByCountPeriod, data: data, apipath: apipath });
        addTask(fetchTask); // ensure server-side prerendering waits for this to complete
      })
      .catch(() => dispatch({ type: ChartActionTypes.RecieveCountByCountPeriod, data: [], apipath: apipath }));
   } catch (err) {
     dispatch({ type: ChartActionTypes.RecieveCountByCountPeriod, data: [], apipath: apipath });
   }

   dispatch({ type: ChartActionTypes.RequestCountByCountPeriod, apipath: apipath });
 },
};

export const requestCountByRefValue = {
  requestCountByRefValue: (apipath: string, top?: number): AppThunkAction<ReceiveCountByRefValueAction | RequestCountByRefValueAction> => (dispatch, getState) => {
   try {
    let path = top === undefined ? apipath : apipath + top;
    let fetchTask = fetch(path, { credentials: 'same-origin' })
      .then(response => response.json() as Promise<ICountBy<IRefValue>[]>)
      .then(data => {
        dispatch({ type: ChartActionTypes.RecieveCountByRefValue, data: data, apipath: apipath });
        addTask(fetchTask); // ensure server-side prerendering waits for this to complete
      })
      .catch(() => dispatch({ type: ChartActionTypes.RecieveCountByRefValue, data: [], apipath: apipath }));
   } catch (err) {
     dispatch({ type: ChartActionTypes.RecieveCountByRefValue, data: [], apipath: apipath });
   }

   dispatch({ type: ChartActionTypes.RequestCountByRefValue, apipath: apipath });
 },
};

export const changeChartType = {
  changeChartType: (charttype: ChartType, chartId: CountByRefValueTypes): AppThunkAction<ChangeChartType> => (dispatch, getState) => {
    dispatch({ type: ChartActionTypes.ChangeChartType, charttype: charttype, chartId: chartId });
  },
};

export const changeChartPeriod = {
  changeChartPeriod: (startPeriod: moment.Moment, chartId: CountByPeriodTypes): AppThunkAction<ChangeChartPeriod> => (dispatch, getState) => {
    dispatch({ type: ChartActionTypes.ChangeChartPeriod, startPeriod: startPeriod, chartId: chartId });
  },
};
