using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace TheCollection.Lib.Converters
{
    public class PngImageConverter
    {
        public const int THUMB_DEFAULT_WIDTH_PARAM = 100;
        public const int THUMB_DEFAULT_HEIGHT_PARAM = 0;

        public static Stream GetStreamPNG(Image pngImage)
        {
            var memoryStream = new System.IO.MemoryStream();
            pngImage.Save(memoryStream, GetPngEncoder(), GetPngEncoderParams());
            return memoryStream;
        }

        public static byte[] GetBytesScaledPNG(Image imgSrc, int iWidth, int iHeight, bool bTransparent = false, bool bCenterAlign = false)
        {
            return ImageConverter.GetBytes(ImageConverter.GetBytesScaledBitmap(imgSrc, iWidth, iHeight, bTransparent, bCenterAlign), GetPngEncoder(), GetPngEncoderParams());
        }

        /// <summary>
        /// The Png Encoder
        /// </summary>
        /// <returns>The Png Encoder</returns>
        static ImageCodecInfo GetPngEncoder()
        {
            ImageCodecInfo[] infos = ImageCodecInfo.GetImageEncoders();

            for (int i = 0; i < infos.Length; i++)
            {
                if (ImageFormat.Png.Guid.Equals(infos[i].FormatID))
                    return infos[i];
            }

            return null;
        }

        /// <summary>
        /// The Png 256 Encoder Params
        /// </summary>
        /// <returns>The Png Encoder Params</returns>
        static EncoderParameters GetPngEncoderParams()
        {
            EncoderParameters encParams = new EncoderParameters(1);
            encParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.ColorDepth, 8L);

            return encParams;
        }
    }
}
