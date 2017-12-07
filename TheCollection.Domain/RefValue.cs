namespace TheCollection.Domain {
    using Newtonsoft.Json;

    public class RefValue : IRef {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [Searchable]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }

    public interface IRef {
        string Id { get; set; }

        string Name { get; set; }
    }
}