using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Threading.Tasks;
using TimeTable.Models.Repository;

namespace TimeTable.Models.Entity
{
    public class Lesson
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public Guid UserId { get; set; }
        public string ClassName { get; set; }
        public Guid? TaskID { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        private Lesson(Guid id, string subject, Guid userId, string className,
                  Guid? taskId, DateOnly date, TimeOnly startTime, TimeOnly endTime)
        {
            Id = id;
            Subject = subject;
            UserId = userId;
            ClassName = className;
            TaskID = taskId;
            Date = date;
            StartTime = startTime;
            EndTime = endTime;
        }

        public static async Task<Lesson> CreateAsync(
         ILessonRepository lessonRepository,
         Guid id,
         string? subject = null,
         Guid? userId = null,
         string? className = null,
         Guid? taskId = null,
         DateOnly? date = null,
         TimeOnly? startTime = null,
         TimeOnly? endTime = null)
        {
            var sourceLesson = await lessonRepository.GetById(id); 

            if (sourceLesson != null)
            return new Lesson(
                id: id,
                subject: subject ?? sourceLesson.Subject,
                userId: userId ?? sourceLesson.UserId,
                className: className ?? sourceLesson.ClassName,
                taskId: taskId ?? sourceLesson.TaskID,
                date: date ?? sourceLesson.Date,
                startTime: startTime ?? sourceLesson.StartTime,
                endTime: endTime ?? sourceLesson.EndTime
            );
            return new Lesson();
        }
        public Lesson()
        {
            Id = Guid.NewGuid();
            Subject = string.Empty;
            UserId = Guid.Empty;    // Внимание: Guid.Empty != null!
            ClassName = string.Empty;
            TaskID = null;          // Оставляем null, если это nullable (Guid?)
            Date = DateOnly.MinValue;
            StartTime = TimeOnly.MinValue;
            EndTime = TimeOnly.MinValue;
        }
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
      
    }
}
