using System;
using System.Collections.Generic;
using System.Linq;

namespace Stores
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

    public class User
    {
        public string Name { get; set; }
        public List<string> SkillSet { get; set; }
        public Dictionary<DayOfWeek, WorkingHours> WorkingDays { get; set; }

        public User()
        {
            SkillSet = new List<string>();
            WorkingDays = new Dictionary<DayOfWeek, WorkingHours>();
            Name = "";
        }


    }

    public class UserStore
    {
        private Dictionary<string, User> Users = new Dictionary<string, User>();
        public List<User> GetUsers()
        {
            return Users.Values.ToList();
        }

        public void SaveUser(User user)
        {
            if (!Users.ContainsKey(user.Name))
            {
                Users.Add(user.Name, user);
            }
        }

        public User GetUser(string userName)
        {
            if (Users.ContainsKey(userName))
                return Users[userName];

            return null;
        }
    }
}
