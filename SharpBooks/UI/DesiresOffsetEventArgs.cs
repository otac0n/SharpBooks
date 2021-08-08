// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.UI
{
    using System;
    using System.Drawing;

    public class DesiresOffsetEventArgs : EventArgs
    {
        public DesiresOffsetEventArgs(Point desiredOffset)
        {
            this.DesiredOffset = desiredOffset;
        }

        public Point DesiredOffset { get; }
    }
}
