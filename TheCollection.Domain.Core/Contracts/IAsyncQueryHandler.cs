namespace TheCollection.Domain.Core.Contracts {
    using System.Threading.Tasks;

    public interface IAsyncQueryHandler<TQuery>
        where TQuery : IQuery {
        Task<IQueryResult> ExecuteAsync(TQuery query);
    }
}
