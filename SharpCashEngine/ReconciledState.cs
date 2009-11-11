using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace SharpCash
{
    public enum ReconciledState
    {
        [Description("n")]
        Unreconciled,

        [Description("y")]
        Reconciled,

        [Description("c")]
        Cleared,

        [Description("v")]
        Voided,

        [Description("f")]
        Frozen
    }
}
