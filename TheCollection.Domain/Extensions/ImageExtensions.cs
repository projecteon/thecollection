namespace TheCollection.Domain.Extensions {
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
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

        // https://stackoverflow.com/questions/38816932/net-core-image-manipulation-crop-resize-file-handling
        // https://andrewlock.net/using-imagesharp-to-resize-images-in-asp-net-core-a-comparison-with-corecompat-system-drawing/
        public static Bitmap GetBytesScaledBitmap(this Image imgSrc, int iWidth, int iHeight, bool bTransparent = false, bool bCenterAlign = false) {
            if (iHeight == 0) {
                // Scale to width (keep aspect)
                var fScale = (float)iWidth / imgSrc.Width;
                iHeight = (int)(imgSrc.Height * fScale);
            }
            else if (iWidth == 0) {
                // Scale to height (keep aspect)
                var fScale = (float)iHeight / imgSrc.Height;
                iWidth = (int)(imgSrc.Width * fScale);
            }

            return AutoFitImage(imgSrc, iWidth, iHeight, bTransparent, bCenterAlign);
        }

        static Bitmap AutoFitImage(Image imgSrc, int iWidth, int iHeight, bool bTransparent = false, bool bCenterAlign = false) {
            try {
                // Autofit
                var recTarget = GetReScale(imgSrc, iWidth, iHeight, bCenterAlign);
                var bmTarget = new Bitmap(iWidth, iHeight, PixelFormat.Format24bppRgb);

                if (bTransparent)
                    bmTarget.MakeTransparent();

                using (var g = Graphics.FromImage(bmTarget)) {
                    if (bTransparent)
                        g.Clear(Color.Transparent);
                    else
                        g.Clear(Color.White);
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.CompositingQuality = CompositingQuality.HighSpeed;
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.DrawImage(imgSrc, recTarget);
                }

                return bmTarget;
            }
            catch (Exception ex) {
                // Rethrow
                throw new Exception("Failed to convert image. Possibly invalid format.", ex);
            }
        }

        static Rectangle GetReScale(Image imgSrc, int iWidth, int iHeight, bool bCenterAlign) {
            // Ratio
            var fRatioTarget = (float)iWidth / iHeight;
            var fRatioSrc = (float)imgSrc.Width / imgSrc.Height;
            if (fRatioSrc > fRatioTarget) {
                return ScaleToWidth(imgSrc, iWidth, iHeight, bCenterAlign);
            }

            return ScaleToHeight(imgSrc, iWidth, iHeight, bCenterAlign);
        }

        static Rectangle ScaleToHeight(Image imgSrc, int iWidth, int iHeight, bool bCenterAlign) {
            var fScale = (float)iHeight / imgSrc.Height;
            var iPaddingWidth = iWidth - (int)Math.Round(imgSrc.Width * fScale);
            if (bCenterAlign && iPaddingWidth > 0) {
                return new Rectangle(iPaddingWidth / 2, 0, (int)Math.Round(imgSrc.Width * fScale), iHeight);
            }

            return new Rectangle(0, 0, (int)(imgSrc.Width * fScale), iHeight);
        }

        static Rectangle ScaleToWidth(Image imgSrc, int iWidth, int iHeight, bool bCenterAlign) {
            var fScale = (float)iWidth / imgSrc.Width;
            var iPaddingHeight = iHeight - (int)Math.Round(imgSrc.Height * fScale);
            if (bCenterAlign && iPaddingHeight > 0) {
                return new Rectangle(0, iPaddingHeight / 2, iWidth, (int)Math.Round(imgSrc.Height * fScale));
            }

            return new Rectangle(0, 0, iWidth, (int)(imgSrc.Height * fScale));
        }
    }
}
