using System.Collections.Generic;

namespace TheCollection.Console.Models
{
    class Thees
    {
        public List<Thee> TheeTotaallijst { get; set; }
    }

    class Thee
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
