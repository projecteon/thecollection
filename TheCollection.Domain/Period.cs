namespace TheCollection.Domain {
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
    }
}
