export enum ValidationActionTypes {
  FieldError = 'FieldError',
  ModelError = 'ModelError',
}

export type FieldErrorAction = {
  type: ValidationActionTypes.FieldError;
  error: {field: string, message: string};
};

export type ModelErrorAction = {
  type: ValidationActionTypes.ModelError;
  errorMessage: string;
};
