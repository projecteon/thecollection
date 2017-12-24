using TheCollection.Domain.Tea;
using TheCollection.Web.Contracts;

namespace TheCollection.Web.Translators.Tea {
    public class BrandDtoToBrandTranslator : ITranslator<Models.Tea.Brand, Brand> {

        public BrandDtoToBrandTranslator(IApplicationUser applicationUser) {
            ApplicationUser = applicationUser;
        }

        IApplicationUser ApplicationUser { get; }

        public void Translate(Models.Tea.Brand source, Brand destination) {
            destination.Name = source.name;
        }
    }
}
