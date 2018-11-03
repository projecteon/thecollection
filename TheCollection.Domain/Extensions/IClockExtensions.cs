namespace TheCollection.Domain.Extensions {

    using NodaTime;

    public static class IClockExtensions {

        public static LocalDateTime NowLocalDateTime(this IClock clock) {
            var now = clock.GetCurrentInstant();
            var tz = DateTimeZoneProviders.Tzdb.GetSystemDefault();
            var zdt = now.InZone(tz);
            return zdt.LocalDateTime;
        }

        public static

        Interval GetTodaysInterval(this IClock clock, DateTimeZone timeZone) {
            var today = clock.GetCurrentInstant().InZone(timeZone).Date;
            var dayStart = timeZone.AtStartOfDay(today);
            var dayEnd = timeZone.AtStartOfDay(today.PlusDays(1));
            return new Interval(dayStart.ToInstant(), dayEnd.ToInstant());
        }
    }
}
