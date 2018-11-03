namespace TheCollection.Domain.Tea {
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;
    using TheCollection.Domain.Core.Contracts;

    public class Image: IEntity {
        public Image(string id, string filename, string uri) {
            Id = id;
            Filename = filename;
            Uri = uri;
        }

        [Key]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "filename")]
        public string Filename { get; }

        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; }
    }
}
