using System;
using EventBooker;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Stores;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataObjects;

namespace EventBookerTests
{
    [TestClass]
    public class BasicEventBookerTests
    {
        EventStore _EventStore = null;
        UserStore _UserStore = null;
        EventManager Diary_ = null;
        List<EventToBook> AppointmentsToBook = null;
        AutoBookResults _AutoBookResults = null;

        public void GivenTheFollowingBookableAppointments(string length, string dueDate, string text)
        {
            EventToBook appointment = new EventToBook();
            appointment.Length = TimeSpan.Parse(length);
            appointment.DueDate = DateTime.Parse(dueDate);
            appointment.Text = text;
            AppointmentsToBook.Add(appointment);
        }

        public void GivenTheUserHasTheFollowingAppointmentsBooked(string userName, string start, string finish, string text)
        {
            Event appt = new Event();
            appt.StartTime = DateTime.Parse(start);
            appt.EndTime = DateTime.Parse(finish);
            appt.Text = text;
            appt.User = userName;
            _EventStore.Appointments.Add(appt);
        }

        public void GivenTheUserWorksTheFollowingHours(string userName, string day, string start, string finish)
        {
            User user = _UserStore.GetUser(userName);

            if(user == null)
            {
                user = new User();
                user.Name = userName;
            }


            WorkingHours hours = new WorkingHours();

            hours.Start = DateTime.Parse(start);
            hours.Finish = DateTime.Parse(finish);


            DayOfWeek d = DayOfWeek.Monday;
            switch (day)
            {
                case "Mon": d = DayOfWeek.Monday; break;
                case "Tue": d = DayOfWeek.Tuesday; break;
                case "Wed": d = DayOfWeek.Wednesday; break;
                case "Thu": d = DayOfWeek.Thursday; break;
                case "Fri": d = DayOfWeek.Friday; break;
                case "Sat": d = DayOfWeek.Saturday; break;
                case "Sun": d = DayOfWeek.Sunday; break;
            }


            user.WorkingDays[d] = hours;


            _UserStore.SaveUser(user);
        }

        public void WhenAutoBookingIsExecuted()
        {
            _AutoBookResults = Diary_.AutoBook(AppointmentsToBook, _UserStore.GetUsers());
            AppointmentsToBook.Clear();
        }


        public void ThenTheUsersHaveTheFollowingAppointmentsBooked(string userName, string dueDate, string startDateTime, string endDateTime, string text)
        {
            Event expected = new Event();
            expected.DueDate = DateTime.Parse(dueDate);
            expected.StartTime = DateTime.Parse(startDateTime);
            expected.EndTime = DateTime.Parse(endDateTime);
            expected.Text = text;

            Event actual = _AutoBookResults.Booked.Where(x => x.Text == expected.Text).FirstOrDefault();

            Assert.IsNotNull(actual, "Appointment not found");
            Assert.AreEqual(expected.DueDate, actual.DueDate, "duedate:" + expected.Id.ToString());
            Assert.AreEqual(expected.EndTime, actual.EndTime, "endtime:" + expected.Id.ToString());
            Assert.AreEqual(expected.StartTime, actual.StartTime, "starttime:" + expected.Id.ToString());
        }


        [TestInitialize]
        public void SetUp()
        {
            _EventStore = new EventStore();
            _UserStore = new UserStore();
            Diary_ = new EventManager(_EventStore);
            AppointmentsToBook = new List<EventToBook>();
            _AutoBookResults = null;
        }


        [TestMethod]
        public void AppointmentsAreBookedInLengthOrderLongestFirst()
        {
            GivenTheFollowingBookableAppointments("1:00:00", "2018-11-19", "1");
            GivenTheFollowingBookableAppointments("2:00:00", "2018-11-19", "2");
            GivenTheFollowingBookableAppointments("3:00:00", "2018-11-19", "3");

            GivenTheUserWorksTheFollowingHours("ME", "Mon", "08:00", "16:00");

            WhenAutoBookingIsExecuted();

            ThenTheUsersHaveTheFollowingAppointmentsBooked("ME", "2018-11-19", "2018-11-19 08:00", "2018-11-19 11:00", "3");
            ThenTheUsersHaveTheFollowingAppointmentsBooked("ME", "2018-11-19", "2018-11-19 11:00", "2018-11-19 13:00", "2");
            ThenTheUsersHaveTheFollowingAppointmentsBooked("ME", "2018-11-19", "2018-11-19 13:00", "2018-11-19 14:00", "1");
        }

        [TestMethod]
        public void AppointmentsAreBookedInLengthOrderByDueDate()
        {
            GivenTheFollowingBookableAppointments("1:00:00", "2018-11-19", "1");
            GivenTheFollowingBookableAppointments("2:00:00", "2018-11-19", "2");
            GivenTheFollowingBookableAppointments("3:00:00", "2018-11-20", "3");
            GivenTheFollowingBookableAppointments("1:20:00", "2018-11-20", "4");

            GivenTheUserWorksTheFollowingHours("ME", "Mon", "08:00", "16:00");
            GivenTheUserWorksTheFollowingHours("ME", "Tue", "08:00", "16:00");

            WhenAutoBookingIsExecuted();


            ThenTheUsersHaveTheFollowingAppointmentsBooked("ME", "2018-11-19", "2018-11-19 08:00", "2018-11-19 10:00", "2");
            ThenTheUsersHaveTheFollowingAppointmentsBooked("ME", "2018-11-19", "2018-11-19 10:00", "2018-11-19 11:00", "1");

            ThenTheUsersHaveTheFollowingAppointmentsBooked("ME", "2018-11-20", "2018-11-20 08:00", "2018-11-20 11:00", "3");
            ThenTheUsersHaveTheFollowingAppointmentsBooked("ME", "2018-11-20", "2018-11-20 11:00", "2018-11-20 12:20", "4");
        }


