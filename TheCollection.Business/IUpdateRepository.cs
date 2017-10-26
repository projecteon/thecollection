namespace TheCollection.Business {

    using System.Threading.Tasks;

    public interface IUpdateRepository<T> where T : class {

        Task<string> UpdateItemAsync(string id, T item);
    }
}
