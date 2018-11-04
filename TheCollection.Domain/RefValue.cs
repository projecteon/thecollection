namespace TheCollection.Domain {
    using Newtonsoft.Json;
    using TheCollection.Domain.Core.Contracts;

    public class RefValue : IRef {
        public RefValue(string id, string name) {
            Id = id;
            Name = name;
        }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; }

        [Searchable]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; }
    }
}
