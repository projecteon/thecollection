namespace TheCollection.Web.Commands {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Documents;
    using TheCollection.Data.DocumentDB;
    using TheCollection.Domain.Contracts;
    using TheCollection.Web.Constants;
    using TheCollection.Web.Contracts;
    using TheCollection.Web.Extensions;
    using TheCollection.Web.Models;
    using TheCollection.Web.Translators;
    using TheCollection.Web.Translators.Tea;

    public class SearchRefValuesCommand<T> where T : class, IRef {
        public SearchRefValuesCommand(IDocumentClient documentDbClient, IWebUser webUser, string refValueCollectionId) {
            DocumentDbClient = documentDbClient;
            WebUser = webUser;
            RefValueCollectionId = refValueCollectionId;
            RefValueTranslator = new IRefToRefValueTranslator();
        }

        IDocumentClient DocumentDbClient { get; }
        public IWebUser WebUser { get; }
        string RefValueCollectionId { get; }
        ITranslator<T, RefValue> RefValueTranslator { get; }

        public async Task<IActionResult> ExecuteAsync(Search search) {
            var refValueRepository = new SearchRepository<T>(DocumentDbClient, DocumentDB.DatabaseId, RefValueCollectionId);
            var applicationCommand = new Application.Services.Commands.SearchRefValuesCommand<T>(refValueRepository, WebUser);
            var applicationResult = await applicationCommand.ExecuteAsync(search);

            switch (applicationResult) {
                case Application.Services.ErrorResult e:
                    return new BadRequestObjectResult(e.Error);
                case Application.Services.NotFoundResult n:
                    return new NotFoundResult();
                case Application.Services.OkObjectResult<IEnumerable<T>> o:
                    var refValueViewModels = RefValueTranslator.Translate(o.Value);
                    return new OkObjectResult(refValueViewModels.OrderBy(x => x.name));
                case null:
                    throw new ArgumentNullException(nameof(applicationResult));
                default:
                    throw new ArgumentException(nameof(applicationResult));
            }

        }
    }
}
