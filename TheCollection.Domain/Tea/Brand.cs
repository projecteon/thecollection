namespace TheCollection.Domain.Tea {
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;
    using TheCollection.Domain.Contracts;
    using TheCollection.Domain.Converters;

    [JsonConverter(typeof(SearchableConverter))]
    public class Brand : IRef, IEntity {
        [Key]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [Searchable]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
