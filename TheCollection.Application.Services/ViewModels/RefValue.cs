namespace TheCollection.Application.Services.ViewModels {

    public class RefValue {
        public RefValue(string id, string name, bool canaddnew) {
            this.id = id;
            this.name = name;
            this.canaddnew = canaddnew;
        }

        public string id { get; }
        public string name { get; }
        public bool canaddnew { get; }
    }
}
