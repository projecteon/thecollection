namespace TheCollection.Web.Commands.Tea {
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Documents;
    using TheCollection.Domain.Tea;
    using TheCollection.Data.DocumentDB;
    using TheCollection.Web.Constants;
    using TheCollection.Web.Extensions;
    using TheCollection.Web.Models;
    using TheCollection.Web.Translators;
    using TheCollection.Web.Translators.Tea;

    public class CreateBagTypeCommand : IAsyncCommand<BagType> {

        public CreateBagTypeCommand(IDocumentClient documentDbClient, IApplicationUser applicationUser) {
            DocumentDbClient = documentDbClient;
            ApplicationUser = applicationUser;
            BagTypeTranslator = new BagTypeToBagTypeTranslator(applicationUser);
        }

        IDocumentClient DocumentDbClient { get; }
        IApplicationUser ApplicationUser { get; }
        ITranslator<BagType, Models.Tea.BagType> BagTypeTranslator { get; }

        public async Task<IActionResult> ExecuteAsync(BagType bagtype) {
            if (bagtype == null) {
                return new BadRequestObjectResult("Brand cannot be null");
            }

            var bagRepository = new CreateRepository<BagType>(DocumentDbClient, DocumentDB.DatabaseId, DocumentDB.BagsCollectionId);
            bagtype.Id = await bagRepository.CreateItemAsync(bagtype);
            return new OkObjectResult(BagTypeTranslator.Translate(bagtype));
        }
    }
}
