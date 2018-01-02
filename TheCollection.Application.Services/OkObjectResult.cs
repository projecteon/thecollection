namespace TheCollection.Application.Services {
    using TheCollection.Application.Services.Contracts;

    public class OkObjectResult<TResult> : IActivityResult {
        public OkObjectResult(TResult result) {
            Value = result;
        }

        public TResult Value { get; }
    }
}
