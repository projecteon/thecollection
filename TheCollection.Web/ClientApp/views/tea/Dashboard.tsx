import * as React from 'react';
import * as moment from 'moment';
import * as c3 from 'c3';
import { Link } from 'react-router';
import { connect } from 'react-redux';
import { IApplicationState }  from '../../store';
import * as DashboardReducer from '../../reducers/tea/dashboard';
import { BAGSCOUNTBYPERIOD } from '../../constants/tea/dashboard';
import { getPeriodsFromNowTill } from '../../util/PeriodUtil';
import { PeriodChart } from '../../components/charts/PeriodChart';
import { BarChart } from '../../components/charts/BarChart';
import { PieChart } from '../../components/charts/PieChart';

import './Dashboard.scss';

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

    return  <div className='col-xs-12 col-sm-6 col-md-4 col-lg-3 blockcol'>
              <div className='block'>
                <div className='header' style={{display: 'flex', justifyContent: 'space-between', alignItems: 'center'}}>
                  <span>Bag Types</span>
                  <i className='fa fa-bar-chart'/>
                </div>
                <PieChart data={data} />
              </div>
            </div>;
  }

  renderBrandChart() {
    if (this.props.brandcount === undefined) {
      return undefined;
    }

    let data = this.props.brandcount.map(btc => {
      return [btc.value.name, btc.count] as c3.PrimitiveArray;
    });

    return  <div className='col-xs-12 col-sm-6 col-md-4 col-lg-3 blockcol'>
              <div className='block'>
                <div className='header' style={{display: 'flex', justifyContent: 'space-between', alignItems: 'center'}}>
                  <span>Brands</span>
                  <div>
                    <i className='fa fa-pie-chart'/>
                    <i className='fa fa-expand'/>
                  </div>
                </div>
                <BarChart data={data} categories={['brands']} />
              </div>
            </div>;
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
