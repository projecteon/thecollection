namespace TheCollection.Application.Services.Translators.Tea {
    using System.Linq;
    using System.Threading.Tasks;
    using TheCollection.Application.Services.Constants;
    using TheCollection.Application.Services.Contracts;
    using TheCollection.Domain.Core.Contracts;
    using TheCollection.Domain.Core.Contracts.Repository;

    public class BagTypeToBagTypeTranslator : IAsyncTranslator<Domain.Tea.BagType, ViewModels.Tea.BagType> {
        public BagTypeToBagTypeTranslator(IGetRepository<IApplicationUser> repository) {
            Repository = repository;
        }

        IGetRepository<IApplicationUser> Repository { get; }

        public async Task<ViewModels.Tea.BagType> Translate(Domain.Tea.BagType source) {
            var applicationUser = await Repository.GetItemAsync();
            var iseditable = applicationUser.Roles.Any(x => x.Name == Roles.SystemAdministrator);
            return new ViewModels.Tea.BagType(source.Id, source.Name, iseditable);
        }
    }
}
