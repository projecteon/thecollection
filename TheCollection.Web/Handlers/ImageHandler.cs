using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Documents;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TheCollection.Web.Extensions;
using TheCollection.Web.Services;

namespace TheCollection.Web.Handlers
{
    public class ImageHandler
    {
        public const string RegEx = @"[/]images[/]([0-9A-Fa-f]{8}[-]([0-9A-Fa-f]{4}[-]){3}[0-9A-Fa-f]{12})[/](\S+.(jpg|png))$";

        public ImageHandler(RequestDelegate next)
        {
            // This is an HTTP Handler, so no need to store next
        }

        public async Task Invoke(HttpContext context, IDocumentClient documentDbClient, IImageService imageService)
        {
            var imagesRepository = new DocumentDBRepository<Business.Tea.Image>(documentDbClient, "TheCollection", "Images");
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
            return ImageConverter.GetBytesPNG(image);
        }

        private string GetContentType(Bitmap image)
        {
            return image.GetMimeType() ?? "image/png";
        }
    }

    public static class ImageHandlerExtensions
    {
        public static IApplicationBuilder UseImageHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ImageHandler>();
        }
    }
}
