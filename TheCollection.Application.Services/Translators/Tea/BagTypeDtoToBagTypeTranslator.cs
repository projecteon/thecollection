namespace TheCollection.Application.Services.Translators.Tea {
    using TheCollection.Application.Services.Contracts;
    using TheCollection.Domain.Tea;

    public class BagTypeDtoToBagTypeTranslator : ITranslator<ViewModels.Tea.BagType, BagType> {
        public BagTypeDtoToBagTypeTranslator(IApplicationUser applicationUser) {
            ApplicationUser = applicationUser;
        }

        IApplicationUser ApplicationUser { get; }

        public void Translate(ViewModels.Tea.BagType source, BagType destination) {
            destination.Name = source.name;
        }
    }
}
