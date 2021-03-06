namespace TheCollection.Domain.Core.Contracts.Repository {
    using System.Threading.Tasks;

    public interface IUpsertRepository<T> where T : class {
        Task<string> UpsertItemAsync(string id, T item);
    }
}
