namespace TheCollection.Application.Services.Queries.Tea {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TheCollection.Domain;
    using TheCollection.Domain.Core.Contracts;
    using TheCollection.Domain.Core.Contracts.Repository;
    using TheCollection.Domain.Tea;

    public class BagsCountByBrandsQueryHandler : IAsyncQueryHandler<BagsCountByBrandsQuery> {
        public BagsCountByBrandsQueryHandler(IGetRepository<Dashboard<IEnumerable<CountBy<RefValue>>>> repository) {
            Repository = repository;
        }

        IGetRepository<Dashboard<IEnumerable<CountBy<RefValue>>>> Repository { get; }

        public async Task<IQueryResult> ExecuteAsync(BagsCountByBrandsQuery query) {
            var bagsCountByBrand = await Repository.GetItemAsync(DashBoardTypes.BagsCountByBrands.Key.ToString());
            if (bagsCountByBrand == null) {
                return new NotFoundResult();
            }

            return new OkResult(bagsCountByBrand.Data.Take(Math.Min(query.Top, 30)).OrderBy(x => x.Value?.Name));
        }
    }
}
