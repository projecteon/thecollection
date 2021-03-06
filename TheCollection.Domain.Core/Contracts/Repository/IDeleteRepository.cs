namespace TheCollection.Domain.Core.Contracts.Repository {
    using System.Threading.Tasks;

    public interface IDeleteRepository<T> where T : class {
        Task DeleteItemAsync(string id);
    }
}
