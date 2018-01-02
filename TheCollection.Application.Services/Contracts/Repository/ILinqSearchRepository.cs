namespace TheCollection.Application.Services.Contracts.Repository {
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface ILinqSearchRepository<T> where T : class {
        Task<IEnumerable<T>> SearchItemsAsync(Expression<Func<T, bool>> predicate = null, int pageSize = 0, int page = 0);
    }
}
