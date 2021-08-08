// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Integration
{
    using System;

    public class StringRange : IRange<int>
    {
        private readonly int length;
        private readonly string source;
        private readonly int start;
        private readonly string value;

        public StringRange(string source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            this.source = source;
            this.start = 0;
            this.length = source.Length;
            this.value = source.Substring(this.start, this.length);
        }

        public StringRange(string source, int start, int length)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (start < 0 || start > source.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(start));
            }

            if (length < 0 || start + length > source.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(start));
            }

            this.source = source;
            this.start = start;
            this.length = length;
            this.value = source.Substring(start, length);
        }

        public int End
        {
            get { return this.start + this.length; }
        }

        bool IRange<int>.EndInclusive
        {
            get { return false; }
        }

        public int Length
        {
            get { return this.length; }
        }

        public int Start
        {
            get { return this.start; }
        }

        bool IRange<int>.StartInclusive
        {
            get { return true; }
        }

        public string Value
        {
            get { return this.value; }
        }

        IRange<int> IRange<int>.Clone(int start, bool startInclusive, int end, bool endInclusive)
        {
            return new StringRange(this.source, start, end - start);
        }
    }
}
