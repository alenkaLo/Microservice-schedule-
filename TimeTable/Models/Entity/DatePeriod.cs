using System;

namespace TimeTable.Models.Entity
{
    public class DatePeriod
    {
        
        public DateOnly StartPeriod {  get; set; }
        public DateOnly EndPeriod { get; set; }
        public List<DayOfWeek> Filter {  get; set; }
        private const int week = 7;
        public DatePeriod(DateOnly startPeriod, DateOnly endPeriod, List<DayOfWeek> filter)
        {
            StartPeriod = startPeriod;
            EndPeriod = endPeriod;
            Filter = filter;
        }
        public bool CheckCorrectPeriod()
        {
            return StartPeriod <= EndPeriod;
        }
        private DateOnly Offset(DayOfWeek day)
        {
            var time = StartPeriod;
            var offset = (int)day - (int)time.DayOfWeek;
            time = (double)time.DayOfWeek <= (double)day
                ? time.AddDays(offset)
                : time.AddDays(week + offset);
            return time;
        }
        public void Start(LessonsToAdd lessonsToAdd)
        {
            while (StartPeriod <= EndPeriod)
            {
                StartPeriod = OffsetByAWeek(lessonsToAdd);
            }
        }
        private DateOnly OffsetByAWeek(LessonsToAdd lessonsToAdd)
        {
            foreach (var day in Filter)
            {
                var time = Offset(day);
                if (time > EndPeriod)
                    continue;
                lessonsToAdd.Add(time);
            }
            return StartPeriod.AddDays(week);
        }

    }
}
