namespace TheCollection.Application.Services.Commands {
    using System.IO;
    using TheCollection.Domain.Core.Contracts;

    public class UploadTeabagImageCommand : ICommand {
        public UploadTeabagImageCommand(Stream fileStream, string fileName) {
            FileStream = fileStream;
            FileName = fileName;
        }

        public Stream FileStream { get; }
        public string FileName { get; }
    }
}
