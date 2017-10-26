namespace TheCollection.Lib.Extensions {

    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;

    public static class ImageExtensions {

        public static byte[] GetBytes(this Image imgSrc, ImageCodecInfo enc, EncoderParameters encParams) {
            var abRet = new ArraySegment<byte>();
            using (var ms = new MemoryStream()) {
                imgSrc.Save(ms, enc, encParams);
                ms.TryGetBuffer(out abRet);
            }

            return abRet.Array;
        }

        public static string GetMimeType(this Image image, string defaultMimeType = "") {
            if (defaultMimeType.IsNullOrWhiteSpace()) {
                return image.RawFormat.GetMimeType();
            }

            return image.RawFormat.GetMimeType() ?? defaultMimeType;
        }
    }
}
