// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

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
