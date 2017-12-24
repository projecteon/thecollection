namespace TheCollection.Web.Models.Tea {
    using TheCollection.Web.Contracts;

    public class Brand : IDto {
        public string id { get; set; }

        public string name { get; set; }

        public bool iseditable { get; set; }
    }
}
