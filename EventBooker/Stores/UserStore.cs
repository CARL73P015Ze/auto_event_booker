using System;
using System.Collections.Generic;
using System.Linq;
using DataObjects;

namespace Stores
{
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
