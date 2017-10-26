namespace TheCollection.Web.Commands
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using TheCollection.Web.Models;
    using Microsoft.Azure.Documents;
    using TheCollection.Web.Constants;
    using TheCollection.Business.Tea;
    using TheCollection.Web.Services;
    using TheCollection.Lib.Extensions;

    public class SearchBrandsCommand : IAsyncCommand<Search>
    {
        private readonly IDocumentClient documentDbClient;

        public SearchBrandsCommand(IDocumentClient documentDbClient)
        {
            this.documentDbClient = documentDbClient;
        }

        public async Task<IActionResult> ExecuteAsync(Search search)
        {
            if (search.searchterm.IsNullOrWhiteSpace())
            {
                return new BadRequestResult();
            }

            var brandsRepository = new SearchRepository<Brand>(documentDbClient, DocumentDB.DatabaseId, DocumentDB.BrandsCollectionId);
            var brands = await brandsRepository.SearchAsync(search.searchterm);

            return new OkObjectResult(brands.OrderBy(brand => brand.Name));
        }
    }
}
