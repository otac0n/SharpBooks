//-----------------------------------------------------------------------
// <copyright file="FavoriteAccountsWidgetFactory.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;
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
            return new FavoriteAccountsWidget(settings);
        }

        public string Configure(ReadOnlyBook book, string currentSettings)
        {
            //var view = new FavoriteAccountsConfiguration();
            //currentSettings = view.GetSettings(book, currentSettings);

            return currentSettings;
        }
    }
}
