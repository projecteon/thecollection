namespace TheCollection.Web.Repositories {
    using System;
    using System.Drawing;
    using System.IO;
    using System.Threading.Tasks;
    using TheCollection.Domain.Core.Contracts.Repository;

    public class ImageFilesystemRepository : IImageRepository {
        public const string Path = @"C:\src\Theedatabase\Afbeeldingen Zakjes\";

        public async Task<bool> Delete(string filename) {
            throw new NotImplementedException();
        }

        public async Task<Bitmap> Get(string filename) {
            return await Task.Run(() => { return new Bitmap($"{Path}{filename}"); });
        }

        public async Task<string> Upload(Stream stream, string filename) {
            //if (!System.IO.Directory.Exists(Path))
            //{
            //    System.IO.Directory.CreateDirectory(Path);
            //}

            //return Task.Run(() => System.IO.File.Copy(path, $"{Path}{filename}", true));
            throw new NotImplementedException();
        }
    }
}
