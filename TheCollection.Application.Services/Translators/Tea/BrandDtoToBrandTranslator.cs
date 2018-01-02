namespace TheCollection.Application.Services.Translators.Tea {
    using TheCollection.Application.Services.Contracts;
    using TheCollection.Domain.Tea;

    public class BrandDtoToBrandTranslator : ITranslator<ViewModels.Tea.Brand, Brand> {
        public BrandDtoToBrandTranslator(IApplicationUser applicationUser) {
            ApplicationUser = applicationUser;
        }

        IApplicationUser ApplicationUser { get; }

        public void Translate(ViewModels.Tea.Brand source, Brand destination) {
            destination.Name = source.name;
        }
    }
}
