using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace TheCollection.Business.Tea
{
    [JsonConverter(typeof(SearchableConverter))]
    public class Bag
    {
        [Key]
        [Searchable]
        public string Id { get; set; }

        [Searchable]
        public int MainID { get; set; }

        [Searchable]
        public Brand Brand { get; set; }

        [Searchable]
        public string Serie { get; set; }

        [Searchable]
        public string Flavour { get; set; }

        [Searchable]
        public string Hallmark { get; set; }

        //[Searchable]
        //public BagType Type { get; set; }

        //[Searchable]
        //public Country Country { get; set; }

        [Searchable]
        public string SerialNumber { get; set; }

        [Searchable]
        public string InsertDate { get; set; }

        [Searchable]
        public string ImageId { get; set; }
    }
}
