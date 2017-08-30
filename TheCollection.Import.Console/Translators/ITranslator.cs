namespace TheCollection.Import.Console.Translators
{
    public interface ITranslator<in TSource, in TDestination>
    {
        void Translate(TSource source, TDestination destination);
    }
}
