namespace TheCollection.Web.Translators.Tea
{
    using TheCollection.Web.Extensions;

    public class BagToBagTranslator : ITranslator<Business.Tea.Bag, Models.Tea.Bag>
    {
        public BagToBagTranslator()
        {
            RefValueTranslator = new RefValueToRefValueTranslator();
        }

        public RefValueToRefValueTranslator RefValueTranslator { get; }

        public void Translate(Business.Tea.Bag source, Models.Tea.Bag destination)
        {
            destination.Id = source.Id;
            destination.MainID = source.MainID;
            destination.Brand = RefValueTranslator.Translate(source.Brand);
            destination.Serie = source.Serie;
            destination.Flavour = source.Flavour;
            destination.Hallmark = source.Hallmark;
            destination.BagType = RefValueTranslator.Translate(source.BagType);
            destination.Country = RefValueTranslator.Translate(source.Country);
            destination.SerialNumber = source.SerialNumber;
            destination.InsertDate = source.InsertDate;
            destination.ImageId = source.ImageId;
        }
    }
}