using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpBooks.ViewModels
{
    internal class SplitViewModel : ViewModelBase
    {
        private Split split;

        public SplitViewModel(Split split)
        {
            this.split = split;
        }

        public DateTime Date
        {
            get
            {
                return this.split.DateCleared ?? this.split.Transaction.Date;
            }
        }

        public bool IsNegative
        {
            get
            {
                return this.split.Amount < 0;
            }
        }

        public string Value
        {
            get
            {
                return this.split.Account.Security.FormatValue(this.split.Amount);
            }
        }
    }
}
