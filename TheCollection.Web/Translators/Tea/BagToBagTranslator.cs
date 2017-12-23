namespace TheCollection.Web.Translators.Tea {
    using TheCollection.Web.Contracts;
    using TheCollection.Web.Extensions;

    public class BagToBagTranslator : ITranslator<Domain.Tea.Bag, Models.Tea.Bag> {

        public BagToBagTranslator(IApplicationUser applicationUser) {
            RefValueTranslator = new RefValueToRefValueTranslator(applicationUser);
            ApplicationUser = applicationUser;
        }

        ITranslator<Domain.RefValue, Models.RefValue> RefValueTranslator { get; }
        IApplicationUser ApplicationUser { get; }

        public void Translate(Domain.Tea.Bag source, Models.Tea.Bag destination) {
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
            destination.iseditable = source.UserId == ApplicationUser.Id;
        }
    }
}
