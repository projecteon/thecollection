namespace TheCollection.Lib.Converters
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using TheCollection.Lib.Extensions;

    public class JpgImageConverter: IImageConverter
    {
        public Stream GetStream(Image pngImage)
        {
            var memoryStream = new MemoryStream();
            pngImage.Save(memoryStream, GetJpegEncoder, GetJPegEncoderParams);
            return memoryStream;
        }

        public byte[] GetBytes(Image imgSrc)
        {
            return imgSrc.GetBytes(GetJpegEncoder, GetJPegEncoderParams);
        }

        public byte[] GetBytesScaled(Image imgSrc, int iWidth, int iHeight)
        {
            return BitmapConverter.GetBytesScaledBitmap(imgSrc, iWidth, iHeight).GetBytes(GetJpegEncoder, GetJPegEncoderParams);
        }
        
        static ImageCodecInfo GetJpegEncoder
        {
            get
            {
                ImageCodecInfo[] infos = ImageCodecInfo.GetImageEncoders();

                for (int i = 0; i < infos.Length; i++)
                {
                    if (ImageFormat.Jpeg.Guid.Equals(infos[i].FormatID))
                        return infos[i];
                }

                return null;
            }
        }

        static EncoderParameters GetJPegEncoderParams
        {
            get
            {
                EncoderParameters encParams = new EncoderParameters(1);
                encParams.Param[0] = new EncoderParameter(Encoder.Quality, 80L);
                return encParams;
            }
        }
    }
}
