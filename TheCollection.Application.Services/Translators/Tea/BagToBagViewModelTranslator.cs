namespace TheCollection.Application.Services.Translators.Tea {
    using System.Threading.Tasks;
    using TheCollection.Domain.Core.Contracts;

    public class BagToBagViewModelTranslator : IAsyncTranslator<Domain.Tea.Bag, ViewModels.Tea.Bag> {
        public BagToBagViewModelTranslator(IAsyncTranslator<Domain.RefValue, ViewModels.RefValue> translator) {
            Translator = translator;
        }

        IAsyncTranslator<Domain.RefValue, ViewModels.RefValue> Translator { get; }

        public async Task<ViewModels.Tea.Bag> Translate(Domain.Tea.Bag source) {
            return new ViewModels.Tea.Bag(source.Id,
                await Translator.Translate(source.Brand),
                source.Serie,
                source.Flavour,
                source.Hallmark,
                await Translator.Translate(source.BagType),
                await Translator.Translate(source.Country),
                source.SerialNumber,
                source.InsertDate,
                source.ImageId);
        }
    }
}
