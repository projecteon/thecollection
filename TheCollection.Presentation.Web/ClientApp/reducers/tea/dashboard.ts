import produce from 'immer';
import * as moment from 'moment';
import { Reducer } from 'redux';
import { ICountBy } from '../../interfaces/ICountBy';
import { IRefValue } from '../../interfaces/IRefValue';
import { ChartType } from '../../types/Chart';
import {
  BAGSCOUNTBYBAGTYPES,
  BAGSCOUNTBYBRAND,
  BAGSCOUNTBYPERIOD,
  TOTALBAGSCOUNTBYPERIOD,
} from '../../constants/tea/dashboard';
import { ChartActionTypes, requestCountByPeriodAction, receiveCountByPeriodAction, requestCountByRefValueAction, receiveCountByRefValueAction, changeChartTypeAction, changeChartPeriodAction } from '../../actions/dashboard/chart';
import { ActionsUnion } from '../../util/Redux';

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

export const actionCreators = {requestCountByPeriodAction, receiveCountByPeriodAction, requestCountByRefValueAction, receiveCountByRefValueAction, changeChartTypeAction, changeChartPeriodAction};
export type DashboardActions = ActionsUnion<typeof actionCreators>;

export const reducer: Reducer<IDashboardState, DashboardActions> = (state = unloadedState, action) =>
  produce(state, draft => {
    switch (action.type) {
      case ChartActionTypes.RequestCountByPeriod:
        if (action.payload.apipath === BAGSCOUNTBYPERIOD && draft.countByPeriodCharts.added) {
          draft.countByPeriodCharts.added = unMapChartData(draft.countByPeriodCharts.added);
          break;
        }

        if (action.payload.apipath === TOTALBAGSCOUNTBYPERIOD && draft.countByPeriodCharts.totalcount) {
          draft.countByPeriodCharts.totalcount = unMapChartData(draft.countByPeriodCharts.totalcount);
        }

        break;
      case ChartActionTypes.RecieveCountByPeriod:
        draft.hasLoadedData = true;
        if (action.payload.apipath === BAGSCOUNTBYPERIOD) {
          draft.countByPeriodCharts.added = mapChartData(draft.countByPeriodCharts.added || countbyperiods, action.payload.data);
          break;
        }

        if (action.payload.apipath === TOTALBAGSCOUNTBYPERIOD) {
          draft.countByPeriodCharts.totalcount = mapChartData(draft.countByPeriodCharts.totalcount || totalCountbyperiods, action.payload.data);
        }

        break;
      case ChartActionTypes.ChangeChartType:
        if (draft.countByRefValueCharts[action.payload.chartId] === undefined) { // https://github.com/Microsoft/TypeScript/issues/14951
          break;
        }

        draft.countByRefValueCharts[action.payload.chartId]!.chartType = action.payload.charttype;
        break;
      case ChartActionTypes.ChangeChartPeriod:
        if (state.countByPeriodCharts[action.payload.chartId] === undefined) {
          break;
        }

        draft.countByPeriodCharts[action.payload.chartId]!.startDate = action.payload.startPeriod;
        break;
      case ChartActionTypes.RecieveCountByRefValue:
        draft.hasLoadedData = true;
        if (action.payload.apipath === BAGSCOUNTBYBRAND) {
          draft.countByRefValueCharts.brands = mapChartData(draft.countByRefValueCharts.brands || countbybrands, action.payload.data);
          break;
        }

        if (action.payload.apipath === BAGSCOUNTBYBAGTYPES) {
          draft.countByRefValueCharts.bagtypes = mapChartData(draft.countByRefValueCharts.bagtypes || countbybagtypes, action.payload.data);
        }

        break;
      case ChartActionTypes.RequestCountByRefValue:
        if (action.payload.apipath === BAGSCOUNTBYBRAND && draft.countByRefValueCharts.brands) {
          draft.countByRefValueCharts.brands = unMapChartData(draft.countByRefValueCharts.brands);
          break;
        }

        if (action.payload.apipath === BAGSCOUNTBYBAGTYPES && draft.countByRefValueCharts.bagtypes) {
          draft.countByRefValueCharts.bagtypes = unMapChartData(draft.countByRefValueCharts.bagtypes);
        }
        break;
      default:
        const exhaustiveCheck: never = action;
    }
  });

function mapChartData<T>(draftState: CountByChart<T>, newData: ICountBy<T>[]):  CountByChart<T> {
  // return {...draftState, ...{data: newData}, ...{isLoading: false}};
  draftState.data = newData;
  draftState.isLoading = false;
  return draftState;
}

function unMapChartData<T>(draftState: CountByChart<T>): CountByChart<T> {
  draftState.data = [];
  draftState.isLoading = true;
  return draftState;
}
