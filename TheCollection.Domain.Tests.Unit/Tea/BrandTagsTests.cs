namespace TheCollection.Domain.Tests.Unit.Tea {
    using System.Linq;
    using TheCollection.Domain.Tea;
    using Xunit;

    [Trait(nameof(Brand), "Tags are created")]
    public class BrandTagsTests {
        private Brand Brand { get; }
        private Searchable SearchableBrand { get; }

        public BrandTagsTests() {
            Brand = new Brand(System.Guid.NewGuid().ToString(), "Twinning");
            SearchableBrand = new Searchable(Brand);
        }

        [Fact(DisplayName = "Tag contains name in lowercase")]
        public void ContainsNameLowerCase() {
            Assert.NotNull(SearchableBrand.Tags.FirstOrDefault(tag => tag == Brand.Name.ToLower()));
        }

        [Fact(DisplayName = "Tag does not contain Id")]
        public void NotContainsId() {
            Assert.Null(SearchableBrand.Tags.FirstOrDefault(tag => tag.ToLower() == Brand.Id.ToLower()));
        }
    }
}
