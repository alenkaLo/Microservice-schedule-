using TimeTable.Models.Entity;
using TimeTable.Models.Repository;

namespace TimeTable.Services
{
    // Слой с бизнес логикой
    public class LessonService : ILessonService
    {
        private readonly ILessonRepository _lessonRepository;
        private const int week = 7;
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
            if (lesson.StartTime > lesson.EndTime) 
                return Guid.Empty;
           return await _lessonRepository.Add(lesson);
        }
        public async Task<Guid[]> AddWithRepeats(Lesson lesson, List<DayOfWeek> filter, DateOnly startPeriod, DateOnly endPeriod)
        {
            LessonsToAdd lessonsToAdd = new LessonsToAdd(new List<Lesson>(), lesson);
            DatePeriod datePeriod = new DatePeriod(startPeriod, endPeriod, filter);
            if (!datePeriod.CheckCorrectPeriod())
                return Array.Empty<Guid>();
            datePeriod.Start(lessonsToAdd);
           return await _lessonRepository.AddList(lessonsToAdd.Lessons);
        }
        public async Task<Guid> Delete(Guid id)
        {
            return await _lessonRepository.Delete(id);
        }
        public async Task<Guid> Update(Guid id, string? subject, Guid? userId, string? className, Guid? taskId, DateOnly? date, TimeOnly? startTime, TimeOnly? endtime)
        {
            return await _lessonRepository.Update(id, subject, userId, className, taskId, date, startTime, endtime);
        }
        public async Task<List<Lesson>> GetAllForPeriod(TimeOnly startTime, TimeOnly endTime, DateOnly startDate, DateOnly endDate)
        {
            return await _lessonRepository.GetAllForPeriod(startTime, endTime, startDate, endDate);
        }
        public async Task<List<Lesson>> GetUserSchedule(Guid id, TimeOnly startTime, TimeOnly endTime, DateOnly startDate, DateOnly endDate)
        {
            return await _lessonRepository.GetUserLessons(id, startTime, endTime, startDate, endDate);
        }

        public async Task<List<Lesson>> GetClassSchedule(string className, TimeOnly startTime, TimeOnly endTime, DateOnly startDate, DateOnly endDate)
        {
            return await _lessonRepository.GetClassLessons(className, startTime, endTime, startDate, endDate);
        }
    }
}
