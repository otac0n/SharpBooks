// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.StandardPlugins
{
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Encapsulates the settings for the "Favorite Accounts" widget.
    /// </summary>
    internal class FavoriteAccountSettings
    {
        public FavoriteAccountSettings()
        {
            this.PathSeperator = Path.DirectorySeparatorChar.ToString();
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
