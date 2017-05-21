using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace testspa.Models
{
    public class Image
    {
        [Key]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "filename")]
        public string Filename { get; set; }

        [JsonProperty(PropertyName = "filepath")]
        public string Filepath { get; set; }
    }
}
