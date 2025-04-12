using TimeTable.Models.Entity;

namespace TimeTable.Models.Repository
{
    public interface ILessonRepository
    {
        Task<List<Lesson>> GetAll();
        Task<Lesson> GetById(Guid id);
        Task<Guid> Add(Lesson lesson);
        Task<Guid> Delete(Guid id);
        Task<Guid> Update(Guid id, string Subject, Guid userId, string className, Guid taskId, DateTime date, DateTime startTime, DateTime endtime);
        Task<List<Lesson>> GetUserLessons(Guid userid);
    }
}
