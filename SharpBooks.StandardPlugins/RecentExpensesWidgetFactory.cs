//-----------------------------------------------------------------------
// <copyright file="RecentExpensesWidgetFactory.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.StandardPlugins
{
    using System;
    using SharpBooks.Plugins;

    internal class RecentExpensesWidgetFactory : IWidgetFactory
    {
        public string Name
        {
            get
            {
                return "Recent Expenses";
            }
        }

        public IWidget CreateInstance(ReadOnlyBook book, string settings)
        {
            return new RecentExpensesWidget(settings);
        }

        public string Configure(ReadOnlyBook book, string currentSettings)
        {
            //var view = new RecentExpensesConfiguration();
            //currentSettings = view.GetSettings(book, currentSettings);

            return currentSettings;
        }
    }
}
