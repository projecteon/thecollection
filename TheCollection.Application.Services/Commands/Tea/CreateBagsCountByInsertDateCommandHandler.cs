namespace TheCollection.Application.Services.Commands.Tea {
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TheCollection.Domain;
    using TheCollection.Domain.Core.Contracts;
    using TheCollection.Domain.Core.Contracts.Repository;
    using TheCollection.Domain.Tea;

    public class CreateBagsCountByInsertDateCommandHandler : IAsyncCommandHandler<CreateBagsCountByInsertDateCommand> {
        public CreateBagsCountByInsertDateCommandHandler(ILinqSearchRepository<Bag> searchRepository, IUpsertRepository<Dashboard<IEnumerable<CountBy<NodaTime.LocalDate>>>> upsertRepository) {
            SearchRepository = searchRepository ?? throw new System.ArgumentNullException(nameof(searchRepository));
            UpsertRepository = upsertRepository ?? throw new System.ArgumentNullException(nameof(upsertRepository));
        }

        ILinqSearchRepository<Bag> SearchRepository { get; }
        IUpsertRepository<Dashboard<IEnumerable<CountBy<NodaTime.LocalDate>>>> UpsertRepository { get; }

        public async Task<ICommandResult> ExecuteAsync(CreateBagsCountByInsertDateCommand command) {
            var bags = await SearchRepository.SearchItemsAsync();
            var queryablebags = bags.AsQueryable();

            var countGroupByLocalDate = new CountGroupBy<Bag, NodaTime.LocalDate, LocalDateComparer>(queryablebags);
            var bagsCountByLocalDate = countGroupByLocalDate.GroupAndCountBy(x => x.InsertDate.With(NodaTime.DateAdjusters.StartOfMonth))
                                                            .OrderByDescending(x => x.Value);
            var dashboard = new Dashboard<IEnumerable<CountBy<NodaTime.LocalDate>>>(DashBoardTypes.BagsCountByPeriod.Key.ToString(), command.User.Id, DashBoardTypes.BagsCountByPeriod, bagsCountByLocalDate);
            await UpsertRepository.UpsertItemAsync(dashboard.Id, dashboard);
            return new OkResult();
        }
    }
}
