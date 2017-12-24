namespace TheCollection.Domain {
    using Newtonsoft.Json;
    using TheCollection.Domain.Contracts;

    public class RefValue : IRef {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [Searchable]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
