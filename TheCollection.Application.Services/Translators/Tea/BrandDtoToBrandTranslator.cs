namespace TheCollection.Application.Services.Translators.Tea {
    using TheCollection.Domain.Core.Contracts;

    public class BrandDtoToBrandTranslator : ITranslator<ViewModels.Tea.Brand, Domain.Tea.Brand> {
        public BrandDtoToBrandTranslator() {
        }

        public Domain.Tea.Brand Translate(ViewModels.Tea.Brand source) {
            return new Domain.Tea.Brand(source.id, source.name);
        }
    }
}
