import produce from 'immer';
import * as moment from 'moment';
import { Reducer } from 'redux';
import { ICountBy } from '../../interfaces/ICountBy';
import { IRefValue } from '../../interfaces/IRefValue';
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
import { ReceiveCountByRefValueAction, ReceiveCountByPeriodAction, RequestCountByPeriodAction, ChangeChartType, ChangeChartPeriod, UpdateCountByPeriodAction, UpdateCountByRefValueAction, RequestCountByRefValueAction } from '../../actions/dashboard/chart';
import { ReceiveBagTypeCountAction, RequestBagTypeCountAction, ReceiveBrandCountAction, RequestBrandCountAction } from '../../actions/tea/dashboard';
import { changeChartType, changeChartPeriod, requestCountByPeriod, requestCountByRefValue, updateCountByRefValue, updateCountByPeriod } from '../../thunks/dashboard/chart';

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
  countByPeriodCharts: {[k in CountByPeriodTypes]?: CountByChart<moment.Moment>};
}

const countbybagtypes: CountByChart<IRefValue> = {description: 'Bag Types', data: [], chartType: 'pie', isLoading: false, startDate: undefined};
const countbybrands: CountByChart<IRefValue> = {description: 'Brands', data: [], chartType: 'bar', isLoading: false, startDate: undefined};
const countbyperiods: CountByChart<moment.Moment> = {description: 'Added', data: [], chartType: 'line', isLoading: false, startDate: moment()};
const totalCountbyperiods: CountByChart<moment.Moment> = {description: 'Total', data: [], chartType: 'line', isLoading: false, startDate: moment()};
const unloadedState: IDashboardState = { countByRefValueCharts: {bagtypes: countbybagtypes, brands: countbybrands}, countByPeriodCharts: {added: countbyperiods, totalcount: totalCountbyperiods} };

export const actionCreators = {...requestCountByPeriod, ...changeChartType, ...changeChartPeriod, ...updateCountByRefValue, ...updateCountByPeriod, ...requestCountByRefValue};

type DashboardActions = ReceiveCountByRefValueAction | RequestCountByRefValueAction | ReceiveCountByPeriodAction | RequestCountByPeriodAction | ChangeChartType | ChangeChartPeriod | UpdateCountByPeriodAction | UpdateCountByRefValueAction;

export const reducer: Reducer<IDashboardState, DashboardActions> = (state = unloadedState, action) =>
  produce(state, draft => {
    switch (action.type) {
      case REQUEST_COUNTBYPERIOD:
        if (action.apipath === BAGSCOUNTBYPERIOD && state.countByPeriodCharts.added) {
          draft.countByPeriodCharts.added = unMapChartData(state.countByPeriodCharts.added);
          break;
        }

        if (action.apipath === TOTALBAGSCOUNTBYPERIOD && state.countByPeriodCharts.totalcount) {
          draft.countByPeriodCharts.totalcount = unMapChartData(state.countByPeriodCharts.totalcount);
        }

        break;
      case RECIEVE_COUNTBYPERIOD:
        draft.hasLoadedData = true;
        if (action.apipath === BAGSCOUNTBYPERIOD) {
          draft.countByPeriodCharts.added = mapChartData(state.countByPeriodCharts.added || countbyperiods, action.data);
          break;
        }

        if (action.apipath === TOTALBAGSCOUNTBYPERIOD) {
          draft.countByPeriodCharts.totalcount = mapChartData(state.countByPeriodCharts.totalcount || totalCountbyperiods, action.data);
        }

        break;
      case CHANGE_CHARTTYPE:
        if (state.countByRefValueCharts[action.chartId] === undefined) { // https://github.com/Microsoft/TypeScript/issues/14951
          break;
        }

        draft.countByRefValueCharts[action.chartId]!.chartType = action.charttype;
        break;
      case CHANGE_CHARTPERIOD:
        if (state.countByPeriodCharts[action.chartId] === undefined) {
          break;
        }

        draft.countByPeriodCharts[action.chartId]!.startDate = action.startPeriod;
        break;
      case RECIEVE_COUNTBYREFVALUE:
        draft.hasLoadedData = true;
        if (action.apipath === BAGSCOUNTBYBRAND) {
          draft.countByRefValueCharts.brands = mapChartData(state.countByRefValueCharts.brands || countbybrands, action.data);
          break;
        }

        if (action.apipath === BAGSCOUNTBYBAGTYPES) {
          draft.countByRefValueCharts.bagtypes = mapChartData(state.countByRefValueCharts.bagtypes || countbybagtypes, action.data);
        }

        break;
      case REQUEST_COUNTBYREFVALUE:
        if (action.apipath === BAGSCOUNTBYBRAND && state.countByRefValueCharts.brands) {
          draft.countByRefValueCharts.brands = unMapChartData(state.countByRefValueCharts.brands);
          break;
        }

        if (action.apipath === BAGSCOUNTBYBAGTYPES && state.countByRefValueCharts.bagtypes) {
          draft.countByRefValueCharts.bagtypes = unMapChartData(state.countByRefValueCharts.bagtypes);
        }
        break;
      default:
        const exhaustiveCheck: never = action;
    }
  });

function mapChartData<T>(previousState: CountByChart<T>, newData: ICountBy<T>[]):  CountByChart<T> {
  return {...previousState, ...{data: newData}, ...{isLoading: false}};
}

function unMapChartData<T>(previousState: CountByChart<T>): CountByChart<T> {
  return {...previousState, ...{data: []}, ...{isLoading: true}};
}
