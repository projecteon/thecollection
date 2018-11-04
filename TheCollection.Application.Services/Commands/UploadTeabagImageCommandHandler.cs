namespace TheCollection.Application.Services.Commands {
    using System;
    using System.Threading.Tasks;
    using TheCollection.Domain.Core.Contracts;
    using TheCollection.Domain.Core.Contracts.Repository;
    using TheCollection.Domain.Tea;

    public class UploadTeabagImageCommandHandler : IAsyncCommandHandler<UploadTeabagImageCommand> {
        public UploadTeabagImageCommandHandler(ISearchRepository<Bag> bagsRepository, ICreateRepository<Image> createImageRepository, IImageRepository imageRepository) {
            BagsRepository = bagsRepository ?? throw new System.ArgumentNullException(nameof(bagsRepository));
            CreateImageRepository = createImageRepository ?? throw new System.ArgumentNullException(nameof(createImageRepository));
            ImageRepository = imageRepository ?? throw new System.ArgumentNullException(nameof(imageRepository));
        }

        ISearchRepository<Bag> BagsRepository { get; }
        ICreateRepository<Image> CreateImageRepository { get; }
        IImageRepository ImageRepository { get; }

        public async Task<ICommandResult> ExecuteAsync(UploadTeabagImageCommand command) {
            try {
                var bagsCount = BagsRepository.SearchRowCountAsync("");
                var fileExtension = System.IO.Path.GetExtension(command.FileName);
                var uri = await ImageRepository.Upload(command.FileStream, $"{bagsCount}.{fileExtension}");
                var newImage = new Image(null, command.FileName, uri);
                var id = await CreateImageRepository.CreateItemAsync(newImage);
                return new CreateResult(id);
            }
            catch (Exception ex) {
                var originalMessage = ex.Message;

                while (ex.InnerException != null)
                    ex = ex.InnerException;
                return new ErrorResult($"{originalMessage} | {ex.Message}");
            }
        }
    }
}
