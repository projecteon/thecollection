import * as React from 'react';
import * as moment from 'moment';
import * as c3 from 'c3';
import { Link } from 'react-router';
import { connect } from 'react-redux';
import { IApplicationState }  from '../../store';
import * as DashboardReducer from '../../reducers/tea/dashboard';
import { getPeriodsFromNowTill } from '../../util/PeriodUtil';
import { Chart } from '../../components/charts/Chart';
import { BAGSCOUNTBYPERIOD } from '../../constants/tea/dashboard';

type DashboardProps =
  DashboardReducer.IDashboardState            // ... state we've requested from the Redux store
  & typeof DashboardReducer.actionCreators; // ... plus action creators we've requested

class Dashboard extends React.Component<DashboardProps, {}> {
  constructor(props: DashboardProps) {
    super();
  }

  componentDidMount() {
    this.props.requestBagTypeCount();
    this.props.requestBrandCount();
    this.props.requestCountByPeriod(BAGSCOUNTBYPERIOD);
  }

  renderBagTypeChart() {
    if (this.props.bagtypecount === undefined) {
      return undefined;
    }

    let data = this.props.bagtypecount.map(btc => {
      if (btc.value === undefined || btc.value === null) {
        return ['None', btc.count] as c3.PrimitiveArray;
      }

      return [btc.value.name, btc.count] as c3.PrimitiveArray;
    });

    return <Chart columns={data} chartType='pie' unloadBeforeLoad={false} />;
  }

  renderBrandChart() {
    if (this.props.brandcount === undefined) {
      return undefined;
    }

    let data = this.props.brandcount.map(btc => {
      return [btc.value.name, btc.count] as c3.PrimitiveArray;
    });

    let axis = { x: { type: 'category', categories: ['brands'] } };
    return <Chart columns={data} chartType='bar' unloadBeforeLoad={false} axis={axis} />;
  }

  renderPeriodChart() {
    if (this.props.bagCountByPeriod === undefined) {
      return undefined;
    }

    let renderPeriods = getPeriodsFromNowTill(moment().add(-1, 'y'));
    let chartPerios = renderPeriods.map(renderPeriod => {
      let bagcount2 = this.props.bagCountByPeriod.find(bagcount => {
        return bagcount.value.year === renderPeriod.year && bagcount.value.month === renderPeriod.month;
      });

      return bagcount2 === undefined ? {count: 0, value: renderPeriod} : bagcount2;
    });

    let x: c3.PrimitiveArray = ['x'];
    let data: c3.PrimitiveArray = ['bag count'];
    chartPerios.forEach(chartPeriod => {
      x.push(`${chartPeriod.value.year}-${chartPeriod.value.month}-01`);
      data.push(chartPeriod.count);
    });

    let axis = { x: { type: 'timeseries', tick: { format: '%Y-%m' } } };
    return <Chart columns={[x, data]} chartType='line' x='x' unloadBeforeLoad={false} axis={axis} />;
  }

  render() {
    return  <div className='row'>
              <div className='col-xs-12 col-sm-6 col-md-4 col-lg-3'>{this.renderBrandChart()}</div>
              <div className='col-xs-12 col-sm-6 col-md-4 col-lg-3'>{this.renderBagTypeChart()}</div>
              <div className='col-xs-12 col-sm-12 col-md-6 col-lg-6'>{this.renderPeriodChart()}</div>
            </div>;
  }
}

export default connect(
    (state: IApplicationState) => state.teadashboard,
    DashboardReducer.actionCreators,
)(Dashboard);
