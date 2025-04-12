using TimeTable.Models.Entity;

namespace TimeTable.Models.Repository
{
    public interface ILessonRepository
    {
        Task<List<Lesson>> GetAll(TimeOnly startTime, TimeOnly endTime, DateOnly startDate, DateOnly endDate);
        Task<Lesson> GetById(Guid id);
        Task<Guid> Add(Lesson lesson);
        Task<Guid> Delete(Guid id);
        Task<Guid> Update(Guid id, string? Subject, Guid? userId, string? className, Guid? taskId, DateOnly? date, TimeOnly? startTime, TimeOnly? endTime);
        Task<List<Lesson>> GetUserLessons(Guid userid, TimeOnly startTime, TimeOnly endTime, DateOnly startDate, DateOnly endDate);
        Task<List<Lesson>> GetClassLessons(string className);
    }
}
