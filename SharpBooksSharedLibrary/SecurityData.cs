//-----------------------------------------------------------------------
// <copyright file="SecurityData.cs" company="(none)">
//  Copyright © 2010 John Gietzen
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;

    /// <summary>
    /// Holds a read-only, persistable copy of a security.
    /// </summary>
    public sealed class SecurityData
    {
        public SecurityData(Security security)
        {
            this.SecurityId = security.SecurityId;
            this.SecurityType = security.SecurityType;
            this.Symbol = security.Symbol;
            this.Name = security.Name;
            this.SignFormat = security.SignFormat;
        }

        public Guid SecurityId
        {
            get;
            private set;
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
