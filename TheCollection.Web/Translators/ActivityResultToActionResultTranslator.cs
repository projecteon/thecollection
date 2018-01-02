namespace TheCollection.Web.Translators {
    using System;
    using Microsoft.AspNetCore.Mvc;
    using TheCollection.Application.Services.Contracts;

    public class ActivityResultToActionResultTranslator<T> {
        public IActionResult Translate(IActivityResult source) {
            switch (source) {
                case Application.Services.ErrorResult e:
                    return new BadRequestObjectResult(e.Error);
                case Application.Services.NotFoundResult n:
                    return new NotFoundResult();
                case Application.Services.OkObjectResult<T> o:
                    return new OkObjectResult(o.Value);
                case null:
                    throw new ArgumentNullException(nameof(source));
                default:
                    throw new ArgumentException(nameof(source));
            }
        }
    }
}
