namespace TheCollection.Domain {
    using System;
    using System.Collections.Generic;

    public class PeriodComparer : IEqualityComparer<Period> {
        public bool Equals(Period periodX, Period periodY) {
            if (Object.ReferenceEquals(periodX, periodY)) return true;
            if (Object.ReferenceEquals(periodX, null) || Object.ReferenceEquals(periodY, null)) return false;
            if (periodX.Year == periodY.Year && periodX.Month == periodY.Month) return true;
            return false;
        }

        public int GetHashCode(Period period) {
            var hashYear = period.Year.GetHashCode();
            var hashMonth = period.Month.GetHashCode();
            return hashYear ^ hashMonth;
        }
    }
}
