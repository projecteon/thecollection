using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Documents.Client;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TheCollection.Web.Extensions;
using TheCollection.Web.Services;

namespace TheCollection.Web.Handlers
{
    public class ThumbnailHandler
    {
        // https://docs.microsoft.com/en-us/azure/storage/storage-use-emulator
        // https://blogs.msdn.microsoft.com/premier_developer/2017/03/14/building-a-simple-photo-album-using-azure-blob-storage-with-net-core/
        // http://stackoverflow.com/questions/41421280/serving-images-from-azure-blob-storage-in-dot-net-core

        public const string RegEx = @"[/]thumbnails[/]([0-9A-Fa-f]{8}[-]([0-9A-Fa-f]{4}[-]){3}[0-9A-Fa-f]{12})[/](\S+.(jpg|png))$";

        public ThumbnailHandler(RequestDelegate next)
        {
            // This is an HTTP Handler, so no need to store next
        }

        public async Task Invoke(HttpContext context, DocumentClient documentDbClient, IImageService imageService)
        {
            var imagesRepository = new DocumentDBRepository<Models.Image>(documentDbClient, "TheCollection", "Images");
            var matches = Regex.Matches(context.Request.Path, RegEx);
            if (matches.Count > 0 && matches[0].Groups.Count > 1)
            {
                var image = await imagesRepository.GetItemAsync(matches[0].Groups[1].Value);
                var bitmap = await imageService.Get(image.Filename);
                var response = GenerateResponse(bitmap);

                context.Response.ContentType = GetContentType(bitmap);
                await context.Response.Body.WriteAsync(response, 0, response.Length);
            }
        }

        private byte[] GenerateResponse(Bitmap image)
        {
            return Thumbnail.CreateThumbnail(image);
        }

        private string GetContentType(Bitmap image)
        {
            return image.GetMimeType() ?? "image/png";
        }
    }

    public static class ThumbnailHandlerExtensions
    {
        public static IApplicationBuilder UseThumbnailHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ThumbnailHandler>();
        }
    }
}
