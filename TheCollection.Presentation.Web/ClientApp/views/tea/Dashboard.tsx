import * as React from 'react';
import * as c3 from 'c3';
import { connect, Dispatch } from 'react-redux';
import { IApplicationState }  from '../../store';
import * as DashboardReducer from '../../reducers/tea/dashboard';
import { BAGSCOUNTBYPERIOD, TOTALBAGSCOUNTBYPERIOD, BAGSCOUNTBYBRAND, BAGSCOUNTBYBAGTYPES } from '../../constants/tea/dashboard';
import { getMonthlyPeriodsYearsBackFrom } from '../../util/PeriodUtil';
import { ICountBy } from '../../interfaces/ICountBy';
import { IRefValue } from '../../interfaces/IRefValue';
import { DashboardBlockHOC } from '../../components/dashboard/DashboardBlockHOC';
import { DashboardPeriodBlockHOC } from '../../components/dashboard/DashboardPeriodBlockHOC';
import { PeriodChart } from '../../components/charts/PeriodChart';
import { BarChart } from '../../components/charts/BarChart';
import { PieChart } from '../../components/charts/PieChart';
import { ChartType } from '../../types/Chart';
import { Moment } from 'moment';
import { CountByPeriodTypes, CountByRefValueTypes } from '../../reducers/tea/dashboard';
import { requestCountByRefValueAction, requestCountByPeriodAction, changeChartPeriodAction, changeChartTypeAction } from '../../actions/dashboard/chart';

import './Dashboard.scss';

// tslint:disable-next-line:variable-name
const PieChartBlock = DashboardBlockHOC(PieChart);
// tslint:disable-next-line:variable-name
const BarChartBlock = DashboardBlockHOC(BarChart);
// tslint:disable-next-line:variable-name
const PeriodChartBlock = DashboardPeriodBlockHOC(PeriodChart);


function mapDispatchToProps(dispatch: Dispatch) {
  return {
    onRequestCountByValueRef: (apipath: string, top?: number) => dispatch(requestCountByRefValueAction(apipath, top)),
    onRequestCountByPeriod: (apipath: string, top?: number) => dispatch(requestCountByPeriodAction(apipath, top)),
    onChangeChartPeriod: (startPeriod: Moment, chartId: CountByPeriodTypes) => dispatch(changeChartPeriodAction(startPeriod, chartId)),
    onChangeChartType: (charttype: ChartType, chartId: CountByRefValueTypes) => dispatch(changeChartTypeAction(charttype, chartId)),
  };
}

function mapStateToProps(state: IApplicationState) {
  return {...state.teadashboard, ...state.ui};
}

type DashboardProps = ReturnType<typeof mapStateToProps> & ReturnType<typeof mapDispatchToProps>;

class Dashboard extends React.PureComponent<DashboardProps, {}> {
  constructor(props: DashboardProps) {
    super(props);

    this.onChartChanged = this.onChartChanged.bind(this);
    this.onNextPeriod = this.onNextPeriod.bind(this);
    this.onPerviousPeriod = this.onPerviousPeriod.bind(this);
    this.onExpandChart = this.onExpandChart.bind(this);
    this.onCompressChart = this.onCompressChart.bind(this);
    this.onRefreshChart = this.onRefreshChart.bind(this);
  }

  componentDidMount() {
    if (this.props.hasLoadedData === true) {
      return;
    }

    this.props.onRequestCountByValueRef(BAGSCOUNTBYBRAND);
    this.props.onRequestCountByPeriod(BAGSCOUNTBYPERIOD);
    this.props.onRequestCountByPeriod(TOTALBAGSCOUNTBYPERIOD);
    this.props.onRequestCountByValueRef(BAGSCOUNTBYBAGTYPES);
  }

  onChartChanged(toChartType: ChartType, chartId: DashboardReducer.CountByRefValueTypes) {
    this.props.onChangeChartType(toChartType, chartId);
  }

  onPerviousPeriod(chartId: DashboardReducer.CountByPeriodTypes) {
    this.props.onChangeChartPeriod(this.props.countByPeriodCharts[chartId]!.startDate!.clone().add(-1, 'y'), chartId);
  }
  onNextPeriod(chartId: DashboardReducer.CountByPeriodTypes) {
    this.props.onChangeChartPeriod(this.props.countByPeriodCharts[chartId]!.startDate!.clone().add(1, 'y'), chartId);
  }

  onExpandChart(chartId: DashboardReducer.CountByRefValueTypes) {
    if (chartId === 'brands') {
      this.props.onRequestCountByValueRef(BAGSCOUNTBYBRAND, this.props.countByRefValueCharts.brands!.data.length + 10);
    }
  }

