using System;

namespace TimeTable.Models.Entity
{
    public class Lesson
    {
        public Lesson Clone()
        {
            Lesson output = new Lesson();
            output.Id = Id;
            output.Subject = Subject;
            output.UserId = UserId;
            output.ClassName = ClassName;
            output.TaskID = TaskID;
            output.Date = Date;
            output.StartTime = StartTime;
            output.EndTime = EndTime;
            return output;
        }
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public Guid UserId { get; set; }
        public string? ClassName { get; set; }
        public Guid TaskID { get; set; }
        public DateTime Date {  get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }
}
