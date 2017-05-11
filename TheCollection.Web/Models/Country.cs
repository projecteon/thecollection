using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace TheCollection.Web.Models
{
    public class Country
    {
        [Key]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
