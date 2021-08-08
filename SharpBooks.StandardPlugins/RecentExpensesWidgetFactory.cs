// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.StandardPlugins
{
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

        public string Configure(ReadOnlyBook book, string currentSettings)
        {
            ////var view = new RecentExpensesConfiguration();
            ////currentSettings = view.GetSettings(book, currentSettings);

            return currentSettings;
        }

        public IWidget CreateInstance(ReadOnlyBook book, string settings)
        {
            return new RecentExpensesWidget(settings);
        }
    }
}
