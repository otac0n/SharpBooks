// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks
{
    using System;

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
