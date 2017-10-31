namespace TheCollection.Import.Console.Models
{
    using System.Collections.Generic;

    public class Merkens
    {
        public List<Merk> tblTheeMerken { get; set; }
    }

    public class Merk
    {
        public string TheeMerk { get; set; }
    }
}
