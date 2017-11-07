import { Chart } from '../../components/charts/Chart';
import * as React from 'react';
import * as c3 from 'c3';
import { Link } from 'react-router';
import { connect } from 'react-redux';
import { IApplicationState }  from '../../store';
import * as DashboardReducer from '../../reducers/tea/dashboard';

type DashboardProps =
  DashboardReducer.IDashboardState            // ... state we've requested from the Redux store
  & typeof DashboardReducer.actionCreators; // ... plus action creators we've requested

class Dashboard extends React.Component<DashboardProps, {}> {
  constructor(props: DashboardProps) {
    super();
  }

  componentDidMount() {
    this.props.requestBagTypeCount();
  }

  renderBagTypeChart() {
    if (this.props.bagtypecount === undefined) {
      return undefined;
    }

    let data = this.props.bagtypecount.map(btc => {
      return [btc.value.name, btc.count] as string | number[];
    });

    return <Chart columns={data} chartType='pie' />;
  }

  render() {
    return <div>{this.renderBagTypeChart()}</div>;
  }
}

export default connect(
    (state: IApplicationState) => state.teadashboard,
    DashboardReducer.actionCreators,
)(Dashboard);
