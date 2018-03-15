namespace TheCollection.Api {
    using System;
    using Microsoft.AspNetCore.Mvc;
    using TheCollection.Domain.Core.Contracts;

    public class IQueryResultToIActionResultTranslator : ITranslator<IQueryResult, IActionResult> {
        public IActionResult Translate(IQueryResult source) {
            switch (source) {
                case Application.Services.Queries.ErrorResult e:
                    return new BadRequestObjectResult(e.Message);
                case Application.Services.Queries.OkResult o:
                    return new OkObjectResult(o.Value);
                case null:
                    throw new ArgumentNullException(nameof(source));
                default:
                    throw new ArgumentException(nameof(source));
            }
        }
    }
}
