namespace TheCollection.Application.Services.Queries.Tea {
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TheCollection.Domain;
    using TheCollection.Domain.Core.Contracts;
    using TheCollection.Domain.Core.Contracts.Repository;
    using TheCollection.Domain.Tea;

    public class BagsCountByBagTypesQueryHandler : IAsyncQueryHandler<BagsCountByBagTypesQuery> {
        public BagsCountByBagTypesQueryHandler(IGetRepository<Dashboard<IEnumerable<CountBy<RefValue>>>> repository) {
            Repository = repository;
        }

        IGetRepository<Dashboard<IEnumerable<CountBy<RefValue>>>> Repository { get; }

        public async Task<IQueryResult> ExecuteAsync(BagsCountByBagTypesQuery query) {
            var bagsCountByBagType = await Repository.GetItemAsync(DashBoardTypes.BagsCountByBagTypes.Key.ToString());
            if (bagsCountByBagType == null) {
                return new NotFoundResult();
            }

            return new OkResult(bagsCountByBagType.Data);
        }
    }
}
