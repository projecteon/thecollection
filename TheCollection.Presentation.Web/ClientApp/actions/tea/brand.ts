import { IBrand } from '../../interfaces/tea/IBrand';
import { createAction } from '../../util/Redux';

export enum BrandActionTypes {
  Add = '[Brand] Add',
  ChangeName = '[Brand] ChangeName',
  Recieve = '[Brand] Recieve',
  Request = '[Brand] Request',
}

export const requestAction = (id: string) => createAction(BrandActionTypes.Request, id);
export const recieveAction = (brand: IBrand) => createAction(BrandActionTypes.Recieve, brand);
export const addAction = (brand: IBrand) => createAction(BrandActionTypes.Add, brand);
export const valueChangedAction = <K extends keyof IBrand>(property: K, value: IBrand[K]) => createAction(BrandActionTypes.ChangeName, {property, value});
