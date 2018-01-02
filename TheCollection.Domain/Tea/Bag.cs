namespace TheCollection.Domain.Tea {
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;
    using NodaTime;
    using TheCollection.Domain.Contracts;
    using TheCollection.Domain.Converters;

    [JsonConverter(typeof(SearchableConverter))]
    public class Bag: IOwnedEntity {
        [Key]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "userid")]
        public string OwnerId { get; set; }

        [Searchable]
        [JsonProperty(PropertyName = "mainid")]
        public int MainID { get; set; }

        [Searchable]
        [JsonProperty(PropertyName = "brand")]
        public RefValue Brand { get; set; }

        [Searchable]
        [JsonProperty(PropertyName = "serie")]
        public string Serie { get; set; }

        [Searchable]
        [JsonProperty(PropertyName = "flavour")]
        public string Flavour { get; set; }

        [Searchable]
        [JsonProperty(PropertyName = "hallmark")]
        public string Hallmark { get; set; }

        [Searchable]
        [JsonProperty(PropertyName = "bagtype")]
        public RefValue BagType { get; set; }

        [Searchable]
        [JsonProperty(PropertyName = "country")]
        public RefValue Country { get; set; }

        [Searchable]
        [JsonProperty(PropertyName = "serialnumber")]
        public string SerialNumber { get; set; }

        [JsonProperty(PropertyName = "insertdate")]
        public LocalDate InsertDate { get; set; }

        [JsonProperty(PropertyName = "imageid")]
        public string ImageId { get; set; }
    }
}
