namespace TheCollection.Application.Services.Queries.Tea {
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TheCollection.Domain;
    using TheCollection.Domain.Core.Contracts;
    using TheCollection.Domain.Core.Contracts.Repository;
    using TheCollection.Domain.Tea;

    public class BagsCountByInsertDateQueryHandler : IAsyncQueryHandler<BagsCountByInsertDateQuery> {
        public BagsCountByInsertDateQueryHandler(IGetRepository<Dashboard<IEnumerable<CountBy<NodaTime.LocalDate>>>> repository) {
            Repository = repository;
        }

        IGetRepository<Dashboard<IEnumerable<CountBy<NodaTime.LocalDate>>>> Repository { get; }

        public async Task<IQueryResult> ExecuteAsync(BagsCountByInsertDateQuery query) {
            var bagsCountByInsertDate = await Repository.GetItemAsync(DashBoardTypes.BagsCountByPeriod.Key.ToString());
            if (bagsCountByInsertDate == null) {
                return new NotFoundResult();
            }

            return new OkResult(bagsCountByInsertDate.Data);
        }
    }
}
