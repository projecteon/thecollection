namespace TheCollection.Application.Services {
    using System.Linq;
    using TheCollection.Application.Services.Contracts;
    using TheCollection.Domain.Extensions;

    public class ActivityAuthorizer : IActivityAuthorizer {
        public ActivityAuthorizer(IApplicationUser applicationUser) {
            ApplicationUser = applicationUser;
        }

        IApplicationUser ApplicationUser { get; }

        public bool IsAuthorized(IActivity activity) {
            if (ApplicationUser == null || ApplicationUser.Roles.None()) {
                return false;
            }

            if (activity == null || activity.ValidRoles.None()) {
                return false;
            }

            if (ApplicationUser.Roles.Any(x => activity.ValidRoles.Any(y => x.Name == y.Name))) {
                return true;
            }

            return false;
        }
    }
}
