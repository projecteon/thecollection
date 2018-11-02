import { IBagType } from '../../interfaces/tea/IBagType';
import { createAction } from '../../util/Redux';

export enum BagTypeActionTypes {
  Add = '[BagType] Add',
  ChangeName = '[BagType] ChangeName',
  Recieve = '[BagType] Recieve',
  Request = '[BagType] Request',
}

export const requestAction = (id: string) => createAction(BagTypeActionTypes.Request, id);
export const recieveAction = (bagType: IBagType) => createAction(BagTypeActionTypes.Recieve, bagType);
export const addAction = (bagType: IBagType) => createAction(BagTypeActionTypes.Add, bagType);
export const valueChangedAction = <K extends keyof IBagType>(property: K, value: IBagType[K]) => createAction(BagTypeActionTypes.ChangeName, {property, value});
