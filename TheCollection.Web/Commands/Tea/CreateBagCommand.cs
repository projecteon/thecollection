namespace TheCollection.Web.Commands.Tea {

    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Documents;
    using TheCollection.Domain.Tea;
    using TheCollection.Data.DocumentDB;
    using TheCollection.Web.Constants;
    using TheCollection.Web.Extensions;
    using TheCollection.Web.Translators;
    using TheCollection.Web.Translators.Tea;
    using TheCollection.Web.Contracts;

    public class CreateBagCommand : IAsyncCommand<Bag> {

        public CreateBagCommand(IDocumentClient documentDbClient, IApplicationUser applicationUser) {
            DocumentDbClient = documentDbClient;
            ApplicationUser = applicationUser;
            BagTranslator = new BagToBagTranslator(applicationUser);
        }

        IDocumentClient DocumentDbClient { get; }
        IApplicationUser ApplicationUser { get; }
        ITranslator<Bag, Models.Tea.Bag> BagTranslator { get; }

        public async Task<IActionResult> ExecuteAsync(Bag bag) {
            if (bag == null) {
                return new BadRequestObjectResult("Bag cannot be null");
            }

            var bagRepository = new CreateRepository<Bag>(DocumentDbClient, DocumentDB.DatabaseId, DocumentDB.Collections.Bags);
            bag.UserId = ApplicationUser.Id;
            bag.Id = await bagRepository.CreateItemAsync(bag);
            return new OkObjectResult(BagTranslator.Translate(bag));
        }
    }
}
