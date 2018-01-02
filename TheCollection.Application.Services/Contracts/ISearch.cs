namespace TheCollection.Application.Services.Contracts {
    public interface ISearch {
        string Searchterm { get; }
        int Pagesize { get; }
    }
}
