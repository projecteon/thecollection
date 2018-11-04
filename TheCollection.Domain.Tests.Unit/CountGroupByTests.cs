namespace TheCollection.Domain.Tests.Unit {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TheCollection.Domain.Tests.Unit.Helpers;
    using Xunit;

    [Trait(nameof(CountGroupBy<string, string, Helpers.StringComparer>), "CountGroupBy tests")]
    public class CountGroupByTests {
        [Fact(DisplayName = "When queryable argument is null, ArgumentNullException is thrown")]
        public void WhenQueryableArgumentIsNullExceptionIsThrown() {
            var dummyList = new List<string>();
            var countGroupBy = new CountGroupBy<string, string, Helpers.StringComparer>(dummyList.AsQueryable());

            Assert.Throws<ArgumentNullException>(() => new CountGroupBy<string, string, Helpers.StringComparer>(null));
        }

        [Fact(DisplayName = "When predicate argument is null, ArgumentNullException is thrown")]
        public void WhenExpressionArgumentIsNullExceptionIsThrown() {
            var dummyList = new List<string>();
            var countGroupBy = new CountGroupBy<string, string, Helpers.StringComparer>(dummyList.AsQueryable());

            Assert.Throws<ArgumentNullException>(() => countGroupBy.GroupAndCountBy(null));
        }

        [Fact(DisplayName = "When queryable argument is empty list, then empty CountBy list is returned")]
        public void WhenQueryableArgumentIsEmptyListThenEmptyCountyByListIsReturned() {
            var dummyList = new List<string>();
            var countGroupBy = new CountGroupBy<string, string, Helpers.StringComparer>(dummyList.AsQueryable());

            var actual = countGroupBy.GroupAndCountBy(x => x).ToList();

            Assert.Equal(new List<CountBy<string>>(), actual);
        }

        [Fact(DisplayName = "When queryable argument has one item, then list with CountBy with count of one is returned")]
        public void WhenQueryableArgumentListHasOneItemThenListWithCountByOfWithCountOfOneIsReturned() {
            var dummyList = new List<string> { "one" };
            var countGroupBy = new CountGroupBy<string, string, Helpers.StringComparer>(dummyList.AsQueryable());

            var actual = countGroupBy.GroupAndCountBy(x => x);

            Assert.Equal(new List<CountBy<string>> { new CountBy<string>("one", 1) }, actual, new CountByComparer<string>());
        }

        [Fact(DisplayName = "When queryable argument has two equal items, then list with CountBy with count of two is returned")]
        public void WhenQueryableArgumentListHasTwoEqualItemsThenListWithCountByOfWithCountOfTwoIsReturned() {
            var dummyList = new List<string> { "one", "one" };
            var countGroupBy = new CountGroupBy<string, string, Helpers.StringComparer>(dummyList.AsQueryable());

            var actual = countGroupBy.GroupAndCountBy(x => x);

            Assert.Equal(new List<CountBy<string>> { new CountBy<string>("one", 2) }, actual, new CountByComparer<string>());
        }

        [Fact(DisplayName = "When queryable argument has two different items, then list with CountBy with count of two is returned")]
        public void WhenQueryableArgumentListHasTwoDifferentItemsThenListWithTwoCountByOfWithCountOfOneIsReturned() {
            var dummyList = new List<string> { "one", "two" };
            var countGroupBy = new CountGroupBy<string, string, Helpers.StringComparer>(dummyList.AsQueryable());

            var actual = countGroupBy.GroupAndCountBy(x => x);

            Assert.Equal(new List<CountBy<string>> { new CountBy<string>("one", 1), new CountBy<string>("two", 1) }, actual, new CountByComparer<string>());
        }
    }
}
