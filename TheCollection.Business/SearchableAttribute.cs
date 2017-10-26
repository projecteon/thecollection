namespace TheCollection.Business
{

    using System;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class SearchableAttribute : Attribute
    {
    }
}
