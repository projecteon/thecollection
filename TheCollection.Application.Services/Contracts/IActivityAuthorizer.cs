namespace TheCollection.Application.Services.Contracts {
    using System.Threading.Tasks;

    public interface IActivityAuthorizer {
        Task<bool> IsAuthorized(IActivity activity);
    }

    public interface IActivityAuthorizer<TEntity> {
        bool IsAuthorized(IActivity activity, TEntity entity);
    }    
}
