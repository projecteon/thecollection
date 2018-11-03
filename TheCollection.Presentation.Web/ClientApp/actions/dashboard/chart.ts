import { ICountBy } from '../../interfaces/ICountBy';
import { IRefValue } from '../../interfaces/IRefValue';
import { ChartType } from '../../types/Chart';
import { Moment } from 'moment';
import { CountByPeriodTypes, CountByRefValueTypes } from '../../reducers/tea/dashboard';
import { createAction } from '../../util/Redux';

export enum ChartActionTypes {
  ChangeChartPeriod = '[Chart] ChangeChartPeriod',
  ChangeChartType = '[Chart] ChangeChartType',
  RecieveCountByPeriod = '[Chart] RecieveCountByPeriod',
  RequestCountByPeriod = '[Chart] RequestCountByPeriod',
  RecieveCountByRefValue = '[Chart] RecieveCountByRefValue',
  RequestCountByRefValue = '[Chart] RequestCountByRefValue',
}

export const receiveCountByRefValueAction = (apipath: string, data: ICountBy<IRefValue>[]) => createAction(ChartActionTypes.RecieveCountByRefValue, {apipath, data});
export const requestCountByRefValueAction = (apipath: string, top?: number) => createAction(ChartActionTypes.RequestCountByRefValue, {apipath, top});
export const receiveCountByPeriodAction = (apipath: string, data: ICountBy<Moment>[]) => createAction(ChartActionTypes.RecieveCountByPeriod, {apipath, data});
export const requestCountByPeriodAction = (apipath: string, top?: number) => createAction(ChartActionTypes.RequestCountByPeriod, {apipath, top});
export const changeChartTypeAction = (charttype: ChartType, chartId: CountByRefValueTypes) => createAction(ChartActionTypes.ChangeChartType, {charttype, chartId});
export const changeChartPeriodAction = (startPeriod: Moment, chartId: CountByPeriodTypes) => createAction(ChartActionTypes.ChangeChartPeriod, {startPeriod, chartId});
