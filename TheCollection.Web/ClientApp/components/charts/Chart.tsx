import * as React from 'react';
import { findDOMNode } from 'react-dom';
import * as c3 from 'c3';
import { ChartType } from '../../types/Chart';

type ChartProps = {
  columns:  c3.PrimitiveArray[];
  chartType: ChartType;
  unloadBeforeLoad: boolean;
  axis?: c3.Axis,
  x?: string,
};

// tslint:disable-next-line:variable-name
export class Chart extends React.Component<ChartProps, {}> {
  private chart: c3.ChartAPI;

  componentDidMount() {
    this.updateChart(this.props);
  }

  componentWillReceiveProps(newProps: ChartProps) {
    this.updateChart(newProps);
  }

  componentWillUnmount() {
    this.destroyChart();
  }

  generateChart(mountNode, config) {
    const newConfig = Object.assign({ bindto: mountNode }, config);
    return c3.generate(newConfig);
  }

  loadNewData(data) {
    this.chart.load(data);
  }

  unloadData() {
    this.chart.unload();
  }

  destroyChart() {
    try {
      this.chart.destroy();
      this.chart = undefined;
    } catch (err) {
      throw new Error('Internal C3 error');
    }
  }

  updateChart(props: ChartProps) {
    let config = { data: {
      columns: this.props.columns as c3.PrimitiveArray[],
      type: this.props.chartType,
    }};

    if (this.props.axis !== undefined) {
      config = Object.assign(config, {axis: props.axis});
    }

    if (this.props.x !== undefined) {
      config.data = Object.assign(config.data, {x: props.x});
    }

    if (!this.chart) {
      this.chart = this.generateChart(findDOMNode(this), config);
    }

    // if (props.unloadBeforeLoad) {
    //     this.unloadData();
    // }

    this.loadNewData(config);
  }

  render() {
    return <div id='chart'></div>;
  }
}
