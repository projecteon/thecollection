namespace TheCollection.Web.Models.Tea
{
    public class Bag
    {
        public string Id { get; set; }

        public int MainID { get; set; }

        public RefValue Brand { get; set; }

        public string Serie { get; set; }

        public string Flavour { get; set; }

        public string Hallmark { get; set; }

        public RefValue BagType { get; set; }

        public RefValue Country { get; set; }

        public string SerialNumber { get; set; }

        public string InsertDate { get; set; }

        public string ImageId { get; set; }
    }
}
