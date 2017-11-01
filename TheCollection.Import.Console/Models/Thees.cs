namespace TheCollection.Import.Console.Models
{
    using System.Collections.Generic;

    public class Thees
    {
        public List<Thee> TheeTotaallijst { get; set; }
    }

    public class Thee
    {
        public int MainID { get; set; }
        public string TheeMerk { get; set; }
        public string TheeSerie { get; set; }
        public string TheeSmaak { get; set; }
        public string TheeKenmerken { get; set; }
        public string TheeSoortzakje { get; set; }
        public string TheeLandvanherkomst { get; set; }
        public string TheeSerienummer { get; set; }
        public string Theeinvoerdatum { get; set; }
    }
}
