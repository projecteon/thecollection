namespace TheCollection.Lib.Converters {

    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using TheCollection.Lib.Extensions;

    public class PngImageConverter : IImageConverter {

        public Stream GetStream(Image pngImage) {
            var memoryStream = new System.IO.MemoryStream();
            pngImage.Save(memoryStream, GetPngEncoder, GetPngEncoderParams);
            return memoryStream;
        }

        public byte[] GetBytes(Image imgSrc) {
            return imgSrc.GetBytes(GetPngEncoder, GetPngEncoderParams);
        }

        public byte[] GetBytesScaled(Image imgSrc, int iWidth, int iHeight) {
            return GetBytesScaled(imgSrc, iWidth, iHeight, false, false);
        }

        public byte[] GetBytesScaled(Image imgSrc, int iWidth, int iHeight, bool bTransparent = false, bool bCenterAlign = false) {
            return BitmapConverter.GetBytesScaledBitmap(imgSrc, iWidth, iHeight, bTransparent, bCenterAlign).GetBytes(GetPngEncoder, GetPngEncoderParams);
        }

        private static ImageCodecInfo GetPngEncoder {
            get {
                ImageCodecInfo[] infos = ImageCodecInfo.GetImageEncoders();

                for (int i = 0; i < infos.Length; i++) {
                    if (ImageFormat.Png.Guid.Equals(infos[i].FormatID))
                        return infos[i];
                }

                return null;
            }
        }

        private static EncoderParameters GetPngEncoderParams {
            get {
                EncoderParameters encParams = new EncoderParameters(1);
                encParams.Param[0] = new EncoderParameter(Encoder.ColorDepth, 8L);
                return encParams;
            }
        }
    }
}
