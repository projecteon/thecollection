import { fetch, addTask } from 'domain-task';
import { routerActions, RouterAction } from 'react-router-redux';
import { AppThunkAction } from '../../store';
import { ICountBy } from '../../interfaces/ICountBy';
import { IPeriod } from '../../interfaces/IPeriod';
import { IRefValue } from '../../interfaces/IRefValue';
import { RECIEVE_BAGTYPECOUNT, REQUEST_BAGTYPECOUNT, RECIEVE_BRANDCOUNT, REQUEST_BRANDCOUNT, RECIEVE_COUNTBYPERIOD, REQUEST_COUNTBYPERIOD } from '../../constants/tea/dashboard';
import { ReceiveBagTypeCountAction, RequestBagTypeCountAction, ReceiveBrandCountAction, RequestBrandCountAction, ReceiveCountByPeriodAction, RequesCountByPeriodAction } from '../../actions/tea/dashboard';

export const requestBagTypeCount = {
   requestBagTypeCount: (): AppThunkAction<ReceiveBagTypeCountAction | RequestBagTypeCountAction> => (dispatch, getState) => {
    try {
      let fetchTask = fetch(`/api/tea/Dashboards/BagTypes/`, { credentials: 'same-origin' })
        .then(response => response.json() as Promise<ICountBy<IRefValue>[]>)
        .then(data => {
          dispatch({ type: RECIEVE_BAGTYPECOUNT, bagtypecount: data });
          addTask(fetchTask); // ensure server-side prerendering waits for this to complete
      });
    } catch (err) {
      dispatch({ type: RECIEVE_BAGTYPECOUNT, bagtypecount: undefined });
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
         dispatch({ type: RECIEVE_BRANDCOUNT, brandcount: data });
         addTask(fetchTask); // ensure server-side prerendering waits for this to complete
     });
   } catch (err) {
     dispatch({ type: RECIEVE_BRANDCOUNT, brandcount: undefined });
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
