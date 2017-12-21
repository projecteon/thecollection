import { fetch, addTask } from 'domain-task';
import { routerActions, RouterAction } from 'react-router-redux';
import { AppThunkAction } from '../../store';
import { ICountBy } from '../../interfaces/ICountBy';
import { IRefValue } from '../../interfaces/IRefValue';
import { ChartType } from '../../types/Chart';
import {RECIEVE_COUNTBYPERIOD, REQUEST_COUNTBYPERIOD, CHANGE_CHARTTYPE} from '../../constants/dashboard/chart';
import { RECIEVE_BAGTYPECOUNT, REQUEST_BAGTYPECOUNT, RECIEVE_BRANDCOUNT, REQUEST_BRANDCOUNT } from '../../constants/tea/dashboard';
import { ReceiveCountByPeriodAction, RequestCountByPeriodAction, ChangeChartType } from '../../actions/dashboard/chart';
import { ReceiveBagTypeCountAction, RequestBagTypeCountAction, ReceiveBrandCountAction, RequestBrandCountAction} from '../../actions/tea/dashboard';

export const requestBagTypeCount = {
   requestBagTypeCount: (): AppThunkAction<ReceiveBagTypeCountAction | RequestBagTypeCountAction> => (dispatch, getState) => {
    try {
      let fetchTask = fetch(`/api/Tea/Dashboards/BagTypes/`, { credentials: 'same-origin' })
        .then(response => response.json() as Promise<ICountBy<IRefValue>[]>)
        .then(data => {
          dispatch({ type: RECIEVE_BAGTYPECOUNT, data: data });
          addTask(fetchTask); // ensure server-side prerendering waits for this to complete
      })
      .catch(() => dispatch({ type: RECIEVE_BAGTYPECOUNT, data: undefined }));
    } catch (err) {
      dispatch({ type: RECIEVE_BAGTYPECOUNT, data: undefined });
    }

    dispatch({ type: REQUEST_BAGTYPECOUNT });
  },
 };

export const requestBrandCount = {
  requestBrandCount: (top?: number): AppThunkAction<ReceiveBrandCountAction | RequestBrandCountAction> => (dispatch, getState) => {
   try {
      let fetchTask = fetch(`/api/Tea/Dashboards/Brands/${top ? top : ''}`, { credentials: 'same-origin' })
        .then(response => response.json() as Promise<ICountBy<IRefValue>[]>)
        .then(data => {
          dispatch({ type: RECIEVE_BRANDCOUNT, data: data });
          addTask(fetchTask); // ensure server-side prerendering waits for this to complete
        })
        .catch(() => dispatch({ type: RECIEVE_BRANDCOUNT, data: undefined }));
   } catch (err) {
     dispatch({ type: RECIEVE_BRANDCOUNT, data: undefined });
   }

   dispatch({ type: REQUEST_BRANDCOUNT });
 },
};
