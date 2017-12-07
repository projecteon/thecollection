namespace TheCollection.Domain.Contracts.Repository {
    using System.Drawing;
    using System.IO;
    using System.Threading.Tasks;

    public interface IImageRepository {
        Task<Bitmap> Get(string filename);

        Task<string> Upload(Stream stream, string filename);

        Task<bool> Delete(string filename);
    }
}
