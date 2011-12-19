﻿//-----------------------------------------------------------------------
// <copyright file="DesiresOffsetEventArgs.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.UI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
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
