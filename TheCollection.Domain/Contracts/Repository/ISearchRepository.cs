namespace TheCollection.Domain.Contracts.Repository {
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ISearchRepository<T> where T : class {
        Task<IEnumerable<T>> SearchAsync(string searchterm, int top = 0);

        Task<long> SearchRowCountAsync(string searchterm);
    }
}
