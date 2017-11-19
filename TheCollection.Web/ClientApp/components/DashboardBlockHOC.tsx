import * as React from 'react';
import {Component, ComponentClass, StatelessComponent} from 'react';
import { ChartType } from '../types/Chart';

type DashboardBlockHOCProps = {
  description: string;
  validTransformations: ChartType[];
  onChartTypeChanged(toChartType: ChartType): void;
  onExpand?(): void;
};

// tslint:disable-next-line:variable-name
export function DashboardBlockHOC<T>(BlockComponent: ComponentClass<T> | StatelessComponent<T>): ComponentClass<T & DashboardBlockHOCProps> {
  return class extends Component<T & DashboardBlockHOCProps, {}> {
    renderConversions() {
      return this.props.validTransformations.map(transformation => {
        return <IconButton charttype={transformation} onClick={this.props.onChartTypeChanged} />;
      });
    }

    render() {
      const {description, ...blockComponentProps} = this.props as any;
      return  <div className='col-xs-12 col-sm-6 col-md-4 col-lg-3 blockcol'>
                <div className='block'>
                  <div className='header' style={{display: 'flex', justifyContent: 'space-between', alignItems: 'center'}}>
                    <span>{description}</span>
                    {this.renderConversions()}
                    {this.props.onExpand ? <i className='fa fa-expand' onClick={this.props.onExpand}/> : undefined}
                  </div>
                  <BlockComponent {...blockComponentProps} />
                </div>
              </div>;
    }
  };
}

// tslint:disable-next-line:variable-name
const IconButton: React.StatelessComponent<{charttype: ChartType, onClick(charttype: ChartType): void}> = props => {
  const onClick = () => {
    props.onClick(props.charttype);
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
