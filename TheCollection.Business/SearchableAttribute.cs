using System;

namespace TheCollection.Business
{

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class SearchableAttribute : Attribute
    {
    }
}
