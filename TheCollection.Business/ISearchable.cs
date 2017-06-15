﻿using System.Collections.Generic;

namespace TheCollection.Business
{
    public interface ISearchable
    {
        IEnumerable<string> Tags { get; }
        string SearchString { get; }
    }
}