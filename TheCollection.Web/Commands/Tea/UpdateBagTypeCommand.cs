namespace TheCollection.Web.Commands.Tea {
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Documents;
    using TheCollection.Data.DocumentDB;
    using TheCollection.Domain.Tea;
    using TheCollection.Web.Constants;
    using TheCollection.Web.Contracts;
    using TheCollection.Web.Extensions;
    using TheCollection.Web.Translators;
    using TheCollection.Web.Translators.Tea;

    public class UpdateBagTypeCommand : IAsyncCommand<Models.Tea.BagType> {
        public UpdateBagTypeCommand(IDocumentClient documentDbClient, IApplicationUser applicationUser) {
            DocumentDbClient = documentDbClient;
            ApplicationUser = applicationUser;
            BagTypeTranslator = new BagTypeToBagTypeTranslator(applicationUser);
            BagTypeDtoTranslator = new BagTypeDtoToBagTypeTranslator(applicationUser);
        }

        IDocumentClient DocumentDbClient { get; }
        IApplicationUser ApplicationUser { get; }
        ITranslator<BagType, Models.Tea.BagType> BagTypeTranslator { get; }
        ITranslator<Models.Tea.BagType, BagType> BagTypeDtoTranslator { get; }

        public async Task<IActionResult> ExecuteAsync(Models.Tea.BagType bagtype) {
            if (bagtype == null) {
                return new BadRequestObjectResult("BagType cannot be null");
            }

            var updateRepository = new UpdateRepository<BagType>(DocumentDbClient, DocumentDB.DatabaseId, DocumentDB.Collections.BagTypes);
            var updateBagType = BagTypeDtoTranslator.Translate(bagtype);
            updateBagType.Id = await updateRepository.UpdateItemAsync(bagtype.id, updateBagType);
            return new OkObjectResult(BagTypeTranslator.Translate(updateBagType));
        }
    }
}
