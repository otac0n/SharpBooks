// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.UI
{
    using System;
    using System.Drawing;

    public class DesiresOffsetEventArgs : EventArgs
    {
        private readonly Point desiredOffset;

        public DesiresOffsetEventArgs(Point desiredOffset)
        {
            this.desiredOffset = desiredOffset;
        }

        public Point DesiredOffset
        {
            get { return this.desiredOffset; }
        }
    }
}
