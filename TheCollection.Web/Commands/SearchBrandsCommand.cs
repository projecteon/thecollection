namespace TheCollection.Web.Commands {

    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Documents;
    using TheCollection.Business.Tea;
    using TheCollection.Data.DocumentDB;
    using TheCollection.Lib.Extensions;
    using TheCollection.Web.Constants;
    using TheCollection.Web.Models;

    public class SearchBrandsCommand : IAsyncCommand<Search> {

        public SearchBrandsCommand(IDocumentClient documentDbClient) {
            DocumentDbClient = documentDbClient;
        }

        public IDocumentClient DocumentDbClient { get; }

        public async Task<IActionResult> ExecuteAsync(Search search) {
            if (search.searchterm.IsNullOrWhiteSpace()) {
                return new BadRequestResult();
            }

            var brandsRepository = new SearchRepository<Brand>(DocumentDbClient, DocumentDB.DatabaseId, DocumentDB.BrandsCollectionId);
            var brands = await brandsRepository.SearchAsync(search.searchterm);

            return new OkObjectResult(brands.OrderBy(brand => brand.Name));
        }
    }
}
