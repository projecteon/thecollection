import * as moment from 'moment';
import * as chart from '../../constants/dashboard/chart';
import { ICountBy } from '../../interfaces/ICountBy';
import { IRefValue } from '../../interfaces/IRefValue';
import { IPeriod } from '../../interfaces/IPeriod';
import { ChartType } from '../../types/Chart';

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
  data: ICountBy<IPeriod>[];
};

export type RequestCountByPeriodAction = {
  type: chart.REQUEST_COUNTBYPERIOD;
  apipath: string;
};

export type ChangeChartType = {
  type: chart.CHANGE_CHARTTYPE;
  charttype: ChartType;
  chartId: string;
};

export type ChangeChartPeriod = {
  type: chart.CHANGE_CHARTPERIOD;
  startPeriod: moment.Moment;
  chartId: string;
};
