import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { AppThunkAction } from '../../store';
import { ICountBy } from '../../interfaces/ICountBy';
import { IRefValue } from '../../interfaces/IRefValue';
import { IPeriod } from '../../interfaces/IPeriod';
import { ChartType } from '../../types/Chart';
import { BAGSCOUNTBYPERIOD, RECIEVE_BAGTYPECOUNT, REQUEST_BAGTYPECOUNT, RECIEVE_BRANDCOUNT, REQUEST_BRANDCOUNT, RECIEVE_COUNTBYPERIOD, REQUEST_COUNTBYPERIOD, CHANGE_CHARTTYPE } from '../../constants/tea/dashboard';
import { ReceiveBagTypeCountAction, RequestBagTypeCountAction, ReceiveBrandCountAction, RequestBrandCountAction, ReceiveCountByPeriodAction, RequesCountByPeriodAction, ChangeChartType } from '../../actions/tea/dashboard';
import { requestBagTypeCount, requestBrandCount, requestCountByPeriod, changeChartType } from '../../thunks/tea/dashboard';
import { descending } from 'd3';

export type CountByChart<T> = {
  description: string;
  chartType: ChartType;
  isLoading: boolean;
  data: ICountBy<T>[];
};

export interface IDashboardState {
  bagCountByPeriod?: ICountBy<IPeriod>[];
  countByRefValueCharts: {[index: string]: CountByChart<IRefValue>};
  countByPeriodCharts: {[index: string]: CountByChart<IPeriod>};
}

export const actionCreators = {...requestBagTypeCount, ...requestBrandCount, ...requestCountByPeriod, ...changeChartType};

const unloadedState: IDashboardState = { countByRefValueCharts: {}, countByPeriodCharts: {} };
type DashboardActions = ReceiveBagTypeCountAction | RequestBagTypeCountAction | ReceiveBrandCountAction | RequestBrandCountAction | ReceiveCountByPeriodAction | RequesCountByPeriodAction | ChangeChartType;
export const reducer: Reducer<IDashboardState> = (state: IDashboardState, action: DashboardActions) => {
  switch (action.type) {
    case REQUEST_BAGTYPECOUNT:
      let w = {...state.countByRefValueCharts.bagtypes, ...{isLoading: true, data: undefined, description: 'Bag Types'}};
      let s = {...state.countByRefValueCharts, ...{'bagtypes': w}};
      return {...state, ...{countByRefValueCharts: s}};
    case RECIEVE_BAGTYPECOUNT:
      let bagtypecount = {...state.countByRefValueCharts, ...createChartState('bagtypes', action.bagtypecount, 'pie', 'Bag Types')};
      return  {...state, ...{countByRefValueCharts: bagtypecount}};
    case REQUEST_BRANDCOUNT:
      let w2 = {...state.countByRefValueCharts.brands, ...{isLoading: true, data: undefined, description: 'Brands'}};
      let s2 = {...state.countByRefValueCharts, ...{'brands': w2}};
      return {...state, ...{countByRefValueCharts: s2}};
    case RECIEVE_BRANDCOUNT:
      let brandcount = {...state.countByRefValueCharts, ...createChartState('brands', action.brandcount, 'bar', 'Brands')};
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
      return  {...state, ...{countByPeriodCharts: periodcount}};
    case CHANGE_CHARTTYPE:
      let newChartType = {...state.countByRefValueCharts[action.chart], ...{chartType: action.charttype}};
      let newChartTypes = {...state.countByRefValueCharts, ...{[action.chart]: newChartType}};
      return {...state, ...{countByRefValueCharts: newChartTypes}};
    default:
      const exhaustiveCheck: never = action;
  }

  return state || unloadedState;
};

function changeToChartType(toChartType: ChartType, chart: string): {[index: string]: ChartType} {
  return {[chart]: toChartType};
}

function createChartState<T>(chart: string, data: ICountBy<T>[], toChartType: ChartType, description: string): {[index: string]: CountByChart<T>} {
  return {[chart]: {description: description, data: data, chartType: toChartType, isLoading: false}};
}
