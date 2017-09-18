using System.Linq;
using TheCollection.Business.Tea;
using Xunit;

namespace TheCollection.Business.Tests.Unit.Tea
{
    [Trait("BagType", "Tags are created")]
    public class BagTypeTagsTests
    {
        BagType BagType { get; }
        Searchable SearchableBrand { get; }

        public BagTypeTagsTests()
        {
            BagType = new BagType { Id = System.Guid.NewGuid().ToString(), Name = "Paper" };
            SearchableBrand = new Searchable(BagType);
        }

        [Fact(DisplayName = "Tag contains name in lowercase")]
        public void ContainsNameLowerCase()
        {
            Assert.True(SearchableBrand.Tags.Any(tag => tag == BagType.Name.ToLower()));
        }

        [Fact(DisplayName = "Tag does not contain Id")]
        public void NotContainsId()
        {
            Assert.False(SearchableBrand.Tags.Any(tag => tag.ToLower() == BagType.Id.ToLower()));
        }
    }
}
