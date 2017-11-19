import Loader from '../../components/Loader';
import * as React from 'react';
import * as moment from 'moment';
import * as c3 from 'c3';
import { Link } from 'react-router';
import { connect } from 'react-redux';
import { IApplicationState }  from '../../store';
import * as DashboardReducer from '../../reducers/tea/dashboard';
import { BAGSCOUNTBYPERIOD } from '../../constants/tea/dashboard';
import { getMonthlyPeriodsFromNowTill, getMonthlyPeriodsFromTill, getMonthlyPeriodsOneYearBackFrom } from '../../util/PeriodUtil';
import { ICountBy } from '../../interfaces/ICountBy';
import { IRefValue } from 'ClientApp/interfaces/IRefValue';
import { DashboardBlockHOC } from '../../components/DashboardBlockHOC';
import { PeriodChart } from '../../components/charts/PeriodChart';
import { BarChart } from '../../components/charts/BarChart';
import { PieChart } from '../../components/charts/PieChart';
import { ChartType } from '../../types/Chart';

import './Dashboard.scss';

// tslint:disable-next-line:variable-name
const PieChartBlock = DashboardBlockHOC(PieChart);
// tslint:disable-next-line:variable-name
const BarChartBlock = DashboardBlockHOC(BarChart);

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

  }

  componentDidMount() {
    this.props.requestBagTypeCount();
    this.props.requestBrandCount();
    this.props.requestCountByPeriod(BAGSCOUNTBYPERIOD);
  }

  onChartChanged(toChartType: ChartType, chart: string) {
    this.props.changeChartType(toChartType, chart);
  }

  onPerviousPeriod() {
    this.props.changeChartPeriod(this.props.countByPeriodCharts.added.startDate.clone().add(-1, 'y'), 'added');
  }
  onNextPeriod() {
    this.props.changeChartPeriod(this.props.countByPeriodCharts.added.startDate.clone().add(1, 'y'), 'added');
  }

  onExpandBrands() {
    this.props.requestBrandCount(this.props.countByRefValueCharts.brands.data.length + 10);
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

  renderCountByRefValueBlock(id: string, countData: DashboardReducer.CountByChart<IRefValue>) {
    let onExpand = undefined;
    if (id === 'brands') {
      onExpand =  {onExpand: this.onExpandBrands};
    }

    let data = this.translate(countData.data);
    if (countData.chartType === 'pie') {
      return  <PieChartBlock key={id} chartId={id} isLoading={countData.isLoading} description={countData.description} validTransformations={['bar']} data={data} onChartTypeChanged={this.onChartChanged} hideLegends={data.length > 10} {...onExpand}/>;
    }

    return <BarChartBlock key={id} chartId={id} isLoading={countData.isLoading} description={countData.description} validTransformations={['pie']} data={data} categories={['bag types']} onChartTypeChanged={this.onChartChanged} hideLegends={data.length > 10} {...onExpand}/>;
  }

  renderCountByRefValueBlocks() {
    let data = [];
    for (let blockData in this.props.countByRefValueCharts) {
      if (!this.props.countByRefValueCharts.hasOwnProperty(blockData)) {
        continue;
      }

      data.push(this.renderCountByRefValueBlock(blockData, this.props.countByRefValueCharts[blockData]));
    }

    return data;
  }

  renderPeriodChart() {
    let data = this.props.countByPeriodCharts.added;
    if (data === undefined) {
      return undefined;
    }

    let renderPeriods = data.startDate === undefined ? getMonthlyPeriodsFromNowTill(moment().add(-1, 'y')) : getMonthlyPeriodsOneYearBackFrom(data.startDate);
    return  <div className='col-xs-12 col-sm-12 col-md-6 col-lg-6 blockcol'>
              <div className='block'>
                <div className='header' style={{display: 'flex', justifyContent: 'space-between', alignItems: 'center'}}>
                  <span>{data.description}</span>
                  <div>
                    {data.isLoading === true ? undefined : <i className='fa fa-chevron-left' onClick={this.onPerviousPeriod} /> }
                    <span style={{marginLeft: 7}}>{`${renderPeriods[0].year}/${renderPeriods[0].month} - ${renderPeriods[renderPeriods.length - 1].year}/${renderPeriods[renderPeriods.length - 1].month}`}</span>
                    {data.isLoading === true ? undefined : <i className='fa fa-chevron-right' onClick={this.onNextPeriod} /> }
                  </div>
                </div>

                {data.isLoading === true ? <Loader isInternalLoader={true} /> : <PeriodChart x={renderPeriods} data={{'bag count': data.data}}/>}
                </div>
            </div>;
  }

  render() {
    return  <div style={{display: 'flex', flexWrap: 'wrap'}} className='dashboard'>
              {this.renderCountByRefValueBlocks()}
              {this.renderPeriodChart()}
            </div>;
  }
}

export default connect(
    (state: IApplicationState) => state.teadashboard,
    DashboardReducer.actionCreators,
)(Dashboard);
