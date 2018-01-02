namespace TheCollection.Web.Models {
    using TheCollection.Application.Services.Contracts;

    public class Search : ISearch {
        public string Searchterm { get; set; }

        public int Pagesize { get; set; }
    }
}
