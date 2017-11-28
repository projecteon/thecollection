import * as React from 'react';
import * as moment from 'moment';
import * as c3 from 'c3';
import { Link } from 'react-router';
import { connect } from 'react-redux';
import { IApplicationState }  from '../../store';
import * as DashboardReducer from '../../reducers/tea/dashboard';
import { BAGSCOUNTBYPERIOD, TOTALBAGSCOUNTBYPERIOD } from '../../constants/tea/dashboard';
import { getMonthlyPeriodsFromNowTill, getMonthlyPeriodsFromTill, getMonthlyPeriodsYearsBackFrom } from '../../util/PeriodUtil';
import { ICountBy } from '../../interfaces/ICountBy';
import { IRefValue } from '../../interfaces/IRefValue';
import { DashboardBlockHOC } from '../../components/DashboardBlockHOC';
import { DashboardPeriodBlockHOC } from '../../components/DashboardPeriodBlockHOC';
import { PeriodChart } from '../../components/charts/PeriodChart';
import { BarChart } from '../../components/charts/BarChart';
import { PieChart } from '../../components/charts/PieChart';
import Loader from '../../components/Loader';
import { ChartType } from '../../types/Chart';

import './Dashboard.scss';

// tslint:disable-next-line:variable-name
const PieChartBlock = DashboardBlockHOC(PieChart);
// tslint:disable-next-line:variable-name
const BarChartBlock = DashboardBlockHOC(BarChart);
// tslint:disable-next-line:variable-name
const PeriodChartBlock = DashboardPeriodBlockHOC(PeriodChart);

type DashboardProps =
  DashboardReducer.IDashboardState            // ... state we've requested from the Redux store
  & typeof DashboardReducer.actionCreators; // ... plus action creators we've requested

class Dashboard extends React.Component<DashboardProps, {}> {
  constructor(props: DashboardProps) {
    super();

    this.onChartChanged = this.onChartChanged.bind(this);
    this.onNextPeriod = this.onNextPeriod.bind(this);
    this.onPerviousPeriod = this.onPerviousPeriod.bind(this);
    this.onExpandBrands = this.onExpandBrands.bind(this);
    this.onRefreshCountByPeriod = this.onRefreshCountByPeriod.bind(this);
  }

  componentDidMount() {
    if (this.props.hasLoadedData === true) {
      return;
    }

    this.props.requestBagTypeCount();
    this.onRefreshCountByPeriod('added');
    this.onRefreshCountByPeriod('totalcount');
    this.props.requestBrandCount();
  }

  onChartChanged(toChartType: ChartType, chart: string) {
    this.props.changeChartType(toChartType, chart);
  }

  onPerviousPeriod(chartId: string) {
    this.props.changeChartPeriod(this.props.countByPeriodCharts[chartId].startDate.clone().add(-1, 'y'), chartId);
  }
  onNextPeriod(chartId: string) {
    this.props.changeChartPeriod(this.props.countByPeriodCharts[chartId].startDate.clone().add(1, 'y'), chartId);
  }

  onExpandBrands() {
    this.props.requestBrandCount(this.props.countByRefValueCharts.brands.data.length + 10);
  }

  onRefreshCountByPeriod(chartId: string) {
    if (chartId === 'added') {
      this.props.requestCountByPeriod(BAGSCOUNTBYPERIOD);
    } else if (chartId === 'totalcount') {
      this.props.requestCountByPeriod(TOTALBAGSCOUNTBYPERIOD);
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
    let blockEvents = undefined;
    if (id === 'brands') {
      blockEvents =  {onExpand: this.onExpandBrands, onRefresh: this.props.requestBrandCount};
    } else if (id === 'bagtypes') {
      blockEvents =  {onRefresh: this.props.requestBagTypeCount};
    }

    let data = this.translate(countData.data);
    if (countData.chartType === 'pie') {
      return  <PieChartBlock key={id} chartId={id} isLoading={countData.isLoading} description={countData.description} validTransformations={['bar']} data={data} onChartTypeChanged={this.onChartChanged} hideLegends={data.length > 10} {...blockEvents}/>;
    }

    return <BarChartBlock key={id} chartId={id} isLoading={countData.isLoading} description={countData.description} validTransformations={['pie']} data={data} categories={[countData.description.toLowerCase()]} onChartTypeChanged={this.onChartChanged} hideLegends={data.length > 10} {...blockEvents}/>;
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

      data.push(this.renderCountByRefValueBlock(blockData as DashboardReducer.CountByRefValueTypes, this.props.countByRefValueCharts[blockData]));
    }

    return data;
  }

  renderPeriodChart() {
    let data = this.props.countByPeriodCharts.added;
    if (data === undefined) {
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
                              onRefresh={this.onRefreshCountByPeriod} />;
  }


  renderTotalPeriodChart() {
    let data = this.props.countByPeriodCharts.totalcount;
    if (data === undefined) {
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
                              onRefresh={this.onRefreshCountByPeriod} />;
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
    (state: IApplicationState) => state.teadashboard,
    DashboardReducer.actionCreators,
)(Dashboard);
