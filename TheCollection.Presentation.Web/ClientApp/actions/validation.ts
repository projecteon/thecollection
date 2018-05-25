import { FIELD_ERROR, MODEL_ERROR } from '../constants/validation';

export type FieldErrorAction = {
  type: FIELD_ERROR;
  error: {field: string, message: string};
};

export type ModelErrorAction = {
  type: MODEL_ERROR;
  errorMessage: string;
};
