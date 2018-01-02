namespace TheCollection.Web.Translators {
    using TheCollection.Web.Contracts;

    public class RefValueDtoToRefValueTranslator : ITranslator<Models.RefValue, Domain.RefValue> {
        public RefValueDtoToRefValueTranslator(IWebUser applicationUser) {
            ApplicationUser = applicationUser;
        }

        IWebUser ApplicationUser { get; }

        public void Translate(Models.RefValue source, Domain.RefValue destination) {
            destination.Id = source?.id;
            destination.Name = source?.name;
        }
    }
}
