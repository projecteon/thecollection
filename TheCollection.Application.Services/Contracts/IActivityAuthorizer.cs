namespace TheCollection.Application.Services.Contracts {
    using System.Linq;
    using TheCollection.Domain.Extensions;

    public interface IActivityAuthorizer {
        bool IsAuthorized(IActivity activity);
    }

    public interface IActivityAuthorizer<TEntity> {
        bool IsAuthorized(IActivity activity, TEntity entity);
    }

    //public class ActivityAuthorizer : IActivityAuthorizer {
    //    public ActivityAuthorizer(IApplicationUser applicationUser) {
    //        ApplicationUser = applicationUser;
    //    }

    //    IApplicationUser ApplicationUser { get; }

    //    public bool IsAuthorized(IActivity activity) {
    //        if (ApplicationUser.Roles.None()) {
    //            return false;
    //        }

    //        if (activity.ValidRoles.None()) {
    //            return false;
    //        }

    //        if (ApplicationUser.Roles.Any(x => activity.ValidRoles.Any(y => x.Name == y.Name))) {
    //            return true;
    //        }

    //        return false;
    //    }
    //}
}
