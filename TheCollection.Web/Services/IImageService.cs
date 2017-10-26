using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace TheCollection.Web.Services {

    public interface IImageService {

        Task<Bitmap> Get(string filename);

        Task<string> Upload(Stream stream, string filename);

        Task<bool> Delete(string filename);
    }
}
