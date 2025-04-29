using TimeTable.Models.Entity;

namespace TimeTable.Models.Repository
{
    public interface ILessonRepository
    {
        Task<List<Lesson>> GetAll();
        Task<Lesson> GetById(Guid id);
        Task<Guid> Add(Lesson lesson);
        Task<Guid[]> AddList(List<Lesson> lessons);
        Task<Guid> Delete(Guid id);
        Task<Guid> Update(Guid id, string? Subject, Guid? userId, string? className, Guid? taskId, DateOnly? date, TimeOnly? startTime, TimeOnly? endTime);
        Task<List<Lesson>> GetAllForPeriod(Period period);
        Task<List<Lesson>> GetUserLessons(Guid userid, Period period);
        Task<List<Lesson>> GetClassLessons(string className, Period period);
    }
}
