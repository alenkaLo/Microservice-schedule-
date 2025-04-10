﻿namespace TimeTable.Contracts
{
    public record LessonRessponse(
        Guid Id,
        string Subject,
        Guid UserId,
        string ClassName,
        Guid TaskID,
        DateTime Date,
        DateTime StartTime,
        DateTime EndTime);
}
