namespace TheCollection.Domain.Core.Contracts.Repository {
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IGetAllRepository<T> where T : class {
        Task<IEnumerable<T>> GetAllAsync();
    }
}
