namespace TheCollection.Lib {

    using System.Drawing;
    using TheCollection.Lib.Converters;

    public class Thumbnail {
        public const int THUMB_DEFAULT_WIDTH_PARAM = 200;
        public const int THUMB_DEFAULT_HEIGHT_PARAM = 0;

        public static byte[] CreateThumbnail(Bitmap src, IImageConverter imageConverter) {
            return imageConverter.GetBytesScaled(src, THUMB_DEFAULT_WIDTH_PARAM, 0);
        }
    }
}
