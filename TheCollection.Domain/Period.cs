namespace TheCollection.Domain {
    using System;
    using Newtonsoft.Json;
    using NodaTime;

    public class Period {
        private static DateTime DefaultDate = DateTime.MinValue;

        [JsonConstructor]
        public Period(int year, int month) {
            Year = year;
            Month = month;
        }

        public Period(LocalDate localDate) {
            Year = localDate.Year;
            Month = localDate.Month;
        }

        public int Year { get; }
        public int Month { get; }
    }
}
