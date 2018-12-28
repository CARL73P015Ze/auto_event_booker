using System;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace Stores
{
    public class Event
    {
        public string Id { get; set; }
        public string User { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Text { get; set; }

        public Event()
        {
            Id = Guid.NewGuid().ToString();
            User = "";
            Text = "";
            DueDate = DateTime.Today;
            StartTime = DueDate;
            EndTime = StartTime.AddHours(1);
        }

        public Event(string id, string user, DateTime dueDate, DateTime startTime, DateTime endTime, string text)
        {
            Id = id;
            User = user;
            DueDate = dueDate;
            StartTime = startTime;
            EndTime = endTime;
            Text = text;
        }
    }

    public class EventStore
    {
        public List<Event> Appointments = new List<Event>();

        public Event FindLastOverlappingEvent(string user, DateTime start, DateTime end)
        {
            return Appointments.Where(x => x.User == user &&
                   DateUtils.Intersects(x.StartTime, x.EndTime, start, end))
                            .OrderByDescending(x => x.EndTime)
                            .FirstOrDefault();
        }

        public void FindFreePeriod(string user, ref DateTime start, ref DateTime end, TimeSpan length)
        {
            Event lastOverlappingAppointment = FindLastOverlappingEvent(user, start, end);
            // might need a way to break out of this loop.
            while (lastOverlappingAppointment != null)
            {
                start = lastOverlappingAppointment.EndTime;
                end = start + length;

                lastOverlappingAppointment = FindLastOverlappingEvent(user, start, end);
            }
        }

    }
}
