using System.Drawing;

namespace TheCollection.Lib
{
    public class Thumbnail
    {
        public static byte[] CreateThumbnail(Bitmap src)
        {
            return Converters.ImageConverter.GetBytesScaledPNG(src, Converters.ImageConverter.THUMB_DEFAULT_WIDTH_PARAM, 0);
        }
    }
}
