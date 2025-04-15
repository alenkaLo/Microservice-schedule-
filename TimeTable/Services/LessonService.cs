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

        public async Task<Guid?> Add(Lesson lesson)
        {
            if (lesson.StartTime > lesson.EndTime) 
                return null;
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
                    lesson.StartTime = lesson.StartTime;
                    lesson.EndTime = lesson.EndTime;
                    lesson.Id=Guid.NewGuid();
                    await _lessonRepository.Add(lesson);
                }
                startTime = startTime.AddDays(week);
            }
            return lesson.Id;
        }
        public async Task<Guid?> Delete(Guid id)
        {
            return await _lessonRepository.Delete(id);
        }
        public async Task<Guid?> Update(Guid id, string? subject, Guid? userId, string? className, Guid? taskId, DateOnly? date, TimeOnly? startTime, TimeOnly? endtime)
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
