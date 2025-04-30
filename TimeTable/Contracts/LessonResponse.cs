using System;

namespace TimeTable.Contracts
{
    public record LessonResponse(
        Guid? Id,
        string Subject,
        Guid UserId,
        string ClassName,
        Guid TaskId,
        DateOnly Date,
        TimeOnly StartTime,
        TimeOnly EndTime,
        bool IsSuccess = true,
        string? ErrorMessage = null)
        : IdResponse(Id, IsSuccess, ErrorMessage)
        {
        public static new LessonResponse Failure(string errorMessage) => new(
            Id: Guid.Empty,
            Subject: string.Empty,
            UserId: Guid.Empty,
            ClassName: string.Empty,
            TaskId: Guid.Empty,
            Date: DateOnly.MinValue,
            StartTime: TimeOnly.MinValue,
            EndTime: TimeOnly.MinValue,
            IsSuccess: false,
            ErrorMessage: errorMessage);
        }

}
