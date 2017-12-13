namespace TheCollection.Web.Commands.Tea {

    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Documents;
    using TheCollection.Domain.Extensions;
    using TheCollection.Domain.Tea;
    using TheCollection.Data.DocumentDB;
    using TheCollection.Web.Constants;
    using TheCollection.Web.Extensions;
    using TheCollection.Web.Models;
    using TheCollection.Web.Translators;
    using TheCollection.Web.Translators.Tea;

    public class SearchBagTypesCommand {

        public SearchBagTypesCommand(IDocumentClient documentDbClient, IApplicationUser applicationUser) {
            DocumentDbClient = documentDbClient;
            BagTypeTranslator = new BagTypeToBagTypeTranslator(applicationUser);
        }

        IDocumentClient DocumentDbClient { get; }
        ITranslator<BagType, Models.Tea.BagType> BagTypeTranslator { get; }

        public async Task<IActionResult> ExecuteAsync(Search search) {
            if (search.searchterm.IsNullOrWhiteSpace()) {
                return new BadRequestResult();
            }

            var bagTypesRepository = new SearchRepository<BagType>(DocumentDbClient, DocumentDB.DatabaseId, DocumentDB.BagTypesCollectionId);
            var bagTypes = await bagTypesRepository.SearchAsync(search.searchterm);
            var sortedbagtypes = bagTypes.OrderBy(bagType => bagType.Name);

            return new OkObjectResult(BagTypeTranslator.Translate(sortedbagtypes));
        }
    }
}
