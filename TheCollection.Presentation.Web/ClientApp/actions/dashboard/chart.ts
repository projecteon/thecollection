import { ICountBy } from '../../interfaces/ICountBy';
import { IRefValue } from '../../interfaces/IRefValue';
import { ChartType } from '../../types/Chart';
import { Moment } from 'moment';
import { CountByPeriodTypes, CountByRefValueTypes } from '../../reducers/tea/dashboard';

export enum ChartActionTypes {
  ChangeChartPeriod = 'ChangeChartPeriod',
  ChangeChartType = 'ChangeChartType',
  RecieveCountByCountPeriod = 'RecieveCountByCountPeriod',
  RequestCountByCountPeriod = 'RequestCountByCountPeriod',
  RecieveCountByRefValue = 'RecieveCountByRefValue',
  RequestCountByRefValue = 'RequestCountByRefValue',
}

export type ReceiveCountByRefValueAction = {
  type: ChartActionTypes.RecieveCountByRefValue;
  apipath: string;
  data: ICountBy<IRefValue>[];
};

export type RequestCountByRefValueAction = {
  type: ChartActionTypes.RequestCountByRefValue;
  apipath: string;
};

export type ReceiveCountByPeriodAction = {
  type: ChartActionTypes.RecieveCountByCountPeriod;
  apipath: string;
  data: ICountBy<Moment>[];
};

export type RequestCountByPeriodAction = {
  type: ChartActionTypes.RequestCountByCountPeriod;
  apipath: string;
};

export type ChangeChartType = {
  type: ChartActionTypes.ChangeChartType;
  charttype: ChartType;
  chartId: CountByRefValueTypes;
};

export type ChangeChartPeriod = {
  type: ChartActionTypes.ChangeChartPeriod;
  startPeriod: Moment;
  chartId: CountByPeriodTypes;
};
