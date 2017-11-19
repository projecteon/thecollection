import { Chart } from './Chart';
import * as React from 'react';
import { ICountBy } from '../../interfaces/ICountBy';
import { IRefValue } from '../../interfaces/IRefValue';

type PieChartProps = {
  data: c3.PrimitiveArray[];
};

// tslint:disable-next-line:variable-name
export const PieChart: React.StatelessComponent<PieChartProps> = props => {
  return  <Chart columns={props.data} chartType='pie' unloadBeforeLoad={false} />;
};
