using Api.Biz;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;

namespace Api.Services
{
    public class CalendarService
    {
        public static Ical.Net.Calendar GenerateCalendar(Schedule schedule)
        {
            var calendar = new Ical.Net.Calendar();
            // set calendar time zone to Turkey Istanbul
            calendar.AddTimeZone(new VTimeZone("Europe/Istanbul"));

            //calendar.Name = "Ders Programı"; // This line cause import problems

            var eventList = new List<Ical.Net.CalendarComponents.CalendarEvent>();

            // find monday of current week
            DateTime monday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
            for (var index = 0; index < schedule.ScheduleDays.Count; index++)
            {
                var scheduleDay = schedule.ScheduleDays[index];
                var day = monday.AddDays(index);
                // iteare each lesson and add to calendar
                foreach (var lesson in scheduleDay.Lessons)
                {
                    var strStartTime = lesson.Time.StartTime;
                    // find datetime of today with hour and minute
                    var start = new DateTime(day.Year, day.Month, day.Day, int.Parse(strStartTime.Split(':')[0]), int.Parse(strStartTime.Split(':')[1]), 0);
                    var strEndTime = lesson.Time.EndTime;
                    var end = new DateTime(day.Year, day.Month, day.Day, int.Parse(strEndTime.Split(':')[0]), int.Parse(strEndTime.Split(':')[1]), 0);

                    var calendarEvent = new Ical.Net.CalendarComponents.CalendarEvent
                    {
                        Start = new CalDateTime(day.Add(start.TimeOfDay)),
                        End = new CalDateTime(day.Add(end.TimeOfDay)),
                        Summary = lesson.Name,
                        Description = lesson.Teacher.Name,
                    };
                    eventList.Add(calendarEvent);
                }
            }


            calendar.Events.AddRange(eventList);

            return calendar;
        }
    }
}