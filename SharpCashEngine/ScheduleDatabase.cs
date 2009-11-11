using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCash
{
    public class ScheduleDatabase
    {
        private Template template;
        private List<Schedule> schedules;

        internal ScheduleDatabase(Template template, IEnumerable<Schedule> schedules)
        {
            this.template = template;
            this.schedules = new List<Schedule>();
            if (schedules != null)
            {
                this.schedules.AddRange(schedules);
            }
        }

        public ICollection<Schedule> Schedules
        {
            get
            {
                return this.schedules.AsReadOnly();
            }
        }

        public Template Template
        {
            get
            {
                return template;
            }
        }
    }
}
