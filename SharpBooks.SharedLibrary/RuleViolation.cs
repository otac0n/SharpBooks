//-----------------------------------------------------------------------
// <copyright file="RuleViolation.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    public class RuleViolation
    {
        public RuleViolation(string source, string message)
        {
            this.Source = source;
            this.Message = message;
        }

        public string Message
        {
            get;
            private set;
        }

        public string Source
        {
            get;
            private set;
        }
    }
}