  onCompressChart(chartId: DashboardReducer.CountByRefValueTypes) {
    if (chartId === 'brands') {
      this.props.onRequestCountByValueRef(BAGSCOUNTBYBRAND, this.props.countByRefValueCharts.brands!.data.length - 10);
    }
  }

  onRefreshChart(chartId: DashboardReducer.CountByPeriodTypes | DashboardReducer.CountByRefValueTypes) {
    if (chartId === 'added') {
      this.props.onRequestCountByPeriod(BAGSCOUNTBYPERIOD);
    } else if (chartId === 'totalcount') {
      this.props.onRequestCountByPeriod(TOTALBAGSCOUNTBYPERIOD);
    } else if (chartId === 'brands') {
      this.props.onRequestCountByValueRef(BAGSCOUNTBYBRAND);
    } else if (chartId === 'bagtypes') {
      this.props.onRequestCountByValueRef(BAGSCOUNTBYBAGTYPES);
    }
  }

  translate(data: ICountBy<IRefValue>[]): c3.PrimitiveArray[] {
    if (data === undefined) {
      return [];
    }

    return data.map(btc => {
      if (btc.value === undefined || btc.value === null) {
        return ['None', btc.count] as c3.PrimitiveArray;
      }

      return [btc.value.name, btc.count] as c3.PrimitiveArray;
    });
  }

  renderCountByRefValueBlock(id: DashboardReducer.CountByRefValueTypes, countData: DashboardReducer.CountByChart<IRefValue>) {
    let blockEvents = {onExpand: this.onExpandChart, onCompress: this.onCompressChart, onRefresh: this.onRefreshChart};
    let data = this.translate(countData.data);
    if (countData.chartType === 'pie') {
      return  <PieChartBlock key={id} chartId={id} isLoading={countData.isLoading} dataCount={data.length} description={countData.description} validTransformations={['bar']} data={data} onChartTypeChanged={this.onChartChanged} hideLegends={data.length > 10} {...blockEvents}/>;
    }

    return <BarChartBlock key={id} chartId={id} isLoading={countData.isLoading} dataCount={data.length} description={countData.description} validTransformations={['pie']} data={data} categories={[countData.description.toLowerCase()]} onChartTypeChanged={this.onChartChanged} hideLegends={data.length > 10} {...blockEvents}/>;
  }

  renderCountByRefValueBlocks() {
    let data = [];
    // https://blog.mariusschulz.com/2017/01/06/typescript-2-1-keyof-and-lookup-typess
    // for (let [key, value] of Object.entries(this.props.countByRefValueCharts)) {
    //   data.push(this.renderCountByRefValueBlock(key, this.props.countByRefValueCharts[value]));
    // }

    for (let blockData in this.props.countByRefValueCharts) {
      if (!this.props.countByRefValueCharts.hasOwnProperty(blockData)) {
        continue;
      }

      let key = blockData as DashboardReducer.CountByRefValueTypes;
      if (this.props.countByRefValueCharts[key] === undefined) {
        continue;
      }

      data.push(this.renderCountByRefValueBlock(key, this.props.countByRefValueCharts[key]!));
    }

    return data;
  }

  renderPeriodChart() {
    let data = this.props.countByPeriodCharts.added;
    if (!data || !data.startDate) {
      return undefined;
    }

    let renderPeriods = getMonthlyPeriodsYearsBackFrom(data.startDate, 1);
    return <PeriodChartBlock  chartId={'added'}
                              isLoading={data.isLoading}
                              description={data.description}
                              x={renderPeriods}
                              data={{'bag count': data.data}}
                              onNextPeriod={this.onNextPeriod}
                              onPerviousPeriod={this.onPerviousPeriod}
                              onRefresh={this.onRefreshChart} />;
  }


  renderTotalPeriodChart() {
    let data = this.props.countByPeriodCharts.totalcount;
    if (!data || !data.startDate) {
      return undefined;
    }

    let renderPeriods = getMonthlyPeriodsYearsBackFrom(data.startDate, 2);
    return <PeriodChartBlock  chartId={'totalcount'}
                              isLoading={data.isLoading}
                              description={data.description}
                              x={renderPeriods}
                              data={{'bag count': data.data}}
                              continuePreviousPeriodCount={true}
                              onNextPeriod={this.onNextPeriod}
                              onPerviousPeriod={this.onPerviousPeriod}
                              onRefresh={this.onRefreshChart} />;
  }

  render() {
    return  <div style={{display: 'flex', flexWrap: 'wrap'}} className='dashboard'>
              {this.renderCountByRefValueBlocks()}
              {this.renderPeriodChart()}
              {this.renderTotalPeriodChart()}
            </div>;
  }
}

export default connect(
  mapStateToProps,
  mapDispatchToProps,
)(Dashboard);
