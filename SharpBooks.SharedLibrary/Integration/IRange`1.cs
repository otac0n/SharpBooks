// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Integration
{
    using System;

    public interface IRange<T> where T : IComparable<T>
    {
        T End { get; }

        bool EndInclusive { get; }

        T Start { get; }

        bool StartInclusive { get; }

        IRange<T> Clone(T start, bool startInclusive, T end, bool endInclusive);
    }
}
