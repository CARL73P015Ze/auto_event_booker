using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataObjects
{
    public class Event
    {
        public string Id { get; set; }
        public string User { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Text { get; set; }
        public int Type { get; set; }

        public Event()
        {
            Id = Guid.NewGuid().ToString();
            User = "";
            Text = "";
            DueDate = DateTime.Today;
            StartTime = DueDate;
            EndTime = StartTime.AddHours(1);
            Type = 1;
        }

        public Event(string id, string user, DateTime dueDate, DateTime startTime, 
                     DateTime endTime, string text, int type)
        {
            Id = id;
            User = user;
            DueDate = dueDate;
            StartTime = startTime;
            EndTime = endTime;
            Text = text;
            Type = type;
        }
    }
}
