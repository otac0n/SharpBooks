using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCash
{
    public class Transaction
    {
        private Guid id;
        private Commodity currency;
        private DateTime datePosted;
        private DateTime dateEntered;
        private string description;
        private List<Split> splits;

        internal Transaction(Guid id, Commodity currency, DateTime datePosted, DateTime dateEntered, string description)
        {
            this.id = id;
            this.currency = currency;
            this.datePosted = datePosted;
            this.dateEntered = dateEntered;
            this.description = description;
            this.splits = new List<Split>();
        }

        internal void AddSplit(Split split)
        {
            this.splits.Add(split);
        }

        internal void AddSplits(IEnumerable<Split> splits)
        {
            this.splits.AddRange(splits);
        }

        public Guid Id
        {
            get
            {
                return this.id;
            }
        }

        public Commodity Currency
        {
            get
            {
                return this.currency;
            }
        }

        public DateTime DatePosted
        {
            get
            {
                return this.datePosted;
            }
        }

        public DateTime DateEntered
        {
            get
            {
                return this.dateEntered;
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }
        }

        public ICollection<Split> Splits
        {
            get
            {
                return this.splits.AsReadOnly();
            }
        }
    }
}
