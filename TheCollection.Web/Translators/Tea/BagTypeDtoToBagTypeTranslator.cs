namespace TheCollection.Web.Translators.Tea {
    using TheCollection.Domain.Tea;
    using TheCollection.Web.Contracts;
    public class BagTypeDtoToBagTypeTranslator : ITranslator<Models.Tea.BagType, BagType> {
        public BagTypeDtoToBagTypeTranslator(IApplicationUser applicationUser) {
            ApplicationUser = applicationUser;
        }

        IApplicationUser ApplicationUser { get; }

        public void Translate(Models.Tea.BagType source, BagType destination) {
            destination.Name = source.name;
        }
    }
}
