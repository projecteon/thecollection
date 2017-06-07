using Newtonsoft.Json;
using TheCollection.Business.Tea;
using Xunit;

namespace TheCollection.Business.Tests.Unit.Tea
{
    public class SearchableBrandTests
    {
        //[Fact]
        //public void Test2()
        //{
        //    var searchableBrand = new Brand { Name = "Twinning" };
        //    var json = JsonConvert.SerializeObject(searchableBrand, Formatting.Indented, new SearchableConverter());
        //    var reloadBrand = JsonConvert.DeserializeObject<Brand>("{\"id\":null,\"name\":\"Twinning\",\"tags\":[\"twinning\"],\"searchstring\":\"twinning\"}");
        //    Assert.Equal(searchableBrand.Name, "twinning");
        //}

        [Fact]
        public void Test2()
        {
            var bag = new Bag { Brand = new Brand { Name = "Twinning" } };
            var json = JsonConvert.SerializeObject(bag);
            //var reloadBrand = JsonConvert.DeserializeObject<Brand>("{\"id\":null,\"name\":\"Twinning\",\"tags\":[\"twinning\"],\"searchstring\":\"twinning\"}");
            Assert.Equal(bag.Brand.Name, "twinning");
        }

        [Fact]
        public void Test3()
        {
            var searchableBrand = new Brand { Name = "Twinning" };
            var json = JsonConvert.SerializeObject(searchableBrand);
            var reloadBrand = JsonConvert.DeserializeObject<Brand>("{\"id\":null,\"name\":\"Twinning\",\"tags\":[\"twinning\"],\"searchstring\":\"twinning\"}");
            Assert.Equal(searchableBrand.Name, "twinning");
        }
    }
}
