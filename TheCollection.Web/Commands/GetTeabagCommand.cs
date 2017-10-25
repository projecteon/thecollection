namespace TheCollection.Web.Commands
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Documents;
    using TheCollection.Business.Tea;
    using TheCollection.Web.Services;
    using TheCollection.Web.Constants;
    using TheCollection.Web.Translators.Tea;
    using TheCollection.Web.Translators;
    using TheCollection.Web.Extensions;
    using TheCollection.Web.Models;

    public class GetTeabagCommand : IAsyncCommand<string>
    {
        public GetTeabagCommand(IDocumentClient documentDbClient, ApplicationUser applicationUser)
        {
            DocumentDbClient = documentDbClient;
            BagTranslator = new BagToBagTranslator(applicationUser);
        }

        public IDocumentClient DocumentDbClient { get; }
        public ITranslator<Bag, Models.Tea.Bag> BagTranslator { get; }

        public async Task<IActionResult> ExecuteAsync(string id)
        {
            var bagsRepository = new GetRepository<Bag>(DocumentDbClient, DocumentDB.DatabaseId, DocumentDB.BagsCollectionId);
            var teabag = await bagsRepository.GetItemAsync(id);
            if (teabag == null)
            {
                return new NotFoundResult();
            }

            var teabagViewModel = BagTranslator.Translate(teabag);
            return new OkObjectResult(teabagViewModel);
        }
    }
}
