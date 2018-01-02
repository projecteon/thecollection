namespace TheCollection.Application.Services.Translators {
    using TheCollection.Application.Services.Contracts;

    public class RefValueDtoToRefValueTranslator : ITranslator<ViewModels.RefValue, Domain.RefValue> {
        public RefValueDtoToRefValueTranslator(IApplicationUser applicationUser) {
            ApplicationUser = applicationUser;
        }

        IApplicationUser ApplicationUser { get; }

        public void Translate(ViewModels.RefValue source, Domain.RefValue destination) {
            destination.Id = source?.id;
            destination.Name = source?.name;
        }
    }
}
