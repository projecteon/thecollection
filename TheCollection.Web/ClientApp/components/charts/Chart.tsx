import * as React from 'react';

type ChartProps = {
  columns:  (string | number[])[];
  chartType: 'line' | 'spline' | 'step' | 'area' | 'area-spline' | 'area-step' | 'bar' | 'scatter' | 'pie' | 'donut' | 'gauge';
};

// tslint:disable-next-line:variable-name
export const Chart: React.StatelessComponent<ChartProps> = props => {

  function componentDidMount() {
    updateChart();
  }

  function componentDidUpdate() {
    updateChart();
  }

  function updateChart() {
    let temp = 1;
  }

  return <div id='chart'></div>;
};

