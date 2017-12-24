namespace TheCollection.Web.Controllers {

    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Documents;
    using TheCollection.Domain.Tea;
    using TheCollection.Data.DocumentDB;
    using TheCollection.Web.Constants;
    using TheCollection.Domain.Contracts.Repository;

    [Route("api/FileUploads")]
    public class FileUploadsController : Controller {
        private readonly IDocumentClient documentDbClient;
        private readonly IImageRepository imageRepository;

        public FileUploadsController(IDocumentClient documentDbClient, IImageRepository imageRepository) {
            this.documentDbClient = documentDbClient;
            this.imageRepository = imageRepository;
        }

        [Route("")]
        public async Task<IActionResult> Post() {
            try {
                var form = await Request.ReadFormAsync();
                var file = form.Files.First();

                //do something with your file => file.OpenReadStream()
                var bagsRepository = new SearchRepository<Bag>(documentDbClient, DocumentDB.DatabaseId, DocumentDB.Collections.Bags);
                var bagsCount = bagsRepository.SearchRowCountAsync("");
                var fileExtension = System.IO.Path.GetExtension(file.FileName);
                var uri = await imageRepository.Upload(file.OpenReadStream(), $"{bagsCount}.{fileExtension}");

                var imagesRepository = new CreateRepository<Image>(documentDbClient, DocumentDB.DatabaseId, DocumentDB.Collections.Images);
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
