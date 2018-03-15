namespace TheCollection.Application.Services.Commands.Tea {
    using System.Linq;
    using System.Threading.Tasks;
    using TheCollection.Application.Services.Contracts;
    using TheCollection.Domain.Core.Contracts;
    using TheCollection.Domain.Core.Contracts.Repository;
    using TheCollection.Domain.Tea;

    public class UpdateBagCommand : ICommand {
        public UpdateBagCommand(string id, ViewModels.Tea.Bag data) {
            Id = id;
            Data = data;
        }

        public string Id { get; }
        public ViewModels.Tea.Bag Data { get; }
    }

    public class UpdateBagCommandHandler : IAsyncCommandHandler<UpdateBagCommand> {
        public UpdateBagCommandHandler(IUpdateRepository<Bag> updateRepository,
                            IGetRepository<Bag> getRepository,
                            ILinqSearchRepository<IActivity> activityRepository,
                            IActivityAuthorizer authorizer,
                            ITranslator<ViewModels.Tea.Bag, Bag, Bag> translator) {
            UpdateRepository = updateRepository ?? throw new System.ArgumentNullException(nameof(updateRepository));
            GetRepository = getRepository ?? throw new System.ArgumentNullException(nameof(getRepository));
            ActivityRepository = activityRepository ?? throw new System.ArgumentNullException(nameof(activityRepository));
            Authorizer = authorizer ?? throw new System.ArgumentNullException(nameof(authorizer));
            Translator = translator ?? throw new System.ArgumentNullException(nameof(translator));
        }

        IUpdateRepository<Bag> UpdateRepository { get; }
        IGetRepository<Bag> GetRepository { get; }
        ILinqSearchRepository<IActivity> ActivityRepository { get; }
        IActivityAuthorizer Authorizer { get; }
        ITranslator<ViewModels.Tea.Bag, Bag, Bag> Translator { get; }

        public async Task<ICommandResult> ExecuteAsync(UpdateBagCommand command) {
            var activity = await ActivityRepository.SearchItemsAsync(x => x.Name == $"{nameof(UpdateBagCommandHandler)}");
            if (await Authorizer.IsAuthorized(activity.FirstOrDefault())) {
                return new ForbidResult();
            }

            if (command == null) {
                return new ErrorResult("Update item cannot be null");
            }

            var previousEntity = await GetRepository.GetItemAsync(command.Id);
            var entity = Translator.Translate(command.Data, previousEntity);
            await UpdateRepository.UpdateItemAsync(command.Id, entity);
            return new OkResult();
        }
    }

    public class CreateBagCommand : ICommand {
        public CreateBagCommand(ViewModels.Tea.Bag data) {
            Data = data;
        }

        public ViewModels.Tea.Bag Data { get; }
    }

    public class CreateBagCommandHandler : IAsyncCommandHandler<CreateBagCommand> {
        public CreateBagCommandHandler(ICreateRepository<Bag> createRepository,
                             ILinqSearchRepository<IActivity> activityRepository,
                             IActivityAuthorizer authorizer,
                             IAsyncTranslator<ViewModels.Tea.Bag, Bag> translator) {
            CreateRepository = createRepository ?? throw new System.ArgumentNullException(nameof(createRepository));
            ActivityRepository = activityRepository ?? throw new System.ArgumentNullException(nameof(activityRepository));
            Authorizer = authorizer ?? throw new System.ArgumentNullException(nameof(authorizer));
            Translator = translator ?? throw new System.ArgumentNullException(nameof(translator));
        }

        ICreateRepository<Bag> CreateRepository { get; }
        ILinqSearchRepository<IActivity> ActivityRepository { get; }
        IActivityAuthorizer Authorizer { get; }
        IAsyncTranslator<ViewModels.Tea.Bag, Bag> Translator { get; }

        public async Task<ICommandResult> ExecuteAsync(CreateBagCommand command) {
            var activity = await ActivityRepository.SearchItemsAsync(x => x.Name == $"{typeof(CreateBagCommandHandler)}");
            if (await Authorizer.IsAuthorized(activity.FirstOrDefault())) {
                return new ForbidResult();
            }

            if (command == null) {
                return new ErrorResult("New item cannot be null");
            }

            var entity = await Translator.Translate(command.Data);
            var id = await CreateRepository.CreateItemAsync(entity);
            return new CreateResult(id);
        }
    }
}
