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

    public class UpdateBagCommand : IAsyncCommand<Models.Tea.Bag> {
        public UpdateBagCommand(IDocumentClient documentDbClient, IApplicationUser applicationUser) {
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
            if (ApplicationUser.Roles.None(x => x.NormalizedName == "sysadmin" || x.NormalizedName == "TeaManager")) {
                return new ForbidResult();
            }

            if (bag == null) {
                return new BadRequestObjectResult("Bag cannot be null");
            }

            var updateRepository = new UpdateRepository<Bag>(DocumentDbClient, DocumentDB.DatabaseId, DocumentDB.Collections.Bags);
            var updateBag = BagDtoTranslator.Translate(bag);
            updateBag.Id = await updateRepository.UpdateItemAsync(bag.id, updateBag);
            return new OkObjectResult(BagTranslator.Translate(updateBag));
        }
    }
}
