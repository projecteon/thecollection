import * as chart from '../../constants/dashboard/chart';
import { ICountBy } from '../../interfaces/ICountBy';
import { IRefValue } from '../../interfaces/IRefValue';
import { ChartType } from '../../types/Chart';
import { Moment } from 'moment';
import { CountByPeriodTypes, CountByRefValueTypes } from '../../reducers/tea/dashboard';

export type UpdateCountByRefValueAction = {
  type: chart.REQUEST_COUNTBYREFVALUE;
  apipath: string;
};

export type ReceiveCountByRefValueAction = {
  type: chart.RECIEVE_COUNTBYREFVALUE;
  apipath: string;
  data: ICountBy<IRefValue>[];
};

export type RequestCountByRefValueAction = {
  type: chart.REQUEST_COUNTBYREFVALUE;
  apipath: string;
};

export type UpdateCountByPeriodAction = {
  type: chart.REQUEST_COUNTBYPERIOD;
  apipath: string;
};

export type ReceiveCountByPeriodAction = {
  type: chart.RECIEVE_COUNTBYPERIOD;
  apipath: string;
  data: ICountBy<Moment>[];
};

export type RequestCountByPeriodAction = {
  type: chart.REQUEST_COUNTBYPERIOD;
  apipath: string;
};

export type ChangeChartType = {
  type: chart.CHANGE_CHARTTYPE;
  charttype: ChartType;
  chartId: CountByRefValueTypes;
};

export type ChangeChartPeriod = {
  type: chart.CHANGE_CHARTPERIOD;
  startPeriod: Moment;
  chartId: CountByPeriodTypes;
};
