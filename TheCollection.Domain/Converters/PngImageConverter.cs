namespace TheCollection.Domain.Converters {
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using TheCollection.Domain.Core.Contracts;
    using TheCollection.Domain.Extensions;

    public class PngImageConverter : IImageConverter {
        public Stream GetStream(Image pngImage) {
            var memoryStream = new MemoryStream();
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
            return imgSrc.GetBytesScaledBitmap(iWidth, iHeight, bTransparent, bCenterAlign).GetBytes(GetPngEncoder, GetPngEncoderParams);
        }

        static ImageCodecInfo GetPngEncoder {
            get {
                var infos = ImageCodecInfo.GetImageEncoders();
                return infos.FirstOrDefault(info => ImageFormat.Png.Guid.Equals(info.FormatID));
            }
        }

        static EncoderParameters GetPngEncoderParams {
            get {
                var encParams = new EncoderParameters(1);
                encParams.Param[0] = new EncoderParameter(Encoder.ColorDepth, 8L);
                return encParams;
            }
        }
    }
}
