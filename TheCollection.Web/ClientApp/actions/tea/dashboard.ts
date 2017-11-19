import { ICountBy } from '../../interfaces/ICountBy';
import { IRefValue } from '../../interfaces/IRefValue';
import { IPeriod } from '../../interfaces/IPeriod';
import * as dashboard from '../../constants/tea/dashboard';
import { ChartType } from '../../types/Chart';

export type ReceiveBagTypeCountAction = {
  type: dashboard.RECIEVE_BAGTYPECOUNT;
  bagtypecount: ICountBy<IRefValue>[];
};

export type RequestBagTypeCountAction = {
  type: dashboard.REQUEST_BAGTYPECOUNT;
};

export type ReceiveBrandCountAction = {
  type: dashboard.RECIEVE_BRANDCOUNT;
  brandcount: ICountBy<IRefValue>[];
};

export type RequestBrandCountAction = {
  type: dashboard.REQUEST_BRANDCOUNT;
};


export type ReceiveCountByPeriodAction = {
  type: dashboard.RECIEVE_COUNTBYPERIOD;
  apipath: string;
  data: ICountBy<IPeriod>[];
};

export type RequesCountByPeriodAction = {
  type: dashboard.REQUEST_COUNTBYPERIOD;
  apipath: string;
};

export type ChangeChartType = {
  type: dashboard.CHANGE_CHARTTYPE;
  charttype: ChartType;
};
