namespace TheCollection.Domain.Tea {
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;
    using TheCollection.Domain.Core.Contracts;
    using TheCollection.Domain.Converters;

    [JsonConverter(typeof(SearchableConverter))]
    public class BagType : IRef, IEntity {
        public BagType(string id, string name) {
            Id = id;
            Name = name;
        }

        [Key]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [Searchable]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
