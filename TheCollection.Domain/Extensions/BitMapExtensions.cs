namespace TheCollection.Domain.Extensions {
    using System.Drawing;
    using TheCollection.Domain.Contracts;

    public static class BitMapExtensions {
        public const int THUMB_DEFAULT_WIDTH_PARAM = 200;
        public const int THUMB_DEFAULT_HEIGHT_PARAM = 0;

        public static byte[] CreateThumbnail(this Bitmap src, IImageConverter imageConverter) {
            return imageConverter.GetBytesScaled(src, THUMB_DEFAULT_WIDTH_PARAM, 0);
        }
    }
}
