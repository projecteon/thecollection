namespace TheCollection.Web.Commands.Tea {
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Documents;
    using TheCollection.Application.Services;
    using TheCollection.Application.Services.Commands;
    using TheCollection.Data.DocumentDB;
    using TheCollection.Domain.Tea;
    using TheCollection.Web.Constants;
    using TheCollection.Web.Contracts;
    using TheCollection.Web.Models;
    using TheCollection.Web.Translators;

    public class SearchBagsCommand : IAsyncCommand<Search> {
        public SearchBagsCommand(IDocumentClient documentDbClient, IWebUser applicationUser) {
            DocumentDbClient = documentDbClient;
            ApplicationUser = applicationUser;
            ActivityTranslator = new ActivityResultToActionResultTranslator<SearchResult<Models.Tea.Bag>>();
        }

        IDocumentClient DocumentDbClient { get; }
        public IWebUser ApplicationUser { get; }
        public ActivityResultToActionResultTranslator<SearchResult<Models.Tea.Bag>> ActivityTranslator { get; }

        public async Task<IActionResult> ExecuteAsync(Search search) {
            var bagsRepository = new SearchRepository<Bag>(DocumentDbClient, DocumentDB.DatabaseId, DocumentDB.Collections.Bags);
            var command = new SearchOwnedCommand<Bag>(bagsRepository, new Activity(), new ActivityAuthorizer(ApplicationUser), OrderBy);
            var result = await command.ExecuteAsync(search);
            return ActivityTranslator.Translate(result);
        }

        static IOrderedEnumerable<Bag> OrderBy(IEnumerable<Bag> bags) {
            return bags.OrderBy(bag => bag.Brand.Name)
                       .ThenBy(bag => bag.Serie)
                       .ThenBy(bag => bag.Hallmark)
                       .ThenBy(bag => bag.BagType?.Name)
                       .ThenBy(bag => bag.Flavour);
        }
    }
}
