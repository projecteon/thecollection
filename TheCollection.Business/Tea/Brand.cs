using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace TheCollection.Business.Tea
{
    [JsonConverter(typeof(SearchableConverter))]
    public class Brand
    {
        [Key]
        public string Id { get; set; }

        [Searchable]
        public string Name { get; set; }
    }
}
