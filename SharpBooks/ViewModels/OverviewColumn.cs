namespace SharpBooks.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using SharpBooks.Plugins;

    public class OverviewColumn
    {
        public int Width
        {
            get;
            private set;
        }

        public IEnumerable<IWidget> Widgets
        {
            get;
            private set;
        }
    }
}
