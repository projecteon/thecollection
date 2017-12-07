namespace TheCollection.Domain.Contracts.Repository {
    using System.Threading.Tasks;

    public interface IGetRepository<T> where T : class {
        Task<T> GetItemAsync(string id = null);
    }
}
