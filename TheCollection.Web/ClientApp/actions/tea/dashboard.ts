import { ICountBy } from '../../interfaces/ICountBy';
import { IRefValue } from '../../interfaces/IRefValue';
import { IPeriod } from '../../interfaces/IPeriod';
import { RECIEVE_BAGTYPECOUNT, REQUEST_BAGTYPECOUNT, RECIEVE_BRANDCOUNT, REQUEST_BRANDCOUNT, RECIEVE_COUNTBYPERIOD, REQUEST_COUNTBYPERIOD } from '../../constants/tea/dashboard';

export type ReceiveBagTypeCountAction = {
  type: RECIEVE_BAGTYPECOUNT;
  bagtypecount: ICountBy<IRefValue>[];
};

export type RequestBagTypeCountAction = {
  type: REQUEST_BAGTYPECOUNT;
};

export type ReceiveBrandCountAction = {
  type: RECIEVE_BRANDCOUNT;
  brandcount: ICountBy<IRefValue>[];
};

export type RequestBrandCountAction = {
  type: REQUEST_BRANDCOUNT;
};


export type ReceiveCountByPeriodAction = {
  type: RECIEVE_COUNTBYPERIOD;
  apipath: string;
  data: ICountBy<IPeriod>[];
};

export type RequesCountByPeriodAction = {
  type: REQUEST_COUNTBYPERIOD;
  apipath: string;
};
