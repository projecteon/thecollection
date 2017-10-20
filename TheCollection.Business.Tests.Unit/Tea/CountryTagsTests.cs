using System.Linq;
using TheCollection.Business.Tea;
using Xunit;

namespace TheCollection.Business.Tests.Unit.Tea
{
    [Trait("Country", "Tags are created")]
    public class CountryTagsTests
    {
        Country Country { get; }
        Searchable SearchableCountry { get; }

        public CountryTagsTests()
        {
            Country = new Country { Id = System.Guid.NewGuid().ToString(), Name = "Norway" };
            SearchableCountry = new Searchable(Country);
        }

        [Fact(DisplayName = "Tag contains name in lowercase")]
        public void ContainsNameLowerCase()
        {
            Assert.True(SearchableCountry.Tags.Any(tag => tag == Country.Name.ToLower()));
        }

        [Fact(DisplayName = "Tag does not contain Id")]
        public void NotContainsId()
        {
            Assert.False(SearchableCountry.Tags.Any(tag => tag.ToLower() == Country.Id.ToLower()));
        }
    }
}
