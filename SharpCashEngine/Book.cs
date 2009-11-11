using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCash
{
    public class Book
    {
        private Guid guid;
        private CommodityDatabase commodityDatabase;
        private PriceDatabase priceDatabase;
        private AccountDatabase accountDatabase;
        private TransactionDatabase transactionDatabase;
        private ScheduleDatabase scheduleDatabase;

        internal Book(Guid guid, CommodityDatabase commodityDatabase, PriceDatabase priceDatabase, AccountDatabase accountDatabase, TransactionDatabase transactionDatabase, ScheduleDatabase scheduleDatabase)
        {
            this.guid = guid;
            this.commodityDatabase = commodityDatabase;
            this.priceDatabase = priceDatabase;
            this.accountDatabase = accountDatabase;
            this.transactionDatabase = transactionDatabase;
            this.scheduleDatabase = scheduleDatabase;
        }

        public Guid Guid
        {
            get
            {
                return this.guid;
            }
        }

        public CommodityDatabase CommodityDatabase
        {
            get
            {
                return this.commodityDatabase;
            }
        }

        public PriceDatabase PriceDatabase
        {
            get
            {
                return this.priceDatabase;
            }
        }

        public AccountDatabase AccountDatabase
        {
            get
            {
                return this.accountDatabase;
            }
        }

        public TransactionDatabase TransactionDatabase
        {
            get
            {
                return this.transactionDatabase;
            }
        }

        public ScheduleDatabase ScheduleDatabase
        {
            get
            {
                return this.scheduleDatabase;
            }
        }
    }
}
