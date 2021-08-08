//-----------------------------------------------------------------------
// <copyright file="FavoriteAccountSettings.cs" company="(none)">
//  Copyright © 2012 John Gietzen. All rights reserved.
// </copyright>
// <author>otac0n</author>
//-----------------------------------------------------------------------

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
