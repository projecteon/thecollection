using System.Drawing;

namespace TheCollection.Web.Extensions
{
    public static class ImageExtensions
    {
        public static string GetMimeType(this Image image)
        {
            return image.RawFormat.GetMimeType();
        }
    }
}
