namespace TheCollection.Presentation.Web.Extensions {
    using Microsoft.AspNetCore.Builder;
    using TheCollection.Presentation.Web.Handlers;

    public static class IApplicationBuilderExtensions {
        public static IApplicationBuilder UseThumbnailHandler(this Microsoft.AspNetCore.Builder.IApplicationBuilder builder) {
            return builder.UseMiddleware<ThumbnailHandler>();
        }

        public static IApplicationBuilder UseImageHandler(this Microsoft.AspNetCore.Builder.IApplicationBuilder builder) {
            return builder.UseMiddleware<ImageHandler>();
        }
    }
}
