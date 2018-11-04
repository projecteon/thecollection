namespace TheCollection.Application.Services.Queries.Tea {
    using TheCollection.Domain.Core.Contracts;

    public class BagsCountByBrandsQuery : IQuery {
        public BagsCountByBrandsQuery(int top = 10) {
            Top = top;
        }

        public int Top { get; }
    }
}
