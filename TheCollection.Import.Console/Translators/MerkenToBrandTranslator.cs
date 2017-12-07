namespace TheCollection.Import.Console.Translators {
    using TheCollection.Domain.Tea;
    using TheCollection.Import.Console.Models;
    using TheCollection.Web.Translators;

    public class MerkToBrandTranslator : ITranslator<Merk, Brand> {
        public void Translate(Merk source, Brand destination) {
            destination.Name = source.TheeMerk.Trim();
        }
    }
}
