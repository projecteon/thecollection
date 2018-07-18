import { fetch, addTask } from 'domain-task';
import { routerActions, RouterAction } from 'react-router-redux';
import { AppThunkAction } from '../../store';
import { IBagType } from '../../interfaces/tea/IBagType';
import { ADD_BAGTYPE, CHANGE_NAME, RECIEVE_BAGTYPE, REQUEST_BAGTYPE } from '../../constants/tea/bagtype';
import { AddBagtypeAction, ChangeNameAction, ReceiveBagtypeAction, RequestBagtypeAction } from '../../actions/tea/bagtype';

import { CHANGE_BAGTYPE } from '../../constants/tea/bag';
import { ChangeBagTypeAction } from '../../actions/tea/bag';

export const requestBagtype = {
   requestBagtype: (bagtypeid?: string): AppThunkAction<ReceiveBagtypeAction | RequestBagtypeAction> => (dispatch, getState) => {
    if (bagtypeid === undefined) {
      dispatch({ type: RECIEVE_BAGTYPE, bagtype: {} as IBagType });
      return;
    }

    try {
      let fetchTask = fetch(`/api/Tea/BagTypes/${bagtypeid}`, { credentials: 'same-origin' })
        .then(response => response.json() as Promise<IBagType>)
        .then(data => {
          dispatch({ type: RECIEVE_BAGTYPE, bagtype: data });
          addTask(fetchTask); // ensure server-side prerendering waits for this to complete
      });
    } catch (err) {
      dispatch({ type: RECIEVE_BAGTYPE, bagtype: {} as IBagType });
    }

    dispatch({ type: REQUEST_BAGTYPE, bagtypeid: bagtypeid });
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
          dispatch({type: CHANGE_BAGTYPE, bagtype: {...data, ...{canaddnew: true}}});
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
    dispatch({type: CHANGE_NAME, name: name});
  },
 };
