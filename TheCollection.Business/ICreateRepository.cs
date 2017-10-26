namespace TheCollection.Business {

    using System.Threading.Tasks;

    public interface ICreateRepository<T> where T : class {

        Task<string> CreateItemAsync(T item);
    }
}
