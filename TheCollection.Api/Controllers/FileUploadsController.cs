namespace TheCollection.Api.Controllers {
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TheCollection.Application.Services.Commands;
    using TheCollection.Domain.Core.Contracts;
    
    [Produces("application/json")]
    [Route("api/FileUploads")]
    public class FileUploadsController : Controller {
        public FileUploadsController(IAsyncCommandHandler<UploadTeabagImageCommand> uploadFileCommand,
                IUrlHelper urlHelper,
                ITranslator<ICommandResult, IActionResult> translator) {
            UploadFileCommand = uploadFileCommand ?? throw new ArgumentNullException(nameof(uploadFileCommand));
            Translator = translator ?? throw new ArgumentNullException(nameof(translator));
        }

        IAsyncCommandHandler<UploadTeabagImageCommand> UploadFileCommand { get; }
        ITranslator<ICommandResult, IActionResult> Translator { get; }

        [HttpPost()]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post() {
            var form = await Request.ReadFormAsync();
            var file = form.Files.First();
            var result = await UploadFileCommand.ExecuteAsync(new UploadTeabagImageCommand(file.OpenReadStream(), file.FileName));
            return Translator.Translate(result);
        }
    }
}
