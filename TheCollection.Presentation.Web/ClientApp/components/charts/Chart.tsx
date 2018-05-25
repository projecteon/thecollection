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
  regions?: c3.RegionOptions[],
  hideLegends?: boolean;
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
    return this.chart.load(data);
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
      columns: props.columns as c3.PrimitiveArray[],
      type: props.chartType,
    }};

    if (props.axis !== undefined) {
      config = Object.assign(config, {axis: props.axis});
    }

    if (props.x !== undefined) {
      config.data = Object.assign(config.data, {x: props.x});
    }

    if (props.regions !== undefined) {
      config = Object.assign(config, {regions: props.regions});
    }

    if (!this.chart) {
      this.chart = this.generateChart(findDOMNode(this), config);
    } else {
      config = Object.assign(config, {columns: config.data.columns});
      config.data = undefined;
    }

    if (props.unloadBeforeLoad) {
      // this.chart.unload({ids: this.props.columns.map(column => { return column[0].toString(); })}, () => { this.loadNewData(config); return true; });
      // config = Object.assign(config, {unload: this.props.columns.map(column => { return column[0]; })});
    }

    if (props.hideLegends === true) {
      this.chart.legend.hide();
    }

    this.loadNewData(config);
  }

  render() {
    return <div id='chart'></div>;
  }
}
