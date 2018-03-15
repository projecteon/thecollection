namespace TheCollection.Application.Services.ViewModels.Tea {
    public class Country {
        public Country(string id, string name, bool isEditable) {
            Id = id;
            Name = name;
            IsEditable = isEditable;
        }

        public string Id { get; }
        public string Name { get; }
        public bool IsEditable { get; }
    }
}
