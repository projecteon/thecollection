import { AppThunkAction } from '../store';
import { FieldErrorAction, ModelErrorAction, ValidationActionTypes } from '../actions/validation';

export const fieldError = {
  fieldError: (field: string, message: string): AppThunkAction<FieldErrorAction> => (dispatch, getState) => {
    dispatch({type: ValidationActionTypes.FieldError, error: {field: field, message: message}});
  },
};

export const modelError = {
  modelError: (errorMessage: string): AppThunkAction<ModelErrorAction> => (dispatch, getState) => {
    dispatch({type: ValidationActionTypes.ModelError, errorMessage: errorMessage});
  },
};

