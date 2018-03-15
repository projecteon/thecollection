namespace TheCollection.Application.Services.Commands.Tea {
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TheCollection.Domain;
    using TheCollection.Domain.Core.Contracts;
    using TheCollection.Domain.Core.Contracts.Repository;
    using TheCollection.Domain.Extensions;
    using TheCollection.Domain.Tea;

    public class CreateTotalBagsCountByInsertDateCommandHandler : IAsyncCommandHandler<CreateTotalBagsCountByInsertDateCommand> {

        public CreateTotalBagsCountByInsertDateCommandHandler(ILinqSearchRepository<Bag> searchRepository, IUpsertRepository<Dashboard<IEnumerable<CountBy<NodaTime.LocalDate>>>> upsertRepository) {
            SearchRepository = searchRepository ?? throw new System.ArgumentNullException(nameof(searchRepository));
            UpsertRepository = upsertRepository ?? throw new System.ArgumentNullException(nameof(upsertRepository));
        }

        ILinqSearchRepository<Bag> SearchRepository { get; }
        IUpsertRepository<Dashboard<IEnumerable<CountBy<NodaTime.LocalDate>>>> UpsertRepository { get; }

        public async Task<ICommandResult> ExecuteAsync(CreateTotalBagsCountByInsertDateCommand command) {
            var bags = await SearchRepository.SearchItemsAsync();
            var queryablebags = bags.AsQueryable();
            var countGroupBy = new CountGroupBy<Bag, NodaTime.LocalDate, LocalDateComparer>(queryablebags);
            var bagsCountByInsertDate = countGroupBy.GroupAndCountBy(x => x.InsertDate.With(NodaTime.DateAdjusters.StartOfMonth))
                                                       .OrderBy(x => x.Value);

            var firstCountBy = bagsCountByInsertDate.First();
            var firstPeriod = firstCountBy.Value.With(NodaTime.DateAdjusters.StartOfMonth).PlusMonths(-1);
            var totalBagsCountByPeriods = bagsCountByInsertDate.ToList().Scan((state, item) => {
                return new CountBy<NodaTime.LocalDate>(item.Value, item.Count + state.Count);
            }, new CountBy<NodaTime.LocalDate>(firstPeriod, 0));
            var dashboard = new Dashboard<IEnumerable<CountBy<NodaTime.LocalDate>>>(DashBoardTypes.BagsCountByPeriod.Key.ToString(), command.User.Id, DashBoardTypes.BagsCountByPeriod, totalBagsCountByPeriods);
            await UpsertRepository.UpsertItemAsync(dashboard.Id, dashboard);
            return new OkResult();
        }
    }
}
