namespace SharpBooks.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Overview
    {
        public IEnumerable<OverviewColumn> Columns
        {
            get;
            private set;
        }
    }
}
