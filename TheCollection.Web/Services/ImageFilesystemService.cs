﻿using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace TheCollection.Web.Services
{
    public class ImageFilesystemService : IImageService
    {
        const string Path = @"D:\Source\projecteon\core_testing\testspa\wwwroot\images\";

        public Task Delete(string path)
        {
            throw new NotImplementedException();
        }

        public Task<Bitmap> Get(string filename)
        {
            return Task.Run(() => { return new Bitmap($"{Path}{filename}"); });
        }

        public Task<string> Upload(Stream stream, string filename)
        {
            //if (!System.IO.Directory.Exists(Path))
            //{
            //    System.IO.Directory.CreateDirectory(Path);
            //}

            //return Task.Run(() => System.IO.File.Copy(path, $"{Path}{filename}", true));
            throw new NotImplementedException();
        }
    }
}