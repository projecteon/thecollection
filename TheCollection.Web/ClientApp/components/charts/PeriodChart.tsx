import { Chart } from './Chart';
import * as React from 'react';
import { ICountBy } from '../../interfaces/ICountBy';
import { IPeriod } from '../../interfaces/IPeriod';

type PeriodChartProps = {
  x: IPeriod[];
  data: { [index: string]: ICountBy<IPeriod>[]};
  continuePreviousPeriodCount?: boolean;
};

// tslint:disable-next-line:variable-name
export const PeriodChart: React.StatelessComponent<PeriodChartProps> = props => {
  const createData = (title: string, data: ICountBy<IPeriod>[]) => {
    let previousCount = 0;
    let xPeriods = props.x.map(x => {
      let periodData = data.find(xdata => {
        if (props.continuePreviousPeriodCount === true) {
          previousCount = xdata.count;
        }

        return xdata.value.year === x.year && xdata.value.month === x.month;
      });

      return periodData === undefined ? {count: previousCount, value: x} : periodData;
    });

    let chartData: c3.PrimitiveArray = [title];
    return  chartData.concat(xPeriods.map(chartPeriod => chartPeriod.count));
  };

  const createLineData = () => {
    let data: c3.PrimitiveArray[] = [];
    for (let dataTitle in props.data) {
      if (!props.data.hasOwnProperty(dataTitle)) {
        continue;
      }

      data.push(createData(dataTitle, props.data[dataTitle]));
    }

    return data;
  };

  const createTimeAxis = () => {
    let x: c3.PrimitiveArray = ['x'];
    props.x.forEach(chartPeriod => {
      x.push(`${chartPeriod.year}-${chartPeriod.month}-01`);
    });

    return x;
  };

  const createColumns = () => {
    let columns = [createTimeAxis()].concat(createLineData());
    return columns;
  };

  const axis = { x: { type: 'timeseries', tick: { format: '%Y-%m' } }, y: { min: 0 } };

  return <Chart columns={createColumns()} chartType='line' x='x' unloadBeforeLoad={false} axis={axis} />;
};
