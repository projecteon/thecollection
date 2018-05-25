import * as React from 'react';
import { Chart } from './Chart';
import { ICountBy } from '../../interfaces/ICountBy';
import { Moment } from 'moment';

type PeriodChartProps = {
  x: Moment[];
  data: { [index: string]: ICountBy<Moment>[]};
  continuePreviousPeriodCount?: boolean;
};

// tslint:disable-next-line:variable-name
export const PeriodChart: React.StatelessComponent<PeriodChartProps> = props => {
  const startCount = (data: ICountBy<Moment>[]) => {
    let previousCount = 0;
    if (props.continuePreviousPeriodCount !== true) {
      return previousCount;
    }

    for (let _data of data) {
      if (_data.value.isAfter(props.x[0])) {
        break;
      }

      if (_data.value.isSame(props.x[0])) {
        break;
      }

      previousCount = _data.count;
    }

    return previousCount;
  };

  const createData = (title: string, data: ICountBy<Moment>[]) => {
    let previousCount = startCount(data);
    let xPeriods = props.x.map(x => {
      let periodData = data.find(xdata => {
        return xdata.value.isSame(x);
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
      x.push(chartPeriod.format('YYYY-MM-DD'));
    });

    return x;
  };

  const createColumns = () => {
    let columns = [createTimeAxis()].concat(createLineData());
    console.log(props);
    return columns;
  };

  const axis = { x: { type: 'timeseries', tick: { format: '%Y-%m' } }, y: { min: 0 } };

  return <Chart columns={createColumns()} chartType='line' x='x' unloadBeforeLoad={false} axis={axis} />;
};
