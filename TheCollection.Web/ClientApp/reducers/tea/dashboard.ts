import * as moment from 'moment';
import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { AppThunkAction } from '../../store';
import { ICountBy } from '../../interfaces/ICountBy';
import { IRefValue } from '../../interfaces/IRefValue';
import { IPeriod } from '../../interfaces/IPeriod';
import { ChartType } from '../../types/Chart';
import {RECIEVE_COUNTBYPERIOD, REQUEST_COUNTBYPERIOD, CHANGE_CHARTTYPE, CHANGE_CHARTPERIOD, RECIEVE_COUNTBYREFVALUE, REQUEST_COUNTBYREFVALUE, UPDATE_COUNTBYPERIOD, UPDATE_COUNTBYREFVALUE} from '../../constants/dashboard/chart';
import {
  BAGSCOUNTBYBAGTYPES,
  BAGSCOUNTBYBRAND,
  BAGSCOUNTBYPERIOD,
  RECIEVE_BAGTYPECOUNT,
  RECIEVE_BRANDCOUNT,
  REQUEST_BAGTYPECOUNT,
  REQUEST_BRANDCOUNT,
  TOTALBAGSCOUNTBYPERIOD,
} from '../../constants/tea/dashboard';
import { ReceiveCountByRefValueAction, ReceiveCountByPeriodAction, RequestCountByPeriodAction, ChangeChartType, ChangeChartPeriod, UpdateCountByPeriodAction, UpdateCountByRefValueAction } from '../../actions/dashboard/chart';
import { ReceiveBagTypeCountAction, RequestBagTypeCountAction, ReceiveBrandCountAction, RequestBrandCountAction } from '../../actions/tea/dashboard';
import { requestBagTypeCount, requestBrandCount, requestCountByPeriod } from '../../thunks/tea/dashboard';
import { changeChartType, changeChartPeriod, updateCountByRefValue, updateCountByPeriod } from '../../thunks/dashboard/chart';

export type CountByRefValueTypes = 'brands' | 'bagtypes';
export type CountByPeriodTypes = 'added' | 'totalcount';

export type CountByChart<T> = {
  description: string;
  chartType: ChartType;
  isLoading: boolean;
  data: ICountBy<T>[];
  startDate?: moment.Moment; // refactor out, not common for all type usages
};

export interface IDashboardState {
  hasLoadedData?: boolean;
  countByRefValueCharts: {[k in CountByRefValueTypes]?: CountByChart<IRefValue>};
  countByPeriodCharts: {[k in CountByPeriodTypes]?: CountByChart<IPeriod>};
}

const countbybagtypes: CountByChart<IRefValue> = {description: 'Bag Types', data: [], chartType: 'pie', isLoading: false, startDate: undefined};
const countbybrands: CountByChart<IRefValue> = {description: 'Brands', data: [], chartType: 'bar', isLoading: false, startDate: undefined};
const countbyperiods: CountByChart<IPeriod> = {description: 'Added', data: [], chartType: 'line', isLoading: false, startDate: moment()};
const totalCountbyperiods: CountByChart<IPeriod> = {description: 'Total', data: [], chartType: 'line', isLoading: false, startDate: moment()};
const unloadedState: IDashboardState = { countByRefValueCharts: {bagtypes: countbybagtypes, brands: countbybrands}, countByPeriodCharts: {added: countbyperiods, totalcount: totalCountbyperiods} };

export const actionCreators = {...requestBagTypeCount, ...requestBrandCount, ...requestCountByPeriod, ...changeChartType, ...changeChartPeriod, ...updateCountByRefValue, ...updateCountByPeriod};

type DashboardActions = ReceiveCountByRefValueAction | ReceiveBagTypeCountAction | RequestBagTypeCountAction | ReceiveBrandCountAction | RequestBrandCountAction | ReceiveCountByPeriodAction | RequestCountByPeriodAction | ChangeChartType | ChangeChartPeriod | UpdateCountByPeriodAction | UpdateCountByRefValueAction;

