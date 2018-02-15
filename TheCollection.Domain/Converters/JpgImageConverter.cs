namespace TheCollection.Domain.Converters {
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using TheCollection.Domain.Core.Contracts;
    using TheCollection.Domain.Extensions;

    public class JpgImageConverter : IImageConverter {
        public Stream GetStream(Image pngImage) {
            var memoryStream = new MemoryStream();
            pngImage.Save(memoryStream, GetJpegEncoder, GetJPegEncoderParams);
            return memoryStream;
        }

        public byte[] GetBytes(Image imgSrc) {
            return imgSrc.GetBytes(GetJpegEncoder, GetJPegEncoderParams);
        }

        public byte[] GetBytesScaled(Image imgSrc, int iWidth, int iHeight) {
            return imgSrc.GetBytesScaledBitmap(iWidth, iHeight).GetBytes(GetJpegEncoder, GetJPegEncoderParams);
        }

        static ImageCodecInfo GetJpegEncoder {
            get {
                var infos = ImageCodecInfo.GetImageEncoders();
                return infos.FirstOrDefault(info => ImageFormat.Jpeg.Guid.Equals(info.FormatID));
            }
        }

        static EncoderParameters GetJPegEncoderParams {
            get {
                var encParams = new EncoderParameters(1);
                encParams.Param[0] = new EncoderParameter(Encoder.Quality, 80L);
                return encParams;
            }
        }
    }
}
