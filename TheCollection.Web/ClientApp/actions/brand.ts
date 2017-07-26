import { IBrand } from '../interfaces/IBrand';
import { ADD_BRAND, RECIEVE_BRAND, REQUEST_BRAND } from '../constants/brand';

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
