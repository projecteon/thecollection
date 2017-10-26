﻿namespace TheCollection.Web.Handlers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Azure.Documents;
    using System;
    using System.Drawing;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using TheCollection.Lib.Converters;
    using TheCollection.Lib.Extensions;
    using TheCollection.Web.Constants;
    using TheCollection.Web.Services;

    public class ImageHandler
    {
        public const string RegEx = @"[/]images[/]([0-9A-Fa-f]{8}[-]([0-9A-Fa-f]{4}[-]){3}[0-9A-Fa-f]{12})[/](\S+.(jpg|png))$";

        public ImageHandler(RequestDelegate next)
        {
            // This is an HTTP Handler, so no need to store next
        }

        public async Task Invoke(HttpContext context, IDocumentClient documentDbClient, IImageService imageService)
        {
            var imagesRepository = new GetRepository<Business.Tea.Image>(documentDbClient, DocumentDB.DatabaseId, DocumentDB.ImagesCollectionId);
            var matches = Regex.Matches(context.Request.Path, RegEx);
            if (matches.Count > 0 && matches[0].Groups.Count > 1)
            {
                var image = await imagesRepository.GetItemAsync(matches[0].Groups[1].Value);
                var bitmap = await imageService.Get(image.Filename);
                var response = GenerateResponse(bitmap, image.Filename);

                context.Response.ContentType = bitmap.GetMimeType("image/png");
                await context.Response.Body.WriteAsync(response, 0, response.Length);
            }
        }

        private byte[] GenerateResponse(Bitmap image, string fileName)
        {
            return ConverterFactory(fileName).GetBytes(image);
        }

        IImageConverter ConverterFactory(string fileName)
        {
            if (fileName.EndsWith("png"))
                return new PngImageConverter();


            if (fileName.EndsWith("jpg"))
                return new JpgImageConverter();

            throw new NotImplementedException();
        }
    }
}
