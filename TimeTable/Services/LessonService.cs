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

    }
}
