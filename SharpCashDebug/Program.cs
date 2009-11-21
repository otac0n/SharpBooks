using SharpCash;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace SharpCash.Debug
{
    class Program
    {
        static void Main()
        {
            var db = new GnuCashDatabase();

            var s = Reports.CashFlow(db);

            var v = new ChartView();
            v.RunApplication(s);
        }
    }
}
