using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace TheCollection.Web.Models
{
    public class Image
    {
        [Key]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "filename")]
        public string Filename { get; set; }

        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }
    }
}
