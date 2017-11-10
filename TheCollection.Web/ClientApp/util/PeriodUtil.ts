import * as moment from 'moment';
import { IPeriod } from '../interfaces/IPeriod';

export function getPeriodsFromNowTill(startDate: moment.Moment) {
  /* the first of current month */
  let nowNormalized = moment().startOf('month').add(1, 'M');
  let startDateNormalized = startDate.clone().startOf('month').add(1, 'M');
  let months: IPeriod[] = [];

  console.log(startDate.clone().startOf('month'), nowNormalized);

  /* .isBefore() as it was asked for the months in between startDate and now */
  while (startDateNormalized.isBefore(nowNormalized)) {
      months.push({year: startDateNormalized.year(), month: startDateNormalized.month() + 1});
      startDateNormalized.add(1, 'M');
  }

  return months;
}
