namespace TheCollection.Domain.Tests.Unit {
    using TheCollection.Domain;
    using Xunit;

    [Trait(nameof(RefValueComparer), "RefValueComparer equality tests")]
    public class RefValueComparerTests {
        [Fact(DisplayName = "When RefValues are same object then return true")]
        public void WhenRefValuesAreSameObjectThenReturnTrue() {
            var refValue = new RefValue { Id = "someid", Name = "somename" };
            var comparer = new RefValueComparer();

            Assert.True(comparer.Equals(refValue, refValue));
        }

        [Fact(DisplayName = "When RefValues have the same id and name then return true")]
        public void WhenRefValuesHaveTheSameIdAndNameThenReturnTrue() {
            var refValue = new RefValue { Id = "someid", Name = "somename" };
            var refValue2 = new RefValue { Id = "someid", Name = "somename" };
            var comparer = new RefValueComparer();

            Assert.True(comparer.Equals(refValue, refValue2));
        }

        [Fact(DisplayName = "When RefValues have the same id but different name then return true")]
        public void WhenRefValuesHaveTheSameIdButDifferentNameThenReturnTrue() {
            var refValue = new RefValue { Id = "someid", Name = "somename" };
            var refValue2 = new RefValue { Id = "someid", Name = "somename2" };
            var comparer = new RefValueComparer();

            Assert.True(comparer.Equals(refValue, refValue2));
        }

        [Fact(DisplayName = "When RefValues have different ids and name then return false")]
        public void WhenRefValuesHaveDifferentIdsAndNameThenReturnTrue() {
            var refValue = new RefValue { Id = "someid", Name = "somename" };
            var refValue2 = new RefValue { Id = "someid2", Name = "somename2" };
            var comparer = new RefValueComparer();

            Assert.False(comparer.Equals(refValue, refValue2));
        }

        [Fact(DisplayName = "When RefValues have different ids but same name then return false")]
        public void WhenRefValuesHaveDifferentIdsButSameNameThenReturnTrue() {
            var refValue = new RefValue { Id = "someid", Name = "somename" };
            var refValue2 = new RefValue { Id = "someid2", Name = "somename" };
            var comparer = new RefValueComparer();

            Assert.False(comparer.Equals(refValue, refValue2));
        }
    }
}
