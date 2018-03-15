namespace TheCollection.Import.Console.Translators {
    using TheCollection.Domain.Core.Contracts;
    using TheCollection.Domain.Tea;
    using TheCollection.Import.Console.Models;

    public class MerkToBrandTranslator : ITranslator<Merk, Brand> {
        public Brand Translate(Merk source) {
            return new Brand(null, source.TheeMerk.Trim());
        }
    }
}
