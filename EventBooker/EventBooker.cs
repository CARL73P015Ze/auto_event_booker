using System;
using System.Collections.Generic;
using System.Linq;
using Stores;
using DataObjects;

namespace EventBooker
{


    public class EventToBook
    {
        public DateTime DueDate { get; set; }
        public TimeSpan Length { get; set; }
        public string SkillSetRequired { get; set; }
        public string Text { get; set; }
        public Location Location { get; set; }
        public string Postcode { get; set; }
        public EventToBook(){
            Text = "";
            SkillSetRequired = "";
            DueDate = DateTime.Today;
            Length = new TimeSpan(1, 0, 0);
        }
    }

    public class AutoBookResults
    {
        public List<Event> Booked { get; private set; }
        public List<EventToBook> FailedToBook { get; private set; }

        public AutoBookResults()
        {
            Booked = new List<Event>();
            FailedToBook = new List<EventToBook>();
        }
    }

    public class EventManager
    {
        EventStore _EventStore;
        readonly TimeSpan _MAX_EVENT_LENGTH;

        public EventManager(EventStore eventStore)
        {
            _MAX_EVENT_LENGTH = new TimeSpan(24, 0, 0);
            _EventStore = eventStore;
        }

        public DateTime? FindValidWorkingDay(User user, DateTime dueDate, TimeSpan apptLength)
        {
            int day = 0;
            while (day < 7)
            {
                if (user.WorkingDays.ContainsKey(dueDate.DayOfWeek))
                {
                    DateTime startOfDay = user.WorkingDays[dueDate.DayOfWeek].Start;
                    DateTime endOfDay = user.WorkingDays[dueDate.DayOfWeek].Finish;
                    TimeSpan lengthOfDay = endOfDay.Subtract(startOfDay);

                    if (apptLength <= lengthOfDay)
                    {
                        DateTime result = dueDate.Date;
                        return result;
                    }
                }

                dueDate = dueDate.AddDays(1);
                day += 1;
            }
            return null;
        }

        private Event AllotAppointment(EventToBook toBook, User diaryUser, DateTime dueDate, DateTime maxDueDate)
        {
            DateTime startOfDay = diaryUser.WorkingDays[dueDate.DayOfWeek].Start;
            DateTime endOfDay = diaryUser.WorkingDays[dueDate.DayOfWeek].Finish;
            DateTime startDate = dueDate.AddHours(startOfDay.Hour);
            startDate = startDate.AddMinutes(startOfDay.Minute);

            DateTime endDate = startDate + toBook.Length;

            _EventStore.FindFreePeriod(diaryUser.Name, ref startDate, ref endDate, toBook.Length);


            if (startDate > maxDueDate)
                return null;
                

            if ((endDate.Date != startDate.Date) ||
                (endDate.TimeOfDay > endOfDay.TimeOfDay))
            {
                dueDate = startDate.Date.AddDays(1);
                DateTime? newDate = FindValidWorkingDay(diaryUser, dueDate, toBook.Length);
                if(newDate != null)
                    return AllotAppointment(toBook, diaryUser, newDate.Value, maxDueDate);
            }

            return new Event(
                Guid.NewGuid().ToString(),
                diaryUser.Name, 
                toBook.DueDate, 
                startDate, 
                endDate,
                toBook.Text);

        }

        private List<EventToBook> OrderBookings(List<EventToBook> eventsToBook)
        {
           return eventsToBook.
                              OrderBy(x => x.DueDate).
                              ThenByDescending(x => x.Length).
                              ToList();
        }


        public AutoBookResults AutoBook(List<EventToBook> eventsToBook, List<User> users)
        {
            AutoBookResults result = new AutoBookResults();
            List<EventToBook> events = OrderBookings(eventsToBook);
            if (users.Count() == 0)
            {
                result.FailedToBook.AddRange(eventsToBook);
                return result;
            }

            foreach (EventToBook toBook in events)
            {
                if(toBook.Length > _MAX_EVENT_LENGTH)
                {
                    result.FailedToBook.Add(toBook);
                    continue;
                }

                Event optimalBooking = null;
                foreach(User user in users)
                {
                    DateTime? dt = FindValidWorkingDay(user, toBook.DueDate, toBook.Length);
                    if (dt != null)
                    {
                        DateTime sDate = dt.Value;

                        const int MAX_DAYS_LOOKAHEAD = 10;
                        DateTime maxDueDate = toBook.DueDate.AddDays(MAX_DAYS_LOOKAHEAD);

                        Event provisonalBooking = null;
                        if (OnEventToBookCheck(toBook, user))
                            provisonalBooking = AllotAppointment(toBook, user, toBook.DueDate, maxDueDate);
                        if (provisonalBooking != null)
                        {
                            if (OnOptimalBookingCheck(optimalBooking, provisonalBooking))
                            {
                                optimalBooking = provisonalBooking;
    
                            }
                        }
                    }
                }


                if (optimalBooking != null)
                {
                    _EventStore.Appointments.Add(optimalBooking);
                    result.Booked.Add(optimalBooking);
                }
                else
                    result.FailedToBook.Add(toBook);
            }

            return result;
        }

        public bool OnEventToBookCheck(EventToBook toBook, User user)
        {
            if(user == null)
                return false;

            if (!string.IsNullOrEmpty(toBook.SkillSetRequired))
            {
                if (!user.SkillSet.Contains(toBook.SkillSetRequired))
                    return false;
            }

            return OnAdditionalEventToBookCheck(toBook, user);
        }

        public bool OnOptimalBookingCheck(Event optimalBooking, Event provisonalBooking)
        {
            if (optimalBooking == null)
                return true;

            if(provisonalBooking.StartTime < optimalBooking.StartTime)
                return true;

            return OnAdditionalOptimalBookingCheck(optimalBooking, provisonalBooking);
        }

        public virtual bool OnAdditionalEventToBookCheck(EventToBook toBook, User user)
        {
            return true;
        }

        public bool OnAdditionalOptimalBookingCheck(Event optimalBooking, Event provisonalBooking)
        {
            return true;
        }

    }
}
