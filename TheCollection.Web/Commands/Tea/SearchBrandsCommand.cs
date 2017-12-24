namespace TheCollection.Web.Commands.Tea {
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Documents;
    using TheCollection.Data.DocumentDB;
    using TheCollection.Domain.Extensions;
    using TheCollection.Domain.Tea;
    using TheCollection.Web.Constants;
    using TheCollection.Web.Contracts;
    using TheCollection.Web.Extensions;
    using TheCollection.Web.Models;
    using TheCollection.Web.Translators;
    using TheCollection.Web.Translators.Tea;

    public class SearchBrandsCommand : IAsyncCommand<Search> {
        public SearchBrandsCommand(IDocumentClient documentDbClient, IApplicationUser applicationUser) {
            DocumentDbClient = documentDbClient;
            BrandTranslator = new BrandToBrandTranslator(applicationUser);
        }

        IDocumentClient DocumentDbClient { get; }
        ITranslator<Brand, Models.Tea.Brand> BrandTranslator { get; }

        public async Task<IActionResult> ExecuteAsync(Search search) {
            if (search.searchterm.IsNullOrWhiteSpace()) {
                return new BadRequestResult();
            }

            var brandsRepository = new SearchRepository<Brand>(DocumentDbClient, DocumentDB.DatabaseId, DocumentDB.Collections.Brands);
            var brands = await brandsRepository.SearchAsync(search.searchterm);
            var sortedbrands = brands.OrderBy(brand => brand.Name);

            return new OkObjectResult(BrandTranslator.Translate(sortedbrands));
        }
    }
}
