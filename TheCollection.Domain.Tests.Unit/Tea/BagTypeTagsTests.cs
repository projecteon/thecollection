namespace TheCollection.Domain.Tests.Unit.Tea {
    using System.Linq;
    using TheCollection.Domain.Tea;
    using Xunit;

    [Trait(nameof(BagType), "Tags are created")]
    public class BagTypeTagsTests {
        private BagType BagType { get; }
        private Searchable SearchableBagType { get; }

        public BagTypeTagsTests() {
            BagType = new BagType { Id = System.Guid.NewGuid().ToString(), Name = "Paper" };
            SearchableBagType = new Searchable(BagType);
        }

        [Fact(DisplayName = "Tag contains name in lowercase")]
        public void ContainsNameLowerCase() {
            Assert.NotNull(SearchableBagType.Tags.FirstOrDefault(tag => tag == BagType.Name.ToLower()));
        }

        [Fact(DisplayName = "Tag does not contain Id")]
        public void NotContainsId() {
            Assert.Null(SearchableBagType.Tags.FirstOrDefault(tag => tag.ToLower() == BagType.Id.ToLower()));
        }
    }
}
