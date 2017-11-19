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

export interface IDashboardState {
  bagtypecount?: ICountBy<IRefValue>[];
  brandcount?: ICountBy<IRefValue>[];
  bagCountByPeriod?: ICountBy<IPeriod>[];
  isLoading: boolean;
  chartType: {[index: string]: ChartType};
}

export const actionCreators = {...requestBagTypeCount, ...requestBrandCount, ...requestCountByPeriod, ...changeChartType};

const unloadedState: IDashboardState = { isLoading: false, chartType: {'Brands': 'bar', 'Bag Types': 'pie'} };
type DashboardActions = ReceiveBagTypeCountAction | RequestBagTypeCountAction | ReceiveBrandCountAction | RequestBrandCountAction | ReceiveCountByPeriodAction | RequesCountByPeriodAction | ChangeChartType;
export const reducer: Reducer<IDashboardState> = (state: IDashboardState, action: DashboardActions) => {
  switch (action.type) {
    case REQUEST_BAGTYPECOUNT:
      return  {...state, ...{ bagtypecount: undefined, isLoading: true }};
    case RECIEVE_BAGTYPECOUNT:
      return  {...state, ...{ bagtypecount: action.bagtypecount, isLoading: false }};
    case REQUEST_BRANDCOUNT:
      return  {...state, ...{ brandcount: undefined, isLoading: true }};
    case RECIEVE_BRANDCOUNT:
      return  {...state, ...{ brandcount: action.brandcount, isLoading: false }};
    case REQUEST_COUNTBYPERIOD:
      return  {...state, ...{ bagCountByPeriod: undefined, isLoading: true }};
    case RECIEVE_COUNTBYPERIOD:
      if (action.apipath !== BAGSCOUNTBYPERIOD) {
        return state;
      }

      return  {...state, ...{ bagCountByPeriod: action.data, isLoading: false }};
    case CHANGE_CHARTTYPE:
      let newChartType = {...state.chartType, ...changeToChartType(action.charttype, action.chart)};
      return {...state, ...{chartType: newChartType}};
    default:
      const exhaustiveCheck: never = action;
  }

  return state || unloadedState;
};

function changeToChartType(toChartType: ChartType, chart: string): {[index: string]: ChartType} {
  return {[chart]: toChartType};
}
