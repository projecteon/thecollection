namespace TheCollection.Web.Translators.Tea
{
    using TheCollection.Business.Tea;
    using TheCollection.Web.Models;

    public class CountryToRefValueTranslator : ITranslator<Country, RefValue>
    {
        public void Translate(Country source, RefValue destination)
        {
            destination.id = source.Id;
            destination.name = source.Name;
        }
    }
}
