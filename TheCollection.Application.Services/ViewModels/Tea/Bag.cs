namespace TheCollection.Application.Services.ViewModels.Tea {
    using NodaTime;
    using TheCollection.Domain.Core.Contracts;

    public class Bag: IEntity {
        public Bag(string id, RefValue brand, string serie, string flavour, string hallmark, RefValue bagType, RefValue country, string serialNumber, LocalDate insertDate, string imageId) {
            Id = id;
            Brand = brand;
            Serie = serie;
            Flavour = flavour;
            Hallmark = hallmark;
            BagType = bagType;
            Country = country;
            SerialNumber = serialNumber;
            InsertDate = insertDate;
            ImageId = imageId;
        }

        public string Id { get; }
        public RefValue Brand { get; }
        public string Serie { get; }
        public string Flavour { get; }
        public string Hallmark { get; }
        public RefValue BagType { get; }
        public RefValue Country { get; }
        public string SerialNumber { get; }
        public LocalDate InsertDate { get; }
        public string ImageId { get; }
        public bool iseditable { get; }
    }
}