export const reducer: Reducer<IDashboardState> = (state: IDashboardState, action: DashboardActions) => {
  switch (action.type) {
    case REQUEST_BAGTYPECOUNT:
      let unloadBagType = {...state.countByRefValueCharts, ...unMapChartDatas('bagtypes', state.countByRefValueCharts.bagtypes)};
      return {...state, ...{countByRefValueCharts: unloadBagType}};
    case RECIEVE_BAGTYPECOUNT:
      let bagtypeCount = {...state.countByRefValueCharts, ...mapChartDatas('bagtypes', state.countByRefValueCharts.bagtypes, action.data)};
      return  {...state, ...{countByRefValueCharts: bagtypeCount}, ...{hasLoadedData: true}};
    case REQUEST_BRANDCOUNT:
      let unloadBrandCount = {...state.countByRefValueCharts, ...unMapChartDatas('brands', state.countByRefValueCharts.brands)};
      return {...state, ...{countByRefValueCharts: unloadBrandCount}};
    case RECIEVE_BRANDCOUNT:
      let brandCount = {...state.countByRefValueCharts, ...mapChartDatas('brands', state.countByRefValueCharts.brands, action.data)};
      return  {...state, ...{countByRefValueCharts: brandCount}, ...{hasLoadedData: true}};
    case REQUEST_COUNTBYPERIOD:
      if (action.apipath === BAGSCOUNTBYPERIOD) {
        let unloadPeriodCount = {...state.countByPeriodCharts, ...unMapChartDatas('added', state.countByPeriodCharts.added)};
        return {...state, ...{countByPeriodCharts: unloadPeriodCount}};
      }

      if (action.apipath === TOTALBAGSCOUNTBYPERIOD) {
        let unloadPeriodCount = {...state.countByPeriodCharts, ...unMapChartDatas('totalcount', state.countByPeriodCharts.totalcount)};
        return {...state, ...{countByPeriodCharts: unloadPeriodCount}};
      }

      return state;
    case RECIEVE_COUNTBYPERIOD:
      if (action.apipath === BAGSCOUNTBYPERIOD) {
        let periodCount = {...state.countByPeriodCharts, ...mapChartDatas('added', state.countByPeriodCharts.added, action.data)};
        return  {...state, ...{countByPeriodCharts: periodCount}, ...{hasLoadedData: true}};
      }

      if (action.apipath === TOTALBAGSCOUNTBYPERIOD) {
        let periodCount = {...state.countByPeriodCharts, ...mapChartDatas('totalcount', state.countByPeriodCharts.totalcount, action.data)};
        return  {...state, ...{countByPeriodCharts: periodCount}, ...{hasLoadedData: true}};
      }

      return state;
    case CHANGE_CHARTTYPE:
      let newChartType = {...state.countByRefValueCharts[action.chartId], ...{chartType: action.charttype}};
      let newChartTypes = {...state.countByRefValueCharts, ...{[action.chartId]: newChartType}};
      return {...state, ...{countByRefValueCharts: newChartTypes}};
    case CHANGE_CHARTPERIOD:
      let newStartPeriod = {...state.countByPeriodCharts[action.chartId], ...{startDate: action.startPeriod}};
      let newStartPeriods = {...state.countByPeriodCharts, ...{[action.chartId]: newStartPeriod}};
      return {...state, ...{countByPeriodCharts: newStartPeriods}};
    case RECIEVE_COUNTBYREFVALUE:
      if (action.apipath === BAGSCOUNTBYBRAND) {
        let refValueCount = {...state.countByRefValueCharts, ...mapChartDatas('brands', state.countByRefValueCharts.brands, action.data)};
        return  {...state, ...{countByRefValueCharts: refValueCount}, ...{hasLoadedData: true}};
      }

      if (action.apipath === BAGSCOUNTBYBAGTYPES) {
        let refValueCount = {...state.countByRefValueCharts, ...mapChartDatas('bagtypes', state.countByRefValueCharts.bagtypes, action.data)};
        return  {...state, ...{countByRefValueCharts: refValueCount}, ...{hasLoadedData: true}};
      }

      return state;
    case REQUEST_COUNTBYREFVALUE:
      if (action.apipath === BAGSCOUNTBYBRAND) {
        let unloadRefValueCount = {...state.countByRefValueCharts, ...unMapChartDatas('brands', state.countByRefValueCharts.brands)};
        return {...state, ...{countByRefValueCharts: unloadRefValueCount}};
      }

      if (action.apipath === BAGSCOUNTBYBAGTYPES) {
        let unloadRefValueCount = {...state.countByRefValueCharts, ...unMapChartDatas('bagtypes', state.countByRefValueCharts.bagtypes)};
        return {...state, ...{countByRefValueCharts: unloadRefValueCount}};
      }

      return state;
    default:
      const exhaustiveCheck: never = action;
  }

  return state || unloadedState;
};

function mapChartDatas<T>(chart: CountByRefValueTypes | CountByPeriodTypes, previousState: CountByChart<T>, newData: ICountBy<T>[]) {
  return {[chart]: mapChartData(previousState, newData)};
}

function mapChartData<T>(previousState: CountByChart<T>, newData: ICountBy<T>[]) {
  return {...previousState, ...{data: newData}, ...{isLoading: false}};
}

function unMapChartDatas<T>(chart: CountByRefValueTypes | CountByPeriodTypes, previousState: CountByChart<T>) {
  return {[chart]: unMapChartData(previousState)};
}

function unMapChartData<T>(previousState: CountByChart<T>) {
  return {...previousState, ...{data: []}, ...{isLoading: true}};
}
