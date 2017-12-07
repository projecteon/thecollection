namespace TheCollection.Domain.Converters {
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;

    public class BitmapConverter {
        // https://stackoverflow.com/questions/38816932/net-core-image-manipulation-crop-resize-file-handling
        // https://andrewlock.net/using-imagesharp-to-resize-images-in-asp-net-core-a-comparison-with-corecompat-system-drawing/

        public static Bitmap GetBytesScaledBitmap(Image imgSrc, int iWidth, int iHeight, bool bTransparent = false, bool bCenterAlign = false) {
            if (iHeight == 0) {
                // Scale to width (keep aspect)
                float fScale = (float)iWidth / imgSrc.Width;
                iHeight = (int)(imgSrc.Height * fScale);
            }
            else if (iWidth == 0) {
                // Scale to height (keep aspect)
                float fScale = (float)iHeight / imgSrc.Height;
                iWidth = (int)(imgSrc.Width * fScale);
            }

            return AutoFitImage(imgSrc, iWidth, iHeight, bTransparent, bCenterAlign);
        }

        private static Bitmap AutoFitImage(Image imgSrc, int iWidth, int iHeight, bool bTransparent = false, bool bCenterAlign = false) {
            Bitmap bmTarget;

            try {
                // Autofit
                Rectangle recTarget = GetReScale(imgSrc, iWidth, iHeight, bCenterAlign);
                bmTarget = new Bitmap(iWidth, iHeight, PixelFormat.Format24bppRgb);

                if (bTransparent)
                    bmTarget.MakeTransparent();

                using (Graphics g = Graphics.FromImage(bmTarget)) {
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
            catch (Exception ex) {
                // Rethrow
                throw new Exception("Failed to convert image. Possibly invalid format.", ex);
            }

            return bmTarget;
        }

        private static Rectangle GetReScale(Image imgSrc, int iWidth, int iHeight, bool bCenterAlign) {
            // Ratio
            float fRatioTarget = (float)iWidth / iHeight;
            float fRatioSrc = (float)imgSrc.Width / imgSrc.Height;
            if (fRatioSrc > fRatioTarget) {
                return ScaleToWidth(imgSrc, iWidth, iHeight, bCenterAlign);
            }

            return ScaleToHeight(imgSrc, iWidth, iHeight, bCenterAlign);
        }

        private static Rectangle ScaleToHeight(Image imgSrc, int iWidth, int iHeight, bool bCenterAlign) {
            var fScale = (float)iHeight / imgSrc.Height;
            int iPaddingWidth = iWidth - (int)Math.Round(imgSrc.Width * fScale);
            if (bCenterAlign && iPaddingWidth > 0) {
                return new Rectangle(iPaddingWidth / 2, 0, (int)Math.Round(imgSrc.Width * fScale), iHeight);
            }

            return new Rectangle(0, 0, (int)(imgSrc.Width * fScale), iHeight);
        }

        private static Rectangle ScaleToWidth(Image imgSrc, int iWidth, int iHeight, bool bCenterAlign) {
            var fScale = (float)iWidth / imgSrc.Width;
            int iPaddingHeight = iHeight - (int)Math.Round(imgSrc.Height * fScale);
            if (bCenterAlign && iPaddingHeight > 0) {
                return new Rectangle(0, iPaddingHeight / 2, iWidth, (int)Math.Round(imgSrc.Height * fScale));
            }

            return new Rectangle(0, 0, iWidth, (int)(imgSrc.Height * fScale));
        }
    }
}
