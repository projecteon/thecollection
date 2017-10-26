using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using TheCollection.Business.Tea;
using TheCollection.Web.Constants;
using TheCollection.Web.Services;

namespace TheCollection.Web.Controllers {

    [Route("api/FileUploads")]
    public class FileUploadsController : Controller {
        private readonly IDocumentClient documentDbClient;
        private readonly IImageService imageService;

        public FileUploadsController(IDocumentClient documentDbClient, IImageService imageService) {
            this.documentDbClient = documentDbClient;
            this.imageService = imageService;
        }

        [Route("")]
        public async Task<IActionResult> Post() {
            try {
                var form = await Request.ReadFormAsync();
                var file = form.Files.First();

                //do something with your file => file.OpenReadStream()
                var bagsRepository = new SearchRepository<Bag>(documentDbClient, DocumentDB.DatabaseId, DocumentDB.BagsCollectionId);
                var bagsCount = bagsRepository.SearchRowCountAsync("");
                var fileExtension = System.IO.Path.GetExtension(file.FileName);
                var uri = await imageService.Upload(file.OpenReadStream(), $"{bagsCount}.{fileExtension}");

                var imagesRepository = new CreateRepository<Image>(documentDbClient, DocumentDB.DatabaseId, DocumentDB.ImagesCollectionId);
                var newImage = new Image { Filename = file.FileName, Uri = uri };
                newImage.Id = await imagesRepository.CreateItemAsync(newImage);

                return Ok(newImage);
            }
            catch (Exception ex) {
                var originalMessage = ex.Message;

                while (ex.InnerException != null)
                    ex = ex.InnerException;
                return BadRequest($"{originalMessage} | {ex.Message}");
            }
        }
    }
}
