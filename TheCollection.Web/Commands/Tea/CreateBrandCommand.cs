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

    public class CreateBrandCommand : IAsyncCommand<Models.Tea.Brand> {
        public CreateBrandCommand(IDocumentClient documentDbClient, IApplicationUser applicationUser) {
            DocumentDbClient = documentDbClient;
            BrandTranslator = new BrandToBrandTranslator(applicationUser);
            BrandDtoTranslator = new BrandDtoToBrandTranslator(applicationUser);
        }

        IDocumentClient DocumentDbClient { get; }
        IApplicationUser ApplicationUser { get; }
        ITranslator<Brand, Models.Tea.Brand> BrandTranslator { get; }
        ITranslator<Models.Tea.Brand, Brand> BrandDtoTranslator { get; }

        public async Task<IActionResult> ExecuteAsync(Models.Tea.Brand brand) {
            if (brand == null) {
                return new BadRequestObjectResult("Brand cannot be null");
            }

            var brandRepository = new CreateRepository<Brand>(DocumentDbClient, DocumentDB.DatabaseId, DocumentDB.Collections.Brands);
            var newBrand = BrandDtoTranslator.Translate(brand);
            newBrand.Id = await brandRepository.CreateItemAsync(newBrand);
            return new OkObjectResult(BrandTranslator.Translate(newBrand));
        }
    }
}
