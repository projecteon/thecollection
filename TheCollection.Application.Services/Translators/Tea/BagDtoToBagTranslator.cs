using TheCollection.Application.Services.Contracts;
using TheCollection.Application.Services.Extensions;

namespace TheCollection.Application.Services.Translators.Tea {

    public class BagDtoToBagTranslator : ITranslator<ViewModels.Tea.Bag, Domain.Tea.Bag> {
        public BagDtoToBagTranslator(IApplicationUser applicationUser) {
            RefValueTranslator = new RefValueDtoToRefValueTranslator(applicationUser);
        }

        ITranslator<ViewModels.RefValue, Domain.RefValue> RefValueTranslator { get; }

        public void Translate(ViewModels.Tea.Bag source, Domain.Tea.Bag destination) {
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
