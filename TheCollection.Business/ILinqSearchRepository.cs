namespace TheCollection.Business {
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ILinqSearchRepository<T> where T : class {
        Task<IEnumerable<T>> SearchItemsAsync(System.Linq.Expressions.Expression<System.Func<T, bool>> predicate = null, int pageSize = 0, int page = 0);
    }
}
