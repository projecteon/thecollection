import * as React from 'react';
import {Component, ComponentClass, StatelessComponent} from 'react';
import { ChartType } from '../types/Chart';
import Loader from './Loader';

type DashboardBlockHOCProps = {
  chartId: string;
  description: string;
  isLoading: boolean;
  dataCount: number;
  validTransformations: ChartType[];
  onChartTypeChanged(toChartType: ChartType, chart: string): void;
  onExpand?(chartId: string): void;
  onCompress?(chartId: string): void;
  onRefresh?(chartId: string): void;
};

// tslint:disable-next-line:variable-name
export function DashboardBlockHOC<T>(BlockComponent: ComponentClass<T> | StatelessComponent<T>): ComponentClass<T & DashboardBlockHOCProps> {
  return class extends Component<T & DashboardBlockHOCProps, {}> {
    constructor(props: T & DashboardBlockHOCProps) {
      super(props);

      this.onCompress = this.onCompress.bind(this);
      this.onExpand = this.onExpand.bind(this);
      this.onRefresh = this.onRefresh.bind(this);
    }

    onRefresh() {
      this.props.onRefresh(this.props.chartId);
    }

    onExpand() {
      this.props.onExpand(this.props.chartId);
    }

    onCompress() {
      this.props.onCompress(this.props.chartId);
    }

    renderConversions() {
      if (this.props.isLoading === true) {
        return undefined;
      }

      return this.props.validTransformations.map(transformation => {
        return <IconButton key={transformation} charttype={transformation} chart={this.props.chartId} onClick={this.props.onChartTypeChanged} />;
      });
    }

    renderExpandButton() {
      if (this.props.onExpand === undefined || this.props.isLoading === true || this.props.dataCount >= 30 || ((this.props.dataCount as number) % 10) !== 0) {
        return undefined;
      }

      return <i className='fa fa-expand' onClick={this.onExpand} />;
    }

    renderCompressButton() {
      if (this.props.onCompress === undefined || this.props.isLoading === true || this.props.dataCount <= 10) {
        return undefined;
      }

      return <i className='fa fa-compress' onClick={this.onCompress} />;
    }

    renderRefreshButton() {
      if (this.props.onRefresh === undefined || this.props.isLoading === true) {
        return undefined;
      }

      return <i className='fa fa-refresh' onClick={this.onRefresh} />;
    }

    render() {
      const {description, ...blockComponentProps} = this.props as any;
      return  <div className='col-xs-12 col-sm-6 col-md-4 col-lg-3 blockcol'>
                <div className='block'>
                  <div className='header' style={{display: 'flex', justifyContent: 'space-between', alignItems: 'center'}}>
                    <span>{description}</span>
                    <div>
                      {this.renderConversions()}
                      {this.renderExpandButton()}
                      {this.renderCompressButton()}
                      {this.renderRefreshButton()}
                    </div>
                  </div>
                  {this.props.isLoading === true ? <Loader isInternalLoader={true} /> : <BlockComponent {...blockComponentProps} />}
                </div>
              </div>;
    }
  };
}

// tslint:disable-next-line:variable-name
const IconButton: React.StatelessComponent<{charttype: ChartType, chart: string, onClick(charttype: ChartType, chart: string): void}> = props => {
  const onClick = () => {
    props.onClick(props.charttype, props.chart);
  };

  const iconChartType = () => {
    switch (props.charttype) {
      case 'bar':
        return 'fa-bar-chart';
      case 'pie':
        return 'fa-pie-chart';
      default:
        return '';
    }
  };

  return <i className={`fa ${iconChartType()}`} onClick={onClick} />;
};
