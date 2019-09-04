using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataObjects
{
    public class WorkingHours
    {
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }
        public WorkingHours()
        {
            Start = DateTime.Today;
            Finish = Start.AddHours(1);
        }
    }

}
