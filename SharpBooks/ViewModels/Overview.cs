using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpBooks.ViewModels
{
    public class Overview
    {
        public IEnumerable<OverviewColumn> Columns
        {
            get;
            private set;
        }
    }
}
