namespace TheCollection.Domain.Core.Contracts {
    using System.Drawing;
    using System.IO;

    public interface IImageConverter {
        Stream GetStream(Image pngImage);

        byte[] GetBytes(Image imgSrc);

        byte[] GetBytesScaled(Image imgSrc, int iWidth, int iHeight);
    }
}
