using System.Drawing.Imaging;
using System.Linq;

namespace TheCollection.Web.Extensions
{
    public static class ImageFormatExtensions
    {
        // http://referencesource.microsoft.com/#System.Drawing/commonui/System/Drawing/Advanced/ImageFormat.cs,96dae44da4d0a9a8,references
        public static string GetMimeType(this ImageFormat imageFormat)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            var imageCodec = codecs.FirstOrDefault(codec => codec.FormatID == imageFormat.Guid);
            return imageCodec?.MimeType;
        }
    }
}
