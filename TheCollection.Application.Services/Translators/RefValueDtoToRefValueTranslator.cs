namespace TheCollection.Application.Services.Translators {
    using TheCollection.Domain.Core.Contracts;

    public class RefValueDtoToRefValueTranslator : ITranslator<ViewModels.RefValue, Domain.RefValue> {
        public RefValueDtoToRefValueTranslator() {
        }

        public Domain.RefValue Translate(ViewModels.RefValue source) {
            return new Domain.RefValue(source.id, source.name);
        }
    }
}
