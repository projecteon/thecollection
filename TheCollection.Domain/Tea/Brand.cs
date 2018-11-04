namespace TheCollection.Domain.Tea {
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;
    using TheCollection.Domain.Core.Contracts;
    using TheCollection.Domain.Converters;

    [JsonConverter(typeof(SearchableConverter))]
    public class Brand : IRef, IEntity {
        public Brand(string id, string name) {
            Id = id;
            Name = name;
        }

        [Key]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; }

        [Searchable]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; }
    }
}
