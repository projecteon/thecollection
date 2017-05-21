using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Drawing;
using System.Threading.Tasks;
using TheCollection.Lib;

namespace TheCollection.Web.Handlers
{
    public class ThumbnailHandler
    {
        // https://docs.microsoft.com/en-us/azure/storage/storage-use-emulator
        // https://blogs.msdn.microsoft.com/premier_developer/2017/03/14/building-a-simple-photo-album-using-azure-blob-storage-with-net-core/
        // http://stackoverflow.com/questions/41421280/serving-images-from-azure-blob-storage-in-dot-net-core

        public const string RegEx = @"/thumbnails/([0-9A-Fa-f]{8}[-]([0-9A-Fa-f]{4}[-]){3}[0-9A-Fa-f]{12})[/]$";

        public ThumbnailHandler(RequestDelegate next)
        {
            // This is an HTTP Handler, so no need to store next
        }

        public async Task Invoke(HttpContext context)
        {
            var response = GenerateResponse(context);

            context.Response.ContentType = GetContentType();
            await context.Response.Body.WriteAsync(response, 0, response.Length);
        }

        // ...

        private byte[] GenerateResponse(HttpContext context)
        {
            return Thumbnail.CreateThumbnail(new Bitmap(@"C:\development\core_testing\testspa\wwwroot\images\1.jpg"));
        }

        private string GetContentType()
        {
            return "image/png";
        }
    }

    public static class TeabagImageHandlerExtensions
    {
        public static IApplicationBuilder UseMyHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ThumbnailHandler>();
        }
    }
}
