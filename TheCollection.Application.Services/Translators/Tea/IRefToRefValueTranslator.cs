namespace TheCollection.Application.Services.Translators.Tea {
    using TheCollection.Domain.Core.Contracts;

    public class IRefToRefValueTranslator : ITranslator<IRef, ViewModels.RefValue> {
        public void Translate(IRef source, ViewModels.RefValue destination) {
            destination.id = source.Id;
            destination.name = source.Name;
        }
    }
}
