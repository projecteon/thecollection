using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using TheCollection.Business.Tea;
using TheCollection.Web.Services;
using TheCollection.Web.Constants;

namespace TheCollection.Web.Commands
{
    public class GetTeabagCommand : IAsyncCommand<string>
    {
        private readonly IDocumentClient documentDbClient;
            
        public GetTeabagCommand(IDocumentClient documentDbClient)
        {
            this.documentDbClient = documentDbClient;
        }

        public async Task<IActionResult> ExecuteAsync(string id)
        {
            var bagsRepository = new GetRepository<Bag>(documentDbClient, DocumentDB.DatabaseId, DocumentDB.BagsCollectionId);
            var teabag = await bagsRepository.GetItemAsync(id);
            if (teabag == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(teabag);
        }
    }
}
