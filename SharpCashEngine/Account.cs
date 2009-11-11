using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCash
{
    public class Account
    {
        private AccountDatabase database;

        private Guid id;
        private string name;
        private string type;
        private Commodity commodity;
        private int commodityScu;
        private string description;
        private Guid? parentId;

        internal Account(AccountDatabase database, Guid id, string name, string type, Commodity commodity, int commodityScu, string description, Guid? parentId)
        {
            this.database = database;

            this.id = id;
            this.name = name;
            this.type = type;
            this.commodity = commodity;
            this.commodityScu = commodityScu;
            this.description = description;
            this.parentId = parentId;
        }

        public Guid Id
        {
            get
            {
                return this.id;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public string Type
        {
            get
            {
                return this.type;
            }
        }

        public Commodity Commodity
        {
            get
            {
                return this.commodity;
            }
        }

        public int CommodityScu
        {
            get
            {
                return this.commodityScu;
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }
        }

        public Account Parent
        {
            get
            {
                if (this.parentId.HasValue)
                {
                    return this.database.FindAccount(this.parentId.Value);
                }
                else
                {
                    return null;
                }
            }
        }

        public string FullName
        {
            get
            {
                var parent = this.Parent;
                if (parent == null)
                {
                    return "";
                }
                var parentName = parent.FullName;
                return parentName + (string.IsNullOrEmpty(parentName) ? string.Empty : this.database.AccountSeperator) + this.Name;
            }
        }
    }
}
