using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using System.Linq;
using System.Threading.Tasks;
using TheCollection.Business.Tea;
using TheCollection.Web.Constants;
using TheCollection.Web.Extensions;
using TheCollection.Web.Models;
using TheCollection.Web.Services;

namespace TheCollection.Web.Commands
{
    public class SearchBagTypesCommand
    {
        private readonly IDocumentClient documentDbClient;

        public SearchBagTypesCommand(IDocumentClient documentDbClient)
        {
            this.documentDbClient = documentDbClient;
        }

        public async Task<IActionResult> ExecuteAsync(Search search)
        {
            if (search.searchterm.IsNullOrWhiteSpace())
            {
                return new BadRequestResult();
            }

            var bagTypesRepository = new SearchRepository<BagType>(documentDbClient, DocumentDB.DatabaseId, DocumentDB.BagTypesCollectionId);
            var bagTypes = await bagTypesRepository.SearchAsync(search.searchterm);

            return new OkObjectResult(bagTypes.OrderBy(bagType => bagType.Name));
        }
    }
}
