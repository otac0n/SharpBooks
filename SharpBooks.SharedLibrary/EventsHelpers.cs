﻿//-----------------------------------------------------------------------
// <copyright file="FormsHelpers.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class EventsHelpers
    {
        public static void SafeInvoke<TEventArgs>(this EventHandler<TEventArgs> foo, object @this, Func<TEventArgs> getArgs) where TEventArgs : EventArgs
        {
            if (foo != null)
            {
                var args = getArgs();
                foo(@this, args);
            }
        }

        public static void SafeInvoke<TEventArgs>(this EventHandler<TEventArgs> foo, object @this, TEventArgs eArgs) where TEventArgs : EventArgs
        {
            if (foo != null)
            {
                foo(@this, eArgs);
            }
        }
    }
}
