namespace TheCollection.Web.Translators.Tea {
    using TheCollection.Web.Contracts;
    using TheCollection.Web.Extensions;

    public class BagDtoToBagTranslator : ITranslator<Models.Tea.Bag, Domain.Tea.Bag> {
        public BagDtoToBagTranslator(IApplicationUser applicationUser) {
            RefValueTranslator = new RefValueDtoToRefValueTranslator(applicationUser);
        }

        ITranslator<Models.RefValue, Domain.RefValue> RefValueTranslator { get; }

        public void Translate(Models.Tea.Bag source, Domain.Tea.Bag destination) {
            destination.MainID = source.mainid;
            destination.Brand = RefValueTranslator.Translate(source.brand);
            destination.Serie = source.serie;
            destination.Flavour = source.flavour;
            destination.Hallmark = source.hallmark;
            destination.BagType = RefValueTranslator.Translate(source.bagtype);
            destination.Country = RefValueTranslator.Translate(source.country);
            destination.SerialNumber = source.serialnumber;
            destination.ImageId = source.imageid;
        }
    }
}
