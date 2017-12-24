namespace TheCollection.Web.Commands.Tea {
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Documents;
    using TheCollection.Data.DocumentDB;
    using TheCollection.Domain.Contracts;
    using TheCollection.Domain.Extensions;
    using TheCollection.Web.Constants;
    using TheCollection.Web.Contracts;
    using TheCollection.Web.Extensions;
    using TheCollection.Web.Translators;

    public class UpdateCommand<TEntity, TDto> : IAsyncCommand<TDto> where TEntity : class, IEntity, new() where TDto : IDto, new() {
        public UpdateCommand(IDocumentClient documentDbClient,
                             IApplicationUser applicationUser,
                             ITranslator<TEntity, TDto> entityTranslator,
                             ITranslator<TDto, TEntity> dtoTranslator) {
            DocumentDbClient = documentDbClient;
            ApplicationUser = applicationUser;
            EntityTranslator = entityTranslator;
            DtoTranslator = dtoTranslator;
        }

        IDocumentClient DocumentDbClient { get; }
        IApplicationUser ApplicationUser { get; }
        ITranslator<TEntity, TDto> EntityTranslator { get; }
        ITranslator<TDto, TEntity> DtoTranslator { get; }

        public async Task<IActionResult> ExecuteAsync(TDto dto) {
            if (ApplicationUser.Roles.None(x => x.NormalizedName == "sysadmin" || x.NormalizedName == "TeaManager")) {
                return new ForbidResult();
            }

            if (dto == null) {
                return new BadRequestObjectResult("Update item cannot be null");
            }

            var entity = DtoTranslator.Translate(dto);
            if (DocumentDB.Collections.EntityToCollectionMap.TryGetValue(entity.GetType(), out var collectionId) == false) {
                return new BadRequestObjectResult("Entity is missing map to a collection store.");
            }

            var updateRepository = new UpdateRepository<TEntity>(DocumentDbClient, DocumentDB.DatabaseId, collectionId);
            entity.Id = await updateRepository.UpdateItemAsync(dto.id, entity);
            return new OkObjectResult(EntityTranslator.Translate(entity));
        }
    }
}
