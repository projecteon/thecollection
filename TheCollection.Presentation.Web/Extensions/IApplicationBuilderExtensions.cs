namespace TheCollection.Presentation.Web.Extensions {
    using System.Text.RegularExpressions;
    using Microsoft.AspNetCore.Builder;
    using TheCollection.Presentation.Web.Handlers;

    public static class IApplicationBuilderExtensions {
        public static IApplicationBuilder UseThumbnailHandler(this IApplicationBuilder app) {
            app.MapWhen(
                context => Regex.IsMatch(context.Request.Path.ToString(), ThumbnailHandler.RegEx),
                appBranch => { appBranch.UseMiddleware<ThumbnailHandler>(); }
            );

            return app;
        }

        public static IApplicationBuilder UseImageHandler(this IApplicationBuilder app) {
            app.MapWhen(
                context => Regex.IsMatch(context.Request.Path.ToString(), ImageHandler.RegEx),
                appBranch => { appBranch.UseMiddleware<ImageHandler>(); }
            );

            return app;
        }

        public static IApplicationBuilder ApplySwagger(this IApplicationBuilder app) {
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TheCollection API V1");
            });

            return app;
        }
    }
}
