import { fetch, addTask } from 'domain-task';
import { routerActions, RouterAction } from 'react-router-redux';
import { AppThunkAction } from '../store';
import { FIELD_ERROR, MODEL_ERROR } from '../constants/validation';
import { FieldErrorAction, ModelErrorAction } from '../actions/validation';

 export const fieldError = {
  fieldError: (field: string, message: string): AppThunkAction<FieldErrorAction> => (dispatch, getState) => {
    dispatch({type: FIELD_ERROR, error: {field: field, message: message}});
  },
 };

 export const modelError = {
  modelError: (errorMessage: string): AppThunkAction<ModelErrorAction> => (dispatch, getState) => {
    dispatch({type: MODEL_ERROR, errorMessage: errorMessage});
  },
 };

