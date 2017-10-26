namespace TheCollection.Business.Tea
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    [JsonConverter(typeof(SearchableConverter))]
    public class BagType : IRef
    {
        [Key]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [Searchable]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
