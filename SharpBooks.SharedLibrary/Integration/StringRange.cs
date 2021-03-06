﻿//-----------------------------------------------------------------------
// <copyright file="StringRange.cs" company="(none)">
//  Copyright © 2012 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Integration
{
    using System;

    public class StringRange : IRange<int>
    {
        private readonly int start;
        private readonly int length;
        private readonly string source;
        private readonly string value;

        public StringRange(string source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            this.source = source;
            this.start = 0;
            this.length = source.Length;
            this.value = source.Substring(start, length);
        }

        public StringRange(string source, int start, int length)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (start < 0 || start > source.Length)
            {
                throw new ArgumentOutOfRangeException("start");
            }

            if (length < 0 || start + length > source.Length)
            {
                throw new ArgumentOutOfRangeException("start");
            }

            this.source = source;
            this.start = start;
            this.length = length;
            this.value = source.Substring(start, length);
        }

        public int Start
        {
            get { return this.start; }
        }

        public int Length
        {
            get { return this.length; }
        }

        public int End
        {
            get { return this.start + this.length; }
        }

        public string Value
        {
            get { return this.value; }
        }

        bool IRange<int>.StartInclusive
        {
            get { return true; }
        }

        bool IRange<int>.EndInclusive
        {
            get { return false; }
        }

        IRange<int> IRange<int>.Clone(int start, bool startInclusive, int end, bool endInclusive)
        {
            return new StringRange(this.source, start, end - start);
        }
    }
}
