import { fetch, addTask } from 'domain-task';
import { routerActions, RouterAction } from 'react-router-redux';
import { AppThunkAction } from '../../store';
import { IRefValue } from '../../interfaces/IRefValue';
import { ICountBy } from '../../interfaces/ICountBy';
import { RECIEVE_BAGTYPECOUNT, REQUEST_BAGTYPECOUNT } from '../../constants/tea/dashboard';
import { ReceiveBagTypeCountAction, RequestBagTypeCountAction } from '../../actions/tea/dashboard';

export const requestBagTypeCount = {
   requestBagTypeCount: (): AppThunkAction<ReceiveBagTypeCountAction | RequestBagTypeCountAction> => (dispatch, getState) => {
    try {
      let fetchTask = fetch(`/api/tea/Dashboard/BagTypes/`, { credentials: 'same-origin' })
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
