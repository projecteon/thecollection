namespace TheCollection.Business
{
    public class RefValue : IRef
    {
        public string Id { get; set; }

        [Searchable]
        public string Name { get; set; }
    }

    public interface IRef
    {
        string Id { get; set; }

        string Name { get; set; }
    }
}
