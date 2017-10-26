using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using TheCollection.Business.Tea;
using TheCollection.Data.DocumentDB;
using TheCollection.Web.Constants;
using TheCollection.Web.Extensions;
using TheCollection.Web.Models;
using TheCollection.Web.Translators.Tea;

namespace TheCollection.Web.Commands {

    public class CreateBagCommand : IAsyncCommand<Bag> {

        public CreateBagCommand(IDocumentClient documentDbClient, ApplicationUser applicationUser) {
            DocumentDbClient = documentDbClient;
            ApplicationUser = applicationUser;
            BagTranslator = new BagToBagTranslator(applicationUser);
        }

        public IDocumentClient DocumentDbClient { get; }
        public ApplicationUser ApplicationUser { get; }
        public BagToBagTranslator BagTranslator { get; }

        public async Task<IActionResult> ExecuteAsync(Bag bag) {
            if (bag == null) {
                return new BadRequestObjectResult("Bag cannot be null");
            }

            var bagRepository = new CreateRepository<Bag>(DocumentDbClient, DocumentDB.DatabaseId, DocumentDB.BagsCollectionId);
            bag.UserId = ApplicationUser.Id;
            bag.Id = await bagRepository.CreateItemAsync(bag);
            return new OkObjectResult(BagTranslator.Translate(bag));
        }
    }
}
