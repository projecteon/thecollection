namespace TheCollection.Application.Services {
    using TheCollection.Application.Services.Contracts;

    public class ErrorResult : IActivityResult {
        public ErrorResult(string error = null) {
            Error = error;
        }

        public string Error { get; }
    }
}
