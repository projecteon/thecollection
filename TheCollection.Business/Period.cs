namespace TheCollection.Business {
    using System;
    using Newtonsoft.Json;
    using TheCollection.Lib.Extensions;

    public class Period {
        private static DateTime DefaultDate = DateTime.MinValue;

        [JsonConstructor]
        public Period(int year, int month) {
            Year = year;
            Month = month;
        }

        public Period(string datestring) {
            var date = DateTime.MinValue;
            if (datestring.IsNotNullOrWhiteSpace()) {
                date = DateTime.Parse(datestring);
            }

            Year = date.Year;
            Month = date.Month;
        }

        public int Year { get; }
        public int Month { get; }

        public int CompareTo(Period that) {
            if (Year > that.Year) return -1;
            if (Year == that.Year) return 0;
            if (Month > that.Month) return -1;
            if (Month == that.Month) return 0;
            return 1;
        }
    }
}
