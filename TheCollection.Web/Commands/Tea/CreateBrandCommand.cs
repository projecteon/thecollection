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

    public class CreateBrandCommand : IAsyncCommand<Brand> {

        public CreateBrandCommand(IDocumentClient documentDbClient, IApplicationUser applicationUser) {
            DocumentDbClient = documentDbClient;
            BrandTranslator = new BrandToBrandTranslator(applicationUser);
        }

        IDocumentClient DocumentDbClient { get; }
        IApplicationUser ApplicationUser { get; }
        ITranslator<Brand, Models.Tea.Brand> BrandTranslator { get; }

        public async Task<IActionResult> ExecuteAsync(Brand brand) {
            if (brand == null) {
                return new BadRequestObjectResult("Brand cannot be null");
            }

            var brandRepository = new CreateRepository<Brand>(DocumentDbClient, DocumentDB.DatabaseId, DocumentDB.BrandsCollectionId);
            brand.Id = await brandRepository.CreateItemAsync(brand);
            return new OkObjectResult(BrandTranslator.Translate(brand));
        }
    }
}
