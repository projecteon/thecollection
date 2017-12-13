namespace TheCollection.Web.Repositories {
    using System;
    using System.Drawing;
    using System.IO;
    using System.Threading.Tasks;
    using TheCollection.Domain.Contracts.Repository;

    public class ImageFilesystemRepository : IImageRepository {
        public const string Path = @"C:\src\projecteon\Theedatabase\Afbeeldingen Zakjes\";

        public Task<bool> Delete(string filename) {
            throw new NotImplementedException();
        }

        public Task<Bitmap> Get(string filename) {
            return Task.Run(() => { return new Bitmap($"{Path}{filename}"); });
        }

        public Task<string> Upload(Stream stream, string filename) {
            //if (!System.IO.Directory.Exists(Path))
            //{
            //    System.IO.Directory.CreateDirectory(Path);
            //}

            //return Task.Run(() => System.IO.File.Copy(path, $"{Path}{filename}", true));
            throw new NotImplementedException();
        }
    }
}
