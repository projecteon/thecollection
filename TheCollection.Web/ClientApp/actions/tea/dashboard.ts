import { ICountBy } from '../../interfaces/ICountBy';
import {IRefValue} from '../../interfaces/IRefValue';
import { RECIEVE_BAGTYPECOUNT, REQUEST_BAGTYPECOUNT } from '../../constants/tea/dashboard';

export type ReceiveBagTypeCountAction = {
  type: RECIEVE_BAGTYPECOUNT;
  bagtypecount: ICountBy<IRefValue>[];
};

export type RequestBagTypeCountAction = {
  type: REQUEST_BAGTYPECOUNT;
};