        [TestMethod]
        public void FindAppointmentSlot()
        {
            GivenTheFollowingBookableAppointments("1:00:00", "2018-11-19", "1");
            GivenTheFollowingBookableAppointments("2:00:00", "2018-11-19", "2");
            GivenTheFollowingBookableAppointments("3:00:00", "2018-11-19", "3");
            GivenTheFollowingBookableAppointments("2:00:00", "2018-11-20", "4");
            GivenTheFollowingBookableAppointments("2:00:00", "2018-11-20", "5");
            GivenTheFollowingBookableAppointments("5:00:00", "2018-11-20", "6");
            GivenTheFollowingBookableAppointments("2:00:00", "2018-11-20", "7");

            GivenTheUserWorksTheFollowingHours("ME", "Mon", "08:00", "16:00");
            GivenTheUserWorksTheFollowingHours("ME", "Tue", "08:00", "16:00");
            GivenTheUserWorksTheFollowingHours("ME", "Wed", "08:00", "16:00");
            GivenTheUserWorksTheFollowingHours("ME", "Thu", "08:00", "16:00");
            GivenTheUserWorksTheFollowingHours("ME", "Fri", "08:00", "16:00");
            GivenTheUserWorksTheFollowingHours("ME", "Sat", "08:00", "16:00");
            GivenTheUserWorksTheFollowingHours("ME", "Sun", "00:00", "00:00");

            WhenAutoBookingIsExecuted();

            ThenTheUsersHaveTheFollowingAppointmentsBooked("ME", "2018-11-19", "2018-11-19 08:00", "2018-11-19 11:00", "3");
            ThenTheUsersHaveTheFollowingAppointmentsBooked("ME", "2018-11-19", "2018-11-19 11:00", "2018-11-19 13:00", "2");
            ThenTheUsersHaveTheFollowingAppointmentsBooked("ME", "2018-11-19", "2018-11-19 13:00", "2018-11-19 14:00", "1");

            ThenTheUsersHaveTheFollowingAppointmentsBooked("ME", "2018-11-20", "2018-11-20 08:00", "2018-11-20 13:00", "6");

            ThenTheUsersHaveTheFollowingAppointmentsBooked("ME", "2018-11-20", "2018-11-20 13:00", "2018-11-20 15:00", "4");
            ThenTheUsersHaveTheFollowingAppointmentsBooked("ME", "2018-11-20", "2018-11-21 08:00", "2018-11-21 10:00", "5");
            ThenTheUsersHaveTheFollowingAppointmentsBooked("ME", "2018-11-20", "2018-11-21 10:00", "2018-11-21 12:00", "7");
        }



        [TestMethod]
        public void FindAppointmentSlotWhenOneAlreadyExists()
        {
            GivenTheUserWorksTheFollowingHours("ME", "Mon", "08:00", "16:00");
            GivenTheUserWorksTheFollowingHours("ME", "Tue", "08:00", "16:00");
            GivenTheUserWorksTheFollowingHours("ME", "Wed", "08:00", "16:00");
            GivenTheUserWorksTheFollowingHours("ME", "Thu", "08:00", "16:00");
            GivenTheUserWorksTheFollowingHours("ME", "Fri", "08:00", "16:00");
            GivenTheUserWorksTheFollowingHours("ME", "Sat", "08:00", "16:00");
            GivenTheUserWorksTheFollowingHours("ME", "Sun", "00:00", "00:00");


            GivenTheUserHasTheFollowingAppointmentsBooked("ME", "2018-11-19 10:00", "2018-11-19 11:00", "2");

            GivenTheFollowingBookableAppointments("2:00:00", "2018-11-19", "1");
            WhenAutoBookingIsExecuted();
            ThenTheUsersHaveTheFollowingAppointmentsBooked("ME", "2018-11-19", "2018-11-19 08:00", "2018-11-19 10:00", "1");
        }

        [TestMethod]
        public void FindAppointmentSlotOnNextWorkingDay()
        {
            GivenTheUserWorksTheFollowingHours("ME", "Mon", "12:00", "16:00");
            GivenTheUserWorksTheFollowingHours("ME", "Tue", "08:00", "16:00");
            GivenTheUserWorksTheFollowingHours("ME", "Wed", "08:00", "16:00");
            GivenTheUserWorksTheFollowingHours("ME", "Thu", "08:00", "16:00");
            GivenTheUserWorksTheFollowingHours("ME", "Fri", "08:00", "16:00");
            GivenTheUserWorksTheFollowingHours("ME", "Sat", "08:00", "16:00");
            GivenTheUserWorksTheFollowingHours("ME", "Sun", "00:00", "00:00");

            GivenTheUserHasTheFollowingAppointmentsBooked("ME", "2018-11-17 09:00", "2018-11-17 18:00", "1");
            GivenTheUserHasTheFollowingAppointmentsBooked("ME", "2018-11-19 09:00", "2018-11-19 12:30", "2");


            GivenTheFollowingBookableAppointments("2:00:00", "2018-11-17", "3");
            WhenAutoBookingIsExecuted();
            ThenTheUsersHaveTheFollowingAppointmentsBooked("ME", "2018-11-17", "2018-11-19 12:30", "2018-11-19 14:30", "3");

        }
    }
}

