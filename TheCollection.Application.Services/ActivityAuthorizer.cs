namespace TheCollection.Application.Services {
    using System.Linq;
    using System.Threading.Tasks;
    using TheCollection.Application.Services.Contracts;
    using TheCollection.Domain.Core.Contracts.Repository;
    using TheCollection.Domain.Extensions;

    public class ActivityAuthorizer : IActivityAuthorizer {
        public ActivityAuthorizer(IGetRepository<IApplicationUser> repository) {
            Repository = repository ?? throw new System.ArgumentNullException(nameof(repository));
        }

        public IGetRepository<IApplicationUser> Repository { get; }

        public async Task<bool> IsAuthorized(IActivity activity) {
            var applicationUser = await Repository.GetItemAsync();
            if (applicationUser == null || applicationUser.Roles.None()) {
                return false;
            }

            if (activity == null || activity.ValidRoles.None()) {
                return false;
            }

            if (applicationUser.Roles.Any(x => activity.ValidRoles.Any(y => x.Name == y.Name))) {
                return true;
            }

            return false;
        }
    }
}
