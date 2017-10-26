namespace TheCollection.Web.Commands
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Documents;
    using TheCollection.Business;
    using TheCollection.Web.Models;
    using TheCollection.Web.Translators;
    using TheCollection.Web.Constants;
    using TheCollection.Web.Services;
    using TheCollection.Web.Extensions;
    using TheCollection.Web.Translators.Tea;
    using TheCollection.Lib.Extensions;

    public class SearchRefValuesCommand<T> where T: class, IRef
    {
        public SearchRefValuesCommand(IDocumentClient documentDbClient, ApplicationUser applicationUser, string refValueCollectionId)
        {
            DocumentDbClient = documentDbClient;
            RefValueCollectionId = refValueCollectionId;
            RefValueTranslator = new IRefToRefValueTranslator();
        }

        public IDocumentClient DocumentDbClient { get; }
        public string RefValueCollectionId { get; }
        public ITranslator<T, Models.RefValue> RefValueTranslator { get; }

        public async Task<IActionResult> ExecuteAsync(Search search)
        {
            if (search.searchterm.IsNullOrWhiteSpace())
            {
                return new BadRequestResult();
            }

            var refValueRepository = new SearchRepository<T>(DocumentDbClient, DocumentDB.DatabaseId, RefValueCollectionId);
            var refValues = await refValueRepository.SearchAsync(search.searchterm);
            if (refValues == null)
            {
                return new NotFoundResult();
            }

            var refValueViewModels = refValues.Select(x => RefValueTranslator.Translate(x));
            return new OkObjectResult(refValueViewModels.OrderBy(x => x.name));
        }
    }
}
