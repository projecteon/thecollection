namespace TheCollection.Web.Commands.Tea {
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Documents;
    using TheCollection.Data.DocumentDB;
    using TheCollection.Domain.Extensions;
    using TheCollection.Domain.Tea;
    using TheCollection.Web.Constants;
    using TheCollection.Web.Contracts;
    using TheCollection.Web.Extensions;
    using TheCollection.Web.Translators;
    using TheCollection.Web.Translators.Tea;

    public class CreateBagCommand : IAsyncCommand<Models.Tea.Bag> {
        public CreateBagCommand(IDocumentClient documentDbClient, IWebUser applicationUser) {
            DocumentDbClient = documentDbClient;
            ApplicationUser = applicationUser;
            BagTranslator = new BagToBagTranslator(applicationUser);
            BagDtoTranslator = new BagDtoToBagTranslator(applicationUser);
        }

        IDocumentClient DocumentDbClient { get; }
        IWebUser ApplicationUser { get; }
        ITranslator<Bag, Models.Tea.Bag> BagTranslator { get; }
        ITranslator<Models.Tea.Bag, Bag> BagDtoTranslator { get; }

        public async Task<IActionResult> ExecuteAsync(Models.Tea.Bag bag) {
            if (ApplicationUser.Roles.None(x => x.Name == Roles.SystemAdministrator || x.Name == Roles.TeaManager)) {
                return new ForbidResult();
            }

            if (bag == null) {
                return new BadRequestObjectResult("Bag cannot be null");
            }

            var bagRepository = new CreateRepository<Bag>(DocumentDbClient, DocumentDB.DatabaseId, DocumentDB.Collections.Bags);
            var newBag = BagDtoTranslator.Translate(bag);
            newBag.OwnerId = ApplicationUser.Id;
            newBag.Id = await bagRepository.CreateItemAsync(newBag);
            return new OkObjectResult(BagTranslator.Translate(newBag));
        }
    }
}
