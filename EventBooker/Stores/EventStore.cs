using System;
using System.Collections.Generic;
using System.Linq;
using Utils;
using DataObjects;

namespace Stores
{
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
