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
           return await _lessonRepository.Add(lesson);
        }
        public async Task<Guid> AddWithRepeat(Lesson lesson, List<DateTime> days, DateTime startPeriod, DateTime endPeriod)
        {
            var startTime = startPeriod.Date;
            if (startTime > endPeriod)
            {
                var time = startTime;
                startTime = endPeriod;
                endPeriod = time;
            }
            while(startTime < endPeriod)
            {
                foreach(var day in days)
                {
                    var time = startTime;
                    if ((double)time.DayOfWeek < (double)day.DayOfWeek)
                        time = time.AddDays((double)day.DayOfWeek - (double)time.DayOfWeek);
                    else
                        time = time.AddDays(week - ((double)time.DayOfWeek - (double)day.DayOfWeek));
                    if (time > endPeriod) 
                        break;
                    lesson.StartTime = time + lesson.StartTime.TimeOfDay;
                    lesson.EndTime = time + lesson.EndTime.TimeOfDay;
                    lesson.Id=Guid.NewGuid();
                    await _lessonRepository.Add(lesson);
                }
                startTime = startTime.AddDays(week);
            }
            return lesson.Id;
        }
        public async Task<Guid> Delete(Guid id)
        {
            return await _lessonRepository.Delete(id);
        }
        public async Task<Guid> Update(Guid id, Guid subjectId, Guid userId, DateTime startTime, DateTime endtime)
        {
            return await _lessonRepository.Update(id, subjectId, userId, startTime, endtime);
        }

        public async Task<List<Lesson>> GetUserSchedule(Guid id)
        {
            return await _lessonRepository.GetUserLessons(id);
        }
    }
}
