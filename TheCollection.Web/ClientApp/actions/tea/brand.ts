import { IBrand } from '../../interfaces/tea/IBrand';
import { ADD_BRAND, CHANGE_NAME, RECIEVE_BRAND, REQUEST_BRAND } from '../../constants/tea/brand';

export type ReceiveBrandAction = {
  type: RECIEVE_BRAND;
  brand: IBrand;
};

export type RequestBrandAction = {
  type: REQUEST_BRAND;
  brandid: string;
};

export type AddBrandAction = {
  type: ADD_BRAND;
  brand: IBrand;
};

export type ChangeNameAction = {
  type: CHANGE_NAME;
  name: string;
};
