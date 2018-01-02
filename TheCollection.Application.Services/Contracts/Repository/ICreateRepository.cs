namespace TheCollection.Application.Services.Contracts.Repository {
    using System.Threading.Tasks;

    public interface ICreateRepository<T> where T : class {
        Task<string> CreateItemAsync(T item);
    }
}
