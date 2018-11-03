namespace TheCollection.Application.Services.Queries.Tea {
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TheCollection.Domain;
    using TheCollection.Domain.Core.Contracts;
    using TheCollection.Domain.Core.Contracts.Repository;
    using TheCollection.Domain.Tea;

    public class TotalBagsCountByInsertDateQueryHandler : IAsyncQueryHandler<TotalBagsCountByInsertDateQuery> {
        public TotalBagsCountByInsertDateQueryHandler(IGetRepository<Dashboard<IEnumerable<CountBy<NodaTime.LocalDate>>>> repository) {
            Repository = repository;
        }

        IGetRepository<Dashboard<IEnumerable<CountBy<NodaTime.LocalDate>>>> Repository { get; }

        public async Task<IQueryResult> ExecuteAsync(TotalBagsCountByInsertDateQuery query) {
            var totalBagsCountByPeriods = await Repository.GetItemAsync(DashBoardTypes.TotalBagsCountByPeriod.Key.ToString());
            if (totalBagsCountByPeriods == null) {
                return new NotFoundResult();
            }

            return new OkResult(totalBagsCountByPeriods.Data);
        }
    }
}
