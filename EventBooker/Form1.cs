using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Stores;
using DataObjects;

namespace EventBooker
{
    public partial class Form1 : Form
    {
        UserStore _UserStore;
        EventStore _EventStore;
        EventManager Diary_;
        List<EventToBook> AppointmentsToBook;
        AutoBookResults _AutoBookResults = null;
        List<Event> Provisonal;

        public Form1()
        {
            InitializeComponent();

            _EventStore = new EventStore();
            _UserStore = new UserStore();
            Diary_ = new EventManager(_EventStore);
            AppointmentsToBook = new List<EventToBook>();

            GivenTheUserWorksTheFollowingHours("ME", "Mon", "12:00", "16:00");
            GivenTheUserWorksTheFollowingHours("ME", "Tue", "08:00", "16:00");
            GivenTheUserWorksTheFollowingHours("ME", "Wed", "08:00", "16:00");
            GivenTheUserWorksTheFollowingHours("ME", "Thu", "08:00", "16:00");
            GivenTheUserWorksTheFollowingHours("ME", "Fri", "08:00", "16:00");
            GivenTheUserWorksTheFollowingHours("ME", "Sat", "08:00", "16:00");
            GivenTheUserWorksTheFollowingHours("ME", "Sun", "00:00", "00:00");

            GivenTheUserWorksTheFollowingHours("Bob", "Mon", "12:00", "16:00");
            GivenTheUserWorksTheFollowingHours("Bob", "Tue", "08:00", "16:00");
            GivenTheUserWorksTheFollowingHours("Bob", "Wed", "08:00", "16:00");
            GivenTheUserWorksTheFollowingHours("Bob", "Thu", "08:00", "16:00");
            GivenTheUserWorksTheFollowingHours("Bob", "Fri", "08:00", "16:00");
            GivenTheUserWorksTheFollowingHours("Bob", "Sat", "08:00", "16:00");
            GivenTheUserWorksTheFollowingHours("Bob", "Sun", "00:00", "00:00");

            GivenTheUserHasTheFollowingAppointmentsBooked("ME", "2018-11-17 09:00", "2018-11-17 18:00");
            GivenTheUserHasTheFollowingAppointmentsBooked("ME", "2018-11-19 09:00", "2018-11-19 12:30");
            GivenTheUserHasTheFollowingAppointmentsBooked("ME", "2018-11-20 15:00", "2018-11-20 16:00");
            GivenTheUserHasTheFollowingAppointmentsBooked("ME", "2018-11-20 08:00", "2018-11-20 09:00");

            GivenTheFollowingBookableAppointments("3:00:00", "2018-11-17");
            GivenTheFollowingBookableAppointments("8:00:00", "2018-11-17");
            GivenTheFollowingBookableAppointments("2:00:00", "2018-11-17");
            GivenTheFollowingBookableAppointments("2:00:00", "2018-11-17");
            GivenTheFollowingBookableAppointments("2:00:00", "2018-11-17");
            GivenTheFollowingBookableAppointments("2:00:00", "2018-11-17");
            GivenTheFollowingBookableAppointments("2:00:00", "2018-11-17");
            GivenTheFollowingBookableAppointments("2:00:00", "2018-11-17");
            GivenTheFollowingBookableAppointments("2:00:00", "2018-11-17");
            GivenTheFollowingBookableAppointments("2:00:00", "2018-11-17");
            GivenTheFollowingBookableAppointments("2:00:00", "2018-11-17");


            Provisonal = _EventStore.Appointments;

        }

        public void GivenTheFollowingBookableAppointments(string length, string dueDate)
        {
            EventToBook appointment = new EventToBook();
            appointment.Length = TimeSpan.Parse(length);
            appointment.DueDate = DateTime.Parse(dueDate);
            appointment.Type = 2;
            AppointmentsToBook.Add(appointment);
        }

