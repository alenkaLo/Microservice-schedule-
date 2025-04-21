using System;

namespace TimeTable.Models.Entity
{
    public class LessonWithOutID
    {
        public string Subject { get; set; }
        public Guid UserId { get; set; }
        public string ClassName { get; set; }
        public Guid? TaskID { get; set; }
        public DateOnly Date {  get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }
}
