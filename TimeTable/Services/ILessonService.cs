using TimeTable.Models.Entity;

namespace TimeTable.Services
{
    public interface ILessonService
    {
        Task<List<Lesson>> GetAllLessons();
        Task<Lesson> GetLessonById(Guid id);

        Task<Guid> Add(Lesson lesson);
        Task<Guid[]> AddWithRepeats(Lesson lesson, List<DayOfWeek> days, DateOnly startPeriod, DateOnly endPeriod);
        Task<Guid> Delete(Guid id);
        Task<Guid> Update(Guid id, string? subject, Guid? userId, string? className, Guid? taskId, DateOnly? date, TimeOnly? startTime, TimeOnly? endtime);
        Task<List<Lesson>> GetAllForPeriod(Period period);
        Task<List<Lesson>> GetUserSchedule(Guid id, Period period);
        Task<List<Lesson>> GetClassSchedule(string className, Period period);
    }
}
