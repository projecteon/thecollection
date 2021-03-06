namespace TheCollection.Presentation.Web.Handlers {
    using System;
    using System.Drawing;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Azure.Documents;
    using TheCollection.Data.DocumentDB.Repositories;
    using TheCollection.Domain.Converters;
    using TheCollection.Domain.Core.Contracts;
    using TheCollection.Domain.Core.Contracts.Repository;
    using TheCollection.Domain.Extensions;
    using TheCollection.Presentation.Web.Constants;

    public class ThumbnailHandler {
        // https://docs.microsoft.com/en-us/azure/storage/storage-use-emulator
        // https://blogs.msdn.microsoft.com/premier_developer/2017/03/14/building-a-simple-photo-album-using-azure-blob-storage-with-net-core/
        // http://stackoverflow.com/questions/41421280/serving-images-from-azure-blob-storage-in-dot-net-core

        public const string RegEx = @"[/]thumbnails[/]([0-9A-Fa-f]{8}[-]([0-9A-Fa-f]{4}[-]){3}[0-9A-Fa-f]{12})[/](\S+.(jpg|png))$";

        public ThumbnailHandler(RequestDelegate next) {
            // This is an HTTP Handler, so no need to store next
        }

        public async Task Invoke(HttpContext context, IDocumentClient documentDbClient, IImageRepository imageRepository) {
            var imagesRepository = new GetRepository<Domain.Tea.Image>(documentDbClient, DocumentDBConstants.DatabaseId, DocumentDBConstants.Collections.Images);
            var matches = Regex.Matches(context.Request.Path, RegEx);
            if (matches.Count > 0 && matches[0].Groups.Count > 1) {
                var image = await imagesRepository.GetItemAsync(matches[0].Groups[1].Value);
                var bitmap = await imageRepository.Get(image.Filename);
                var response = GenerateResponse(bitmap, image.Filename);

                context.Response.ContentType = bitmap.GetMimeType("image/png");
                await context.Response.Body.WriteAsync(response, 0, response.Length);
            }
        }

        private byte[] GenerateResponse(Bitmap image, string fileName) {
            return image.CreateThumbnail(ConverterFactory(fileName));
        }

        private IImageConverter ConverterFactory(string fileName) {
            if (fileName.EndsWith("png"))
                return new PngImageConverter();

            if (fileName.EndsWith("jpg"))
                return new JpgImageConverter();

            throw new NotImplementedException();
        }
    }
}
