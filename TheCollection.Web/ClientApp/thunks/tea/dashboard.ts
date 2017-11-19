import { fetch, addTask } from 'domain-task';
import { routerActions, RouterAction } from 'react-router-redux';
import { AppThunkAction } from '../../store';
import { ICountBy } from '../../interfaces/ICountBy';
import { IPeriod } from '../../interfaces/IPeriod';
import { IRefValue } from '../../interfaces/IRefValue';
import { ChartType } from '../../types/Chart';
import {RECIEVE_COUNTBYPERIOD, REQUEST_COUNTBYPERIOD, CHANGE_CHARTTYPE} from '../../constants/dashboard/chart';
import { RECIEVE_BAGTYPECOUNT, REQUEST_BAGTYPECOUNT, RECIEVE_BRANDCOUNT, REQUEST_BRANDCOUNT } from '../../constants/tea/dashboard';
import { ReceiveCountByPeriodAction, RequesCountByPeriodAction, ChangeChartType } from '../../actions/dashboard/chart';
import { ReceiveBagTypeCountAction, RequestBagTypeCountAction, ReceiveBrandCountAction, RequestBrandCountAction} from '../../actions/tea/dashboard';

export const requestBagTypeCount = {
   requestBagTypeCount: (): AppThunkAction<ReceiveBagTypeCountAction | RequestBagTypeCountAction> => (dispatch, getState) => {
    try {
      let fetchTask = fetch(`/api/tea/Dashboards/BagTypes/`, { credentials: 'same-origin' })
        .then(response => response.json() as Promise<ICountBy<IRefValue>[]>)
        .then(data => {
          dispatch({ type: RECIEVE_BAGTYPECOUNT, data: data });
          addTask(fetchTask); // ensure server-side prerendering waits for this to complete
      });
    } catch (err) {
      dispatch({ type: RECIEVE_BAGTYPECOUNT, data: undefined });
    }

    dispatch({ type: REQUEST_BAGTYPECOUNT });
  },
 };

export const requestBrandCount = {
  requestBrandCount: (): AppThunkAction<ReceiveBrandCountAction | RequestBrandCountAction> => (dispatch, getState) => {
   try {
     let fetchTask = fetch(`/api/tea/Dashboards/Brands/`, { credentials: 'same-origin' })
       .then(response => response.json() as Promise<ICountBy<IRefValue>[]>)
       .then(data => {
         dispatch({ type: RECIEVE_BRANDCOUNT, data: data });
         addTask(fetchTask); // ensure server-side prerendering waits for this to complete
     });
   } catch (err) {
     dispatch({ type: RECIEVE_BRANDCOUNT, data: undefined });
   }

   dispatch({ type: REQUEST_BRANDCOUNT });
 },
};

export const requestCountByPeriod = {
  requestCountByPeriod: (apipath: string): AppThunkAction<ReceiveCountByPeriodAction | RequesCountByPeriodAction> => (dispatch, getState) => {
   try {
     let fetchTask = fetch(apipath, { credentials: 'same-origin' })
       .then(response => response.json() as Promise<ICountBy<IPeriod>[]>)
       .then(data => {
         dispatch({ type: RECIEVE_COUNTBYPERIOD, data: data, apipath: apipath });
         addTask(fetchTask); // ensure server-side prerendering waits for this to complete
     });
   } catch (err) {
     dispatch({ type: RECIEVE_COUNTBYPERIOD, data: undefined, apipath: apipath });
   }

   dispatch({ type: REQUEST_COUNTBYPERIOD, apipath: apipath });
 },
};

export const changeChartType = {
  changeChartType: (charttype: ChartType, chart: string): AppThunkAction<ChangeChartType> => (dispatch, getState) => {
    dispatch({ type: CHANGE_CHARTTYPE, charttype: charttype, chartId: chart });
  },
};
