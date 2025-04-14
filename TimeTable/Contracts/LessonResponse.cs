using System;

namespace TimeTable.Contracts
{
    public record LessonRessponse(
        Guid? Id,
        string Subject,
        Guid UserId,
        string ClassName,
        Guid TaskID,
        DateOnly Date,
        TimeOnly StartTime,
        TimeOnly EndTime);
}
