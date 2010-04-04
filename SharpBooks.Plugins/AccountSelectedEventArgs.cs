namespace SharpBooks.Plugins
{
    using System;

    public class AccountSelectedEventArgs : EventArgs
    {
        public Guid AccountId
        {
            get;
            set;
        }
    }
}
