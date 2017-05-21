using System.Drawing;

namespace TheCollection.Lib
{
    public class Thumbnail
    {
        public static byte[] CreateThumbnail(Bitmap src)
        {
            return ImageConverter.GetBytesScaledPNG(src, ImageConverter.THUMB_DEFAULT_WIDTH_PARAM, 0);
        }
    }
}
