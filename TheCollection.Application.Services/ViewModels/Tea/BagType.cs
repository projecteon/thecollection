namespace TheCollection.Application.Services.ViewModels.Tea {
    public class BagType {
        public BagType(string id, string name, bool iseditable) {
            this.id = id;
            this.name = name;
            this.iseditable = iseditable;
        }

        public string id { get; set; }

        public string name { get; set; }

        public bool iseditable { get; set; }
    }
}
