//-----------------------------------------------------------------------
// <copyright file="FredPriceSource.cs" company="Microsoft">
//  Copyright (c) 2010 Microsoft
// </copyright>
// <author>otac0n</author>
//-----------------------------------------------------------------------

namespace FredPriceSource
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class FredPriceSource
    {
        // Series ID: DEXUSUK = EXchange rate, US dollars, UK pounds
        private const string UrlFormat = "http://api.stlouisfed.org/fred/series/observations?limit=1&sort_order=desc&api_key=9727d6c06b0f3c20cfab02a4fb70f3e3&series_id={0}";
    }
}
