namespace TheCollection.Application.Services.Translators.Tea {
    using System.Linq;
    using System.Threading.Tasks;
    using TheCollection.Application.Services.Constants;
    using TheCollection.Application.Services.Contracts;
    using TheCollection.Domain.Core.Contracts;
    using TheCollection.Domain.Core.Contracts.Repository;

    public class BrandToBrandTranslator : IAsyncTranslator<Domain.Tea.Brand, ViewModels.Tea.Brand> {
        public BrandToBrandTranslator(IGetRepository<IApplicationUser> repository) {
            Repository = repository ?? throw new System.ArgumentNullException(nameof(repository));
        }

        public IGetRepository<IApplicationUser> Repository { get; }

        public async Task<ViewModels.Tea.Brand> Translate(Domain.Tea.Brand source) {
            var applicationUser = await Repository.GetItemAsync();
            var iseditable = applicationUser.Roles.Any(x => x.Name == Roles.SystemAdministrator);
            return new ViewModels.Tea.Brand(source.Id, source.Name, iseditable);
        }
    }
}
