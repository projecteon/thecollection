namespace TheCollection.Application.Services.Commands.Tea {
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TheCollection.Domain;
    using TheCollection.Domain.Core.Contracts;
    using TheCollection.Domain.Core.Contracts.Repository;
    using TheCollection.Domain.Tea;

    public class CreateBagsCountByBagTypesCommandHandler : IAsyncCommandHandler<CreateBagsCountByBagTypesCommand> {
        public CreateBagsCountByBagTypesCommandHandler(ILinqSearchRepository<Bag> searchRepository, IUpsertRepository<Dashboard<IEnumerable<CountBy<RefValue>>>> upsertRepository) {
            SearchRepository = searchRepository ?? throw new System.ArgumentNullException(nameof(searchRepository));
            UpsertRepository = upsertRepository ?? throw new System.ArgumentNullException(nameof(upsertRepository));
        }

        ILinqSearchRepository<Bag> SearchRepository { get; }
        IUpsertRepository<Dashboard<IEnumerable<CountBy<RefValue>>>> UpsertRepository { get; }

        public async Task<ICommandResult> ExecuteAsync(CreateBagsCountByBagTypesCommand command) {
            var bags = await SearchRepository.SearchItemsAsync();
            var queryablebags = bags.AsQueryable();
            var countGroupByIRef = new CountGroupBy<Bag, RefValue, RefValueComparer>(queryablebags);
            var bagsCountByBagType = countGroupByIRef.GroupAndCountBy(x => x.BagType);
            var dashboard = new Dashboard<IEnumerable<CountBy<RefValue>>>(DashBoardTypes.BagsCountByBagTypes.Key.ToString(), command.User.Id, DashBoardTypes.BagsCountByBagTypes, bagsCountByBagType);
            await UpsertRepository.UpsertItemAsync(dashboard.Id, dashboard);
            return new OkResult();
        }
    }
}
