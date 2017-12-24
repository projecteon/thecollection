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

    public class UpdateBrandCommand : IAsyncCommand<Models.Tea.Brand> {
        public UpdateBrandCommand(IDocumentClient documentDbClient, IApplicationUser applicationUser) {
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

            var updateRepository = new UpdateRepository<Brand>(DocumentDbClient, DocumentDB.DatabaseId, DocumentDB.Collections.Brands);
            var updateBrand = BrandDtoTranslator.Translate(brand);
            updateBrand.Id = await updateRepository.UpdateItemAsync(brand.id, updateBrand);
            return new OkObjectResult(BrandTranslator.Translate(updateBrand));
        }
    }
}
