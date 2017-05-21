using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TheCollection.Lib;

namespace TheCollection.Web.Handlers
{
    public class TeabagImageHandler
    {
        public TeabagImageHandler(RequestDelegate next)
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
            return builder.UseMiddleware<TeabagImageHandler>();
        }
    }
}
