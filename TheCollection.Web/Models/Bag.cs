using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace TheCollection.Web.Models
{
    public class Bag : ISearchable
    {
        [Key]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "mainid")]
        public int MainID { get; set; }

        [JsonProperty(PropertyName = "brand")]
        public Brand Brand { get; set; }

        [JsonProperty(PropertyName = "serie")]
        public string Serie { get; set; }

        [JsonProperty(PropertyName = "flavour")]
        public string Flavour { get; set; }

        [JsonProperty(PropertyName = "hallmark")]
        public string Hallmark { get; set; }

        [JsonProperty(PropertyName = "type")]
        public BagType Type { get; set; }

        [JsonProperty(PropertyName = "country")]
        public Country Country { get; set; }

        [JsonProperty(PropertyName = "serialnumber")]
        public string SerialNumber { get; set; }

        [JsonProperty(PropertyName = "insertdate")]
        public string InsertDate { get; set; }

        [JsonProperty(PropertyName = "image")]
        public string Image { get; set; }

        [JsonProperty(PropertyName = "tags")]
        public IEnumerable<string> Tags
        {
            get { return Services.Tags.Generate(this.Flavour + " " + this.Brand?.Name + " " + this.Country?.Name + " " + this.SerialNumber + " " + this.Hallmark + " " + this.Serie); }
        }

        [JsonProperty(PropertyName = "searchstring")]
        public string SearchString
        {
            get { return Tags.Aggregate((current, next) => current + " " + next); }
        }
    }
}
