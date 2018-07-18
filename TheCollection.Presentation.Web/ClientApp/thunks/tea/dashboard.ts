import { fetch, addTask } from 'domain-task';
import { AppThunkAction } from '../../store';
import { ICountBy } from '../../interfaces/ICountBy';
import { IRefValue } from '../../interfaces/IRefValue';
import { RECIEVE_BAGTYPECOUNT, REQUEST_BAGTYPECOUNT, RECIEVE_BRANDCOUNT, REQUEST_BRANDCOUNT } from '../../constants/tea/dashboard';
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
      .catch(() => dispatch({ type: RECIEVE_BAGTYPECOUNT, data: [] }));
    } catch (err) {
      dispatch({ type: RECIEVE_BAGTYPECOUNT, data: [] });
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
        .catch(() => dispatch({ type: RECIEVE_BRANDCOUNT, data: [] }));
   } catch (err) {
     dispatch({ type: RECIEVE_BRANDCOUNT, data: [] });
   }

   dispatch({ type: REQUEST_BRANDCOUNT });
 },
};
