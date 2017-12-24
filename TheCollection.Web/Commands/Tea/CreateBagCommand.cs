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

    public class CreateBagCommand : IAsyncCommand<Models.Tea.Bag> {
        public CreateBagCommand(IDocumentClient documentDbClient, IApplicationUser applicationUser) {
            DocumentDbClient = documentDbClient;
            ApplicationUser = applicationUser;
            BagTranslator = new BagToBagTranslator(applicationUser);
            BagDtoTranslator = new BagDtoToBagTranslator(applicationUser);
        }

        IDocumentClient DocumentDbClient { get; }
        IApplicationUser ApplicationUser { get; }
        ITranslator<Bag, Models.Tea.Bag> BagTranslator { get; }
        ITranslator<Models.Tea.Bag, Bag> BagDtoTranslator { get; }

        public async Task<IActionResult> ExecuteAsync(Models.Tea.Bag bag) {
            if (bag == null) {
                return new BadRequestObjectResult("Bag cannot be null");
            }

            var bagRepository = new CreateRepository<Bag>(DocumentDbClient, DocumentDB.DatabaseId, DocumentDB.Collections.Bags);
            var newBag = BagDtoTranslator.Translate(bag);
            newBag.UserId = ApplicationUser.Id;
            newBag.Id = await bagRepository.CreateItemAsync(newBag);
            return new OkObjectResult(BagTranslator.Translate(newBag));
        }
    }
}
