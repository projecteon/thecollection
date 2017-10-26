namespace TheCollection.Business.Tests.Unit.Tea {

    using System.Linq;
    using TheCollection.Business.Tea;
    using Xunit;

    [Trait("Country", "Tags are created")]
    public class CountryTagsTests {
        private Country Country { get; }
        private Searchable SearchableCountry { get; }

        public CountryTagsTests() {
            Country = new Country { Id = System.Guid.NewGuid().ToString(), Name = "Norway" };
            SearchableCountry = new Searchable(Country);
        }

        [Fact(DisplayName = "Tag contains name in lowercase")]
        public void ContainsNameLowerCase() {
            Assert.NotNull(SearchableCountry.Tags.FirstOrDefault(tag => tag == Country.Name.ToLower()));
        }

        [Fact(DisplayName = "Tag does not contain Id")]
        public void NotContainsId() {
            Assert.Null(SearchableCountry.Tags.FirstOrDefault(tag => tag.ToLower() == Country.Id.ToLower()));
        }
    }
}
