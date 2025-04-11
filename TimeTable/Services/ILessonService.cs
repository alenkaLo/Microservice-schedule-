using TimeTable.Models.Entity;

namespace TimeTable.Services
{
    public interface ILessonService
    {
        Task<List<Lesson>> GetAllLessons();
        Task<Lesson> GetLessonById(Guid id);
        Task<Guid> Add(Lesson lesson);
        Task<Guid> Delete(Guid id);
        Task<Guid> Update(Guid id, String subject, Guid userId, string className, Guid taskId, DateTime date, DateTime startTime, DateTime endtime);
        Task<List<Lesson>> GetUserSchedule(Guid id);
        Task GiveMark(Guid subjectId, Guid userId, int Mark);

    }
}
