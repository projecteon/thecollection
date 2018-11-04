namespace TheCollection.Application.Services.Translators.Tea {
    using System;
    using System.Threading.Tasks;
    using NodaTime;
    using TheCollection.Application.Services.Contracts;
    using TheCollection.Domain.Core.Contracts;
    using TheCollection.Domain.Core.Contracts.Repository;

    public class BagViewModelToUpdateBagTranslator : ITranslator<ViewModels.Tea.Bag, Domain.Tea.Bag, Domain.Tea.Bag> {
        public BagViewModelToUpdateBagTranslator(ITranslator<ViewModels.RefValue, Domain.RefValue> refValueTranslator) {
            RefValueTranslator = refValueTranslator;
        }

        ITranslator<ViewModels.RefValue, Domain.RefValue> RefValueTranslator { get; }

        Domain.Tea.Bag ITranslator<ViewModels.Tea.Bag, Domain.Tea.Bag, Domain.Tea.Bag>.Translate(ViewModels.Tea.Bag source, Domain.Tea.Bag destinationSource) {
            return new Domain.Tea.Bag(destinationSource.Id,
                destinationSource.OwnerId,
                destinationSource.MainID,
                RefValueTranslator.Translate(source.Brand),
                source.Serie,
                source.Flavour,
                source.Hallmark,
                RefValueTranslator.Translate(source.BagType),
                RefValueTranslator.Translate(source.Country),
                source.SerialNumber,
                destinationSource.InsertDate,
                source.ImageId);
        }
    }

    public class BagViewModelToCreateBagTranslator : IAsyncTranslator<ViewModels.Tea.Bag, Domain.Tea.Bag> {
        public BagViewModelToCreateBagTranslator(ITranslator<ViewModels.RefValue, Domain.RefValue> refValueTranslator,
                ISearchRepository<Domain.Tea.Bag> bagRepository,
                IGetRepository<IApplicationUser> userRepository,
                IClock clock) {
            RefValueTranslator = refValueTranslator ?? throw new System.ArgumentNullException(nameof(refValueTranslator));
            BagRepository = bagRepository ?? throw new System.ArgumentNullException(nameof(bagRepository));
            UserRepository = userRepository ?? throw new System.ArgumentNullException(nameof(userRepository));
            Clock = clock ?? throw new System.ArgumentNullException(nameof(clock));
        }

        ITranslator<ViewModels.RefValue, Domain.RefValue> RefValueTranslator { get; }
        ISearchRepository<Domain.Tea.Bag> BagRepository { get; }
        IGetRepository<IApplicationUser> UserRepository { get; }
        IClock Clock { get; }

        public async Task<Domain.Tea.Bag> Translate(ViewModels.Tea.Bag source) {
            var applicationUser = await UserRepository.GetItemAsync();
            var bagsCount = await BagRepository.SearchRowCountAsync($"{nameof(Domain.Tea.Bag.OwnerId).ToLower()} = {applicationUser.Id}");
            var localNow = Clock.GetCurrentInstant().InUtc().LocalDateTime.Date;
            return new Domain.Tea.Bag(null,
                applicationUser.Id,
                Convert.ToInt32(bagsCount),
                RefValueTranslator.Translate(source.Brand),
                source.Serie,
                source.Flavour,
                source.Hallmark,
                RefValueTranslator.Translate(source.BagType),
                RefValueTranslator.Translate(source.Country),
                source.SerialNumber,
                localNow,
                source.ImageId);
        }
    }
}
