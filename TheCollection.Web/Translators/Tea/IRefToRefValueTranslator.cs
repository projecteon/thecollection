namespace TheCollection.Web.Translators.Tea {

    using TheCollection.Business;

    public class IRefToRefValueTranslator : ITranslator<IRef, Models.RefValue> {

        public void Translate(IRef source, Models.RefValue destination) {
            destination.id = source.Id;
            destination.name = source.Name;
        }
    }
}
