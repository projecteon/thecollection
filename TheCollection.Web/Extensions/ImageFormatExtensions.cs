using System.Drawing.Imaging;
using System.Linq;

namespace TheCollection.Web.Extensions
{
    public static class ImageFormatExtensions
    {
        public static string GetMimeType(this ImageFormat imageFormat)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            return codecs.First(codec => codec.FormatID == imageFormat.Guid).MimeType;
        }
    }
}
