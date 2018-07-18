namespace TheCollection.Application.Services.Commands.Tea {
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TheCollection.Domain;
    using TheCollection.Domain.Core.Contracts;
    using TheCollection.Domain.Core.Contracts.Repository;
    using TheCollection.Domain.Tea;

    public class CreateBagsCountByBrandsCommandHandler : IAsyncCommandHandler<CreateBagsCountByBrandsCommand> {
        public CreateBagsCountByBrandsCommandHandler(ILinqSearchRepository<Bag> searchRepository, IUpsertRepository<Dashboard<IEnumerable<CountBy<RefValue>>>> upsertRepository) {
            SearchRepository = searchRepository ?? throw new System.ArgumentNullException(nameof(searchRepository));
            UpsertRepository = upsertRepository ?? throw new System.ArgumentNullException(nameof(upsertRepository));
        }

        ILinqSearchRepository<Bag> SearchRepository { get; }
        IUpsertRepository<Dashboard<IEnumerable<CountBy<RefValue>>>> UpsertRepository { get; }

        public async Task<ICommandResult> ExecuteAsync(CreateBagsCountByBrandsCommand command) {
            var bags = await SearchRepository.SearchItemsAsync();
            var queryablebags = bags.AsQueryable();
            var strangeBug = queryablebags.Where(x => x.Brand == null);
            var countGroupByIRef = new CountGroupBy<Bag, RefValue, RefValueComparer>(queryablebags);
            var bagsCountByBrand = countGroupByIRef.GroupAndCountBy(x => x.Brand)
                                                   .OrderByDescending(x => x.Count)
                                                   .Take(30);
            var dashboard = new Dashboard<IEnumerable<CountBy<RefValue>>>(DashBoardTypes.BagsCountByBrands.Key.ToString(), command.User.Id, DashBoardTypes.BagsCountByBagTypes, bagsCountByBrand);
            await UpsertRepository.UpsertItemAsync(dashboard.Id, dashboard);
            return new OkResult();
        }
    }
}
