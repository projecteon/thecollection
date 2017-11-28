namespace TheCollection.Business.Tea
{
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;

    public enum DashBoardTypes {
        TotalBagsCountByPeriod,
        BagsCountByPeriod,
        BagsCountByBagTypes,
        BagsCountByBrands
    }

    public class Dashboard<T> {

        [Key]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "userid")]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "data")]
        public T Data { get; set; }
    }
}
