import * as moment from 'moment';
import { IPeriod } from '../interfaces/IPeriod';

export function getMonthlyPeriodsOneYearBackFrom(endDate: moment.Moment) {
  /* the first of current month */
  let startDate = endDate.clone().startOf('month').add(-1, 'y');
  return getMonthlyPeriodsFromTill(startDate, endDate);
}

export function getMonthlyPeriodsFromNowTill(startDate: moment.Moment) {
  /* the first of current month */
  let endDateNormalized = moment().startOf('month').add(1, 'M');
  return getMonthlyPeriodsFromTill(startDate, endDateNormalized);
}

export function getMonthlyPeriodsFromTill(startDate: moment.Moment, endDate: moment.Moment) {
  console.log(startDate, endDate);
  /* the first of current month */
  let startDateNormalized = startDate.clone().startOf('month').add(1, 'M');
  let months: IPeriod[] = [];

  /* .isBefore() as it was asked for the months in between startDate and now */
  while (startDateNormalized.isBefore(endDate)) {
      months.push({year: startDateNormalized.year(), month: startDateNormalized.month() + 1});
      startDateNormalized.add(1, 'M');
  }

  return months;
}
