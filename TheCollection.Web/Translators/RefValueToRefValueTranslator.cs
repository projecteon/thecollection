namespace TheCollection.Web.Translators
{
    public class RefValueToRefValueTranslator : ITranslator<Business.RefValue, Models.RefValue>
    {
        public void Translate(Business.RefValue source, Models.RefValue destination)
        {
            destination.Id = source.Id;
            destination.Name = source.Name;
        }
    }
}