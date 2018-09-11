import { fetch, addTask } from 'domain-task';
import { routerActions, RouterAction } from 'react-router-redux';
import { AppThunkAction } from '../../store';
import { IBagType } from '../../interfaces/tea/IBagType';
import { BagTypeActionTypes, ChangeNameAction, ReceiveBagtypeAction, RequestBagtypeAction } from '../../actions/tea/bagtype';

import { BagActionTypes, ChangeBagTypeAction } from '../../actions/tea/bag';

export const requestBagtype = {
   requestBagtype: (bagtypeid?: string): AppThunkAction<ReceiveBagtypeAction | RequestBagtypeAction> => (dispatch, getState) => {
    if (bagtypeid === undefined) {
      dispatch({ type: BagTypeActionTypes.Recieve, bagtype: {} as IBagType });
      return;
    }

    try {
      let fetchTask = fetch(`/api/Tea/BagTypes/${bagtypeid}`, { credentials: 'same-origin' })
        .then(response => response.json() as Promise<IBagType>)
        .then(data => {
          dispatch({ type: BagTypeActionTypes.Recieve, bagtype: data });
          addTask(fetchTask); // ensure server-side prerendering waits for this to complete
      });
    } catch (err) {
      dispatch({ type: BagTypeActionTypes.Recieve, bagtype: {} as IBagType });
    }

    dispatch({ type: BagTypeActionTypes.Request, bagtypeid: bagtypeid });
  },
 };

export const addBagtype = {
  addBagtype: (bagtype: IBagType): AppThunkAction<ChangeBagTypeAction | RouterAction> => (dispatch, getState) => {
    try {
      let addBrandTask = fetch(`/api/Tea/BagTypes/`, {
          headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
          },
          credentials: 'same-origin',
          method: 'post',
          body: JSON.stringify(bagtype),
        })
        .then(response => response.json() as Promise<IBagType>)
        .then(data => {
          dispatch({type: BagActionTypes.ChangeBagType, bagtype: {...data, ...{canaddnew: true}}});
          dispatch(routerActions.goBack());
          addTask(addBrandTask); // ensure server-side prerendering waits for this to complete
      });
    } catch (err) {
      console.log(err);
    }
  },
 };

export const changeName = {
  changeName: (name: string): AppThunkAction<ChangeNameAction> => (dispatch, getState) => {
    dispatch({type: BagTypeActionTypes.ChangeName, name: name});
  },
 };
