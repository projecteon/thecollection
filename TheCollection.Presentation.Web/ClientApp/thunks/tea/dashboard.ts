import { fetch, addTask } from 'domain-task';
import { AppThunkAction } from '../../store';
import { ICountBy } from '../../interfaces/ICountBy';
import { IRefValue } from '../../interfaces/IRefValue';
import { DashboardActionTypes, ReceiveBagTypeCountAction, RequestBagTypeCountAction, ReceiveBrandCountAction, RequestBrandCountAction} from '../../actions/tea/dashboard';

export const requestBagTypeCount = {
   requestBagTypeCount: (): AppThunkAction<ReceiveBagTypeCountAction | RequestBagTypeCountAction> => (dispatch, getState) => {
    try {
      let fetchTask = fetch(`/api/Tea/Dashboards/BagTypes/`, { credentials: 'same-origin' })
        .then(response => response.json() as Promise<ICountBy<IRefValue>[]>)
        .then(data => {
          dispatch({ type: DashboardActionTypes.RecieveBagTypeCount, data: data });
          addTask(fetchTask); // ensure server-side prerendering waits for this to complete
      })
      .catch(() => dispatch({ type: DashboardActionTypes.RecieveBagTypeCount, data: [] }));
    } catch (err) {
      dispatch({ type: DashboardActionTypes.RecieveBagTypeCount, data: [] });
    }

    dispatch({ type: DashboardActionTypes.RequestBagTypeCount });
  },
 };

export const requestBrandCount = {
  requestBrandCount: (top?: number): AppThunkAction<ReceiveBrandCountAction | RequestBrandCountAction> => (dispatch, getState) => {
   try {
      let fetchTask = fetch(`/api/Tea/Dashboards/Brands/${top ? top : ''}`, { credentials: 'same-origin' })
        .then(response => response.json() as Promise<ICountBy<IRefValue>[]>)
        .then(data => {
          dispatch({ type: DashboardActionTypes.RecieveBrandCount, data: data });
          addTask(fetchTask); // ensure server-side prerendering waits for this to complete
        })
        .catch(() => dispatch({ type: DashboardActionTypes.RecieveBrandCount, data: [] }));
   } catch (err) {
     dispatch({ type: DashboardActionTypes.RecieveBrandCount, data: [] });
   }

   dispatch({ type: DashboardActionTypes.RequestBrandCount });
 },
};
