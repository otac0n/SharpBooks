//-----------------------------------------------------------------------
// <copyright file="IRange`1.cs" company="(none)">
//  Copyright © 2011 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Integration
{
    using System;

    public interface IRange<out T> where T : IComparable<T>
    {
        T Start { get; }

        bool StartInclusive { get; }

        T End { get; }

        bool EndInclusive { get; }
    }
}
