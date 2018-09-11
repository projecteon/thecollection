import { IBagType } from '../../interfaces/tea/IBagType';

export enum BagTypeActionTypes {
  Add = '[BagType] Add',
  ChangeName = '[BagType] ChangeName',
  Recieve = '[BagType] Recieve',
  Request = '[BagType] Request',
}

export type ReceiveBagtypeAction = {
  type: BagTypeActionTypes.Recieve;
  bagtype: IBagType;
};

export type RequestBagtypeAction = {
  type: BagTypeActionTypes.Request;
  bagtypeid: string;
};

export type AddBagtypeAction = {
  type: BagTypeActionTypes.Add;
  bagtype: IBagType;
};

export type ChangeNameAction = {
  type: BagTypeActionTypes.ChangeName;
  name: string;
};
