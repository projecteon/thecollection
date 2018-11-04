import { Chart } from './Chart';
import * as React from 'react';
import { ICountBy } from '../../interfaces/ICountBy';
import { IRefValue } from '../../interfaces/IRefValue';

type BarChartProps = {
  data: c3.PrimitiveArray[];
  categories: string[];
  hideLegends?: boolean;
};

// tslint:disable-next-line:variable-name
export const BarChart: React.StatelessComponent<BarChartProps> = props => {
  const axis = { x: { type: 'category', categories: props.categories } };

  return  <Chart columns={props.data} chartType='bar' unloadBeforeLoad={false} axis={axis} hideLegends={props.hideLegends}/>;
};
