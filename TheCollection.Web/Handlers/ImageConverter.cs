using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace TheCollection.Web.Handlers
{
    public class ImageConverter
    {
        // https://stackoverflow.com/questions/38816932/net-core-image-manipulation-crop-resize-file-handling
        // https://andrewlock.net/using-imagesharp-to-resize-images-in-asp-net-core-a-comparison-with-corecompat-system-drawing/

        public const int THUMB_DEFAULT_WIDTH_PARAM = 200;
        public const int THUMB_DEFAULT_HEIGHT_PARAM = 0;

        public static Stream GetStreamPNG(Image pngImage)
        {
            var memoryStream = new System.IO.MemoryStream();
            pngImage.Save(memoryStream, GetPngEncoder(), GetPngEncoderParams());
            return memoryStream;
        }

        public static byte[] GetBytesScaledJPEG(Image imgSrc, int iWidth, int iHeight)
        {
            var scaledBitmap = GetBytesScaledBitmap(imgSrc, iWidth, iHeight);
            return GetBytes(scaledBitmap, GetJpegEncoder(), GetJPegEncoderParams());
        }

        public static byte[] GetBytesScaledPNG(Image imgSrc, int iWidth, int iHeight, bool bTransparent = false, bool bCenterAlign = false)
        {
            var scaledBitmap = GetBytesScaledBitmap(imgSrc, iWidth, iHeight, bTransparent, bCenterAlign);
            return GetBytes(scaledBitmap, GetPngEncoder(), GetPngEncoderParams());
        }

        public static Bitmap GetBytesScaledBitmap(Image imgSrc, int iWidth, int iHeight, bool bTransparent = false, bool bCenterAlign = false)
        {
            if (iHeight == 0)
            {
                // Scale to width (keep aspect)
                float fScale = (float)iWidth / imgSrc.Width;
                iHeight = (int)(imgSrc.Height * fScale);
            }
            else if (iWidth == 0)
            {
                // Scale to height (keep aspect)
                float fScale = (float)iHeight / imgSrc.Height;
                iWidth = (int)(imgSrc.Width * fScale);
            }

            return AutoFitImage(imgSrc, iWidth, iHeight, bTransparent, bCenterAlign);
        }

        private static byte[] GetBytes(Image imgSrc, ImageCodecInfo enc, EncoderParameters encParams)
        {
            ArraySegment<byte> abRet = new ArraySegment<byte>();
            // To MemoryStream
            using (MemoryStream ms = new MemoryStream())
            {
                imgSrc.Save(ms, enc, encParams);

                // To Bytes
                ms.TryGetBuffer(out abRet);
            }
            return abRet.Array;
        }

        private static Bitmap AutoFitImage(Image imgSrc, int iWidth, int iHeight, bool bTransparent = false, bool bCenterAlign = false)
        {
            Bitmap bmTarget;

            try
            {
                // Ratio
                float fRatioTarget = (float)iWidth / iHeight;
                float fRatioSrc = (float)imgSrc.Width / imgSrc.Height;

                // Autofit
                Rectangle recTarget;

                // Scale
                float fScale;
                if (fRatioSrc > fRatioTarget)
                {
                    // Scale to width
                    fScale = (float)iWidth / imgSrc.Width;

                    int iPaddingHeight = iHeight - (int)Math.Round(imgSrc.Height * fScale);

                    if (bCenterAlign && iPaddingHeight > 0)
                        recTarget = new Rectangle(0, iPaddingHeight / 2, iWidth, (int)Math.Round(imgSrc.Height * fScale));
                    else
                        recTarget = new Rectangle(0, 0, iWidth, (int)(imgSrc.Height * fScale));
                }
                else
                {
                    // Scale to height
                    fScale = (float)iHeight / imgSrc.Height;

                    int iPaddingWidth = iWidth - (int)Math.Round(imgSrc.Width * fScale);

                    if (bCenterAlign && iPaddingWidth > 0)
                        recTarget = new Rectangle(iPaddingWidth / 2, 0, (int)Math.Round(imgSrc.Width * fScale), iHeight);
                    else
                        recTarget = new Rectangle(0, 0, (int)(imgSrc.Width * fScale), iHeight);
                }

                bmTarget = new Bitmap(iWidth, iHeight, PixelFormat.Format24bppRgb);

                if (bTransparent)
                    bmTarget.MakeTransparent();

                using (Graphics g = Graphics.FromImage(bmTarget))
                {
                    if (bTransparent)
                        g.Clear(Color.Transparent);
                    else
                        g.Clear(Color.White);

                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.CompositingQuality = CompositingQuality.HighSpeed;
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.DrawImage(imgSrc, recTarget.X, recTarget.Y, recTarget.Width, recTarget.Height);
                }
            }
            catch (Exception ex)
            {
                // Rethrow
                throw new Exception("Failed to convert image. Possibly invalid format.", ex);
            }

            return bmTarget;
        }

        #region Encoders
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

        static ImageCodecInfo GetGifEncoder()
        {
            ImageCodecInfo[] infos = ImageCodecInfo.GetImageEncoders();

            for (int i = 0; i < infos.Length; i++)
            {
                if (ImageFormat.Gif.Guid.Equals(infos[i].FormatID))
                    return infos[i];
            }

            return null;
        }

        static EncoderParameters GetGifEncoderParams()
        {
            EncoderParameters encParams = new EncoderParameters(1);
            encParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 80L);

            return encParams;
        }

        /// <summary>
        /// The Tiff Encoder
        /// </summary>
        /// <returns>The Tiff Encoder</returns>
        static ImageCodecInfo GetTiffEncoder()
        {
            ImageCodecInfo[] infos = ImageCodecInfo.GetImageEncoders();

            for (int i = 0; i < infos.Length; i++)
            {
                if (ImageFormat.Tiff.Guid.Equals(infos[i].FormatID))
                    return infos[i];
            }

            return null;
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

        static void GetEncoders(string sFilePath, out ImageCodecInfo idi, out EncoderParameters ep)
        {
            GetEncoders(out idi, out ep);
        }

        static void GetEncoders(out ImageCodecInfo idi, out EncoderParameters ep)
        {
            idi = GetJpegEncoder();
            ep = GetJPegEncoderParams();
        }
        #endregion
    }
}
