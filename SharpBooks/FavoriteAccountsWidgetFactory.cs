//-----------------------------------------------------------------------
// <copyright file="FavoriteAccountsWidgetFactory.cs" company="Microsoft">
//  Copyright © 2010 Microsoft
// </copyright>
// <author>otac0n</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using SharpBooks.Plugins;

    internal class FavoriteAccountsWidgetFactory : IWidgetFactory
    {
        public string Name
        {
            get
            {
                return "Favorite Accounts";
            }
        }

        public IWidget CreateInstance(ReadOnlyBook book, string settings)
        {
            return new FavoriteAccountsWidget();
        }

        public string Configure(ReadOnlyBook book, string currentSettings)
        {
            return currentSettings;
        }
    }
}
