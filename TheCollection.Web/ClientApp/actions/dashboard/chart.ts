import * as chart from '../../constants/dashboard/chart';
import { ICountBy } from '../../interfaces/ICountBy';
import { IRefValue } from '../../interfaces/IRefValue';
import { IPeriod } from '../../interfaces/IPeriod';
import { ChartType } from '../../types/Chart';

export type ReceiveCountByPeriodAction = {
  type: chart.RECIEVE_COUNTBYPERIOD;
  apipath: string;
  data: ICountBy<IPeriod>[];
};

export type RequesCountByPeriodAction = {
  type: chart.REQUEST_COUNTBYPERIOD;
  apipath: string;
};

export type ChangeChartType = {
  type: chart.CHANGE_CHARTTYPE;
  charttype: ChartType;
  chartId: string;
};
