import { IBagType } from '../interfaces/IBagType';
import { ADD_BAGTYPE, CHANGE_NAME, RECIEVE_BAGTYPE, REQUEST_BAGTYPE } from '../constants/bagtype';

export type ReceiveBagtypeAction = {
  type: RECIEVE_BAGTYPE;
  bagtype: IBagType;
};

export type RequestBagtypeAction = {
  type: REQUEST_BAGTYPE;
  bagtypeid: string;
};

export type AddBagtypeAction = {
  type: ADD_BAGTYPE;
  bagtype: IBagType;
};

export type ChangeNameAction = {
  type: CHANGE_NAME;
  name: string;
};
