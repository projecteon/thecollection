using System.Drawing;
using System.Drawing.Imaging;

namespace TheCollection.Lib.Converters
{
    public class JpgImageConverter
    {
        public static byte[] GetBytesScaledJPEG(Image imgSrc, int iWidth, int iHeight)
        {
            return ImageConverter.GetBytes(ImageConverter.GetBytesScaledBitmap(imgSrc, iWidth, iHeight), GetJpegEncoder(), GetJPegEncoderParams());
        }
        
        /// <summary>
         /// The Jpeg Encoder
         /// </summary>
         /// <returns>The Jpeg Encoder</returns>
        static ImageCodecInfo GetJpegEncoder()
        {
            ImageCodecInfo[] infos = ImageCodecInfo.GetImageEncoders();

            for (int i = 0; i < infos.Length; i++)
            {
                if (ImageFormat.Jpeg.Guid.Equals(infos[i].FormatID))
                    return infos[i];
            }

            return null;
        }

        /// <summary>
        /// The Jpeg Compression Encoder Params (High Quality)
        /// </summary>
        /// <returns>The Tiff Encoder Params</returns>
        static EncoderParameters GetJPegEncoderParams()
        {
            EncoderParameters encParams = new EncoderParameters(1);
            encParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 80L);

            return encParams;
        }
    }
}
