import * as moment from 'moment';
import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { AppThunkAction } from '../../store';
import { ICountBy } from '../../interfaces/ICountBy';
import { IRefValue } from '../../interfaces/IRefValue';
import { IPeriod } from '../../interfaces/IPeriod';
import { ChartType } from '../../types/Chart';
import {RECIEVE_COUNTBYPERIOD, REQUEST_COUNTBYPERIOD, CHANGE_CHARTTYPE, CHANGE_CHARTPERIOD} from '../../constants/dashboard/chart';
import { BAGSCOUNTBYPERIOD, RECIEVE_BAGTYPECOUNT, REQUEST_BAGTYPECOUNT, RECIEVE_BRANDCOUNT, REQUEST_BRANDCOUNT } from '../../constants/tea/dashboard';
import { ReceiveCountByPeriodAction, RequesCountByPeriodAction, ChangeChartType, ChangeChartPeriod } from '../../actions/dashboard/chart';
import { ReceiveBagTypeCountAction, RequestBagTypeCountAction, ReceiveBrandCountAction, RequestBrandCountAction } from '../../actions/tea/dashboard';
import { requestBagTypeCount, requestBrandCount, requestCountByPeriod } from '../../thunks/tea/dashboard';
import { changeChartType, changeChartPeriod } from '../../thunks/dashboard/chart';
// import { descending } from 'd3';

export type CountByChart<T> = {
  description: string;
  chartType: ChartType;
  isLoading: boolean;
  data: ICountBy<T>[];
  startDate?: moment.Moment; // refactor out, not common for all type usages
};

export interface IDashboardState {
  bagCountByPeriod?: ICountBy<IPeriod>[];
  countByRefValueCharts: {[index: string]: CountByChart<IRefValue>};
  countByPeriodCharts: {[index: string]: CountByChart<IPeriod>};
}

export const actionCreators = {...requestBagTypeCount, ...requestBrandCount, ...requestCountByPeriod, ...changeChartType, ...changeChartPeriod};

const unloadedState: IDashboardState = { countByRefValueCharts: {}, countByPeriodCharts: {} };
type DashboardActions = ReceiveBagTypeCountAction | RequestBagTypeCountAction | ReceiveBrandCountAction | RequestBrandCountAction | ReceiveCountByPeriodAction | RequesCountByPeriodAction | ChangeChartType | ChangeChartPeriod;
export const reducer: Reducer<IDashboardState> = (state: IDashboardState, action: DashboardActions) => {
  switch (action.type) {
    case REQUEST_BAGTYPECOUNT:
      let w = {...state.countByRefValueCharts.bagtypes, ...{isLoading: true, data: undefined, description: 'Bag Types'}};
      let s = {...state.countByRefValueCharts, ...{'bagtypes': w}};
      return {...state, ...{countByRefValueCharts: s}};
    case RECIEVE_BAGTYPECOUNT:
      let bagtypecount = {...state.countByRefValueCharts, ...createChartState('bagtypes', action.data, 'pie', 'Bag Types')};
      return  {...state, ...{countByRefValueCharts: bagtypecount}};
    case REQUEST_BRANDCOUNT:
      let w2 = {...state.countByRefValueCharts.brands, ...{isLoading: true, data: undefined, description: 'Brands'}};
      let s2 = {...state.countByRefValueCharts, ...{'brands': w2}};
      return {...state, ...{countByRefValueCharts: s2}};
    case RECIEVE_BRANDCOUNT:
      let brandcount = {...state.countByRefValueCharts, ...createChartState('brands', action.data, 'bar', 'Brands')};
      return  {...state, ...{countByRefValueCharts: brandcount}};
    case REQUEST_COUNTBYPERIOD:
      let w3 = {...state.countByPeriodCharts.added, ...{isLoading: true, data: undefined, description: 'Added'}};
      let s3 = {...state.countByPeriodCharts, ...{'added': w3}};
      return {...state, ...{countByPeriodCharts: s3}};
    case RECIEVE_COUNTBYPERIOD:
      if (action.apipath !== BAGSCOUNTBYPERIOD) {
        return state;
      }

      let periodcount = {...state.countByPeriodCharts, ...createChartState('added', action.data, 'line', 'Added')};
      periodcount.added.startDate = moment();
      return  {...state, ...{countByPeriodCharts: periodcount}};
    case CHANGE_CHARTTYPE:
      let newChartType = {...state.countByRefValueCharts[action.chartId], ...{chartType: action.charttype}};
      let newChartTypes = {...state.countByRefValueCharts, ...{[action.chartId]: newChartType}};
      return {...state, ...{countByRefValueCharts: newChartTypes}};
    case CHANGE_CHARTPERIOD:
      let newStartPeriod = {...state.countByPeriodCharts[action.chartId], ...{startDate: action.startPeriod}};
      let newStartPeriods = {...state.countByPeriodCharts, ...{[action.chartId]: newStartPeriod}};
      return {...state, ...{countByPeriodCharts: newStartPeriods}};
    default:
      const exhaustiveCheck: never = action;
  }

  return state || unloadedState;
};

function createChartState<T>(chart: string, data: ICountBy<T>[], toChartType: ChartType, description: string) {
  return {[chart]: {description: description, data: data, chartType: toChartType, isLoading: false, startDate: undefined}};
}
