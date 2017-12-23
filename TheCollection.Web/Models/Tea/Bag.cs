using NodaTime;

namespace TheCollection.Web.Models.Tea {

    public class Bag {
        public string id { get; set; }

        public int mainid { get; set; }

        public RefValue brand { get; set; }

        public string serie { get; set; }

        public string flavour { get; set; }

        public string hallmark { get; set; }

        public RefValue bagtype { get; set; }

        public RefValue country { get; set; }

        public string serialnumber { get; set; }

        public LocalDate insertdate { get; set; }

        public string imageid { get; set; }

        public bool iseditable { get; set; }
    }
}
