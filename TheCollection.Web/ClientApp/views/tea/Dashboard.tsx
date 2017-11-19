import * as React from 'react';
import * as moment from 'moment';
import * as c3 from 'c3';
import { Link } from 'react-router';
import { connect } from 'react-redux';
import { IApplicationState }  from '../../store';
import * as DashboardReducer from '../../reducers/tea/dashboard';
import { BAGSCOUNTBYPERIOD } from '../../constants/tea/dashboard';
import { getPeriodsFromNowTill } from '../../util/PeriodUtil';
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
  }

  componentDidMount() {
    this.props.requestBagTypeCount();
    this.props.requestBrandCount();
    this.props.requestCountByPeriod(BAGSCOUNTBYPERIOD);
  }

  onChartChanged(toChartType: ChartType, chart: string) {
    this.props.changeChartType(toChartType, chart);
  }

  translate(data: ICountBy<IRefValue>[]): c3.PrimitiveArray[] {
    return data.map(btc => {
      if (btc.value === undefined || btc.value === null) {
        return ['None', btc.count] as c3.PrimitiveArray;
      }

      return [btc.value.name, btc.count] as c3.PrimitiveArray;
    });
  }

  renderBagTypeChart() {
    if (this.props.bagtypecount === undefined) {
      return undefined;
    }

    let data = this.translate(this.props.bagtypecount);
    if (this.props.chartType['Bag Types'] === 'pie') {
      return  <PieChartBlock description='Bag Types' validTransformations={['bar']} data={data} onChartTypeChanged={this.onChartChanged}/>;
    }

    return <BarChartBlock description='Bag Types' validTransformations={['pie']} data={data} categories={['bag types']} onChartTypeChanged={this.onChartChanged} />;
  }

  renderBrandChart() {
    if (this.props.brandcount === undefined) {
      return undefined;
    }

    let data = this.translate(this.props.brandcount);
    if (this.props.chartType['Brands'] === 'pie') {
      return  <PieChartBlock description='Brands' validTransformations={['bar']} data={data} onChartTypeChanged={this.onChartChanged}/>;
    }

    return <BarChartBlock description='Brands' validTransformations={['pie']} data={data} categories={['brands']} onChartTypeChanged={this.onChartChanged} />;
  }

  renderPeriodChart() {
    if (this.props.bagCountByPeriod === undefined) {
      return undefined;
    }

    let renderPeriods = getPeriodsFromNowTill(moment().add(-1, 'y'));
    return  <div className='col-xs-12 col-sm-12 col-md-6 col-lg-6 blockcol'>
              <div className='block'>
                <div className='header' style={{display: 'flex', justifyContent: 'space-between', alignItems: 'center'}}>
                  <span>Added</span>
                  <span>{`${renderPeriods[0].year}/${renderPeriods[0].month} - ${renderPeriods[renderPeriods.length - 1].year}/${renderPeriods[renderPeriods.length - 1].month}`}</span>
                </div>
                <PeriodChart x={renderPeriods} data={{'bag count': this.props.bagCountByPeriod}}/>
              </div>
            </div>;
  }

  render() {
    return  <div style={{display: 'flex', flexWrap: 'wrap'}} className='dashboard'>
              {this.renderBrandChart()}
              {this.renderBagTypeChart()}
              {this.renderPeriodChart()}
            </div>;
  }
}

export default connect(
    (state: IApplicationState) => state.teadashboard,
    DashboardReducer.actionCreators,
)(Dashboard);
