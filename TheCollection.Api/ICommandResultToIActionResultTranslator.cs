namespace TheCollection.Api {
    using System;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.AspNetCore.Mvc.Routing;
    using TheCollection.Domain.Core.Contracts;

    public class ICommandResultToIActionResultTranslator : ITranslator<ICommandResult, IActionResult> {
        public ICommandResultToIActionResultTranslator(IUrlHelper urlHelper) {
            UrlHelper = urlHelper ?? throw new ArgumentNullException(nameof(urlHelper));
        }

        IUrlHelper UrlHelper { get; }

        public IActionResult Translate(ICommandResult iCommandResult) {
            switch (iCommandResult) {
                case Application.Services.Commands.ErrorResult e:
                    return new BadRequestObjectResult(e.Message);
                case Application.Services.Commands.OkResult o:
                    return new Microsoft.AspNetCore.Mvc.OkResult();
                case Application.Services.Commands.CreateResult c:
                    return new Microsoft.AspNetCore.Mvc.CreatedResult(UrlHelper.Link("", new { id = c.Id }), new { id = c.Id });
                case null:
                    throw new ArgumentNullException(nameof(iCommandResult));
                default:
                    throw new ArgumentException(nameof(iCommandResult));
            }
        }
    }
}
