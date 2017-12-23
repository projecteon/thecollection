import * as moment from 'moment';

export function getMonthlyPeriodsYearsBackFrom(endDate: moment.Moment, years: number) {
  /* the first of current month */
  let startDate = endDate.clone().startOf('month').add(-1 * years, 'y');
  return getMonthlyPeriodsFromTill(startDate, endDate);
}

export function getMonthlyPeriodsFromNowTill(startDate: moment.Moment) {
  /* the first of current month */
  let endDateNormalized = moment().startOf('month').add(1, 'M');
  return getMonthlyPeriodsFromTill(startDate, endDateNormalized);
}

export function getMonthlyPeriodsFromTill(startDate: moment.Moment, endDate: moment.Moment) {
  /* the first of current month */
  let startDateNormalized = startDate.clone().startOf('month').add(1, 'M');
  let months: moment.Moment[] = [];

  /* .isBefore() as it was asked for the months in between startDate and now */
  while (startDateNormalized.isBefore(endDate)) {
      startDateNormalized.add(1, 'M');
      months.push(startDateNormalized.clone());
  }

  return months;
}
