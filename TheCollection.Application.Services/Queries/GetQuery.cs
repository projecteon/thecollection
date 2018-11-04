namespace TheCollection.Application.Services.Queries {
    using TheCollection.Domain.Core.Contracts;

    public class GetQuery: IQuery {
        public GetQuery(string id) {
            Id = id;
        }

        public string Id { get; }
    }
}
