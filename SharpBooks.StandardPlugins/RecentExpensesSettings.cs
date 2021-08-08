// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

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
