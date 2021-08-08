//-----------------------------------------------------------------------
// <copyright file="RecentExpensesSettings.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>otac0n</author>
//-----------------------------------------------------------------------

namespace SharpBooks.StandardPlugins
{
    using System.Collections.Generic;

    /// <summary>
    /// Encapsulates the settings for the "Recent Expenses" widget.
    /// </summary>
    internal class RecentExpensesSettings
    {
        public RecentExpensesSettings()
        {
            this.PathSeperator = @"\";
            this.AccountPaths = new List<string>();
        }

        public List<string> AccountPaths
        {
            get;
            set;
        }

        public string PathSeperator
        {
            get;
            set;
        }
    }
}
