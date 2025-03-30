using TimeTable.Models.Entity;

namespace TimeTable.Models.Repository
{
    public interface ILessonRepository
    {
        Task<List<Lesson>> GetAll();
        Task<Lesson> GetById(Guid id);
        Task<Guid> Add(Lesson lesson);
    }
}
