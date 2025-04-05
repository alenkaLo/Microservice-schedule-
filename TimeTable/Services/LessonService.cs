using TimeTable.Models.Entity;
using TimeTable.Models.Repository;

namespace TimeTable.Services
{
    // Слой с бизнес логикой
    public class LessonService : ILessonService
    {
        private readonly ILessonRepository _lessonRepository;


        public LessonService(ILessonRepository lessonRepository)
        {
            _lessonRepository = lessonRepository;
        }
  
        public async Task<List<Lesson>> GetAllLessons()
        {
            return await _lessonRepository.GetAll();
        }

        public async Task<Lesson> GetLessonById(Guid id)
        {
            return await _lessonRepository.GetById(id);
        }

        public async Task<Guid> Add(Lesson lesson)
        {
           return await _lessonRepository.Add(lesson);
        }
        public async Task<Guid> Delete(Guid id)
        {
            return await _lessonRepository.Delete(id);
        }
        public async Task<Guid> Update(Guid id, Guid subjectId, Guid userId, string className, DateTime startTime, DateTime endtime)
        {
            return await _lessonRepository.Update(id, subjectId, userId, className, startTime, endtime);
        }

        public async Task<List<Lesson>> GetUserSchedule(Guid id)
        {
            return await _lessonRepository.GetUserLessons(id);
        }
        public async Task GiveMark(Guid subjectId, Guid userId, int Mark)
        {
            await Task.Delay(0);
        }
    }
}
