﻿//-----------------------------------------------------------------------
// <copyright file="FavoriteAccountSettings.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>otac0n</author>
//-----------------------------------------------------------------------

namespace SharpBooks.StandardPlugins
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Encapsulates the settings for the "Favorite Accounts" widget.
    /// </summary>
    internal class FavoriteAccountSettings
    {
        public FavoriteAccountSettings()
        {
            this.PathSeperator = @"\";
            this.AccountPaths = new List<string>();
        }

        public string PathSeperator
        {
            get;
            set;
        }

        public List<string> AccountPaths
        {
            get;
            set;
        }
    }
}
