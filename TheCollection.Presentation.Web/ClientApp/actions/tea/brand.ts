import { IBrand } from '../../interfaces/tea/IBrand';

export enum BrandActionTypes {
  Add = '[Brand] Add',
  ChangeName = '[Brand] ChangeName',
  Recieve = '[Brand] Recieve',
  Request = '[Brand] Request',
}

export type ReceiveBrandAction = {
  type: BrandActionTypes.Recieve;
  brand: IBrand;
};

export type RequestBrandAction = {
  type: BrandActionTypes.Request;
  brandid: string;
};

export type AddBrandAction = {
  type: BrandActionTypes.Add;
  brand: IBrand;
};

export type ChangeNameAction = {
  type: BrandActionTypes.ChangeName;
  name: string;
};
