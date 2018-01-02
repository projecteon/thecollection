namespace TheCollection.Application.Services.ViewModels {
    using TheCollection.Application.Services.Contracts;

    public class Search : ISearch {
        public string Searchterm { get; set; }

        public int Pagesize { get; set; }
    }
}
