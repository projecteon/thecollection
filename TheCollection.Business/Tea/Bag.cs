using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace TheCollection.Business.Tea
{
    [JsonConverter(typeof(SearchableConverter))]
    public class Bag
    {
        [Key]
        public string Id { get; set; }

        public int MainID { get; set; }

        [Searchable]
        public RefValue Brand { get; set; }

        [Searchable]
        public string Serie { get; set; }

        [Searchable]
        public string Flavour { get; set; }

        [Searchable]
        public string Hallmark { get; set; }

        [Searchable]
        public RefValue BagType { get; set; }

        [Searchable]
        public RefValue Country { get; set; }

        [Searchable]
        public string SerialNumber { get; set; }

        public string InsertDate { get; set; }

        public string ImageId { get; set; }
    }
}
