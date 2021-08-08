// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Integration
{
    using System;

    public class StringRange : IRange<int>
    {
        private readonly string source;
        private readonly string value;

        public StringRange(string source)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
            this.Start = 0;
            this.Length = source.Length;
            this.value = source.Substring(this.Start, this.Length);
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
            this.Start = start;
            this.Length = length;
            this.value = source.Substring(start, length);
        }

        /// <inheritdoc/>
        public int End => this.Start + this.Length;

        /// <inheritdoc/>
        bool IRange<int>.EndInclusive => false;

        public int Length { get; }

        /// <inheritdoc/>
        public int Start { get; }

        /// <inheritdoc/>
        bool IRange<int>.StartInclusive => true;

        public string Value => this.value;

        /// <inheritdoc/>
        IRange<int> IRange<int>.Clone(int start, bool startInclusive, int end, bool endInclusive)
        {
            return new StringRange(this.source, start, end - start);
        }
    }
}
