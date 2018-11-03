namespace TheCollection.Application.Services.Queries {
    using TheCollection.Domain.Core.Contracts;

    public class OkResult : IQueryResult {
        public OkResult(object result) {
            Value = result;
        }

        public object Value { get; }
    }
}
