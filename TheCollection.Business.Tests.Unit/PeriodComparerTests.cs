namespace TheCollection.Business.Tests.Unit {
    using TheCollection.Business;
    using Xunit;

    [Trait("PeriodComparerTests", "PeriodComparer equality tests")]
    public class PeriodComparerTests {
        [Fact(DisplayName = "When periods are same object then return true")]
        public void WhenPeriodsAreSameObjectThenReturnTrue() {
            var period = new Period(2017, 1);
            var comparer = new PeriodComparer();

            Assert.True(comparer.Equals(period, period));
        }

        [Fact(DisplayName = "When periods dates are the same then return true")]
        public void WhenPeriodsDatesAreTheSameThenReturnTrue() {
            var period = new Period(2017, 1);
            var period2 = new Period(2017, 1);
            var comparer = new PeriodComparer();

            Assert.True(comparer.Equals(period, period2));
        }

        [Fact(DisplayName = "When periods month dates are not the same then return false")]
        public void WhenPeriodsMonthDatesAreNotTheSameThenReturnFalse() {
            var period = new Period(2017, 1);
            var period2 = new Period(2017, 2);
            var comparer = new PeriodComparer();

            Assert.False(comparer.Equals(period, period2));
        }

        [Fact(DisplayName = "When periods year dates are not the same then return false")]
        public void WhenPeriodsYearDatesAreNotTheSameThenReturnFalse() {
            var period = new Period(2016, 1);
            var period2 = new Period(2017, 1);
            var comparer = new PeriodComparer();

            Assert.False(comparer.Equals(period, period2));
        }

        [Fact(DisplayName = "When periods year and month dates are not the same then return false")]
        public void WhenPeriodsYearAndMonthDatesAreNotTheSameThenReturnFalse() {
            var period = new Period(2016, 1);
            var period2 = new Period(2017, 2);
            var comparer = new PeriodComparer();

            Assert.False(comparer.Equals(period, period2));
        }
    }
}
