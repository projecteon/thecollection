namespace TheCollection.Application.Services.Translators.Tea {
    using System.Linq;
    using TheCollection.Application.Services.Constants;
    using TheCollection.Application.Services.Contracts;
    using TheCollection.Application.Services.Extensions;

    public class BagToBagTranslator : ITranslator<Domain.Tea.Bag, ViewModels.Tea.Bag> {

        public BagToBagTranslator(IApplicationUser applicationUser) {
            RefValueTranslator = new RefValueToRefValueTranslator(applicationUser);
            ApplicationUser = applicationUser;
        }

        ITranslator<Domain.RefValue, ViewModels.RefValue> RefValueTranslator { get; }
        IApplicationUser ApplicationUser { get; }

        public void Translate(Domain.Tea.Bag source, ViewModels.Tea.Bag destination) {
            destination.id = source.Id;
            destination.mainid = source.MainID;
            destination.brand = RefValueTranslator.Translate(source.Brand);
            destination.serie = source.Serie;
            destination.flavour = source.Flavour;
            destination.hallmark = source.Hallmark;
            destination.bagtype = RefValueTranslator.Translate(source.BagType);
            destination.country = RefValueTranslator.Translate(source.Country);
            destination.serialnumber = source.SerialNumber;
            destination.insertdate = source.InsertDate;
            destination.imageid = source.ImageId;
            destination.iseditable = source.OwnerId == ApplicationUser.Id || ApplicationUser.Roles.Any(x => x.Name == Roles.SystemAdministrator);
        }
    }
}
