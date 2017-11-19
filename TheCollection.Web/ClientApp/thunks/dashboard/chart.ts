import * as moment from 'moment';
import { fetch, addTask } from 'domain-task';
import { routerActions, RouterAction } from 'react-router-redux';
import { AppThunkAction } from '../../store';
import { ChartType } from '../../types/Chart';
import { CHANGE_CHARTTYPE, CHANGE_CHARTPERIOD } from '../../constants/dashboard/chart';
import { ChangeChartType, ChangeChartPeriod } from '../../actions/dashboard/chart';

export const changeChartType = {
  changeChartType: (charttype: ChartType, chartId: string): AppThunkAction<ChangeChartType> => (dispatch, getState) => {
    dispatch({ type: CHANGE_CHARTTYPE, charttype: charttype, chartId: chartId });
  },
};

export const changeChartPeriod = {
  changeChartPeriod: (startPeriod: moment.Moment, chartId: string): AppThunkAction<ChangeChartPeriod> => (dispatch, getState) => {
    dispatch({ type: CHANGE_CHARTPERIOD, startPeriod: startPeriod, chartId: chartId });
  },
};
