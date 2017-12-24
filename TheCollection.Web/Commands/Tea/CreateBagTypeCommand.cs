namespace TheCollection.Web.Commands.Tea {
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Documents;
    using TheCollection.Domain.Tea;
    using TheCollection.Data.DocumentDB;
    using TheCollection.Web.Constants;
    using TheCollection.Web.Extensions;
    using TheCollection.Web.Translators;
    using TheCollection.Web.Translators.Tea;
    using TheCollection.Web.Contracts;

    public class CreateBagTypeCommand : IAsyncCommand<Models.Tea.BagType> {

        public CreateBagTypeCommand(IDocumentClient documentDbClient, IApplicationUser applicationUser) {
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

            var bagRepository = new CreateRepository<BagType>(DocumentDbClient, DocumentDB.DatabaseId, DocumentDB.Collections.BagTypes);
            var newBagType = BagTypeDtoTranslator.Translate(bagtype);
            newBagType.Id = await bagRepository.CreateItemAsync(newBagType);
            return new OkObjectResult(BagTypeTranslator.Translate(newBagType));
        }
    }
}