        public void GivenTheUserHasTheFollowingAppointmentsBooked(string userName, string start, string finish)
        {
            Event appt = new Event();
            appt.User = userName;
            appt.StartTime = DateTime.Parse(start);
            appt.EndTime = DateTime.Parse(finish);
            appt.Id = Guid.NewGuid().ToString();
            _EventStore.Appointments.Add(appt);
        }

        public void GivenTheUserWorksTheFollowingHours(string userName, string day, string start, string finish)
        {
            User user = _UserStore.GetUser(userName);
            if (user == null)
            {
                user = new User()
                {
                    Name = userName
                };
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

        public void ExecuteAutoBooking()
        {
            _AutoBookResults = Diary_.AutoBook(AppointmentsToBook, _UserStore.GetUsers());
            AppointmentsToBook.Clear();
        }

        // bit hacky, but will do for a quick demo gui
        private void events_Paint(object sender, PaintEventArgs e)
        {
            const int SIZE_OF_HOUR = 40; //pixels

            // just a quick dirty draw call to render the events
            Color ME = Color.Red;
            Color BOB = Color.Blue;
            Color c2 = Color.Black;
            Graphics g = e.Graphics;

            int y = 20;

            var first = Provisonal.OrderBy(x => x.StartTime).First();
            int firstDay = first.StartTime.DayOfYear;

            DateTime dt = first.StartTime;
            Font font = new Font("Ariel", 11);
            Brush brush = Brushes.Gray;
            Pen pen = Pens.Black;
            Pen whitepen = Pens.Blue;
            Brush brush2 = Brushes.Green;

            Brush workingPeriod = Brushes.PaleGoldenrod;

            g.Clear(Color.White);
            Brush currentBrush = brush;


            var users = _UserStore.GetUsers();

            for (int i = 0; i < users.Count(); i++)
            {
                int yoffset = 300 * i - 20;
                for (int day = 0; day < 13; day++)
                {
                    var user = users[i]; 
                    var workingDay = user.WorkingDays[dt.DayOfWeek];

                    int workingS = workingDay.Start.Hour * 60 + workingDay.Start.Minute;
                    int workingF = workingDay.Finish.Hour * 60 + workingDay.Finish.Minute;
                    if (workingS != workingF)
                    {

                        int xs = 20 + (int)(workingS / 60 * SIZE_OF_HOUR);
                        int xf = 20 + (int)(workingF / 60 * SIZE_OF_HOUR);

                        g.FillRectangle(workingPeriod, xs, yoffset + y + (day * 20), xf - xs, 20);

                    }
                    dt = dt.AddDays(1);


                    string text = dt.ToLongDateString() + ":" + dt.DayOfWeek.ToString();

                    g.DrawString(text, font, brush, 20, yoffset + y + (day * 20));
                    g.DrawLine(pen, 20, yoffset + y + (day * 20), 800, yoffset + y + (day * 20));
                }
            }          

            
            // 60 mins = 10 px
            foreach (Event ev in Provisonal)
            {
                if (ev.Type == 1)
                    currentBrush = brush;
                else
                    currentBrush = brush2;
                int yoffset = 0;
                if (ev.User != "ME")
                    yoffset = 300;


                y = (ev.StartTime.DayOfYear - firstDay) * 20;
                double mins = ev.StartTime.Hour * 60 + ev.StartTime.Minute;
                int x = (int)(mins / 60 * SIZE_OF_HOUR);

                mins = (ev.EndTime.Hour * 60 + ev.EndTime.Minute);
                int width = (int)(mins / 60 * SIZE_OF_HOUR);
                width = width - x;

                g.FillRectangle(currentBrush, 20 + x, yoffset + y, width, 20);
                g.DrawRectangle(whitepen, 20 + x, yoffset + y, width, 20);
           
            }

            for (int hour = 8; hour < 24; hour += 4)
            {
                int x = (int)(hour * SIZE_OF_HOUR);
                g.DrawLine(pen, x, 0, x, 600);

                g.DrawString(hour.ToString()+":00", font, brush, x, 600); 
            }
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            ExecuteAutoBooking();
            events.Refresh();
        }
    }
}
