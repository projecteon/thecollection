namespace TheCollection.Business.Tea {

    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;

    [JsonConverter(typeof(SearchableConverter))]
    public class Country : IRef {

        [Key]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [Searchable]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
