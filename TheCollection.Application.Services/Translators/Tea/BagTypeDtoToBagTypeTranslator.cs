namespace TheCollection.Application.Services.Translators.Tea {
    using TheCollection.Domain.Core.Contracts;

    public class BagTypeDtoToBagTypeTranslator : ITranslator<ViewModels.Tea.BagType, Domain.Tea.BagType> {
        public BagTypeDtoToBagTypeTranslator() {
        }

        public Domain.Tea.BagType Translate(ViewModels.Tea.BagType source) {
            return new Domain.Tea.BagType(source.id, source.name);
        }
    }
}
