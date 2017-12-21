import * as dashboard from '../../constants/tea/dashboard';
import { ICountBy } from '../../interfaces/ICountBy';
import { IRefValue } from '../../interfaces/IRefValue';
import { ChartType } from '../../types/Chart';

export type ReceiveBagTypeCountAction = {
  type: dashboard.RECIEVE_BAGTYPECOUNT;
  data: ICountBy<IRefValue>[];
};

export type RequestBagTypeCountAction = {
  type: dashboard.REQUEST_BAGTYPECOUNT;
};

export type ReceiveBrandCountAction = {
  type: dashboard.RECIEVE_BRANDCOUNT;
  data: ICountBy<IRefValue>[];
};

export type RequestBrandCountAction = {
  type: dashboard.REQUEST_BRANDCOUNT;
};
