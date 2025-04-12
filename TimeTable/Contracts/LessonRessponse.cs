namespace TimeTable.Contracts
{
    public record LessonRessponse(
        Guid Id,
        string Subject,
        Guid UserId,
        string ClassName,
        Guid? TaskID,
        string Date,
        string StartTime,
        string EndTime);

}
