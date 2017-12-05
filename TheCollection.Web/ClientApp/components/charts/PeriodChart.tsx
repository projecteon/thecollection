import * as React from 'react';
import { Chart } from './Chart';
import { ICountBy } from '../../interfaces/ICountBy';
import { IPeriod } from '../../interfaces/IPeriod';

type PeriodChartProps = {
  x: IPeriod[];
  data: { [index: string]: ICountBy<IPeriod>[]};
  continuePreviousPeriodCount?: boolean;
};

// tslint:disable-next-line:variable-name
export const PeriodChart: React.StatelessComponent<PeriodChartProps> = props => {
  const startCount = (data: ICountBy<IPeriod>[]) => {
    let previousCount = 0;
    if (props.continuePreviousPeriodCount !== true) {
      return previousCount;
    }

    for (let _data of data) {
      if (_data.value.year > props.x[0].year) {
        break;
      }

      if (_data.value.year === props.x[0].year && _data.value.month >= props.x[0].month) {
        break;
      }

      previousCount = _data.count;
    }

    return previousCount;
  };

  const createData = (title: string, data: ICountBy<IPeriod>[]) => {
    let previousCount = startCount(data);
    let xPeriods = props.x.map(x => {
      let periodData = data.find(xdata => {
        return xdata.value.year === x.year && xdata.value.month === x.month;
      });

      if (periodData === undefined) {
        return {count: previousCount, value: x};
      }

      if (props.continuePreviousPeriodCount === true) {
        previousCount = periodData.count;
      }

      return periodData;
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
