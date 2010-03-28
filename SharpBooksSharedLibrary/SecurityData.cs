//-----------------------------------------------------------------------
// <copyright file="SecurityData.cs" company="(none)">
//  Copyright © 2010 John Gietzen
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class SecurityData
    {
        public SecurityData(Security security)
        {
            this.SecurityType = security.SecurityType;
            this.Symbol = security.Symbol;
            this.Name = security.Name;
            this.SignFormat = security.SignFormat;
        }

        public SecurityType SecurityType
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        public string Symbol
        {
            get;
            private set;
        }

        public string SignFormat
        {
            get;
            private set;
        }
    }
}
