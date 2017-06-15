using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace TheCollection.Lib.Converters
{
    class ImageConverter
    {
        public const int THUMB_DEFAULT_WIDTH_PARAM = 100;
        public const int THUMB_DEFAULT_HEIGHT_PARAM = 0;

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

        public static byte[] GetBytes(Image imgSrc, ImageCodecInfo enc, EncoderParameters encParams)
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
                    g.DrawImage(imgSrc, recTarget);
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
        #endregion
    }
}
