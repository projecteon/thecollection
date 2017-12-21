namespace TheCollection.Domain {
    using System;
    using Newtonsoft.Json;
    using NodaTime;

    public class Period : IComparable<Period> {
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
        
        public int CompareTo(Period other) {
            if (Year > other.Year) return -1;
            if (Year == other.Year) {
                if (Month > other.Month) return -1;
                if (Month == other.Month) return 0;
                return 1;
            }
            return 1;
        }
    }
}
