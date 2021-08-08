//-----------------------------------------------------------------------
// <copyright file="IRange`1.cs" company="(none)">
//  Copyright © 2011 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

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
