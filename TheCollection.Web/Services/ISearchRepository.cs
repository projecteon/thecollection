namespace TheCollection.Web.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ISearchRepository<T> where T : class
    {
        Task<IEnumerable<T>> SearchAsync(string searchterm, int top = 0);
    }
}
