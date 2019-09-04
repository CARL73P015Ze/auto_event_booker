using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataObjects
{

    public class User
    {
        public string Name { get; set; }
        public List<string> SkillSet { get; set; }
        public Dictionary<DayOfWeek, WorkingHours> WorkingDays { get; set; }
        public Location Location { get; set; }
        public User()
        {
            SkillSet = new List<string>();
            WorkingDays = new Dictionary<DayOfWeek, WorkingHours>();
            Name = "";
        }


    }
}
