using TheCollection.Business.Tea;
using TheCollection.Import.Console.Models;

namespace TheCollection.Import.Console.Translators
{
    public class MerkToBrandTranslator : ITranslator<Merk, Brand>
    {
        public void Translate(Merk source, Brand destination)
        {
            destination.Name = source.TheeMerk.Trim();
        }
    }
}
