//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GnuCashIntegration.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class ScheduledTransaction
    {
        public string Guid { get; set; }
        public string Name { get; set; }
        public long Enabled { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string LastOccurence { get; set; }
        public long NumOccurences { get; set; }
        public long RemainingOccurences { get; set; }
        public long AutoCreate { get; set; }
        public long AutoNotify { get; set; }
        public long adv_creation { get; set; }
        public long adv_notify { get; set; }
        public long InstanceCount { get; set; }
        public string TemplateAccountGuid { get; set; }
    }
}
