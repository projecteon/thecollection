import * as React from 'react';
import * as moment from 'moment';
import {Component, ComponentClass, StatelessComponent} from 'react';
import Loader from '../Loader';

type DashboardPeriodBlockHOCProps = {
  chartId: string;
  description: string;
  x: moment.Moment[];
  isLoading: boolean;
  onRefresh?(chartId: string): void;
  onPerviousPeriod(chartId: string): void;
  onNextPeriod(chartId: string): void;
};

// tslint:disable-next-line:variable-name
export function DashboardPeriodBlockHOC<T>(BlockComponent: ComponentClass<T> | StatelessComponent<T>): ComponentClass<T & DashboardPeriodBlockHOCProps> {
  return class extends Component<T & DashboardPeriodBlockHOCProps, {}> {
    constructor(props: any) {
      super(props);

      this.onNextPeriod = this.onNextPeriod.bind(this);
      this.onPerviousPeriod = this.onPerviousPeriod.bind(this);
      this.onRefresh = this.onRefresh.bind(this);
    }

    onRefresh() {
      if (this.props.onRefresh) {
        this.props.onRefresh(this.props.chartId);
      }
    }

    onPerviousPeriod() {
      this.props.onPerviousPeriod(this.props.chartId);
    }

    onNextPeriod() {
      this.props.onNextPeriod(this.props.chartId);
    }

    render() {
      const startDate = this.props.x[0];
      const endDate = this.props.x[this.props.x.length - 1];
      const {description, ...blockComponentProps} = this.props as any;
      return  <div className='col-xs-12 col-sm-12 col-md-6 col-lg-6 blockcol'>
              <div className='block'>
                <div className='header' style={{display: 'flex', justifyContent: 'space-between', alignItems: 'center'}}>
                  <span>{description}</span>
                  <div>
                    {this.props.isLoading === true ? undefined : <i className='fa fa-chevron-left' onClick={this.onPerviousPeriod} /> }
                    <span style={{marginLeft: 7}}>{`${startDate.year()}/${startDate.month()} - ${endDate.year()}/${endDate.month()}`}</span>
                    {this.props.isLoading === true ? undefined : <i className='fa fa-chevron-right' onClick={this.onNextPeriod} /> }
                    {this.props.isLoading === true ? undefined : <i className='fa fa-refresh' onClick={this.onRefresh} /> }
                  </div>
                </div>
                {this.props.isLoading === true ? <Loader isInternalLoader={true} /> : <BlockComponent {...blockComponentProps} />}
                </div>
            </div>;
    }
  };
}
