using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using TheCollection.Business.Tea;
using Xunit;

namespace TheCollection.Business.Tests.Unit.Tea
{
    [Trait("Brand", "Tags are created")]
    public class BrandTagTests
    {
        Brand Brand { get; }
        Searchable SearchableBrand { get; }

        public BrandTagTests()
        {            
            Brand = new Brand { Id = System.Guid.NewGuid().ToString(), Name = "Twinning" };
            SearchableBrand = new Searchable(Brand);
        }

        [Fact(DisplayName = "Tag contains name in lowercase")]
        public void ContainsNameLowerCase()
        {
            Assert.True(SearchableBrand.Tags.Any(tag => tag == Brand.Name.ToLower()));
        }

        [Fact(DisplayName = "Tag does not contain Id")]
        public void NotContainsId()
        {
            Assert.False(SearchableBrand.Tags.Any(tag => tag.ToLower() == Brand.Id.ToLower()));
        }
    }
}
